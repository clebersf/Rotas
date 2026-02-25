@echo off
setlocal enableextensions

REM ===== Caminhos padrao (edite conforme seu ambiente) =====
set "SRC=C:\Dados\Rotas_081025\Rotas_081025\Vale.Tops.Integration.OpcDaDriver.Service\bin\x86\Debug\net472"
set "DST=C:\Program Files\Vale"

if "%~3"=="" (
  echo Uso: Install-Instance.cmd ServiceName Description PLC
  echo Ex.: Install-Instance.cmd PLC1 "Driver PLC 1" 1
  exit /b 1
)

set "SVC=%~1"
set "DESC=%~2"
set "PLC=%~3"

powershell -ExecutionPolicy Bypass -File "%~dp0Install-OpcDaDriverInstance.ps1" ^
  -SourceBuildDir "%SRC%" -InstanceRoot "%DST%" -ServiceName "%SVC%" -ServiceDescription "%DESC%" -PlcId %PLC% -StartAfterInstall

if errorlevel 1 (
  echo [ERRO] Instalacao falhou.
  exit /b 1
) else (
  echo [OK] Instalacao concluida.
)

endlocal & exit /b 0
