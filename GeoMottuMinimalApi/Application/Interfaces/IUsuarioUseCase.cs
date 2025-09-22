using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Interfaces
{
    public interface IUsuarioUseCase
    {
        Task<PageResultModel<IEnumerable<UsuarioEntity>>> GetAllUsuariosAsync(int offSe = 0, int take = 3);
        Task<UsuarioEntity?> GetUsuarioByIdAsync(int id);
        Task<UsuarioEntity?> GetUsuarioByEmailAsync(string email);
        Task<UsuarioEntity?> CreateUsuarioAsync(UsuarioDto usuario);
        Task<UsuarioEntity?> UpdateUsuarioAsync(int id, UsuarioDto usuario);
        Task<UsuarioEntity?> DeleteUsuarioAsync(int id);
    }
}
