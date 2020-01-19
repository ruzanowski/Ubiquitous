#!/bin/bash
cd src/BuildingBlocks/U.IntegrationEventLog
dotnet pack /p:PackageVersion=${UBIQUITOUS_VERSION}.$CI_JOB_ID --no-restore -o .

echo Uploading package to MyGet using branch $CI_COMMIT_REF_NAME

case "$TRAVIS_BRANCH" in
  "master")
    dotnet nuget push *.nupkg -k $MYGET_API_KEY -s https://www.myget.org/F/ubiquitous/api/v2/package
    ;;
  "develop")
    dotnet nuget push *.nupkg -k $MYGET_API_KEY -s https://www.myget.org/F/ubiquitous-develop/api/v2/package
    ;;
esac