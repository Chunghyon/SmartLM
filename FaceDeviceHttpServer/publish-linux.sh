#!/bin/bash
set -e
cd "$(dirname "$0")"
dotnet publish FaceDeviceHttpServer.Linux.csproj -c Release -f net9.0 -r linux-x64 --self-contained true -o ../dist/fdhs-linux
echo "Published to ../dist/fdhs-linux"
