#!/bin/bash

set -x

pfexec zadm create -b "$1" "$2" < $3
