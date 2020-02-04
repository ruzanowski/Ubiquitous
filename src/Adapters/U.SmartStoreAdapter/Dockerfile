FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app
COPY . .
RUN dotnet restore /property:Configuration=Release --configfile nuget.develop.config
RUN dotnet publish -c Release -o /publish

# Restore
#ARG NUGET_BRANCH_RESTORE
#RUN if [ "$CI_COMMIT_REF_NAME" = "master" ] ; then echo Branch is $CI_COMMIT_REF_NAME ; else $NUGET_BRANCH_RESTORE=.develop ; fi
#echo $NUGET_BRANCH_RESTORE

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /publish .
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]

