#!/bin/bash

set -x

pfexec zadm create -b "$1" -i "$2" "$3" < $4
pfexec echo "nameserver 1.1.1.1" > $5
pfexec mv $6 $7
