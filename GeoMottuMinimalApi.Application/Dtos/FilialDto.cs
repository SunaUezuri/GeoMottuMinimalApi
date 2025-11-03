using GeoMottuMinimalApi.Domain.Enums;

namespace GeoMottuMinimalApi.Application.Dtos
{
    public record FilialDto(string Nome, PaisesFiliais PaisesFiliais, string Estado, string Endereco);
}
