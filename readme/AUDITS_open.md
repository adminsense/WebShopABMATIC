# Audit System — WebShopABMATIC

![Status](https://img.shields.io/badge/Status-Complete-22c55e?style=flat-square) ![Phase](https://img.shields.io/badge/Phase%203-core%20done-512BD4?style=flat-square) ![Reference](https://img.shields.io/badge/Based%20on-AB-MATIC-informational?style=flat-square)

> **Purpose:** Plan and track audit logging for WebShopABMATIC (Auth-7).  
> **AB-MATIC reference:** full production spec (90+ actions, grid, modal) — original template doc removed from repo; this file is the live tracker.  
> **Identity baseline:** Auth-6 done — `ICurrentUserContext` + `CurrentUserSnapshot` for user/audit labels.  
> **Mark ✅ when done · ⬜ when pending.**

| Track | Scope | Status |
|-------|--------|--------|
| **Phase 1** | Badges + logging hooks: **CRUD**, **Login**, **Report**, **Logout** | ✅ Done |
| **Phase 2** | Admin grid `/admin/audit-logs`, filters, details modal | ✅ Done |
| **Phase 3** | Checkout / Mollie / PasswordReset badges | ✅ Core done (`StockAdjust` deferred) |

**Current focus:** _Phase 6 — `StockAdjust` audit badge (movement API exists)_

---

## 1. Why a new audit table (not only `OrderLogs`)

| Source | Table | Today | Fit for global audit? |
|--------|-------|-------|------------------------|
| Legacy ERP | `[Projects].[OrderLogs]` | `OrderId`, `UserId` (int), `Description` | ⬜ Order-scoped only — keep for dossier timeline |
| AB-MATIC pattern | `AuditLogs` (app-owned) | Full action, JSON old/new, IP, severity | ✅ Implemented (`ApplicationDbContext`) |
| Identity | `AspNetUsers` | Login store | ⬜ Not an audit trail (events logged to `AuditLogs`) |

**Decision (implemented):**

- ✅ **7.1a** **`AuditLogs`** table — EF entity `Infrastructure/Identity/AuditLog.cs`, migration `20260607132504_AuditLogs`, separate from legacy `OrderLogs`
- ✅ **7.1b** **`IAuditService`** (Application) + **`AuditService`** (Infrastructure) — domain CRUD via **`AuditSaveChangesInterceptor`** on `WebShopABMATICDbContext`; Identity/auth/export via explicit `LogAsync` calls
- ✅ **7.1c** **`IdentityUserId`** (string) + optional **`LegacyStaffUserId`** (int) on each row — from `ICurrentUserContext` / request override

---

## 2. Phase 1 — Action badges (MVP only)

Start with **five families** from AB-MATIC. Defer workflow, Mollie, file-manager, 2FA, contract, etc.

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

- ✅ **B.1** Shared CSS in `wwwroot/css/admin.css` — `.audit-badge-*` aligned with AB-MATIC colours
- ✅ **B.2** Razor component `AuditActionBadge.razor` — maps `action` string → badge HTML
- ✅ **B.3** Constants `AuditActions` in `Application/Audit/AuditActions.cs` — `Create`, `Update`, `Delete`, `Login`, `LoginFailed`, `Logout`, `ReportExport`
- ✅ **B.4** Legend modal on `/admin/audit-logs` (“Badges legend” button)

### 2.2 Severity & status (supporting, not separate badges)

| Field | Values | Notes |
|-------|--------|-------|
| **Severity** | Information, Warning, Error, Critical | Login failures → Warning; unhandled exception → Error |
| **Success** | ✅ / ❌ | Same as AB-MATIC grid Status column |

---

## 3. Architecture (hexagonal)

```text
WebShopABMATICDbContext.SaveChangesAsync
  → AuditSaveChangesInterceptor (after successful save)
       → IAuditService.LogAsync(Create|Update|Delete, entity name + JSON snapshot)

AdminLogin.razor / SignIn.razor / Account/Logout / StoreHeader
  → IAuditService.LogAsync(Login|LoginFailed|Logout, …)

GridExportService (all admin *List exports)
  → IAuditService.LogAsync(ReportExport, reportKey = FileBaseName, …)

ApplicationUserAccountRepository / CustomerRegistrationRepository / IdentityPasswordService
  → IAuditService.LogAsync(Create|Update, entity: ApplicationUser, …)
```

| Concern | Location (implemented) |
|---------|------------------------|
| Action codes | `Application/Audit/AuditActions.cs`, `AuditSeverity` |
| Inbound port | `IAuditService` |
| Outbound port | `IAuditLogRepository` |
| Writer | `Infrastructure/Audit/AuditService.cs` |
| Domain CRUD hook | `Infrastructure/Audit/AuditSaveChangesInterceptor.cs` |
| Entity + map | `Infrastructure/Identity/AuditLog.cs`, `ApplicationDbContext` |
| User context | `ICurrentUserContext` |
| IP / User-Agent | `AuditLogRepository.AddAsync` via `IHttpContextAccessor` |

**Rules (from AB-MATIC — apply here):**

- ✅ **A.1** One audit row per **business operation** — interceptor batches one row per changed entity per save; avoid duplicate manual + interceptor on same entity in one save
- ✅ **A.2** Compliance trail uses **`IAuditService`**, not `ILogger`
- ✅ **A.3** Never log passwords, tokens, Mollie secrets, or full payment payloads
- ✅ **A.4** `OldValues` / `NewValues` as JSON snapshots (field-level diff optional later)

---

## 4. Authentication events (Login / Logout)

| Event | Action | Route / trigger | Policy | Log? |
|-------|--------|-----------------|--------|------|
| Admin login success | `Login` | `/admin/login` | Admin/Manager | ✅ |
| Admin login failed | `LoginFailed` | `/admin/login` | — | ✅ |
| Store login success | `Login` | `/sign-in` | Customer | ✅ |
| Store login failed | `LoginFailed` | `/sign-in` | — | ✅ |
| Staff via store sign-in | `Login` | `/sign-in` → redirect `/admin` | Admin/Manager (no Customer) | ✅ |
| Store registration | `Create` | `/sign-up` → `CustomerRegistrationRepository` | `ApplicationUser` + domain `Customer`/`Project` via interceptor | ✅ |
| Admin logout | `Logout` | `Account/Logout` (admin top bar) | — | ✅ |
| Store sign out | `Logout` | `StoreHeader` sign out | — | ✅ |
| Password reset (admin) | `PasswordReset` | `/admin/users`, `IdentityPasswordService` | Admin | ✅ |
| Self profile update | `Update` | `/admin/profile`, `/my-account` | — | ✅ |
| Self password change | `PasswordReset` | `/admin/profile`, `/my-account` | — | ✅ |
| Circuit closed (session end) | `Logout` | `AuditCircuitHandler` | — | ✅ |

**NewValues examples:**

- Login success: `{ "email": "admin@webshop.com", "roles": ["Admin","Manager"] }`
- Login failed: `{ "email": "…", "reason": "InvalidPassword" }` — no password field
- Circuit logout: `{ "email": "…", "reason": "CircuitClosed" }` — skipped if manual logout within 20s (`IManualLogoutTracker`)

**Infrastructure:** ✅ `AuditCircuitHandler` registered in `Program.cs` · manual logout deduplication via `ManualLogoutTracker`

---

## 5. Report events (Report badge)

All admin `*List.razor` pages with **EXPORT** (CSV/PDF) log via `GridExportService` → `ReportExport`.

| reportKey (`EntityName` / `FileBaseName`) | Trigger | Format | Status |
|-------------------------------------------|---------|--------|--------|
| `products`, `customers`, `users`, `orders`, … | Matching admin list EXPORT | CSV / PDF | ✅ |
| `stock-movements`, `stock-overview`, … | Stock admin exports | CSV / PDF | ✅ |
| `audit-logs` | Audit log grid export | CSV / PDF | ✅ |

**Log shape (implemented):**

```json
{
  "reportKey": "products",
  "title": "Product list",
  "format": "csv",
  "rowCount": 42
}
```

- ✅ **R.1** `ReportExport` action + purple badge — wired in `GridExportService`
- ✅ **R.2** All current admin grid exports produce audit rows

---

## 6. CRUD inventory — admin entities

Each row: **entity** → **route** → **repository/use case** → log **Create / Update / Delete**.

Legend: ✅ logged · ⬜ not wired · n/a no operation · **auto** = `AuditSaveChangesInterceptor` on domain `SaveChangesAsync`

### 6.1 Webshop hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `WebshopStructure` | `/admin/webshop-structures` | `WebshopStructureRepository` | ✅ auto | ✅ auto | ✅ auto |
| `WebshopProductStructure` | `/admin/webshop-product-structures` | `WebshopProductStructureRepository` | ✅ auto | ✅ auto | ✅ auto |

### 6.2 Catalog hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `Product` | `/admin/products` | `ProductRepository` | ✅ auto | ✅ auto | ✅ auto (`IsInactive` → `Delete`) |
| `ProductPrice` | `/admin/product-prices` | `ProductPriceRepository` | ✅ auto | ✅ auto | ✅ auto |
| `ProductQuantityTier` | `/admin/product-tiers` | `ProductQuantityTierRepository` | ✅ auto | ✅ auto | ✅ auto |
| `ProductOption` | `/admin/product-options` | `ProductOptionRepository` | ✅ auto | ✅ auto | ✅ auto |
| `PriceListCategory` | `/admin/price-list-categories` | `PriceListCategoryRepository` | ✅ auto | ✅ auto | ✅ auto |
| `Manufacturer` | `/admin/manufacturers` | `ManufacturerRepository` | ✅ auto | ✅ auto | ✅ auto |
| `Supplier` | `/admin/suppliers` | `SupplierRepository` | ✅ auto | ✅ auto | ✅ auto |
| `AzureFile` (product image) | via `LocalProductMediaService` | `IProductMediaPort` | ✅ auto | ✅ auto | n/a |

### 6.3 Customers hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `Customer` | `/admin/customers` | `CustomerRepository` | ✅ auto | ✅ auto | ✅ auto |
| `CustomerDeliveryAddress` | `/admin/delivery-addresses` | `CustomerDeliveryAddressRepository` | ✅ auto | ✅ auto | ✅ auto |
| `CustomerProductDiscount` | `/admin/customer-discounts` | `CustomerProductDiscountRepository` | ✅ auto | ✅ auto | ✅ auto |
| `CustomerType` | `/admin/customer-types` | `CustomerTypeRepository` | ✅ auto | ✅ auto | ✅ auto |

### 6.4 Sales hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `Order` | `/admin/orders` | `OrderRepository` | ✅ auto | ✅ auto | n/a |
| `OrderStatus` | `/admin/order-statuses` | `OrderStatusRepository` | ✅ auto | ✅ auto | ✅ auto |
| `DeliveryType` | `/admin/delivery-types` | `DeliveryTypeRepository` | ✅ auto | ✅ auto | ✅ auto |

### 6.5 Stock hub

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `StockLocation` | `/admin/stock-locations` | `StockLocationRepository` | ✅ auto | ✅ auto | ✅ auto |
| `ProductStockLocation` | `/admin/product-stock` | `ProductStockLocationRepository` | ✅ auto | ✅ auto | ✅ auto |
| Stock overview / movements | `/admin/stock/*` | read-only today | n/a | n/a | n/a |

### 6.6 Settings & identity

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `ApplicationUser` (Identity) | `/admin/users` | `ApplicationUserAccountRepository` | ✅ manual | ✅ manual | n/a |
| `PaymentMethod` | `/admin/payment-methods` | `PaymentMethodRepository` | ✅ auto | ✅ auto | ✅ auto |
| `VatType` | `/admin/vat-types` | `VatTypeRepository` | ✅ auto | ✅ auto | ✅ auto |
| `StaffUser` (legacy HR) | `/admin/staff-users` | `StaffUserRepository` | ✅ auto | ✅ auto | ✅ auto |
| `UserGroup` (legacy) | `/admin/user-groups` | `UserGroupRepository` | ✅ auto | ✅ auto | ✅ auto |
| Profile (self) | `/admin/profile`, `/my-account` | `Profile.razor` / `StoreProfileRepository` | n/a | ✅ manual | n/a |

### 6.7 Storefront (non-admin CRUD)

| Entity | Route | Port / repository | Create | Update | Delete |
|--------|-------|-------------------|--------|--------|--------|
| `Order` + lines (checkout) | `/cart` → checkout | `StoreOrderRepository` | ✅ auto | n/a | n/a |
| `Customer` + Identity (sign-up) | `/sign-up` | `CustomerRegistrationRepository` | ✅ manual + auto | n/a | n/a |
| Store profile save | `/my-account` | `StoreProfileRepository` | n/a | ✅ manual + auto | n/a |
| Checkout started | `/cart` → checkout | `CheckoutUseCase` | ✅ `CheckoutStarted` | n/a | n/a |
| Mollie webhook (paid) | `/api/webhooks/mollie/payments` | `ProcessMollieWebhookUseCase` | ✅ `PaymentPaid` | n/a | n/a |

**CRUD summary:** Domain entities → **automatic** via interceptor (incl. `AzureFile` / product images) · Identity → **manual** · Store checkout → **`CheckoutStarted`** + order rows via interceptor · Mollie → **`PaymentPaid`**

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

- ✅ **7.1d** Indexes: `(Timestamp)`, `(IdentityUserId, Timestamp)`, `(EntityName, EntityId)`, `(Action, Success)` — migration `AuditLogs`

---

## 8. Phase 2 — Admin UI (implemented)

| Item | AB-MATIC | WebShop | Status |
|------|----------|---------|--------|
| Route | `/audit-logs` | `/admin/audit-logs` | ✅ |
| Access | Admin only | `AppPolicies.AdminOnly` | ✅ |
| Menu | Sidebar + clipboard icon | Sidebar + Settings hub card + export on grid | ✅ |
| Grid filters | Date, Action, Severity, User, Entity, Status | Date range, action, severity, user search, success/fail | ✅ |
| Details modal | Old/New JSON, IP, duration | Same pattern | ✅ |
| Legend modal | Badge legend | ✅ | ✅ |
| Dev seed | — | `seeds.sql` (12 rows) + `AuditLogSeed.cs` fallback if table empty | ✅ |
| Pagination | 50/page | 25/page (configurable filter) | ✅ |

---

## 9. Phase 3 — Extra badges

| Badge | Action | When | Status |
|-------|--------|------|--------|
| 💳 | `CheckoutStarted` | Order placed (pre/post pay) | ✅ `CheckoutUseCase` |
| 💳 | `PaymentPaid` | Mollie webhook marks advance payment paid | ✅ `ProcessMollieWebhookUseCase` |
| 🔐 | `PasswordReset` | Admin reset modal or self-service password change | ✅ |
| 📄 | `Create` / `Update` on `AzureFile` | Product primary image upload | ✅ auto (interceptor) |
| 📦 | `StockAdjust` | Dedicated stock write API | ⬜ Badge pending — **`IStockMovementService` + adjustment UI shipped**; CRUD interceptor still logs Create/Update on same rows |

---

## 10. Implementation roadmap (checklist)

### Foundation

- ✅ **F.1** `AuditLog` entity + EF migration applied locally
- ✅ **F.2** `IAuditService` + `AuditService` + DI registration
- ✅ **F.3** `AuditActionBadge.razor` + admin.css badge classes
- ✅ **F.4** Dev seed + manual smoke via `/admin/audit-logs`

### Phase 1 — Logging hooks

- ✅ **L.1** Login / LoginFailed — `/admin/login` + `/sign-in`
- ✅ **L.2** Logout — `Account/Logout` + store header sign out
- ✅ **L.2b** Circuit-handler logout — `AuditCircuitHandler` + `ManualLogoutTracker`
- ✅ **L.3** Domain CRUD — all entities on `WebShopABMATICDbContext` via `AuditSaveChangesInterceptor`
- ✅ **L.4** Identity user CRUD — `ApplicationUserAccountRepository`, registration, password reset
- ✅ **L.5** Store checkout `Order` Create (interceptor on domain save)
- ✅ **L.6** `ReportExport` on all admin grid exports (`GridExportService`)

- ✅ **L.7** `CheckoutStarted` + `PaymentPaid` (Phase 3)
- ✅ **L.8** Self-profile + `PasswordReset` on `/admin/profile` and `/my-account`

### Phase 2 — Read UI

- ✅ **U.1** `IAuditLogAdminPort` + `AuditLogList.razor`
- ✅ **U.2** Filters + legend modal
- ✅ **U.3** Details modal
- ✅ **U.4** Settings hub card **Audit log** + sidebar entry

### Documentation sync

- ✅ **D.1** [AUTH_IDENTITY_ROADMAP_open.md](./AUTH_IDENTITY_ROADMAP_open.md) Auth-7
- ✅ **D.2** [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) §3.5 Audit logging

---

## 11. Quick progress

```
Foundation   [██████████] 4/4
Badges (B)   [██████████] 4/4
Auth (L)     [██████████] 8/8
CRUD         [██████████] auto + identity + profile
Reports (R)  [██████████] 2/2
Admin UI (U) [██████████] 4/4
Phase 3      [█████████░] 4/5  (StockAdjust deferred)
```

### Key implementation files

| Area | Path |
|------|------|
| Entity + migration | `Infrastructure/Identity/AuditLog.cs`, migration `AuditLogs` |
| Write service | `Infrastructure/Audit/AuditService.cs` |
| Domain CRUD hook | `Infrastructure/Audit/AuditSaveChangesInterceptor.cs` |
| Circuit logout | `Infrastructure/Audit/AuditCircuitHandler.cs`, `ManualLogoutTracker.cs` |
| Checkout / Mollie | `CheckoutUseCase.cs`, `ProcessMollieWebhookUseCase.cs` |
| Self profile | `Profile.razor`, `StoreProfileRepository.cs` |
| Export hook | `Web/Services/GridExportService.cs` |
| Admin UI | `Web/Components/Pages/Admin/AuditLogList.razor` |
| Badge component | `Web/Components/Admin/AuditActionBadge.razor` |
| Dev seed | `Infrastructure/Seeding/AuditLogSeed.cs` |
| Settings hub | `Infrastructure/Admin/AdminHubRegistry.cs` |

---

## 12. What we are **not** copying from AB-MATIC (yet)

- Workflow badges (`WKFLOWSAVE`, …)
- Payment schedule / rent index badges
- File manager / Word viewer document badges
- 2FA enable/disable
- Contract status transitions
- 90+ action catalogue — WebShop MVP uses **10 action codes** (Phase 1 + Phase 3 core)

---

## Documentation

- 🔐 [Auth identity roadmap — Auth-7](./AUTH_IDENTITY_ROADMAP_open.md)
- 🖥️ [Admin — user context §2.7](./SPEC_ADMIN.md)
- 🏗️ [Infrastructure patterns](./PATTERNS_CODE_AND_INFRASTRUCTURE.md)
- 🏠 [Main README](../README.md)

---

**© 2026 AdminSense. All rights reserved.**
