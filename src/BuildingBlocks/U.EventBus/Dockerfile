FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
COPY . .
WORKDIR src/BuildingBlocks/U.EventBus
ENTRYPOINT ["dotnet", "U.EventBus.dll"]
