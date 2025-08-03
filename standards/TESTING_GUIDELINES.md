# 🧪 Diretrizes de Testes e Cobertura de Código

Este documento descreve a estratégia de testes automatizados e as diretrizes de cobertura de código para o projeto **API de Vendas (Ambev.DeveloperEvaluation)**. A qualidade é um pilar fundamental, e a automação de testes é essencial para garantir a robustez, a confiabilidade e a capacidade de evolução da API.

## 🎯 Filosofia de Testes

Nossa filosofia de testes é baseada na **Pirâmide de Testes**, onde a maior parte dos testes é composta por testes rápidos e baratos (unitários), complementados por testes de integração e funcionais.

* **Rapidez:** Testes devem ser executados rapidamente para fornecer feedback contínuo.
* **Confiabilidade:** Testes devem ser determinísticos e confiáveis, não falhando intermitentemente (*flaky tests*).
* **Manutenibilidade:** Testes devem ser fáceis de escrever, ler e manter.
* **Cobertura:** Testes devem cobrir a lógica de negócio crítica e os fluxos mais importantes da aplicação.

## 📊 Tipos de Testes e Ferramentas

Utilizamos diferentes tipos de testes para garantir a qualidade em todas as camadas da aplicação.

### 1. Testes Unitários

* **Propósito:** Validar a menor unidade de código isoladamente (ex: uma classe, um método, uma entidade de domínio). Testes unitários **não devem ter dependências externas** como banco de dados ou APIs.
* **Foco:** Lógica de negócio pura, algoritmos, validações internas de domínio.
* **Ferramentas:**
    * **Framework de Teste:** `xUnit`.
    * **Mocks/Fakes:** `NSubstitute` para criar dublês de teste e isolar dependências.
    * **Geração de Dados:** `Bogus` (Faker) para criar dados de teste realistas.
* **Localização:** `tests/Ambev.DeveloperEvaluation.Unit/`.
* **Cobertura de Código Alvo:**
    * **Mínimo de 80% de cobertura** para a lógica de negócio nas camadas de **Domínio** e **Aplicação**.

### 2. Testes de Integração

* **Propósito:** Validar a interação entre diferentes componentes internos do sistema, principalmente a integração da camada de aplicação com a infraestrutura (banco de dados).
* **Foco:** Fluxos de dados, comunicação entre a aplicação e o banco de dados (via repositórios do EF Core).
* **Ferramentas:**
    * `xUnit` com a capacidade de inicializar um banco de dados real (geralmente em um contêiner Docker) para garantir que as consultas e comandos do EF Core funcionem como esperado.
* **Localização:** `tests/Ambev.DeveloperEvaluation.Integration/`.

### 3. Testes Funcionais / End-to-End (E2E)

* **Propósito:** Validar o sistema completo de ponta a ponta, simulando cenários de cliente real através da interface externa (API REST).
* **Foco:** Fluxos completos de negócio, testando os *controllers*, a serialização, a autenticação/autorização e a resposta HTTP.
* **Ferramentas:**
    * `Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory` para hospedar a API em memória e fazer chamadas HTTP reais aos endpoints.
* **Localização:** `tests/Ambev.DeveloperEvaluation.Functional/`.

## 📈 Cobertura de Código

A cobertura de código é uma métrica importante para identificar áreas não testadas.

* **Política de Cobertura:**
    * **Objetivo Mínimo:** Atingir **80% de cobertura de linha** para os projetos `.Domain` e `.Application`.
    * **Obrigatoriedade em CI:** A verificação de cobertura será parte do pipeline de Integração Contínua. *Pull Requests* (PRs) que diminuam a cobertura podem ser bloqueados até que a cobertura seja restaurada.
* **Ferramentas:**
    * **`coverlet.collector`:** Ferramenta padrão para coletar métricas de cobertura com `dotnet test`.
    * **Exemplo de Execução com Cobertura:**
        ```bash
        dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults
        ```

## 🚀 Executando Testes e Verificando Cobertura

### 1. Rodar Todos os Testes

Para executar todos os testes da solução, a partir da raiz do projeto:

```bash
dotnet test
```

### 2. Rodar Testes de um Projeto Específico

Para rodar testes de um projeto específico (ex: testes unitários):

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Unit/
```

### 3. Gerar Relatório de Cobertura HTML Localmente

Para facilitar a análise da cobertura localmente com um relatório HTML, utilize os scripts fornecidos na raiz do projeto.

1.  Certifique-se de que o **ReportGenerator** esteja instalado como uma ferramenta global do .NET (se não estiver, execute: `dotnet tool install --global ReportGenerator`).
2.  Abra seu terminal na **raiz do repositório**.
3.  Execute o script apropriado para seu sistema operacional:
    * **Linux/macOS:**
        ```bash
        ./coverage-report.sh
        ```
    * **Windows:**
        ```bash
        .\coverage-report.bat
        ```
4.  Após a execução, o relatório HTML estará disponível na pasta `CoverageReport`. Abra o arquivo `index.htm` em seu navegador.

## 🔄 Integração Contínua (CI)

Os testes automatizados e a verificação de cobertura de código são partes integrantes do nosso pipeline de CI (ex: GitHub Actions). Qualquer *Pull Request* deve passar por essas verificações para ser integrado à branch `main`, garantindo que novas contribuições mantenham os padrões de qualidade.
