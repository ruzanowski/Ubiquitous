FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

COPY scripts scripts/

COPY BuildingBlocks/Common/*.csproj /src/csproj-files/
COPY BuildingBlocks/U.EventBus.RabbitMQ/*.csproj /src/csproj-files/
COPY BuildingBlocks/U.IntegrationEventLog/*.csproj /src/csproj-files/
COPY Services/Product/*/*.csproj /src/csproj-files/

COPY . .
WORKDIR /src/Services/Product/U.ProductService
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "U.ProductService.dll"]
