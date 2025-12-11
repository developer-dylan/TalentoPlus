# Imagen base de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagen de build con certificados instalados
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# FIX: Instalar certificados para que https://api.nuget.org funcione
RUN apt-get update && apt-get install -y --no-install-recommends ca-certificates && update-ca-certificates

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos de proyecto
COPY ["TalentoPlus.Api/TalentoPlus.Api.csproj", "TalentoPlus.Api/"]
COPY ["TalentoPlus.Data/TalentoPlus.Data.csproj", "TalentoPlus.Data/"]

# Restaurar dependencias (ahora sí funciona)
RUN dotnet restore "TalentoPlus.Api/TalentoPlus.Api.csproj"

# Copiar todo el código
COPY . .

WORKDIR "/src/TalentoPlus.Api"

# Compilar
RUN dotnet build "./TalentoPlus.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TalentoPlus.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalentoPlus.Api.dll"]
