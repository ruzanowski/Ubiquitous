FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
RUN apk add --no-cache bash

WORKDIR /app
COPY ./src/Services/Fetch/U.FetchService/bin/docker .
EXPOSE 5200/tcp
ENV ASPNETCORE_URLS http://*:5200
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.FetchService.dll"]
