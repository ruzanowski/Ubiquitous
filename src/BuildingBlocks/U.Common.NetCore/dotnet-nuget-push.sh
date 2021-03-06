#!/bin/bash
dotnet pack --no-build --no-restore -c Release /p:Version=${UBIQUITOUS_VERSION}.${CI_PIPELINE_IID} -o .

echo Uploading package to MyGet using branch $CI_COMMIT_REF_NAME

case "$CI_COMMIT_REF_NAME" in
  "master")
    dotnet nuget push *.nupkg -k $MYGET_API_KEY -s https://www.myget.org/F/ubiquitous/api/v2/package
    ;;
  * | "develop")
    dotnet nuget push *.nupkg -k $MYGET_API_KEY -s https://www.myget.org/F/ubiquitous-develop/api/v2/package
    ;;
esac