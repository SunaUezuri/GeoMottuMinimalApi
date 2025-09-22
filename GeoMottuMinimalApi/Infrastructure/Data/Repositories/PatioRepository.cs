using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Interfaces;
using GeoMottuMinimalApi.Infrastructure.Data.AppDatas;
using Microsoft.EntityFrameworkCore;

namespace GeoMottuMinimalApi.Infrastructure.Data.Repositories
{
    public class PatioRepository : IPatioRepository
    {
        private readonly ApplicationContext _context;

        public PatioRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<PatioEntity?> CreatePatioAsync(PatioEntity patio)
        {
            _context.Patio.Add(patio);
            _context.SaveChanges();

            return patio;
        }

        public async Task<PatioEntity?> DeletePatioAsync(int id)
        {
            var result = await GetPatioByIdAsync(id);

            if (result is not null)
            {
                _context.Patio.Remove(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<PageResultModel<IEnumerable<PatioEntity>>> GetAllPatiosAsync(int offSet = 0, int take = 3)
        {
            var totalRegistros = await _context.Patio.CountAsync();

            var result = await _context
                .Patio
                .Include(f => f.Filial)
                .Include(m => m.Motos)
                .OrderBy(p => p.Id)
                .Skip(offSet)
                .Take(take)
                .ToListAsync();

            return new PageResultModel<IEnumerable<PatioEntity>>
            {
                Data = result,
                Offset = offSet,
                Take = take,
                Total = totalRegistros
            };
        }

        public async Task<int> GetCurrentMotoCountAsync(int patioId)
        {
            return await _context.Moto.CountAsync(m => m.PatioId == patioId);
        }

        public async Task<PatioEntity?> GetPatioByIdAsync(int id)
        {
            var result = await _context.Patio
                .Include(f => f.Filial)
                .Include(m => m.Motos)
                .FirstOrDefaultAsync(m => m.Id == id);

            return result;
        }

        public async Task<PatioEntity?> UpdatePatioAsync(int id, PatioEntity patio)
        {
            var result = await GetPatioByIdAsync(id);

            if (result is not null)
            {
                result.CapacidadeTotal = patio.CapacidadeTotal;
                result.LocalizacaoReferencia = patio.LocalizacaoReferencia;
                result.Tamanho = patio.Tamanho;
                result.TipoDoPatio = patio.TipoDoPatio;
                result.FilialId = patio.FilialId;

                _context.Patio.Update(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }
    }
}
