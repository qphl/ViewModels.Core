#!/bin/bash
set -e
version="0.0.0"
if [ -n "$1" ]; then version="$1"
fi

dotnet test src/ViewModels.Core.Tests/ViewModels.Core.Tests.csproj

dotnet pack src/ViewModels.Core/ViewModels.Core.csproj -o ../../dist -p:Version="$version" -p:PackageVersion="$version" -c Release