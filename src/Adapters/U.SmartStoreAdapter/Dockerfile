FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
ARG PROJECT_NAME=U.SmartStoreAdapter
ARG NUGET_CONFIG_FILE='nuget.develop.config'
ARG CONFIG_FILE_FEED='--configfile "${NUGET_CONFIG_FILE}"'
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY $PROJECT_NAME.Application/*.csproj .
COPY $PROJECT_NAME.Domain/*.csproj .
COPY $PROJECT_NAME.Persistance/*.csproj .
RUN dotnet restore "./$PROJECT_NAME.csproj" /property:Configuration=Release $CONFIG_FILE_FEED

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
ENTRYPOINT ["dotnet", "$PROJECT_NAME.dll"]