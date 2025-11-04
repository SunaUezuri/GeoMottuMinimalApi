namespace GeoMottuMinimalApi.Application.Dtos
{
    public record UsuarioUpdateDto(string Nome, string Email, string Senha, string Role, int FilialId);
}
