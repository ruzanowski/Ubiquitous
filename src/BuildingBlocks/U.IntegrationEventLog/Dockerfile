FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /app
COPY ./src/BuildingBlocks/U.IntegrationEventLog/bin/docker .
ENTRYPOINT ["dotnet", "U.IntegrationEventLog.dll"]
