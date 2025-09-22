using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Moto
{
    public class MotoRequestSample : IExamplesProvider<MotoDto>
    {
        public MotoDto GetExamples()
        {
            return new MotoDto(
                Placa: "BRA2E19",
                Chassi: "9C6KDR030H0123456",
                CodPlacaIot: "IOT-MOTTU-789XYZ",
                ModeloMoto: ModeloMoto.MottuSport,
                Motor: 600.5,
                Proprietario: "Ana Carolina",
                PosicaoX: -23.682,
                PosicaoY: -46.875,
                PatioId: 1
            );
        }
    }

}
