using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Interfaces;
using GeoMottuMinimalApi.Infrastructure.Data.AppDatas;
using Microsoft.EntityFrameworkCore;

namespace GeoMottuMinimalApi.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationContext _context;

        public UsuarioRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<UsuarioEntity?> CreateUsuarioAsync(UsuarioEntity usuario)
        {
           _context.Usuario.Add(usuario);
           _context.SaveChanges();

            return usuario;
        }

        public async Task<UsuarioEntity?> DeleteUsuarioAsync(int id)
        {
            var result = await GetUsuarioByIdAsync(id);

            if (result is not null)
            {
                _context.Usuario.Remove(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<PageResultModel<IEnumerable<UsuarioEntity>>> GetAllUsuariosAsync(int offSet, int take)
        {
            var totalRegistros = await _context.Usuario.CountAsync();

            var result = await _context
                .Usuario
                .Include(u => u.Filial)
                .OrderBy(u => u.Nome)
                .Skip(offSet)
                .Take(take)
                .ToListAsync();

            return new PageResultModel<IEnumerable<UsuarioEntity>>
            {
                Data = result,
                Offset = offSet,
                Take = take,
                Total = totalRegistros
            };
        }

        public async Task<UsuarioEntity?> GetUsuarioByEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UsuarioEntity?> GetUsuarioByIdAsync(int id)
        {
            var result = await _context.Usuario
                .Include(u => u.Filial)
                .FirstOrDefaultAsync(u => u.Id == id);

            return result;
        }

        public async Task<UsuarioEntity?> UpdateUsuarioAsync(int id, UsuarioEntity usuario)
        {
            var result = await GetUsuarioByIdAsync(id);

            if (result is not null)
            {
                result.Nome = usuario.Nome;
                result.Email = usuario.Email;
                result.Senha = usuario.Senha;
                result.FilialId = usuario.FilialId;

                _context.Usuario.Update(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }
    }
}
