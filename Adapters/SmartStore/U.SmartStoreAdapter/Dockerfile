#FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
#WORKDIR /src
#
## Copy csproj and restore as distinct layers
#COPY ["Common/", "Common/"]
#COPY ["Adapters/Adapter.SmartStore/", "Adapters/Adapter.SmartStore/"]
#RUN cd Adapters/Adapter.SmartStore/U.SmartStoreAdapter/ && dotnet restore
#
## Copy everything else and build
#FROM build AS publish
#WORKDIR /src/SmartStoreAdapter
#RUN dotnet publish -c Release -o out
#
## Build runtime image
#FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
#WORKDIR /app
#COPY --from=publish /src/SmartStoreAdapter/out .
#ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]


#---------------------------------------------------------------------

#      <ProjectReference Include="..\U.SmartStoreAdapter.Api\U.SmartStoreAdapter.Api.csproj" />
#      <ProjectReference Include="..\U.SmartStoreAdapter.Application\U.SmartStoreAdapter.Application.csproj" />


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app

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
