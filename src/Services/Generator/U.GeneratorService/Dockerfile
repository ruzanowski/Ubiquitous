FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /app
COPY ./src/Services/Generator/U.GeneratorService/bin/docker .

EXPOSE 5700/tcp
ENV ASPNETCORE_URLS http://*:5700
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT ["dotnet", "U.GeneratorService.dll"]
