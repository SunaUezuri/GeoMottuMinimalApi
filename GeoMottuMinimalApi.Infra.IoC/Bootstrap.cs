using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Application.UseCases;
using GeoMottuMinimalApi.Domain.Interfaces;
using GeoMottuMinimalApi.Infrastructure.Data.AppDatas;
using GeoMottuMinimalApi.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeoMottuMinimalApi.Infra.IoC
{
    public class Bootstrap
    {
        public static void AddIoC(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseOracle(configuration.GetConnectionString("Oracle"));
            });

            // Repositórios da aplicação
            services.AddTransient<IMotoRepository, MotoRepository>();
            services.AddTransient<IPatioRepository, PatioRepository>();
            services.AddTransient<IFilialRepository, FilialRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();

            // UseCases da aplicação
            services.AddTransient<IMotoUseCase, MotoUseCase>();
            services.AddTransient<IPatioUseCase, PatioUseCase>();
            services.AddTransient<IFilialUseCase, FilialUseCase>();
            services.AddTransient<IUsuarioUseCase, UsuarioUseCase>();
        }
    }
}
