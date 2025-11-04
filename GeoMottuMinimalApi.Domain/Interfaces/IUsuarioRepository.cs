using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<PageResultModel<IEnumerable<UsuarioEntity>>> GetAllUsuariosAsync(int offSet = 0, int take = 3);
        Task<UsuarioEntity?> GetUsuarioByIdAsync(int id);
        Task<UsuarioEntity?> GetUsuarioByEmailAsync(string email);
        Task<UsuarioEntity?> CreateUsuarioAsync(UsuarioEntity usuario);
        Task<UsuarioEntity?> UpdateUsuarioAsync(int id, UsuarioEntity usuario);
        Task<UsuarioEntity?> DeleteUsuarioAsync(int id);
        Task<UsuarioEntity?> AutenticarAsync(string email, string senha);
    }
}
