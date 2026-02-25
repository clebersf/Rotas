@echo off
setlocal enableextensions
setlocal enabledelayedexpansion

REM ===== Root esperado das instancias =====
set "EXPECTED_ROOT=C:\Program Files\Vale\"

if "%~1"=="" (
  echo Uso: Uninstall-Instance.cmd ServiceName [removefiles]
  exit /b 1
)

set "SVCNAME=%~1"
set "REMOVE=%~2"

set "SCRIPTDIR=%~dp0"
set "SCRIPTDIR=%SCRIPTDIR:"=%"

echo [INFO] Nome do servico: %SVCNAME%

echo [INFO] Parando servico (se estiver rodando)...
sc stop "%SVCNAME%" >nul 2>&1

REM Obter pasta do executavel via registro
set "IMGPATH="
for /f "tokens=1,2,*" %%A in ('reg query "HKLM\SYSTEM\CurrentControlSet\Services\%SVCNAME%" /v ImagePath 2^>nul ^| find /i "ImagePath"') do (
  set "IMGPATH=%%C"
)

if defined IMGPATH (
  set "IMGPATH=!IMGPATH:"=!"
  for %%P in ("!IMGPATH!") do set "INSTDIR=%%~dpP"
  echo [INFO] Pasta da instancia detectada: !INSTDIR!
) else (
  echo [WARN] Caminho do executavel nao encontrado no registro.
)

echo [INFO] Removendo servico do SCM...
sc delete "%SVCNAME%" >nul 2>&1

echo [INFO] Remocao do servico concluida.

if /i "%REMOVE%"=="removefiles" (
  if not defined INSTDIR (
    echo [WARN] Diretorio da instancia nao encontrado. Nada removido.
    goto END
  )

  set "CHK=!INSTDIR:"=!"

  if /i "!CHK!"=="%SCRIPTDIR%" (
    echo [ALERTA] INSTDIR coincide com a pasta do desinstalador. Abortando remocao.
    goto END
  )

  if defined EXPECTED_ROOT (
    set "ROOTCHK=%EXPECTED_ROOT%"
    if not "%ROOTCHK:~-1%"=="\" set "ROOTCHK=%ROOTCHK%\"
    set "TMP=!CHK:%ROOTCHK%=!"
    if /i "!TMP!"=="!CHK!" (
      echo [ALERTA] '!CHK!' nao esta sob '%EXPECTED_ROOT%'. Abortando remocao.
      goto END
    )
  )

  dir /b /a:-d "!CHK!\*.cmd" >nul 2>&1
  if not errorlevel 1 (
    echo [ALERTA] Encontrado .cmd dentro de '!CHK!'. Abortando remocao.
    goto END
  )

  echo [INFO] Removendo pasta da instancia: !CHK!
  rmdir /s /q "!CHK!"
  echo [INFO] Pasta removida.
) else (
  echo [INFO] Arquivos nao removidos (adicione 'removefiles' para remover a pasta da instancia).
)

:END
echo [OK] Desinstalacao concluida.
endlocal & endlocal & exit /b 0
