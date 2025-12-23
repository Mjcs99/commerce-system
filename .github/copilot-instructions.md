# Copilot Instructions for E-Commerce System

## Project Overview

This is an **e-commerce system** with domain-driven architecture focused on correctness, concurrency handling, and idempotency. The README.md defines the core domain entities and database schema - treat this as the source of truth for data model design.

## Core Architectural Principles

### 1. **Inventory Concurrency Model**
- **Separation of Concerns**: Inventory (`InventoryItem`) is **intentionally decoupled** from Product to prevent contention. Never store inventory state in the Product entity.
- **Optimistic Locking**: `InventoryItem` uses `RowVersion` (timestamp/rowversion) for concurrency control - always check this on updates to prevent lost updates.
- **State Tracking**: Inventory tracks both `QuantityAvailable` and `QuantityReserved` as separate fields. `QuantityAvailable + QuantityReserved = physical stock`.

### 2. **Order Immutability & Snapshotting**
- Orders are **write-once after completion**. Design APIs to prevent post-fulfillment modifications.
- `OrderItem` stores product snapshots (`SKU`, `ProductName`, `UnitPriceAmount`) - these are historical records and must never reference the live Product entity.
- Order statuses follow strict state machine: `PendingPayment` → `Paid` → `Fulfilled` → (optionally) `Refunded` or `Cancelled`.

### 3. **Idempotency & Payment Handling**
- **Payment Deduplication**: `PaymentIntentId` (from external providers like Stripe/PayPal) must be unique. Use this for request deduplication.
- All monetary amounts use `decimal(18,2)` - never floating-point numbers.
- Orders cannot exist without a valid payment reference - enforce this at the domain boundary.

### 4. **Foreign Key Constraints**
- Products cannot be deleted if referenced by orders (due to historical snapshots).
- Maintain referential integrity: `Order.BuyerId` → Buyer, `Product.BrandId` → Brand, `InventoryItem.WarehouseLocationId` → WarehouseLocation.

## Data Model Guidelines

When implementing entities or storage:
1. **Currencies**: Use ISO-4217 3-letter codes (e.g., `USD`) in `char(3)` fields.
2. **Price Validation**: All `*Amount` fields must be `>= 0`. Implement in both domain logic and database constraints.
3. **Timestamps**: Both `CreatedAt` and `UpdatedAt` required for audit trails.
4. **Booleans**: Use `IsActive` boolean flags for soft-deletes rather than hard deletes.

## Common Implementation Patterns

### Example: Inventory Reservation
When code reserves inventory:
```
1. Load InventoryItem with current RowVersion
2. Validate: QuantityAvailable >= requested_quantity
3. Update: QuantityAvailable -= qty; QuantityReserved += qty
4. Submit with RowVersion for optimistic lock
5. On conflict: Retry or fail gracefully
```

### Example: Order Creation
1. Create Order entity with initial state: `PendingPayment`
2. Create OrderItem snapshots (copy product data at order time)
3. Link to Payment entity with external PaymentIntentId
4. Calculate totals from OrderItems + Tax + Shipping
5. Never modify Order after status transitions to `Paid`

## Development Workflow Notes

- **Schema Source**: All data model decisions are documented in README.md - review before implementing storage.
- **No Implementation Code Yet**: This project is in architectural/schema design phase. New developers should understand the entity relationships and constraints before building APIs/services.
- **Concurrency First**: This system is designed for high-concurrency scenarios - optimistic locking and idempotency are not optional.

## Testing Considerations

When writing tests:
- Test inventory reservation with concurrent updates (RowVersion conflicts)
- Test payment idempotency (same PaymentIntentId, different request attempts)
- Test order immutability (prevent modifications after fulfillment)
- Validate monetary calculations with edge cases (rounding, multi-currency)
