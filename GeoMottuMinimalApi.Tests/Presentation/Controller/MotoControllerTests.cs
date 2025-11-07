using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Tests.Presentation.Handlers;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace GeoMottuMinimalApi.Tests.Presentation.Controller
{
    public class MotoControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly Mock<IMotoUseCase> _useCaseMock;

        public MotoControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _useCaseMock = _factory.MotoUseCaseMock;

            _useCaseMock.Reset();
        }

        private MotoEntity CriarMotoValida(int id = 1)
        {
            return new MotoEntity
            {
                Id = id,
                Placa = $"MOT-{id:000}",
                Chassi = $"CHASSI-{id:000}",
                CodPlacaIot = $"IOT-{id:000}",
                Modelo = ModeloMoto.MottuSport,
                Motor = 125,
                PatioId = 1
            };
        }

        private MotoDto CriarMotoDtoValido()
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
                1 // PatioId
            );
        }

        /* Tests */

        [Fact(DisplayName = "GET /list deve retornar 200 OK com dados")]
        [Trait("Controller", "Moto")]
        public async Task GetDeveRetornar200QuandoHaMotos()
        {
            var listaMotos = new List<MotoEntity> { CriarMotoValida(1) };
            var pageResult = new PageResultModel<IEnumerable<MotoEntity>>
            {
                Data = listaMotos,
                Total = 1,
                Offset = 0,
                Take = 3
            };
            var successResult = OperationResult<PageResultModel<IEnumerable<MotoEntity>>>.Success(pageResult);

            _useCaseMock.Setup(u => u.GetAllMotosAsync(0, 3))
                        .ReturnsAsync(successResult);

            var response = await _client.GetAsync("/api/Moto/list?offSet=0&take=3");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data[0].id);
            Assert.Equal("MOT-001", (string)content.data[0].placa);
        }

        [Fact(DisplayName = "GET /list deve retornar 204 NoContent quando lista vazia")]
        [Trait("Controller", "Moto")]
        public async Task GetDeveRetornar204QuandoListaVazia()
        {
            var failureResult = OperationResult<PageResultModel<IEnumerable<MotoEntity>>>
                .Failure("Nenhuma moto encontrada", (int)HttpStatusCode.NoContent);

            _useCaseMock.Setup(u => u.GetAllMotosAsync(0, 3))
                        .ReturnsAsync(failureResult);

            var response = await _client.GetAsync("/api/Moto/list?offSet=0&take=3");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "GET /list/{id} deve retornar 200 OK quando moto existe")]
        [Trait("Controller", "Moto")]
        public async Task GetByIdDeveRetornar200QuandoMotoExiste()
        {
            var moto = CriarMotoValida(1);
            var successResult = OperationResult<MotoEntity?>.Success(moto);
            _useCaseMock.Setup(u => u.GetMotoByIdAsync(1))
                        .ReturnsAsync(successResult);

            var response = await _client.GetAsync("/api/Moto/list/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "GET /placa/{placa} deve retornar 404 NotFound")]
        [Trait("Controller", "Moto")]
        public async Task GetByPlacaDeveRetornar404QuandoPlacaNaoExiste()
        {
            var failureResult = OperationResult<MotoEntity?>
                .Failure("Moto não encontrada", (int)HttpStatusCode.NotFound);

            _useCaseMock.Setup(u => u.GetByPlacaAsync("NAO-EXISTE"))
                        .ReturnsAsync(failureResult);

            var response = await _client.GetAsync("/api/Moto/placa/NAO-EXISTE");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "POST /create deve retornar 200 OK com dados válidos")]
        [Trait("Controller", "Moto")]
        public async Task PostDeveRetornarOKQuandoCriaComSucesso()
        {
            var motoDto = CriarMotoDtoValido();
            var motoCriada = CriarMotoValida(1);
            var successResult = OperationResult<MotoEntity?>.Success(motoCriada);

            _useCaseMock.Setup(u => u.CreateMotoAsync(It.IsAny<MotoDto>()))
                        .ReturnsAsync(successResult);

            var response = await _client.PostAsJsonAsync("/api/Moto/create", motoDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data.id);
        }

        [Fact(DisplayName = "POST /create deve retornar 400 BadRequest quando pátio lotado")]
        [Trait("Controller", "Moto")]
        public async Task PostDeveRetornar400QuandoPatioLotado()
        {
            var motoDto = CriarMotoDtoValido();

            var failureResult = OperationResult<MotoEntity?>
                .Failure("Capacidade máxima do pátio atingida", (int)HttpStatusCode.BadRequest); // Assumindo 400

            _useCaseMock.Setup(u => u.CreateMotoAsync(It.IsAny<MotoDto>()))
                        .ReturnsAsync(failureResult);

            var response = await _client.PostAsJsonAsync("/api/Moto/create", motoDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Capacidade máxima do pátio atingida", errorString);
        }

        [Fact(DisplayName = "PUT /update/{id} deve retornar 200 OK com dados válidos")]
        [Trait("Controller", "Moto")]
        public async Task PutDeveRetornarOKQuandoAtualizaComSucesso()
        {
            var motoDto = CriarMotoDtoValido();
            var motoAtualizada = CriarMotoValida(1);
            var successResult = OperationResult<MotoEntity?>.Success(motoAtualizada);

            _useCaseMock.Setup(u => u.UpdateMotoAsync(1, It.IsAny<MotoDto>()))
                        .ReturnsAsync(successResult);

            var response = await _client.PutAsJsonAsync("/api/Moto/update/1", motoDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "DELETE /delete/{id} deve retornar 404 NotFound quando Id não existe")]
        [Trait("Controller", "Moto")]
        public async Task Delete_DeveRetornar404_QuandoIdNaoExiste()
        {
            var failureResult = OperationResult<MotoEntity?>
                .Failure("Moto não encontrada", (int)HttpStatusCode.NotFound);

            _useCaseMock.Setup(u => u.DeleteMotoAsync(99))
                        .ReturnsAsync(failureResult);

            var response = await _client.DeleteAsync("/api/Moto/delete/99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
