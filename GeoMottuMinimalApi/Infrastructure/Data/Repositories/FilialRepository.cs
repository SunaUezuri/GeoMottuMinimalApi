using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Interfaces;
using GeoMottuMinimalApi.Infrastructure.Data.AppDatas;
using Microsoft.EntityFrameworkCore;

namespace GeoMottuMinimalApi.Infrastructure.Data.Repositories
{
    public class FilialRepository : IFilialRepository
    {
        private readonly ApplicationContext _context;

        public FilialRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<FilialEntity?> CreateFilialAsync(FilialEntity filial)
        {
            _context.Filial.Add(filial);
            _context.SaveChanges();

            return filial;
        }

        public async Task<FilialEntity?> DeleteFilialAsync(int id)
        {
            var result = await GetFilialByIdAsync(id);

            if (result is not null)
            {
                _context.Filial.Remove(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<PageResultModel<IEnumerable<FilialEntity>>> GetAllFiliaisAsync(int offSet = 0, int take = 3)
        {
            var totalRegistros = await _context.Filial.CountAsync();

            var result = await _context
                .Filial
                .Include(u => u.Usuarios)
                .OrderBy(p => p.Patios)
                .Skip(offSet)
                .Take(take)
                .ToListAsync();

            return new PageResultModel<IEnumerable<FilialEntity>>
            {
                Data = result,
                Offset = offSet,
                Take = take,
                Total = totalRegistros
            };
        }

        public async Task<FilialEntity?> GetFilialByIdAsync(int id)
        {
            var result = await _context.Filial
                .Include(p => p.Patios)
                .Include(u => u.Usuarios)
                .FirstOrDefaultAsync(f => f.Id == id);

            return result;
        }

        public async Task<FilialEntity?> UpdateFilialAsync(int id, FilialEntity filial)
        {
            var result = await GetFilialByIdAsync(id);

            if (result is not null)
            {
                result.Nome = filial.Nome;
                result.PaisFilial = filial.PaisFilial;
                result.Estado = filial.Estado;
                result.Endereco = filial.Endereco;
                
                _context.Filial.Update(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }
    }
}
