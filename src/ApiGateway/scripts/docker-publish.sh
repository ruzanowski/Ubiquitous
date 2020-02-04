#!/bin/sh
DOCKER_TAG=''
NUGET_CONFIG_FILE=''

case "$CI_COMMIT_REF_NAME" in
  "master")
    DOCKER_TAG=latest
    NUGET_CONFIG_FILE=nuget.config

    ;;
  * | "develop")
    DOCKER_TAG=develop
    NUGET_CONFIG_FILE=nuget.develop.config
    ;;
esac

docker info
apk update
apk upgrade
apk add python python-dev py-pip build-base libffi-dev openssl-dev
pip install docker-compose
docker login -u $CI_REGISTRY_USER -p "$CI_REGISTRY_PASSWORD" $CI_REGISTRY
docker build --build-arg NUGET_CONFIG_FILE=${NUGET_CONFIG_FILE} --build-arg PROJECT_NAME=U.SmartStoreAdapter --build-arg PORT=5000 -t registry.gitlab.com/ruzanowski/ubiquitous/apigateway:$DOCKER_TAG .
docker push registry.gitlab.com/ruzanowski/ubiquitous/apigateway:$DOCKER_TAG