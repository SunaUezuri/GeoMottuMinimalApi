using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Doc.Samples.Patio;
using GeoMottuMinimalApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Presentation.Controllers
{
    [Route("api/patio")]
    [ApiController]
    public class PatioController : ControllerBase
    {
        private readonly IPatioUseCase _patioUseCase;

        public PatioController(IPatioUseCase patioUseCase)
        {
            _patioUseCase = patioUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todos os pátios de forma paginada",
            Description = "Retorna uma lista paginada de pátios com links HATEOAS."
        )]
        [SwaggerResponse(statusCode: 200, description: "Lista de pátios retornada com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nenhum pátio encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(PatioResponseListSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> Get(int offSet = 0, int take = 3)
        {
            var result = await _patioUseCase.GetAllPatiosAsync(offSet, take);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            var hateoas = new
            {
                data = result.Value.Data.Select(patio => new
                {
                    patio.Id,
                    patio.CapacidadeTotal,
                    patio.LocalizacaoReferencia,
                    patio.Tamanho,
                    patio.TipoDoPatio,
                    patio.FilialId,
                    links = new object[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetById), "Patio", new { id = patio.Id }, Request.Scheme) },
                        new { rel = "update", href = Url.Action(nameof(Put), "Patio", new { id = patio.Id }, Request.Scheme) },
                        new { rel = "delete", href = Url.Action(nameof(Delete), "Patio", new { id = patio.Id }, Request.Scheme) }
                    }
                }),
                links = new object[]
                {
                    new { rel = "self", href = Url.Action(nameof(Get), "Patio", null, Request.Scheme) },
                    new { rel = "create", href = Url.Action(nameof(Post), "Patio", null, Request.Scheme) }
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
            Summary = "Obtém um pátio por ID",
            Description = "Retorna o pátio correspondente ao ID informado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Pátio encontrado", type: typeof(PatioEntity))]
        [SwaggerResponse(statusCode: 404, description: "Pátio não encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(PatioResponseSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _patioUseCase.GetPatioByIdAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cria um novo pátio",
            Description = "Cadastra um novo pátio no sistema."
        )]
        [SwaggerRequestExample(typeof(PatioDto), typeof(PatioRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Pátio criado com sucesso", type: typeof(PatioEntity))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponseExample(statusCode: 201, typeof(PatioResponseSample))]
        public async Task<IActionResult> Post([FromBody] PatioDto patioDto)
        {
            var result = await _patioUseCase.CreatePatioAsync(patioDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualiza um pátio existente",
            Description = "Altera os dados de um pátio pelo seu ID."
        )]
        [SwaggerRequestExample(typeof(PatioDto), typeof(PatioRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Pátio atualizado com sucesso", type: typeof(PatioEntity))]
        [SwaggerResponse(statusCode: 404, description: "Pátio não encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(PatioResponseSample))]
        public async Task<IActionResult> Put(int id, [FromBody] PatioDto patioDto)
        {
            var result = await _patioUseCase.UpdatePatioAsync(id, patioDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Exclui um pátio",
            Description = "Remove um pátio do sistema pelo seu ID."
        )]
        [SwaggerResponse(statusCode: 200, description: "Pátio excluído com sucesso", type: typeof(PatioEntity))]
        [SwaggerResponse(statusCode: 404, description: "Pátio não encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(PatioResponseSample))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patioUseCase.DeletePatioAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result.Value);
        }
    }
}