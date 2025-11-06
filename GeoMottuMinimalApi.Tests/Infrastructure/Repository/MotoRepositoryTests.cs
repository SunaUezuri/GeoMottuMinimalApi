using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Infrastructure.Data.Repositories;

namespace GeoMottuMinimalApi.Tests.Infrastructure.Repository
{
    public class MotoRepositoryTests : BaseRepositoryTests
    {
        private readonly MotoRepository _repository;
        private readonly FilialRepository _filialRepository;
        private readonly PatioRepository _patioRepository;

        public MotoRepositoryTests()
        {
            _repository = new MotoRepository(Context);
            _filialRepository = new FilialRepository(Context);
            _patioRepository = new PatioRepository(Context);
        }
        private FilialEntity CriarFilialValida()
        {
            return new FilialEntity
            {
                Nome = "Filial Moto",
                PaisFilial = PaisesFiliais.Brasil,
                Estado = "SP",
                Endereco = "Rua Teste Moto, 123"
            };
        }
        private PatioEntity CriarPatioValido(FilialEntity filial)
        {
            return new PatioEntity
            {
                CapacidadeTotal = 50,
                TipoDoPatio = TipoPatio.Disponivel, // Valor não-default (1)
                Filial = filial
            };
        }
        private MotoEntity CriarMotoValida(PatioEntity patio, string placa, ModeloMoto modelo = ModeloMoto.MottuSport)
        {
            return new MotoEntity
            {
                Placa = placa,
                Chassi = $"CHASSI_{placa}",
                CodPlacaIot = $"IOT_{placa}",
                Modelo = modelo,
                Motor = 125,
                Patio = patio
            };
        }
        private async Task<PatioEntity> CriarEsalvarPatioPadraoAsync()
        {
            var filial = CriarFilialValida();
            var patio = CriarPatioValido(filial);
            Context.Filial.Add(filial);
            Context.Patio.Add(patio);
            await Context.SaveChangesAsync();
            return patio;
        }

        /* Tests */

        [Fact(DisplayName = "CreateMotoAsync deve salvar e retornar a moto com Id")]
        [Trait("Repository", "Moto")]
        public async Task CreateMotoAsyncDeveAdicionarEretornarMoto()
        {
            var patio = await CriarEsalvarPatioPadraoAsync();
            var novaMoto = new MotoEntity
            {
                Placa = "NEW-001",
                Chassi = "CHASSI_NEW-001",
                CodPlacaIot = "IOT_NEW-001",
                Modelo = ModeloMoto.MottuE,
                Motor = 3000,
                PatioId = patio.Id
            };

            var salva = await _repository.CreateMotoAsync(novaMoto);

            Assert.NotNull(salva);
            Assert.Equal("NEW-001", salva.Placa);
            Assert.True(salva.Id > 0);
            Assert.Equal(patio.Id, salva.PatioId);

            var reloaded = await Context.Moto.FindAsync(salva.Id);
            Assert.NotNull(reloaded);
            Assert.Equal(ModeloMoto.MottuE, reloaded.Modelo);
        }

        [Fact(DisplayName = "GetMotoByIdAsync deve retornar moto com Pátio (Include)")]
        [Trait("Repository", "Moto")]
        public async Task GetMotoByIdAsyncDeveRetornarMotoComPatio()
        {
            var patio = await CriarEsalvarPatioPadraoAsync();
            var moto = CriarMotoValida(patio, "GET-001");
            Context.Moto.Add(moto);
            await Context.SaveChangesAsync();

            var result = await _repository.GetMotoByIdAsync(moto.Id);

            Assert.NotNull(result);
            Assert.Equal("GET-001", result.Placa);

            Assert.NotNull(result.Patio);
            Assert.Equal(patio.Id, result.Patio.Id);
        }

        [Fact(DisplayName = "GetAllMotosAsync deve paginar e ordenar por Id")]
        [Trait("Repository", "Moto")]
        public async Task GetAllMotosAsyncDevePaginarEOrdenarPorId()
        {
            var patio = await CriarEsalvarPatioPadraoAsync();
            var m1 = CriarMotoValida(patio, "M-001");
            var m2 = CriarMotoValida(patio, "M-002");
            var m3 = CriarMotoValida(patio, "M-003");
            var m4 = CriarMotoValida(patio, "M-004");
            Context.Moto.AddRange(m1, m2, m3, m4);
            await Context.SaveChangesAsync();

            var page = await _repository.GetAllMotosAsync(offSet: 1, take: 2);

            Assert.NotNull(page);
            Assert.Equal(4, page.Total);
            Assert.Equal(1, page.Offset);
            Assert.Equal(2, page.Take);
            Assert.NotNull(page.Data);

            var dataList = page.Data.ToList();
            Assert.Equal(2, dataList.Count);

            Assert.Equal("M-002", dataList[0].Placa);
            Assert.Equal("M-003", dataList[1].Placa);
        }

        [Fact(DisplayName = "UpdateMotoAsync deve atualizar dados no banco")]
        [Trait("Repository", "Moto")]
        public async Task UpdateMotoAsyncDeveAtualizarDadosNoBanco()
        {
            var patio1 = await CriarEsalvarPatioPadraoAsync();
            var patio2 = CriarPatioValido(patio1.Filial);
            Context.Patio.Add(patio2);
            await Context.SaveChangesAsync();

            var motoOriginal = CriarMotoValida(patio1, "OLD-001");
            Context.Moto.Add(motoOriginal);
            await Context.SaveChangesAsync();

            var motoModificada = new MotoEntity
            {
                Placa = "UPDATED-001",
                Modelo = ModeloMoto.MottuE,
                Motor = 5000,
                Chassi = "CHASSI_UPDATED-001",
                PatioId = patio2.Id
            };

            var result = await _repository.UpdateMotoAsync(motoOriginal.Id, motoModificada);

            Assert.NotNull(result);
            Assert.Equal("UPDATED-001", result.Placa);
            Assert.Equal(ModeloMoto.MottuE, result.Modelo);
            Assert.Equal(patio2.Id, result.PatioId);

            var reloaded = await Context.Moto.FindAsync(motoOriginal.Id);
            Assert.NotNull(reloaded);
            Assert.Equal(5000, reloaded.Motor);
        }

        [Fact(DisplayName = "DeleteMotoAsync deve remover moto do banco")]
        [Trait("Repository", "Moto")]
        public async Task DeleteMotoAsyncDeveRemoverMotoDoBanco()
        {
            var patio = await CriarEsalvarPatioPadraoAsync();
            var moto = CriarMotoValida(patio, "DEL-001");
            Context.Moto.Add(moto);
            await Context.SaveChangesAsync();
            var id = moto.Id;

            var result = await _repository.DeleteMotoAsync(id);

            Assert.NotNull(result);
            Assert.Equal("DEL-001", result.Placa);

            var reloaded = await Context.Moto.FindAsync(id);
            Assert.Null(reloaded);
        }

        [Theory(DisplayName = "GetByPlacaAsync deve encontrar placa ignorando case")]
        [Trait("Repository", "Moto")]
        [InlineData("PLACA-001")]
        [InlineData("placa-001")]
        [InlineData("pLaCa-001")] 
        public async Task GetByPlacaAsyncDeveEncontrarIgnorandoCase(string placaBusca)
        {
            var patio = await CriarEsalvarPatioPadraoAsync();
            var moto = CriarMotoValida(patio, "PLACA-001");
            Context.Moto.Add(moto);
            await Context.SaveChangesAsync();

            var result = await _repository.GetByPlacaAsync(placaBusca);

            Assert.NotNull(result);
            Assert.Equal("PLACA-001", result.Placa);
        }

        [Theory(DisplayName = "GetByChassiAsync deve encontrar chassi ignorando case")]
        [Trait("Repository", "Moto")]
        [InlineData("CHASSI_ABC-123")]
        [InlineData("chassi_abc-123")]
        public async Task GetByChassiAsyncDeveEncontrarIgnorandoCase(string chassiBusca)
        {
            var patio = await CriarEsalvarPatioPadraoAsync();
            var moto = CriarMotoValida(patio, "ABC-123");
            Context.Moto.Add(moto);
            await Context.SaveChangesAsync();

            var result = await _repository.GetByChassiAsync(chassiBusca);

            Assert.NotNull(result);
            Assert.Equal("CHASSI_ABC-123", result.Chassi);
        }

        [Fact(DisplayName = "GetByModeloAsync deve filtrar e paginar corretamente")]
        [Trait("Repository", "Moto")]
        public async Task GetByModeloAsyncDeveFiltrarEPaginar()
        {
            var patio = await CriarEsalvarPatioPadraoAsync();

            Context.Moto.Add(CriarMotoValida(patio, "SPORT-1", ModeloMoto.MottuSport));
            Context.Moto.Add(CriarMotoValida(patio, "SPORT-2", ModeloMoto.MottuSport));
            Context.Moto.Add(CriarMotoValida(patio, "SPORT-3", ModeloMoto.MottuSport));

            Context.Moto.Add(CriarMotoValida(patio, "E-1", ModeloMoto.MottuE));
            Context.Moto.Add(CriarMotoValida(patio, "E-2", ModeloMoto.MottuE));

            await Context.SaveChangesAsync();

            var page = await _repository.GetByModeloAsync(ModeloMoto.MottuSport, offSet: 1, take: 2);

            Assert.NotNull(page);
            Assert.Equal(3, page.Total);
            Assert.Equal(1, page.Offset);
            Assert.Equal(2, page.Take);

            var dataList = page.Data.ToList();
            Assert.Equal(2, dataList.Count);
            Assert.Equal("SPORT-2", dataList[0].Placa);
            Assert.Equal("SPORT-3", dataList[1].Placa);
        }
    }
}