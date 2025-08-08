### Sales

Este documento descreve os endpoints do microsserviço `Sales-API`, responsável por gerenciar o ciclo de vida das vendas no sistema de e-commerce. O serviço é o coração da plataforma, aplicando as regras de negócio de desconto e orquestrando transações distribuídas através do padrão Saga.

A arquitetura do `Sales-API` segue os princípios de Domain-Driven Design (DDD), com o agregado de domínio `Sale` encapsulando toda a lógica de negócio, como a aplicação de descontos e a validação de itens. O serviço utiliza o padrão CQRS e a comunicação assíncrona com o barramento de eventos (RabbitMQ) para manter o alto desempenho e o desacoplamento.

#### **Modelo de Agregado `Sale`**

O agregado `Sale` representa uma transação de venda e é a entidade raiz do domínio de vendas. Ele é persistido no banco de dados **PostgreSQL** dedicado a este microsserviço.

  * **Id** (`Guid`): Identificador único da venda.
  * **UserId** (`Guid`): Identificador do usuário que realizou a compra.
  * **TotalAmount** (`decimal`): Valor total da venda, incluindo descontos.
  * **DiscountAmount** (`decimal`): Valor total dos descontos aplicados.
  * **SaleStatus** (`enum`): Status atual da venda (`Created`, `Cancelled`, `PendingPayment`, etc.).
  * **SaleItems** (`List<SaleItem>`): Lista de itens incluídos na venda.

#### **Modelo de Entidade `SaleItem`**

Cada `SaleItem` é parte do agregado `Sale` e representa um produto individual em uma venda.

  * **Id** (`Guid`): Identificador único do item de venda.
  * **ProductId** (`Guid`): Identificador do produto vendido.
  * **Quantity** (`int`): Quantidade do produto na venda.
  * **Price** (`decimal`): Preço unitário do produto no momento da venda.
  * **Discount** (`decimal`): Desconto aplicado ao item.

-----

### **Endpoints da API**

#### `POST /api/sales` - Criar uma Nova Venda

  * **Descrição:** Registra uma nova transação de venda. A lógica de negócio para cálculo de descontos é aplicada na camada de domínio antes da persistência.
  * **Regras de Desconto:** A lógica é implementada usando o padrão `Specification` [cite: `backend/src/Sales/Ambev.SalesDeveloperEvaluation.Domain/Specifications/EligibleFor10PercentDiscountSpecification.cs`].
      * **10% de desconto:** Para compras de 4 a 9 itens do mesmo produto.
      * **20% de desconto:** Para compras de 10 a 20 itens do mesmo produto.
      * **Quantidade máxima:** Não é permitido vender mais de 20 itens idênticos em uma única venda.
  * **Padrão Saga:** Após a criação bem-sucedida, o microsserviço publica um evento de domínio (`SaleCreatedEvent`) no barramento de eventos (RabbitMQ), iniciando o fluxo da Saga para outros serviços.

**Request Body**

```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "items": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantity": 5
    }
  ]
}
```

**Exemplo de Resposta (201 Created)**

```json
{
  "success": true,
  "message": "Sale created successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "totalAmount": 100.0,
    "discountAmount": 10.0,
    "status": "Created"
  }
}
```

#### `GET /api/sales` - Listar Vendas por Cliente

  * **Descrição:** Retorna uma lista paginada de todas as vendas realizadas pelo cliente autenticado.
  * **Autorização:** Requer um token JWT e a autorização adequada para acessar as vendas do usuário logado.

#### `GET /api/sales/{id}` - Obter Detalhes de uma Venda

  * **Descrição:** Retorna os detalhes de uma venda específica, permitindo acesso apenas ao usuário que a criou ou a um gerente.
  * **Autorização:** Requer autenticação com JWT e autorização para acessar a venda específica.

#### `DELETE /api/sales/{id}` - Cancelar uma Venda

  * **Descrição:** Cancela uma venda existente, alterando seu status.
  * **Regra de Negócio:** Apenas o usuário que criou a venda ou um gerente pode cancelá-la.
  * **Padrão Saga:** Ao cancelar, um evento de domínio (`SaleCancelledEvent`) é publicado no RabbitMQ para acionar transações de compensação em outros serviços (e.g., reverter o pagamento ou o estoque).

#### `GET /api/manager/sales` - Listar Todas as Vendas (Gerente)

  * **Descrição:** Endpoint restrito que retorna uma lista paginada de todas as vendas do sistema.
  * **Autorização:** Apenas usuários com o papel `Manager` podem acessar este endpoint [cite: `backend/src/Sales/Ambev.SalesDeveloperEvaluation.WebApi/Features/Manager/ManagerSalesController.cs`].

#### `GET /api/manager/sales/{id}` - Obter Detalhes de uma Venda (Gerente)

  * **Descrição:** Retorna os detalhes de uma venda específica para gerentes.
  * **Autorização:** Restrito ao papel `Manager`.

#### `DELETE /api/manager/sales/{id}` - Cancelar uma Venda (Gerente)

  * **Descrição:** Permite que um gerente cancele qualquer venda no sistema.
  * **Autorização:** Restrito ao papel `Manager`.