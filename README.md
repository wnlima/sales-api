# Projeto E-commerce: Gerenciamento de Vendas, Produtos e Usuários
![C4 Model - Contexto](/docs/diagrams/C4-Context.png)
*Diagrama de Contexto*
![C4 Model - Containers](/docs/diagrams/C4-Containers.png)
 *Diagrama de Containers*

## 🚀 Visão Geral do Projeto

O **Projeto E-commerce** é uma iniciativa para desenvolver uma API de e-commerce robusta, escalável e resiliente, com foco no gerenciamento de vendas, produtos e usuários. Desenvolvido com uma arquitetura de **Microsserviços** e utilizando as tecnologias **C\#/.NET 8+**, o projeto visa fornecer uma plataforma sólida para o núcleo de um sistema de vendas online. Nosso objetivo é construir uma solução de alta performance que possa ser facilmente expandida e integrada a outros sistemas, como de pagamentos, entrega e controle de fraude.

Este repositório contém o código-fonte do **Produto Mínimo Viável (MVP)**, implementado como uma prova de conceito (POC) para validar a arquitetura proposta e as principais funcionalidades de e-commerce.

## ✨ Destaques da Arquitetura e Tecnologia

  * **Arquitetura:** Microsserviços com Domain-Driven Design (DDD), CQRS e Padrão Saga.
  * **Linguagem & Framework:** C\# / .NET 8+.
  * **Bancos de Dados:** PostgreSQL (exclusivamente).
  * **Comunicação Assíncrona:** RabbitMQ como Message Broker, essencial para a orquestração de transações com o Padrão Saga.
  * **Cache & Resiliência:** Redis para otimização de performance e gerenciamento de estado.
  * **Orquestração:** Conteinerização com Docker e preparação para implantação em Kubernetes.
  * **Infraestrutura como Código (IaC):** Terraform para gestão de infraestrutura multi-cloud.
  * **Segurança:** Autenticação via JWT, gerenciamento de senhas com `BCryptPasswordHasher` e Secure by Design.
  * **Observabilidade:** Preparação para OpenTelemetry para traces, logs e métricas, visando integração com ferramentas de mercado (Datadog, Elastic, Dynatrace).

## 📖 Sumário da Documentação

Para facilitar a navegação e o entendimento do projeto, consulte os seguintes documentos essenciais:

  * **🏛️ Decisões Arquiteturais Chave (/ARCHITECTURE\_DECISIONS.md):** Um resumo conciso das principais escolhas de arquitetura e padrões de design do projeto, explicando o *porquê* de cada decisão.
      * [🏛️ ARQUITETURA\_DECISIONS.md](/ARCHITECTURE_DECISIONS.md)
  * **Visão Estratégica Completa (PDF):** Aprofunde-se no documento estratégico principal que detalha o escopo, requisitos de negócio e a arquitetura geral da solução.
      * [📄 Developer Evaluation - Visão Estratégica e Arquitetura da Solução.pdf](/Developer Evaluation - Visão Estratégica e Arquitetura da Solução.pdf)
  * **Guias Essenciais para Desenvolvedores:**
      * [🚀 Inicie Aqui\! (GET\_STARTED.md)](/standards/GET_STARTED.md): Guia passo a passo para configurar seu ambiente de desenvolvimento e executar o projeto localmente.
      * [🤝 Como Contribuir (CONTRIBUTING.md)](/standards/CONTRIBUTING.md): Entenda nosso fluxo de trabalho, padrões de commit e o processo para Pull Requests.
      * [🧑‍💻 Diretrizes de Codificação (CODING\_GUIDELINES.md)](/standards/CODING_GUIDELINES.md): Conheça os padrões de código C\#/.NET, boas práticas e princípios de design aplicados no projeto.
      * [🧪 Diretrizes de Testes (TESTING\_GUIDELINES.md)](/standards/TESTING_GUIDELINES.md): Saiba mais sobre nossa estratégia de testes, os tipos de testes (unitários, integração), e as políticas de cobertura de código.
  * **Detalhes da Infraestrutura:**
      * [⚙️ Infraestrutura do Projeto (INFRASTRUCTURE.md)](/standards/INFRASTRUCTURE.md): Documentação sobre a configuração de Docker, Kubernetes, Infraestrutura como Código com Terraform e a estratégia multi-cloud.
  * **Documentação dos Microsserviços:**
      * [👤 Serviço de Usuários (README.md)](/docs/users-api.md): Documentação detalhada sobre o microsserviço responsável pelo gerenciamento de usuários e autenticação.
      * [📦 Serviço de Produtos (README.md)](/docs/products-api.md): Documentação específica do microsserviço que gerencia o catálogo de produtos.
      * [🛒 Serviço de Vendas (README.md)](/docs/sales-api.md): Documentação específica do microsserviço que gerencia o ciclo de vida das vendas.

## 🏛️ Estrutura do Repositório

Este é um *monorepo* que organiza os diferentes microsserviços, bibliotecas compartilhadas e a documentação do projeto, conforme a estrutura abaixo:

```
.
├── .github/                                  # Configurações do GitHub Actions
│   └── workflows/                            # Pipelines de CI/CD (ci-pipeline.yml)
├── docs/                                     # Documentação de alto nível, diagramas e decisões arquiteturais
│   ├── project-structure.md                  # Estrutura do projeto
│   ├── tech-stack.md                         # Stack de tecnologia
│   ├── users-api.md                          # Documentação do microsserviço de usuários
│   ├── products-api.md                       # Documentação do microsserviço de produtos
├── backend/                                  # Código-fonte do backend
│   ├── src/
│   │   ├── Users/
│   │   ├── Products/
│   │   └── Sales/
│   ├── tests/
│   └── docker-compose.yml                    # Arquivo para orquestração local de serviços (PostgreSQL, RabbitMQ, Redis)
├── standards/                                # Padrões organizacionais e diretrizes
├── README.md                                 # Este arquivo
├── coverage-report.sh                        # Script para geração de relatório de cobertura de testes
```

## ▶️ Rodando o Projeto Localmente (POC)

Para instruções detalhadas sobre como configurar seu ambiente de desenvolvimento e executar os microsserviços e suas dependências (PostgreSQL, RabbitMQ) localmente usando Docker, consulte o guia:

  * **[🚀 Inicie Aqui\! (GET\_STARTED.md)](/standards/GET_STARTED.md)**

### Acesso às APIs (Swagger)

Com os serviços rodando localmente:

  * **Serviço de Usuários API:** `http://localhost:5000/swagger` (ou a porta configurada em `launchSettings.json` ou no `docker-compose.yml`)
  * **Serviço de Produtos API:** `http://localhost:5001/swagger` (ou a porta configurada)
  * **Serviço de Vendas API:** `http://localhost:5002/swagger` (ou a porta configurada)

## 🧪 Executando Testes

Para executar os testes unitários e de integração de todos os serviços e gerar relatórios de cobertura:

  * **[🧪 Diretrizes de Testes (TESTING\_GUIDELINES.md)](/standards/TESTING_GUIDELINES.md)**

Resumidamente, na pasta `backend/`:

```bash
./coverage-report.sh
```

## 🔒 Gerenciamento de Dados Sensíveis

**ATENÇÃO:** No ambiente de desenvolvimento local, utilize o **User Secrets** do .NET. Para ambientes de Produção, é **mandatório** o uso de soluções de *Secrets Management* da nuvem (ex: Azure Key Vault, AWS Secrets Manager). Consulte as [🧑‍💻 Diretrizes de Codificação](/standards/CODING_GUIDELINES.md) para mais detalhes.

## ⚙️ Pipelines de CI/CD (GitHub Actions)

Este repositório utiliza GitHub Actions para Integração Contínua (CI). O pipeline (`.github/workflows/ci-pipeline.yml`) automatiza:

  * Build dos projetos.
  * Execução de testes e verificação de cobertura mínima.
  * (Futuras etapas) Análise estática, build de imagens Docker e deployment.

-----

## 📞 Contato

Para dúvidas, sugestões ou suporte, entre em contato com [Willian Lima](https://www.linkedin.com/in/w-lima).

![Perfil do LinkedIn](https://media.licdn.com/dms/image/v2/D4D03AQGRObzA0_NRkg/profile-displayphoto-shrink_200_200/profile-displayphoto-shrink_200_200/0/1703104875697?e=1751500800&v=beta&t=jWwem7-YUYxBoktc3ayzIMLMdT4RlMQcsh-WlFW0pTM)