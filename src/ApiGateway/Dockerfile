FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /source
ARG NUGET_CONFIG_FILE

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY . .
RUN dotnet restore "./U.ApiGateway.csproj" /property:Configuration=Release --configfile "${NUGET_CONFIG_FILE}"

# copy everything else and build app
WORKDIR /source
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app ./

# environment variables
ENV ASPNETCORE_URLS http://*:4500
ENTRYPOINT ["dotnet", "U.ApiGateway.dll"]