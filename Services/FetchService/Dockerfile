FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY Common/*.csproj ./U.Common/
COPY Adapters/Adapter.SmartStore/U.SmartStoreAdapter.Api/*.csproj ./SmartStoreAdapter.Api/
COPY Services/FetchService/*.csproj ./FetchService/
COPY Services/FetchService.Application/*.csproj ./FetchService.Application/
COPY Services/FetchService.Persistance/*.csproj ./FetchService.Persistance/
COPY Services/FetchService.Domain/*.csproj ./FetchService.Domain/
RUN cd U.Common && dotnet restore
RUN cd SmartStoreAdapter.Api && dotnet restore
RUN cd FetchService && dotnet restore

COPY Common ./U.Common/
COPY Adapters/Adapter.SmartStore/U.SmartStoreAdapter.Api ./SmartStoreAdapter.Api/
COPY . .
RUN cd U.Common && dotnet build
RUN cd SmartStore.Api && dotnet build
RUN cd FetchService && dotnet build

# Copy everything else and build
FROM build AS publish
WORKDIR /src/FetchService
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=publish /src/FetchService/out .
ENTRYPOINT ["dotnet", "U.FetchService.dll"]