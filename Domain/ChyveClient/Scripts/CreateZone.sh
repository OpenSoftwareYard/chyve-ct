#!/bin/bash

set -x

pfexec zadm create -b "$1" -i "$2" "$3" < $4
