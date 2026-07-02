# Capa 1: Runtime de ejecución optimizado para .NET 10
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

# Capa 2: SDK de .NET 10 para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# COPIAR PROYECTOS (Mantiene la caché de Docker si no cambian las dependencias)
COPY ["apps/Notification.Worker/Notification.Worker.csproj", "apps/Notification.Worker/"]
COPY ["shared/TicketFlow.Shared/TicketFlow.Shared.csproj", "shared/TicketFlow.Shared/"]
COPY ["src/Notification.Infrastructure/Notification.Infrastructure.csproj", "src/Notification.Infrastructure/"]

# SI TU WORKER SE COMUNICA CON EL DOMINIO DE BOOKING, DESCOMENTA ESTA LÍNEA:
# COPY ["src/Booking.Domain/Booking.Domain.csproj", "src/Booking.Domain/"]

# Restaurar los paquetes NuGet de la solución
RUN dotnet restore "apps/Notification.Worker/Notification.Worker.csproj"

# Copiar absolutamente todo el código fuente restante de la solución
COPY . .

# Compilar el proyecto en modo Release
WORKDIR "/src/apps/Notification.Worker"
RUN dotnet build "Notification.Worker.csproj" -c Release -o /app/build

# Capa 3: Publicación y optimización de binarios
FROM build AS publish
RUN dotnet publish "Notification.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Capa 4: Imagen final ligera con el código compilado
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.Worker.dll"]