### Users

Este documento descreve os endpoints do microsserviço `Users-API`, responsável pelo gerenciamento de usuários e pelo fluxo de autenticação via JWT. O serviço segue os princípios de Domain-Driven Design (DDD) e Clean Architecture, garantindo uma base sólida, segura e de fácil manutenção para o gerenciamento de identidades.

A lógica de negócio e as validações para a entidade `User` estão contidas na camada de `Domain`, enquanto a orquestração e a comunicação entre as camadas são gerenciadas na camada de `Application`, utilizando o padrão CQRS e `MediatR`.

#### **Modelo de Entidade `User`**

A entidade `User` é a representação central do domínio de usuários e é persistida no banco de dados **PostgreSQL**. A persistência é mapeada por `EF Core`, garantindo que campos como `Email`, `Username`, `Password`, `Role` e `Status` sejam corretamente armazenados.

  * **Id** (`Guid`): Identificador único do usuário.
  * **Username** (`string`): Nome de usuário, deve ser único.
  * **Email** (`string`): Endereço de e-mail, deve ser único.
  * **Password** (`string`): Senha do usuário, armazenada de forma segura com hash (`BCrypt`).
  * **Phone** (`string`): Telefone do usuário.
  * **Role** (`enum`): Papel do usuário no sistema (`Customer`, `Manager`, `Admin`).
  * **Status** (`enum`): Status do usuário (`Active`, `Inactive`, `Suspended`).

-----

### **Endpoints da API**

#### `POST /api/users` - Criar um Novo Usuário

  * **Descrição:** Permite o registro de um novo usuário no sistema. A senha é automaticamente hashed antes de ser armazenada.
  * **Validação:** A requisição é validada usando `FluentValidation` para garantir que o `Email` seja válido, a `Username` e `Password` atendam aos requisitos de segurança, e a `Role` e o `Status` não sejam `None` ou `Unknown`.

**Request Body**

```json
{
  "username": "string",
  "email": "string",
  "password": "string",
  "phone": "string",
  "status": "string (enum: Active, Inactive, Suspended)",
  "role": "string (enum: Customer, Manager, Admin)"
}
```

**Exemplo de Resposta (201 Created)**

```json
{
  "success": true,
  "message": "User created successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
}
```

#### `GET /api/users/{id}` - Obter Detalhes de um Usuário

  * **Descrição:** Busca o perfil de um usuário pelo seu ID. Esta operação requer autenticação com um token JWT.
  * **Resposta:** Retorna os detalhes do usuário, incluindo seu ID, nome de usuário, e-mail, telefone e papéis.
  * **Validação:** Valida se o `id` na URL não é vazio (`Guid.Empty`).

**Exemplo de Resposta (200 OK)**

```json
{
  "success": true,
  "message": "User retrieved successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "nome do usuário",
    "email": "user@email.com",
    "phone": "11999999999",
    "role": "Customer",
    "status": "Active"
  }
}
```

#### `DELETE /api/users/{id}` - Deletar um Usuário

  * **Descrição:** Remove um usuário do sistema pelo seu ID. Esta operação requer autenticação com um token JWT e autorização adequada.
  * **Resposta:** Retorna uma confirmação de sucesso.
  * **Validação:** Verifica se o `id` não é vazio.

**Exemplo de Resposta (200 OK)**

```json
{
  "success": true,
  "message": "User deleted successfully",
  "data": null
}
```

-----

### **Endpoints de Autenticação (`/auth`)**

#### `POST /api/auth` - Autenticar um Usuário

  * **Descrição:** Autentica um usuário com `email` e `password`.
  * **Resposta:** Se as credenciais forem válidas, retorna um token JWT para ser usado em requisições futuras.
  * **Validação:** Valida o formato do e-mail e verifica se a senha fornecida corresponde ao hash armazenado no banco de dados.

**Request Body**

```json
{
  "email": "user@email.com",
  "password": "uma_senha_valida"
}
```

**Exemplo de Resposta (200 OK)**

```json
{
  "success": true,
  "message": "User authenticated successfully",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "email": "user@email.com",
    "name": "nome do usuário",
    "role": "Customer"
  }
}
```