using GeoMottuMinimalApi.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Usuario
{
    public class AuthUserRequestSample : IExamplesProvider<AuthUserDto>
    {
        public AuthUserDto GetExamples()
        {
            return new AuthUserDto(
                email: "fernanda@geomottu.com",
                senha: "SenhaSuperSegura123!"
                );
        }
    }
}
