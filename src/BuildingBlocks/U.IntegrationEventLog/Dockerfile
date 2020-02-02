FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
RUN apk add --no-cache bash

WORKDIR /app
COPY ./bin/docker .
ENTRYPOINT ["dotnet", "U.IntegrationEventLog.dll"]
