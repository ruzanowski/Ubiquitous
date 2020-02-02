FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
RUN apk add --no-cache bash

WORKDIR /app
COPY . .

EXPOSE 5300/tcp
ENV ASPNETCORE_URLS http://*:5300
ENV ASPNETCORE_ENVIRONMENT Docker

ENTRYPOINT ["dotnet", "U.SubscriptionService.dll"]