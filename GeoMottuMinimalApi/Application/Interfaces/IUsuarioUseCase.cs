using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Interfaces
{
    public interface IUsuarioUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>> GetAllUsuariosAsync(int offSe = 0, int take = 3);
        Task<OperationResult<UsuarioEntity?>> GetUsuarioByIdAsync(int id);
        Task<OperationResult<UsuarioEntity?>> GetUsuarioByEmailAsync(string email);
        Task<OperationResult<UsuarioEntity?>> CreateUsuarioAsync(UsuarioDto usuario);
        Task<OperationResult<UsuarioEntity?>> UpdateUsuarioAsync(int id, UsuarioDto usuario);
        Task<OperationResult<UsuarioEntity?>> DeleteUsuarioAsync(int id);
    }
}
