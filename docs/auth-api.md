### Autenticação (Auth) - Users-API

Este documento detalha os endpoints do microsserviço `Users-API` dedicados ao processo de autenticação e geração de tokens JWT. Esta é a porta de entrada segura para o sistema, garantindo que apenas usuários autenticados e autorizados possam acessar os demais serviços.

A lógica de autenticação está encapsulada na camada de `Application` do microsserviço, utilizando o padrão **CQRS** com o comando `AuthenticateUserCommand`.

#### **Padrão de Segurança: Autenticação com JWT**

O sistema utiliza **JSON Web Tokens (JWT)** para a autenticação. Após um login bem-sucedido, o `Users-API` gera um token assinado que contém as informações do usuário (como ID e papéis). Este token deve ser incluído no header `Authorization` de todas as requisições subsequentes para endpoints protegidos, utilizando o formato `Bearer <token>`.

-----

### **Endpoint da API**

#### `POST /api/auth` - Autenticar um Usuário

  * **Descrição:** Autentica um usuário através de seu `email` e `password`. Se as credenciais estiverem corretas, um token JWT será gerado e retornado na resposta. Este token concede acesso temporário aos recursos do sistema.
  * **Validação:** A requisição é validada para garantir que o `email` esteja em um formato válido e que a `password` não esteja vazia. A lógica de negócio verifica a existência do usuário e compara a senha fornecida com o hash armazenado de forma segura.

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

-----

*Nota: Para detalhes sobre os demais endpoints do microsserviço de usuários, consulte o arquivo `users-api.md`.*