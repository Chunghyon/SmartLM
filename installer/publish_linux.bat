@echo off
setlocal
cd /d "%~dp0"

echo Publish FDHS_LINUX...
dotnet publish ..\FaceDeviceHttpServer\FaceDeviceHttpServer.Linux.csproj -c Release -r linux-x64 --self-contained true -o dist\fdhs-linux
if errorlevel 1 goto :fail

echo OK.
exit /b 0

:fail
echo PUBLISH FAILED
exit /b 1
