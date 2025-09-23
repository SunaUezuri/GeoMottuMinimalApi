# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Otimização de Cache: Copia os arquivos de projeto primeiro.
COPY ["GeoMottuMinimalApi.sln", "."]
COPY ["GeoMottuMinimalApi/GeoMottuMinimalApi.csproj", "GeoMottuMinimalApi/"]
RUN dotnet restore "GeoMottuMinimalApi.sln"

# Agora, copia o restante do código-fonte.
COPY . .
WORKDIR "/source/GeoMottuMinimalApi"

# Publica a aplicação em modo Release.
RUN dotnet publish "GeoMottuMinimalApi.csproj" -c Release -o /app/publish --no-restore

# ---

# Estágio 2: Imagem Final (Execução)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia apenas os arquivos publicados do estágio de build.
COPY --from=build /app/publish .

# Cria um usuário de sistema dedicado para a aplicação.
RUN useradd -m -s /bin/bash appuser

# Muda a propriedade de todos os arquivos da aplicação para o novo usuário.
RUN chown -R appuser:appuser /app

# Muda o contexto de execução para o novo usuário.
USER appuser

# Expõe a porta que a aplicação vai usar internamente no container.
EXPOSE 8080

# Configura o Kestrel (servidor do .NET) para ouvir na porta exposta.
ENV ASPNETCORE_URLS=http://+:8080

# Define o comando de entrada que será executado quando o container iniciar.
ENTRYPOINT ["dotnet", "GeoMottuMinimalApi.dll"]
