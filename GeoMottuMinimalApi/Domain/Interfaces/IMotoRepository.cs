using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;

namespace GeoMottuMinimalApi.Domain.Interfaces
{
    public interface IMotoRepository
    {
        Task<PageResultModel<IEnumerable<MotoEntity>>> GetAllMotosAsync(int offSet, int take);
        Task<PageResultModel<IEnumerable<MotoEntity>>> GetByModeloAsync(ModeloMoto modelo, int offSet, int take);
        Task<MotoEntity?> GetMotoByIdAsync(int id);
        Task<MotoEntity?> GetByChassiAsync(string chassi);
        Task<MotoEntity?> GetByPlacaAsync(string placa);

        Task<MotoEntity?> CreateMotoAsync(MotoEntity moto);
        Task<MotoEntity?> UpdateMotoAsync(int id, MotoEntity moto);
        Task<MotoEntity?> DeleteMotoAsync(int id);

    }
}
