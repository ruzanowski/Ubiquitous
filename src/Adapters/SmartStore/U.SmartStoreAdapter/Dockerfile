FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
RUN apk add --no-cache bash

WORKDIR /app
EXPOSE 5100/tcp
COPY ./src/Adapters/SmartStore/U.SmartStoreAdapter/bin/docker .

ENV ASPNETCORE_URLS http://*:5100
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]
