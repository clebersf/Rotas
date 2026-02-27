# 2. Modelo de Domínio e Infraestrutura (C# / Entity Framework)

Este capítulo explica o coração do sistema no lado do código (C#). A modelagem das regras de negócio (camada de Domínio) e a comunicação com o banco de dados (camada de Infraestrutura) foram construídas utilizando o **Entity Framework 6.4.4 (EF)** no paradigma **Code-First**.

> 💡 **Para iniciantes: O que é o Entity Framework e o Code-First?**
> Antigamente, os programadores precisavam abrir o SQL Server, criar tabelas na mão, e depois escrever dezenas de linhas de código C# apenas para conectar o software ao banco. 
> O *Entity Framework (EF)* é uma ferramenta (ORM) que faz essa ponte magicamente. No paradigma *Code-First*, **o código C# manda no banco de dados**. Você cria classes simples em C# (POCOs) e o EF se encarrega de ir até o SQL Server e criar as tabelas, as colunas e os relacionamentos correspondentes.

---

## 2.1. O Padrão Arquitetural `Location` (A Classe Mestra)

A genialidade da modelagem deste sistema está na forma como os equipamentos físicos foram traduzidos para o código através da herança por composição.

A entidade central do sistema é a classe **`Location`**. Pense nela como um "Nó Genérico" no mapa do porto. Todo equipamento (uma esteira, um PLC, um motor) é, antes de tudo, um `Location`.

**Como isso funciona no código C#?**
O sistema não usa herança clássica (`class Conveyor : Location`). Em vez disso, ele usa **Composição 1-para-1** forçada através de *Data Annotations* (instruções entre colchetes acima da classe):

```csharp
[Table("Conveyor")] // Avisa o EF para criar uma tabela chamada "Conveyor"
public partial class Conveyor : Entity
{
    [Key] // Avisa o EF que a propriedade abaixo é a Chave Primária (PK)
    [ForeignKey("Location")] // Avisa que também é uma Chave Estrangeira (FK) apontando para a tabela Location
    public virtual long Id { get; set; }
    
    // Propriedade de Navegação: O C# entende que este Conveyor PERTENCE a um Location específico.
    public virtual Location Location { get; set; } 
}

```

Por que fazer assim?
Para o motor de busca de rotas (matemática de grafos), uma esteira ou um navio são apenas "Nós" genéricos com um ID (Location). Mas quando precisamos saber a velocidade ou potência daquela esteira, o EF carrega a especialização (Conveyor) que tem o mesmo ID.
## 2.2 Diagrama de Classes Core (C#)
O diagrama abaixo ilustra como as classes C# do projeto Vale.Tops.Domain estão conectadas na memória da aplicação (refletindo o diagrama do banco de dados).

classDiagram
    direction TB

    %% Entidade Central
    class Location {
        +long Id
        +long? ParentId
        +long? TypeId
        +string Name
        +string Alias
    }

    class Type {
        +long Id
        +string Name
    }

    %% Especializações 1:1
    class Conveyor { +long Id }
    class Tripper { +long Id }
    class Feeder { +long Id }
    class Damper { +long Id }
    class Reversal { +long Id }
    class Origin { +long Id }
    class Destination { +long Id }
    class Plc { +long Id \n +string Resource }

    %% Grafos e Filas
    class OxD { +long Id \n +long OriginId \n +long DestinationId }
    class rRouteOxD { +long Id \n +long OxDId \n +long LocationId }
    class RouteGraph { +long Id \n +string Route }
    class rRouteGraphSequence { +long Id \n +int Order \n +long LocationId \n +long RouteGraphId }

    class rRouteQueue { +long Id \n +DateTime dh }
    class rRouteActive { +long Id \n +DateTime dh }

    %% Automação (OPC)
    class Tag { +long Id \n +long PlcId \n +string Name }
    class rInstrumentMeasure { +long Id \n +string Value \n +DateTime dh \n +long TagId }
    class rTagWrite { +long Id \n +bool Write \n +string Value }

    %% Relacionamentos do Core
    Location "1" <-- "0..*" Location : Parent
    Type "1" <-- "0..*" Location : Classifica

    Location "1" *-- "0..1" Conveyor : Extends
    Location "1" *-- "0..1" Tripper : Extends
    Location "1" *-- "0..1" Feeder : Extends
    Location "1" *-- "0..1" Origin : Extends
    Location "1" *-- "0..1" Plc : Extends

    Location "1" *-- "0..1" OxD : Extends
    OxD "1" *-- "0..*" rRouteOxD : Possui

    RouteGraph "1" *-- "0..*" rRouteGraphSequence : Sequencia
    Location "1" <-- "0..*" rRouteGraphSequence : Faz parte de

    Location "1" *-- "0..1" rRouteQueue : Em Fila
    Location "1" *-- "0..1" rRouteActive : Em Operação

    Plc "1" *-- "0..*" Tag : Contém
    Tag "1" *-- "0..*" rInstrumentMeasure : Lê
    Location "1" *-- "0..1" rInstrumentMeasure : Recebe Valor
    Tag "1" *-- "0..1" rTagWrite : Escreve

## 2.3. O Contexto do Entity Framework (O "Gerente" do Banco)
O Entity Framework precisa de uma classe que atue como o "Gerente" da conexão. No projeto Infrastructure, essa classe é a WriteReadContext (que herda de DbContext).
Como ele cria as tabelas? (Fluent API)
Dentro do WriteReadContext, existe um método especial chamado OnModelCreating. É lá que configuramos regras que as Data Annotations (os colchetes nas classes) não dão conta de fazer.
Chamamos isso de Fluent API. Veja um exemplo:
```csharp
protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
    // Diz ao EF: "Toda esteira (Conveyor) OBRIGATORIAMENTE precisa de um Location"
    modelBuilder.Entity<Conveyor>().HasRequired(e => e.Location);    
    // Diz ao EF: "Um Location pode estar dentro de várias Sequências de Grafo. 
    // Mas se deletar o Location, NÃO delete a Sequência junto (WillCascadeOnDelete = false)"
    modelBuilder.Entity<Location>()
        .HasMany(e => e.rRouteSequence)
        .WithRequired(e => e.Location)
        .HasForeignKey(e => e.LocationId)
        .WillCascadeOnDelete(false);
}
```
## 2.4. Acesso aos Dados: Repository Pattern e CQRS
Em vez de as telas do sistema "falarem" diretamente com o banco de dados (o que viraria uma bagunça), o projeto usa o Padrão de Repositório (Repository Pattern). Pense no Repositório como um garçom: você pede a ele (em C#) para buscar uma Rota, e ele vai até a cozinha (banco de dados), pega a informação e te entrega.
Além disso, o sistema divide os garçons em dois tipos, usando o conceito de CQRS (Command Query Responsibility Segregation):
Repositórios de Escrita (WriteRead<T>)
Usados quando precisamos alterar dados (Insert, Update, Delete). Ele usa o WriteReadContext.
Repositórios de Leitura (ReadOnly<T>)
Usados APENAS para buscar dados e ler Views do banco. Como ele sabe que não vai alterar nada, ele não "rastreia" os objetos na memória (NoTracking implícito), tornando a busca no banco muito mais rápida. Ele usa o ReadOnlyContext.
## 2.5. Injeção de Dependência (Autofac)
Se uma tela (WPF) precisar buscar a lista de Correias, ela não instancia o banco de dados diretamente (new WriteReadContext()). Em vez disso, ela pede a lista para uma Interface (um "contrato" C# chamado IConveyorWriteRead).
💡 Por que usar Interfaces? Se amanhã a empresa decidir trocar o SQL Server pela nuvem, basta criar uma classe nova que assine o contrato da Interface. As telas do WPF não precisarão ser alteradas!
Para ligar a Interface com a Classe real, usamos o Autofac (um sistema de Injeção de Dependência). No arquivo RepositoryModule.cs, ensinamos o programa:
```csharp
// "Autofac, sempre que alguém pedir uma Interface genérica (IWriteRead<T>), 
// entregue a classe concreta (WriteRead<T>)!"
builder.RegisterGeneric(typeof(WriteRead<>))
       .As(typeof(IWriteRead<>))
       .InstancePerLifetimeScope();
```
## 2.6. Como funciona a consulta no dia a dia? (LINQ)
Graças a toda essa arquitetura (EF + Repositórios + Injeção de Dependência), o programador não precisa escrever consultas em SQL duro (SELECT * FROM...) no meio do código da tela. Ele usa o LINQ (Language Integrated Query), que permite fazer buscas usando a própria linguagem C#.
Exemplo Prático (Tela Add.xaml.cs):
Buscando todas as rotas ativas que possuem Píer 1 no nome e listando na tela:
```csharp
// 1. O Autofac já entregou a instância do repositório pronta para uso:
// private IrRouteActiveWriteRead rRouteActiveWriteRead;

// 2. Usando o LINQ para buscar no banco e cruzar tabelas (JOIN) sem escrever SQL:
var rotasAtivas = (from act in this.rRouteActiveWriteRead.All() // Pega tudo da tabela rRouteActive
                   join loc in locations on act.Id equals loc.Id // Junta com a tabela de Locations
                   where loc.Name.Contains("Pier1") // Filtra apenas as que vão pro Píer 1
                   select new Route { // Transforma o resultado num objeto simples para a tela
                       Id = loc.Id,
                       Completa = loc.Name
                   }).ToList();
```
O Entity Framework traduzirá esse bloco LINQ silenciosamente em uma query SQL altamente otimizada e executará no SQL Server.                   
