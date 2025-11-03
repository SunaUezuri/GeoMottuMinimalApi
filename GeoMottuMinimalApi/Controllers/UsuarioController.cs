using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Doc.Samples.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioUseCase _usuarioUseCase;

        public UsuarioController(IUsuarioUseCase usuarioUseCase)
        {
            _usuarioUseCase = usuarioUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todos os usuários de forma paginada",
            Description = "Retorna uma lista paginada de usuários com links HATEOAS."
        )]
        [SwaggerResponse(statusCode: 200, description: "Lista de usuários retornada com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nenhum usuário encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(UsuarioResponseListSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> Get(int offSet = 0, int take = 3)
        {
            var result = await _usuarioUseCase.GetAllUsuariosAsync(offSet, take);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            var hateoas = new
            {
                data = result.Value.Data.Select(usuario => new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    usuario.FilialId,
                    usuario.Senha,
                    usuario.CadastradoEm,
                    links = new object[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetById), "Usuario", new { id = usuario.Id }, Request.Scheme) },
                        new { rel = "update", href = Url.Action(nameof(Put), "Usuario", new { id = usuario.Id }, Request.Scheme) },
                        new { rel = "delete", href = Url.Action(nameof(Delete), "Usuario", new { id = usuario.Id }, Request.Scheme) }
                    }
                }),
                links = new object[]
                {
                    new { rel = "self", href = Url.Action(nameof(Get), "Usuario", null, Request.Scheme) },
                    new { rel = "create", href = Url.Action(nameof(Post), "Usuario", null, Request.Scheme) }
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
        [SwaggerOperation(Summary = "Obtém um usuário por ID")]
        [SwaggerResponse(statusCode: 200, description: "Usuário encontrado")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(UsuarioResponseSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _usuarioUseCase.GetUsuarioByIdAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("email/{email}")]
        [SwaggerOperation(Summary = "Obtém um usuário por E-mail")]
        [SwaggerResponse(statusCode: 200, description: "Usuário encontrado")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(UsuarioResponseSample))]
        [EnableRateLimiting("ratelimit")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _usuarioUseCase.GetUsuarioByEmailAsync(email);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo usuário")]
        [SwaggerRequestExample(typeof(UsuarioDto), typeof(UsuarioRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Usuário criado com sucesso")]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponseExample(statusCode: 201, typeof(UsuarioResponseSample))]
        public async Task<IActionResult> Post([FromBody] UsuarioDto usuarioDto)
        {
            var result = await _usuarioUseCase.CreateUsuarioAsync(usuarioDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um usuário existente")]
        [SwaggerRequestExample(typeof(UsuarioDto), typeof(UsuarioRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Usuário atualizado com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(UsuarioResponseSample))]
        public async Task<IActionResult> Put(int id, [FromBody] UsuarioDto usuarioDto)
        {
            var result = await _usuarioUseCase.UpdateUsuarioAsync(id, usuarioDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um usuário")]
        [SwaggerResponse(statusCode: 200, description: "Usuário excluído com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponseExample(statusCode: 200, typeof(UsuarioResponseSample))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usuarioUseCase.DeleteUsuarioAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Error);
            }

            return StatusCode(result.StatusCode, result);
        }
    }
}