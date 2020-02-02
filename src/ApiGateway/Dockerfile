FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
EXPOSE 4500/tcp
COPY . .

ENV ASPNETCORE_URLS http://*:4500
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.ApiGateway.dll"]
