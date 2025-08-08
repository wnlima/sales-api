# **✅ Planejamento de Cenários de Testes (TDD) \- Sales API**

Este documento descreve todos os cenários de testes que serão (ou podem ser) implementados, organizados por tipo e camada. O objetivo é garantir total cobertura das regras de negócio, integrações, persistência e qualidade do código.

## 🧪 Tipos de Testes

* **Testes de Unidade**: Regras de negócio isoladas na camada de Domínio.  
* **Testes de Aplicação**: Handlers de comandos/queries e validações (FluentValidation).  
* **Testes Funcionais (End-to-End)**: Endpoints da API via HTTP, validando o fluxo completo.  
* **Testes de Efeitos Colaterais**: Verificação de logs e eventos (ex: SaleCreated).  
* **Testes de Performance (futuros)**: Verificação de tempo/resposta e carga.

## 🧩 Domínio: Regras de Negócio (Testes de Unidade)

### 📦 Agregado de Venda (Sale)

- [x] Deve calcular o total da venda com base nos itens.  
- [x] Deve aplicar 10% de desconto para um item entre 4 e 9 unidades.  
- [x] Deve aplicar 20% de desconto para um item entre 10 e 20 unidades.  
- [x] Não deve aplicar desconto para um item com menos de 4 unidades.  
- [x] Deve calcular corretamente o valor final da venda somando todos os itens.  
- [x] Deve proibir a adição de um item com quantidade superior a 20\.  
- [x] Deve proibir a adição de um item com quantidade zero ou negativa.  
- [x] Deve proibir a adição do mesmo produto duas vezes na mesma venda.  
- [x] Deve proibir a adição de um item a uma venda já cancelada.  
- [x] Deve alterar o status para Canceled ao chamar o método Cancel().  
- [x] Deve proibir o cancelamento de uma venda que já está cancelada.

## ⚙️ Camada de Aplicação (Testes de Unidade/Integraçã**

### 🧾 CreateSaleHandler

- [x] Deve criar uma venda válida e retornar o ID e o total.  
- [x] Deve lançar exceção se o CustomerId informado não existir.  
- [x] Deve lançar exceção se o BranchId informado não existir.  
- [x] Deve lançar exceção se um ProductId informado não existir.  
- [x] Deve persistir corretamente todos os dados da venda e seus itens.  
- [x] Deve gerar um log estruturado para o evento SaleCreated.

### ✏️ UpdateSaleHandler

- [x] Deve permitir a atualização de uma venda existente.  
- [x] Deve recalcular o total da venda após adicionar, remover ou alterar itens.  
- [x] Deve gerar um log estruturado para o evento SaleModified.  
- [x] Deve lançar exceção ao tentar atualizar uma venda que não existe.  
- [x] Deve lançar exceção ao tentar atualizar uma venda já cancelada.

### ❌ CancelSaleHandler

- [x] Deve buscar a venda, alterar seu status para Canceled e persistir.  
- [x] Deve gerar um log estruturado para o evento SaleCancelled.  
- [x] Deve lançar exceção ao tentar cancelar uma venda que não existe.

## 🌐 API: Testes Funcionais (End-to-End)

### 🔼 POST /sales

- [x] Deve retornar 201 Created com a localização e o corpo da resposta para uma venda válida.  
- [x] Deve retornar 400 Bad Request se o corpo da requisição for inválido (ex: sem itens, quantidade \> 20).  
- [x] Deve retornar 404 Not Found se o cliente, filial ou algum produto não for encontrado.  
- [x] Deve persistir os dados corretamente no banco de dados.

### 📥 GET /sales/{id}

- [x] Deve retornar 200 OK e os dados completos da venda.  
- [x] Deve retornar 404 Not Found se a venda não existir.

### 📝 PUT /sales/{id}

- [x] Deve retornar 200 OK com os dados atualizados da venda.  
- [x] Deve retornar 400 Bad Request para dados de atualização inválidos.  
- [x] Deve retornar 404 Not Found se a venda a ser atualizada não existir.  
- [x] Deve retornar 409 Conflict (ou 400\) se tentar atualizar uma venda cancelada.

### 🗑️ DELETE /sales/{id} (Cancelamento)

- [x] Deve retornar 204 No Content após cancelar com sucesso.  
- [x] Deve alterar o status da venda para "Canceled" no banco de dados.  
- [x] Deve retornar 404 Not Found se a venda não existir.  
- [x] Deve retornar 409 Conflict (ou 400\) se a venda já estiver cancelada.

## 🧪 Futuro (Testes Possíveis)

- [x] Consulta de vendas com filtros (por data, cliente, filial).  
- [x] Paginação e ordenação nos endpoints de listagem.  
- [x] Testes de autenticação e autorização nos endpoints (cenários 401 e 403).  
- [x] Testes de carga com muitos itens ou muitas vendas simultâneas.  
- [x] Testes de resiliência (ex: o que acontece se o banco de dados estiver offline).

## 📌 Observação

Este documento serve como guia vivo e documentação técnica das intenções de cobertura de testes da aplicação. Os cenários marcados com [x] são os que já foram implementados.