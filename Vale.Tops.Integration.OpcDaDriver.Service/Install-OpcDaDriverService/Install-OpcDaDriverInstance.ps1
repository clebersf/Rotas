param(
    [Parameter(Mandatory=$true)][string]$SourceBuildDir,
    [Parameter(Mandatory=$true)][string]$InstanceRoot,
    [Parameter(Mandatory=$true)][string]$ServiceName,
    [Parameter(Mandatory=$true)][string]$ServiceDescription,
    [Parameter(Mandatory=$true)][int]$PlcId,
    [string]$ServiceAccount = "LocalSystem",
    [string]$ServicePassword,
    [switch]$StartAfterInstall
)

$ErrorActionPreference = 'Stop'
function Write-Info($m){ Write-Host "[INFO] $m" -ForegroundColor Cyan }
function Write-Warn($m){ Write-Host "[WARN] $m" -ForegroundColor Yellow }

# 1) Validar origem
if (-not (Test-Path -LiteralPath $SourceBuildDir)) { throw "SourceBuildDir nao encontrado: $SourceBuildDir" }

# 2) Descobrir EXE do servico (pega o primeiro .exe)
$exe = Get-ChildItem -LiteralPath $SourceBuildDir -Filter *.exe -File | Select-Object -First 1
if (-not $exe) { throw "Nenhum EXE encontrado em $SourceBuildDir" }
$serviceExeName = $exe.Name

# 3) Pasta final = InstanceRoot\ServiceName
$dest = Join-Path $InstanceRoot $ServiceName
if (-not (Test-Path $dest)) { New-Item -ItemType Directory -Path $dest | Out-Null }

# 4) Copiar binarios RECURSIVAMENTE (mantem .xml, subpastas, etc.)
Write-Info "Copiando binarios recursivamente para $dest"
Copy-Item -LiteralPath (Join-Path $SourceBuildDir '*') -Destination $dest -Recurse -Force

# 5) CONFIG: copiar .exe.config do build (se existir) e ajustar somente as chaves pedidas
$srcExeConfig = Join-Path $SourceBuildDir ($serviceExeName + '.config')
$dstExeConfig = Join-Path $dest        ($serviceExeName + '.config')

if (Test-Path -LiteralPath $srcExeConfig) {
  Write-Info "Copiando config original do build..."
  Copy-Item -LiteralPath $srcExeConfig -Destination $dstExeConfig -Force
} else {
  Write-Warn "Config do build nao encontrado; criando minimo."
  @"
<?xml version='1.0' encoding='utf-8'?>
<configuration>
  <appSettings/>
  <connectionStrings/>
  <startup useLegacyV2RuntimeActivationPolicy='true'>
    <supportedRuntime version='v4.0' sku='.NETFramework,Version=v4.7.2' />
  </startup>
</configuration>
"@ | Out-File -Encoding UTF8 $dstExeConfig
}

# 6) Ajustar chaves preservando todo o resto
[xml]$cfg = Get-Content -LiteralPath $dstExeConfig
if (-not $cfg.configuration) { $cfg.AppendChild($cfg.CreateElement('configuration')) | Out-Null }
if (-not $cfg.configuration.appSettings) { $cfg.configuration.AppendChild($cfg.CreateElement('appSettings')) | Out-Null }
$app = $cfg.configuration.appSettings

function Set-App([string]$k,[string]$v){
  $n = $app.add | Where-Object { $_.key -eq $k }
  if ($n){ $n.value = $v }
  else { $n = $cfg.CreateElement('add'); $n.SetAttribute('key',$k); $n.SetAttribute('value',$v); [void]$app.AppendChild($n) }
}

Set-App -k 'Service.Name'        -v $ServiceName
Set-App -k 'Service.Description' -v $ServiceDescription
Set-App -k 'PLC.Id'              -v $PlcId
Set-App -k 'Logs.WriteEventViewer' -v 'true'

$cfg.Save($dstExeConfig)
Write-Info "Config ajustado: $dstExeConfig"

# 7) Registrar servico
$binPath = '"' + (Join-Path $dest $serviceExeName) + '"'
if ($ServiceAccount -eq 'LocalSystem') {
  sc.exe create "$ServiceName" binPath= $binPath start= auto | Out-Null
} else {
  sc.exe create "$ServiceName" binPath= $binPath start= auto obj= "$ServiceAccount" password= "$ServicePassword" | Out-Null
}
sc.exe description "$ServiceName" "$ServiceDescription" | Out-Null
sc.exe failure     "$ServiceName" reset= 86400 actions= restart/5000/restart/5000/restart/15000 | Out-Null
try { sc.exe config "$ServiceName" start= delayed-auto | Out-Null } catch { }

if ($StartAfterInstall) {
  Write-Info "Iniciando $ServiceName ..."
  sc.exe start "$ServiceName" | Out-Null
}

Write-Host "OK. Instancia instalada em: $dest" -ForegroundColor Green
