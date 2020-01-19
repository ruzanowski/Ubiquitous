FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
RUN apk add --no-cache bash

WORKDIR /app
COPY ./src/BuildingBlocks/U.EventBus.RabbitMQ/bin/docker .
ENTRYPOINT ["dotnet", "U.EventBus.RabbitMQ.dll"]
