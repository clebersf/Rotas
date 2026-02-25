# Vale.Tops.Integration.OpcDaDriver

Windows Service (per PLC) for OPC DA with RSLinx Classic.

## Build
1. Copy `OpcNetApi.dll` and `OpcNetApi.Com.dll` to `libs/OPC/`.
2. Open `Vale.Tops.Integration.OpcDaDriver.sln` in Visual Studio 2019+ (or MSBuild).
3. Build in Release (x86 target is configured).

## Configure
- Ensure your database connection is correct in `Vale.Tops.Integration.OpcDaDriver/App.config` (`Connection_AssetManager`).
- In the database, set `rTagGroup.WindowsService` to `Vale.OPCDA.Driver.PLC1` .. `PLC25` and fill `OpcServer`, `AddrOpcServer`, `Rate`, and `TagId`.

## Install services
```powershell
# Install (and start) 25 services (PLC1..PLC25)
powershell -ExecutionPolicy Bypass -File .\scripts\Install-Services.ps1 -Start

# Uninstall
powershell -ExecutionPolicy Bypass -File .\scripts\Uninstall-Services.ps1
```

Each service passes `--name=Vale.OPCDA.Driver.PLCN` to the executable, which overrides `Service.Name` in AppSettings.
