FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /source
ARG PROJECT_NAME='U.SmartStoreAdapter'

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY $PROJECT_NAME.Application/*.csproj ./$PROJECT_NAME.Application
COPY $PROJECT_NAME.Domain/*.csproj ./$PROJECT_NAME.Domain
COPY $PROJECT_NAME.Persistance/*.csproj ./$PROJECT_NAME.Persistance
COPY . .
RUN dotnet restore "./$PROJECT_NAME.csproj" /property:Configuration=Release --configfile nuget.develop.config

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