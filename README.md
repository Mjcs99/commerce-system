# Core Domain Entities

This document defines the core database entities for the e-commerce system.
The schema is designed to enforce correctness, support concurrency, and
handle real-world failure scenarios (retries, duplicate events, partial
failures).

---

## Product

Represents a sellable item. Products are relatively stable and do **not**
contain inventory state.

### Fields
- `ProductId` (GUID, PK)
- `SKU` (string, unique)
- `Name` (string)
- `Description` (string)
- `BrandId` (GUID, FK)
- `PriceAmount` (decimal(18,2))
- `PriceCurrency` (char(3), ISO-4217)
- `IsActive` (bool)
- `CreatedAt` (datetime)
- `UpdatedAt` (datetime)

### Constraints
- `SKU` must be unique
- `PriceAmount >= 0`
- Products cannot be deleted if referenced by orders

---

## InventoryItem

Represents inventory state for a product at a specific location.
This entity is concurrency-sensitive.

### Fields
- `InventoryItemId` (GUID, PK)
- `ProductId` (GUID, FK)
- `WarehouseLocationId` (GUID, FK)
- `QuantityAvailable` (int)
- `QuantityReserved` (int)
- `RowVersion` (rowversion / timestamp)

### Constraints
- `QuantityAvailable >= 0`
- `QuantityReserved >= 0`
- `(QuantityAvailable + QuantityReserved)` represents physical stock
- Inventory updates must be transactional

---

## WarehouseLocation

Represents a physical or logical inventory location.

### Fields
- `WarehouseLocationId` (GUID, PK)
- `Name` (string)
- `Region` (string)
- `IsActive` (bool)

---

## Brand

Represents a product brand.

### Fields
- `BrandId` (GUID, PK)
- `Name` (string, unique)
- `IsActive` (bool)

---

## Order

Represents a customer order. Orders are immutable once completed and store
snapshots of pricing and product information.

### Fields
- `OrderId` (GUID, PK)
- `BuyerId` (GUID)
- `Status` (enum: `PendingPayment`, `Paid`, `Fulfilled`, `Cancelled`, `Refunded`)
- `SubtotalAmount` (decimal(18,2))
- `TaxAmount` (decimal(18,2))
- `ShippingAmount` (decimal(18,2))
- `TotalAmount` (decimal(18,2))
- `Currency` (char(3))
- `PaymentProvider` (string)
- `PaymentIntentId` (string, unique)
- `CreatedAt` (datetime)
- `UpdatedAt` (datetime)

### Constraints
- `PaymentIntentId` must be unique (idempotency)
- Orders cannot be modified after fulfillment
- Orders cannot exist without a valid payment reference

---

## OrderItem

Represents a single line item within an order.
All product data is stored as a snapshot to preserve historical accuracy.

### Fields
- `OrderItemId` (GUID, PK)
- `OrderId` (GUID, FK)
- `ProductId` (GUID, FK)
- `SKU` (string, snapshot)
- `ProductName` (string, snapshot)
- `UnitPriceAmount` (decimal(18,2), snapshot)
- `Quantity` (int)
- `LineTotalAmount` (decimal(18,2))

### Constraints
- `Quantity > 0`
- `LineTotalAmount = UnitPriceAmount * Quantity`

---

## Payment 

Represents the payment lifecycle independently from the order lifecycle.

### Fields
- `PaymentId` (GUID, PK)
- `OrderId` (GUID, FK)
- `Provider` (string)
- `ProviderPaymentId` (string, unique)
- `Amount` (decimal(18,2))
- `Currency` (char(3))
- `Status` (enum)
- `CreatedAt` (datetime)

---

## Design Notes

- Inventory is intentionally separated from Product to prevent contention
  and overselling.
- Orders and OrderItems store snapshots to ensure historical correctness.
- Idempotency is enforced using unique external references (e.g. payment IDs).
- All monetary values use decimals to avoid floating-point errors.

