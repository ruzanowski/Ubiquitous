FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["Common/", "Common/"]
COPY ["Adapters/Adapter.SmartStore/", "Adapters/Adapter.SmartStore/"]
RUN cd Adapters/Adapter.SmartStore/U.SmartStoreAdapter/ && dotnet restore

# Copy everything else and build
FROM build AS publish
WORKDIR /src/SmartStoreAdapter
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=publish /src/SmartStoreAdapter/out .
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]