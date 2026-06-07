# 🧱 Infrastructure (WebShopABMATIC vNext)

![Status](https://img.shields.io/badge/Status-Admin%20implemented-28a745?style=flat-square) ![Runtime](https://img.shields.io/badge/Runtime-.NET%208-512BD4?style=flat-square&logo=dotnet&logoColor=white) ![UI](https://img.shields.io/badge/UI-Blazor%20Server-512BD4?style=flat-square&logo=blazor&logoColor=white) ![DB](https://img.shields.io/badge/DB-SQL%20Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)

This document defines the **infrastructure and platform conventions** for WebShopABMATIC vNext (Storefront + Admin).

**Legend:** ✅ = implemented in the solution · ⏳ = planned / not started yet

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
    AdminCrudDefaults.cs
  Media/              ← IProductMediaPort adapter
  Admin/              ← AdminHubRegistry (static config adapter)
  DependencyInjection.cs   ← registers outbound adapters only

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
└── scripts/                      ← SQL + codegen
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

- **ASP.NET Core Identity** (cookie auth) for Blazor Server.  
  ✅ `ApplicationUser`, `ApplicationDbContext`, login at `/Account/Login`

### 3.2 Roles (minimum)

- `Admin`  
  ✅ `Application/Auth/AppRoles.cs`
- `Manager`  
  ✅
- `Customer`  
  ✅ (seed user for future storefront)

### 3.3 Authorization rules (baseline)

- **Storefront**
  - Public: catalog, product detail  
    ⏳ HTML mock only
  - Customer-only: cart, checkout, my orders, profile  
    ⏳ HTML mock only

- **Admin**
  - Role required: `Admin` or `Manager`  
    ✅ `[Authorize(Policy = AppPolicies.AdminOrManager)]` on all `/admin/*` pages
  - User & role management: `Admin` only  
    ✅ Policy `AppPolicies.AdminOnly` registered (ready for staff-user screens)

### 3.4 Implementation notes

- Use `[Authorize]` on pages/components in admin area.  
  ✅ All admin routes protected

- Prefer **policy names** for readability:
  - `AppPolicies.AdminOnly`  
    ✅
  - `AppPolicies.AdminOrManager`  
    ✅ Used on admin hub, dashboard, lists, forms
  - `AppPolicies.CustomerOnly`  
    ✅ Registered; storefront not wired yet

- Login redirect: Admin/Manager → `/admin` after sign-in  
  ✅ `/admin/login`, `/sign-in` (staff without Customer role)

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
  ✅ LocalDB in Development; SQL Server in Staging/Production config

- EF Core migrations committed to repo.  
  ✅ Identity: `Infrastructure/Identity/Migrations/InitialIdentity`

- Domain DB: `WebShopABMATICDbContext` → connection `connWebShopABMATIC`  
  ✅ Same server/database as Identity in dev; legacy schema from `scripts/WebShopABMATIC-create-local.sql`

### 4.2 Migrations workflow

- Local dev:
  ```bash
  dotnet ef database update --project Infrastructure/WebShopABMATIC.Infrastructure.csproj --startup-project Web/WebShopABMATIC.Web.csproj --context ApplicationDbContext
  ```
  ✅ Applied manually via `scripts/apply-pending-schema.sql` or `scripts/apply-local-database.ps1` — **not** on app startup

- For CI/Prod: run migrations as part of release  
  ⏳ Pipeline not configured yet

### 4.3 Seeding (required for mock-first)

**Identity (Development — implemented)**

| User | Password | Roles |
|------|----------|-------|
| `admin@webshop.com` | `Admin@12345` | Admin, Manager |
| `manager@webshop.com` | `Manager@12345` | Manager |
| `customer@webshop.com` | `Customer@12345` | Customer |

✅ `Infrastructure/Seeding/IdentitySeed.cs` — run with `dotnet run -- --seed-identity` or `apply-local-database.ps1`

**Domain / catalog seed (planned)**

- Products with image URLs and stock values  
  ⏳ Store mock uses local images (`docs/images/product*.png`, Hard drive 1–6); domain seed pending
- Discounts with codes (e.g., `SUMMER10`, `FREESHIP`)  
  ⏳
- Customer profile linked to Identity  
  ⏳

Seed strategy:

- **Dev-only seed** via explicit command (`dotnet run -- --seed-identity`)  
  ✅ Identity roles + users
- Optional: explicit `Seed` command/endpoint for dev environments only  
  ⏳

---

## 🧰 5. Configuration & environments

### 5.1 Environments

- `Development`  
  ✅ `appsettings.Development.json` → LocalDB `WebShopABMATIC_Dev`
- `Staging`  
  ✅ `appsettings.Staging.json` (Azure SQL placeholders)
- `Production`  
  ✅ `appsettings.Production.json` (Azure SQL placeholders)

### 5.2 Configuration sources

- `appsettings.json` — base connection strings  
  ✅
- `appsettings.{Environment}.json`  
  ✅ Development, Staging, Production
- environment variables (preferred for secrets)  
  ✅ Supported by ASP.NET Core host
- Azure App Service configuration (for production secrets)  
  ⏳ Documented; not deployed yet

### 5.3 Secrets (do not commit)

- DB connection string  
  ✅ Placeholders only in repo; use User Secrets / env vars locally
- Identity secrets (if any custom)  
  ✅ Default Identity; no custom secrets in repo
- External providers (if added later)  
  ⏳

**Connection string keys**

| Key | Purpose |
|-----|---------|
| `DefaultConnection` | ASP.NET Identity (`ApplicationDbContext`) |
| `connWebShopABMATIC` | Persistence / domain schema (`WebShopABMATICDbContext`) |

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

### Run the admin app

```bash
cd Web
dotnet run
```

1. Open the URL shown in the console (HTTPS from `launchSettings.json`)
2. Sign in: `admin@webshop.com` / `Admin@12345`
3. You are redirected to `/admin`

> **Note:** Product/Customer/Order lists read from the legacy domain schema. Apply EF migrations, then run `scripts/seeds.sql` on `WebShopABMATIC` — see [DATA_DEMO_SEED.md](DATA_DEMO_SEED.md). Alternatively use `scripts/WebShopABMATIC-create-local.sql` or an existing ABMATIC database. Dashboard KPIs safely return `0` if tables are missing.

### HTML prototypes and Blazor storefront

✅ `docs/mock-loja.html` — light-blue storefront reference (Hard drive 1–6)  
✅ **Blazor store (partial):** `/` catalog, `/product/{id}`, `/cart`, `/orders`, `/store/sign-in` — UI calls `IStoreCatalogPort` → `StoreCatalogService`  
✅ Admin entry from store via **Admin Panel** when staff is signed in  
✅ [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md) — screen reference with `readme/images/*_screen.png`

---

## 🖼️ 6. Media (product images)

**Target model:** legacy `Files.AzureFiles` linked to `Products.Product` via `ProductId` (`IsPrimaryImage`, `PublishToWeb`, `BlobRef`). Phase 1 uses a **fictitious local blob** under `wwwroot/media/products/`. Full spec: [AZUREBLOB_open.md](AZUREBLOB_open.md).

### 6.1 Current storefront (prototype)

- ✅ Static assets: `wwwroot/images/product1.png` … `product6.png` via `IStoreCatalogPort` / `StoreCatalogService`
- ⏳ Replace with `AzureFiles` query when media port is implemented

### 6.2 Planned storage phases

| Phase | Storage | DB |
|-------|---------|-----|
| **1** | Local filesystem (fictitious Azure) | `AzureFiles.BlobRef` |
| **2** | Real Azure Blob Storage | Same table contract |

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

## ☁️ 9. Hosting (Azure target)

### 9.1 App hosting

- Azure App Service  
  ⏳

### 9.2 Database hosting

- Azure SQL Database  
  ⏳ Connection string placeholders in Staging/Production appsettings

### 9.3 Deployment model

- One app: Storefront + Admin  
  ✅ Single `WebShopABMATIC.Web` host; admin implemented, storefront pending

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
  ✅ Identity cookie auth for admin

### 10.2 How to extract (recommended)

- Browser DevTools → Network → HAR  
  ⏳ Live site `adminsenceweb.azurewebsites.net` currently returns Blazor error

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

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**
