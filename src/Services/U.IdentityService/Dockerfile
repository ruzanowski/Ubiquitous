FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /source
ARG NUGET_CONFIG_FILE

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY U.IdentityService.Application/*.csproj ./U.IdentityService.Application
COPY U.IdentityService.Domain/*.csproj ./U.IdentityService.Domain
COPY U.IdentityService.Persistance/*.csproj ./U.IdentityService.Persistance
COPY . .
RUN dotnet restore "./U.IdentityService.csproj" /property:Configuration=Release --configfile "${NUGET_CONFIG_FILE}"

# copy everything else and build app
WORKDIR /source
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build /app ./

# environment variables
ENV ASPNETCORE_URLS http://*:5400
ENTRYPOINT ["dotnet", "U.IdentityService.dll"]