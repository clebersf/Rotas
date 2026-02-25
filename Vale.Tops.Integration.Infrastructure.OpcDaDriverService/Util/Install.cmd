@echo off
setlocal

REM === Ajuste o nome do EXE se for diferente ===
set EXE=%~dp0Vale.Tops.Integration.OpcDaDriver.Service.exe

REM Caminhos padrao do InstallUtil (x64 e x86) - .NET Framework 4.x
set INSTALLUTIL64=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe
set INSTALLUTIL32=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe

if not exist "%EXE%" (
  echo ERRO: executavel nao encontrado: "%EXE%"
  exit /b 1
)

set INSTALLUTIL=
if exist "%INSTALLUTIL64%" set INSTALLUTIL="%INSTALLUTIL64%"
if "%INSTALLUTIL%"=="" if exist "%INSTALLUTIL32%" set INSTALLUTIL="%INSTALLUTIL32%"

if "%INSTALLUTIL%"=="" (
  echo ERRO: InstallUtil.exe nao encontrado no .NET Framework 4.x
  echo  - Procure em C:\Windows\Microsoft.NET\Framework*\v4.0.30319\
  echo  - Instale o .NET Framework Developer Pack OU use a Opção B (PowerShell)
  exit /b 1
)

echo Instalando usando %INSTALLUTIL% ...
%INSTALLUTIL% /ShowCallStack "%EXE%"
set RC=%ERRORLEVEL%
if not "%RC%"=="0" (
  echo Falha ao instalar (InstallUtil rc=%RC%)
  exit /b %RC%
)

echo [OK] Instalado com sucesso.
echo Iniciando o servico...
sc start "PLACEHOLDER" >nul 2>&1
REM Observacao: o ProjectInstaller define o nome do servico conforme o App.config.
REM Para iniciar, use 'services.msc' ou 'sc start NOME_DO_SERVICO' (ver abaixo).
exit /b 0
``