# Web Store — Functional Specification

![Status](https://img.shields.io/badge/Status-Blazor%20storefront%20live-28a745?style=flat-square) ![Auth](https://img.shields.io/badge/Login-Legacy%20WebshopLogin-512BD4?style=flat-square) ![Orders](https://img.shields.io/badge/My%20orders-%2Forders-0dcaf0?style=flat-square)

> [!IMPORTANT]
> **Executive Summary:** B2B storefront for catalog, cart, checkout (Mollie mock PrePay), and **customer account area** (profile + **order history**). Customer never uses `/admin` for their own purchases — that is staff-only ([SPEC_ADMIN.md](./SPEC_ADMIN.md)).

### Coverage statistics

| Category | Count | Status | Notes |
|----------|-------|--------|-------|
| **Auth flows** | 2 | ✅ | Customer `/sign-in`; staff `/admin/login` (separate ERP tables) |
| **Account area** | 3 | ✅ | `/my-account`, `/orders`, `/orders/{id}` |
| **Checkout** | PrePay | ✅ | Cart → Mollie mock → payment-return → confirmation |

### Implementation quality

| Aspect | Status | Details |
|--------|--------|---------|
| **Catalog UX** | ✅ | `IStoreCatalogPort` — lazy products per category; icons on demand |
| **Checkout** | ✅ | `CheckoutUseCase` + Mollie mock; stock on pay |
| **Customer login** | ✅ | Legacy `WebshopLogin` + hash/salt → role `Customer` |
| **Order history** | ✅ | Header **My orders** → `/orders`; detail `/orders/{id}` |
| **Staff entry** | ✅ | Header **Admin** → `/admin/login` (`StaffUsers`) |

---

## Overview

| Artifact | Path | Role |
|----------|------|------|
| **Store UI** | `WebShopABMATIC.Client/Components/Pages/Store/` | Blazor storefront |
| **Admin data** | ERP tables via repositories | Maintained in admin panel |
| **Admin spec** | [SPEC_ADMIN.md](./SPEC_ADMIN.md) | Staff panel + auth §2 |

### Implementation status

| Area | Blazor | Backend |
|------|--------|---------|
| **Catalog browse** | ✅ `Catalog.razor` | `StoreCatalogService` |
| **Product detail** | ✅ `ProductDetail.razor` | Same port |
| **Cart / checkout** | ✅ `Cart.razor` | `StoreCartService` + `ICheckoutPort` |
| **Orders list** | ✅ `Orders.razor` | `ICheckoutPort.GetCustomerOrdersAsync` |
| **Order detail / confirmation** | ✅ | `GetOrderSummaryAsync` |
| **Customer sign-in** | ✅ `SignIn.razor` | `POST /account/store-login` |
| **Admin entry** | ✅ Header **Admin** | `POST /account/admin-login` |

### Backend architecture (hexagonal)

Store pages inject **inbound ports** only — same hexagonal stack as admin:

```text
Catalog.razor
  → IStoreCatalogPort              (Application/Ports)
  → StoreCatalogService            (Infrastructure/Store)
  → WebShopABMATICDbContext + IProductMediaPort
```

Future: `IOrderService` / `ICartService` as inbound ports with use cases in Application; checkout persists via outbound `IOrderRepository`.

---

## 🛒 1. Visual design and catalog imagery

The storefront uses a **light blue** theme (`--primary: #0ea5e9`, soft backgrounds). Product cards show image, name, price, and stock hint.

### 1.1 Catalog products (prototype SKUs)

| SKU | Image | Mock name | Category | Mock stock |
|-----|-------|-----------|----------|------------|
| 1 | ![Hard drive 1](docs/images/product1.png) | Hard drive 1 | storage | 24 |
| 2 | ![Hard drive 2](docs/images/product2.png) | Hard drive 2 | storage | 18 |
| 3 | ![Hard drive 3](docs/images/product3.png) | Hard drive 3 | ssd | 32 |
| 4 | ![Hard drive 4](docs/images/product4.png) | Hard drive 4 | ssd | 15 |
| 5 | ![Hard drive 5](docs/images/product5.png) | Hard drive 5 | hdd | 9 (low) |
| 6 | ![Hard drive 6](docs/images/product6.png) | Hard drive 6 | hdd | 41 |

**Production mapping:** Each row becomes a `Product` with `ShowOnWebshop = true`, linked `ProductPrice` for `GrossSalesPrice`, and `ProductStockLocation.Quantity` for availability.

### 1.2 Screen regions (prototype)

| Region | Purpose |
|--------|---------|
| **Header** | Logo, search, account menu, cart badge |
| **Category chips** | Filter by `WebshopStructure` / category id |
| **Product grid** | Cards with image, price, stock line |
| **Product detail** | Large image, description, options, quantity, add to cart |
| **Cart drawer / page** | Line items, quantities, subtotal |
| **Checkout** | Delivery address, delivery type, payment method, confirm |
| **Account** | Profile, orders history, delivery addresses |
| **Footer** | Link to admin panel (staff) |

Open the prototype:

```text
docs/mock-loja.html
```

(relative to repository root; open in browser or via static file server)

---

## 🔐 2. Authentication and login

### 2.1 Customer identity model

| Concept | Entity / field | Description |
|---------|----------------|-------------|
| **Store login** | `Customer.WebshopLogin` / email | Shop username |
| **Password** | `PasswordWebshop` + `SaltWebshop` | Legacy hash (not AspNetUsers at runtime) |
| **Role** | `AppRoles.Customer` | Policy `CustomerOnly` for store routes |

> [!NOTE]
> Customers typically get credentials from admin (`WebshopLogin`). Self-register exists at `/sign-up` when enabled.

### 2.2 Login flow

```mermaid
sequenceDiagram
  participant C as Customer
  participant S as Web Store
  participant L as LegacySignInService
  participant DB as Customers table

  C->>S: Enter WebshopLogin + password
  S->>L: POST /account/store-login
  L->>DB: Resolve Customer + verify hash
  L-->>S: Cookie + Customer role + CustomerId
  S-->>C: Store with My orders / My account
```

| Step | Behaviour |
|------|-----------|
| 1 | Customer opens **Login** → `/sign-in` |
| 2 | Enters webshop login + password |
| 3 | Cookie session; `CustomerId` for pricing, addresses, orders |
| 4 | Header shows **My orders** + account name |

**Runtime:** `Customers.WebshopLogin` + hash/salt on `abmatic_test`.

### 2.3 Logout

- Header **Sign out** → `/account/logout`. Guest may browse catalog; checkout needs login.

### 2.4 Staff access from store

- Header **Admin** → `/admin/login` with **StaffUsers** credentials (separate from customer).
- Customers must not access `/admin/*` (`AdminOrManager` policy).
- **Customer order history is never in admin for that buyer’s self-service** — use store **My orders**.

---

## 📋 3. Registrations and master data (what the store consumes)

The store does not own master data; it **reads** configurations maintained in the admin panel.

### 3.1 Data dependencies

| Admin registration | Store usage |
|--------------------|-------------|
| **Product** + `ShowOnWebshop` | Visible catalog |
| **ProductPrice** | Current valid sales price per product |
| **ProductQuantityTier** | Volume discount at quantity breaks |
| **ProductOption** | Configurable lines on product detail |
| **WebshopStructure** / **WebshopProductStructure** | Navigation and category filters |
| **Customer** | Login, company name, default terms |
| **CustomerDeliveryAddress** | Checkout ship-to selection |
| **CustomerProductDiscount** | Customer-specific price override |
| **CustomerType** | Default discount %, delivery defaults |
| **DeliveryType** | Checkout delivery options and costs |
| **PaymentMethod** | Checkout payment choice |
| **VatType** | Line and order VAT calculation |
| **ProductStockLocation** | Stock hints and cart validation |

### 3.2 Customer-facing “registrations”

| Action | Who | Result |
|--------|-----|--------|
| **Account created** | Admin | New `Customer` + `WebshopLogin` |
| **Delivery address added** | Customer (profile) or Admin | `CustomerDeliveryAddress` |
| **Order placed** | Customer | New `Order` + `OrderLine` rows |
| **Password change** | Customer | Update Identity password (and legacy hash if synced) |

---

## 🧩 4. Storefront functionality

### 4.1 Catalog and search

| Feature | Description | Validation / rules |
|---------|-------------|-------------------|
| **Product list** | Grid of products with image, name, price | Only `ShowOnWebshop = true` |
| **Category filter** | Chips map to `WebshopStructure` ids | Empty category shows no products |
| **Search** | Text match on name, part number, EAN | Case-insensitive (planned server-side) |
| **Sort** | Price, name (planned) | — |

### 4.2 Product detail

| Feature | Description |
|---------|-------------|
| **Hero image** | From product media or default asset |
| **Meta line** | `ProductId`, `ShowOnWebshop`, tags |
| **Description** | `WebshopDescriptionNl` / EN / FR |
| **Price** | Current `ProductPrice.GrossSalesPrice` (customer discounts applied) |
| **Options** | Required/optional `ProductOption` values |
| **Stock line** | e.g. “24 in stock” from stock location |
| **Quantity** | Spinner before add to cart |
| **Add to cart** | Creates/updates cart line with options |

### 4.3 Cart

| Feature | Description |
|---------|-------------|
| **Line items** | Product, qty, unit price, option surcharges |
| **Update qty** | Recalculate tiers and totals |
| **Remove line** | — |
| **Subtotal / VAT** | Per `VatType` on lines |
| **Persistence** | Logged-in: server cart; guest: session (TBD) |

### 4.4 Checkout

| Step | Fields / logic |
|------|----------------|
| **Delivery address** | Select `CustomerDeliveryAddress` or default |
| **Delivery type** | `DeliveryType` — cost rules per type |
| **Payment method** | `PaymentMethod` |
| **Review** | Lines, discounts, VAT, total |
| **Submit** | Create `Order`, `OrderLine`; trigger stock reservation per `OrderStatus` |

### 4.5 Account area (logged-in customer)

| Screen | Route | Content |
|--------|-------|---------|
| **My orders** | `/orders` | List of this customer’s orders + payment status |
| **Order detail** | `/orders/{id}` | Lines, totals, Mollie id when PrePay |
| **Order confirmation** | `/orders/{id}/confirmation` | After successful pay; links back to My orders |
| **My account** | `/my-account` | Profile + link to My orders; password change |
| **Nav** | `StoreHeader` | **My orders** + account name when role `Customer` |

> [!IMPORTANT]
> After checkout, the customer stays in the **store** account area. Staff use `/admin/orders` to see **all** customers’ orders.

---

## 📦 5. Stock validation

Stock behaviour must stay **consistent** with admin rules ([SPEC_ADMIN.md §4](SPEC_ADMIN.md#4-stock-validation-and-alerts)).

### 5.1 Display rules (catalog and detail)

| Condition | UI behaviour | Implementation |
|-----------|----------------|----------------|
| `available > MinQuantity` (or min = 0) | Green “N in stock” | `StoreProductDto` from default location |
| `available <= MinQuantity` and `> 0` | Orange **low** class | `IsLowStock` — uses DB `MinQuantity`, not hardcoded 10 |
| `available = 0` | “Out of stock” | `IsOutOfStock` |
| Product not on webshop | Hidden | `ShowOnWebshop != true` |

**Implemented** in `StoreCatalogService`, `Catalog.razor`, `ProductDetail.razor` (May 2026).

### 5.2 Cart and checkout validation (planned)

| Rule | When | Action |
|------|------|--------|
| **Reserve on submit** | Order created with initial status | ⬜ Not used — **decrement on pay** (PrePay) or checkout (PostPay) via `IStockMovementService` |
| **Sufficient stock** | Add to cart / checkout | ✅ Reject if `requestedQty > available` |
| **Consume on fulfilment** | Status with `AffectsStock` | Decrease `Quantity`, release reservation |
| **Multi-location** | Warehouse selection (future) | Pick `ProductStockLocation` with `IsDefault` or nearest |

> [!WARNING]
> The HTML prototype does **not** enforce server-side stock checks. Implement validation in the application service before persisting `OrderLine` rows.

### 5.3 Order status interaction

| `OrderStatus` flag | Effect on stock |
|--------------------|-----------------|
| `ReserveStock = true` | Reserve quantity when order enters status |
| `AffectsStock = true` | Deduct on-hand when order reaches status |

Configured by staff in admin → **Sales** → **Order status**.

---

## 💰 6. Pricing and discounts

| Source | Applied when |
|--------|--------------|
| **ProductPrice** (valid date range) | All customers — base list price |
| **ProductQuantityTier** | Line quantity meets `MinimumQuantity` |
| **CustomerProductDiscount** | Logged-in customer, matching product |
| **CustomerType** base discount | Default % for customer segment |

**Display:** Show struck-through list price when discount applies (planned).

---

## 📊 7. Dashboards (customer vs operations)

### 7.1 Customer-facing (store)

| View | Purpose |
|------|---------|
| **Order history** | Status, date, total, lines |
| **Open orders** | Awaiting acceptance / shipment |
| **Quick reorder** | Copy lines from past `Order` (planned) |

No financial YTD dashboard on the store — that remains **admin** ([SPEC_ADMIN.md §5](SPEC_ADMIN.md#5-dashboards-and-reporting)).

### 7.2 Operational visibility (admin only)

Store activity appears on the **admin dashboard**:

- Orders this month / pending acceptance
- Products on webshop count
- Low stock alerts affecting catalog availability

---

## ✅ 8. Validations summary

| Area | Rule |
|------|------|
| **Login** | Valid credentials; account active; lockout after failed attempts (Identity) |
| **Catalog** | `ShowOnWebshop`; inactive products excluded |
| **Cart qty** | Integer &gt; 0; max per tier if configured |
| **Stock** | Available quantity ≥ line qty at checkout |
| **Required options** | All `ProductOption` with `IsRequired` selected |
| **Checkout** | Delivery address and type required; payment method required |
| **VAT** | Valid `VatType` on lines |
| **Authorization** | Customer may only see own `Order` and `CustomerId` data |

---

## 🔄 9. User journeys

```mermaid
flowchart LR
  Browse[Browse catalog] --> Detail[Product detail]
  Detail --> Cart[Add to cart]
  Cart --> Login{Logged in?}
  Login -->|No| SignIn[Sign in]
  SignIn --> Checkout[Checkout]
  Login -->|Yes| Checkout
  Checkout --> Order[Order created]
  Order --> Confirm[Confirmation page]
```

### 9.1 Guest vs logged-in

| Capability | Guest | Logged-in customer |
|------------|-------|-------------------|
| Browse catalog | ✅ | ✅ |
| View prices | List price | List + customer discounts |
| Add to cart | ✅ (session) | ✅ (persisted) |
| Checkout | ❌ | ✅ |
| Order history | ❌ | ✅ |

---

## 🗺️ 10. Prototype vs production roadmap

| Milestone | Deliverable |
|-----------|-------------|
| **M1** | HTML prototype — UX sign-off (`mock-loja.html`) |
| **M2** | Blazor storefront project, shared Application/Infrastructure |
| **M3** | Identity Customer login bound to `Customer.WebshopLogin` |
| **M4** | Live catalog from SQL + real images |
| **M5** | Cart, checkout, `Order` creation, stock validation |
| **M6** | Customer account area and order tracking |

---

## 📁 11. Related files

| File | Description |
|------|-------------|
| `docs/mock-loja.html` | Full storefront prototype |
| `docs/images/product*.png` | Product thumbnails |
| `docs/mock-admin.html` | Admin prototype (linked from store footer) |
| [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md) | Screen-by-screen entity mapping |

### Run prototype

Open `docs/mock-loja.html` in a browser. No build required.

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**
