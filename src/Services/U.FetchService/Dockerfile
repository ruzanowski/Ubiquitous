FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
COPY . .
EXPOSE 5200/tcp
ENV ASPNETCORE_URLS http://*:5200
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.FetchService.dll"]
