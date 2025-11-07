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
    public class FilialControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly Mock<IFilialUseCase> _useCaseMock;

        public FilialControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _useCaseMock = _factory.FilialUseCaseMock;
            _useCaseMock.Reset();
        }

        private FilialEntity CriarFilialValida(int id = 1)
        {
            return new FilialEntity
            {
                Id = id,
                Nome = "Filial Teste",
                PaisFilial = PaisesFiliais.Brasil,
                Estado = "SP"
            };
        }

        private FilialDto CriarFilialDtoValida()
        {
            return new FilialDto("Filial DTO", PaisesFiliais.Mexico, "MX", "Calle Teste");
        }

        /* Tests */

        [Fact(DisplayName = "GET /list deve retornar 200 OK com dados")]
        [Trait("Controller", "Filial")]
        public async Task Get_DeveRetornar200QuandoHaFiliais()
        {
            var listaFiliais = new List<FilialEntity> { CriarFilialValida(1) };
            var pageResult = new PageResultModel<IEnumerable<FilialEntity>>
            {
                Data = listaFiliais,
                Total = 1,
                Offset = 0,
                Take = 3
            };
            var successResult = OperationResult<PageResultModel<IEnumerable<FilialEntity>>>.Success(pageResult);

            _useCaseMock.Setup(u => u.GetAllFiliaisAsync(0, 3))
                        .ReturnsAsync(successResult);

            var response = await _client.GetAsync("/api/Filial/list?offSet=0&take=3");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data[0].id);
            Assert.Equal("Filial Teste", (string)content.data[0].nome);
        }

        [Fact(DisplayName = "GET /list deve retornar 204 NoContent quando UseCase falha")]
        [Trait("Controller", "Filial")]
        public async Task Get_DeveRetornar204QuandoUseCaseFalhaComNoContent()
        {
            var failureResult = OperationResult<PageResultModel<IEnumerable<FilialEntity>>>
                .Failure("Nenhuma filial encontrada", (int)HttpStatusCode.NoContent);

            _useCaseMock.Setup(u => u.GetAllFiliaisAsync(0, 3))
                        .ReturnsAsync(failureResult);

            var response = await _client.GetAsync("/api/Filial/list?offSet=0&take=3");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "GET /list/{id} deve retornar 200 OK quando filial existe")]
        [Trait("Controller", "Filial")]
        public async Task GetByIdDeveRetornar200QuandoFilialExiste()
        {
            var filial = CriarFilialValida(1);
            var successResult = OperationResult<FilialEntity?>.Success(filial);
            _useCaseMock.Setup(u => u.GetFilialByIdAsync(1))
                        .ReturnsAsync(successResult);

            var response = await _client.GetAsync("/api/Filial/list/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data.id);
        }

        [Fact(DisplayName = "GET /list/{id} deve retornar 404 NotFound quando filial não existe")]
        [Trait("Controller", "Filial")]
        public async Task GetByIdDeveRetornar404QuandoFilialNaoExiste()
        {
            var failureResult = OperationResult<FilialEntity?>
                .Failure("Filial não encontrada", (int)HttpStatusCode.NotFound);

            _useCaseMock.Setup(u => u.GetFilialByIdAsync(99))
                        .ReturnsAsync(failureResult);

            var response = await _client.GetAsync("/api/Filial/list/99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "POST /create deve retornar 200 OK (ou 201) com dados válidos")]
        [Trait("Controller", "Filial")]
        public async Task PostDeveRetornarOKQuandoCriaComSucesso()
        {
            var filialDto = CriarFilialDtoValida();
            var filialCriada = CriarFilialValida(1);

            var successResult = OperationResult<FilialEntity?>.Success(filialCriada);

            _useCaseMock.Setup(u => u.CreateFilialAsync(It.IsAny<FilialDto>()))
                        .ReturnsAsync(successResult);

            var response = await _client.PostAsJsonAsync("/api/Filial/create", filialDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(1, (int)content.data.id);
        }

        [Fact(DisplayName = "PUT /update/{id} deve retornar 200 OK com dados válidos")]
        [Trait("Controller", "Filial")]
        public async Task PutDeveRetornarOKQuandoAtualizaComSucesso()
        {
            var filialDto = CriarFilialDtoValida();
            var filialAtualizada = CriarFilialValida(1);
            var successResult = OperationResult<FilialEntity?>.Success(filialAtualizada);

            _useCaseMock.Setup(u => u.UpdateFilialAsync(1, It.IsAny<FilialDto>()))
                        .ReturnsAsync(successResult);
            
            var response = await _client.PutAsJsonAsync("/api/Filial/update/1", filialDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "PUT /update/{id} deve retornar 404 NotFound quando Id não existe")]
        [Trait("Controller", "Filial")]
        public async Task PutDeveRetornar404QuandoIdNaoExiste()
        {
            var filialDto = CriarFilialDtoValida();
            var failureResult = OperationResult<FilialEntity?>
                .Failure("Filial não encontrada", (int)HttpStatusCode.NotFound);

            _useCaseMock.Setup(u => u.UpdateFilialAsync(99, It.IsAny<FilialDto>()))
                        .ReturnsAsync(failureResult);

            var response = await _client.PutAsJsonAsync("/api/Filial/update/99", filialDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "DELETE /delete/{id} deve retornar 200 OK quando Id existe")]
        [Trait("Controller", "Filial")]
        public async Task DeleteDeveRetornarOKQuandoIdExiste()
        {
            var filialDeletada = CriarFilialValida(1);
            var successResult = OperationResult<FilialEntity?>.Success(filialDeletada);
            _useCaseMock.Setup(u => u.DeleteFilialAsync(1))
                        .ReturnsAsync(successResult);

            var response = await _client.DeleteAsync("/api/Filial/delete/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "DELETE /delete/{id} deve retornar 404 NotFound quando Id não existe")]
        [Trait("Controller", "Filial")]
        public async Task DeleteDeveRetornar404QuandoIdNaoExiste()
        {
            var failureResult = OperationResult<FilialEntity?>
                .Failure("Filial não encontrada", (int)HttpStatusCode.NotFound);

            _useCaseMock.Setup(u => u.DeleteFilialAsync(99))
                        .ReturnsAsync(failureResult);

            var response = await _client.DeleteAsync("/api/Filial/delete/99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
