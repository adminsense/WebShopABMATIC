# Audit System — WebShopABMATIC (plan)

![Status](https://img.shields.io/badge/Status-Planning-f59e0b?style=flat-square) ![Phase](https://img.shields.io/badge/Phase%201-MVP%20badges-512BD4?style=flat-square) ![Template](https://img.shields.io/badge/Based%20on-IMMO%20template-informational?style=flat-square)

> **Purpose:** Plan and track audit logging for WebShopABMATIC before implementation (Auth-7).  
> **IMMO reference:** [AUDIT_TEMPLATE_TO BUILD.md](./AUDIT_TEMPLATE_TO%20BUILD.md) — full production spec (90+ actions, grid, modal).  
> **Identity baseline:** Auth-6 done — `ICurrentUserContext` + `CurrentUserSnapshot` for user/audit labels.  
> **Mark ✅ when done · ⬜ when pending.**

| Track | Scope | Status |
|-------|--------|--------|
| **Phase 1** | Badges + logging hooks: **CRUD**, **Login**, **Report**, **Logout** | ⬜ Not started |
| **Phase 2** | Admin grid `/admin/audit-logs`, filters, details modal | ⬜ Deferred |
| **Phase 3** | Domain-specific badges (Checkout, Mollie, Stock write, …) | ⬜ Deferred |

**Current focus:** _Phase 1 — design + badge vocabulary_

---

## 1. Why a new audit table (not only `OrderLogs`)

| Source | Table | Today | Fit for global audit? |
|--------|-------|-------|------------------------|
| Legacy ERP | `[Projects].[OrderLogs]` | `OrderId`, `UserId` (int), `Description` | ⬜ Order-scoped only — keep for dossier timeline |
| IMMO pattern | `AuditLogs` (app-owned) | Full action, JSON old/new, IP, severity | ✅ Target for Auth-7 |
| Identity | `AspNetUsers` | Login store | ⬜ Not an audit trail |

**Decision (proposed):**

- ⬜ **7.1a** Add **`Logging.AuditLogs`** (or `Audit.AuditLogs`) — new EF entity + migration, separate from legacy `OrderLogs`
- ⬜ **7.1b** Port **`IAuditService`** (Application) + **`AuditService`** (Infrastructure) — **single-layer** rule: repositories/use cases log **after** `SaveChangesAsync`, not Blazor pages
- ⬜ **7.1c** Store **`IdentityUserId`** (string) + optional **`LegacyStaffUserId`** (int) on each row — aligns with Auth-6 bridge

---

## 2. Phase 1 — Action badges (MVP only)

Start with **five families** from IMMO. Defer workflow, Mollie, file-manager, 2FA, contract, etc.

### 2.1 Badge legend (grid + modal)

| Badge | Action code | Bootstrap / CSS | Icon | When to use |
|-------|-------------|-----------------|------|-------------|
| 🟢 **CREATE** | `Create` | `badge bg-success` | — | New row persisted |
| 🔵 **UPDATE** | `Update` | `badge bg-primary` | — | Existing row changed |
| 🔴 **DELETE** | `Delete` | `badge bg-danger` | — | Row removed or soft-deleted |
| 🔵 **LOGIN** | `Login` | `badge bg-primary` | — | Successful authentication |
| 🔴 **LOGIN** (failed) | `LoginFailed` | `badge bg-danger` | — | Wrong password, locked account, wrong role |
| ⚫ **LOGOUT** | `Logout` | `badge bg-secondary` | — | Manual logout, session end |
| 🟣 **REPORT** | `ReportExport` | `badge bg-info` or `.audit-badge-report` `#6f42c1` | 📊 | Export / download of aggregated data |

**Phase 1 implementation checklist (badges):**

- ⬜ **B.1** Shared CSS in `wwwroot/css/admin.css` — `.audit-badge-*` aligned with IMMO colours
- ⬜ **B.2** Razor component `AuditActionBadge.razor` — maps `action` string → badge HTML
- ⬜ **B.3** Enum or constants `AuditActions` in Application — `Create`, `Update`, `Delete`, `Login`, `LoginFailed`, `Logout`, `ReportExport`
- ⬜ **B.4** Legend modal on future grid (copy IMMO “Badges legend” button) — can ship with Phase 2 UI

### 2.2 Severity & status (supporting, not separate badges)

| Field | Values | Notes |
|-------|--------|-------|
| **Severity** | Information, Warning, Error, Critical | Login failures → Warning; unhandled exception → Error |
| **Success** | ✅ / ❌ | Same as IMMO grid Status column |

---

## 3. Architecture (hexagonal)

```text
ProductList.razor
  → IProductAdminPort → ProductAdminUseCase → ProductRepository.SaveAsync
       → SaveChangesAsync
       → IAuditService.LogAsync(Create|Update|Delete, entity: Product, …)

SignIn.razor / AdminLogin.razor
  → SignInManager (after result)
       → IAuditService.LogAsync(Login|LoginFailed, …)

Store checkout (future badge phase)
  → CheckoutUseCase → StoreOrderRepository
       → IAuditService.LogAsync(Create, entity: Order, …)
```

| Concern | Location (proposed) |
|---------|---------------------|
| DTO / action codes | `Application/Audit/AuditLogEntry.cs`, `AuditActions.cs` |
| Inbound port | `IAuditService` |
| Outbound port | `IAuditLogRepository` |
| Writer | `Infrastructure/Audit/AuditService.cs` |
| Entity + map | `Model/Entities/AuditLog.cs`, `WebShopABMATICModelBuilder` |
| User context | Existing `ICurrentUserContext` |
| IP / User-Agent | `IHttpContextAccessor` inside `AuditService` |

**Rules (from IMMO — apply here):**

- ⬜ **A.1** One audit row per **business operation** (no duplicate page + repository logs)
- ⬜ **A.2** Do **not** use `ILogger` for compliance trail — use `IAuditService` only
- ⬜ **A.3** Never log passwords, tokens, Mollie secrets, or full payment payloads
- ⬜ **A.4** `OldValues` / `NewValues` as JSON snapshots (field-level diff optional later)

---

## 4. Authentication events (Login / Logout)

| Event | Action | Route / trigger | Policy | Log? |
|-------|--------|-----------------|--------|------|
| Admin login success | `Login` | `/Account/Login` or `/admin/login` | Admin/Manager | ⬜ |
| Admin login failed | `LoginFailed` | same | — | ⬜ |
| Store login success | `Login` | `/sign-in` | Customer | ⬜ |
| Store login failed | `LoginFailed` | `/sign-in` | — | ⬜ |
| Store registration | `Create` | `/sign-up` | Entity: `ApplicationUser` + `Customer` | ⬜ (or dedicated `Register` later) |
| Admin logout | `Logout` | Admin layout top bar | — | ⬜ |
| Store session end | `Logout` | Store layout / circuit close | — | ⬜ |
| Password reset (admin) | `Update` | System users / Customers reset modal | Admin | ⬜ |

**NewValues examples:**

- Login success: `{ "email": "admin@webshop.com", "roles": ["Admin","Manager"] }`
- Login failed: `{ "email": "…", "reason": "InvalidPassword" }` — no password field

**Infrastructure note:** ⬜ Optional `AppCircuitHandler` (see [CODE_PATTERNS_AND_INFRASTRUCTURE.md](./CODE_PATTERNS_AND_INFRASTRUCTURE.md)) for **Logout** on circuit closed — same as IMMO session end.

---

## 5. Report events (Report badge)

WebShop has **no** IMMO-style report suite yet. Phase 1 defines **reportKey** vocabulary for future exports.

| reportKey (Entity column) | Trigger (future) | Format | Status |
|--------------------------|------------------|--------|--------|
| `StockMovementsReport` | `/admin/stock/movements` export | CSV | ⬜ |
| `StockOverviewReport` | `/admin/stock/overview` export | CSV | ⬜ |
| `OrdersReport` | `/admin/orders` export | CSV/PDF | ⬜ |
| `CustomersReport` | `/admin/customers` export | CSV | ⬜ |
| `ProductsCatalogReport` | `/admin/products` export | CSV | ⬜ |

**Log shape (IMMO-compatible):**

```json
{
  "reportKey": "StockMovementsReport",
  "format": "csv",
  "filters": { "dateFrom": "2026-01-01", "productId": null }
}
```

- ⬜ **R.1** `ReportExport` action + purple badge when first export endpoint exists
- ⬜ **R.2** Until exports exist: no `ReportExport` rows — badge spec ready only

---

## 6. CRUD inventory — admin entities

Each row: **entity** → **route** → **repository/use case** → log **Create / Update / Delete**.

Legend: ✅ logged · ⬜ not wired · n/a no operation

### 6.1 Webshop hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `WebshopStructure` | `/admin/webshop-structures` | `WebshopStructureRepository` | ⬜ | ⬜ | ⬜ |
| `WebshopProductStructure` | `/admin/webshop-product-structures` | `WebshopProductStructureRepository` | ⬜ | ⬜ | ⬜ |

### 6.2 Catalog hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `Product` | `/admin/products` | `ProductRepository` + `ProductAdminUseCase` | ⬜ | ⬜ | ⬜ (soft) |
| `ProductPrice` | `/admin/product-prices` | `ProductPriceRepository` | ⬜ | ⬜ | ⬜ |
| `ProductQuantityTier` | `/admin/product-tiers` | `ProductQuantityTierRepository` | ⬜ | ⬜ | ⬜ |
| `ProductOption` | `/admin/product-options` | `ProductOptionRepository` | ⬜ | ⬜ | ⬜ |
| `PriceListCategory` | `/admin/price-list-categories` | `PriceListCategoryRepository` | ⬜ | ⬜ | ⬜ |
| `Manufacturer` | `/admin/manufacturers` | `ManufacturerRepository` | ⬜ | ⬜ | ⬜ |
| `Supplier` | `/admin/suppliers` | `SupplierRepository` | ⬜ | ⬜ | ⬜ |
| `AzureFile` (product image) | via `LocalProductMediaService` | `IProductMediaPort` | ⬜ | ⬜ | n/a |

### 6.3 Customers hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `Customer` | `/admin/customers` | `CustomerRepository` | ⬜ | ⬜ | ⬜ |
| `CustomerDeliveryAddress` | `/admin/delivery-addresses` | `CustomerDeliveryAddressRepository` | ⬜ | ⬜ | ⬜ |
| `CustomerProductDiscount` | `/admin/customer-discounts` | `CustomerProductDiscountRepository` | ⬜ | ⬜ | ⬜ |
| `CustomerType` | `/admin/customer-types` | `CustomerTypeRepository` | ⬜ | ⬜ | ⬜ |

### 6.4 Sales hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `Order` | `/admin/orders` | `OrderRepository` | ⬜ | ⬜ | n/a |
| `OrderStatus` | `/admin/order-statuses` | `OrderStatusRepository` | ⬜ | ⬜ | ⬜ |
| `DeliveryType` | `/admin/delivery-types` | `DeliveryTypeRepository` | ⬜ | ⬜ | ⬜ |

### 6.5 Stock hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `StockLocation` | `/admin/stock-locations` | `StockLocationRepository` | ⬜ | ⬜ | ⬜ |
| `ProductStockLocation` | `/admin/product-stock` | `ProductStockLocationRepository` | ⬜ | ⬜ | ⬜ |
| Stock overview / movements | `/admin/stock/*` | read-only today | n/a | n/a | n/a |

### 6.6 Settings & identity

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `SystemUser` (Identity) | `/admin/system-users` | `SystemUserRepository` | ⬜ | ⬜ | n/a |
| `PaymentMethod` | `/admin/payment-methods` | `PaymentMethodRepository` | ⬜ | ⬜ | ⬜ |
| `VatType` | `/admin/vat-types` | `VatTypeRepository` | ⬜ | ⬜ | ⬜ |
| `StaffUser` (legacy HR) | `/admin/staff-users` | `StaffUserRepository` | ⬜ | ⬜ | ⬜ |
| `UserGroup` (legacy) | `/admin/user-groups` | `UserGroupRepository` | ⬜ | ⬜ | ⬜ |
| Profile (self) | `/admin/profile` | Identity `UserManager` | n/a | ⬜ | n/a |

### 6.7 Storefront (non-admin CRUD)

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `Order` + lines (checkout) | `/cart` → checkout | `StoreOrderRepository` | ⬜ | n/a | n/a |
| `Customer` + Identity (sign-up) | `/sign-up` | `CustomerRegistrationRepository` | ⬜ | n/a | n/a |
| Mollie webhook (paid) | `/api/webhooks/mollie/payments` | `ProcessMollieWebhookUseCase` | ⬜ (Phase 3: `PaymentPaid`) | n/a | n/a |

**CRUD summary:** 0 / ~60 operations wired · target Phase 1: **infrastructure + login/logout + 2–3 pilot entities** (e.g. `Product`, `Customer`, `Order` checkout)

---

## 7. Proposed `AuditLog` row shape

| Column | Type | Notes |
|--------|------|-------|
| `Id` | `bigint` identity | PK |
| `Timestamp` | `datetime2` UTC | Default `GETUTCDATE()` |
| `Action` | `nvarchar(50)` | `Create`, `Update`, `Delete`, `Login`, … |
| `EntityName` | `nvarchar(100)` | e.g. `Product`, `Order`, `ReportExport` |
| `EntityId` | `nvarchar(50)` | String for flexibility (Identity ids) |
| `IdentityUserId` | `nvarchar(450)` | AspNet user id |
| `LegacyStaffUserId` | `int?` | From `ICurrentUserContext.StaffUserId` |
| `UserDisplayName` | `nvarchar(256)` | Email or name at time of action |
| `Severity` | `nvarchar(20)` | Information / Warning / Error / Critical |
| `Success` | `bit` | |
| `ErrorMessage` | `nvarchar(max)` | When failed |
| `IpAddress` | `nvarchar(45)` | |
| `UserAgent` | `nvarchar(512)` | |
| `OldValues` | `nvarchar(max)` | JSON |
| `NewValues` | `nvarchar(max)` | JSON |
| `DurationMs` | `int?` | Optional performance |
| `AdditionalInfo` | `nvarchar(max)` | Optional JSON |

- ⬜ **7.1d** Indexes: `(Timestamp DESC)`, `(IdentityUserId, Timestamp)`, `(EntityName, EntityId)`, `(Action, Success)`

---

## 8. Phase 2 — Admin UI (IMMO parity, deferred)

| Item | IMMO | WebShop plan | Status |
|------|------|--------------|--------|
| Route | `/audit-logs` | `/admin/audit-logs` | ⬜ |
| Access | Admin only | `AppPolicies.AdminOnly` | ⬜ |
| Menu | Sidebar + clipboard icon | Settings hub card or sidebar entry | ⬜ |
| Grid filters | Date, Action, Severity, User, Entity, Status | Same subset (Phase 1 actions only) | ⬜ |
| Details modal | Old/New JSON, IP, duration | Same pattern | ⬜ |
| Pagination | 50/page | 50/page | ⬜ |

---

## 9. Phase 3 — Extra badges (explicitly out of Phase 1)

Defer until MVP stable — listed so they are not confused with Phase 1 scope.

| Badge | Action | When |
|-------|--------|------|
| 💳 | `CheckoutStarted` / `PaymentPaid` | Mollie flow |
| 📦 | `StockAdjust` | Phase D stock writes |
| 🔐 | `PasswordReset` | Already in Auth-4 UI |
| 📄 | `CREATEDOC` / product media | AzureFile uploads |

---

## 10. Implementation roadmap (checklist)

### Foundation

- ⬜ **F.1** `AuditLog` entity + EF migration applied locally
- ⬜ **F.2** `IAuditService` + `AuditService` + DI registration
- ⬜ **F.3** `AuditActionBadge.razor` + admin.css badge classes
- ⬜ **F.4** Unit/integration smoke: write + read one log row

### Phase 1 — Logging hooks

- ⬜ **L.1** Login / LoginFailed — admin + store
- ⬜ **L.2** Logout — admin (+ optional circuit handler)
- ⬜ **L.3** Pilot CRUD: `Product` Create/Update/Delete (soft)
- ⬜ **L.4** Pilot CRUD: `Customer` Create/Update/Delete
- ⬜ **L.5** Store checkout `Order` Create
- ⬜ **L.6** First `ReportExport` when export API exists

### Phase 2 — Read UI

- ⬜ **U.1** `IAuditLogAdminPort` + read-only grid
- ⬜ **U.2** Filters + legend modal
- ⬜ **U.3** Details modal
- ⬜ **U.4** Hub card + `ADMIN.md` section

### Documentation sync

- ⬜ **D.1** Update [AUTH_IDENTITY_ROADMAP.md](./AUTH_IDENTITY_ROADMAP.md) Auth-7 with link here
- ⬜ **D.2** Update [INFRASTRUCTURE.md](./INFRASTRUCTURE.md) § audit when `IAuditService` exists

---

## 11. Quick progress

```
Foundation   [__________] 0/4
Badges (B)   [__________] 0/4
Auth (L)     [__________] 0/2
CRUD pilot   [__________] 0/3
Reports (R)  [__________] 0/2
Admin UI (U) [__________] 0/4
```

---

## 12. What we are **not** copying from IMMO (yet)

- Workflow badges (`WKFLOWSAVE`, …)
- Payment schedule / rent index badges
- File manager / Word viewer document badges
- 2FA enable/disable
- Contract status transitions
- 90+ action catalogue — WebShop MVP targets **~7 action codes** in Phase 1

---

## Documentation

- 📋 [IMMO audit template](./AUDIT_TEMPLATE_TO%20BUILD.md) — full reference implementation
- 🔐 [Auth identity roadmap — Auth-7](./AUTH_IDENTITY_ROADMAP.md)
- 🖥️ [Admin — user context §2.7](./ADMIN.md)
- 🏗️ [Infrastructure patterns](./CODE_PATTERNS_AND_INFRASTRUCTURE.md)
- 🏠 [Main README](../README.md)

---

**© 2026 AdminSense. All rights reserved.**
