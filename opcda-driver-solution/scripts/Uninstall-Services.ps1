for ($i=1; $i -le 25; $i++) {
    $svc = "Vale.OPCDA.Driver.PLC$($i)"
    sc.exe stop $svc | Out-Null
    sc.exe delete $svc | Out-Null
    Write-Host "Removed $svc"
}
