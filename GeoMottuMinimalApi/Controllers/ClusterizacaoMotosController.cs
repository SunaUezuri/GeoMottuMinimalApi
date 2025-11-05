using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.ML;
using Microsoft.ML.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoMottuMinimalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClusterizacaoMotosController : ControllerBase
    {
        private readonly MLContext _mlContext;
        private readonly string _caminhoModelo;
        private readonly IMotoUseCase _motoUseCase;

        public ClusterizacaoMotosController(IMotoUseCase motoUseCase)
        {
            _mlContext = new MLContext(seed: 0);
            _caminhoModelo = Path.Combine(Environment.CurrentDirectory, "Treinamento", "ModeloClusterMotos.zip");
            _motoUseCase = motoUseCase;
        }

        public class MotoLocalizacaoInput
        {
            [LoadColumn(0)] public float PosicaoX { get; set; }
            [LoadColumn(1)] public float PosicaoY { get; set; }
        }

        public class MotoClusterPrediction
        {
            [ColumnName("PredictedLabel")] public uint ClusterId { get; set; }
            [ColumnName("Score")] public float[] Distancias { get; set; }
        }

        public class ClusterPredictionResponse
        {
            public MotoLocalizacaoInput LocalizacaoEntrada { get; set; }
            public uint ClusterPrevisto { get; set; }
            public float[] Distancias { get; set; }
        }


        [HttpGet("Treinar")]
        [SwaggerOperation(
            Summary = "Treina o modelo de clusterização de motos",
            Description = "Inicia o processo de treinamento do modelo de ML (KMeans) usando todos os dados de geolocalização das motos. Esta operação pode ser demorada e deve ser executada esporadicamente."
        )]
        [SwaggerResponse(statusCode: 200, description: "Modelo treinado e salvo com sucesso")]
        [SwaggerResponse(statusCode: 400, description: "Falha ao treinar o modelo (ex: dados insuficientes ou erro no processo)")]
        [SwaggerResponse(statusCode: 401, description: "Autenticação necessária")]
        [SwaggerResponse(statusCode: 403, description: "Ação não permitida (requer role ADMIN)")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Treinar()
        {
            try
            {
                var diretorio = Path.GetDirectoryName(_caminhoModelo);
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                await TreinarModelo();
                return Ok(new { data = "Modelo de clusterização de motos treinado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Falha ao treinar modelo: " + ex.Message });
            }
        }

        [HttpGet("PreverCluster")]
        [SwaggerOperation(
            Summary = "Prevê o cluster de uma geolocalização",
            Description = "Recebe uma coordenada (X, Y) e retorna o ID do cluster (grupo) ao qual ela pertence, com base no modelo treinado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Previsão realizada com sucesso", type: typeof(ClusterPredictionResponse))]
        [SwaggerResponse(statusCode: 400, description: "O modelo ainda não foi treinado")]
        [SwaggerResponse(statusCode: 401, description: "Ação não permitida")]
        [EnableRateLimiting("ratelimit")]
        [Authorize(Roles = "USER, ADMIN")]
        public IActionResult PreverCluster([FromQuery] double posX, [FromQuery] double posY)
        {
            if (!System.IO.File.Exists(_caminhoModelo))
            {
                return BadRequest(new { erro = "O modelo ainda não foi treinado! Acesse /api/ClusterizacaoMotos/Treinar primeiro." });
            }

            ITransformer modelo = _mlContext.Model.Load(_caminhoModelo, out var schema);
            var engine = _mlContext.Model.CreatePredictionEngine<MotoLocalizacaoInput, MotoClusterPrediction>(modelo);

            var entrada = new MotoLocalizacaoInput
            {
                PosicaoX = (float)posX,
                PosicaoY = (float)posY
            };

            var predicao = engine.Predict(entrada);

            var response = new ClusterPredictionResponse
            {
                LocalizacaoEntrada = entrada,
                ClusterPrevisto = predicao.ClusterId,
                Distancias = predicao.Distancias
            };

            return Ok(new { data = response });
        }

        private async Task TreinarModelo()
        {
            var todasAsMotos = new List<MotoEntity>();
            const int TAMANHO_PAGINA = 1000;
            int offSet = 0;
            bool continuarBuscando = true;

            Console.WriteLine("Iniciando busca de dados para treinamento...");

            while (continuarBuscando)
            {
                var operacaoResultado = await _motoUseCase.GetAllMotosAsync(offSet, TAMANHO_PAGINA);

                if (operacaoResultado.IsSuccess && operacaoResultado.Value.Data != null && operacaoResultado.Value.Data.Any())
                {
                    todasAsMotos.AddRange(operacaoResultado.Value.Data);
                    offSet += TAMANHO_PAGINA;
                    Console.WriteLine($"... {todasAsMotos.Count} motos carregadas...");
                }
                else
                {
                    continuarBuscando = false;
                    if (!operacaoResultado.IsSuccess && operacaoResultado.StatusCode != 204)
                    {
                        throw new Exception($"Falha ao buscar dados do UseCase: {operacaoResultado.Error ?? "Erro desconhecido"}");
                    }
                }
            }

            Console.WriteLine($"Busca finalizada. Total de {todasAsMotos.Count} motos carregadas.");

            if (todasAsMotos.Count < 4)
            {
                throw new Exception($"Não há dados suficientes para treinar (mínimo de 4 registros necessários). Encontrados: {todasAsMotos.Count}");
            }

            var dadosDeTreino = todasAsMotos.Select(moto => new MotoLocalizacaoInput
            {
                PosicaoX = (float)moto.PosicaoX,
                PosicaoY = (float)moto.PosicaoY
            });

            IDataView dadosView = _mlContext.Data.LoadFromEnumerable(dadosDeTreino);

            var pipeline = _mlContext.Transforms
                .Concatenate("Features", nameof(MotoLocalizacaoInput.PosicaoX), nameof(MotoLocalizacaoInput.PosicaoY))
                .Append(_mlContext.Clustering.Trainers.KMeans(numberOfClusters: 4));

            Console.WriteLine("Iniciando treinamento do modelo...");
            var modelo = pipeline.Fit(dadosView);
            Console.WriteLine("Treinamento concluído.");

            _mlContext.Model.Save(modelo, dadosView.Schema, _caminhoModelo);
            Console.WriteLine($"Modelo salvo em: {_caminhoModelo}");
        }
    }
}
