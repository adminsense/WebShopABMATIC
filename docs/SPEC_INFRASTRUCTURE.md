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
| **Blob Storage** | Account `abmatic` · container `files` · `https://abmatic.blob.core.windows.net` | Product image bytes — [DATA_AZUREBLOB.md](DATA_AZUREBLOB.md) |
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
| **Driven adapters** | `Infrastructure/` | EF repositories, Identity, media — implement **outbound ports** |
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

- **Static mocks** (`docs/mocks/`) define screen requirements and reference DTO fields
  ✅ `docs/mocks/mock-admin.html`, `docs/mocks/mock-loja.html`, [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md)

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
├── WebShopABMATIC/               ← host + Application, Domain, Infrastructure, …
├── WebShopABMATIC.Client/        ← Blazor UI (Components, wwwroot)
├── WebShopABMATIC.Tests/
├── docs/                       ← documentation (SPEC_*, SPEC_*_open, AMENDMENTS, mocks/, archive/)
│   └── docs/                     ← HTML mocks
```

- `docs/mocks/` — static HTML prototypes  
  ✅
- `docs/` — project documentation (see `docs/README.md`)  
  ✅

---

## 🔐 3. Identity, roles and access control

### 3.1 Authentication

- **Legacy ABMATIC cookie auth** (current runtime) — validates against `Settings.StaffUsers` (admin) and `Customers.Customers` (store).  
  ✅ `LegacySignInService`, `LegacyCookieAuthentication`, HTTP login endpoints (`/account/admin-login`, `/account/store-login`)  
  ✅ Cookie is authoritative — **no** in-memory server browser-session gate (removed; it broke login after App Service recycle / multi-instance)

- **ASP.NET Identity** — `AspNet*` / Identity migrations may remain as unused artifacts in the repo; **runtime auth uses legacy cookies** only (see [SPEC_ADMIN.md](./SPEC_ADMIN.md) §2).

- **Blazor circuits:** `LegacyAuthenticationStateProvider` bridges cookie auth into Interactive Server (prerender **on**). Azure App Service must have **Web sockets = On** (same as working Immo apps — see troubleshooting below).

### 3.2 Roles (minimum)

- `Admin`  
  ✅ `Application/Auth/AppRoles.cs`
- `Manager`  
  ✅
- `Customer`  
  ✅ (future storefront Identity bridge)

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

- **Staff (admin):** `Settings.StaffUsers` — `Login` + `Password`.
- **Store customer:** `Customers.Customers` — `WebshopLogin` + `PasswordWebshop` / `SaltWebshop`.

Use credentials from the connected `abmatic_test` database. Inspect logins in admin (`/admin/staff-users`, `/admin/customers`) or SSMS.

### 3.5 Audit logging (Auth-7) — legacy database ✅

**No `AuditLogs` table and no ASP.NET Identity audit.** All events persist to **existing legacy tables** only:

| Legacy table | EF entity | Written by |
|--------------|-----------|------------|
| `[Projecten].[DossierLog]` | `OrderLog` | Checkout, Mollie paid/expired, cancel, stock-on-order |
| `[Logging].[ProjectActiviteit]` | `ProjectActivity` | Project activity codes (`LegacyProjectActivityCodes`) when order has `ProjectId` |
| `[Logging].[Error]` | `AppError` | Auth (login/logout/fail), admin CRUD (interceptor), exports, unhandled exceptions |

**Code:**

- **`IAuditService`** → `LegacyAuditService` (`Infrastructure/Audit/LegacyAuditService.cs`)
- **`LegacyAuditWriter`** — inserts into legacy tables (no schema changes)
- **`LegacyAuditSaveChangesInterceptor`** — EF `SaveChanges` → `[Logging].[Error]` for Create/Update/Delete
- **`LegacyExceptionLoggingMiddleware`** — HTTP 500 → `[Logging].[Error]`
- **Auth** — `POST /account/login`, `/account/logout`, store header sign-out → `[Logging].[Error]` (`ModuleName = Auth`)
- **Checkout / Mollie** — `CheckoutUseCase`, `ProcessMollieWebhookUseCase` → `DossierLog` + `ProjectActiviteit`
- **Exports** — `GridExportService` → `ReportExport` in `[Logging].[Error]`
- **Stock** — `StockMovementService` → `DossierLog` on order-linked stock ops; journal remains `[Products].[StockBeweging]`

**Admin UI:**

- `/admin/audit-logs` — reads `[Logging].[Error]` (Settings hub)
- `/admin/orders` — **Order log (DossierLog)** when editing an order

**Enums:** `LegacyAuditModules`, `LegacyProjectActivityCodes` — align `Actie` values with `SELECT DISTINCT Actie FROM Logging.ProjectActiviteit` on your DB if needed.

---

## 💾 4. Database (DB-first — no app migrations)

### 4.1 Source of truth

- **Azure SQL** `abmatic.database.windows.net` / database **`abmatic_test`**.
- Connection: `connWebShopABMATIC` → `WebShopABMATICDbContext`.
- Physical schema is the **Dutch ERP** (139 business tables). C# uses English names mapped via `WebShopABMATICModelBuilder`.

> [!IMPORTANT]
> **Database first (global — every feature).** The live ERP database is authoritative.  
> **Never invent** columns, tables, EF migrations, `Migrate()` / `EnsureCreated()`, or SQL/schema scripts in this repo to create or alter the ERP schema — store, admin, freight, payments, stock, docs, etc.  
> The app **adapts** to existing tables/columns (encode PSP/payment data in documented legacy fields when needed).  
> Identity `AspNet*` migrations may still exist as dead code in the repo — **not** used at runtime for login.

### 4.2 Data source

All catalog, customers, orders, and stock data come from **`abmatic_test`**. There is **no seed script** and **no migration workflow** for day-to-day development.

- Schema / mapping: [DATA_DUTCH_ENGLISH_MODEL.md](DATA_DUTCH_ENGLISH_MODEL.md), [DATA_SUMMARY.md](DATA_SUMMARY.md)
- Product images: `AzureFiles` + blob — [DATA_AZUREBLOB.md](DATA_AZUREBLOB.md)
- Authentication: legacy tables only — [§3.1](#31-authentication)

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
| `Mollie:ApiKey` | Real Mollie payments | User Secrets / Azure | ⬜ **After client keys** — keep mock until then |
| `Mollie:UseMock` | Mock checkout without API key | **`true` (required until client keys)** | `false` only after keys |
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
  ⏳ Prod go-live — [SPEC_MOLLIE_PAYMENTS_open.md](SPEC_MOLLIE_PAYMENTS_open.md)

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

Matches `docs/mocks/mock-admin.html` and [PATTERNS_UI_QUICK_START.md](PATTERNS_UI_QUICK_START.md):

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
2. **Admin:** `/admin/login` — `StaffUsers.Login` / `StaffUsers.Password`
3. **Store:** `/sign-in` — `WebshopLogin` / password

**IIS / Visual Studio:** output under `.\bin\Debug\net10.0\WebShopABMATIC.dll`. Rebuild after code changes.

> **Data:** Lists read from `abmatic_test` — see [DATA_SUMMARY.md](DATA_SUMMARY.md).

### HTML prototypes and Blazor storefront

✅ `docs/mocks/mock-loja.html` — light-blue storefront reference  
✅ **Blazor store:** `/` catalog (12 products), `/product/{id}`, `/cart`, `/orders`, `/sign-in`, `/my-account` — `IStoreCatalogPort` → `StoreCatalogService`  
✅ Categories from ERP `ProductStructuur` on `abmatic_test`  
✅ Product images via `IProductMediaPort` → Azure Blob SAS or local fallback  
✅ Admin entry from store when staff is signed in  
✅ [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md) — screen reference with `docs/images/*_screen.png`

---

## 🖼️ 6. Media (product images)

**Target model:** legacy `[Bestanden].[AzureFile]` linked to `Products.Product` via `ProductId` (`IsPrimaryImage`, `PublishToWeb`, `BlobRef`). Production uses Azure Blob container `files` with SAS URLs; local dev falls back to `wwwroot/media/products/`. Full spec: [DATA_AZUREBLOB.md](DATA_AZUREBLOB.md).

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

- **Legacy DB persistence** — `[Logging].[Error]`, `[Projecten].[DossierLog]`, `[Logging].[ProjectActiviteit]` via `LegacyAuditService` ✅
- **ASP.NET Core console logging** — default only ⏳

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
- build + smoke of store/admin pages (no DB migration step)
  ⏳

---

## ☁️ 9. Hosting (Azure)

### 9.1 App Service — `abmaticwebshop`

| Setting | Value |
|---------|--------|
| **App name** | `abmaticwebshop` |
| **Default URL** | `https://abmaticwebshop.azurewebsites.net` |
| **Stack** | .NET 10, Windows, Blazor Server (in-process) |
| **Detection** | `WEBSITE_SITE_NAME` → enables Azure-specific `Program.cs` paths |

**Required portal settings** (Blazor Server on Azure):

| Setting | Value | Why |
|---------|--------|-----|
| **Web sockets** | **On** | Blazor interactive / `_blazor` — without it, Long Polling only (slow, fragile) |
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
  ✅ See [DATA_AZUREBLOB.md](DATA_AZUREBLOB.md)

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

- `docs/mocks/mock-data/*.json`  
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

### Blazor blank UI / login or cart “does nothing” on Azure

| Symptom | Cause | Fix |
|---------|-------|-----|
| Console: `Failed to connect via WebSockets` / `_blazor/negotiate` **without** `WebSockets` | WebSockets **Off** on App Service | Portal → App Service → **Configuration → General settings → Web sockets → On** → Save (restart). Reference: working Immo `bcimmoapp` negotiate lists WebSockets |
| Login / cart clicks do nothing | Circuit never attaches (blank interactive shell) | Enable WebSockets; publish build with prerender on + cookie-only auth |
| Login succeeds then immediately logged out | Was: in-memory session store rejecting cookie after recycle | **Fixed in code** — cookie-only auth; redeploy latest |
| ARR stickiness | Multi-instance without affinity | Keep **ARR affinity** On (default cookie `ARRAffinity`) |
| 404 storm for static assets | Stale DLLs or wrong `web.config` path | Rebuild Release; fix `web.config` to published DLL; clear stale published DLLs |

### Wrong port locally

| Symptom | Cause | Fix |
|---------|-------|-----|
| App on unexpected port | Old test profile | Use **5090** per `launchSettings.json` — not 5091 or 44350 |

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview
- 🗺️ [Implementation roadmap](SPEC_IMPLEMENTATION_ROADMAP_open.md)
- 🖼️ [Azure Blob media](DATA_AZUREBLOB.md)
- 📊 [Database summary](DATA_SUMMARY.md)
- 💳 [Mollie go-live](SPEC_MOLLIE_PAYMENTS_open.md)
- 🔐 [Admin auth (current)](SPEC_ADMIN.md)

---

**© 2026 AdminSense. All rights reserved.**
