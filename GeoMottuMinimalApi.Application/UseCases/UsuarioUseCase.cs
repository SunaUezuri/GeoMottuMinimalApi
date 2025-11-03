using GeoMottuMinimalApi.Application.Dtos;
using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Application.Mappers;
using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Interfaces;
using System.Net;

namespace GeoMottuMinimalApi.Application.UseCases
{
    public class UsuarioUseCase : IUsuarioUseCase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioUseCase(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<UsuarioEntity?>> CreateUsuarioAsync(UsuarioDto usuario)
        {
            try
            {
                var result = await _repository.CreateUsuarioAsync(usuario.ToUsuarioEntity());
                return OperationResult<UsuarioEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<UsuarioEntity?>.Failure("Ocorreu um erro ao salvar o usuário");
            }
        }

        public async Task<OperationResult<UsuarioEntity?>> DeleteUsuarioAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteUsuarioAsync(id);
                if (result is null)
                {
                    return OperationResult<UsuarioEntity?>.Failure("Usuário não encontrado", (int)HttpStatusCode.NotFound);
                }

                return OperationResult<UsuarioEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<UsuarioEntity?>.Failure("Ocorreu um erro ao deletar o usuário");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>> GetAllUsuariosAsync(int offSe = 0, int take = 3)
        {
            try
            {
                var result = await _repository.GetAllUsuariosAsync(offSe, take);
                return OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<PageResultModel<IEnumerable<UsuarioEntity>>>.Failure("Ocorreu um erro ao buscar os usuários");
            }
        }

        public async Task<OperationResult<UsuarioEntity?>> GetUsuarioByEmailAsync(string email)
        {
            try
            {
                var result = await _repository.GetUsuarioByEmailAsync(email);
                if (result is null)
                {
                    return OperationResult<UsuarioEntity?>.Failure("Usuário não encontrado", (int)HttpStatusCode.NotFound);
                }
                return OperationResult<UsuarioEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<UsuarioEntity?>.Failure("Ocorreu um erro ao buscar o usuário");
            }
        }

        public async Task<OperationResult<UsuarioEntity?>> GetUsuarioByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetUsuarioByIdAsync(id);
                if (result is null)
                {
                    return OperationResult<UsuarioEntity?>.Failure("Usuário não encontrado", (int)HttpStatusCode.NotFound);
                }
                return OperationResult<UsuarioEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<UsuarioEntity?>.Failure("Ocorreu um erro ao buscar o usuário");
            }
        }

        public async Task<OperationResult<UsuarioEntity?>> UpdateUsuarioAsync(int id, UsuarioDto usuario)
        {
            try
            {
                var result = await _repository.UpdateUsuarioAsync(id, usuario.ToUsuarioEntity());
                if (result is null)
                {
                    return OperationResult<UsuarioEntity?>.Failure("Usuário não encontrado", (int)HttpStatusCode.NotFound);
                }
                return OperationResult<UsuarioEntity?>.Success(result);
            }
            catch (Exception)
            {
                return OperationResult<UsuarioEntity?>.Failure("Ocorreu um erro ao atualizar o usuário");
            }
        }
    }
}
