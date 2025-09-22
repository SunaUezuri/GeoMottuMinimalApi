using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace GeoMottuMinimalApi.Doc.Samples.Moto
{
    public class MotoResponseListSample : IExamplesProvider<IEnumerable<MotoEntity>>
    {
        public IEnumerable<MotoEntity> GetExamples()
        {
            return new List<MotoEntity>
            {
                new MotoEntity
                {
                    Id = 101,
                    Placa = "ABC1D23",
                    Chassi = "9C2KD0800J0654321",
                    CodPlacaIot = "IOT-MOTTU-111ABC",
                    Modelo = ModeloMoto.MottuE,
                    Motor = 150.0,
                    Proprietario = "Carlos Almeida",
                    PosicaoX = -23.550,
                    PosicaoY = -46.633,
                    PatioId = 2,
                    CriadoEm = DateTime.Now.AddDays(-5)
                },
                new MotoEntity
                {
                    Id = 102,
                    Placa = "XYZ9F87",
                    Chassi = "8A1BDR050G0987654",
                    CodPlacaIot = "IOT-MOTTU-222DEF",
                    Modelo = ModeloMoto.MottuPop,
                    Motor = 883.0,
                    Proprietario = "",
                    PosicaoX = -22.906,
                    PosicaoY = -43.172,
                    PatioId = 1,
                    CriadoEm = DateTime.Now.AddDays(-2)
                }
            };
        }
    }
}
