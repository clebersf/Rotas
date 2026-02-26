# 4. Camada de Apresentação (WPF UI)

A interface operacional `Vale.Rotas` (desenvolvida em C# WPF) atua como o CCO (Centro de Controle Operacional), oferecendo monitoramento em tempo real e orquestração das demandas.

## 4.1 Características Técnicas da Aplicação
* **Single-Instance Application (Mutex):** A classe `App.xaml.cs` utiliza um `Mutex` global e bibliotecas Win32 (`user32.dll`, `SetForegroundWindow`, `AttachThreadInput`) para garantir que os operadores não abram instâncias duplicadas (o que poderia causar falhas de transação).
* **Gestão de UI Threads:** Consultas pesadas ao Entity Framework rodam via `Dispatcher.BeginInvoke(DispatcherPriority.Background)` através de um `DispatcherTimer` a cada 10 segundos.

## 4.2 Fluxo Operacional na Interface
1. **Seleção de Rotas (`Add.xaml`):** Janela modal com filtros inteligentes (Embarque, Descarga, Píer). Salva registros provisórios na tabela `rRouteQueue`.
2. **Substituição de Ativos (Hot-Swap):** O sistema permite trocar equipamentos falhos na hora ("Trocar pneu com o carro andando"). A função `fn_RouteReplacer_I_Route_O_Route` avalia rotas alternativas que terminam no mesmo destino.
3. **Validação de Intertravamento (`Consistency.xaml`):** Consulta a visão de *Safety* do banco. Mostra ao usuário de forma amigável onde estão as inconsistências físicas (ex: "Sentido de reversão de correia - Inconsistente").
4. **Ativação (`MainWindow.xaml`):** Clicar em "Operar" deleta a rota da fila (`rRouteQueue`) e a move para as Rotas Ativas (`rRouteActive`), invocando a SP que acionará o PLC.
