# 3. Motor de Segurança e Banco de Dados (SQL Server)

O Microsoft SQL Server atua não apenas como repositório de dados, mas como o **Cérebro de Segurança e Intertravamento** da planta. Grande parte do esforço computacional ocorre dentro do banco através de *Views* e *Stored Procedures*.

> 💡 **Para iniciantes:** 
> * Uma **View (vw_)** é como uma "câmera de segurança": ela não altera nada, apenas junta dados de várias tabelas complexas e exibe uma imagem limpa e fácil de ler.
> * Uma **Stored Procedure (sp_)** é como um "robô trabalhador": ela executa ações reais (Inserir, Atualizar, Deletar dados) baseada em uma receita passo a passo.
> * Uma **Function (fn_)** é uma calculadora: você entrega um valor, ela faz uma conta lógica e devolve um resultado para ser usado dentro das Views.

Abaixo, detalhamos as principais engrenagens do banco de dados divididas por "Departamentos".

---

## 3.1. Grupo de Segurança (Consistência e Intertravamento)

Antes do sistema permitir que o operador ligue uma rota, ele precisa garantir que as máquinas físicas estão na posição correta e que não haverá colisões.

* **Functions Físicas (`fn_Consistency_I_Route_Damper`, `_Feeder`, `_Tripper`, `_Reversal`)**
  * **O que faz:** Verifica se a "chave" da esteira (o equipamento roteador) está virada para o lado certo.
  * **Como funciona no código:** A função recebe o ID da Rota. Ela faz um `JOIN` entre o *Valor Desejado* para aquela rota (`REF_POS.Value`) e o *Valor Atual* lido do PLC (`MED_CON.Value`). Se forem iguais, retorna `1` (Permitido). Se a máquina estiver virada para o lado errado, retorna `0` (Bloqueado).

* **Function Lógica (`fn_Consistency_I_Route_Rule`)**
  * **O que faz:** Evita gargalos lógicos, como tentar usar uma esteira além da sua capacidade ou tentar iniciar duas rotas no mesmo Virador de Vagões.
  * **Como funciona no código:** Ela conta quantas rotas já estão ativas (`rRouteActive`) passando por uma determinada esteira e compara com o limite cadastrado (`ValueFloat`). Se ultrapassar, ela retorna um texto de erro (Ex: *"O nº de rotas chegou no limite"*).

* **A View Mestra (`vw_Route_Consistency`)**
  * **O que faz:** É a tela de "Ok final" para o WPF.
  * **Como funciona no código:** Ela agrupa todas as funções citadas acima (`tableoftrue.dumper`, `tableoftrue.feeder`, etc.). Se qualquer uma delas for `0` (Falsa), o status global da rota vira `0` e o botão "Iniciar" fica bloqueado na tela do operador.

---

## 3.2. Grupo de Acionamento (Comunicação com PLC)

Quando a rota está consistente e o operador clica em "Iniciar", o banco precisa avisar o Windows Service (OPC) para ligar os motores.

* **`vw_Tag_Bool_Activate`**
  * **O que faz:** Uma lista gigante com todas as "chaves elétricas" (Tags) que precisam ser ligadas.
  * **Como funciona no código:** Ela usa vários comandos `UNION` para juntar as permissões de Dampers, Feeders e Equipamentos que fazem parte da rota selecionada.

* **`sp_Route_Tag_Bool_Activate`**
  * **O que faz:** É o gatilho de partida.
  * **Como funciona no código:** Ela pega a lista da view acima e executa um `UPDATE` na tabela `rTagWrite`, definindo o campo `Write = 1` e `Value = '1'`. Assim que isso acontece, o driver OPC lê essa tabela e envia o choque elétrico (sinal lógico) para o painel da planta.

* **`sp_Tag_Deactivate_Permission`**
  * **O que faz:** Desliga as permissões de equipamentos que não fazem mais parte de nenhuma rota ativa (economia de energia e segurança).
  * **Como funciona no código:** Compara as permissões ativas contra as rotas ativas. O que sobrar (órfão), recebe um `UPDATE` definindo `Value = '0'`.

---

## 3.3. Grupo de Faturamento (Produção, Balanças e GPV)

O sistema não apenas liga máquinas, ele pesa o minério para enviar relatórios ao GPV (Gestão de Portos da Vale), o que impacta diretamente o faturamento da empresa.

* **Views de Balança (`vw_Route_Scale_Origin`, `vw_Route_Scale_Destination`)**
  * **O que faz:** Encontra as balanças exatas que estão no caminho da rota.
  * **Como funciona no código:** Filtra a tabela de instrumentos buscando apenas os do tipo Balança (`TypeId = 42`) e junta com a sequência da rota (`rRouteSequence`) para saber quanto material entrou (Origem) e quanto saiu (Destino).

* **`sp_Production_Discharge_Ins` e `sp_Production_Discharge_Upd`**
  * **O que faz:** A `_Ins` (Insert) cria a "pasta do lote" assim que a rota inicia. A `_Upd` (Update) atualiza o peso e a quantidade de vagões de segundo a segundo.
  * **Como funciona no código:** A procedure lê a balança e faz uma matemática simples: `Load = Valor Atual da Balança - Valor Inicial (InitialLoad)`. Ela também conta os vagões baseada na tag de giro do virador (`vw_Tag_Overturned_Wagons_Instrument`).

* **`sp_Production_All_Shift_Closing` (Fechamento de Turno)**
  * **O que faz:** É o "Relógio de Ponto" do sistema. A cada 6 horas, corta as produções ativas e começa uma nova.
  * **Como funciona no código:** Tem um `IF` no início que checa a hora do servidor (`DATEPART(hour,getdate()) = 0, 6, 12, 18`). Se bater, ela força a flag `Final = 1` nas produções antigas e cria produções novas limpas para o próximo turno.

* **`sp_Production_All_Clean` (O Faxineiro)**
  * **O que faz:** Limpa sujeiras do banco de dados para evitar lentidão.
  * **Como funciona no código:** Deleta produções que foram fechadas há mais de 2 horas e que transportaram "0 toneladas" (rotas que foram ativadas por engano ou para testes).

---

## 3.4. Grupo de Supervisório e Saúde do Sistema (Health Check)

O sistema possui rotinas para avisar os operadores físicos (que estão dentro das cabines das máquinas) sobre o que o sistema automático está fazendo.

* **`sp_SPV_Exec_Route_CNs`, `_EPs`, `_RCs`**
  * **O que faz:** Atualiza os painéis (IHM/Displays) de Carregadores de Navios (CN), Empilhadeiras (EP) e Recuperadoras (RC).
  * **Como funciona no código:** Faz um `UPDATE` na tabela de tags de escrita (`rTagWrite`) enviando o nome da rota (Ex: *"Rota Origem 1 para Píer 2"*) para que o operador da máquina saiba qual material está chegando na esteira dele.

## 3.5 Diagrama de Entidade-Relacionamento (ERD)

O diagrama abaixo ilustra a estrutura física do banco de dados gerado pelo Entity Framework (Code-First). 

A modelagem é fortemente baseada no conceito de herança através da entidade **`Location`** (que atua como tabela central). Observe que equipamentos (como `Conveyor`, `Tripper`, `Plc`) compartilham suas chaves primárias (PK) com o ID da tabela `Location` (FK), formando relações 1-para-1 rigorosas.

```mermaid
erDiagram
    %% ==========================================
    %% BLOCO CENTRAL: LOCATION E HIERARQUIA
    %% ==========================================
    Location {
        bigint Id PK
        bigint ParentId FK "Auto-referência (Hierarquia)"
        bigint TypeId FK
        varchar Name
        nvarchar Alias
    }
    
    Type {
        bigint Id PK
        nvarchar Name
        nvarchar Description
    }

    Location ||--o{ Location : "ParentId (Nó Pai/Filho)"
    Type ||--o{ Location : "Classifica o Location"

    %% ==========================================
    %% BLOCO: ESPECIALIZAÇÕES FÍSICAS (1:1 com Location)
    %% ==========================================
    Conveyor { bigint Id PK,FK }
    Tripper { bigint Id PK,FK }
    Feeder { bigint Id PK,FK }
    Damper { bigint Id PK,FK }
    Reversal { bigint Id PK,FK }
    Origin { bigint Id PK,FK }
    Destination { bigint Id PK,FK }
    Plc { bigint Id PK,FK }
    Berth { bigint Id PK,FK }
    Compartment { bigint Id PK,FK }

    Location ||--|| Conveyor : "é um"
    Location ||--|| Tripper : "é um"
    Location ||--|| Feeder : "é um"
    Location ||--|| Damper : "é um"
    Location ||--|| Reversal : "é um"
    Location ||--|| Origin : "é um"
    Location ||--|| Destination : "é um"
    Location ||--|| Plc : "é um"
    Location ||--|| Berth : "é um"
    Location ||--|| Compartment : "é um"

    %% ==========================================
    %% BLOCO: ROTEAMENTO (Grafos e Sequências)
    %% ==========================================
    RouteGraph {
        bigint Id PK
        nvarchar Route
    }
    
    rRouteGraphSequence {
        bigint Id PK
        int Order
        bigint LocationId FK "Aresta/Nó do Caminho"
        bigint RouteGraphId FK
    }

    OxD {
        bigint Id PK,FK
        bigint OriginId FK
        bigint DestinationId FK
    }

    rRouteOxD {
        bigint Id PK
        bigint OxDId FK
        bigint LocationId FK
    }

    Location ||--o{ rRouteGraphSequence : "Faz parte de"
    RouteGraph ||--o{ rRouteGraphSequence : "Possui Passos"
    Location ||--|| OxD : "é um"
    OxD ||--o{ rRouteOxD : "Mapeia Rotas"

    %% ==========================================
    %% BLOCO: GESTÃO DE ESTADO (Fila e Ativas)
    %% ==========================================
    rRouteQueue {
        bigint Id PK,FK "Ref. à Rota"
        datetime dh
    }
    
    rRouteActive {
        bigint Id PK,FK "Ref. à Rota"
        datetime dh
    }
    
    rRouteReplace {
        bigint Id PK,FK "Nova Rota"
        bigint LocationId FK "Rota Substituída"
        datetime dh
    }

    Location ||--|| rRouteQueue : "Está na Fila"
    Location ||--|| rRouteActive : "Está Operando"
    Location ||--|| rRouteReplace : "Substitui"

    %% ==========================================
    %% BLOCO: AUTOMAÇÃO (OPC DA / CLP)
    %% ==========================================
    Tag {
        bigint Id PK
        bigint PlcId FK
        nvarchar Name
    }

    rInstrumentMeasure {
        bigint Id PK,FK "Ref. ao Instrumento (Location)"
        nvarchar Value
        datetime dh
        datetime LastDh
        bigint TagId FK
    }

    rTagWrite {
        bigint Id PK,FK
        bit Write
        nvarchar Value
    }

    Plc ||--o{ Tag : "Possui Tags"
    Tag ||--o{ rInstrumentMeasure : "Lê do PLC"
    Location ||--|| rInstrumentMeasure : "Possui Valor Lido"
    Tag ||--|| rTagWrite : "Escreve no PLC"

    %% ==========================================
    %% BLOCO: PRODUÇÃO E RATEIO
    %% ==========================================
    Production {
        uniqueidentifier Id PK
        datetime dhi
        datetime dhf
        bit Final
        bit Active
    }

    rProductionRoute {
        uniqueidentifier Id PK
        uniqueidentifier ProductionId FK
        bigint RouteId FK
        float Load
        int NWagon
        float InitialLoad
        int InitialNWagon
    }

    rProductionStock {
        uniqueidentifier Id PK,FK
        float Load
        float InitialLoad
    }

    Production ||--o{ rProductionRoute : "Gera Histórico (Origem)"
    Production ||--|| rProductionStock : "Gera Estoque (Destino)"
    Location ||--o{ rProductionRoute : "Transportou"

* **`vw_Tag_Media_Driver_Optimized` e `sp_Tag_Calc_Media_Driver`**
  * **O que faz:** É o "médico" do driver de comunicação OPC. Ele monitora se a leitura do PLC travou.
  * **Como funciona no código:** O código calcula o tempo entre a última leitura (`LastDh`) e o momento atual. Se a média de atraso passar de 7 segundos ou o dado for mais velho que 60 segundos, a View classifica o status como `'Bad'`. A *Stored Procedure* lê isso e, se achar um `'Bad'`, insere um alarme automático na tabela de `Log` avisando: *"Travamento de Driver de Comunicação"*.
```
