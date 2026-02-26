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
