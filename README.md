# Sistema Rotas - Terminal Operations System (TOPS)

[![.NET Framework](https://img.shields.io/badge/.NET-4.6.1-blue.svg)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/EF-Code_First-orange.svg)](https://learn.microsoft.com/en-us/ef/ef6/)
[![WPF](https://img.shields.io/badge/UI-WPF-blueviolet.svg)](#)

O **Sistema Rotas** é um módulo de missão crítica do ecossistema de Automação Portuária (Terminal de Minério de Ferro em Vitória-ES). Ele gerencia, valida e automatiza o roteamento de minério de ferro através de uma complexa rede de ativos físicos (esteiras, viradores de vagões, empilhadeiras, carregadores de navios).

Ele atua como a ponte direta entre o planejamento de produção (Tático) e a automação de chão de fábrica (Operacional), convertendo algoritmos de otimização em sinais elétricos via protocolo OPC DA.

## ⚙️ Tecnologias Utilizadas
* **Backend / UI:** C# .NET Framework 4.6.1 / 4.7.2, WPF (Windows Presentation Foundation)
* **Banco de Dados:** Microsoft SQL Server
* **Persistência:** Entity Framework 6.4.4 (Code-First) com Padrão Repository e CQRS
* **Integração Industrial:** Windows Service atuando como cliente OPC DA (Interop/COM)
* **Injeção de Dependência:** Autofac

## 📖 Documentação da Arquitetura

A documentação detalhada da solução foi dividida por módulos para facilitar a leitura e manutenção. Acesse os links abaixo:

1. [Visão Geral e Base Matemática (VNS)](./docs/01-visao-geral.md)
2. [Modelo de Domínio e Entity Framework](./docs/02-modelo-dominio.md)
3. [Motor de Segurança e Banco de Dados](./docs/03-banco-dados.md)
4. [Camada de Apresentação (WPF UI)](./docs/04-interface-wpf.md)
5. [Integração Industrial (OpcClientService)](./docs/05-opc-client.md)
6. [Integração Industrial (OpcClientService)](./docs/05-opc-client.md)
7. [Integração Industrial (OpcClientService)](./docs/05-opc-client.md)
---
*Projeto baseado na modelagem matemática e heurística da dissertação de Cleber Silva Ferreira (2021).*
