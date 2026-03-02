

# 7. Manual de Manutenção e Diagnóstico (Troubleshooting)

Este manual é destinado à equipe de **Engenharia e Manutenção de Automação**. Ele detalha os procedimentos para diagnosticar falhas de comunicação entre o Sistema Rotas e a planta física, além de instruir sobre a configuração e reinicialização dos serviços OPC.

---

## 7.1. Diagnóstico de Falhas no Banco de Dados (SQL Server)

Quando uma rota recusa a iniciar e a interface do operador (WPF) não é suficiente para entender o problema, a equipe de manutenção deve acessar o **SQL Server Management Studio (SSMS)** e consultar as *Views* de diagnóstico.

### 7.1.1 Onde verificar as tags do PLC? (`rInstrumentMeasure`)
A tabela `rInstrumentMeasure` é o "espelho" do PLC no banco de dados. 
Se uma máquina física mudou de posição no campo, mas o Sistema Rotas não reconhece, o problema pode ser a comunicação entre o OPC e o banco.

1. Execute um `SELECT` na tabela `rInstrumentMeasure`.
2. Observe a coluna **`dh`** (Data e Hora da última leitura).
3. **Regra de Ouro:** Se a coluna `dh` estiver **mais de 3 minutos atrasada** em relação ao relógio atual, significa que o *Windows Service (Driver OPC)* travou ou o CLP está desligado/desconectado da rede.

> 📸 *(Insira aqui o Print da tela mostrando o SELECT na tabela rInstrumentMeasure)*  
> `![Tabela rInstrumentMeasure](../assets/tabela-rinstrumentmeasure.png)`

### 7.1.2 Script Rápido para Diagnóstico (Health Check)
Para facilitar, você pode rodar a *Query* abaixo para listar os Drivers OPC e identificar rapidamente se algum deles parou de comunicar com o banco:

```sql
SELECT Id, 
       Name, 
       ValueDateTime AS TimeAccumulated, 
       ValueInt AS CycleCount,
       CASE 
           WHEN DATEDIFF(second, LastDh, GETDATE()) > 60 THEN 'Bad' 
           ELSE 'Good' 
       END AS Status
FROM vw_Tag_Media_Driver
```

Se a coluna **Status** retornar `'Bad'`, o Driver daquele equipamento específico precisa ser reiniciado.

> 📸 *(Insira aqui o Print da Query rodando no SSMS mostrando a coluna Good/Bad)*  
> `![Query de Health Check](../assets/query-health-check.png)`

### 7.1.3 Investigando Intertravamentos (Views de Consistência)
Se o operador relatar que uma rota não fica "Consistente", você pode investigar a fundo qual tag exata está bloqueando o sistema acessando as Views de Consistência, como a `vw_Route_Consistency_Tripper` ou `vw_Route_Consistency_Damper`.

* A coluna **`Desired`** mostra o que o Sistema Rotas exige (ex: Valor `1`).
* A coluna **`Current`** mostra o que o CLP está reportando agora (ex: Valor `0`).
* A coluna **`Veredict`** mostrará `Wrong` (Errado).

> 📸 *(Insira aqui o Print mostrando a View de Consistência do Damper)*  
> `![View Consistência Damper](../assets/view-consistencia-damper.png)`

---

## 7.2. Configuração e Reinicialização dos Serviços (Windows Services)

O Sistema Rotas não é um programa único, mas sim um cluster de **22 Serviços de Windows** rodando em paralelo no servidor (um serviço isolado para cada CLP da planta). Isso evita que a falha de rede de um CLP trave todo o porto.

### 7.2.1 Como os Drivers sabem o que ler? (`rTagGroup`)
Os serviços não têm as tags *hardcoded* (fixas no código). Ao iniciar, o serviço lê a tabela `rTagGroup`. 
* A coluna **`Rate`** define a velocidade do *polling* (geralmente `5000` milissegundos).
* A coluna **`WindowsService`** define qual serviço é dono de qual CLP.

> 📸 *(Insira aqui o Print da tabela rTagGroup no SQL Server)*  
> `![Tabela rTagGroup](../assets/tabela-rtaggroup.png)`

### 7.2.2 Como configurar o arquivo do Serviço (`App.config`)
Se for necessário adicionar um novo Driver OPC ou alterar as configurações de banco de um Driver existente, siga os passos:

1. Acesse o servidor via *Remote Desktop*.
2. Navegue até a pasta do serviço: `C:\Program Files\Vale\Route Rotas OpcClientService [Nome da Máquina]`.
3. Localize o arquivo `Vale.Tops.Integration.OpcClientService.exe.config`.
4. **Importante:** Abra o Bloco de Notas (`notepad.exe`) como **Administrador**, caso contrário o Windows não deixará salvar o arquivo na pasta `Program Files`.
5. Arraste o arquivo `.config` para dentro do Bloco de Notas.

**Parâmetros cruciais a serem verificados no `.config`:**
* `<add key="Service.Name" value="PLC_XX" />`: Deve bater com o nome cadastrado no banco.
* `<add key="PLC.Id" value="XX" />`: O *ParentId* que o serviço buscará na tabela `rTagGroup`.
* **ConnectionStrings:** Verifique se as tags `<add name="Connection_AssetManager"...` apontam para a base de dados ativa do Cluster (`PS-DP-DTO-VS01` ou `VS02`).

> 📸 *(Insira aqui o Print do bloco de notas com os destaques vermelhos no arquivo config)*  
> `![Arquivo Config do Driver](../assets/arquivo-config.png)`

### 7.2.3 Como Instalar ou Reiniciar o Serviço no Windows
Se um serviço apresentar falha fatal (Status `'Bad'` no banco), a equipe de manutenção deve reiniciá-lo pelo Windows.

1. Abra o Menu Iniciar e digite **`services.msc`**.
2. Procure pelos serviços iniciados com o nome **`Route - Rotas - OpcClientService...`**.
3. Clique com o botão direito e selecione **`Reiniciar (Restart)`**.

> 📸 *(Insira aqui o Print da lista de serviços do Windows (services.msc))*  
> `![Lista de Serviços do Windows](../assets/lista-servicos-windows.png)`

**Instalando um serviço novo (Prompt de Comando):**
Se um CLP novo foi adicionado à planta, você precisará registrar o serviço no Windows.
1. Abra o CMD como Administrador.
2. Navegue até a pasta do .NET Framework: 
   `cd C:\Windows\Microsoft.NET\Framework\v4.0.30319`
3. Execute o instalador apontando para a pasta do seu novo serviço:
   `InstallUtil.exe "C:\Program Files\Vale\OpcClientService_Novo\Vale.Tops.Integration.OpcClientService.exe"`
4. Após a mensagem de sucesso (*The transacted install has completed*), vá ao `services.msc` e inicie o serviço manualmente.

> 📸 *(Insira aqui o Print da tela preta do CMD mostrando o InstallUtil executando com sucesso)*  
> `![Instalação via CMD](../assets/cmd-installutil.png)`

