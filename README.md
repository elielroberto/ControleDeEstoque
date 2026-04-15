#  Controle de Estoque

Sistema de gerenciamento de estoque desenvolvido com foco em organização, escalabilidade e aplicação prática de boas práticas em APIs REST.

Este projeto permite o cadastro de produtos e controle de movimentações, garantindo consistência dos dados e validações de regras de negócio.

---

##  Funcionalidades

-  Cadastro de produtos
-  Consulta por ID e listagem geral
-  Atualização de produtos
-  Remoção de produtos
-  Validação de SKU (não pode ser vazio ou duplicado)
-  Controle de status ativo/inativo
-  Tratamento padronizado de respostas HTTP (400, 404, 409, etc.)

---

##  Arquitetura

O projeto foi desenvolvido utilizando arquitetura em camadas:

- **Controller** → Responsável pelos endpoints da API
- **Service** → Contém as regras de negócio
- **Repository** → Acesso aos dados

Aplicando princípios como:
-  Separação de responsabilidades (SRP)
-  Injeção de dependência
-  Organização e manutenibilidade do código

---

##  Testes Unitários

O projeto possui cobertura de testes unitários utilizando:

- xUnit
- Moq

Foram testados cenários como:

-  Criação de produto com dados inválidos
-  Validação de SKU duplicado
-  Criação com sucesso
-  Busca por ID (existente e inexistente)
-  Atualização de produto
-  Exclusão de produto
-  Listagem geral

Garantindo maior confiabilidade e qualidade do sistema.

---

##  Endpoints

###  Criar produto
```http
POST /api/products
