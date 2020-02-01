FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
RUN apk add --no-cache bash

WORKDIR /app
COPY . .

EXPOSE 5700/tcp
ENV ASPNETCORE_URLS http://*:5700
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.GeneratorService.dll"]
