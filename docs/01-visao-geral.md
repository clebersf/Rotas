# 1. Visão Geral e Contexto de Negócio

O **Sistema Rotas** é um software de missão crítica que atua como o "controlador de tráfego" de um complexo portuário de minério de ferro. Ele é o responsável por ligar, desligar e monitorar, de forma segura e automatizada, o caminho que o minério de ferro percorre desde o momento em que chega de trem até o momento em que é colocado no navio.

Para entender o software, é fundamental primeiro entender como funciona a operação física do porto.

---

## 1.1 O Cenário da Operação (A Jornada do Minério)

Imagine um porto de mineração como um imenso labirinto de estradas. O minério de ferro chega das minas através de longos trens de carga. Ao chegar no porto, esse material precisa ser descarregado e levado para dois destinos possíveis:
1. **Estocagem:** Guardado em gigantescas pilhas no pátio para ser exportado depois.
2. **Embarque Direto:** Colocado diretamente no porão de um navio que está atracado no píer.

Para transportar milhares de toneladas de minério de um ponto a outro sem usar caminhões, o porto utiliza uma rede massiva de **Correias Transportadoras** (esteiras gigantes). O agrupamento de máquinas que se conectam para levar o minério de um Ponto A para um Ponto B é o que chamamos de **Rota**.

---

## 1.2 Os Equipamentos (Atores do Sistema)

Uma Rota é composta por três partes principais: uma Origem, um Caminho e um Destino. Abaixo estão os equipamentos físicos reais que o "Sistema Rotas" controla:

### ⚙️ As Origens (Onde o minério entra na rota)
* **Viradores de Vagões (VV):** Máquinas colossais que agarram os vagões do trem e os giram de cabeça para baixo, despejando o minério nas correias subterrâneas.
* **Recuperadoras:** Máquinas com rodas de caçambas gigantes que ficam nos pátios. Elas "raspam" o minério que estava estocado nas pilhas e o jogam nas correias para ser levado ao navio.

### 🛣️ O Caminho (Por onde o minério passa)
* **Correias Transportadoras (Conveyors):** São as esteiras de borracha movidas a grandes motores elétricos. Algumas possuem quilômetros de extensão.
* **Equipamentos de Direcionamento (Trippers, Dampers, Feeders):** São os "desvios de trilho". Como uma correia cruza com várias outras, essas máquinas mecânicas (chutes, gavetas, alimentadores) definem se o minério vai seguir reto, virar à direita ou cair para a esteira de baixo.

### 🎯 Os Destinos (Onde o minério é entregue)
* **Empilhadeiras (Stackers):** Máquinas de pátio que recebem o minério da correia e o despejam no chão, formando as pilhas de estoque organizadas por tipo de material.
* **Carregadores de Navios (Ship Loaders):** Equipamentos monstruosos localizados no píer. Eles possuem uma "tromba" (lança) que desce até o porão do navio, despejando o minério com precisão para não desequilibrar a embarcação.

---

## 1.3 O Propósito do "Sistema Rotas"

Ligar todas essas máquinas manualmente seria um pesadelo logístico e extremamente perigoso. Se um operador ligar a esteira 1, mas esquecer de ligar a esteira 2 que vem logo à frente, toneladas de minério cairão no chão, causando soterramento de máquinas, paradas de dias e prejuízos milionários. Além disso, misturar dois tipos diferentes de minério (cruzamento de rotas) arruína a qualidade do produto vendido ao cliente.

É aqui que o **Sistema Rotas** entra. Seus três propósitos fundamentais são:

1. **Garantir a Segurança (Intertravamento Lógico):** 
   O sistema possui regras rígidas no banco de dados. Antes de permitir que uma rota funcione, ele verifica eletronicamente se os desvios (Trippers/Dampers) estão virados para o lado certo, se não há outra máquina operando no mesmo espaço físico (risco de colisão) e se os equipamentos estão com falhas.

2. **Automatizar a Partida e Parada:**
   Quando o operador aprova a rota na tela, o software assume o controle via protocolo industrial (OPC DA). Ele toca as sirenes de alerta no pátio e liga os motores em uma **sequência reversa estrita** (primeiro liga o destino, depois as correias, e por último a origem). Isso garante que quando o minério cair, a esteira de baixo já estará rodando.

3. **Rastreabilidade e Faturamento (Produção):**
   Ao longo do caminho, o sistema lê as Balanças (instrumentos instalados debaixo das correias) para calcular em tempo real quantas toneladas de minério passaram por ali. Essa informação é enviada aos sistemas corporativos (GPV) para fechamento de relatórios e faturamento dos navios.

Resumindo: O Sistema Rotas é a ponte que transforma o planejamento em ação física, garantindo que o minério flua pela "cidade" de esteiras sem acidentes, de forma ágil e totalmente monitorada.
