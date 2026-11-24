# PROYECTO FINAL - TECNOLOGIA WEB I
# Dockerfile elegido para construir la imagen final publicada en Docker Hub.
# Proyecto: Circle Rush - .NET 10

# ETAPA 1: COMPILACION
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["ProyectoTecWeb.csproj", "./"]

RUN dotnet restore "ProyectoTecWeb.csproj"

COPY . .

RUN dotnet publish "ProyectoTecWeb.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false


# ETAPA 2: EJECUCION
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

# Las cuatro instancias comparten esta carpeta para validar la misma cookie.
# La imagen la crea con el usuario de la aplicacion para que el volumen pueda escribir.
RUN mkdir -p /auth-keys && chown -R $APP_UID:$APP_UID /auth-keys

ENV ASPNETCORE_ENVIRONMENT=Production
ENV PORT=8080

EXPOSE 8080

USER $APP_UID

ENTRYPOINT ["dotnet", "ProyectoTecWeb.dll"]
