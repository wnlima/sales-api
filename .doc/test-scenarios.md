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

- [ ] Deve calcular o total da venda com base nos itens.  
- [ ] Deve aplicar 10% de desconto para um item entre 4 e 9 unidades.  
- [ ] Deve aplicar 20% de desconto para um item entre 10 e 20 unidades.  
- [ ] Não deve aplicar desconto para um item com menos de 4 unidades.  
- [ ] Deve calcular corretamente o valor final da venda somando todos os itens.  
- [ ] Deve proibir a adição de um item com quantidade superior a 20\.  
- [ ] Deve proibir a adição de um item com quantidade zero ou negativa.  
- [ ] Deve proibir a adição do mesmo produto duas vezes na mesma venda.  
- [ ] Deve proibir a adição de um item a uma venda já cancelada.  
- [ ] Deve alterar o status para Canceled ao chamar o método Cancel().  
- [ ] Deve proibir o cancelamento de uma venda que já está cancelada.

## ⚙️ Camada de Aplicação (Testes de Unidade/Integraçã**

### 🧾 CreateSaleHandler

- [ ] Deve criar uma venda válida e retornar o ID e o total.  
- [ ] Deve lançar exceção se o CustomerId informado não existir.  
- [ ] Deve lançar exceção se o BranchId informado não existir.  
- [ ] Deve lançar exceção se um ProductId informado não existir.  
- [ ] Deve persistir corretamente todos os dados da venda e seus itens.  
- [ ] Deve gerar um log estruturado para o evento SaleCreated.

### ✏️ UpdateSaleHandler

- [ ] Deve permitir a atualização de uma venda existente.  
- [ ] Deve recalcular o total da venda após adicionar, remover ou alterar itens.  
- [ ] Deve gerar um log estruturado para o evento SaleModified.  
- [ ] Deve lançar exceção ao tentar atualizar uma venda que não existe.  
- [ ] Deve lançar exceção ao tentar atualizar uma venda já cancelada.

### ❌ CancelSaleHandler

- [ ] Deve buscar a venda, alterar seu status para Canceled e persistir.  
- [ ] Deve gerar um log estruturado para o evento SaleCancelled.  
- [ ] Deve lançar exceção ao tentar cancelar uma venda que não existe.

## 🌐 API: Testes Funcionais (End-to-End)

### 🔼 POST /sales

- [ ] Deve retornar 201 Created com a localização e o corpo da resposta para uma venda válida.  
- [ ] Deve retornar 400 Bad Request se o corpo da requisição for inválido (ex: sem itens, quantidade \> 20).  
- [ ] Deve retornar 404 Not Found se o cliente, filial ou algum produto não for encontrado.  
- [ ] Deve persistir os dados corretamente no banco de dados.

### 📥 GET /sales/{id}

- [ ] Deve retornar 200 OK e os dados completos da venda.  
- [ ] Deve retornar 404 Not Found se a venda não existir.

### 📝 PUT /sales/{id}

- [ ] Deve retornar 200 OK com os dados atualizados da venda.  
- [ ] Deve retornar 400 Bad Request para dados de atualização inválidos.  
- [ ] Deve retornar 404 Not Found se a venda a ser atualizada não existir.  
- [ ] Deve retornar 409 Conflict (ou 400\) se tentar atualizar uma venda cancelada.

### 🗑️ DELETE /sales/{id} (Cancelamento)

- [ ] Deve retornar 204 No Content após cancelar com sucesso.  
- [ ] Deve alterar o status da venda para "Canceled" no banco de dados.  
- [ ] Deve retornar 404 Not Found se a venda não existir.  
- [ ] Deve retornar 409 Conflict (ou 400\) se a venda já estiver cancelada.

## 🧪 Futuro (Testes Possíveis)

- [ ] Consulta de vendas com filtros (por data, cliente, filial).  
- [ ] Paginação e ordenação nos endpoints de listagem.  
- [ ] Testes de autenticação e autorização nos endpoints (cenários 401 e 403).  
- [ ] Testes de carga com muitos itens ou muitas vendas simultâneas.  
- [ ] Testes de resiliência (ex: o que acontece se o banco de dados estiver offline).

## 📌 Observação

Este documento serve como guia vivo e documentação técnica das intenções de cobertura de testes da aplicação. Os cenários marcados com [x] são os que já foram implementados.