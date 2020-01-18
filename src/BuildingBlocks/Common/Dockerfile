FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
COPY . .
WORKDIR src/BuildingBlocks/Common
ENTRYPOINT ["dotnet", "U.Common.dll"]
