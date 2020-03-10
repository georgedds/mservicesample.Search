#!/bin/bash
TAG=''
BUILD_NO=

case "$GIT_BRANCH" in
  "dev")
    TAG=dev
    BUILD_NO=$TRAVIS_BUILD_NUMBER
    ;;  
esac

REPOSITORY= mservicesample/mservicesample.search.api

echo $REPOSITORY:$TAG
echo $REPOSITORY:$BUILD_NO
echo $GIT_BRANCH

