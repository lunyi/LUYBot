#!/bin/bash

tag="$1"

if [[ "$(docker images -q $tag 2> /dev/null)" != "" ]]; then
    docker image rm $tag
fi

docker build -f TGPBot/Dockerfile -t $tag --rm --no-cache TGPBot/.
