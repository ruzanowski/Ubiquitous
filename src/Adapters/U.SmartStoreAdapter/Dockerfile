FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
ARG NUGET_CONFIG_FILE
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY U.*/*.csproj .
RUN dotnet restore /property:Configuration=Release --configfile $NUGET_CONFIG_FILE

# copy everything else and build app
WORKDIR /source
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app ./

# environment variables
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]