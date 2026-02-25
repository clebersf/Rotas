@echo off
setlocal enabledelayedexpansion

 rem === Ajuste o caminho do executável (relativo ou absoluto) ===
set EXE_PATH=%~dp0Vale.Tops.Integration.Infrastructure.OpcClientService.exe

 rem === Lê Service.Name e Service.Description do App.config via PowerShell ===
for /f "usebackq tokens=*" %%i in (`powershell -NoLogo -NoProfile -Command ^
  "$cfg=[xml](Get-Content '%EXE_PATH%.config');" ^
  "$name=$cfg.configuration.appSettings.add | ? { $_.key -eq 'Service.Name' } | select -expand value;" ^
  "$desc=$cfg.configuration.appSettings.add | ? { $_.key -eq 'Service.Description' } | select -expand value;" ^
  "Write-Output ($name+'|'+$desc)"`) do (
  set LINE=%%i
)

for /f "tokens=1,2 delims=|" %%a in ("%LINE%") do (
  set SVC_NAME=%%~a
  set SVC_DESC=%%~b
)

if "%SVC_NAME%"=="" (
  echo [ERRO] Nao foi possivel ler 'Service.Name' do App.config.
  exit /b 1
)

 rem === Cria o servico. Por padrao roda como LocalSystem e Start=auto ===
sc create "%SVC_NAME%" binPath= "\"%EXE_PATH%\"" start= auto DisplayName= "%SVC_NAME%"
if errorlevel 1 (
  echo [ERRO] Falha ao criar o servico. Verifique permissao de administrador.
  exit /b 1
)

 rem === Aplica a descricao do servico (aparece no Services.msc) ===
if not "%SVC_DESC%"=="" (
  sc description "%SVC_NAME%" "%SVC_DESC%"
)

 rem === Inicia o servico ===
sc start "%SVC_NAME%"

echo [OK] Servico '%SVC_NAME%' instalado e iniciado.
exit /b 0