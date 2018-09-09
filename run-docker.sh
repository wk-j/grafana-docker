#!/bin/sh

export ROOT=$(pwd)/.working

docker-compose down
docker-compose up