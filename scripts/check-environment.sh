#!/bin/bash

if [[ -z "$MONITORING_POSTGRES_CONNECTION_STRING" ]]; then
    echo -e "\e[1;31mERROR - 'MONITORING_POSTGRES_CONNECTION_STRING' has not been provided \e[0m"
    exit 1
fi
