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

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Booking.API.dll"]
