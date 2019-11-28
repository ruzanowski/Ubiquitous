FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 4500/tcp
ENV ASPNETCORE_URLS http://*:4500

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

COPY src/BuildingBlocks/Common/*.csproj /src/csproj-files/
COPY src/ApiGateway/*.csproj /src/csproj-files/

COPY . .

WORKDIR /src/src/ApiGateway
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "U.ApiGateway.dll"]
