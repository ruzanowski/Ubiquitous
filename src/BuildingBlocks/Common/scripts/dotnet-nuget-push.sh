#!/bin/sh
echo Currently in
pwd
ls

dotnet pack /p:PackageVersion=${UBIQUITOUS_VERSION}.${CI_JOB_ID} --no-restore -o .

echo Uploading package to MyGet using branch $CI_COMMIT_REF_NAME
echo Currently in
pwd
ls

case "$CI_COMMIT_REF_NAME" in
  "master")
    dotnet nuget push ${UBIQUITOUS_VERSION}.${CI_JOB_ID}.nupkg -k ${MYGET_API_KEY} -source "https://www.myget.org/F/ubiquitous/api/v2/package"
    ;;
  * | "develop")
    dotnet nuget push ${UBIQUITOUS_VERSION}.${CI_JOB_ID}.nupkg -k ${MYGET_API_KEY} -source "https://www.myget.org/F/ubiquitous-develop/api/v2/package"
    ;;
esac