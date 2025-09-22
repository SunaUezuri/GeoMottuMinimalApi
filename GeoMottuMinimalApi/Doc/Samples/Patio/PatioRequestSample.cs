using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Patio
{
    public class PatioRequestSample : IExamplesProvider<PatioDto>
    {
        public PatioDto GetExamples()
        {
            return new PatioDto(
                CapacidadeTotal: 100,
                LocalReferencia: "Próximo a saída, portão azul",
                Tamanho: 350.5,
                TipoPatio: TipoPatio.Disponivel,
                FilialId: 1
            );
        }
    }
}
