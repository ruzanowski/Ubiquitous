#!/bin/sh
case "$CI_COMMIT_REF_NAME" in
  "master")
  dotnet restore -s https://www.myget.org/F/ubiquitous
    ;;
  * | "develop")
  dotnet restore -s https://www.myget.org/F/ubiquitous-develop
    ;;
esac
