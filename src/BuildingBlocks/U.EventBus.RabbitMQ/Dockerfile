FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
COPY . .
WORKDIR src/BuildingBlocks/U.EventBus.RabbitMQ
ENTRYPOINT ["dotnet", "U.EventBus.RabbitMQ.dll"]
