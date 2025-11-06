using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Domain.Errors;
using GeoMottuMinimalApi.Infrastructure.Data.Repositories;

namespace GeoMottuMinimalApi.Tests.Infrastructure.Repository
{
    public class UsuarioRepositoryTests : BaseRepositoryTests
    {
        private readonly UsuarioRepository _repository;
        private readonly FilialRepository _filialRepository;

        public UsuarioRepositoryTests()
        {
            _repository = new UsuarioRepository(Context);
            _filialRepository = new FilialRepository(Context);
        }

        private FilialEntity CriarFilialValida(string nome = "Filial de Teste")
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

        private UsuarioEntity CriarUsuarioValido(string nome, string email, FilialEntity filial)
        {
            return new UsuarioEntity
            {
                Nome = nome,
                Email = email,
                Senha = "senha_padrao_123",
                Role = "USER",
                Filial = filial,
                CadastradoEm = DateTime.Now
            };
        }

        /* Tests */

        [Fact(DisplayName = "CreateUsuarioAsync deve salvar e retornar o usuário com Id")]
        [Trait("Repository", "Usuario")]
        public async Task CreateUsuarioAsyncDeveAdicionarEretornarUsuario()
        {
            var filial = CriarFilialValida();
            await _filialRepository.CreateFilialAsync(filial);

            var novoUsuario = new UsuarioEntity
            {
                Nome = "Usuario Novo",
                Email = "novo@mottu.com",
                Senha = "123",
                Role = "ADMIN",
                FilialId = filial.Id
            };

            var salvo = await _repository.CreateUsuarioAsync(novoUsuario);

            Assert.NotNull(salvo);
            Assert.Equal("Usuario Novo", salvo.Nome);
            Assert.True(salvo.Id > 0);
            Assert.Equal(filial.Id, salvo.FilialId);

            var reloaded = await Context.Usuario.FindAsync(salvo.Id);
            Assert.NotNull(reloaded);
            Assert.Equal("novo@mottu.com", reloaded.Email);
        }

        [Fact(DisplayName = "GetUsuarioByIdAsync deve retornar usuário com Filial (Include)")]
        [Trait("Repository", "Usuario")]
        public async Task GetUsuarioByIdAsyncDeveRetornarUsuarioComFilial()
        {
            var filial = CriarFilialValida("Filial do GetById");
            var usuario = CriarUsuarioValido("Usuario GetById", "get@mottu.com", filial);

            Context.Filial.Add(filial);
            Context.Usuario.Add(usuario);
            await Context.SaveChangesAsync();

            var result = await _repository.GetUsuarioByIdAsync(usuario.Id);

            Assert.NotNull(result);
            Assert.Equal("Usuario GetById", result.Nome);

            Assert.NotNull(result.Filial);
            Assert.Equal("Filial do GetById", result.Filial.Nome);
        }

        [Fact(DisplayName = "GetUsuarioByEmailAsync deve retornar o usuário correto")]
        [Trait("Repository", "Usuario")]
        public async Task GetUsuarioByEmailAsyncDeveRetornarUsuarioCorreto()
        {
            var filial = CriarFilialValida();
            var usuario = CriarUsuarioValido("Usuario Email", "email@mottu.com", filial);
            Context.Filial.Add(filial);
            Context.Usuario.Add(usuario);
            await Context.SaveChangesAsync();

            var result = await _repository.GetUsuarioByEmailAsync("email@mottu.com");
            var resultNulo = await _repository.GetUsuarioByEmailAsync("naoexiste@mottu.com");

            Assert.NotNull(result);
            Assert.Equal("Usuario Email", result.Nome);
            Assert.Null(resultNulo);
        }

        [Fact(DisplayName = "GetAllUsuariosAsync deve paginar e ordenar por Nome")]
        [Trait("Repository", "Usuario")]
        public async Task GetAllUsuariosAsync_DevePaginarEOrdenarPorNome()
        {
            var filial = CriarFilialValida();
            Context.Filial.Add(filial);

            var u1 = CriarUsuarioValido("Zelia", "z@mottu.com", filial);
            var u2 = CriarUsuarioValido("Ana", "a@mottu.com", filial);
            var u3 = CriarUsuarioValido("Carlos", "c@mottu.com", filial);
            var u4 = CriarUsuarioValido("Bruno", "b@mottu.com", filial);
            Context.Usuario.AddRange(u1, u2, u3, u4);
            await Context.SaveChangesAsync();

            var page = await _repository.GetAllUsuariosAsync(offSet: 1, take: 2);

            Assert.NotNull(page);
            Assert.Equal(4, page.Total);
            Assert.Equal(1, page.Offset);
            Assert.Equal(2, page.Take);
            Assert.NotNull(page.Data);

            var dataList = page.Data.ToList();
            Assert.Equal(2, dataList.Count);

            Assert.Equal("Bruno", dataList[0].Nome);
            Assert.Equal("Carlos", dataList[1].Nome);
        }

        [Fact(DisplayName = "UpdateUsuarioAsync deve atualizar dados no banco")]
        [Trait("Repository", "Usuario")]
        public async Task UpdateUsuarioAsyncDeveAtualizarDadosNoBanco()
        {
            var filial = CriarFilialValida();
            var filialNova = CriarFilialValida("Filial Nova");
            Context.Filial.AddRange(filial, filialNova);

            var usuarioOriginal = CriarUsuarioValido("Nome Original", "original@mottu.com", filial);
            Context.Usuario.Add(usuarioOriginal);
            await Context.SaveChangesAsync();

            var usuarioModificado = new UsuarioEntity
            {
                Nome = "Nome Modificado",
                Email = "novo@mottu.com",
                Senha = "nova_senha",
                Role = "ADMIN",
                FilialId = filialNova.Id
            };

            var result = await _repository.UpdateUsuarioAsync(usuarioOriginal.Id, usuarioModificado);

            Assert.NotNull(result);
            Assert.Equal("Nome Modificado", result.Nome);
            Assert.Equal("ADMIN", result.Role);
            Assert.Equal(filialNova.Id, result.FilialId);

            var reloaded = await Context.Usuario.FindAsync(usuarioOriginal.Id);
            Assert.NotNull(reloaded);
            Assert.Equal("novo@mottu.com", reloaded.Email);
        }

        [Fact(DisplayName = "DeleteUsuarioAsync deve remover usuário do banco")]
        [Trait("Repository", "Usuario")]
        public async Task DeleteUsuarioAsyncDeveRemoverUsuarioDoBanco()
        {
            var filial = CriarFilialValida();
            var usuario = CriarUsuarioValido("Para Deletar", "delete@mottu.com", filial);
            Context.Filial.Add(filial);
            Context.Usuario.Add(usuario);
            await Context.SaveChangesAsync();
            var id = usuario.Id;

            var result = await _repository.DeleteUsuarioAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Para Deletar", result.Nome);

            var reloaded = await Context.Usuario.FindAsync(id);
            Assert.Null(reloaded);
        }

        [Fact(DisplayName = "AutenticarAsync com credenciais corretas deve retornar usuário")]
        [Trait("Repository", "Usuario")]
        public async Task AutenticarAsyncCredenciaisCorretas_DeveRetornarUsuario()
        {
            var filial = CriarFilialValida();
            var usuario = CriarUsuarioValido("User Auth", "auth@mottu.com", filial);
            usuario.Senha = "senha_secreta_123";
            Context.Filial.Add(filial);
            Context.Usuario.Add(usuario);
            await Context.SaveChangesAsync();

            var result = await _repository.AutenticarAsync("auth@mottu.com", "senha_secreta_123");

            Assert.NotNull(result);
            Assert.Equal("User Auth", result.Nome);
        }

        [Fact(DisplayName = "AutenticarAsync com senha errada deve lançar NoContentException")]
        [Trait("Repository", "Usuario")]
        public async Task AutenticarAsync_SenhaErrada_DeveLancarNoContentException()
        {
            var filial = CriarFilialValida();
            var usuario = CriarUsuarioValido("User Auth", "auth@mottu.com", filial);
            usuario.Senha = "senha_secreta_123";
            Context.Filial.Add(filial);
            Context.Usuario.Add(usuario);
            await Context.SaveChangesAsync();

            await Assert.ThrowsAsync<NoContentException>(
                () => _repository.AutenticarAsync("auth@mottu.com", "senha_errada")
            );
        }

        [Fact(DisplayName = "AutenticarAsync com email errado deve lançar NoContentException")]
        [Trait("Repository", "Usuario")]
        public async Task AutenticarAsync_EmailErrado_DeveLancarNoContentException()
        {
            await Assert.ThrowsAsync<NoContentException>(
                () => _repository.AutenticarAsync("naoexiste@mottu.com", "senha_secreta_123")
            );
        }
    }
}
