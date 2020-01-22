#!/bin/sh
case "$CI_COMMIT_REF_NAME" in
  "master")
  dotnet restore -s https://www.myget.org/F/ubiquitous -s https://api.nuget.org/v3/index.json
    ;;
  * | "develop")
  dotnet restore -s https://www.myget.org/F/ubiquitous-develop -s https://api.nuget.org/v3/index.json
    ;;
esac