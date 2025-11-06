using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.UseCases;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Domain.Interfaces;
using Moq;
using System.Net;

namespace GeoMottuMinimalApi.Tests.Application.UseCases
{
    public class PatioUseCaseTests
    {
        private readonly Mock<IPatioRepository> _repositoryMock;
        private readonly PatioUseCase _useCase;
        public PatioUseCaseTests()
        {
            _repositoryMock = new Mock<IPatioRepository>();
            _useCase = new PatioUseCase(_repositoryMock.Object);
        }

        private PatioEntity CriarPatioValido(int id = 1)
        {
            return new PatioEntity
            {
                Id = id,
                CapacidadeTotal = 100,
                Tamanho = 500,
                TipoDoPatio = TipoPatio.Disponivel,
                FilialId = 1
            };
        }

        private PatioDto CriarPatioDtoValido()
        {
            return new PatioDto(150, "Referencia DTO", 700, TipoPatio.Disponivel, 2);
        }

        /* Tests */

        [Fact(DisplayName = "GetPatioByIdAsync com Id existente deve retornar Success")]
        [Trait("UseCase", "Patio")]
        public async Task GetPatioByIdAsyncIdExistenteDeveRetornarSuccess()
        {
            var patio = CriarPatioValido(1);
            _repositoryMock.Setup(r => r.GetPatioByIdAsync(1))
                           .ReturnsAsync(patio);

            var result = await _useCase.GetPatioByIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact(DisplayName = "GetPatioByIdAsync com Id inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Patio")]
        public async Task GetPatioByIdAsyncIdInexistenteDeveRetornarFailureNotFound()
        {
            _repositoryMock.Setup(r => r.GetPatioByIdAsync(99))
                           .ReturnsAsync((PatioEntity?)null); 

            var result = await _useCase.GetPatioByIdAsync(99);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Pátio não encontrado", result.Error);
        }

        [Fact(DisplayName = "GetPatioByIdAsync com exceção do repositório deve retornar Failure")]
        [Trait("UseCase", "Patio")]
        public async Task GetPatioByIdAsyncRepositorioLancaExcecaoDeveRetornarFailure()
        {
            _repositoryMock.Setup(r => r.GetPatioByIdAsync(1))
                           .ThrowsAsync(new Exception("Erro de banco"));

            var result = await _useCase.GetPatioByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Contains("Ocorreu um erro ao buscar o pátio", result.Error);
        }

        [Fact(DisplayName = "GetAllPatiosAsync deve retornar Success com os dados")]
        [Trait("UseCase", "Patio")]
        public async Task GetAllPatiosAsyncDeveRetornarSuccessComDados()
        {
            var listaPatios = new List<PatioEntity> { CriarPatioValido(1), CriarPatioValido(2) };
            var pageResult = new PageResultModel<IEnumerable<PatioEntity>>
            {
                Data = listaPatios,
                Total = 2,
                Offset = 0,
                Take = 3
            };

            _repositoryMock.Setup(r => r.GetAllPatiosAsync(0, 3))
                           .ReturnsAsync(pageResult);

            var result = await _useCase.GetAllPatiosAsync(0, 3);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Total);
        }

        [Fact(DisplayName = "CreatePatioAsync deve retornar Success com o pátio criado")]
        [Trait("UseCase", "Patio")]
        public async Task CreatePatioAsyncDeveRetornarSuccessComPatioCriado()
        {
            var patioDto = CriarPatioDtoValido();
            var patioCriado = CriarPatioValido(5);

            _repositoryMock.Setup(r => r.CreatePatioAsync(It.IsAny<PatioEntity>()))
                           .ReturnsAsync(patioCriado);

            var result = await _useCase.CreatePatioAsync(patioDto);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(5, result.Value.Id);

            _repositoryMock.Verify(r => r.CreatePatioAsync(It.IsAny<PatioEntity>()), Times.Once);
        }

        [Fact(DisplayName = "DeletePatioAsync com Id existente deve retornar Success")]
        [Trait("UseCase", "Patio")]
        public async Task DeletePatioAsyncIdExistenteDeveRetornarSuccess()
        {
            var patio = CriarPatioValido(1);
            _repositoryMock.Setup(r => r.DeletePatioAsync(1))
                           .ReturnsAsync(patio);

            var result = await _useCase.DeletePatioAsync(1);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact(DisplayName = "DeletePatioAsync com Id inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Patio")]
        public async Task DeletePatioAsyncIdInexistenteDeveRetornarFailureNotFound()
        {
            _repositoryMock.Setup(r => r.DeletePatioAsync(99))
                           .ReturnsAsync((PatioEntity?)null);

            var result = await _useCase.DeletePatioAsync(99);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Pátio não encontrado", result.Error);
        }

        [Fact(DisplayName = "UpdatePatioAsync com Id inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Patio")]
        public async Task UpdatePatioAsync_IdInexistente_DeveRetornarFailureNotFound()
        {
            var patioDto = CriarPatioDtoValido();
            _repositoryMock.Setup(r => r.UpdatePatioAsync(99, It.IsAny<PatioEntity>()))
                           .ReturnsAsync((PatioEntity?)null);

            var result = await _useCase.UpdatePatioAsync(99, patioDto);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Pátio não encontrado", result.Error);
        }
    }
}