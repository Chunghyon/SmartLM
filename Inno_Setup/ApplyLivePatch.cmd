@echo off
taskkill /F /IM Card3500.exe >nul 2>nul
copy /Y "D:\Documents\Smart_LM_China\Inno_Setup\Card3500.exe" "C:\Program Files (x86)\Access Control System\Card3500.exe"
copy /Y "D:\Documents\Smart_LM_China\Inno_Setup\Korean.XML" "C:\Program Files (x86)\Access Control System\Korean.XML"
start "" "C:\Program Files (x86)\Access Control System\Card3500.exe"
echo Done
pause
