FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 5500/tcp
ENV ASPNETCORE_URLS http://*:5500

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

COPY src/BuildingBlocks/Common/*.csproj /src/csproj-files/
COPY src/BuildingBlocks/U.EventBus/*.csproj /src/csproj-files/
COPY src/BuildingBlocks/U.EventBus.RabbitMQ/*.csproj /src/csproj-files/
COPY src/BuildingBlocks/U.IntegrationEventLog/*.csproj /src/csproj-files/
COPY src/Services/Notification/*.csproj /src/csproj-files/

COPY . .

WORKDIR /src/src/Services/Notification
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "U.NotificationService.dll"]
