using GeoMottuMinimalApi.Application.Interfaces;
using GeoMottuMinimalApi.Application.UseCases;
using GeoMottuMinimalApi.Domain.Interfaces;
using GeoMottuMinimalApi.Infrastructure.Data.AppDatas;
using GeoMottuMinimalApi.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
});

// Repositórios da aplicação
builder.Services.AddTransient<IMotoRepository, MotoRepository>();
builder.Services.AddTransient<IPatioRepository, PatioRepository>();
builder.Services.AddTransient<IFilialRepository, FilialRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

// UseCases da aplicação
builder.Services.AddTransient<IMotoUseCase, MotoUseCase>();
builder.Services.AddTransient<IPatioUseCase, PatioUseCase>();
builder.Services.AddTransient<IFilialUseCase, FilialUseCase>();
builder.Services.AddTransient<IUsuarioUseCase, UsuarioUseCase>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.ExampleFilters();
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.AddRateLimiter(options => {
    options.AddFixedWindowLimiter(policyName: "ratelimit", opt => {
        opt.PermitLimit = 20;
        opt.Window = TimeSpan.FromSeconds(60);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddResponseCompression(options => {
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});



var app = builder.Build();

ApplyMigrations(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();
app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ApplyMigrations(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        // Verifica se existem migrations pendentes e as aplica
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("Aplicando migrations pendentes...");
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations aplicadas com sucesso.");
        }
        else
        {
            Console.WriteLine("Banco de dados já está atualizado.");
        }
    }
}
