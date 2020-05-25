FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source
ARG NUGET_CONFIG_FILE

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY U.SmartStoreAdapter.Application/*.csproj ./U.SmartStoreAdapter.Application
COPY U.SmartStoreAdapter.Domain/*.csproj ./U.SmartStoreAdapter.Domain
COPY U.SmartStoreAdapter.Persistance/*.csproj ./U.SmartStoreAdapter.Persistance
COPY . .
RUN dotnet restore "./U.SmartStoreAdapter.csproj" /property:Configuration=Release --configfile "${NUGET_CONFIG_FILE}"

# copy everything else and build app
WORKDIR /source
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./

# environment variables
ENV ASPNETCORE_URLS http://*:5100
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]