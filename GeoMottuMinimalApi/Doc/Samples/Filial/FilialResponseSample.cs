using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Filial
{
    public class FilialResponseSample : IExamplesProvider<FilialEntity>
    {
        public FilialEntity GetExamples()
        {
            return new FilialEntity
            {
                Id = 1,
                Nome = "Filial São Paulo - Centro",
                PaisFilial = PaisesFiliais.Brasil,
                Estado = "São Paulo",
                Endereco = "Avenida Paulista, 1578 - Bela Vista, São Paulo - SP",
                CriadoEm = DateTime.Now.AddMonths(-6).AddDays(-12)
            };
        }
    }
}
