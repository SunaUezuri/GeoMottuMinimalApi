using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Interfaces
{
    public interface IPatioUseCase
    {
        Task<PageResultModel<IEnumerable<PatioEntity>>> GetAllPatiosAsync(int offSet = 0, int take = 3);
        Task<PatioEntity?> GetPatioByIdAsync(int id);
        Task<PatioEntity?> CreatePatioAsync(PatioDto patio);
        Task<PatioEntity?> UpdatePatioAsync(int id, PatioDto patio);
        Task<PatioEntity?> DeletePatioAsync(int id);
    }
}
