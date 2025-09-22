using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Domain.Interfaces
{
    public interface IPatioRepository
    {
        Task<PageResultModel<IEnumerable<PatioEntity>>> GetAllPatiosAsync(int offSet, int take);
        Task<int> GetCurrentMotoCountAsync(int patioId);
        Task<PatioEntity?> GetPatioByIdAsync(int id);
        Task<PatioEntity?> CreatePatioAsync(PatioEntity patio);
        Task<PatioEntity?> UpdatePatioAsync(int id, PatioEntity patio);
        Task<PatioEntity?> DeletePatioAsync(int id);
    }
}
