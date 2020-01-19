FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /app
COPY ./src/BuildingBlocks/Common/bin/docker .
ENTRYPOINT ["dotnet", "U.Common.dll"]
