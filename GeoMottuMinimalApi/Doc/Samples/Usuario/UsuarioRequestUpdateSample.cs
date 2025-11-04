using GeoMottuMinimalApi.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Usuario
{
    public class UsuarioRequestUpdateSample : IExamplesProvider<UsuarioUpdateDto>
    {
        public UsuarioUpdateDto GetExamples()
        {
            return new UsuarioUpdateDto(
                Nome: "Fernanda Lima",
                Email: "fernanda.lima@geomottu.com",
                Senha: "SenhaSuperSegura123!",
                Role: "ADMIN",
                FilialId: 1
                );
        }
    }
}
