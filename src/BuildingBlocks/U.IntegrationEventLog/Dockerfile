FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
COPY . .
WORKDIR src/BuildingBlocks/U.IntegrationEventLog
ENTRYPOINT ["dotnet", "U.IntegrationEventLog.dll"]
