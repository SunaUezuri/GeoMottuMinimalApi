using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;

namespace GeoMottuMinimalApi.Application.Interfaces
{
    public interface IMotoUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<MotoEntity>>>> GetAllMotosAsync(int offSet = 0, int take = 3);
        Task<OperationResult<PageResultModel<IEnumerable<MotoEntity>>>> GetByModeloAsync(ModeloMoto modelo, int offSet = 0, int take = 0);
        Task<OperationResult<MotoEntity?>> GetMotoByIdAsync(int id);
        Task<OperationResult<MotoEntity?>> GetByChassiAsync(string chassi);
        Task<OperationResult<MotoEntity?>> GetByPlacaAsync(string placa);

        Task<OperationResult<MotoEntity?>> CreateMotoAsync(MotoDto moto);
        Task<OperationResult<MotoEntity?>> UpdateMotoAsync(int id, MotoDto moto);
        Task<OperationResult<MotoEntity?>> DeleteMotoAsync(int id);
    }
}
