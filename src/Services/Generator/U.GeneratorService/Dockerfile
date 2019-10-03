FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 5700/tcp
ENV ASPNETCORE_URLS http://*:5700

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

COPY src/BuildingBlocks/Common/*.csproj /src/csproj-files/
COPY src/Services/Generator/*/*.csproj /src/csproj-files/

COPY . .

WORKDIR /src/src/Services/Generator/U.GeneratorService
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "U.GeneratorService.dll"]
