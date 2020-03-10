#!/bin/bash
TAG=''
BUILD_NO=

case "$GIT_BRANCH" in
  "dev")
    TAG=dev
    BUILD_NO=$TRAVIS_BUILD_NUMBER
    ;;  
esac

echo $DOCKER_REPOSITORY:$TAG
echo $DOCKER_REPOSITORY:$BUILD_NO
echo $GIT_BRANCH

