FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
RUN apk add --no-cache bash

WORKDIR /app
COPY . .

EXPOSE 5700/tcp
ENV ASPNETCORE_URLS http://*:5700
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.GeneratorService.dll"]
