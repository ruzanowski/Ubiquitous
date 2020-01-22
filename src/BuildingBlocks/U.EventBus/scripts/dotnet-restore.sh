#!/bin/sh
case "$CI_COMMIT_REF_NAME" in
  "master")
  dotnet restore /property:Configuration=Release -s https://www.myget.org/F/ubiquitous/api/v3/index.json -s https://api.nuget.org/v3/index.json
    ;;
  * | "develop")
  dotnet restore /property:Configuration=Release -s https://www.myget.org/F/ubiquitous-develop/api/v3/index.json -s https://api.nuget.org/v3/index.json
    ;;
esac