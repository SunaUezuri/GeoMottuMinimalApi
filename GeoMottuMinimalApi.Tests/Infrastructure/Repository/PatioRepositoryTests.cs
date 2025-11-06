using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Infrastructure.Data.Repositories;

namespace GeoMottuMinimalApi.Tests.Infrastructure.Repositories
{
    public class PatioRepositoryTests : BaseRepositoryTests
    {
        private readonly PatioRepository _repository;
        private readonly FilialRepository _filialRepository;
        private readonly MotoRepository _motoRepository;

        public PatioRepositoryTests()
        {
            _repository = new PatioRepository(Context);
            _filialRepository = new FilialRepository(Context);
            _motoRepository = new MotoRepository(Context);
        }
        private FilialEntity CriarFilialValida(string nome = "Filial Pátio")
        {
            return new FilialEntity
            {
                Nome = nome,
                PaisFilial = PaisesFiliais.Brasil,
                Estado = "SP",
                Endereco = "Rua Teste Pátio, 123",
                CriadoEm = DateTime.Now
            };
        }

        private PatioEntity CriarPatioValido(FilialEntity filial, string referencia = "Pátio A")
        {
            return new PatioEntity
            {
                CapacidadeTotal = 50,
                LocalizacaoReferencia = referencia,
                Tamanho = 200,
                TipoDoPatio = TipoPatio.Disponivel,
                Filial = filial
            };
        }

        private MotoEntity CriarMotoValida(PatioEntity patio, string placa)
        {
            return new MotoEntity
            {
                Placa = placa,
                Chassi = $"CHASSI_{placa}",

                CodPlacaIot = $"IOT_{placa}",

                Modelo = ModeloMoto.MottuSport,
                Motor = 125,
                Patio = patio
            };
        }

        /* Tests */

        [Fact(DisplayName = "CreatePatioAsync deve salvar e retornar o pátio com Id")]
        [Trait("Repository", "Patio")]
        public async Task CreatePatioAsyncDeveAdicionarEretornarPatio()
        {
            var filial = CriarFilialValida();
            await _filialRepository.CreateFilialAsync(filial);

            var novoPatio = CriarPatioValido(filial);
            novoPatio.FilialId = filial.Id;
            novoPatio.Filial = null;

            var salvo = await _repository.CreatePatioAsync(novoPatio);

            Assert.NotNull(salvo);
            Assert.Equal("Pátio A", salvo.LocalizacaoReferencia);
            Assert.True(salvo.Id > 0);
            Assert.Equal(filial.Id, salvo.FilialId);

            var reloaded = await Context.Patio.FindAsync(salvo.Id);
            Assert.NotNull(reloaded);
            Assert.Equal(TipoPatio.Disponivel, reloaded.TipoDoPatio);
        }

        [Fact(DisplayName = "GetPatioByIdAsync deve retornar pátio com Filial e Motos (Include)")]
        [Trait("Repository", "Patio")]
        public async Task GetPatioByIdAsyncDeveRetornarPatioComRelacionamentos()
        {
            var filial = CriarFilialValida();
            var patio = CriarPatioValido(filial);
            var moto = CriarMotoValida(patio, "ABC-1111");

            Context.Filial.Add(filial);
            Context.Patio.Add(patio);
            Context.Moto.Add(moto);
            await Context.SaveChangesAsync();

            var result = await _repository.GetPatioByIdAsync(patio.Id);

            Assert.NotNull(result);
            Assert.Equal("Pátio A", result.LocalizacaoReferencia);

            Assert.NotNull(result.Filial);
            Assert.Equal(filial.Nome, result.Filial.Nome);

            Assert.NotNull(result.Motos);
            Assert.Single(result.Motos);
            Assert.Equal("ABC-1111", result.Motos.First().Placa);
        }

        [Fact(DisplayName = "GetAllPatiosAsync deve paginar e ordenar por Id")]
        [Trait("Repository", "Patio")]
        public async Task GetAllPatiosAsyncDevePaginarEOrdenarPorId()
        {
            var filial = CriarFilialValida();
            Context.Filial.Add(filial);

            var p1 = CriarPatioValido(filial, "Patio A");
            var p2 = CriarPatioValido(filial, "Patio B");
            var p3 = CriarPatioValido(filial, "Patio C");
            var p4 = CriarPatioValido(filial, "Patio D");
            Context.Patio.AddRange(p1, p2, p3, p4);
            await Context.SaveChangesAsync();

            var page = await _repository.GetAllPatiosAsync(offSet: 1, take: 2);

            Assert.NotNull(page);
            Assert.Equal(4, page.Total);
            Assert.Equal(1, page.Offset);
            Assert.Equal(2, page.Take);
            Assert.NotNull(page.Data);

            var dataList = page.Data.ToList();
            Assert.Equal(2, dataList.Count);

            Assert.Equal("Patio B", dataList[0].LocalizacaoReferencia);
            Assert.Equal("Patio C", dataList[1].LocalizacaoReferencia);
        }

        [Fact(DisplayName = "UpdatePatioAsync deve atualizar dados no banco")]
        [Trait("Repository", "Patio")]
        public async Task UpdatePatioAsyncDeveAtualizarDadosNoBanco()
        {
            var filial1 = CriarFilialValida("Filial 1");
            var filial2 = CriarFilialValida("Filial 2");
            Context.Filial.AddRange(filial1, filial2);

            var patioOriginal = CriarPatioValido(filial1);
            Context.Patio.Add(patioOriginal);
            await Context.SaveChangesAsync();

            var patioModificado = new PatioEntity
            {
                CapacidadeTotal = 999,
                TipoDoPatio = TipoPatio.Manutencao,
                Tamanho = 123.45,
                FilialId = filial2.Id
            };

            var result = await _repository.UpdatePatioAsync(patioOriginal.Id, patioModificado);

            Assert.NotNull(result);
            Assert.Equal(999, result.CapacidadeTotal);
            Assert.Equal(TipoPatio.Manutencao, result.TipoDoPatio);
            Assert.Equal(filial2.Id, result.FilialId);

            var reloaded = await Context.Patio.FindAsync(patioOriginal.Id);
            Assert.NotNull(reloaded);
            Assert.Equal(999, reloaded.CapacidadeTotal);
        }

        [Fact(DisplayName = "DeletePatioAsync deve remover pátio do banco")]
        [Trait("Repository", "Patio")]
        public async Task DeletePatioAsyncDeveRemoverPatioDoBanco()
        {
            var filial = CriarFilialValida();
            var patio = CriarPatioValido(filial);
            Context.Filial.Add(filial);
            Context.Patio.Add(patio);
            await Context.SaveChangesAsync();
            var id = patio.Id;

            var result = await _repository.DeletePatioAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Pátio A", result.LocalizacaoReferencia);

            var reloaded = await Context.Patio.FindAsync(id);
            Assert.Null(reloaded);
        }

        [Fact(DisplayName = "GetCurrentMotoCountAsync deve retornar contagem correta")]
        [Trait("Repository", "Patio")]
        public async Task GetCurrentMotoCountAsyncDeveRetornarContagemCorreta()
        {
            var filial = CriarFilialValida();
            Context.Filial.Add(filial);
            var patio1 = CriarPatioValido(filial, "Patio 1");
            var patio2 = CriarPatioValido(filial, "Patio 2");
            Context.Patio.AddRange(patio1, patio2);

            var m1 = CriarMotoValida(patio1, "AAA-0001");
            var m2 = CriarMotoValida(patio1, "AAA-0002");
            var m3 = CriarMotoValida(patio1, "AAA-0003");

            var m4 = CriarMotoValida(patio2, "BBB-0001");

            Context.Moto.AddRange(m1, m2, m3, m4);
            await Context.SaveChangesAsync();

            var contagemP1 = await _repository.GetCurrentMotoCountAsync(patio1.Id);
            var contagemP2 = await _repository.GetCurrentMotoCountAsync(patio2.Id);
            var contagemP99 = await _repository.GetCurrentMotoCountAsync(999);

            Assert.Equal(3, contagemP1);
            Assert.Equal(1, contagemP2);
            Assert.Equal(0, contagemP99);
        }
    }
}