using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Moto
{
    public class MotoResponseSample : IExamplesProvider<MotoEntity>
    {
        public MotoEntity GetExamples()
        {
            return new MotoEntity
            {
                Id = 101,
                Placa = "ABC1D23",
                Chassi = "9C2KD0800J0654321",
                CodPlacaIot = "IOT-MOTTU-111ABC",
                Modelo = ModeloMoto.MottuSport,
                Motor = 150.0,
                Proprietario = "Carlos Almeida",
                PosicaoX = -23.550,
                PosicaoY = -46.633,
                PatioId = 2,
                CriadoEm = DateTime.Now.AddDays(-5)
            };
        }
    }
}
