#!/bin/bash

env="$1"

# switch to docker compose folder
cd /docker/$env

# pull docker image
docker-compose -f docker-compose.yml -f docker-compose.$env.yml -p $env pull botservice

# up container 
docker-compose -f docker-compose.yml -f docker-compose.$env.yml -p $env up -d --no-deps --build botservice

#remove dangling images (image name should be the same as docker-compose.yml without tag)
docker rmi -f $(docker images umon-build.ugsdev.com:5000/tgpbot -f dangling=true -q)