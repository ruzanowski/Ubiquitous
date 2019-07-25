FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

COPY scripts scripts/

COPY BuildingBlocks/Common/*.csproj /src/csproj-files/
COPY BuildingBlocks/U.EventBus.RabbitMQ/*.csproj /src/csproj-files/
COPY BuildingBlocks/*/*.csproj /src/csproj-files/
COPY BuildingBlocks/U.IntegrationEventLog/*.csproj /src/csproj-files/
COPY Adapters/SmartStore/*/*.csproj /src/csproj-files/

COPY . .
WORKDIR /src/Adapters/SmartStore/U.SmartStoreAdapter
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]
