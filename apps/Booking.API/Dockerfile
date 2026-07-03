# syntax=docker/dockerfile:1
# Este Dockerfile espera ejecutarse con el contexto en la raiz del repo,
# tal como lo hace el docker-compose.yml ubicado alli:
#   docker build -f apps/Booking.API/Dockerfile .

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore apps/Booking.API/Booking.API.csproj
RUN dotnet publish apps/Booking.API/Booking.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# curl se usa solo para el HEALTHCHECK de abajo (evidencia de que la API responde)
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

HEALTHCHECK --interval=30s --timeout=5s --start-period=15s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Booking.API.dll"]
