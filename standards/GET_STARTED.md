# 🚀 Inicie Aqui! Guia de Setup do Ambiente de Desenvolvimento

Bem-vindo(a) ao guia de início rápido do projeto **API de Vendas (Ambev.DeveloperEvaluation)**! Este documento o(a) guiará pela configuração do seu ambiente de desenvolvimento local, permitindo que você comece a contribuir rapidamente.

## 📋 Pré-requisitos

Certifique-se de que as seguintes ferramentas e softwares estejam instalados em sua máquina:

1.  **Git:** Para clonar o repositório e gerenciar o controle de versão.
    * [Download Git](https://git-scm.com/downloads)
2.  **Docker Desktop:** Essencial para rodar nossa dependência de banco de dados (PostgreSQL).
    * [Download Docker Desktop](https://www.docker.com/products/docker-desktop)
    * **Importante:** Certifique-se de que o Docker Desktop esteja **executando** antes de tentar subir os serviços.
3.  **.NET 8 SDK:** O *framework* principal para o desenvolvimento da aplicação.
    * [Download .NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
4.  **Editor de Código / IDE:**
    * **Visual Studio Code:** Leve e extensível.
        * [Download VS Code](https://code.visualstudio.com/download)
        * **Extensões Recomendadas:** C# (Microsoft), Docker, GitLens, EditorConfig for VS Code.
    * **Visual Studio 2022:** Uma IDE completa para desenvolvimento .NET.
5.  **Cliente HTTP (Opcional, mas Recomendado):** Para testar a API localmente.
    * **Postman:** [Download Postman](https://www.postman.com/downloads/)
    * **Insomnia:** [Download Insomnia](https://insomnia.rest/download)

## ⬇️ Configuração do Ambiente

Siga os passos abaixo para configurar seu ambiente:

### 1. Clonar o Repositório

Abra seu terminal ou prompt de comando e execute:

```bash
git clone https://github.com/wnlima/sales-api.git
cd sales-api
```

### 2. Configurar User Secrets (Credenciais)

Para rodar a API localmente, você precisará configurar a string de conexão. É **mandatório** o uso do **.NET User Secrets** para isso, garantindo que suas credenciais **não sejam versionadas** no código.

1.  Navegue até a pasta do projeto da API:

    ```bash
    cd src/Ambev.DeveloperEvaluation.WebApi
    ```

2.  Inicialize os User Secrets (se ainda não o fez):

    ```bash
    dotnet user-secrets init
    ```

3.  Defina a string de conexão para o PostgreSQL. **Substitua `sua_senha_forte` por uma senha que você controlará localmente:**

    ```bash
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=sales_api_db;Username=postgres;Password=sua_senha_forte"
    ```
    *Nota: O usuário `postgres` e a senha são definidos no arquivo `docker-compose.yml`.*

### 3. Subir Dependências com Docker Compose

A dependência de infraestrutura (PostgreSQL) é orquestrada via Docker Compose.

1.  Na raiz do repositório (`sales-api/`), execute o comando para subir o serviço em segundo plano:

    ```bash
    docker compose up -d
    ```

2.  Verifique se o contêiner subiu corretamente:

    ```bash
    docker compose ps
    ```
    Você deve ver o status `Up` ou `healthy` para o contêiner do PostgreSQL.

### 4. Executar Migrações de Banco de Dados (PostgreSQL)

Após o banco de dados estar em execução, aplique as migrações do Entity Framework Core para criar o schema.

1.  A partir da raiz do repositório (`sales-api/`), execute:

    ```bash
    dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM/ --startup-project src/Ambev.DeveloperEvaluation.WebApi/ --context DefaultContext
    ```
    *Este comando aplicará todas as migrações pendentes, criando as tabelas necessárias no PostgreSQL.*

2.  **Nota:** Se precisar **criar novas migrações** no futuro (após alterações no modelo), use o comando `dotnet ef migrations add [NomeDaSuaMigracao]` com os mesmos parâmetros:

    ```bash
    # Exemplo para criar uma nova migração
    dotnet ef migrations add SuaNovaMigracao --project src/Ambev.DeveloperEvaluation.ORM/ --startup-project src/Ambev.DeveloperEvaluation.WebApi/ --context DefaultContext
    ```

### 5. Rodar a API

Após as dependências e o banco de dados estarem configurados, você pode iniciar a API.

#### Opção A: Via Terminal

1.  Certifique-se de estar na raiz do repositório (`sales-api/`).
2.  Execute o comando:
    ```bash
    dotnet run --project src/Ambev.DeveloperEvaluation.WebApi/
    ```
3.  A API estará disponível em `https://localhost:7043` e `http://localhost:5194` (as portas podem variar, verifique o output do terminal). O Swagger UI estará em `https://localhost:7043/swagger`.

#### Opção B: Via Visual Studio

1.  Abra a solution (`Ambev.DeveloperEvaluation.sln`) no Visual Studio.
2.  Defina `Ambev.DeveloperEvaluation.WebApi` como projeto de inicialização.
3.  Pressione `F5` ou clique no botão "Start" para iniciar o projeto.

### 6. Executando Testes Automatizados

Para rodar todos os testes automatizados do projeto:

1.  Na raiz do repositório (`sales-api/`), execute o comando:
    ```bash
    dotnet test
    ```

---

## 🛑 Parando o Projeto

Para parar os serviços e limpar os recursos do Docker:

1.  **Pare a API:**
    * Se você a iniciou via terminal, pressione `Ctrl+C`.
    * Se usou o Visual Studio, pare a depuração.

2.  **Pare e Remova o Contêiner Docker:**
    * Na raiz do repositório (`sales-api/`), execute:
    ```bash
    docker compose down
    ```
    *Este comando irá parar e remover o contêiner, a rede e os volumes criados.*
