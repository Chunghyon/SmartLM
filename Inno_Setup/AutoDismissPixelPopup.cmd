@echo off
start "" /min powershell -ExecutionPolicy Bypass -WindowStyle Hidden -File "%~dp0AutoDismissPixelPopup.ps1"
exit /b 0
