using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;

namespace GeoMottuMinimalApi.Application.Interfaces
{
    public interface IMotoUseCase
    {
        Task<PageResultModel<IEnumerable<MotoEntity>>> GetAllMotosAsync(int offSet = 0, int take = 3);
        Task<PageResultModel<IEnumerable<MotoEntity>>> GetByModeloAsync(ModeloMoto modelo, int offSet = 0, int take = 0);
        Task<MotoEntity?> GetMotoByIdAsync(int id);
        Task<MotoEntity?> GetByChassiAsync(string chassi);
        Task<MotoEntity?> GetByPlacaAsync(string placa);

        Task<MotoEntity?> CreateMotoAsync(MotoDto moto);
        Task<MotoEntity?> UpdateMotoAsync(int id, MotoDto moto);
        Task<MotoEntity?> DeleteMotoAsync(int id);
    }
}
