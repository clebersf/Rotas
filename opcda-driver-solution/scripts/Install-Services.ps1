param(
    [string]$ExePath = "$PSScriptRoot\..\Vale.Tops.Integration.OpcDaDriver\bin\Release\Vale.Tops.Integration.OpcDaDriver.exe",
    [string]$DisplayPrefix = "Vale OPCDA Driver",
    [switch]$Start
)

if (!(Test-Path $ExePath)) { Write-Error "Executable not found: $ExePath"; exit 1 }

for ($i=1; $i -le 25; $i++) {
    $svc = "Vale.OPCDA.Driver.PLC$($i)"
    $binPath = '"' + $ExePath + '" --name=' + $svc
    sc.exe create $svc binPath= $binPath start= auto DisplayName= '"' + $DisplayPrefix + ' - PLC' + $i + '"'
    sc.exe description $svc "OPC DA driver service for PLC$($i) (RSLinx)"
    if ($Start) { sc.exe start $svc }
}
