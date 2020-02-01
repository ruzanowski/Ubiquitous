FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
RUN apk add --no-cache bash

WORKDIR /app
EXPOSE 4500/tcp
COPY . .

ENV ASPNETCORE_URLS http://*:4500
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.ApiGateway.dll"]
