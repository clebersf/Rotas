# 2. Modelo de Domínio e Infraestrutura (C# / EF)

A camada de domínio (`Vale.Tops.Domain`) e a camada de infraestrutura (`Vale.Tops.Integration.Infrastructure.DataBase`) foram construídas utilizando o **Entity Framework 6.4.4** no paradigma **Code-First**.

## 2.1 O Padrão Arquitetural `Location`
A entidade central do sistema é a classe **`Location`**. O sistema utiliza uma abordagem de **Composição 1-para-1** rigorosa (usando `[Key]` e `[ForeignKey]`). 

Para o motor de busca, tudo é um "Nó" (`Location`) com auto-referência (`ParentId`), permitindo modelar a planta em formato de árvore. Para as regras de negócio de intertravamento, os equipamentos assumem suas especializações técnicas (`Conveyor`, `Tripper`, `Plc`, `Tag`).

## 2.2 Diagrama de Classes Core (Domínio)

```mermaid
classDiagram
    direction TB

    class Location {
        +long Id
        +long? ParentId
        +string Name
    }

    %% Ativos Físicos (1:1)
    class Conveyor { +long Id }
    class Tripper { +long Id }
    class Origin { +long Id }
    class Destination { +long Id }

    %% Grafos e Filas
    class OxD { +long OriginId \n +long DestinationId }
    class rRouteSequence { +int Order \n +long LocationId }
    class rRouteActive { +DateTime dh }

    %% Automação
    class Plc { +string Resource }
    class Tag { +string Name }
    class rInstrumentMeasure { +string Value \n +DateTime dh }
    class rTagWrite { +string Value \n +bool Write }

    Location "1" <-- "0..*" Location : Parent
    Location "1" *-- "0..1" Conveyor : extends
    Location "1" *-- "0..1" Tripper : extends
    Location "1" *-- "0..1" rRouteActive : State

    OxD "1" --> "1" Origin
    OxD "1" --> "1" Destination
    rRouteSequence "*" --> "1" Location : Step
    
    Plc "1" *-- "*" Tag
    rInstrumentMeasure "*" --> "1" Tag : Reads
    rTagWrite "1" --> "1" Tag : Writes
```
## 2.3 Camada de Persistência (CQRS)
O sistema implementa o Repository Pattern segregando as responsabilidades de leitura e escrita para evitar locks transacionais pesados no SQL:
WriteReadContext: Contexto do Entity Framework para transações ACID (Insert, Update, Delete). Ex: IWriteRead<Location>.
ReadOnlyContext: Contexto NoTracking utilizado puramente para leitura das Views e acionamento de Stored Procedures (como a sp_Route_Tag_Bool_Activate).
Autofac: Utilizado para injeção de dependência (InstancePerLifetimeScope), resolvendo as interfaces genéricas dos repositórios.
