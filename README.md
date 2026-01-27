# Commerce — Clean Architecture E-Commerce Platform

A full-stack e-commerce platform built with **ASP.NET Core, EF Core, Azure, and React (TypeScript)**, emphasizing clean architecture, reliability, and real-world failure handling across backend and frontend boundaries.

The project is designed as a **production-style system**, showcasing backend architecture, cloud integration, and a modern frontend consuming a versioned HTTP API.

---

## What this project demonstrates

- Clean Architecture with clear separation of **Domain, Application, Infrastructure, and API** layers
- A modern **React + TypeScript frontend** consuming a versioned REST API
- Robust error handling with semantic exceptions and global exception mapping
- Reliable order processing using the **Outbox pattern**
- Claims-based role authorization (Admin vs Customer) using **Azure Entra ID App Roles**
- Reliable, decoupled transactional email delivery
- Failure-safe media uploads with compensation logic

---

## Architecture

The system follows a layered Clean Architecture approach on the backend, paired with a decoupled frontend:

### Backend
- **Domain** – core business entities and invariants
- **Application** – use cases, commands/queries, and failure semantics
- **Infrastructure** – EF Core persistence, repositories, blob storage, Unit of Work, messaging, email delivery, and background processing
- **API** – HTTP endpoints, authentication/authorization (Azure Entra ID), and exception translation

### Frontend
- **React + TypeScript** application
- Consumes the backend via a versioned REST API
- Implements product browsing, filtering, and order flows
- Clean separation between UI concerns and API contracts

---

## Key design decisions

### Semantic application exceptions
- Application services throw semantic exceptions (e.g. `NotFound`, `Validation`)
- Infrastructure failures are modeled explicitly (e.g. Service Bus, Blob Storage, Email)
- The API layer translates failures into consistent HTTP `ProblemDetails` responses via centralized error handling

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
- Email confirmation and order notifications are sent via **Azure Communication Services (ACS)**
- Email delivery is treated as a **side effect**, not part of core domain logic
- The Application layer expresses intent (e.g. *OrderPlaced*, *EmailConfirmationRequested*)
- The Infrastructure layer handles delivery, retries, and external service concerns
- Email failures do **not** cause request failures or corrupt transactional state

---

## Features

### Backend
- Product and category management
- Order creation with transactional integrity
- Background outbox publishers for events and emails
- Admin-only product and image management
- Role-based authorization using Azure Entra ID

### Frontend
- Product browsing with pagination and filtering
- Category, brand, and material filtering via query parameters
- Integration with secured API endpoints
- Responsive UI built with React and TypeScript

---

## Tech stack

### Backend
- ASP.NET Core
- Entity Framework Core
- SQLite (development) / MySQL-ready
- Azure Blob Storage
- Azure Entra ID (CIAM)
- Azure Service Bus (outbox publishing)
- Azure Communication Services (Email)

### Frontend
- React
- TypeScript
- Modern client-side routing and API consumption

---

## Running the project

This repository is intended as an **architectural and full-stack design showcase**.

Local execution requires configuration of:
- Azure Entra ID (App registrations & roles)
- Azure Blob Storage
- Azure Service Bus
- Azure Communication Services (Email)

Detailed setup steps are intentionally omitted to keep the focus on **architecture, design decisions, and production-style patterns** rather than quickstart scaffolding.

