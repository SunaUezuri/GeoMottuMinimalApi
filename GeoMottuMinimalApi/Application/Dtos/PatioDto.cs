using GeoMottuMinimalApi.Domain.Enums;

namespace GeoMottuMinimalApi.Application.Dtos
{
    public record PatioDto(int CapacidadeTotal, string LocalReferencia, double Tamanho, TipoPatio TipoPatio, int FilialId);
}
