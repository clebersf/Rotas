Documentação Arquitetural e Funcional: Sistema Rotas (TOPS)
1. Visão Geral do Sistema
O Sistema Rotas é um módulo de missão crítica do ecossistema TOPS (Terminal Operations System), operando em um terminal portuário de minério de ferro em Vitória-ES.
O sistema gerencia, valida e automatiza o roteamento de minério através de uma complexa rede de ativos físicos (esteiras, viradores de vagões, empilhadeiras, carregadores de navio). Ele atua como a ponte entre o planejamento de produção (nível tático) e a automação de chão de fábrica (nível operacional).
Fundamentação Teórica: A modelagem matemática e lógica do sistema é baseada na dissertação de Cleber Silva Ferreira (2021), utilizando modelagem em grafos e a heurística VNS (Variable Neighborhood Search) para minimizar o tempo de atendimento (
t
m
a
x
tmax
), consumo de energia e desgaste físico dos equipamentos.
2. Stack Tecnológico
Linguagem: C#
Framework: .NET Framework 4.6.1 / 4.7.2
ORM: Entity Framework 6.4.4 (Abordagem Code-First)
Banco de Dados: Microsoft SQL Server (Atuando como Motor de Regras e Intertravamento)
Interface (UI): WPF (Windows Presentation Foundation)
Integração Industrial: Windows Service (OPC Client DA via COM/Interop)
3. Arquitetura da Solução e Projetos
A solution está dividida nas seguintes camadas lógicas:
Vale.Tops.Domain: Modelagem rica de entidades (POCOs).
Vale.Tops.Integration.Infrastructure.DataBase: Padrão Repository, CQRS e mapeamento EF.
Vale.Tops.Integration.OpcDaDriver.Service: Integração OPC/PLC em tempo real.
Vale.Rotas: Apresentação WPF.
4. Modelo de Domínio (Entity Framework Code-First)
4.1. O Padrão Arquitetural Location (Núcleo do Sistema)
O coração da modelagem é a entidade abstrata Location. Ela representa um "Nó" físico ou lógico dentro do complexo portuário. O sistema utiliza uma abordagem de Composição 1-para-1 (simulando herança).
Equipamentos específicos (Conveyor, Tripper, Feeder, Plc) compartilham o mesmo Id de um Location. Para o motor de grafos, uma esteira é apenas um Location; para o intertravamento físico, ela é um Conveyor.
code
C#
[Table("Conveyor")]
public partial class Conveyor : Entity
{
    [Key]
    [ForeignKey("Location")]
    public virtual long Id { get; set; }
    public virtual Location Location { get; set; }
}
4.2. Diagrama de Classes Simplificado (Core Engine)
code
Mermaid
classDiagram
    direction TB

    %% Entidade Central
    class Location {
        +long Id
        +long? ParentId
        +string Name
    }

    %% Especializações 1:1 (Ativos Físicos)
    class Conveyor { +long Id }
    class Tripper { +long Id }
    class Origin { +long Id }
    class Destination { +long Id }

    %% Topologia e Estado
    class OxD { +long OriginId \n +long DestinationId }
    class rRouteSequence { +int Order \n +long LocationId }
    class rRouteActive { +DateTime dh }

    %% Automação e PLC
    class Plc { +string Resource }
    class Tag { +string Name }
    class rInstrumentMeasure { +string Value \n +DateTime dh }
    class rTagWrite { +string Value \n +bool Write }

    %% Relacionamentos
    Location "1" <-- "0..*" Location : Parent (Hierarquia)
    Location "1" *-- "0..1" Conveyor : extends
    Location "1" *-- "0..1" Tripper : extends
    Location "1" *-- "0..1" rRouteActive : State

    OxD "1" --> "1" Origin
    OxD "1" --> "1" Destination
    rRouteSequence "*" --> "1" Location : Step
    
    Plc "1" *-- "*" Tag
    rInstrumentMeasure "*" --> "1" Tag : Reads
    rTagWrite "1" --> "1" Tag : Writes
5. Arquitetura de Dados e Motor SQL (Safety e Lógica)
O SQL Server não é apenas um repositório, mas o verdadeiro motor de regras de negócio (intertravamento e segurança). Validações críticas ocorrem no banco para garantir latência mínima e evitar colisões físicas na planta.
5.1. Matrizes e Bloqueios Geométricos
Matriz de Bloqueio (bkr): Restringe o uso simultâneo de rotas incompatíveis (cruzamento físico).
Matriz de Interseção (itkr): Mapeia quais rotas podem operar juntas no mesmo fluxo lógico.
Restrição de Maré: Bloqueia a finalização da rota se o calado do navio impedir desatracação segura.
5.2. Views e Functions de Consistência
Antes da ativação, funções fn_Consistency_I_Route_* avaliam se o hardware está pronto:
Verifica posição de Dampers e Feeders.
Verifica sentido da correia (Reversal).
Retorna BoolVeredict e texto (Ex: "Sentido de reversão de correia - Inconsistente").
5.3. Processamento de Produção e Relatórios (Rateio GPV)
sp_ProductionRoute_Discharge_Upd: Lê as tags de balança e vagões virados, atualizando o totalizador (Load / NWagon).
sp_Production_All_Shift_Closing: Trigger que roda a cada 6 horas (00h, 06h, 12h, 18h) para fechar balanços de turno automaticamente.
6. Camada de Infraestrutura (Padrão CQRS e Repositórios)
Para evitar locks no banco de dados entre as rotinas de leitura massiva e os updates críticos do PLC, implementou-se a separação de responsabilidades (CQRS):
WriteReadContext: Contexto transacional (Entity Framework). Usado por WriteRead<T> para operações de INSERT, UPDATE e DELETE.
ReadOnlyContext: Contexto otimizado (NoTracking). Usado por ReadOnly<T> para carregar as Views de consistência, performance de leitura e executar Stored Procedures (sp_Route_Tag_Bool_Activate).
Injeção de Dependência: O container Autofac resolve os repositórios em tempo de execução (builder.RegisterGeneric(typeof(WriteRead<>)).As(typeof(IWriteRead<>))).
7. Camada de Apresentação (WPF - Interface do Operador)
O aplicativo CCO (Centro de Controle Operacional) provê a interface reativa para o operador gerenciar a fila.
Single-Instance via Mutex: A classe App.xaml.cs utiliza um Mutex e integrações Win32 (user32.dll, AttachThreadInput, SetForegroundWindow) para garantir que apenas uma tela rode por máquina. Tentar abrir o app novamente apenas pisca e foca a janela já aberta.
Gestão Assíncrona (DispatcherTimer): A interface atualiza o status dos PLCs a cada 10 segundos chamando o SQL de forma contínua em threads de background (DispatcherPriority.Background), mantendo a UI responsiva.
Funcionalidades Core:
Enfileirar Rotas Sugeridas (VNS).
Validar Consistência (Checar alarmes e bloqueios).
Iniciar Operação (Move para rRouteActive).
Substituir Rotas ("Trocar pneu com carro andando" via tabela rRouteReplace).
8. Integração Industrial (Windows Service OPC Client)
O projeto OpcDaDriver.Service é o "Gêmeo Digital". É um serviço Windows que converte as decisões lógicas do SQL Server em tensão elétrica nos PLCs através do protocolo OPC DA.
8.1. Desafios Técnicos Resolvidos
O OPC DA Clássico usa COM/DCOM, que é instável em .NET multi-thread. A solução arquitetural:
Thread STA Dedicada: A comunicação roda exclusivamente em um Single-Threaded Apartment.
Win32 Message Pump: Implementado um loop nativo (MsgWaitForMultipleObjects / DispatchMessage) para não bloquear callbacks da automação.
8.2. Fluxo Produtor-Consumidor
code
Mermaid
graph TD
    subgraph OPC Server (Planta Física)
        PLC[(Memória do PLC)]
    end

    subgraph Windows Service (OpcDaPlcRunner)
        STA[Thread STA \n OPC COM Interop]
        Q[(ConcurrentQueue)]
        TaskFlush[[Task FlushMeasures \n 300ms]]
        TaskWrite[[Task PollWrites \n 500ms]]
        
        STA -- OnDataChange --> Q
        Q -- Dequeue Batch --> TaskFlush
    end

    subgraph SQL Server (Entity Framework)
        DB_MEASURE[(Tabela rInstrumentMeasure)]
        DB_WRITE[(Tabela rTagWrite)]
    end

    PLC -- Subscribe --> STA
    TaskFlush -- EF SaveChanges() --> DB_MEASURE
    
    DB_WRITE -- Polling --> TaskWrite
    TaskWrite -- SyncWrite --> STA
    STA -- Grava Valor --> PLC
Leitura (Planta 
→
→
 SQL): O evento Group_DataChange (OnValue) enfileira variáveis na ConcurrentQueue. A cada 300ms, o FlushMeasuresLoop agrupa as leituras e faz um único SaveChanges massivo, reduzindo gargalos de IO no SQL.
Escrita (SQL 
→
→
 Planta): A rotina PollWritesLoop lê comandos onde Write = true na tabela rTagWrite. A função CoerceToCanonicalVariant converte o tipo string para o binário estrito exigido pelo PLC (VT_BOOL, VT_R4, etc.) e executa um SyncWrite via Interop.
9. Fluxo de Vida Completo (Da Interface à Máquina)
O diagrama abaixo consolida a arquitetura completa quando o operador inicia uma rota.
code
Mermaid
sequenceDiagram
    autonumber
    actor Operador
    participant UI as WPF (Vale.Rotas)
    participant EF as Entity Framework
    participant SQL as SQL Server
    participant OPC as Windows Service
    participant PLC as Automação Física

    Note over Operador, SQL: 1. Validação Lógica
    Operador->>UI: Clica "Consistência"
    UI->>EF: ReadOnlyContext (Views)
    EF->>SQL: SELECT nas UDFs de Intertravamento
    SQL-->>UI: Retorna: "Tudo OK"
    
    Note over Operador, SQL: 2. Transição de Estado
    Operador->>UI: Clica "Iniciar Operação"
    UI->>EF: WriteReadContext.Save()
    EF->>SQL: DELETE de rRouteQueue <br/> INSERT em rRouteActive
    UI->>EF: Exec() sp_Route_Tag_Bool_Activate
    EF->>SQL: UPDATE rTagWrite SET Write=1, Value='1'
    
    Note over SQL, PLC: 3. Execução Física
    loop Polling (500ms)
        OPC->>SQL: Select Tags pendentes de escrita
        SQL-->>OPC: Retorna comandos da rota
        OPC->>OPC: CoerceToCanonicalVariant()
        OPC->>PLC: Protocolo OPC DA (Liga Sirene/Motores)
        PLC-->>OPC: Confirma Sucesso (HRESULT 0)
        OPC->>SQL: UPDATE rTagWrite SET Write=0
    end
