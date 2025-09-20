using GeoMottuMinimalApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoMottuMinimalApi.Infrastructure.Data.AppDatas
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MotoEntity>()
                .Property(m => m.Modelo)
                .HasConversion<string>();

            modelBuilder.Entity<PatioEntity>()
                .Property(p => p.TipoDoPatio)
                .HasConversion<string>();

            modelBuilder.Entity<FilialEntity>()
                .Property(f => f.PaisFilial)
                .HasConversion<string>();
        }

        public DbSet<MotoEntity> Moto { get; set; }
        public DbSet<UsuarioEntity> Usuario { get; set; }
        public DbSet<PatioEntity> Patio { get; set; }
        public DbSet<FilialEntity> Filial { get; set; }
    }
}
