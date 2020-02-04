FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app
COPY . .
RUN dotnet restore /property:Configuration=Release --configfile nuget.develop.config
RUN dotnet publish -c Release -o /out

# Restore
#ARG NUGET_BRANCH_RESTORE
#RUN if [ "$CI_COMMIT_REF_NAME" = "master" ] ; then echo Branch is $CI_COMMIT_REF_NAME ; else $NUGET_BRANCH_RESTORE=.develop ; fi
#echo $NUGET_BRANCH_RESTORE

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]

