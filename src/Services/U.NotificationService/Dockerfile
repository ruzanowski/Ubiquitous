FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /source
ARG NUGET_CONFIG_FILE

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY U.NotificationService.Application/*.csproj ./U.NotificationService.Application
COPY U.NotificationService.Domain/*.csproj ./U.NotificationService.Domain
COPY U.NotificationService.Infrastructure/*.csproj ./U.NotificationService.Infrastructure
COPY U.NotificationService.PeriodicSender/*.csproj ./U.NotificationService.PeriodicSender
COPY . .
RUN dotnet restore "./U.NotificationService.csproj" /property:Configuration=Release --configfile "${NUGET_CONFIG_FILE}"

# copy everything else and build app
WORKDIR /source
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app ./

# environment variables
ENV ASPNETCORE_URLS http://*:5500
ENTRYPOINT ["dotnet", "U.NotificationService.dll"]