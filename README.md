# Commerce — Clean Architecture Backend

A backend-focused e-commerce system built with ASP.NET Core, EF Core, and Azure, emphasizing clean architecture, reliability, and real-world failure handling.

## What this project demonstrates

- Clean Architecture with clear separation of Domain, Application, Infrastructure, and API layers
- Robust error handling with semantic exceptions and global exception mapping
- Reliable order processing using the Outbox pattern
- Claims-based role authorization (Admin vs Customer) using Azure Entra ID App Roles
- Reliable, decoupled transactional email delivery

## Architecture

The system follows a layered Clean Architecture approach:

- **Domain** – core business entities and invariants
- **Application** – use cases, commands/queries, and failure semantics
- **Infrastructure** – EF Core persistence, repositories, blob storage, Unit of Work, messaging, email delivery, and background processing
- **API** – HTTP endpoints, authentication/authorization (Azure Entra ID), and exception translation

## Key design decisions

### Semantic application exceptions
- Application services throw semantic exceptions (e.g. NotFound, Validation)
- Infrastructure failures are modeled explicitly (e.g. Service Bus, Blob Storage, Email)
- The API layer translates these into consistent HTTP ProblemDetails responses via centralized error handling

### Unit of Work
- A Unit of Work coordinates persistence across repositories
- Ensures related changes (orders, outbox messages, images) are committed atomically
- Centralizes transaction boundaries and persistence concerns

### Outbox pattern
- Order creation and event enqueue occur within the same EF Core transaction
- Prevents lost events and inconsistent state
- Background publishers reliably dispatch events to external systems

### Failure-safe media uploads
- Blob uploads are compensated if database persistence fails
- Prevents orphaned blobs in storage

### Email confirmation & transactional notifications
- Email confirmation and order notifications are sent via Azure Communication Services (ACS)
- Email delivery is treated as a **side effect**, not part of core domain logic
- The Application layer expresses intent (e.g. *OrderPlaced*, *EmailConfirmationRequested*)
- The Infrastructure layer handles delivery, retries, and external service concerns
- Email failures do **not** cause request failures or corrupt transactional state

## Features

- Product browsing with pagination and filtering
- Admin-only product and image management
- Order creation with transactional integrity
- Background outbox publishers for events and emails
- Email confirmation and order confirmation notifications

## Tech stack

- ASP.NET Core
- Entity Framework Core
- SQLite (dev) / MySQL-ready
- Azure Blob Storage
- Azure Entra ID (CIAM)
- Azure Service Bus (outbox publishing)
- Azure Communication Services (Email)

## Running the project

This repository is primarily intended as an architectural and backend design showcase.

It is not configured for one-click local execution out of the box.
