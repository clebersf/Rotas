
# Vale.Tops.Integration.OpcDaDriver.Service (fix7)

Windows Service com **health-check** e **file logging** (diário) para o driver OPC DA.

## Novidades (fix7)
- **File logging** (UTF-8, sem BOM) com rotação diária (`opcda_YYYYMMDD.log`).
  - Pasta configurável: `Logs.Path` (padrão: `%PROGRAMDATA%\OPCDAService\logs`).
  - Também pode logar no **Event Viewer** (`Logs.WriteEventViewer=true`).
- **Health monitor** periódico:
  - Config: `Health.Enabled=true`, `Health.LogIntervalMs=60000`.
  - Loga: `Connected`, `Items`, `TotalUpdates`, `UpdatesDelta` (desde o último health), `Queue`, `LastChangeUTC`, `LastFlushUTC`, `LastError`.

## Como rodar em CONSOLE (debug)
```
Vale.Tops.Integration.OpcDaDriver.Service.exe /console
```

## Como instalar como serviço
1) Compile Release x86 e copie os binários para `C:\Apps\OPCDAService`.
2) Abra CMD/PowerShell **elevado** e execute:
```
sc create "Vale.OPCDA.Driver" binPath= "C:\Apps\OPCDAService\Vale.Tops.Integration.OpcDaDriver.Service.exe" start= auto
sc start  "Vale.OPCDA.Driver"
```
3) Ajuste a **conta de logon** se precisar de credenciais específicas para DCOM/OPC.
4) Logs de evento: **Event Viewer → Application** (Source: `Vale.OPCDA.Driver`).
5) Logs de arquivo: pasta definida em `Logs.Path`.

## Notas
- Para Matrikon, mantenha `ItemId.UseBrackets=false`; para RSLinx, use `true`.
- Para OPC remoto, valide DCOM/Firewall e execute o serviço sob um usuário autorizado no servidor OPC.

