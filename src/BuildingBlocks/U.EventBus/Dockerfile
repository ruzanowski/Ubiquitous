FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app
COPY ./bin/docker .
ENTRYPOINT ["dotnet", "U.EventBus.dll"]
