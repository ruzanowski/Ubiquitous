FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /source
ARG PROJECT_NAME
ARG NUGET_CONFIG_FILE
ARG PORT

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY . .
RUN dotnet restore "./$PROJECT_NAME.csproj" /property:Configuration=Release --configfile "${NUGET_CONFIG_FILE}"

# copy everything else and build app
WORKDIR /source
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app ./

# environment variables
ENV ASPNETCORE_URLS http://*:${PORT}
ENTRYPOINT ["dotnet", "$PROJECT_NAME.dll"]