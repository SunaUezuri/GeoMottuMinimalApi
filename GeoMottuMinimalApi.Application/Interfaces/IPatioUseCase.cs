using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Interfaces
{
    public interface IPatioUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<PatioEntity>>>> GetAllPatiosAsync(int offSet = 0, int take = 3);
        Task<OperationResult<PatioEntity?>> GetPatioByIdAsync(int id);
        Task<OperationResult<PatioEntity?>> CreatePatioAsync(PatioDto patio);
        Task<OperationResult<PatioEntity?>> UpdatePatioAsync(int id, PatioDto patio);
        Task<OperationResult<PatioEntity?>> DeletePatioAsync(int id);
    }
}
