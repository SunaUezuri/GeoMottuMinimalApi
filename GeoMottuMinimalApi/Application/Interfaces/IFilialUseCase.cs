using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;

namespace GeoMottuMinimalApi.Application.Interfaces
{
    public interface IFilialUseCase
    {
        Task<PageResultModel<IEnumerable<FilialEntity>>> GetAllFiliaisAsync(int offSet = 0, int take = 3);
        Task<FilialEntity?> GetFilialByIdAsync(int id);
        Task<FilialEntity?> CreateFilialAsync(FilialDto filial);
        Task<FilialEntity?> UpdateFilialAsync(int id, FilialDto filial);
        Task<FilialEntity?> DeleteFilialAsync(int id);
    }
}
