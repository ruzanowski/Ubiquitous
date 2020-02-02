FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app
COPY . .
RUN dotnet publish . -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
COPY --from=build /app/out .

EXPOSE 5300/tcp
ENV ASPNETCORE_URLS http://*:5300
ENV ASPNETCORE_ENVIRONMENT Docker

ENTRYPOINT ["dotnet", "U.SubscriptionService.dll"]