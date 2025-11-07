using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.UseCases;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Domain.Interfaces;
using Moq;
using System.Net;

namespace GeoMottuMinimalApi.Tests.Application.UseCases
{
    public class MotoUseCaseTests
    {
        private readonly Mock<IMotoRepository> _repositoryMock;
        private readonly Mock<IPatioRepository> _patioRepositoryMock;
        private readonly MotoUseCase _useCase;

        public MotoUseCaseTests()
        {
            _repositoryMock = new Mock<IMotoRepository>();
            _patioRepositoryMock = new Mock<IPatioRepository>();
            _useCase = new MotoUseCase(_repositoryMock.Object, _patioRepositoryMock.Object);
        }

        private MotoEntity CriarMotoValida(int id = 1, int patioId = 1)
        {
            return new MotoEntity
            {
                Id = id,
                Placa = $"MOT-{id:000}",
                Chassi = $"CHASSI-{id:000}",
                CodPlacaIot = $"IOT-{id:000}",
                Modelo = ModeloMoto.MottuSport,
                Motor = 125,
                PatioId = patioId
            };
        }

        private MotoDto CriarMotoDtoValido(int patioId = 1)
        {
            return new MotoDto(
                "DTO-001",
                "CHASSI-DTO-001",
                "IOT-DTO-001",
                ModeloMoto.MottuE,
                3000,
                "Proprietario DTO",
                -46.1,
                -23.1,
                patioId
            );
        }

        private PatioEntity CriarPatioValido(int id = 1, int capacidade = 10)
        {
            return new PatioEntity
            {
                Id = id,
                CapacidadeTotal = capacidade,
                TipoDoPatio = TipoPatio.Disponivel
            };
        }

        private PageResultModel<IEnumerable<MotoEntity>> CriarPageResultVazio()
        {
            return new PageResultModel<IEnumerable<MotoEntity>>
            {
                Data = new List<MotoEntity>(),
                Total = 0,
                Offset = 0,
                Take = 3
            };
        }

        /* Tests */

        [Fact(DisplayName = "CreateMotoAsync com pátio cheio deve retornar Failure")]
        [Trait("UseCase", "Moto")]
        public async Task CreateMotoAsyncPatioCheioDeveRetornarFailureCapacidadeMaxima()
        {
            var motoDto = CriarMotoDtoValido(patioId: 1);
            var patio = CriarPatioValido(id: 1, capacidade: 10);

            _patioRepositoryMock.Setup(r => r.GetPatioByIdAsync(1))
                                .ReturnsAsync(patio);

            _patioRepositoryMock.Setup(r => r.GetCurrentMotoCountAsync(1))
                                .ReturnsAsync(10);
            var result = await _useCase.CreateMotoAsync(motoDto);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Contains("Capacidade máxima do pátio atingida", result.Error);
            _repositoryMock.Verify(r => r.CreateMotoAsync(It.IsAny<MotoEntity>()), Times.Never);
        }

        [Fact(DisplayName = "CreateMotoAsync com pátio inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Moto")]
        public async Task CreateMotoAsyncPatioInexistenteDeveRetornarFailureNotFound()
        {
            var motoDto = CriarMotoDtoValido(patioId: 99);
            _patioRepositoryMock.Setup(r => r.GetPatioByIdAsync(99))
                                .ReturnsAsync((PatioEntity?)null);

            var result = await _useCase.CreateMotoAsync(motoDto);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Pátio não encontrado", result.Error);
        }

        [Fact(DisplayName = "CreateMotoAsync com pátio vago deve retornar Success")]
        [Trait("UseCase", "Moto")]
        public async Task CreateMotoAsyncPatioVagoDeveRetornarSuccess()
        {
            var motoDto = CriarMotoDtoValido(patioId: 1);
            var patio = CriarPatioValido(id: 1, capacidade: 10);
            var motoCriada = CriarMotoValida(id: 123);

            _patioRepositoryMock.Setup(r => r.GetPatioByIdAsync(1))
                                .ReturnsAsync(patio);

            _patioRepositoryMock.Setup(r => r.GetCurrentMotoCountAsync(1))
                                .ReturnsAsync(5);

            _repositoryMock.Setup(r => r.CreateMotoAsync(It.IsAny<MotoEntity>()))
                           .ReturnsAsync(motoCriada);

            var result = await _useCase.CreateMotoAsync(motoDto);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(123, result.Value.Id);
            _repositoryMock.Verify(r => r.CreateMotoAsync(It.IsAny<MotoEntity>()), Times.Once);
        }

        [Fact(DisplayName = "UpdateMotoAsync movendo para pátio lotado deve retornar Failure")]
        [Trait("UseCase", "Moto")]
        public async Task UpdateMotoAsyncMoverParaPatioLotadoDeveRetornarFailureCapacidadeMaxima()
        {
            var motoOriginal = CriarMotoValida(id: 1, patioId: 1);
            var motoDto = CriarMotoDtoValido(patioId: 2);
            var patioNovo = CriarPatioValido(id: 2, capacidade: 20);

            _repositoryMock.Setup(r => r.GetMotoByIdAsync(1))
                           .ReturnsAsync(motoOriginal);

            _patioRepositoryMock.Setup(r => r.GetPatioByIdAsync(2))
                                .ReturnsAsync(patioNovo);

            _patioRepositoryMock.Setup(r => r.GetCurrentMotoCountAsync(2))
                                .ReturnsAsync(20);

            var result = await _useCase.UpdateMotoAsync(1, motoDto);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Capacidade máxima do pátio atingida", result.Error);
            _repositoryMock.Verify(r => r.UpdateMotoAsync(It.IsAny<int>(), It.IsAny<MotoEntity>()), Times.Never);
        }

        [Fact(DisplayName = "UpdateMotoAsync sem mudar pátio deve retornar Success")]
        [Trait("UseCase", "Moto")]
        public async Task UpdateMotoAsyncSemMudarPatioDeveRetornarSuccess()
        {
            var motoOriginal = CriarMotoValida(id: 1, patioId: 1);
            var motoDto = CriarMotoDtoValido(patioId: 1);

            _repositoryMock.Setup(r => r.GetMotoByIdAsync(1))
                           .ReturnsAsync(motoOriginal);

            _repositoryMock.Setup(r => r.UpdateMotoAsync(1, It.IsAny<MotoEntity>()))
                           .ReturnsAsync(CriarMotoValida(1, 1)); // Retorna a moto "atualizada"

            var result = await _useCase.UpdateMotoAsync(1, motoDto);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _patioRepositoryMock.Verify(r => r.GetPatioByIdAsync(It.IsAny<int>()), Times.Never);
            _patioRepositoryMock.Verify(r => r.GetCurrentMotoCountAsync(It.IsAny<int>()), Times.Never);

            _repositoryMock.Verify(r => r.UpdateMotoAsync(1, It.IsAny<MotoEntity>()), Times.Once);
        }

        [Fact(DisplayName = "GetAllMotosAsync com lista vazia deve retornar Failure NoContent")]
        [Trait("UseCase", "Moto")]
        public async Task GetAllMotosAsyncListaVaziaDeveRetornarFailureNoContent()
        {
            var pageResultVazio = CriarPageResultVazio();
            _repositoryMock.Setup(r => r.GetAllMotosAsync(0, 3))
                           .ReturnsAsync(pageResultVazio);

            var result = await _useCase.GetAllMotosAsync(0, 3);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);
            Assert.Contains("Nenhuma moto encontrada", result.Error);
        }

        [Fact(DisplayName = "GetByModeloAsync com lista vazia deve retornar Failure NoContent")]
        [Trait("UseCase", "Moto")]
        public async Task GetByModeloAsyncListaVaziaDeveRetornarFailureNoContent()
        {
            var pageResultVazio = CriarPageResultVazio();
            _repositoryMock.Setup(r => r.GetByModeloAsync(ModeloMoto.MottuE, 0, 3))
                           .ReturnsAsync(pageResultVazio);

            var result = await _useCase.GetByModeloAsync(ModeloMoto.MottuE, 0, 3);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);
            Assert.Contains("Nenhuma moto encontrada para o modelo informado", result.Error);
        }

        [Fact(DisplayName = "GetByPlacaAsync com placa inexistente deve retornar Failure NotFound")]
        [Trait("UseCase", "Moto")]
        public async Task GetByPlacaAsyncPlacaInexistenteDeveRetornarFailureNotFound()
        {
            _repositoryMock.Setup(r => r.GetByPlacaAsync("NAO-EXISTE"))
                           .ReturnsAsync((MotoEntity?)null);

            var result = await _useCase.GetByPlacaAsync("NAO-EXISTE");

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Moto não encontrada", result.Error);
        }
    }
}