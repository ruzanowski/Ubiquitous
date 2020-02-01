FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
RUN apk add --no-cache bash

WORKDIR /app
COPY . .

EXPOSE 5300/tcp
ENV ASPNETCORE_URLS http://*:5300
ENV ASPNETCORE_ENVIRONMENT Docker

ENTRYPOINT ["dotnet", "U.SubscriptionService.dll"]