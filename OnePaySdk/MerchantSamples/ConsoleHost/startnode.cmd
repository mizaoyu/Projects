@echo off
set local
set INETROOT=c:\code
cd /d %~dp0
for /F %%i in ('%INETROOT%\OnePaySdk\packages\Npm.js.1.3.15.10\tools\npm root -g') do set NODE_PATH=%%i
rem %INETROOT%\OnePaySdk\packages\Npm.js.1.3.15.10\tools\npm install -g express@3.x
%INETROOT%\OnePaySdk\packages\Node.js.0.12.0\node.exe main.js
