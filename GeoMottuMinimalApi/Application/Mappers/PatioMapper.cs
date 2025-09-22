using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Mappers
{
    public static class PatioMapper
    {
        public static PatioEntity ToPatioEntity(this PatioDto dto)
        {
            return new PatioEntity
            {
                CapacidadeTotal = dto.CapacidadeTotal,
                LocalizacaoReferencia = dto.LocalReferencia,
                Tamanho = dto.Tamanho,
                TipoDoPatio = dto.TipoPatio,
                FilialId = dto.FilialId
            };
        }
    }
}
