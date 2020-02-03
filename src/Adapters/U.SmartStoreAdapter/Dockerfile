# Define base image
FROM microsoft/dotnet:2.2-sdk AS build-env
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

# Copy project files
WORKDIR /source
COPY ["U.SmartStoreAdapter.csproj", "./U.SmartStoreAdapter.csproj"]

# Restore
ARG NUGET_BRANCH_RESTORE
RUN if [ "$CI_COMMIT_REF_NAME" = "master" ] ; then echo Branch is $CI_COMMIT_REF_NAME ; else $NUGET_BRANCH_RESTORE=.develop ; fi
RUN dotnet restore "./U.SmartStoreAdapter.csproj" --configfile nuget"${NUGET_BRANCH_RESTORE}".config

# Copy all source code
COPY . .

# Publish
WORKDIR /source/src
RUN dotnet publish -c Release -o /publish

# Runtime
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /publish
COPY --from=build-env /publish .
ENTRYPOINT ["dotnet", "U.SmartStoreAdapter.dll"]

