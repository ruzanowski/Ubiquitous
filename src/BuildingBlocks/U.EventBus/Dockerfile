FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /app
COPY ./src/BuildingBlocks/U.EventBus/bin/docker .
ENTRYPOINT ["dotnet", "U.EventBus.dll"]
