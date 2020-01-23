FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
RUN apk add --no-cache bash

WORKDIR /app
COPY ./bin/docker .
ENTRYPOINT ["dotnet", "U.IntegrationEventLog.dll"]
