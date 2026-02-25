@echo off
setlocal

set EXE=%~dp0Vale.Tops.Integration.OpcDaDriver.Service.exe

set INSTALLUTIL64=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe
set INSTALLUTIL32=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe

set INSTALLUTIL=
if exist "%INSTALLUTIL64%" set INSTALLUTIL="%INSTALLUTIL64%"
if "%INSTALLUTIL%"=="" if exist "%INSTALLUTIL32%" set INSTALLUTIL="%INSTALLUTIL32%"

if "%INSTALLUTIL%"=="" (
  echo ERRO: InstallUtil.exe nao encontrado no .NET Framework 4.x
  exit /b 1
)

echo Desinstalando usando %INSTALLUTIL% ...
%INSTALLUTIL% /u /ShowCallStack "%EXE%"
set RC=%ERRORLEVEL%
if not "%RC%"=="0" (
  echo Falha ao desinstalar (InstallUtil rc=%RC%)
  exit /b %RC%
)

echo [OK] Desinstalado com sucesso.
exit /b 0