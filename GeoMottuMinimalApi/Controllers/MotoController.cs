using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Doc.Samples.Moto;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly IMotoUseCase _motoUseCase;

        public MotoController(IMotoUseCase motoUseCase)
        {
            _motoUseCase = motoUseCase;
        }

        [HttpGet("list")]
        [SwaggerOperation(
            Summary = "Lista todas as motos de forma paginada",
            Description = "Retorna uma lista paginada de motos com links HATEOAS."
        )]
        [SwaggerResponse(statusCode: 200, description: "Lista de motos retornada com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma moto encontrada")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoResponseListSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> Get(int offSet = 0, int take = 3)
        {
            var result = await _motoUseCase.GetAllMotosAsync(offSet, take);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            var hateoas = new
            {
                data = result.Value.Data.Select(moto => new
                {
                    moto.Id,
                    moto.Placa,
                    moto.Chassi,
                    moto.Modelo,
                    moto.CodPlacaIot,
                    moto.Motor,
                    moto.Proprietario,
                    moto.PosicaoX,
                    moto.PosicaoY,
                    links = new object[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetById), "Moto", new { id = moto.Id }, Request.Scheme) },
                        new { rel = "update", href = Url.Action(nameof(Put), "Moto", new { id = moto.Id }, Request.Scheme) },
                        new { rel = "delete", href = Url.Action(nameof(Delete), "Moto", new { id = moto.Id }, Request.Scheme) }
                    }
                }),
                links = new object[]
                {
                    new { rel = "self", href = Url.Action(nameof(Get), "Moto", null, Request.Scheme) },
                    new { rel = "create", href = Url.Action(nameof(Post), "Moto", null, Request.Scheme) }
                },
                pagination = new
                {
                    result.Value.Offset,
                    result.Value.Take,
                    result.Value.Total
                }
            };

            return StatusCode(result.StatusCode, hateoas);
        }

        [HttpGet("list/{id}")]
        [SwaggerOperation(
            Summary = "Obtém uma moto por ID",
            Description = "Retorna a moto correspondente ao ID informado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Moto encontrada", type: typeof(MotoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Moto não encontrada")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoResponseSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _motoUseCase.GetMotoByIdAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpGet("placa/{placa}")]
        [SwaggerOperation(
            Summary = "Obtém uma moto pela Placa",
            Description = "Retorna a moto correspondente à placa informada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Moto encontrada", type: typeof(MotoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Moto não encontrada")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoResponseSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> GetByPlaca(string placa)
        {
            var result = await _motoUseCase.GetByPlacaAsync(placa);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpGet("chassi/{chassi}")]
        [SwaggerOperation(
            Summary = "Obtém uma moto pelo Chassi",
            Description = "Retorna a moto correspondente ao chassi informado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Moto encontrada", type: typeof(MotoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Moto não encontrada")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoResponseSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> GetByChassi(string chassi)
        {
            var result = await _motoUseCase.GetByChassiAsync(chassi);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpGet("modelo/{modelo}")]
        [SwaggerOperation(
            Summary = "Lista motos por modelo",
            Description = "Retorna uma lista paginada de motos de um modelo específico, com links HATEOAS."
        )]
        [SwaggerResponse(statusCode: 200, description: "Lista de motos retornada com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma moto encontrada para o modelo")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoResponseListSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> GetByModelo(ModeloMoto modelo, int offSet = 0, int take = 3)
        {
            var result = await _motoUseCase.GetByModeloAsync(modelo, offSet, take);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            var hateoas = new
            {
                data = result.Value.Data.Select(moto => new
                {
                    moto.Id,
                    moto.Placa,
                    moto.Chassi,
                    moto.Modelo,
                    moto.Proprietario,
                    links = new object[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetById), "Moto", new { id = moto.Id }, Request.Scheme) },
                        new { rel = "update", href = Url.Action(nameof(Put), "Moto", new { id = moto.Id }, Request.Scheme) },
                        new { rel = "delete", href = Url.Action(nameof(Delete), "Moto", new { id = moto.Id }, Request.Scheme) }
                    }
                }),
                links = new object[]
                {
                     new { rel = "self", href = Url.Action(nameof(GetByModelo), "Moto", new { modelo }, Request.Scheme) }
                },
                pagination = new
                {
                    result.Value.Offset,
                    result.Value.Take,
                    result.Value.Total
                }
            };

            return StatusCode(result.StatusCode, hateoas);
        }

        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Cria uma nova moto",
            Description = "Cadastra uma nova moto no sistema."
        )]
        [SwaggerRequestExample(typeof(MotoDto), typeof(MotoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Moto criada com sucesso", type: typeof(MotoEntity))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos ou pátio lotado")]
        [SwaggerResponse(statusCode: 404, description: "Pátio não encontrado")]
        [SwaggerResponseExample(statusCode: 201, typeof(MotoResponseSample))]
        public async Task<IActionResult> Post(MotoDto motoDto)
        {
            var result = await _motoUseCase.CreateMotoAsync(motoDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update/{id}")]
        [SwaggerOperation(
            Summary = "Atualiza uma moto existente",
            Description = "Altera os dados de uma moto pelo seu ID."
        )]
        [SwaggerRequestExample(typeof(MotoDto), typeof(MotoRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Moto atualizada com sucesso", type: typeof(MotoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Moto ou Pátio não encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoResponseSample))]
        public async Task<IActionResult> Put(int id, MotoDto motoDto)
        {
            var result = await _motoUseCase.UpdateMotoAsync(id, motoDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpDelete("delete/{id}")]
        [SwaggerOperation(
            Summary = "Exclui uma moto",
            Description = "Remove uma moto do sistema pelo seu ID."
        )]
        [SwaggerResponse(statusCode: 200, description: "Moto excluída com sucesso", type: typeof(MotoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Moto não encontrada")]
        [SwaggerResponseExample(statusCode: 200, typeof(MotoResponseSample))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _motoUseCase.DeleteMotoAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }
    }
}