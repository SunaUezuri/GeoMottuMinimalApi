using GeoMottuMinimalApi.Domain.Enums;

namespace GeoMottuMinimalApi.Application.Dtos
{
    public record MotoDto(string Placa, string Chassi, string CodPlacaIot, ModeloMoto ModeloMoto, double Motor, string Proprietario, double PosicaoX, double PosicaoY, int PatioId);
}
