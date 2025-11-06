using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.UseCases;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Interfaces;
using Moq;
using System.Net;
using GeoMottuMinimalApi.Domain.Errors;

namespace GeoMottuMinimalApi.Tests.Application.UseCases
{
    public class UsuarioUseCaseTests
    {
        private readonly Mock<IUsuarioRepository> _repositoryMock;
        private readonly UsuarioUseCase _useCase;
        public UsuarioUseCaseTests()
        {
            _repositoryMock = new Mock<IUsuarioRepository>();
            _useCase = new UsuarioUseCase(_repositoryMock.Object);
        }

        private UsuarioEntity CriarUsuarioValido(int id = 1)
        {
            return new UsuarioEntity
            {
                Id = id,
                Nome = "Usuario Teste",
                Email = "teste@mottu.com",
                Senha = "123",
                Role = "USER",
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

        [Fact(DisplayName = "GetUsuarioByIdAsync com Id existente deve retornar Success")]
        [Trait("UseCase", "Usuario")]
        public async Task GetUsuarioByIdAsyncIdExistenteDeveRetornarSuccess()
        {
            var usuario = CriarUsuarioValido(1);
            _repositoryMock.Setup(r => r.GetUsuarioByIdAsync(1))
                           .ReturnsAsync(usuario);

            var result = await _useCase.GetUsuarioByIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact(DisplayName = "GetUsuarioByIdAsync com Id inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Usuario")]
        public async Task GetUsuarioByIdAsyncIdInexistenteDeveRetornarFailureNotFound()
        {
            _repositoryMock.Setup(r => r.GetUsuarioByIdAsync(99))
                           .ReturnsAsync((UsuarioEntity?)null);

            var result = await _useCase.GetUsuarioByIdAsync(99);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Usuário não encontrado", result.Error);
        }


        [Fact(DisplayName = "GetUsuarioByEmailAsync com Email existente deve retornar Success")]
        [Trait("UseCase", "Usuario")]
        public async Task GetUsuarioByEmailAsyncEmailExistenteDeveRetornarSuccess()
        {
            var usuario = CriarUsuarioValido(1);
            _repositoryMock.Setup(r => r.GetUsuarioByEmailAsync("teste@mottu.com"))
                           .ReturnsAsync(usuario);

            var result = await _useCase.GetUsuarioByEmailAsync("teste@mottu.com");

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("teste@mottu.com", result.Value.Email);
        }

        [Fact(DisplayName = "GetUsuarioByEmailAsync com Email inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Usuario")]
        public async Task GetUsuarioByEmailAsyncEmailInexistenteDeveRetornarFailureNotFound()
        {
            _repositoryMock.Setup(r => r.GetUsuarioByEmailAsync("naoexiste@mottu.com"))
                           .ReturnsAsync((UsuarioEntity?)null); 

            var result = await _useCase.GetUsuarioByEmailAsync("naoexiste@mottu.com");

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Usuário não encontrado", result.Error);
        }

        [Fact(DisplayName = "CreateUsuarioAsync deve retornar Success com o usuário criado")]
        [Trait("UseCase", "Usuario")]
        public async Task CreateUsuarioAsyncDeveRetornarSuccessComUsuarioCriado()
        {
            var usuarioDto = CriarUsuarioDtoValido();
            var usuarioCriado = CriarUsuarioValido(5);

            _repositoryMock.Setup(r => r.CreateUsuarioAsync(It.IsAny<UsuarioEntity>()))
                           .ReturnsAsync(usuarioCriado);

            var result = await _useCase.CreateUsuarioAsync(usuarioDto);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(5, result.Value.Id);

            _repositoryMock.Verify(r => r.CreateUsuarioAsync(It.IsAny<UsuarioEntity>()), Times.Once);
        }

        [Fact(DisplayName = "DeleteUsuarioAsync com Id existente deve retornar Success")]
        [Trait("UseCase", "Usuario")]
        public async Task DeleteUsuarioAsyncIdExistenteDeveRetornarSuccess()
        {
            var usuario = CriarUsuarioValido(1);
            _repositoryMock.Setup(r => r.DeleteUsuarioAsync(1))
                           .ReturnsAsync(usuario);

            var result = await _useCase.DeleteUsuarioAsync(1);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact(DisplayName = "AutenticarUserAsync com credenciais corretas deve retornar Success")]
        [Trait("UseCase", "Usuario")]
        public async Task AutenticarUserAsyncCredenciaisCorretasDeveRetornarSuccess()
        {
            var authDto = CriarAuthDtoValido();
            var usuario = CriarUsuarioValido(1);
            _repositoryMock.Setup(r => r.AutenticarAsync(authDto.email, authDto.senha))
                           .ReturnsAsync(usuario);

            var result = await _useCase.AutenticarUserAsync(authDto);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact(DisplayName = "AutenticarUserAsync com credenciais incorretas deve retornar Failure")]
        [Trait("UseCase", "Usuario")]
        public async Task AutenticarUserAsyncCredenciaisIncorretasDeveRetornarFailure()
        {
            var authDto = new AuthUserDto("email@errado.com", "senha");
            _repositoryMock.Setup(r => r.AutenticarAsync(authDto.email, authDto.senha))
                           .ThrowsAsync(new NoContentException("Usuário não encontrado"));

            var result = await _useCase.AutenticarUserAsync(authDto);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("Ocorreu um erro ao buscar o cliente", result.Error);
        }
    }
}