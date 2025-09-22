using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Interfaces
{
    public interface IFilialUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<FilialEntity>>>> GetAllFiliaisAsync(int offSet = 0, int take = 3);
        Task<OperationResult<FilialEntity?>> GetFilialByIdAsync(int id);
        Task<OperationResult<FilialEntity?>> CreateFilialAsync(FilialDto filial);
        Task<OperationResult<FilialEntity?>> UpdateFilialAsync(int id, FilialDto filial);
        Task<OperationResult<FilialEntity?>> DeleteFilialAsync(int id);
    }
}
