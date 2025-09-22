using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Mappers
{
    public static class MotoMapper
    {
        public static MotoEntity ToMotoEntity(this MotoDto dto)
        {
            return new MotoEntity
            {
                Placa = dto.Placa,
                Chassi = dto.Chassi,
                CodPlacaIot = dto.CodPlacaIot,
                Modelo = dto.ModeloMoto,
                Motor = dto.Motor,
                Proprietario = dto.Proprietario,
                PosicaoX = dto.PosicaoX,
                PosicaoY = dto.PosicaoY,
                PatioId = dto.PatioId
            };
        }
    }
}
