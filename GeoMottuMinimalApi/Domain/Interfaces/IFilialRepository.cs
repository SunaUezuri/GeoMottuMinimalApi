using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Domain.Interfaces
{
    public interface IFilialRepository
    {
        Task<PageResultModel<IEnumerable<FilialEntity>>> GetAllFiliaisAsync(int offSet, int take);
        Task<FilialEntity?> GetFilialByIdAsync(int id);
        Task<FilialEntity> CreateFilialAsync(FilialEntity filial);
        Task<FilialEntity?> UpdateFilialAsync(int id, FilialEntity filial);
        Task<FilialEntity?> DeleteFilialAsync(int id);
    }
}
