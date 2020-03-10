#!/bin/bash
TAG='dev'
BUILD_NO=

case "$GIT_BRANCH" in
  "dev")
    TAG=dev
    BUILD_NO=$TRAVIS_BUILD_NUMBER
    ;;  
esac

REPOSITORY= mservicesample/mservicesample.search.api

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker build -t $REPOSITORY:$TAG -t $REPOSITORY:$BUILD_NO .
docker push $REPOSITORY:$TAG
docker push $REPOSITORY:$BUILD_NO