FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
COPY . .

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.ProductService.dll"]