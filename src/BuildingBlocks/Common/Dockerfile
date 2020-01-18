FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
COPY . .
WORKDIR src/BuildingBlocks/Common

RUN docker build -t registry.gitlab.com/ruzanowski/ubiquitous .

ENTRYPOINT ["dotnet", "U.Common.dll"]
