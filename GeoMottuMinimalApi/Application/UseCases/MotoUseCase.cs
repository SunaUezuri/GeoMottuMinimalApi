using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Application.Mappers;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Domain.Interfaces;

namespace GeoMottuMinimalApi.Application.UseCases
{
    public class MotoUseCase : IMotoUseCase
    {
        private readonly IMotoRepository _repository;
        private readonly IPatioRepository _patioRepository;

        public MotoUseCase(IMotoRepository repository, IPatioRepository patioRepository)
        {
            _repository = repository;
            _patioRepository = patioRepository;
        }

        public async Task<OperationResult<MotoEntity?>> CreateMotoAsync(MotoDto moto)
        {
            try
            {
                var patio = await _patioRepository.GetPatioByIdAsync(moto.PatioId);
                if(patio is null)
                {
                    return OperationResult<MotoEntity?>.Failure("Pátio não encontrado", 404);
                }

                var contagemPatio = await _patioRepository.GetCurrentMotoCountAsync(moto.PatioId);

                if(contagemPatio >= patio.CapacidadeTotal)
                {
                    return OperationResult<MotoEntity?>.Failure("Capacidade máxima do pátio atingida");
                }

                var result = await _repository.CreateMotoAsync(moto.ToMotoEntity());

                return OperationResult<MotoEntity?>.Success(result);
            } catch
            {
                return OperationResult<MotoEntity?>.Failure("Ocorreu um erro ao salvar a moto");
            }
        }

        public async Task<MotoEntity?> DeleteMotoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResultModel<IEnumerable<MotoEntity>>> GetAllMotosAsync(int offSet = 0, int take = 3)
        {
            throw new NotImplementedException();
        }

        public async Task<MotoEntity?> GetByChassiAsync(string chassi)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResultModel<IEnumerable<MotoEntity>>> GetByModeloAsync(ModeloMoto modelo, int offSet = 0, int take = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<MotoEntity?> GetByPlacaAsync(string placa)
        {
            throw new NotImplementedException();
        }

        public async Task<MotoEntity?> GetMotoByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<MotoEntity?> UpdateMotoAsync(int id, MotoDto moto)
        {
            throw new NotImplementedException();
        }
    }
}
