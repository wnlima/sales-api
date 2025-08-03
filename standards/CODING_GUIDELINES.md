# 🧑‍💻 Diretrizes de Codificação C#/.NET para o Projeto de API de Vendas

Este documento estabelece as diretrizes e padrões de codificação para o desenvolvimento do projeto **API de Vendas (Ambev.DeveloperEvaluation)** em C#/.NET 8+. Aderir a estas diretrizes é crucial para garantir a consistência, manutenibilidade, escalabilidade e segurança do nosso codebase.

## 🎯 Princípios Fundamentais

Nossas diretrizes de codificação são baseadas nos seguintes princípios:

* **Clareza e Legibilidade:** O código deve ser fácil de ler, entender e depurar por qualquer desenvolvedor da equipe.
* **Simplicidade:** Prefira soluções simples e diretas em vez de abordagens excessivamente complexas ou super-engenheiradas.
* **Manutenibilidade:** O código deve ser fácil de modificar, depurar e estender no futuro, minimizando o impacto de alterações.
* **Testabilidade:** O design do código deve facilitar a escrita e execução de testes automatizados (unitários, integração), promovendo a validação contínua.
* **Segurança:** A segurança deve ser uma preocupação inerente desde a fase de design (Secure by Design) até a implementação e operação.
* **Performance:** Otimize o código onde o desempenho for criticamente importante, mas sempre buscando um equilíbrio com a legibilidade e manutenibilidade.

## 🧱 Padrões de Design e Arquitetura

O projeto adota padrões arquiteturais e de design específicos para garantir a robustez, escalabilidade e alinhamento com o domínio de negócio:

* **Domain-Driven Design (DDD):**
    * Foco na modelagem de um domínio de negócio rico, com a **linguagem ubíqua** bem definida e refletida no código.
    * Uso de **Agregados, Entidades, Value Objects e Domain Events** para encapsular e proteger a lógica de negócio, garantir a consistência e a rastreabilidade das operações.
    * Separação clara entre as camadas de domínio, aplicação, infraestrutura e apresentação para isolar preocupações.
* **CQRS (Command Query Responsibility Segregation):**
    * Separação explícita entre operações de **Comando (escrita)** e **Consulta (leitura)**.
    * Uso de **MediatR** para despachar comandos e consultas, promovendo um design mais limpo, desacoplado e testável.
* **Princípios SOLID:**
    * **Single Responsibility Principle (SRP):** Cada classe, módulo ou componente deve ter uma única razão para mudar.
    * **Open/Closed Principle (OCP):** Entidades de software devem ser abertas para extensão, mas fechadas para modificação.
    * **Liskov Substitution Principle (LSP):** Objetos de uma superclasse devem ser substituíveis por objetos de suas subclasses sem afetar a corretude do programa.
    * **Interface Segregation Principle (ISP):** Clientes não devem ser forçados a depender de interfaces que não utilizam.
    * **Dependency Inversion Principle (DIP):** Módulos de alto nível não devem depender de módulos de baixo nível; ambos devem depender de abstrações.

## 🗃️ Estrutura do Projeto

A solução seguirá uma estrutura de projetos modular baseada em Clean Architecture para separar responsabilidades:

```
/
├── src/
│   ├── Ambev.DeveloperEvaluation.Api/          # Camada de Apresentação (API RESTful, Controllers)
│   ├── Ambev.DeveloperEvaluation.Application/  # Camada de Aplicação (Orquestra o domínio, DTOs, Command/Query Handlers, Validações)
│   ├── Ambev.DeveloperEvaluation.Domain/       # Camada de Domínio (Entidades, Value Objects, Agregados, Domain Events, Interfaces de Repositório)
│   ├── Ambev.DeveloperEvaluation.Infrastructure/ # Camada de Infraestrutura (EF Core, Repositórios, Serviços Externos)
│   └── Ambev.DeveloperEvaluation.IoC/          # Camada de Injeção de Dependência
└── tests/
    ├── Ambev.DeveloperEvaluation.Unit/         # Testes Unitários
    ├── Ambev.DeveloperEvaluation.Integration/  # Testes de Integração
    └── Ambev.DeveloperEvaluation.Functional/   # Testes Funcionais/End-to-End
```

## 📝 Convenções de Nomenclatura e Estilo

A consistência é chave para um codebase limpo e compreensível.

* **Nomenclatura:**
    * **PascalCase:** Para classes, interfaces, enums, propriedades públicas, métodos públicos e *namespaces*.
    * **camelCase:** Para variáveis locais e parâmetros de método.
    * **Prefixo `_` em campos privados:** Use `_camelCase` para campos privados (ex: `_saleRepository`).
    * **Prefixo 'I' para Interfaces:** Todas as interfaces devem começar com o prefixo 'I' (ex: `ISaleRepository`, `IEventPublisher`).
* **Espaços em Branco:** Use 4 espaços para indentação (não tabs).
* **Linhas em Branco:** Use linhas em branco para separar blocos lógicos de código, métodos e propriedades para melhorar a legibilidade.
* **Chaves (`{}`):** As chaves de abertura (`{`) devem estar na mesma linha da declaração que as precede.
    ```csharp
    public class Sale
    {
        public void CalculateTotal()
        {
            // código
        }
    }
    ```
* **Uso de `var`:** Prefira `var` quando o tipo da variável for óbvio pela inicialização.
    ```csharp
    var sale = new Sale(...); // Tipo óbvio
    ```
* **Operadores de Nulidade:** Use `?.` e `??` para um tratamento de nulos mais limpo e seguro.
    ```csharp
    string customerName = sale?.Customer?.Name ?? "Cliente não informado";
    ```
* **Strings Interpoladas:** Prefira strings interpoladas (`$""`) em vez de `string.Format()` ou concatenação.
    ```csharp
    _logger.LogInformation($"Venda {saleId} criada com sucesso.");
    ```
* **Uso de LINQ:** Prefira a sintaxe de método de LINQ.

## 🛠️ Boas Práticas de Desenvolvimento

* **Injeção de Dependência (DI):** Utilize o contêiner de DI nativo do .NET para gerenciar dependências. Injete interfaces, não implementações concretas.
* **Tratamento de Exceções:**
    * Evite "engolir" exceções silenciosamente. Capture exceções específicas e registre-as.
    * Utilize middlewares para tratamento global de exceções na API, mapeando-as para respostas HTTP padronizadas (ex: 400, 404, 500).
    * Nunca exponha detalhes internos de exceções em produção.
* **Logging:**
    * Utilize um framework de logging configurado (como Serilog) e injete `ILogger<T>`.
    * Configure logs **estruturados** (JSON) para facilitar a análise.
    * **Evite registrar dados sensíveis** (senhas, PII, credenciais).
* **Validações:**
    * Implemente validações na camada de Aplicação usando **FluentValidation** para `Commands` e `Queries`.
    * Garanta as invariantes de negócio dentro das entidades de Domínio, lançando exceções de domínio (`DomainException`).
* **Assincronicidade (`async`/`await`):**
    * Utilize `async` e `await` para todas as operações I/O-bound (banco de dados, chamadas HTTP).
    * Sempre que possível, utilize `.ConfigureAwait(false)` em bibliotecas para evitar deadlocks.
* **Imutabilidade:**
    * Use tipos imutáveis (ex: *record types*) para Value Objects e DTOs para reduzir efeitos colaterais.

## 🔒 Gerenciamento de Dados Sensíveis e Segurança

* **Dados Sensíveis no Código:** **Jamais** armazene credenciais, chaves de API, ou quaisquer dados sensíveis diretamente no código-fonte ou em arquivos `appsettings.json` versionados.
* **Desenvolvimento Local (User Secrets):** Utilize o **.NET User Secrets** para gerenciar dados sensíveis localmente.
* **Ambientes de Produção (Secrets Management):** Em ambientes de produção, é **mandatório** o uso de um cofre de segredos (ex: Azure Key Vault, AWS Secrets Manager, HashiCorp Vault).
* **Validação e Sanitização de Entradas:** Todas as entradas de usuário e dados de sistemas externos devem ser rigorosamente validadas para prevenir ataques de injeção.
* **Autenticação e Autorização:**
    * Utilize JWT para proteger os endpoints da API.
    * Implemente autorização baseada em papéis (RBAC) ou *claims* para controlar o acesso.
    * Aplique os atributos `[Authorize]` e `[AllowAnonymous]` corretamente.
* **Criptografia:**
    * Garanta que todos os dados sensíveis sejam criptografados **em trânsito** (HTTPS/TLS 1.2+) e **em repouso** (criptografia de banco de dados).

## 🧪 Estratégia de Testes

Para informações detalhadas sobre nossa abordagem de testes (unitários, integração, etc.) e a política de cobertura de código, consulte o documento específico `TESTING_GUIDELINES.md`.

## 🤝 Colaboração e Revisão de Código

Para entender o processo de contribuição de código, incluindo o fluxo de trabalho do Git (GitFlow), padrões de *commit messages* (Commits Semânticos) e o processo de Pull Request (PR), consulte o documento `CONTRIBUTING.md`.
