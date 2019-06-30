FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY Common/*.csproj ./Common/
COPY Adapters/Adapter.SmartStore/U.SmartStoreAdapter.Api/*.csproj ./SmartStoreAdapter.Api/
COPY Adapters/Adapter.SmartStore/U.SmartStoreAdapter/*.csproj ./SmartStoreAdapter/
RUN cd SmartStoreAdapter && dotnet restore

COPY Common ./Common/
COPY Adapters/Adapter.SmartStore/U.SmartStoreAdapter.Api ./SmartStoreAdapter.Api/
COPY Adapters/Adapter.SmartStore/U.SmartStoreAdapter ./SmartStoreAdapter/
RUN cd SmartStoreAdapter && dotnet build

# Copy everything else and build
FROM build AS publish
WORKDIR /src/SmartStoreAdapter
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=publish /src/SmartStoreAdapter/out .
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]