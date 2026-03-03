# 1. Visão Geral e Contexto de Negócio

O **Sistema Rotas** é um software de missão crítica que atua como o "controlador de tráfego" do Complexo Portuário de Tubarão (Vitória-ES). Ele é responsável por orquestrar, de forma segura e automatizada, o caminho que o minério de ferro percorre desde a sua chegada nos trens até o embarque nos navios.

Para compreender a arquitetura e o código deste sistema, é fundamental primeiro entender a física da operação portuária e os gigantescos equipamentos envolvidos.

---

## 1.1 O Cenário da Operação (A Jornada do Minério)

O minério de ferro é classificado como **Granel Sólido** (carga transportada em grandes volumes sem embalagem). Ele chega ao porto através de longas composições de trens vindas das minas. 

Ao chegar, esse material não pode simplesmente ser jogado no chão. Ele precisa ser direcionado para três fluxos logísticos principais:
1. **Descarga para Estocagem:** O minério sai do trem e vai para os pátios, formando gigantescas pilhas organizadas por tipo de produto.
2. **Embarque a partir do Estoque:** O minério é retirado das pilhas do pátio e levado até os navios.
3. **Embarque Direto (Trânsito):** O minério sai diretamente do trem para o porão do navio, sem passar pelo estoque.

Para movimentar milhões de toneladas de minério nessas três modalidades, o porto não utiliza caminhões, mas sim uma intrincada malha de **Correias Transportadoras** (esteiras de borracha motorizadas). 

A combinação lógica que conecta uma máquina inicial, passa por várias esteiras e chega a uma máquina final é o que chamamos de **Rota de Minério**. Devido ao tamanho do porto, existem mais de 5.000 rotas possíveis.

---

## 1.2 Os Equipamentos (Os Atores Físicos)

Uma Rota é sempre composta por uma **Origem**, um **Caminho** (com desvios) e um **Destino**. Abaixo estão as máquinas que o Sistema Rotas controla remotamente:

### ⚙️ Equipamentos de Origem (Onde a rota nasce)
* **Viradores de Vagões (VV):** Máquinas colossais que travam dois vagões de trem por vez e os giram de ponta-cabeça (180 graus). O minério cai pela gravidade em grandes funis subterrâneos que alimentam a primeira correia da rota.
* **Recuperadoras:** Máquinas que se movem sobre trilhos nos pátios. Possuem uma enorme "roda de caçambas" giratória na ponta que raspa as pilhas de minério, retirando o material do estoque e jogando-o na esteira para ser exportado.

### 🎯 Equipamentos de Destino (Onde a rota morre)
* **Empilhadeiras:** Máquinas de pátio que recebem o minério vindo das esteiras e o despejam ordenadamente no chão, formando as pilhas de estocagem.
* **Carregadores de Navios:** Estruturas móveis localizadas nos píeres. Elas possuem lanças telescópicas ("trombas") que descem até o porão do navio para despejar o minério com precisão, evitando o desequilíbrio da embarcação.

### 🛣️ Equipamentos de Caminho e Desvio (Os "Roteadores")
* **Correias Transportadoras:** Esteiras motorizadas. Algumas possuem quilômetros de extensão. Algumas são **reversíveis** (podem girar tanto para frente quanto para trás).
* **Cabeças Móveis (Feeders):** São trilhos mecânicos na ponta de uma esteira que se movem lateralmente. Elas escolhem em qual "buraco" (chute) o minério vai cair, definindo qual será a próxima esteira da rota.
* **Dampers:** Placas defletoras (semelhantes a uma cancela) usadas em correias reversíveis. Elas garantem que o minério caia no sentido correto do fluxo, evitando que o material espirre para fora da esteira.
* **Trippers:** Um mecanismo móvel instalado nas esteiras do pátio. Se o *Tripper* estiver na posição **"Alto"**, ele levanta a correia como uma rampa, forçando o minério a subir para a Empilhadeira. Se estiver **"Baixo"**, o minério passa reto por baixo da máquina para seguir viagem até outra esteira.

---

## 1.3 O Propósito do "Sistema Rotas"

Ligar todas essas máquinas através de botões manuais seria um pesadelo logístico. Se o operador ligar uma esteira que manda minério para a direita, mas a *Cabeça Móvel* estiver apontada para a esquerda, toneladas de minério cairão no chão, causando o entupimento de dutos, soterramento de máquinas, queima de motores e a parada do porto por dias. 

O **Sistema Rotas** existe para eliminar a falha humana. Seus três propósitos vitais são:

### 1. Testes de Consistência e Intertravamento (Safety Lógico)
Antes do sistema enviar o sinal elétrico para ligar os motores, o banco de dados lê os sensores das máquinas (via OPC) e faz um *Check-up* de segurança (Consistência):
* **Limite de Carga:** Verifica se a esteira já não está transportando outras rotas simultâneas além da sua capacidade nominal de toneladas/hora.
* **Posição de Cabeças Móveis e Dampers:** Confirma se a gaveta mecânica está fisicamente apontada para o destino desejado pela rota.
* **Posição do Tripper:** Confirma se a rampa da empilhadeira está na posição correta (Alto/Baixo).
* **Sentido da Correia:** Verifica se as correias reversíveis estão programadas para girar na direção correta.
* *Se qualquer máquina estiver na posição errada, a Rota fica com status "Inconsistente" e o botão de acionamento é bloqueado.*

### 2. Automação de Inicialização e "Hot-Swap"
Uma vez autorizada, a rota não liga toda de uma vez. O Sistema Rotas envia comandos para ligar os equipamentos de **trás para frente** (primeiro o Destino, depois o Caminho, por último a Origem). Assim, quando o minério cair, a esteira de baixo já estará em movimento.
* **Rotas Substitutas (Hot-Swap):** O sistema possui inteligência para evitar o desligamento total do porto. Se um trem terminar de descarregar no Virador 1, e o próximo trem estiver no Virador 2, o sistema desliga apenas a origem antiga e liga a nova, mantendo todas as correias transportadoras em comum rodando sem interrupção, economizando tempo e energia elétrica.

### 3. Integração com Faturamento
O sistema realiza cálculos baseados nas leituras de balanças instaladas debaixo das esteiras. Ele apura exatamente quantas toneladas saíram da Origem e chegaram no Destino, enviando essa "nota fiscal" de produção para os sistemas corporativos de alto nível.

## 1.4 Arquitetura e Topologia do Sistema

O Sistema Rotas não é um aplicativo isolado rodando em um único computador. Por se tratar de uma operação crítica (onde uma falha de comunicação pode parar o porto), ele foi desenhado com uma arquitetura distribuída, separando as responsabilidades entre o Centro de Controle, os Servidores e o Chão de Fábrica.

> 📸 *(Topologia física e lógica da rede de automação do porto)*
> ![Arquitetura do Sistema](../assets/arquitetura-sistema.png)

Abaixo, explicamos o papel de cada bloco representado no diagrama acima:

### 🖥️ 1. CCO (Centro de Controle Operacional)
* **O que é:** É a sala onde os operadores ficam posicionados acompanhando os monitores. 
* **O Componente (5 App Cliente):** Representa as 5 máquinas físicas rodando a interface gráfica (WPF) do Sistema Rotas. Esses aplicativos clientes se conectam apenas ao **Servidor 1** para consultar o Banco de Dados. Nenhum cliente se comunica diretamente com a planta, garantindo segurança contra comandos acidentais e ataques à rede industrial.

### 🗄️ 2. Servidor 1 (O "Cérebro" e o Banco de Dados)
Este servidor é a espinha dorsal do sistema. Ele abriga dois componentes críticos:
* **BD (Banco de Dados SQL Server):** É o cérebro lógico. Ele recebe os cliques dos 5 operadores do CCO, roda as validações de segurança (*Views/Stored Procedures*) e guarda todo o histórico de produção. 
* **12 Drivers Clientes OPC:** São 12 *Windows Services* independentes rodando no background deste servidor. Cada serviço é "dono" de um PLC específico.
* **RSLinx (OPC Server):** É um software de prateleira da Rockwell. Ele funciona como um "tradutor". Os 12 Drivers enviam comandos lógicos para o RSLinx, e o RSLinx traduz isso para a linguagem elétrica que a rede de automação entende.

### 🛡️ 3. Servidor 2 (O Servidor de Apoio)
* **O que é:** Para aliviar a carga de processamento do Servidor 1, existe um segundo servidor de apoio.
* **O Componente:** Ele não possui um Banco de Dados próprio (ele aponta para o BD do Servidor 1). Sua única função é abrigar mais **10 Drivers Clientes OPC** e seu próprio **RSLinx**.
* **Vantagem:** Se o Servidor 2 reiniciar, o porto não para completamente. Apenas as 10 máquinas controladas por ele ficarão em modo de segurança até o sistema voltar, enquanto as outras 12 (do Servidor 1) continuarão operando normalmente.

### 🏭 4. Planta (O Chão de Fábrica)
* **O que é:** O ambiente físico (ao ar livre), onde estão as correias, os navios e os motores.
* **O Componente (22 PLCs):** PLC (*Programmable Logic Controller*) é o computador industrial de carcaça reforçada que fica dentro dos painéis elétricos ao lado das esteiras. No diagrama, vemos **12 PLCs** conectados ao Servidor 1 e **10 PLCs** conectados ao Servidor 2. Eles são a "ponta da linha": recebem o comando do RSLinx e aplicam a tensão (voltagem) nos cabos que ligam os motores gigantes.
