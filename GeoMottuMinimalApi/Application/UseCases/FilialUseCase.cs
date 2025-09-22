using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Application.Mappers;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Interfaces;
using System.Net;

namespace GeoMottuMinimalApi.Application.UseCases
{
    public class FilialUseCase : IFilialUseCase
    {
        private readonly IFilialRepository _repository;

        public FilialUseCase(IFilialRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<FilialEntity?>> CreateFilialAsync(FilialDto filial)
        {
            try
            {
                var result = await _repository.CreateFilialAsync(filial.ToFilialEntity());

                return OperationResult<FilialEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<FilialEntity?>.Failure("Ocorreu um erro ao salvar a filial");
            }
        }

        public async Task<OperationResult<FilialEntity?>> DeleteFilialAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteFilialAsync(id);
                if (result is null)
                {
                    return OperationResult<FilialEntity?>.Failure("Filial não encontrada", (int)HttpStatusCode.NotFound);
                }

                return OperationResult<FilialEntity?>.Success(result);
            }catch(Exception)
            {
                return OperationResult<FilialEntity?>.Failure("Ocorreu um erro ao deletar a filial");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<FilialEntity>>>> GetAllFiliaisAsync(int offSet = 0, int take = 3)
        {
            try
            {
                var result = await _repository.GetAllFiliaisAsync(offSet, take);
                return OperationResult<PageResultModel<IEnumerable<FilialEntity>>>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<PageResultModel<IEnumerable<FilialEntity>>>.Failure("Ocorreu um erro ao buscar as filiais");
            }
        }

        public async Task<OperationResult<FilialEntity?>> GetFilialByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetFilialByIdAsync(id);
                if (result is null)
                {
                    return OperationResult<FilialEntity?>.Failure("Filial não encontrada", (int)HttpStatusCode.NotFound);
                }
                return OperationResult<FilialEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<FilialEntity?>.Failure("Ocorreu um erro ao buscar a filial");
            }
        }

        public async Task<OperationResult<FilialEntity?>> UpdateFilialAsync(int id, FilialDto filial)
        {
            try
            {
                var result = await _repository.UpdateFilialAsync(id, filial.ToFilialEntity());
                if (result is null)
                {
                    return OperationResult<FilialEntity?>.Failure("Filial não encontrada", (int)HttpStatusCode.NotFound);
                }
                return OperationResult<FilialEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<FilialEntity?>.Failure("Ocorreu um erro ao atualizar a filial");
            }
        }
    }
}
