#!/bin/sh
case "$CI_COMMIT_REF_NAME" in
  "master")
  dotnet restore -s https://www.myget.org/F/ubiquitous/api/v2/package
    ;;
  * | "develop")
  dotnet restore -s https://www.myget.org/F/ubiquitous-develop/api/v2/package
    ;;
esac

dotnet build -c Release --no-cache