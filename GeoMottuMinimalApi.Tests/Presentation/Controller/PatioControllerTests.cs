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
    public class PatioControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly Mock<IPatioUseCase> _useCaseMock;

        public PatioControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _useCaseMock = _factory.PatioUseCaseMock;
            _useCaseMock.Reset();
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

        [Fact(DisplayName = "GET /list deve retornar 200 OK com dados")]
        [Trait("Controller", "Patio")]
        public async Task GetDeveRetornar200QuandoHaPatios()
        {
            var listaPatios = new List<PatioEntity> { CriarPatioValido(1) };
            var pageResult = new PageResultModel<IEnumerable<PatioEntity>>
            {
                Data = listaPatios,
                Total = 1,
                Offset = 0,
                Take = 3
            };
            var successResult = OperationResult<PageResultModel<IEnumerable<PatioEntity>>>.Success(pageResult);

            _useCaseMock.Setup(u => u.GetAllPatiosAsync(0, 3))
                        .ReturnsAsync(successResult);

            var response = await _client.GetAsync("/api/Patio/list?offSet=0&take=3");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data[0].id);
            Assert.Equal(100, (int)content.data[0].capacidadeTotal);
        }

        [Fact(DisplayName = "GET /list deve retornar 204 NoContent quando UseCase falha")]
        [Trait("Controller", "Patio")]
        public async Task GetDeveRetornar204QuandoUseCaseFalhaComNoContent()
        {
            var failureResult = OperationResult<PageResultModel<IEnumerable<PatioEntity>>>
                .Failure("Nenhum pátio encontrado", (int)HttpStatusCode.NoContent);

            _useCaseMock.Setup(u => u.GetAllPatiosAsync(0, 3))
                        .ReturnsAsync(failureResult);

            var response = await _client.GetAsync("/api/Patio/list?offSet=0&take=3");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "GET /list/{id} deve retornar 200 OK quando pátio existe")]
        [Trait("Controller", "Patio")]
        public async Task GetByIdDeveRetornar200QuandoPatioExiste()
        {
            var patio = CriarPatioValido(1);
            var successResult = OperationResult<PatioEntity?>.Success(patio);
            _useCaseMock.Setup(u => u.GetPatioByIdAsync(1))
                        .ReturnsAsync(successResult);

            var response = await _client.GetAsync("/api/Patio/list/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data.id);
        }

        [Fact(DisplayName = "GET /list/{id} deve retornar 404 NotFound quando pátio não existe")]
        [Trait("Controller", "Patio")]
        public async Task GetByIdDeveRetornar404QuandoPatioNaoExiste()
        {
            var failureResult = OperationResult<PatioEntity?>
                .Failure("Pátio não encontrado", (int)HttpStatusCode.NotFound);

            _useCaseMock.Setup(u => u.GetPatioByIdAsync(99))
                        .ReturnsAsync(failureResult);

            var response = await _client.GetAsync("/api/Patio/list/99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "POST /create deve retornar 200 OK (ou 201) com dados válidos")]
        [Trait("Controller", "Patio")]
        public async Task PostDeveRetornarOKQuandoCriaComSucesso()
        {
            var patioDto = CriarPatioDtoValido();
            var patioCriado = CriarPatioValido(1);

            var successResult = OperationResult<PatioEntity?>.Success(patioCriado);

            _useCaseMock.Setup(u => u.CreatePatioAsync(It.IsAny<PatioDto>()))
                        .ReturnsAsync(successResult);

            var response = await _client.PostAsJsonAsync("/api/Patio/create", patioDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data.id);
        }

        [Fact(DisplayName = "PUT /update/{id} deve retornar 200 OK com dados válidos")]
        [Trait("Controller", "Patio")]
        public async Task PutDeveRetornarOKQuandoAtualizaComSucesso()
        {
            var patioDto = CriarPatioDtoValido();
            var patioAtualizado = CriarPatioValido(1);
            var successResult = OperationResult<PatioEntity?>.Success(patioAtualizado);

            _useCaseMock.Setup(u => u.UpdatePatioAsync(1, It.IsAny<PatioDto>()))
                        .ReturnsAsync(successResult);

            var response = await _client.PutAsJsonAsync("/api/Patio/update/1", patioDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "PUT /update/{id} deve retornar 404 NotFound quando Id não existe")]
        [Trait("Controller", "Patio")]
        public async Task PutDeveRetornar404QuandoIdNaoExiste()
        {
            var patioDto = CriarPatioDtoValido();
            var failureResult = OperationResult<PatioEntity?>
                .Failure("Pátio não encontrado", (int)HttpStatusCode.NotFound);

            _useCaseMock.Setup(u => u.UpdatePatioAsync(99, It.IsAny<PatioDto>()))
                        .ReturnsAsync(failureResult);

            var response = await _client.PutAsJsonAsync("/api/Patio/update/99", patioDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "DELETE /delete/{id} deve retornar 200 OK quando Id existe")]
        [Trait("Controller", "Patio")]
        public async Task DeleteDeveRetornarOKQuandoIdExiste()
        {
            var patioDeletado = CriarPatioValido(1);
            var successResult = OperationResult<PatioEntity?>.Success(patioDeletado);
            _useCaseMock.Setup(u => u.DeletePatioAsync(1))
                        .ReturnsAsync(successResult);

            var response = await _client.DeleteAsync("/api/Patio/delete/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "DELETE /delete/{id} deve retornar 404 NotFound quando Id não existe")]
        [Trait("Controller", "Patio")]
        public async Task DeleteDeveRetornar404QuandoIdNaoExiste()
        {
            var failureResult = OperationResult<PatioEntity?>
                .Failure("Pátio não encontrado", (int)HttpStatusCode.NotFound);

            _useCaseMock.Setup(u => u.DeletePatioAsync(99))
                        .ReturnsAsync(failureResult);

            var response = await _client.DeleteAsync("/api/Patio/delete/99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
