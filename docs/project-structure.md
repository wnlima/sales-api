### Estrutura do Repositório

Este repositório é um **monorepo** que hospeda a solução de e-commerce completa, organizada por domínios de negócio e seguindo os princípios de **Clean Architecture** e **DDD**. A estrutura visa facilitar o desenvolvimento, a manutenção e a escalabilidade dos microsserviços.

-----

### **Visão Geral**

```
.
├── .github/
│   └── workflows/              # Pipelines de CI/CD (GitHub Actions)
├── docs/                       # Documentação técnica e de negócio
├── standards/                  # Padrões e diretrizes do projeto
├── backend/
│   ├── src/                    # Código-fonte dos microsserviços
│   │   ├── Ambev.DeveloperEvaluation.Domain.Common/   # Projeto com elementos de domínio comuns a todos os serviços
│   │   ├── Ambev.DeveloperEvaluation.WebApi.Common/   # Projeto com elementos de API comuns a todos os serviços
│   │   ├── Products/           # Microsserviço de Produtos
│   │   ├── Sales/              # Microsserviço de Vendas
│   │   └── Users/              # Microsserviço de Usuários
│   └── tests/                  # Projetos de testes para cada microsserviço
│       ├── Ambev.ProductsDeveloperEvaluation.Tests.Integration/
│       ├── Ambev.ProductsDeveloperEvaluation.Tests.Unit/
│       ├── Ambev.SalesDeveloperEvaluation.Tests.Integration/
│       ├── Ambev.SalesDeveloperEvaluation.Tests.Unit/
│       ├── Ambev.UsersDeveloperEvaluation.Tests.Integration/
│       └── Ambev.UsersDeveloperEvaluation.Tests.Unit/
├── README.md
├── .env                        # Variáveis de ambiente para o Docker Compose
├── docker-compose.yml          # Definições de contêineres para ambiente local
├── docker-compose.override.yml # Configurações para sobrescrever o docker-compose.yml em ambiente de desenvolvimento
└── ...
```

-----

### **Descrição dos Diretórios e Projetos**

#### `backend/src/`

Contém o código-fonte principal da solução. Cada microsserviço está em seu próprio diretório, e cada um deles segue a estrutura de Clean Architecture, dividida em camadas lógicas:

  * **`{ServiceName}.Domain`**: A camada central da aplicação. Contém a lógica de negócio, entidades (`Aggregate`), objetos de valor, exceções e eventos de domínio. É a camada mais interna e não tem dependências externas.
      * Exemplo: `Ambev.ProductsDeveloperEvaluation.Domain`
  * **`{ServiceName}.Application`**: A camada de orquestração. Contém os casos de uso (`Commands`, `Queries`), seus respectivos `Handlers`, validadores (`Validators`) e as interfaces que o `Domain` precisa (e.g., interfaces de repositórios). Segue o padrão CQRS.
      * Exemplo: `Ambev.SalesDeveloperEvaluation.Application`
  * **`{ServiceName}.Infrastructure`**: A camada de infraestrutura. Implementa as interfaces de repositório definidas na camada `Domain` usando tecnologias concretas (como EF Core para PostgreSQL). Também contém a implementação do publicador de eventos.
      * Exemplo: `Ambev.UsersDeveloperEvaluation.Infrastructure`
  * **`{ServiceName}.IoC`**: Módulo de Inversão de Controle para gerenciar as dependências de injeção entre as camadas.
      * Exemplo: `Ambev.ProductsDeveloperEvaluation.IoC`
  * **`{ServiceName}.WebApi`**: A camada de apresentação (interface com o usuário). Contém os controladores de API (`Controllers`), DTOs de requisição/resposta e o middleware de tratamento de erros.
      * Exemplo: `Ambev.SalesDeveloperEvaluation.WebApi`

#### `backend/tests/`

Contém todos os projetos de testes automatizados, organizados por microsserviço e tipo de teste:

  * **`Tests.Unit`**: Projetos dedicados a testes unitários, focados em validar a lógica de negócio isoladamente, principalmente nas camadas de `Domain` e `Application`.
  * **`Tests.Integration`**: Projetos de testes de integração, que validam o fluxo completo de um endpoint de API, incluindo a interação com a infraestrutura (banco de dados, message broker).

#### `docker-compose.yml` e `docker-compose.override.yml`

  * **`docker-compose.yml`**: Arquivo de configuração base para orquestrar e rodar todos os serviços e suas dependências (PostgreSQL, RabbitMQ, Redis) em um ambiente local. Define as configurações padrão e as portas expostas.
  * **`docker-compose.override.yml`**: Arquivo para sobrescrever as configurações do `docker-compose.yml` no ambiente de desenvolvimento. Ele pode ser usado para adicionar volume mapping, expor portas adicionais, ou configurar variáveis de ambiente específicas sem alterar o arquivo principal.

#### `.env`

Arquivo que armazena as variáveis de ambiente, como strings de conexão e senhas, para serem usadas pelos arquivos `docker-compose`. Isso garante que as credenciais e configurações sensíveis não sejam expostas no código-fonte.