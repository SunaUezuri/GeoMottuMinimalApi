using GeoMottuMinimalApi.Domain.Entities;
using GeoMottuMinimalApi.Domain.Enums;
using GeoMottuMinimalApi.Domain.Interfaces;
using GeoMottuMinimalApi.Infrastructure.Data.AppDatas;
using Microsoft.EntityFrameworkCore;

namespace GeoMottuMinimalApi.Infrastructure.Data.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        private readonly ApplicationContext _context;

        public MotoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<MotoEntity?> CreateMotoAsync(MotoEntity moto)
        {
            _context.Moto.Add(moto);
            _context.SaveChanges();

            return moto;

        }

        public async Task<MotoEntity?> DeleteMotoAsync(int id)
        {
            var result = await GetMotoByIdAsync(id);

            if (result is not null)
            {
                _context.Moto.Remove(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }

        public async Task<PageResultModel<IEnumerable<MotoEntity>>> GetAllMotosAsync(int offSet = 0, int take = 3)
        {
            var totalRegistros = await _context.Moto.CountAsync();

            var result = await _context
                .Moto
                .Include(m => m.Patio)
                .OrderBy(m => m.Id)
                .Skip(offSet)
                .Take(take)
                .ToListAsync();

            return new PageResultModel<IEnumerable<MotoEntity>>
            {
                Data = result,
                Offset = offSet,
                Take = take,
                Total = totalRegistros
            };
        }

        public async Task<MotoEntity?> GetByChassiAsync(string chassi)
        {
            return await _context.Moto
                .FirstOrDefaultAsync(m => m.Chassi.ToUpper() == chassi.ToUpper());
        }

        public async Task<PageResultModel<IEnumerable<MotoEntity>>> GetByModeloAsync(ModeloMoto modelo, int offSet = 0, int take = 3)
        {
            var query = _context.Moto.Where(m => m.Modelo == modelo);

            
            var totalRegistros = await query.CountAsync();

            var result = await query
                .Include(m => m.Patio)
                .OrderBy(m => m.Id)
                .Skip(offSet)
                .Take(take)
                .ToListAsync();

            return new PageResultModel<IEnumerable<MotoEntity>>
            {
                Data = result,
                Offset = offSet,
                Take = take,
                Total = totalRegistros
            };
        }

        public async Task<MotoEntity?> GetByPlacaAsync(string placa)
        {
            return await _context.Moto
                .FirstOrDefaultAsync(m => m.Placa != null && m.Placa.ToUpper() == placa.ToUpper());
        }

        public async Task<MotoEntity?> GetMotoByIdAsync(int id)
        {
            var result = await _context.Moto
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);

            return result;
        }

        public async Task<MotoEntity?> UpdateMotoAsync(int id, MotoEntity moto)
        {
            var result = await GetMotoByIdAsync(id);

            if (result is not null)
            {
                result.Placa = moto.Placa;
                result.Chassi = moto.Chassi;
                result.CodPlacaIot = moto.CodPlacaIot;
                result.Modelo = moto.Modelo;
                result.Motor = moto.Motor;
                result.Proprietario = moto.Proprietario;
                result.PosicaoX = moto.PosicaoX;
                result.PosicaoY = moto.PosicaoY;
                result.PatioId = moto.PatioId;

                _context.Moto.Update(result);
                _context.SaveChanges();
                return result;
            }

            return null;
        }
    }
}
