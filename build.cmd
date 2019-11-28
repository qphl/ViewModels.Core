@echo off

SET VERSION=0.0.0
IF NOT [%1]==[] (set VERSION=%1)

SET TAG=0.0.0
IF NOT [%2]==[] (set TAG=%2)
SET TAG=%TAG:tags/=%

dotnet test .\src\ViewModels.Core.Tests\ViewModels.Core.Tests.csproj
if %errorlevel% neq 0 exit /b %errorlevel%

dotnet pack .\src\ViewModels.Core\ViewModels.Core.csproj -o .\dist -p:Version=%version% -p:PackageVersion=%version% -p:Tag="%TAG%" -c Release
exit /b %errorlevel%