# 1. Visão Geral e Base Teórica

O **Sistema Rotas** converte o layout físico de um porto de minério de ferro em um **grafo direcionado**, onde o roteamento ótimo é calculado e executado de forma automatizada e segura.

## 1.1 Contexto de Negócio
O processo de embarque exige a movimentação do produto desde a chegada nos vagões até o carregamento nos navios ou estocagem nos pátios. 
* **Origens (Nós Iniciais):** Viradores de Vagões, Recuperadoras.
* **Destinos (Nós Finais):** Empilhadeiras, Carregadores de Navios, Usinas.
* **Caminhos (Arestas):** Correias Transportadoras (unidirecionais ou reversíveis).
* **Roteadores Lógicos:** Alimentadores Móveis (Feeders) e Trippers.

## 1.2 Base Matemática (Dissertação de Mestrado)
A lógica por trás do roteamento inteligente da aplicação é fundamentada na dissertação de *Cleber Silva Ferreira (2021)*.

O problema de negócio foi modelado utilizando **Programação Linear Inteira Mista (PLIM)** e uma heurística baseada no método **VNS (Variable Neighborhood Search)**. O objetivo da modelagem é encontrar o caminho ótimo que minimize:
1. **$tmax$:** O tempo total de atendimento da lista de demandas de produção.
2. **Consumo de Energia:** Escolhendo rotas elétricas mais eficientes.
3. **Probabilidade de Falhas:** Evitando gargalos operacionais baseados no histórico de quebra de componentes.
