# 🚀 Fraud Detection System  
### DDD • Clean Architecture • Modular Monolith • Outbox Pattern • Docker

A professional backend system designed to simulate a financial fraud detection engine, built with .NET using a modular architecture based on **Domain-Driven Design (DDD)**, **Clean Architecture**, and the **Outbox Pattern**.

This project was developed with a strong focus on advanced software engineering, rich domain modeling, and a scalable architecture ready for high-throughput scenarios.

---

# 🧠 Purpose

Build the architectural foundation of an antifraud system capable of:

- Processing high-volume transactions  
- Supporting eventual consistency  
- Persisting domain events reliably  
- Enabling asynchronous processing via Worker  
- Running in a fully containerized environment  
- Serving as a high-level backend technical case study  

---

# 🏗 Architecture

Vertical modular architecture (feature-based):

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

### Applied Principles

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

Shared foundation containing:

- `Entity`  
- `AggregateRoot`  
- `ValueObject`  
- `DomainEvent`  
- `Result<T>`  

Enables rich domain modeling without coupling to infrastructure.

---

# 👤 Customers Module (Implemented)

## Aggregate Roots

### Customer

Responsible for:

- Customer identity  
- Email (ValueObject)  
- Country (ValueObject)  
- Status (blocked / active)  
- Emitting a DomainEvent upon creation  

### CustomerMetrics

Responsible for:

- Incremental aggregated metrics  
- Total transactions  
- Average transaction value  
- Total rejections  
- Concurrency control  

---

## 🔹 Value Objects

- `Email`  
- `Country`  

Immutable and with encapsulated validation.

---

## 🔹 Domain Event

`CustomerCreatedDomainEvent`

Automatically emitted by the Aggregate when a customer is created.

---

# 🔁 Outbox Pattern (Implemented)

All DomainEvents are intercepted in the `SaveChangesAsync` method of `FraudDbContext`.

Flow:

1. Aggregate generates a DomainEvent  
2. DbContext intercepts events  
3. Event is serialized into JSON  
4. Event is persisted in the `outbox_events` table  
5. Event can be processed later by the Worker  

Created table:

- `outbox_events`

Fields:

- Id  
- EventType  
- Payload  
- OccurredOn  
- ProcessedOn  
- RetryCount  

Benefits:

- Decoupling between domain and processing  
- Reliable event persistence  
- Foundation for asynchronous processing  
- Realistic eventual consistency  

---

# 🌐 REST API

Implemented endpoint:

`POST /api/customers`

Example:

```json
{
  "name": "Gabriel",
  "email": "gabriel@email.com",
  "country": "BR"
}
```

Response:

`201 Created`

Returns the `Guid` of the created customer.

---

# 🐳 Infrastructure (Docker)

Fully containerized environment.

Services:

- PostgreSQL 16  
- Redis 7  
- Seq (structured logging)  

Start containers:

```bash
docker compose up -d
```

---

# 🗄 Database

Apply migrations:

```bash
dotnet ef database update -p FraudDetection.API -s FraudDetection.API
```

Created tables:

- customers  
- customer_metrics  
- outbox_events  
- __EFMigrationsHistory  

---

# ▶️ Run the Project

Run API:

```bash
dotnet run --project FraudDetection.API
```

Swagger available at:

```
https://localhost:{PORT}/swagger
```

---

# 🧰 Tech Stack

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

# 📊 Current Status

✔ Modular architecture structured  
✔ BuildingBlocks implemented  
✔ Customers aggregate fully implemented  
✔ Value Objects implemented  
✔ Repository Pattern applied  
✔ Migration applied  
✔ Functional endpoint  
✔ Outbox Pattern implemented  
✔ DomainEvents persistence working  
✔ Docker environment configured  

---

# 🚧 Next Steps

- Implement Worker for Outbox consumption  
- Automatically create CustomerMetrics via events  
- Implement Transactions module  
- Implement dynamic Rule Engine  
- Implement incremental behavioral metrics  
- Simulate high transaction volume  
- Create analytical endpoints  
- Implement strategic Redis caching  
- Add unit and integration tests  

---

# 🎯 Purpose

This project was developed as a technical portfolio focused on:

- Advanced software engineering  
- Clean and modular architecture  
- Rich domain modeling  
- Reliable asynchronous integration  
- Scalable backend ready for evolution  

Status: Actively under development  
Focus: High-level backend + distributed architecture  
