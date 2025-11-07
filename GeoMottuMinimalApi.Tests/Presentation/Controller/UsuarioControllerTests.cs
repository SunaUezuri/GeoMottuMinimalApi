using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Tests.Presentation.Handlers;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace GeoMottuMinimalApi.Tests.Presentation.Controller
{
    public class UsuarioControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly Mock<IUsuarioUseCase> _useCaseMock;

        public UsuarioControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _useCaseMock = _factory.UsuarioUseCaseMock;
            _useCaseMock.Reset();
        }

        private UsuarioEntity CriarUsuarioValido(int id = 1, string role = "ADMIN")
        {
            return new UsuarioEntity
            {
                Id = id,
                Nome = "Usuario Teste",
                Email = "teste@mottu.com",
                Senha = "123",
                Role = role,
                FilialId = 1
            };
        }

        private UsuarioDto CriarUsuarioDtoValido()
        {
            return new UsuarioDto("Novo User", "novo@mottu.com", "senha123", 1);
        }

        private UsuarioUpdateDto CriarUsuarioUpdateDtoValido()
        {
            return new UsuarioUpdateDto("User Update", "update@mottu.com", "novaSenha", "ADMIN", 2);
        }

        private AuthUserDto CriarAuthDtoValido()
        {
            return new AuthUserDto("teste@mottu.com", "123");
        }
        
        /* Tests */

        [Fact(DisplayName = "GET /list ([AllowAnonymous]) deve retornar 200 OK")]
        [Trait("Controller", "Usuario")]
        public async Task GetDeveRetornar200QuandoHaUsuarios()
        {
            var listaUsuarios = new List<UsuarioEntity> { CriarUsuarioValido(1) };
            var pageResult = new PageResultModel<IEnumerable<UsuarioEntity>>
            {
                Data = listaUsuarios,
                Total = 1,
                Offset = 0,
                Take = 3
            };
            var successResult = OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>.Success(pageResult);

            _useCaseMock.Setup(u => u.GetAllUsuariosAsync(0, 3))
                        .ReturnsAsync(successResult);

            var response = await _client.GetAsync("/api/Usuario/list?offSet=0&take=3");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /* Tests */

        [Fact(DisplayName = "GET /list/{id} ([Authorize(ADMIN)]) deve retornar 200 OK")]
        [Trait("Controller", "Usuario")]
        public async Task GetByIdDeveRetornar200QuandoUsuarioExiste()
        {
            var usuario = CriarUsuarioValido(1);
            var successResult = OperationResult<UsuarioEntity?>.Success(usuario);
            _useCaseMock.Setup(u => u.GetUsuarioByIdAsync(1))
                        .ReturnsAsync(successResult);

            var response = await _client.GetAsync("/api/Usuario/list/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data.id);
        }

        [Fact(DisplayName = "GET /list/{id} ([Authorize(ADMIN)]) deve retornar 404 NotFound")]
        [Trait("Controller", "Usuario")]
        public async Task GetByIdDeveRetornar404QuandoUsuarioNaoExiste()
        {
            var failureResult = OperationResult<UsuarioEntity?>
                .Failure("Usuário não encontrado", (int)HttpStatusCode.NotFound);

            _useCaseMock.Setup(u => u.GetUsuarioByIdAsync(99))
                        .ReturnsAsync(failureResult);

            var response = await _client.GetAsync("/api/Usuario/list/99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "POST /create ([AllowAnonymous]) deve retornar OK")]
        [Trait("Controller", "Usuario")]
        public async Task PostDeveRetornarOKQuandoCriaComSucesso()
        {
            var usuarioDto = CriarUsuarioDtoValido();
            var usuarioCriado = CriarUsuarioValido(1);

            var successResult = OperationResult<UsuarioEntity?>.Success(usuarioCriado);

            _useCaseMock.Setup(u => u.CreateUsuarioAsync(It.IsAny<UsuarioDto>()))
                        .ReturnsAsync(successResult);

            var response = await _client.PostAsJsonAsync("/api/Usuario/create", usuarioDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data.id);
            Assert.Equal("Usuario Teste", (string)content.data.nome);
        }

        [Fact(DisplayName = "POST /auth ([AllowAnonymous]) deve retornar 200 OK e um Token JWT")]
        [Trait("Controller", "Usuario")]
        public async Task AuthDeveRetornar200ETokenQuandoCredenciaisSaoValidas()
        {
            var authDto = CriarAuthDtoValido();
            var usuario = CriarUsuarioValido(1, "ADMIN");

            var successResult = OperationResult<UsuarioEntity?>.Success(usuario);
            _useCaseMock.Setup(u => u.AutenticarUserAsync(authDto))
                        .ReturnsAsync(successResult);

            var response = await _client.PostAsJsonAsync("/api/Usuario/auth", authDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);

            string token = content.token;
            Assert.NotNull(token);
            Assert.StartsWith("ey", token);
            Assert.Equal(1, (int)content.user.data.id);
            Assert.Equal("teste@mottu.com", (string)content.user.data.email);
        }

        [Fact(DisplayName = "POST /auth ([AllowAnonymous]) deve retornar 500 quando UseCase falha")]
        [Trait("Controller", "Usuario")]
        public async Task AuthDeveRetornar500QuandoUseCaseFalha()
        {
            var authDto = CriarAuthDtoValido();
            var failureResult = OperationResult<UsuarioEntity?>
                .Failure("Erro genérico", (int)HttpStatusCode.InternalServerError);

            _useCaseMock.Setup(u => u.AutenticarUserAsync(authDto))
                        .ReturnsAsync(failureResult);

            var response = await _client.PostAsJsonAsync("/api/Usuario/auth", authDto);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
