using GeoMottuMinimalApi.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Usuario
{
    public class UsuarioResponseListSample : IExamplesProvider<IEnumerable<UsuarioEntity>>
    {
        public IEnumerable<UsuarioEntity> GetExamples()
        {
            return new List<UsuarioEntity>
            {
                new UsuarioEntity
                {
                    Id = 1,
                    Nome = "Fernanda Lima",
                    Email = "fernanda.lima@geomottu.com",
                    Senha = "$SenhaSuper957",
                    FilialId = 1,
                    CadastradoEm = DateTime.Now.AddDays(-10)
                },
                new UsuarioEntity
                {
                    Id = 2,
                    Nome = "Ricardo Alves",
                    Email = "ricardo.alves@geomottu.com",
                    Senha = "SenhaSuperSegura654",
                    FilialId = 2,
                    CadastradoEm = DateTime.Now.AddDays(-5)
                }
            };
        }
    }
}
