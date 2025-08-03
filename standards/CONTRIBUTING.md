# 🤝 Como Contribuir com Código para a API de Vendas

Bem-vindo(a) ao guia de contribuição de código do projeto **API de Vendas (Ambev.DeveloperEvaluation)**! Agradecemos o seu interesse em colaborar conosco para construir uma API robusta, escalável e bem documentada.

Este documento detalha o fluxo de trabalho para contribuições de código, as boas práticas esperadas e os padrões de qualidade que regem o desenvolvimento deste projeto.

## Sumário

1.  [Processo de Contribuição de Código](#1-processo-de-contribuição-de-código)
    * [1.1. Configuração do Ambiente](#11-configuração-do-ambiente)
    * [1.2. Fluxo de Trabalho do Git](#12-fluxo-de-trabalho-do-git)
    * [1.3. Boas Práticas de Desenvolvimento](#13-boas-práticas-de-desenvolvimento)
    * [1.4. Testes](#14-testes)
    * [1.5. Documentação no Código (XML Comments)](#15-documentação-no-código-xml-comments)
    * [1.6. Padrões de Commit Messages](#16-padrões-de-commit-messages)
    * [1.7. Criando um Pull Request (PR)](#17-criando-um-pull-request-pr)
2.  [Revisão de Código](#2-revisão-de-código)
3.  [Reconhecimento da Contribuição](#3-reconhecimento-da-contribuição)

---

## 1. Processo de Contribuição de Código

Para contribuir com código, siga o fluxo de trabalho detalhado abaixo.

### 1.1. Configuração do Ambiente

Antes de iniciar, certifique-se de que seu ambiente esteja devidamente configurado. Consulte o guia `README.md` para instruções detalhadas sobre a instalação de pré-requisitos, configuração de secrets e execução local das dependências (como o banco de dados PostgreSQL via Docker).

### 1.2. Fluxo de Trabalho do Git

Utilizamos um fluxo de trabalho baseado em *feature branches* (GitFlow) e *pull requests* para gerenciar o desenvolvimento e garantir a estabilidade da branch `main`.

1.  **Faça um *fork* do Repositório:** Clique no botão "Fork" no GitHub para criar uma cópia do repositório em sua conta pessoal.
2.  **Clone o seu *fork*:**
    ```bash
    git clone https://github.com/SEU-USUARIO/sales-api.git
    cd sales-api
    ```
3.  **Adicione o repositório original como 'upstream' remoto:**
    Isso permitirá que você sincronize facilmente seu *fork* com as últimas mudanças do projeto principal.
    ```bash
    git remote add upstream https://github.com/wnlima/sales-api.git
    ```
4.  **Mantenha sua branch `main` atualizada:**
    Sempre antes de começar a trabalhar, atualize sua branch `main` local:
    ```bash
    git checkout main
    git pull upstream main
    ```
5.  **Crie uma nova *feature branch*:**
    Crie uma branch com um nome descritivo. Use o formato `feature/[nome-da-feature]` ou `bugfix/[nome-da-correcao]` (ex: `feature/registrar-nova-venda`, `bugfix/corrigir-calculo-de-desconto`).
    ```bash
    git checkout -b feature/sua-nova-funcionalidade
    ```
6.  **Desenvolva suas mudanças.**
7.  **Faça commits atômicos e descritivos.** (Veja [Padrões de Commit Messages](#16-padrões-de-commit-messages))
8.  **Faça *push* da sua *feature branch* para o seu *fork*:**
    ```bash
    git push origin feature/sua-nova-funcionalidade
    ```
9.  **Abra um Pull Request (PR)** (Veja [Criando um Pull Request (PR)](#17-criando-um-pull-request-pr)).

### 1.3. Boas Práticas de Desenvolvimento

Aderimos a um conjunto rigoroso de boas práticas para garantir a qualidade do código.

* **SOLID Principles:** Aplicação rigorosa dos princípios SOLID.
* **Design Patterns:** Utilização de padrões apropriados (Repository, Unit of Work, Mediator).
* **Clean Architecture:** Organização do código em camadas claras (Domain -> Application -> Infrastructure -> Api).
* **Domain-Driven Design (DDD):** Foco no domínio de negócio, com entidades, agregados e eventos de domínio bem definidos.
* **CQRS (Command Query Responsibility Segregation):** Separação clara entre comandos e queries, implementada com MediatR.
* **Consistência:** Mantenha a consistência com o estilo de código e as convenções já existentes no projeto.
* **Tratamento de Erros:** Implemente um tratamento de erros robusto, utilizando o middleware de exceções e respostas padronizadas da API.

Consulte as [🧑‍💻 Diretrizes de Codificação (CODING_GUIDELINES.md)](CODING_GUIDELINES.md) para mais detalhes.

### 1.4. Testes

A qualidade do código é fundamental, e os testes são uma parte essencial do nosso processo.

* **Tipos de Testes:**
    * **Testes Unitários:** Focam em unidades isoladas de código (entidades de domínio, handlers do MediatR). Devem ser rápidos e não depender de infraestrutura externa.
    * **Testes de Integração:** Verificam a interação entre componentes (ex: aplicação e banco de dados).
* **Cobertura de Testes:** Buscamos alta cobertura. Todas as novas funcionalidades e correções devem ser acompanhadas por testes relevantes.
* **Executando Testes:** Para rodar todos os testes automatizados da solução:
    ```bash
    dotnet test
    ```
    Para mais detalhes sobre a estratégia de testes, consulte o `TESTING_GUIDELINES.md`.

### 1.5. Documentação no Código (XML Comments)

A documentação no código é um pilar da manutenibilidade. Todo o código C# público (classes, interfaces, métodos, propriedades) DEVE incluir documentação XML.

* **Padrão Mandatório:** Utilize as tags XML de documentação do .NET (`<summary>`, `<remarks>`, `<param>`, `<returns>`).
* **Clareza e Utilidade:** Comentários devem ser claros, concisos e agregar valor, explicando o "porquê" e o "o quê", especialmente para lógica de negócio complexa.
* **Referências a Requisitos:** A tag `<remarks>` pode ser usada para referenciar explicitamente os requisitos atendidos.

### 1.6. Padrões de Commit Messages

Adotamos o padrão [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) para garantir commits significativos e padronizados.

Exemplos de tipos de commit:

* `feat`: Uma nova funcionalidade (ex: `feat: adicionar endpoint para registro de venda`)
* `fix`: Uma correção de bug (ex: `fix: corrigir calculo de total da venda`)
* `docs`: Alterações apenas na documentação (ex: `docs: atualizar guia de contribuicao`)
* `chore`: Mudanças de manutenção que não afetam a funcionalidade (ex: `chore: atualizar dependencias do nuget`)
* `refactor`: Refatoração de código sem mudança de comportamento (ex: `refactor: refatorar entidade Sale para aplicar regras`)
* `test`: Adição ou modificação de testes (ex: `test: adicionar testes unitarios para SaleItem`)
* `perf`: Melhoria de performance (ex: `perf: otimizar consulta de produtos por id`)
* `build`: Mudanças no sistema de build ou dependências (ex: `build: configurar pipeline de CI/CD`)

### 1.7. Criando um Pull Request (PR)

Quando suas mudanças estiverem prontas e testadas, siga estes passos:

1.  **Sincronize sua branch:**
    Certifique-se de que sua *feature branch* esteja atualizada com as últimas mudanças da `main` para evitar conflitos.
    ```bash
    git pull upstream main
    ```
    Resolva quaisquer conflitos de merge que possam surgir.

2.  **Faça *push* final:**
    ```bash
    git push origin feature/sua-nova-funcionalidade
    ```

3.  **Abra um Pull Request:** Vá para a página do repositório no GitHub e crie um novo Pull Request da sua *feature branch* para a branch `main`.

4.  **Descrição do PR:** Preencha a descrição do PR com:
    * **Título Claro:** Um título conciso que resuma as mudanças.
    * **Descrição Detalhada:** Explique o "o quê" e o "porquê" das mudanças.
    * **Testes:** Mencione como as mudanças foram testadas.
    * **Checklist:** Inclua um checklist para garantir que todos os pontos de qualidade foram abordados (ex: `[x] Testes escritos`, `[x] Documentação atualizada`, `[x] Padrões de código seguidos`).

## 2. Revisão de Código

Todos os Pull Requests passarão por uma revisão de código. O objetivo é:

* Garantir a qualidade, segurança e manutenibilidade do código.
* Compartilhar conhecimento e disseminar as boas práticas.
* Identificar bugs ou oportunidades de melhoria.
* Garantir a adesão às diretrizes do projeto.

Esteja aberto(a) a feedback e discussões construtivas.

## 3. Reconhecimento da Contribuição

Todas as contribuições são valorizadas! Se seu Pull Request for aceito, seu nome será reconhecido.

Agradecemos novamente por sua colaboração!
