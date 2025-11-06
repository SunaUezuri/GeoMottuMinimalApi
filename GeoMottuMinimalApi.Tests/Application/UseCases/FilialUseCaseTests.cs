using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.UseCases;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Domain.Interfaces;
using Moq;
using System.Net;

namespace GeoMottuMinimalApi.Tests.Application.UseCases
{
    public class FilialUseCaseTests
    {
        private readonly Mock<IFilialRepository> _repositoryMock;
        private readonly FilialUseCase _useCase;
        public FilialUseCaseTests()
        {
            _repositoryMock = new Mock<IFilialRepository>();
            _useCase = new FilialUseCase(_repositoryMock.Object);
        }
        private FilialEntity CriarFilialValida(int id = 1)
        {
            return new FilialEntity
            {
                Id = id,
                Nome = "Filial Teste",
                PaisFilial = PaisesFiliais.Brasil,
                Estado = "SP",
                Endereco = "Rua Teste, 123"
            };
        }

        private FilialDto CriarFilialDtoValida()
        {
            return new FilialDto("Filial DTO", PaisesFiliais.Mexico, "MX", "Calle Teste");
        }

        /* Tests */

        [Fact(DisplayName = "GetFilialByIdAsync com Id existente deve retornar Success")]
        [Trait("UseCase", "Filial")]
        public async Task GetFilialByIdAsyncIdExistenteDeveRetornarSuccess()
        {
            var filial = CriarFilialValida(1);
            _repositoryMock.Setup(r => r.GetFilialByIdAsync(1))
                           .ReturnsAsync(filial);

            var result = await _useCase.GetFilialByIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("Filial Teste", result.Value.Nome);
        }

        [Fact(DisplayName = "GetFilialByIdAsync com Id inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Filial")]
        public async Task GetFilialByIdAsyncIdInexistente_DeveRetornarFailureNotFound()
        {
            _repositoryMock.Setup(r => r.GetFilialByIdAsync(99))
                           .ReturnsAsync((FilialEntity?)null);

            var result = await _useCase.GetFilialByIdAsync(99);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Filial não encontrada", result.Error);
        }

        [Fact(DisplayName = "GetFilialByIdAsync com exceção do repositório deve retornar Failure")]
        [Trait("UseCase", "Filial")]
        public async Task GetFilialByIdAsyncRepositorioLancaExcecao_DeveRetornarFailure()
        {
            _repositoryMock.Setup(r => r.GetFilialByIdAsync(1))
                           .ThrowsAsync(new Exception("Erro de banco"));

            var result = await _useCase.GetFilialByIdAsync(1);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Contains("Ocorreu um erro ao buscar a filial", result.Error);
        }

        [Fact(DisplayName = "GetAllFiliaisAsync deve retornar Success com os dados")]
        [Trait("UseCase", "Filial")]
        public async Task GetAllFiliaisAsyncDeveRetornarSuccessComDados()
        {
            var listaFiliais = new List<FilialEntity> { CriarFilialValida(1), CriarFilialValida(2) };
            var pageResult = new PageResultModel<IEnumerable<FilialEntity>>
            {
                Data = listaFiliais,
                Total = 2,
                Offset = 0,
                Take = 3
            };

            _repositoryMock.Setup(r => r.GetAllFiliaisAsync(0, 3))
                           .ReturnsAsync(pageResult);

            var result = await _useCase.GetAllFiliaisAsync(0, 3);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value.Data);
            Assert.Equal(2, result.Value.Total);
            Assert.Equal(2, result.Value.Data.Count());
        }

        [Fact(DisplayName = "CreateFilialAsync deve retornar Success com a filial criada")]
        [Trait("UseCase", "Filial")]
        public async Task CreateFilialAsyncDeveRetornarSuccessComFilialCriada()
        {
            var filialDto = CriarFilialDtoValida();
            var filialCriada = CriarFilialValida(5);

            _repositoryMock.Setup(r => r.CreateFilialAsync(It.IsAny<FilialEntity>()))
                           .ReturnsAsync(filialCriada);

            var result = await _useCase.CreateFilialAsync(filialDto);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(5, result.Value.Id); 

            _repositoryMock.Verify(r => r.CreateFilialAsync(It.IsAny<FilialEntity>()), Times.Once);
        }

        [Fact(DisplayName = "DeleteFilialAsync com Id existente deve retornar Success")]
        [Trait("UseCase", "Filial")]
        public async Task DeleteFilialAsyncIdExistente_DeveRetornarSuccess()
        {
            var filial = CriarFilialValida(1);
            _repositoryMock.Setup(r => r.DeleteFilialAsync(1))
                           .ReturnsAsync(filial);

            var result = await _useCase.DeleteFilialAsync(1);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact(DisplayName = "DeleteFilialAsync com Id inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Filial")]
        public async Task DeleteFilialAsyncIdInexistenteDeveRetornarFailureNotFound()
        {
            _repositoryMock.Setup(r => r.DeleteFilialAsync(99))
                           .ReturnsAsync((FilialEntity?)null); 
            var result = await _useCase.DeleteFilialAsync(99);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Filial não encontrada", result.Error);
        }

        [Fact(DisplayName = "UpdateFilialAsync com Id inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Filial")]
        public async Task UpdateFilialAsyncIdInexistenteDeveRetornarFailureNotFound()
        {
            var filialDto = CriarFilialDtoValida();
            _repositoryMock.Setup(r => r.UpdateFilialAsync(99, It.IsAny<FilialEntity>()))
                           .ReturnsAsync((FilialEntity?)null); 

            var result = await _useCase.UpdateFilialAsync(99, filialDto);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Filial não encontrada", result.Error);
        }
    }
}