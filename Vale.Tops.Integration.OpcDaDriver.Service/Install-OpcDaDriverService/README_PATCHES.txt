OPC DA Driver - Pacote com PATCHES
=====================================

Este pacote aplica as correções discutidas:

1) Install-OpcDaDriverInstance.ps1
   - Cópia **recursiva** dos binários (mantém .xml e subpastas)
   - Copia o **.exe.config do build** e **ajusta somente** Service.Name, Service.Description e PLC.Id (preserva connectionStrings, timers, etc.)
   - Liga Logs.WriteEventViewer=true por padrão

2) Install-Instance.cmd
   - Wrapper de instalação com os caminhos informados (SRC Debug e DST Program Files\Vale)

3) Uninstall-Instance.cmd
   - Desinstala serviço e remove a pasta **C:\Program Files\Vale\<ServiceName>** quando usado com "removefiles"
   - Proteções contra remoção da pasta do próprio script e fora do root esperado

4) ServiceHost.cs (PATCH)
   - Define **CurrentDirectory = BaseDirectory** no OnStart e registra no log (evita logs em System32)

Como usar
--------
1) Edite ServiceName/Description/PLC ao chamar o Install-Instance.cmd:
   Install-Instance.cmd PLC1 "Driver PLC 1" 1

2) Desinstalar:
   Uninstall-Instance.cmd PLC1 removefiles

Diagnóstico
-----------
- Verifique o Event Viewer (Application) com Logs.WriteEventViewer=true
- O FileLog passa a usar a pasta do executável (BaseDirectory)
