# 🚀 Fraud Detection System  
### DDD • Clean Architecture • Modular Monolith • Outbox Pattern • Docker

Sistema backend profissional para simulação de um mecanismo de detecção de fraudes financeiras, desenvolvido em .NET com arquitetura modular baseada em **Domain-Driven Design (DDD)**, **Clean Architecture** e **Outbox Pattern**.

Projeto construído com foco em engenharia de software avançada, modelagem rica de domínio e arquitetura preparada para alto volume.

---

# 🧠 Objetivo

Construir a base arquitetural de um sistema antifraude capaz de:

- Processar transações em alto volume
- Trabalhar com consistência eventual
- Persistir eventos de domínio de forma confiável
- Suportar processamento assíncrono via Worker
- Ser executado de forma containerizada
- Servir como case técnico de backend avançado

---

# 🏗 Arquitetura

Arquitetura modular vertical (feature-based):

FraudDetection  
│
├── FraudDetection.API  
├── FraudDetection.Worker  
├── FraudDetection.BuildingBlocks  
│
└── Modules  
├── Customers  
├── Cards  
├── Transactions  
├── RiskManagement  
└── Analytics  

### Princípios aplicados

- Domain-Driven Design (DDD)
- Aggregate Roots
- Value Objects
- Domain Events
- Repository Pattern
- Outbox Pattern
- Eventual Consistency
- Clean Architecture

---

# 📦 BuildingBlocks

Base compartilhada contendo:

- `Entity`
- `AggregateRoot`
- `ValueObject`
- `DomainEvent`
- `Result<T>`

Permite modelagem rica sem acoplamento à infraestrutura.

---

# 👤 Módulo Customers (Implementado)

## Aggregate Roots

### Customer

Responsável por:
- Identidade do cliente
- Email (ValueObject)
- País (ValueObject)
- Status (bloqueado / ativo)
- Emissão de DomainEvent ao ser criado

### CustomerMetrics

Responsável por:
- Métricas agregadas incrementais
- Total de transações
- Média de valor
- Total de rejeições
- Concurrency control

---

## 🔹 Value Objects

- `Email`
- `Country`

Imutáveis e com validação encapsulada.

---

## 🔹 Domain Event

`CustomerCreatedDomainEvent`

Emitido automaticamente pelo Aggregate quando um cliente é criado.

---

# 🔁 Outbox Pattern (Implementado)

Todos os DomainEvents são interceptados no `SaveChangesAsync` do `FraudDbContext`.

Fluxo:

1. Aggregate gera DomainEvent  
2. DbContext intercepta eventos  
3. Evento é serializado em JSON  
4. Evento é persistido na tabela `outbox_events`  
5. Evento poderá ser processado pelo Worker  

Tabela criada:

- `outbox_events`

Campos:

- Id  
- EventType  
- Payload  
- OccurredOn  
- ProcessedOn  
- RetryCount  

Benefícios:

- Desacoplamento entre domínio e processamento
- Persistência confiável de eventos
- Base para processamento assíncrono
- Consistência eventual realista

---

# 🌐 API REST

Endpoint implementado:

`POST /api/customers`

Exemplo:

{
  "name": "Gabriel",
  "email": "gabriel@email.com",
  "country": "BR"
}

Retorno:

`201 Created`

Com o `Guid` do cliente criado.

---

# 🐳 Infraestrutura (Docker)

Ambiente totalmente containerizado.

Serviços:

- PostgreSQL 16
- Redis 7
- Seq (logs estruturados)

Subir containers:

docker compose up -d

---

# 🗄 Banco de Dados

Aplicar migrations:

dotnet ef database update -p FraudDetection.API -s FraudDetection.API

Tabelas criadas:

- customers
- customer_metrics
- outbox_events
- __EFMigrationsHistory

---

# ▶️ Executar Projeto

Executar API:

dotnet run --project FraudDetection.API

Swagger disponível em:

https://localhost:{PORT}/swagger

---

# 🧰 Stack Tecnológica

- .NET
- Entity Framework Core
- PostgreSQL
- Redis
- MediatR
- Docker
- Swagger
- Clean Architecture
- Domain-Driven Design
- Outbox Pattern

---

# 📊 Status Atual

✔ Arquitetura modular estruturada  
✔ BuildingBlocks implementado  
✔ Customers aggregate completo  
✔ Value Objects implementados  
✔ Repository Pattern aplicado  
✔ Migration aplicada  
✔ Endpoint funcional  
✔ Outbox Pattern implementado  
✔ Persistência de DomainEvents funcionando  
✔ Ambiente Docker configurado  

---

# 🚧 Próximos Passos

- Implementar Worker para consumo do Outbox
- Criar CustomerMetrics automaticamente via evento
- Implementar módulo Transactions
- Implementar Rule Engine dinâmica
- Implementar métricas comportamentais incrementais
- Simular alto volume de transações
- Criar endpoints analíticos
- Implementar cache Redis estratégico
- Adicionar testes unitários e de integração

---

# 🎯 Propósito

Projeto desenvolvido como portfólio técnico focado em:

- Engenharia de software avançada
- Arquitetura limpa e modular
- Modelagem rica de domínio
- Integração assíncrona confiável
- Backend escalável e preparado para evolução

Status: Em desenvolvimento ativo  
Foco: Backend de alto nível + Arquitetura distribuída
