using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Doc.Samples.Filial;
using GeoMottuMinimalApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Presentation.Controllers
{
    [Route("api/filial")]
    [ApiController]
    public class FilialController : ControllerBase
    {
        private readonly IFilialUseCase _filialUseCase;

        public FilialController(IFilialUseCase filialUseCase)
        {
            _filialUseCase = filialUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as filiais de forma paginada",
            Description = "Retorna uma lista paginada de filiais com links HATEOAS."
        )]
        [SwaggerResponse(statusCode: 200, description: "Lista de filiais retornada com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma filial encontrada")]
        [SwaggerResponseExample(statusCode: 200, typeof(FilialResponseListSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> Get(int offSet = 0, int take = 3)
        {
            var result = await _filialUseCase.GetAllFiliaisAsync(offSet, take);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            var hateoas = new
            {
                data = result.Value.Data.Select(filial => new
                {
                    filial.Id,
                    filial.Nome,
                    filial.PaisFilial,
                    filial.Estado,
                    filial.Endereco,
                    links = new object[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetById), "Filial", new { id = filial.Id }, Request.Scheme) },
                        new { rel = "update", href = Url.Action(nameof(Put), "Filial", new { id = filial.Id }, Request.Scheme) },
                        new { rel = "delete", href = Url.Action(nameof(Delete), "Filial", new { id = filial.Id }, Request.Scheme) }
                    }
                }),
                links = new object[]
                {
                    new { rel = "self", href = Url.Action(nameof(Get), "Filial", null, Request.Scheme) },
                    new { rel = "create", href = Url.Action(nameof(Post), "Filial", null, Request.Scheme) }
                },
                pagination = new
                {
                    result.Value.Offset,
                    result.Value.Take,
                    result.Value.Total
                }
            };

            return StatusCode(result.StatusCode, hateoas);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtém uma filial por ID",
            Description = "Retorna a filial correspondente ao ID informado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Filial encontrada", type: typeof(FilialEntity))]
        [SwaggerResponse(statusCode: 404, description: "Filial não encontrada")]
        [SwaggerResponseExample(statusCode: 200, typeof(FilialResponseSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _filialUseCase.GetFilialByIdAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cria uma nova filial",
            Description = "Cadastra uma nova filial no sistema."
        )]
        [SwaggerRequestExample(typeof(FilialDto), typeof(FilialRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Filial criada com sucesso", type: typeof(FilialEntity))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponseExample(statusCode: 201, typeof(FilialResponseSample))]
        public async Task<IActionResult> Post(FilialDto filialDto)
        {
            var result = await _filialUseCase.CreateFilialAsync(filialDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualiza uma filial existente",
            Description = "Altera os dados de uma filial pelo seu ID."
        )]
        [SwaggerRequestExample(typeof(FilialDto), typeof(FilialRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Filial atualizada com sucesso", type: typeof(FilialEntity))]
        [SwaggerResponse(statusCode: 404, description: "Filial não encontrada")]
        [SwaggerResponseExample(statusCode: 200, typeof(FilialResponseSample))]
        public async Task<IActionResult> Put(int id, [FromBody] FilialDto filialDto)
        {
            var result = await _filialUseCase.UpdateFilialAsync(id, filialDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Exclui uma filial",
            Description = "Remove uma filial do sistema pelo seu ID."
        )]
        [SwaggerResponse(statusCode: 200, description: "Filial excluída com sucesso", type: typeof(FilialEntity))]
        [SwaggerResponse(statusCode: 404, description: "Filial não encontrada")]
        [SwaggerResponseExample(statusCode: 200, typeof(FilialResponseSample))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _filialUseCase.DeleteFilialAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }
    }
}