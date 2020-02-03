FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
COPY . .

EXPOSE 5500/tcp
ENV ASPNETCORE_URLS http://*:5500
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.NotificationService.dll"]
