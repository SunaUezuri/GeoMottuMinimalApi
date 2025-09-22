using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Filial
{
    public class FilialResponseListSample : IExamplesProvider<IEnumerable<FilialEntity>>
    {
        public IEnumerable<FilialEntity> GetExamples()
        {
            return new List<FilialEntity>
            {
                new FilialEntity
                {
                    Id = 1,
                    Nome = "Filial São Paulo - Centro",
                    PaisFilial = PaisesFiliais.Brasil,
                    Estado = "São Paulo",
                    Endereco = "Avenida Paulista, 1578 - Bela Vista, São Paulo - SP",
                    CriadoEm = DateTime.Now.AddMonths(-6).AddDays(-20)
                },
                new FilialEntity
                {
                    Id = 2,
                    Nome = "Filial Rio de Janeiro - Copacabana",
                    PaisFilial = PaisesFiliais.Brasil,
                    Estado = "Rio de Janeiro",
                    Endereco = "Avenida Atlântica, 1702 - Copacabana, Rio de Janeiro - RJ",
                    CriadoEm = DateTime.Now.AddMonths(-3).AddDays(-5)
                }
            };
        }
    }
}
