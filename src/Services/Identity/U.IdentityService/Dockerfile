FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
COPY . .

EXPOSE 5400/tcp
ENV ASPNETCORE_URLS http://*:5400
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.IdentityService.dll"]
