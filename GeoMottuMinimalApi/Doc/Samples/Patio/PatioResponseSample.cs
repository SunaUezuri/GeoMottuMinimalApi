using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Patio
{
    public class PatioResponseSample : IExamplesProvider<PatioEntity>
    {
        public PatioEntity GetExamples()
        {
            return new PatioEntity
            {
                Id = 1,
                CapacidadeTotal = 100,
                LocalizacaoReferencia = "Próximo a saída, portão azul",
                Tamanho = 350.5,
                TipoDoPatio = TipoPatio.Disponivel,
                FilialId = 1,
                CriadoEm = DateTime.Now.AddDays(-30)
            };
        }
    }
}
