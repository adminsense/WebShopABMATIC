# 🧱 Infrastructure (WebShopABMATIC vNext)

![Status](https://img.shields.io/badge/Status-Store%20%2B%20Admin%20live-28a745?style=flat-square) ![Runtime](https://img.shields.io/badge/Runtime-.NET%208-512BD4?style=flat-square&logo=dotnet&logoColor=white) ![UI](https://img.shields.io/badge/UI-Blazor%20Server-512BD4?style=flat-square&logo=blazor&logoColor=white) ![DB](https://img.shields.io/badge/DB-Azure%20SQL-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white) ![Host](https://img.shields.io/badge/Host-Azure%20App%20Service-0078D4?style=flat-square&logo=microsoftazure&logoColor=white)

This document defines the **infrastructure and platform conventions** for WebShopABMATIC vNext (Storefront + Admin).

**Legend:** ✅ = implemented in the solution · ⏳ = planned / not started yet

### Azure platform map (ABMATIC — verified)

| Resource | Name / endpoint | Role |
|----------|-----------------|------|
| **App Service** | `abmaticwebshop` → `https://abmaticwebshop.azurewebsites.net` | Hosts Blazor Server (store + admin) |
| **Visual reference** | `https://adminsenceweb.azurewebsites.net/` | Earlier admin shell mock (not the vNext deploy target) |
| **Azure SQL** | `abmatic.database.windows.net` / database `abmatic_test` | Legacy ERP schema + vNext EF tables |
| **Blob Storage** | Account `abmatic` · container `files` · `https://abmatic.blob.core.windows.net` | Product image bytes — [AZUREBLOB.md](AZUREBLOB.md) |
| **File Share** | `abmaticprojecten` / share `projecten` | ERP project files — **not** webshop catalog |

> **Secrets:** connection strings, storage keys, and Mollie API keys live in **Azure App Service → Configuration → Application settings** (or local User Secrets). Never commit `Web/appsettings.json` — it is listed in `.gitignore`.

---

## ✅ 1. Architecture (target shape)

### 1.1 UI composition

- **Storefront**: public pages + customer-authenticated pages (cart, orders, profile)
  - ✅ Blazor storefront at `/` (catalog, product, cart, sign-in, orders)
- **Admin**: staff-authenticated pages (role-protected)
  - ✅ Blazor Server admin area live in `Web/`

### 1.2 Layers (hexagonal architecture)

| Ring | Project | Responsibility |
|------|---------|----------------|
| **Driving adapter (UI)** | `Web/` | Blazor pages — inject **inbound ports** only, no EF |
| **Application** | `Application/` | **Use cases**, DTOs, **inbound + outbound port** interfaces |
| **Domain** | `Domain/` | Entities, value objects, domain rules (pure .NET, no EF) |
| **Driven adapters** | `Infrastructure/` | EF repositories, Identity, media, seed — implement **outbound ports** |
| **Persistence models** | `Model/` + `Persistence/` | EF entity classes + `WebShopABMATICDbContext` (legacy schema) |

**Dependency rule (inward only):**

```
Web  →  Application  →  Domain
Infrastructure  →  Application  →  Domain
Infrastructure  →  Persistence / Model
```

**Data flow:**

```
Blazor page
  → IProductAdminPort          (inbound / driving port)
  → ProductAdminUseCase        (application use case)
  → IProductRepository         (outbound / driven port)
  → ProductRepository + Mapper (infrastructure adapter)
  → WebShopABMATICDbContext    (persistence)
```

**Folder layout:**

```
Application/
  Ports/
    Inbound/          ← IProductAdminPort, ICustomerAdminPort, …
    Outbound/         ← IProductRepository, IManufacturerRepository, …
  UseCases/Admin/     ← *AdminUseCase (implements inbound ports)
  Admin/              ← DTOs + filters (UI ↔ application contract)
  DependencyInjection.cs

Domain/
  Catalog/Products/   ← Product aggregate (domain rules)
  MasterData/         ← Manufacturer, …

Infrastructure/
  Persistence/
    Repositories/     ← EF adapters (outbound port implementations)
    Mappers/          ← Domain ↔ persistence entity mapping
  Auth/               ← Legacy sign-in, cookie auth, current user context
  Media/              ← IProductMediaPort (Azure Blob + local fallback)
  Payments/           ← Mollie adapters (mock + real)
  Notifications/      ← Low-stock email mock / queue
  Admin/              ← AdminHubRegistry (static config adapter)
  DependencyInjection.cs

Model/Entities/       ← EF persistence models (not domain)
Persistence/          ← DbContext
Web/                  ← Blazor driving adapter
```

- **UI (Blazor Server)**: pages/components only, no EF queries  
  ✅ Admin pages call inbound ports only

- **Application**: use cases + DTOs + port interfaces  
  ✅ `Application/UseCases/Admin/*`, `Application/Ports/Inbound|Outbound`

- **Domain**: pure business entities  
  ✅ `Domain/Catalog/Products/Product.cs` (expand per aggregate)

- **Infrastructure**: driven adapters (EF, files, Identity)  
  ✅ `Infrastructure/Persistence/Repositories/*`

- **Persistence models**: legacy EF entities under `Model/Entities/`  
  ✅ Mapped to/from domain in infrastructure mappers

### 1.3 Data flow (mock-first → real)

- **Static mocks** (`docs/`) define screen requirements and reference DTO fields  
  ✅ `docs/mock-admin.html`, `docs/mock-loja.html`, [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md)

- **DTOs** become the contract between UI and Application services  
  ✅ `ProductDto`, `CustomerDto`, `OrderSummaryDto`, `AdminDashboardDto`, etc.

- **Ports (inbound)** provide a stable API the UI calls  
  ✅ `IAdminDashboardPort`, `IProductAdminPort`, `ICustomerAdminPort`, `IOrderAdminPort`, `IAdminHubPort`, …

- **Use cases** implement inbound ports; **repositories** implement outbound ports  
  ✅ `Application/UseCases/Admin/*AdminUseCase` → `Application/Ports/Outbound/*Repository` → `Infrastructure/Persistence/Repositories/*Repository`

- **DI registration**  
  ✅ `Program.cs`: `AddWebShopApplication()` (use cases) + `AddWebShopInfrastructure()` (EF repos, Identity, media)

---

## 🗂️ 2. Repository conventions (folders)

Suggested organization (adapt to current solution layout):

Repository root (`WebShopABMATIC/` — git repo):

```
WebShopABMATIC/                 ← repo root (solution parent)
├── WebShopABMATIC.sln
├── Domain/                       ← domain entities (hexagonal core)
├── Application/                  ← use cases, DTOs, inbound/outbound ports
├── Infrastructure/               ← driven adapters (EF repos, Identity, media)
├── Web/                          ← driving adapter (Blazor UI)
├── Model/                        ← EF persistence models (legacy entities)
├── Persistence/                  ← DbContext + ModelBuilder
├── docs/                         ← HTML mocks
├── readme/                       ← documentation
└── Sql/                      ← SQL + codegen
```

- `docs/` — static prototypes (HTML mocks)  
  ✅
- `readme/` — project documentation  
  ✅
- Solution projects (flat at repo root):
  - `Web/` — Blazor UI (driving adapter)  
    ✅
  - `Application/` — use cases, DTOs, port interfaces  
    ✅
  - `Domain/` — pure domain entities  
    ✅
  - `Infrastructure/` — outbound adapters, Identity, seed  
    ✅
  - `Model/` — EF persistence models  
    ✅
  - `Persistence/` — `WebShopABMATICDbContext`, model builder  
    ✅

---

## 🔐 3. Identity, roles and access control

### 3.1 Authentication

- **Legacy ABMATIC cookie auth** (current runtime) — validates against `Settings.StaffUsers` (admin) and `Customers.Customers` (store).  
  ✅ `LegacySignInService`, `LegacyCookieAuthentication`, HTTP login endpoints (`/account/admin-login`, `/account/store-login`)

- **ASP.NET Identity** — migrations and `IdentitySeed` remain in repo (`ApplicationDbContext`, `AspNetUsers`); login UI routes exist but **runtime auth uses legacy cookies** aligned with the ERP password model.  
  See [AUTH_IDENTITY_ROADMAP_open.md](./AUTH_IDENTITY_ROADMAP_open.md) for the Identity ↔ domain bridge history.

- **Blazor circuits:** `LegacyAuthenticationStateProvider` bridges cookie auth into interactive Server components on Azure.

### 3.2 Roles (minimum)

- `Admin`  
  ✅ `Application/Auth/AppRoles.cs`
- `Manager`  
  ✅
- `Customer`  
  ✅ (seed user for future storefront)

### 3.3 Authorization rules (baseline)

- **Storefront**
  - Public: catalog (`/`), product detail (`/product/{id}`)  
    ✅ Blazor store live — 12 products on home page, categories from `ProductStructuur`
  - Customer-only: cart, checkout, orders, `/my-account`  
    ✅ `/sign-in`, `/cart`, `/orders`, `/my-account`

- **Admin**
  - Role required: `Admin` or `Manager` (from legacy `StaffUsers` flags)  
    ✅ `[Authorize(Policy = AppPolicies.AdminOrManager)]` on all `/admin/*` pages
  - User & role management: `Admin` only  
    ✅ Policy `AppPolicies.AdminOnly` on sensitive screens

### 3.4 Implementation notes

- Use `[Authorize]` on pages/components in admin area.  
  ✅ All admin routes protected

- Prefer **policy names** for readability:
  - `AppPolicies.AdminOnly`  
    ✅
  - `AppPolicies.AdminOrManager`  
    ✅ Used on admin hub, dashboard, lists, forms
  - `AppPolicies.CustomerOnly`  
    ✅ Registered; used on `/my-account`

- Login redirect: Admin/Manager → `/admin` after sign-in  
  ✅ `/admin/login` (HTTP POST), `/sign-in` (store customers)

- **Dev / demo staff:** use `Settings.StaffUsers` rows from `seeds.sql` (legacy login + password fields).  
  **Dev store customer:** `Customers` row with matching `WebshopLogin` + password from seed.

### 3.5 Audit logging (Auth-7)

- **`AuditLogs`** table on `ApplicationDbContext` — migration `AuditLogs`  
  ✅ `Infrastructure/Identity/AuditLog.cs`

- **`IAuditService`** — single write path for compliance events  
  ✅ `Infrastructure/Audit/AuditService.cs`

- **Domain CRUD** — `AuditSaveChangesInterceptor` on `WebShopABMATICDbContext` (Create / Update / Delete JSON snapshots)  
  ✅ Includes `AzureFile` product images via `LocalProductMediaService`

- **Auth events** — Login, LoginFailed, Logout (manual + circuit close via `AuditCircuitHandler`)  
  ✅ Admin login, store sign-in, `Account/Logout`, store header sign-out

- **Exports** — `ReportExport` on all admin grid CSV/PDF via `GridExportService`  
  ✅

- **Store checkout** — `CheckoutStarted` (order placed), `PaymentPaid` (Mollie webhook)  
  ✅ `CheckoutUseCase`, `ProcessMollieWebhookUseCase`

- **Self-profile** — `/admin/profile`, `/my-account` profile + `PasswordReset` action  
  ✅

- **Admin UI** — `/admin/audit-logs` (filters, legend, detail modal, export); Settings hub card  
  ✅ See [AUDITS_open.md](./AUDITS_open.md)

---

## 💾 4. Database & migrations

### 4.1 Database

- **SQL Server** as the primary persistence.  
  ✅ Azure SQL `abmatic.database.windows.net` (database `abmatic_test`) in all environments

- EF Core migrations committed to repo.  
  ✅ Identity: `Infrastructure/Identity/Migrations/InitialIdentity`

- Domain DB: `WebShopABMATICDbContext` → connection `connWebShopABMATIC`  
  ✅ Same server/database as Identity; legacy schema from `Sql/WebShopABMATIC-create-local.sql`

### 4.2 Migrations workflow

- Local dev:
  ```bash
  dotnet ef database update --project Infrastructure/WebShopABMATIC.Infrastructure.csproj --startup-project Web/WebShopABMATIC.Web.csproj --context ApplicationDbContext
  ```
  ✅ Applied manually via `Sql/apply-pending-schema.sql` — **not** on app startup

- For CI/Prod: run migrations as part of release  
  ⏳ Pipeline not configured yet

### 4.3 Seeding (required for mock-first)

**Login credentials** live in SQL (`seeds.sql`), not AspNetUsers — see [§3.1](#31-authentication).

**Demo logins** (after `seeds.sql` on `abmatic_test`):

| Login | Password | Access |
|-------|----------|--------|
| `admin@webshop.com` | `demo` | Admin + Manager (`StaffUsers`) |
| `manager@webshop.com` | `demo` | Manager |
| `customer@webshop.com` | `demo` | Store customer (`Customers`, CustomerId 4) |

> On Azure with **real ERP data**, use existing `StaffUsers` / `Customers` credentials from the database.

**Domain / catalog seed (implemented)**

- Products, prices, stock, orders, CRM, alerts, audit demo rows  
  ✅ `Sql/seeds.sql` — full inventory in [SUNDAY.md](SUNDAY.md) · [DATA_SUMMARY.md](DATA_SUMMARY.md)
- Product images metadata (`AzureFileFolders` + `AzureFiles` for all `ShowOnWebshop` SKUs)  
  ✅ Blob bytes in Azure container `files` when `AzureStorage:ConnectionString` is set — [AZUREBLOB.md](AZUREBLOB.md)
- Customer product discounts  
  ✅ 3 rows in `seeds.sql`

Seed strategy:

- **SQL demo:** `sqlcmd … -i Sql\seeds.sql` against `abmatic_test`  
  ✅ Idempotent `Sql/seeds.sql`
- **Schema:** `Sql/apply-pending-schema.sql`  
  ✅ See [DATA_DEMO_SEED.md](DATA_DEMO_SEED.md)

---

## 🧰 5. Configuration & environments

### 5.1 Environments

Single **Azure SQL** database (`abmatic_test`) is used for local dev and deployed App Service — no separate dev/staging/prod database split in config.

| Environment | Config source | Notes |
|-------------|---------------|-------|
| **Local** | `Web/appsettings.json` (gitignored) + User Secrets | Port **5090** (`launchSettings.json`) |
| **Azure App Service** | Portal → **Configuration** → Application settings | Detected via `WEBSITE_SITE_NAME` env var |

`appsettings.Production.json` may be tracked for non-secret defaults; secrets always via portal or env vars.

### 5.2 Configuration keys (application settings)

| Section / key | Purpose | Local | App Service |
|---------------|---------|-------|-------------|
| `ConnectionStrings:connWebShopABMATIC` | Domain EF (`WebShopABMATICDbContext`) | ✅ | ✅ Required |
| `ConnectionStrings:DefaultConnection` | Identity / audit EF (if enabled) | Optional | Optional |
| `AzureStorage:ConnectionString` | Blob account `abmatic` | User Secrets | ✅ Configuration |
| `AzureStorage:ContainerName` | Product images container | `files` | `files` |
| `AzureStorage:UseSasUrls` | Private container SAS URLs | `true` | `true` |
| `AzureStorage:SasValidityHours` | SAS cache TTL | `12` | `12` |
| `Mollie:ApiKey` | Real Mollie payments | User Secrets | ✅ Prod go-live |
| `Mollie:UseMock` | Mock checkout without API key | `true` (dev) | `false` (prod) |
| `Notifications:LowStock:UseMock` | In-app mock vs SMTP queue | `true` (dev) | `false` (prod) |

Example **Azure App Service** block (placeholders — never commit real values):

```json
{
  "ConnectionStrings": {
    "connWebShopABMATIC": "Server=tcp:abmatic.database.windows.net,1433;Database=abmatic_test;..."
  },
  "AzureStorage": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=abmatic;AccountKey=…",
    "ContainerName": "files",
    "UseSasUrls": true,
    "SasValidityHours": 12
  }
}
```

Portal path: **App Service `abmaticwebshop`** → **Settings** → **Environment variables** (or **Configuration** → **Application settings**).

### 5.3 Secrets (do not commit)

- DB connection string  
  ✅ `abmatic.database.windows.net` / `abmatic_test` — credentials in App Service or User Secrets only
- Azure Storage account key / connection string  
  ✅ Same — rotate if exposed in chat or email
- Mollie API key (test / live)  
  ⏳ Prod go-live — [MOLLIE_PAYMENTS_open.md](MOLLIE_PAYMENTS_open.md)

**Connection string keys**

| Key | Purpose |
|-----|---------|
| `connWebShopABMATIC` | Persistence / domain schema (`WebShopABMATICDbContext`) — **primary** |
| `DefaultConnection` | ASP.NET Identity (`ApplicationDbContext`) when Identity path is enabled |

---

## 🖥️ Admin implementation (completed — May 2026)

This section records everything delivered beyond the baseline items 1–5 above.

### Solution projects

| Project | Path |
|---------|------|
| WebShopABMATIC.Web | `Web/` |
| WebShopABMATIC.Application | `Application/` |
| WebShopABMATIC.Infrastructure | `Infrastructure/` |
| WebShopABMATIC.Domain | `Domain/` |
| WebShopABMATIC.Data | `Model/` |
| WebShopABMATIC.Data.Persistence | `Persistence/` |

✅ All added to `WebShopABMATIC.sln`

### Admin UI (AB-MATIC-style layout)

Matches `docs/mock-admin.html` and [PATTERNS_UI_QUICK_START.md](PATTERNS_UI_QUICK_START.md):

| Route | Screen | Status |
|-------|--------|--------|
| `/admin` | Dashboard — portfolio KPI cards | ✅ |
| `/admin/hub/{id}` | Hub — entity cards (Webshop, Catalog, …) | ✅ |
| `/admin/products` | Product — form card + list grid + image upload | ✅ |
| `/admin/customers`, `/admin/orders`, … | All hub entities (21 routes) | ✅ Form + grid per entity (`*List.razor`) |
| `/admin/profile` | Staff profile | ✅ |

Each entity page follows the **Product pattern**: back link → header → create/edit form → searchable list with edit/delete (Orders: edit only).

**Shell components:** `AdminLayout`, `AdminSidebar`, `AdminTopBar`, `wwwroot/css/admin.css`  
✅ Dark sidebar, top bar (Hello + Logout), footer date/version

### Application layer (hexagonal)

| Layer | Location | Admin examples |
|-------|----------|----------------|
| **Inbound ports** | `Application/Ports/Inbound/` | `IProductAdminPort`, `ICustomerAdminPort`, … |
| **Use cases** | `Application/UseCases/Admin/` | `ProductAdminUseCase`, `CustomerAdminUseCase`, … |
| **Outbound ports** | `Application/Ports/Outbound/` | `IProductRepository`, `ICustomerRepository`, `IProductMediaPort`, … |
| **DTOs** | `Application/Admin/` | `ProductDto`, `CustomerEditDto`, … |
| **Domain** | `Domain/` | `Product` aggregate (expand per entity) |
| **Repositories** | `Infrastructure/Persistence/Repositories/` | `ProductRepository`, `CustomerRepository`, … |
| **Hub config** | `Infrastructure/Admin/AdminHubRegistry.cs` | Static card routes (driving adapter config) |

### Run the app (local)

```bash
cd Web
dotnet run
```

1. Open **http://localhost:5090** (`launchSettings.json` / IIS Express)
2. **Admin:** `/admin/login` — `StaffUsers.Login` / `StaffUsers.Password` (e.g. `admin@webshop.com` / `demo` after demo seed)
3. **Store:** `/sign-in` — `WebshopLogin` / password (e.g. `customer@webshop.com` / `demo` after demo seed)

**IIS / Visual Studio:** `Web/web.config` points to `.\bin\Debug\net8.0\WebShopABMATIC.Web.dll`. Rebuild after code changes; do not commit stray DLLs from `Web/` root (see `.gitignore`).

> **Data:** Lists read from `abmatic_test`. Run `Sql\seeds.sql` for demo rows — [DATA_DEMO_SEED.md](DATA_DEMO_SEED.md).

### HTML prototypes and Blazor storefront

✅ `docs/mock-loja.html` — light-blue storefront reference  
✅ **Blazor store:** `/` catalog (12 products), `/product/{id}`, `/cart`, `/orders`, `/sign-in`, `/my-account` — `IStoreCatalogPort` → `StoreCatalogService`  
✅ Categories from ERP `ProductStructuur` on client DB; demo seed uses `WebshopProductStructures`  
✅ Product images via `IProductMediaPort` → Azure Blob SAS or local fallback  
✅ Admin entry from store when staff is signed in  
✅ [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md) — screen reference with `readme/images/*_screen.png`

---

## 🖼️ 6. Media (product images)

**Target model:** legacy `[Bestanden].[AzureFile]` linked to `Products.Product` via `ProductId` (`IsPrimaryImage`, `PublishToWeb`, `BlobRef`). Production uses Azure Blob container `files` with SAS URLs; local dev falls back to `wwwroot/media/products/`. Full spec: [AZUREBLOB.md](AZUREBLOB.md).

### 6.1 Storefront (implemented)

- ✅ Placeholder assets: `wwwroot/images/product1.png` … `product6.png` (fallback when no `AzureFile` row)
- ✅ `StoreCatalogService` reads primary image via `IProductMediaPort` → `[Bestanden].[AzureFile]`
- ✅ Production: SAS URLs from container `files` on account `abmatic`

### 6.2 Storage phases

| Phase | Storage | DB |
|-------|---------|-----|
| **1** | Local filesystem (dev fallback) | `AzureFiles.BlobRef` — ✅ |
| **2** | Real Azure Blob Storage (`files`) | Same table contract — ✅ |

---

## 📈 7. Observability (logging + health)

### 7.1 Logging

- Structured logging (built-in)  
  ⏳ Default ASP.NET Core logging only

### 7.2 Health checks

- `/health` endpoint including DB connectivity  
  ⏳

### 7.3 Error handling

- Blazor `ErrorBoundary`  
  ⏳
- Loading/error/empty states on admin screens  
  ✅ Basic loading states on Product/Customer/Order lists and dashboard

---

## 🚦 8. CI/CD (baseline)

- restore + build  
  ⏳
- run tests  
  ⏳
- database migrations compile check  
  ⏳

---

## ☁️ 9. Hosting (Azure)

### 9.1 App Service — `abmaticwebshop`

| Setting | Value |
|---------|--------|
| **App name** | `abmaticwebshop` |
| **Default URL** | `https://abmaticwebshop.azurewebsites.net` |
| **Stack** | .NET 8, Windows, Blazor Server (in-process) |
| **Detection** | `WEBSITE_SITE_NAME` → enables Azure-specific `Program.cs` paths |

**Required portal settings** (Blazor Server on Azure):

| Setting | Value | Why |
|---------|--------|-----|
| **Web sockets** | **On** | SignalR / `_blazor` hub — without it, Long Polling only (slow, fragile) |
| **Always On** | **On** | Keeps circuits alive (if plan allows) |
| **HTTPS Only** | **On** | Secure cookies + Mollie redirects |

**Application settings** (minimum): `connWebShopABMATIC`, `AzureStorage:ConnectionString`, `AzureStorage:ContainerName` = `files`. Add `Mollie:ApiKey` for prod payments.

**Runtime behaviour in `Program.cs`:**

- `ForwardedHeaders` (X-Forwarded-For / Proto) for Azure reverse proxy
- **Data Protection** keys persisted to `%HOME%\data\keys` (survives restarts)
- Cookie auth: `.WebShopABMATIC.Auth`, login `/sign-in`, admin redirect via `/admin/login`

**Deploy:**

```bash
dotnet publish Web/WebShopABMATIC.Web.csproj -c Release -o ./publish
```

Publish output to App Service (Visual Studio Publish, ZIP deploy, or CI). Ensure `web.config` `aspNetCore` points to the published DLL (not a stale Debug path under `Web/`).

**Stdout logs:** `Web/web.config` enables `stdoutLogEnabled="true"` → `.\logs\stdout` on the App Service file share (Kudu).

### 9.2 Database hosting

- **Azure SQL Database**  
  ✅ Server `abmatic.database.windows.net`, database `abmatic_test` — legacy Dutch schema (139 business tables) + vNext EF additions on the same catalog

### 9.3 Blob storage

- **Storage account** `abmatic`, container **`files`** (private → SAS)  
  ✅ See [AZUREBLOB.md](AZUREBLOB.md)

### 9.4 Deployment model

- **One app:** Storefront + Admin in single `WebShopABMATIC.Web` host  
  ✅ Both areas live; shared connection string and blob config

### 9.5 Reference site (not vNext deploy)

- `https://adminsenceweb.azurewebsites.net/` — visual reference for AB-MATIC admin shell ([MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md)). vNext deploy target is **`abmaticwebshop`**.

---

## 🧪 10. “Get data from live site” (practical extraction checklist)

### 10.1 What we need to extract

- Routes/screens  
  ⏳ Partially covered by HTML mocks + admin hub registry
- Entities/DTOs  
  ✅ Aligned with `Model/Entities/` and admin DTOs
- API endpoints  
  ⏳ Blazor Server uses ports, not REST yet
- Auth flow  
  ✅ Legacy cookie auth (staff + customers); HTTP POST login endpoints for Azure compatibility

### 10.2 How to extract (recommended)

- Browser DevTools → Network → HAR  
  ✅ Reference UI: [adminsenceweb.azurewebsites.net](https://adminsenceweb.azurewebsites.net/) · vNext: `abmaticwebshop.azurewebsites.net`

### 10.3 Output artifacts (in-repo)

- `docs/mock-data/*.json`  
  ⏳
- endpoint → DTO → screen mapping doc  
  ✅ [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md)

---

## 🔧 Troubleshooting (Visual Studio / local run)

### Antiforgery error on Sign out

| Symptom | Cause | Fix |
|---------|-------|-----|
| `AntiforgeryValidationException` on logout | POST form with `AntiforgeryToken` from an **interactive** Blazor component | **Fixed:** Sign out calls `SignInManager.SignOutAsync()` in code, then `forceLoad` to `/` (store and admin top bar) |

### Admin Panel does nothing after login

| Symptom | Cause | Fix |
|---------|-------|-----|
| Click **Admin Panel** after staff login | Link always pointed to `/admin/login` | **Fixed:** When Admin/Manager is signed in, link goes to `/admin`; `/admin/login` auto-redirects to `/admin` if already authenticated |

### Stuck on “Redirecting…” at `/`

| Symptom | Cause | Fix |
|---------|-------|-----|
| Browser shows only **Redirecting…** at `localhost:44350/` | Client-side `NavigateTo` in `Home.razor` did not complete under static SSR | **Fixed:** `Program.cs` `MapGet("/")` returns HTTP 302 to `/Account/Login` or `/admin`; `Home.razor` removed |
| IIS Express opens `/` | Launch profile default URL | Set `launchUrl` to `Account/Login` in `launchSettings.json` |

### NavigationException in debugger

| Symptom | Cause | Fix |
|---------|-------|-----|
| Debugger breaks on `NavigationException` | Blazor throws this **by design** on some `NavigateTo` calls | Press **Continue (F5)** or disable **User-Unhandled** for that exception type |

### MSB3027 / could not copy `*.dll`

| Symptom | Cause | Fix |
|---------|-------|-----|
| Build fails: file locked by `iisexpress.exe` or `devenv.exe` | App still running under IIS Express while rebuilding | **Stop Debugging** (Shift+F5) in Visual Studio, then **Build → Rebuild**. If needed: Task Manager → end **IIS Express** for this site |

### Blazor blank `/admin` or WebSocket warnings on Azure

| Symptom | Cause | Fix |
|---------|-------|-----|
| Console: `Failed to connect via WebSockets, using Long Polling` | WebSockets **Off** on App Service | Portal → `abmaticwebshop` → **Web sockets** → **On** → Restart |
| `/admin` blank after login | Circuit auth / prerender | Ensure latest publish; `LegacyAuthenticationStateProvider` + HTTP login endpoints; check stdout logs |
| 404 storm for static assets | Stale DLLs or wrong `web.config` path | Rebuild Release; fix `web.config` to published DLL; clear `Web/*.dll` from repo root |

### Wrong port locally

| Symptom | Cause | Fix |
|---------|-------|-----|
| App on unexpected port | Old test profile | Use **5090** per `launchSettings.json` — not 5091 or 44350 |

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview
- 🗺️ [Implementation roadmap](IMPLEMENTATION_ROADMAP_open.md)
- 🖼️ [Azure Blob media](AZUREBLOB.md)
- 🌱 [Demo seed](SUNDAY.md) · [DATA_DEMO_SEED.md](DATA_DEMO_SEED.md)
- 💳 [Mollie go-live](MOLLIE_PAYMENTS_open.md)
- 🔐 [Auth roadmap](AUTH_IDENTITY_ROADMAP_open.md)

---

**© 2026 AdminSense. All rights reserved.**
