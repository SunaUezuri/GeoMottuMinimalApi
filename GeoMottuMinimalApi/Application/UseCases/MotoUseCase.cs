using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Application.Mappers;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Domain.Interfaces;
using System.Net;

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
                    return OperationResult<MotoEntity?>.Failure("Pátio não encontrado", (int)HttpStatusCode.NotFound);
                }

                var contagemPatio = await _patioRepository.GetCurrentMotoCountAsync(moto.PatioId);

                if(contagemPatio >= patio.CapacidadeTotal)
                {
                    return OperationResult<MotoEntity?>.Failure("Capacidade máxima do pátio atingida");
                }

                var result = await _repository.CreateMotoAsync(moto.ToMotoEntity());

                return OperationResult<MotoEntity?>.Success(result);
            } catch(Exception)
            {
                return OperationResult<MotoEntity?>.Failure("Ocorreu um erro ao salvar a moto");
            }
        }

        public async Task<OperationResult<MotoEntity?>> DeleteMotoAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteMotoAsync(id);

                if (result is null)
                {
                    return OperationResult<MotoEntity?>.Failure("Moto não encontrada", (int)HttpStatusCode.NotFound);
                }

                return OperationResult<MotoEntity?>.Success(result);
            }
            catch(Exception)
            {
                return OperationResult<MotoEntity?>.Failure("Ocorreu um erro ao deletar a moto");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<MotoEntity>>>> GetAllMotosAsync(int offSet = 0, int take = 3)
        {
            try
            {
                var result = await _repository.GetAllMotosAsync(offSet, take);

                if(!result.Data.Any())
                {
                    return OperationResult<PageResultModel<IEnumerable<MotoEntity>>>.Failure("Nenhuma moto encontrada", (int)HttpStatusCode.NoContent);
                }

                return OperationResult<PageResultModel<IEnumerable<MotoEntity>>>.Success(result);
            } catch(Exception)
            {
                return OperationResult<PageResultModel<IEnumerable<MotoEntity>>>.Failure("Ocorreu um erro ao buscar as motos");
            }
        }

        public async Task<OperationResult<MotoEntity?>> GetByChassiAsync(string chassi)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<MotoEntity>>>> GetByModeloAsync(ModeloMoto modelo, int offSet = 0, int take = 0)
        {
            try
            {
                var result = await _repository.GetByModeloAsync(modelo, offSet, take);

                if (!result.Data.Any())
                {
                    return OperationResult<PageResultModel<IEnumerable<MotoEntity>>>.Failure("Nenhuma moto encontrada para o modelo informado", (int)HttpStatusCode.NoContent);
                }

                return OperationResult<PageResultModel<IEnumerable<MotoEntity>>>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<PageResultModel<IEnumerable<MotoEntity>>>.Failure("Ocorreu um erro ao buscar as motos por modelo");
            }
        }

        public async Task<OperationResult<MotoEntity?>> GetByPlacaAsync(string placa)
        {
            try
            {
                var result = await _repository.GetByPlacaAsync(placa);

                if(result is null)
                {
                    return OperationResult<MotoEntity?>.Failure("Moto não encontrada", (int)HttpStatusCode.NotFound);
                }

                return OperationResult<MotoEntity?>.Success(result);
            }
            catch(Exception)
            {
                return OperationResult<MotoEntity?>.Failure("Ocorreu um erro ao buscar a moto pela placa");
            }
        }

        public async Task<OperationResult<MotoEntity?>> GetMotoByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetMotoByIdAsync(id);
                if(result is null)
                {
                    return OperationResult<MotoEntity?>.Failure("Moto não encontrada", (int)HttpStatusCode.NotFound);
                }
                return OperationResult<MotoEntity?>.Success(result);
            }
            catch(Exception)
            {
                return OperationResult<MotoEntity?>.Failure("Ocorreu um erro ao buscar a moto pelo ID");
            }
        }

        public async Task<OperationResult<MotoEntity?>> UpdateMotoAsync(int id, MotoDto moto)
        {
            try
            {
                var motoOriginal = await _repository.GetMotoByIdAsync(id);
                if(motoOriginal is null)
                {
                    return OperationResult<MotoEntity?>.Failure("Moto não encontrada", (int)HttpStatusCode.NotFound);
                }

                bool patioChanged = motoOriginal.PatioId != moto.PatioId;

                if (patioChanged)
                {
                    int newPatioId = moto.PatioId;

                    var newPatio = await _patioRepository.GetPatioByIdAsync(newPatioId);
                    if(newPatio is null)
                    {
                        return OperationResult<MotoEntity?>.Failure("Pátio não encontrado", (int)HttpStatusCode.NotFound);
                    }

                    var countPatio = await _patioRepository.GetCurrentMotoCountAsync(newPatioId);

                    if(countPatio >= newPatio.CapacidadeTotal)
                    {
                        return OperationResult<MotoEntity?>.Failure("Capacidade máxima do pátio atingida");
                    }
                }
                var result = await _repository.UpdateMotoAsync(id, moto.ToMotoEntity());

                if (result is null)
                {
                    return OperationResult<MotoEntity?>.Failure("Ocorreu um erro ao atualizar a moto");
                }

                return OperationResult<MotoEntity?>.Success(result);
            }
            catch(Exception)
            {
                return OperationResult<MotoEntity?>.Failure("Ocorreu um erro ao atualizar a moto");
            }
        }
    }
}
