### Products

Este documento descreve os endpoints do microsserviço `Products-API`, responsável por gerenciar o catálogo de produtos e o controle de estoque. O serviço utiliza uma arquitetura de microsserviços com DDD e Clean Architecture para garantir um gerenciamento eficiente e seguro.

-----

#### `POST /api/products` - Criar um Novo Produto

  * **Descrição:** Permite o registro de um novo produto no catálogo.
  * **Request Body:**
    ```json
    {
      "name": "string",
      "description": "string",
      "price": "number",
      "quantityInStock": "integer",
      "isActive": "boolean"
    }
    ```
  * **Response (201 Created):**
    ```json
    {
      "success": "boolean",
      "message": "string",
      "data": {
        "id": "guid"
      }
    }
    ```

-----

#### `GET /api/products` - Listar Produtos com Paginação

  * **Descrição:** Retorna uma lista paginada de todos os produtos ativos.
  * **Query Parameters:**
      * `_page` (opcional): Número da página (padrão: 1)
      * `_size` (opcional): Número de itens por página (padrão: 10)
      * `_order` (opcional): Ordenação dos resultados (ex: "name asc", "price desc")
  * **Response (200 OK):**
    ```json
    {
      "data": [
        {
          "id": "guid",
          "name": "string",
          "description": "string",
          "price": "number",
          "quantityInStock": "integer",
          "isActive": "boolean"
        }
      ],
      "totalItems": "integer",
      "currentPage": "integer",
      "totalPages": "integer"
    }
    ```

-----

#### `GET /api/products/{id}` - Obter Detalhes de um Produto

  * **Descrição:** Busca um produto específico pelo seu ID.
  * **Path Parameters:**
      * `id`: ID do produto (guid)
  * **Response (200 OK):**
    ```json
    {
      "success": "boolean",
      "message": "string",
      "data": {
        "id": "guid",
        "name": "string",
        "description": "string",
        "price": "number",
        "quantityInStock": "integer",
        "isActive": "boolean"
      }
    }
    ```

-----

#### `PUT /api/products/{id}` - Atualizar um Produto

  * **Descrição:** Atualiza as informações de um produto existente.
  * **Path Parameters:**
      * `id`: ID do produto (guid)
  * **Request Body:**
    ```json
    {
      "name": "string",
      "description": "string",
      "price": "number",
      "quantityInStock": "integer",
      "isActive": "boolean"
    }
    ```
  * **Response (200 OK):**
    ```json
    {
      "success": "boolean",
      "message": "string",
      "data": null
    }
    ```

-----

#### `DELETE /api/products/{id}` - Excluir um Produto

  * **Descrição:** Remove um produto do catálogo. A exclusão pode ser lógica, alterando o status `isActive` para `false`.
  * **Path Parameters:**
      * `id`: ID do produto (guid)
  * **Response (200 OK):**
    ```json
    {
      "success": "boolean",
      "message": "string",
      "data": null
    }
    ```