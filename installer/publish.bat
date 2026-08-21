@echo off
setlocal
cd /d "%~dp0"

echo [1/2] Publish FDHS...
dotnet publish ..\FaceDeviceHttpServer\FaceDeviceHttpServer.csproj -c Release -r win-x64 --self-contained true -o dist\FDHS
if errorlevel 1 goto :fail
if exist dist\FDHS\App_Data rmdir /s /q dist\FDHS\App_Data

echo [2/2] Publish FDDC...
dotnet publish ..\FaceDeviceDesktopClient\FaceDeviceDesktopClient.csproj -c Release -r win-x64 --self-contained true -o dist\FDDC
if errorlevel 1 goto :fail

echo OK. Next: compile SmartLM.iss with Inno Setup.
timeout /t 3
exit /b 0

:fail
echo PUBLISH FAILED
pause
exit /b 1
