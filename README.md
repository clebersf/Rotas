# Sistema Rotas - Terminal Operations System (TOPS)

[![.NET Framework](https://img.shields.io/badge/.NET-4.6.1-blue.svg)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/EF-Code_First-orange.svg)](https://learn.microsoft.com/en-us/ef/ef6/)
[![WPF](https://img.shields.io/badge/UI-WPF-blueviolet.svg)](#)

O **Sistema Rotas** é um módulo de missão crítica do ecossistema de Automação Portuária. Ele gerencia, valida e automatiza o roteamento de minério de ferro através de uma rede de ativos físicos (esteiras, viradores de vagões, empilhadeiras, navios).

Atua como ponte direta entre o planejamento de produção (Tático) e a automação de chão de fábrica (Operacional), convertendo algoritmos matemáticos (VNS) em sinais elétricos via protocolo OPC DA.

## ⚙️ Tecnologias e Arquitetura
* **Backend / UI:** C# .NET Framework 4.6.1 / 4.7.2, WPF (Windows Presentation Foundation)
* **Banco de Dados:** Microsoft SQL Server (Motor de Consistência e Intertravamento)
* **Persistência:** Entity Framework 6.4 (Code-First) com Padrão Repository e CQRS
* **Integração Industrial:** Windows Service atuando como cliente OPC DA (Interop/COM)
* **Injeção de Dependência:** Autofac

## 📖 Documentação Completa (Wiki)
A arquitetura detalhada, diagramas UML, modelagem de dados e regras de negócio estão documentados na Wiki do projeto. Acesse pelos links abaixo:

1. [Visão Geral e Base Teórica (Algoritmo VNS)](link-para-a-wiki)
2. [Modelo de Domínio e Banco de Dados (Entity Framework)](link-para-a-wiki)
3. [Motor de Segurança e Consistência (SQL Server)](link-para-a-wiki)
4. [Camada de Apresentação (WPF UI)](link-para-a-wiki)
5. [Integração Industrial (OpcClientService)](link-para-a-wiki)

## 🚀 Fluxo de Vida (Da Interface à Máquina)
*(Copie aqui apenas o último diagrama de sequência `mermaid` da seção 9 da documentação gerada anteriormente)*
