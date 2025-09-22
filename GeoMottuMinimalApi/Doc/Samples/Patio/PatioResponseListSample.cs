using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Patio
{
    public class PatioResponseListSample : IExamplesProvider<IEnumerable<PatioEntity>>
    {
        public IEnumerable<PatioEntity> GetExamples()
        {
            return new List<PatioEntity>
            {
                new PatioEntity
                {
                    Id = 1,
                    CapacidadeTotal = 100,
                    LocalizacaoReferencia = "Próximo a saída, portão azul",
                    Tamanho = 350.5,
                    TipoDoPatio = TipoPatio.Manutencao,
                    FilialId = 1,
                    CriadoEm = DateTime.Now.AddDays(-30)
                },
                new PatioEntity
                {
                    Id = 2,
                    CapacidadeTotal = 250,
                    LocalizacaoReferencia = "Perto do Coworking",
                    Tamanho = 600.0,
                    TipoDoPatio = TipoPatio.Disponivel,
                    FilialId = 1,
                    CriadoEm = DateTime.Now.AddDays(-15)
                }
            };
        }
    }
}
