using GeoMottuMinimalApi.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Usuario
{
    public class UsuarioResponseSample : IExamplesProvider<UsuarioEntity>
    {
        public UsuarioEntity GetExamples()
        {
            return new UsuarioEntity
            {
                Id = 1,
                Nome = "Fernanda Lima",
                Email = "fernanda.lima@geomottu.com",
                Senha = "@Senha5687",
                FilialId = 1,
                Role = "ADMIN",
                CadastradoEm = DateTime.Now.AddDays(-10)
            };
        }
    }
}
