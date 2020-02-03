FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
EXPOSE 5100/tcp
COPY ./bin/docker .

ENV ASPNETCORE_URLS http://*:5100
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]
