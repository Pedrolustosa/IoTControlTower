#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IoTControlTower.API/IoTControlTower.API.csproj", "IoTControlTower.API/"]
COPY ["IoTControlTower.Infra.IoC/IoTControlTower.Infra.IoC.csproj", "IoTControlTower.Infra.IoC/"]
COPY ["IoTControlTower.Application/IoTControlTower.Application.csproj", "IoTControlTower.Application/"]
COPY ["IoTControlTower.Domain/IoTControlTower.Domain.csproj", "IoTControlTower.Domain/"]
COPY ["IoTControlTower.Infra/IoTControlTower.Infra.csproj", "IoTControlTower.Infra/"]
RUN dotnet restore "./IoTControlTower.API/./IoTControlTower.API.csproj"
COPY . .
WORKDIR "/src/IoTControlTower.API"
RUN dotnet build "./IoTControlTower.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IoTControlTower.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IoTControlTower.API.dll"]