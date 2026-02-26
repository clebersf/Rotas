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
```
## 5.2 Fluxo de Leitura (OnDataChange)
O sistema não faz polling de leitura, para não estrangular a rede industrial. Quando um bit muda na planta, o evento Group_DataChange joga o valor e o timestamp numa fila thread-safe. A cada 300ms, a Task FlushMeasuresLoopAsync esvazia a fila e aplica um update massivo no banco (Tabela rInstrumentMeasure).
## 5.3 Fluxo de Escrita (Ativação de Planta) 
A cada 500ms, a rotina PollWritesLoopAsync verifica a tabela rTagWrite.
* Quando uma tag tem a flag Write = 1, o serviço converte a String gerada pelo sistema de roteamento para o tipo binário exigido pelo controlador (CoerceToCanonicalVariant -> VT_BOOL, VT_R4).
* A instrução SyncWrite é enviada à Thread STA. Se não houver erros (HRESULT 0), a flag de escrita é limpa no banco.
## 5.4 Resiliência
* Algoritmo de Backoff Exponencial (2s até 60s) para evitar quedas no Switch Industrial caso o OPC Server fique indisponível.
* Verificador de saúde (Health Check) que acusa Staleness caso a média de atualização (UpdatesDelta) pare de progredir ou ultrapasse os 7 segundos.
