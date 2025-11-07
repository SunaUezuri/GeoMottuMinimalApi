using GeoMottuMinimalApi.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace GeoMottuMinimalApi.Tests.Presentation.Handlers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IFilialUseCase> FilialUseCaseMock { get; }
        public Mock<IUsuarioUseCase> UsuarioUseCaseMock { get; }
        public Mock<IPatioUseCase> PatioUseCaseMock { get; }
        public Mock<IMotoUseCase> MotoUseCaseMock { get; }
        public CustomWebApplicationFactory()
        {
            FilialUseCaseMock = new Mock<IFilialUseCase>();
            UsuarioUseCaseMock = new Mock<IUsuarioUseCase>();
            PatioUseCaseMock = new Mock<IPatioUseCase>();
            MotoUseCaseMock = new Mock<IMotoUseCase>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IFilialUseCase));
                services.RemoveAll(typeof(IUsuarioUseCase));
                services.RemoveAll(typeof(IPatioUseCase));
                services.RemoveAll(typeof(IMotoUseCase));

                services.AddSingleton(FilialUseCaseMock.Object);
                services.AddSingleton(UsuarioUseCaseMock.Object);
                services.AddSingleton(PatioUseCaseMock.Object);
                services.AddSingleton(MotoUseCaseMock.Object);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.Scheme;
                    options.DefaultChallengeScheme = TestAuthHandler.Scheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.Scheme, _ => { });
            });
        }
    }
}
