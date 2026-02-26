# 5. Integração Industrial (Windows Service OPC Client)

O projeto `OpcDaDriver.Service` é o verdadeiro "Gêmeo Digital". É um Windows Service autônomo e altamente resiliente que traduz a intenção do banco de dados (C# / SQL) em tensão elétrica nos painéis industriais (PLC) via protocolo **OPC DA** Clássico (COM/Interop).

## 5.1 Arquitetura Produtor-Consumidor (Padrão Multithread)
Para contornar a notória instabilidade do protocolo COM da Microsoft em .NET, a comunicação do OPC foi isolada:
* **Thread STA + Message Pump:** O OPC só conversa com uma thread única (`ApartmentState.STA`). Um laço Win32 nativo (`MsgWaitForMultipleObjects`) mantém a fila de mensagens do Windows viva para receber os callbacks.

```mermaid
graph TD
    subgraph OPC Server (Planta)
        PLC[(Memória do PLC)]
    end

    subgraph Windows Service
        STA[Thread STA \n OPC COM Interop]
        Q[(ConcurrentQueue)]
        TaskFlush[[Task FlushMeasures \n 300ms]]
        TaskWrite[[Task PollWrites \n 500ms]]
        
        STA -- OnDataChange --> Q
        Q -- Dequeue Batch --> TaskFlush
    end

    subgraph SQL Server
        DB_MEASURE[(rInstrumentMeasure)]
        DB_WRITE[(rTagWrite)]
    end

    PLC -- Subscribe --> STA
    TaskFlush -- EF SaveChanges() --> DB_MEASURE
    DB_WRITE -- Polling --> TaskWrite
    TaskWrite -- SyncWrite --> STA
    STA -- Grava Valor --> PLC
