using GeoMottuMinimalApi.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Usuario
{
    public class UsuarioRequestSample : IExamplesProvider<UsuarioDto>
    {
        public UsuarioDto GetExamples()
        {
            return new UsuarioDto(
                Nome: "Fernanda Lima",
                Email: "fernanda.lima@geomottu.com",
                Senha: "SenhaSuperSegura123!",
                FilialId: 1
            );
        }
    }
}
