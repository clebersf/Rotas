@echo off
setlocal enabledelayedexpansion

rem === Caminho do executável do serviço ===
set EXE_PATH=%~dp0SeuServico.exe

rem === Lê Service.Name do App.config via PowerShell ===
for /f "usebackq tokens=*" %%i in (`powershell -NoLogo -NoProfile -Command ^
  "$cfg=Get-Content '%EXE_PATH%.config';" ^
  "$name=$cfg.configuration.appSettings.add | ? { $_.key -eq 'Service.Name' } | select -expand value;" ^
  "Write-Output $name"`) do (
  set SVC_NAME=%%i
)

if "%SVC_NAME%"=="" (
  echo [ERRO] Nao foi possivel ler 'Service.Name' do App.config.
  exit /b 1
)

echo [INFO] Desinstalando servico "%SVC_NAME%"...

rem === Para o servico (se estiver rodando) ===
sc query "%SVC_NAME%" >nul 2>&1
if errorlevel 1 (
  echo [INFO] Servico nao encontrado. Nada a remover.
  exit /b 0
)

sc stop "%SVC_NAME%" >nul 2>&1

rem === Aguarda o servico parar (loop simples) ===
for /l %%x in (1,1,20) do (
  sc query "%SVC_NAME%" | find "STOPPED" >nul
  if not errorlevel 1 goto :DELETE
  timeout /t 1 >nul
)

:DELETE
rem === Remove o servico do SCM ===
sc delete "%SVC_NAME%"
if errorlevel 1 (
  echo [ERRO] Falha ao remover o servico.
  exit /b 1
)

echo [OK] Servico "%SVC_NAME%" removido com sucesso.
exit /b 0