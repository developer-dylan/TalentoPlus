# Imagen base de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagen de build con certificados instalados
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# FIX: Instalar certificados para permitir HTTPS en NuGet (soluciona NU1301)
RUN apt-get update && apt-get install -y --no-install-recommends ca-certificates && update-ca-certificates

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos del proyecto
COPY ["TalentoPlus.Web/TalentoPlus.Web.csproj", "TalentoPlus.Web/"]
COPY ["TalentoPlus.Data/TalentoPlus.Data.csproj", "TalentoPlus.Data/"]

# Restaurar dependencias (funcionará correctamente ahora)
RUN dotnet restore "TalentoPlus.Web/TalentoPlus.Web.csproj"

# Copiar todo el código fuente
COPY . .

# Cambiar a la carpeta del proyecto Web
WORKDIR "/src/TalentoPlus.Web"

# Compilar en modo Release
RUN dotnet build "./TalentoPlus.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TalentoPlus.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalentoPlus.Web.dll"]
