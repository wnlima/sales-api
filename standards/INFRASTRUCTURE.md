# ⚙️ Configuração e Gerenciamento da Infraestrutura do Projeto

Este documento descreve a estratégia para a configuração e o gerenciamento da infraestrutura do projeto **API de Vendas (Ambev.DeveloperEvaluation)**, com foco em automação, consistência e boas práticas para o ambiente de desenvolvimento.

## 🚀 Visão Geral da Estratégia de Infraestrutura

A infraestrutura do projeto será gerenciada de forma declarativa, utilizando ferramentas padrão de mercado para garantir um ambiente de desenvolvimento reprodutível e consistente. A abordagem visa simplificar o setup e permitir que o foco permaneça no desenvolvimento da aplicação.

## 🐳 Conteinerização (Docker)

A aplicação e suas dependências serão conteinerizadas usando Docker. Isso nos proporciona portabilidade, isolamento e um ambiente de execução consistente desde o desenvolvimento local até uma possível implantação em produção.

* **Docker:** A API e o banco de dados PostgreSQL serão definidos e gerenciados através de arquivos `Dockerfile` e `docker-compose.yml`.
    * **`Dockerfile`:** Cada aplicação terá seu próprio Dockerfile, responsável por construir uma imagem otimizada contendo a aplicação e suas dependências de runtime.
    * **`docker-compose.yml`:** Orquestra os contêineres necessários para o ambiente de desenvolvimento local, incluindo a API e o banco de dados, configurando redes e volumes para a comunicação e persistência de dados.

## 🗄️ Serviços de Infraestrutura Chave

Os seguintes serviços de infraestrutura são a base para a execução do projeto:

1.  **Banco de Dados PostgreSQL:**
    * Uma instância do PostgreSQL será executada em um contêiner Docker, gerenciado pelo `docker-compose.yml`.
    * Isso garante um banco de dados limpo e isolado para o desenvolvimento e para a execução de testes de integração.
    * Os dados podem ser persistidos localmente através de volumes Docker para sobreviver a reinicializações dos contêineres.

2.  **Contêiner da Aplicação:**
    * A API de Vendas será executada em seu próprio contêiner Docker, construído a partir do seu `Dockerfile`.
    * O `docker-compose.yml` irá gerenciar a construção da imagem e a execução do contêiner, conectando-o à rede do banco de dados.

## 🌳 Estrutura de Arquivos de Infraestrutura

Os arquivos relacionados à infraestrutura estão localizados na raiz do projeto e dentro do projeto da API para manter a simplicidade.

```
/
├── docker-compose.yml      # Orquestra os serviços de desenvolvimento (API, DB)
├── .dockerignore           # Especifica arquivos a serem ignorados pelo Docker
└── src/
    └── Ambev.DeveloperEvaluation.WebApi/
        └── Dockerfile      # Define como construir a imagem da API
```

## 🤝 Colaboração e Boas Práticas

Mesmo em um projeto individual, seguir boas práticas de infraestrutura é fundamental para demonstrar senioridade:

* **Infraestrutura Declarativa:** Utilizar o `docker-compose.yml` para definir o estado desejado do ambiente local.
* **Segurança:** Nunca "hardcodar" senhas ou dados sensíveis no `docker-compose.yml` ou `Dockerfile`. Utilizar variáveis de ambiente e o mecanismo de *User Secrets* do .NET.
* **Reprodutibilidade:** Qualquer pessoa com acesso ao repositório deve ser capaz de recriar o ambiente de desenvolvimento local executando `docker compose up`.

## 🚧 Próximos Passos

* Validar e refinar o `Dockerfile` para garantir imagens otimizadas (multi-stage builds).
* Manter o `docker-compose.yml` atualizado conforme novas dependências sejam (hipoteticamente) adicionadas.
* Garantir que os scripts de migração do Entity Framework funcionem perfeitamente com o banco de dados conteinerizado.
