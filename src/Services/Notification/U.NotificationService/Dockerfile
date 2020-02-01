FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
RUN apk add --no-cache bash

WORKDIR /app
COPY . .

EXPOSE 5500/tcp
ENV ASPNETCORE_URLS http://*:5500
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.NotificationService.dll"]
