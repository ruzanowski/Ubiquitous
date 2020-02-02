FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
COPY ./bin/docker .
ENTRYPOINT ["dotnet", "U.EventBus.RabbitMQ.dll"]
