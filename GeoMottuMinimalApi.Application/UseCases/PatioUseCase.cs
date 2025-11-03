using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Application.Mappers;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Interfaces;
using System.Net;

namespace GeoMottuMinimalApi.Application.UseCases
{
    public class PatioUseCase : IPatioUseCase
    {
        private readonly IPatioRepository _repository;

        public PatioUseCase(IPatioRepository repository)
        {
            _repository = repository;
        }
        public async Task<OperationResult<PatioEntity?>> CreatePatioAsync(PatioDto patio)
        {
            try
            {
                var result = await _repository.CreatePatioAsync(patio.ToPatioEntity());

                return OperationResult<PatioEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<PatioEntity?>.Failure("Ocorreu um erro ao salvar o pátio");
            }
        }

        public async Task<OperationResult<PatioEntity?>> DeletePatioAsync(int id)
        {
            try
            {
                var result = await _repository.DeletePatioAsync(id);
                if (result is null)
                {
                    return OperationResult<PatioEntity?>.Failure("Pátio não encontrado", (int)HttpStatusCode.NotFound);
                }
                return OperationResult<PatioEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<PatioEntity?>.Failure("Ocorreu um erro ao deletar o pátio");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<PatioEntity>>>> GetAllPatiosAsync(int offSet = 0, int take = 3)
        {
            try
            {
                var result = await _repository.GetAllPatiosAsync(offSet, take);
                return OperationResult<PageResultModel<IEnumerable<PatioEntity>>>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<PageResultModel<IEnumerable<PatioEntity>>>.Failure("Ocorreu um erro ao buscar os pátios");
            }
        }

        public async Task<OperationResult<PatioEntity?>> GetPatioByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetPatioByIdAsync(id);
                if (result is null)
                {
                    return OperationResult<PatioEntity?>.Failure("Pátio não encontrado", (int)HttpStatusCode.NotFound);
                }
                return OperationResult<PatioEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<PatioEntity?>.Failure("Ocorreu um erro ao buscar o pátio");
            }
        }

        public async Task<OperationResult<PatioEntity?>> UpdatePatioAsync(int id, PatioDto patio)
        {
            try
            {
                var result = await _repository.UpdatePatioAsync(id, patio.ToPatioEntity());
                if (result is null)
                {
                    return OperationResult<PatioEntity?>.Failure("Pátio não encontrado", (int)HttpStatusCode.NotFound);
                }
                return OperationResult<PatioEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<PatioEntity?>.Failure("Ocorreu um erro ao atualizar o pátio");
            }
        }
    }
}
