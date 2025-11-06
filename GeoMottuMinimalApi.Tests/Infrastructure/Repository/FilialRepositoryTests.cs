using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Infrastructure.Data.Repositories;

namespace GeoMottuMinimalApi.Tests.Infrastructure.Repository
{
    public class FilialRepositoryTests : BaseRepositoryTests
    {
        private readonly FilialRepository _repository;

        public FilialRepositoryTests()
        {
            _repository = new FilialRepository(Context);
        }

        private FilialEntity CriarFilialValida(string nome)
        {
            return new FilialEntity
            {
                Nome = nome,
                PaisFilial = PaisesFiliais.Brasil,
                Estado = "SP",
                Endereco = "Rua Teste, 123",
                CriadoEm = DateTime.Now
            };
        }

        /* Tests */

        [Fact(DisplayName = "CreateFilialAsync deve salvar e retornar a filial com Id")]
        [Trait("Repository", "Filial")]
        public async Task CreateFilialAsyncDeveAdicionarEretornarFilial()
        {
            var novaFilial = CriarFilialValida("Filial Nova");

            var salva = await _repository.CreateFilialAsync(novaFilial);

            Assert.NotNull(salva);
            Assert.Equal("Filial Nova", salva.Nome);
            Assert.True(salva.Id > 0); 

            var reloaded = await Context.Filial.FindAsync(salva.Id);
            Assert.NotNull(reloaded);
            Assert.Equal("Filial Nova", reloaded.Nome);
            Assert.Equal(PaisesFiliais.Brasil, reloaded.PaisFilial);
        }

        [Fact(DisplayName = "GetFilialByIdAsync deve retornar filial com relacionamentos")]
        [Trait("Repository", "Filial")]
        public async Task GetFilialByIdAsyncDeveRetornarFilialComRelacionamentos()
        {
            var filial = CriarFilialValida("Filial Central");

            var usuario = new UsuarioEntity
            {
                Nome = "User Teste",
                Email = "a@a.com",
                Senha = "123",
                Role = "USER",
                Filial = filial
            };

            var patio = new PatioEntity
            {
                CapacidadeTotal = 50,
                Tamanho = 100,
                TipoDoPatio = TipoPatio.Disponivel,
                Filial = filial
            };

            Context.Filial.Add(filial);
            Context.Usuario.Add(usuario);
            Context.Patio.Add(patio);
            await Context.SaveChangesAsync();

            var result = await _repository.GetFilialByIdAsync(filial.Id);

            Assert.NotNull(result);
            Assert.Equal("Filial Central", result.Nome);

            Assert.NotNull(result.Usuarios);
            Assert.Single(result.Usuarios);
            Assert.Equal("User Teste", result.Usuarios.First().Nome);

            Assert.NotNull(result.Patios);
            Assert.Single(result.Patios);
            Assert.Equal(50, result.Patios.First().CapacidadeTotal);
        }

        [Fact(DisplayName = "GetFilialByIdAsync deve retornar null para Id inexistente")]
        [Trait("Repository", "Filial")]
        public async Task GetFilialByIdAsyncDeveRetornarNullParaIdInexistente()
        {
            var result = await _repository.GetFilialByIdAsync(999);

            Assert.Null(result);
        }

        [Fact(DisplayName = "GetAllFiliaisAsync deve paginar e ordenar por Id")]
        [Trait("Repository", "Filial")]
        public async Task GetAllFiliaisAsyncDevePaginarEOrdenarPorId()
        {
            var f1 = CriarFilialValida("Filial A");
            var f2 = CriarFilialValida("Filial B");
            var f3 = CriarFilialValida("Filial C");
            var f4 = CriarFilialValida("Filial D");
            Context.Filial.AddRange(f1, f2, f3, f4);
            await Context.SaveChangesAsync();

            var page = await _repository.GetAllFiliaisAsync(offSet: 1, take: 2);

            Assert.NotNull(page);
            Assert.Equal(4, page.Total);
            Assert.Equal(1, page.Offset);
            Assert.Equal(2, page.Take);     
            Assert.NotNull(page.Data);

            var dataList = page.Data.ToList();
            Assert.Equal(2, dataList.Count);

            Assert.Equal("Filial B", dataList[0].Nome);
            Assert.Equal("Filial C", dataList[1].Nome);
        }

        [Fact(DisplayName = "UpdateFilialAsync deve atualizar dados no banco")]
        [Trait("Repository", "Filial")]
        public async Task UpdateFilialAsyncDeveAtualizarDadosNoBanco()
        {
            var filialOriginal = CriarFilialValida("Nome Original");
            Context.Filial.Add(filialOriginal);
            await Context.SaveChangesAsync();

            var filialModificada = new FilialEntity
            {
                Nome = "Nome Modificado",
                Estado = "RJ",
                PaisFilial = PaisesFiliais.Mexico,
                Endereco = "Endereço Novo"
            };

            var result = await _repository.UpdateFilialAsync(filialOriginal.Id, filialModificada);

            Assert.NotNull(result);
            Assert.Equal("Nome Modificado", result.Nome);
            Assert.Equal("RJ", result.Estado);
            Assert.Equal(PaisesFiliais.Mexico, result.PaisFilial);

            var reloaded = await Context.Filial.FindAsync(filialOriginal.Id);
            Assert.NotNull(reloaded);
            Assert.Equal("Nome Modificado", reloaded.Nome);
        }

        [Fact(DisplayName = "DeleteFilialAsync deve remover filial do banco")]
        [Trait("Repository", "Filial")]
        public async Task DeleteFilialAsyncDeveRemoverFilialDoBanco()
        {
            var filial = CriarFilialValida("Para Deletar");
            Context.Filial.Add(filial);
            await Context.SaveChangesAsync();
            var id = filial.Id;

            var result = await _repository.DeleteFilialAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Para Deletar", result.Nome);

            var reloaded = await Context.Filial.FindAsync(id);
            Assert.Null(reloaded);
        }
    }
}
