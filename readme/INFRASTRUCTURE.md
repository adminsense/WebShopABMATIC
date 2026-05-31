# 🧱 Infrastructure (WebShopABMATIC vNext)

![Status](https://img.shields.io/badge/Status-Admin%20implemented-28a745?style=flat-square) ![Runtime](https://img.shields.io/badge/Runtime-.NET%208-512BD4?style=flat-square&logo=dotnet&logoColor=white) ![UI](https://img.shields.io/badge/UI-Blazor%20Server-512BD4?style=flat-square&logo=blazor&logoColor=white) ![DB](https://img.shields.io/badge/DB-SQL%20Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)

This document defines the **infrastructure and platform conventions** for WebShopABMATIC vNext (Storefront + Admin).

**Legend:** ✅ = implemented in the solution · ⏳ = planned / not started yet

---

## ✅ 1. Architecture (target shape)

### 1.1 UI composition

- **Storefront**: public pages + customer-authenticated pages (cart, orders, profile)
  - ⏳ HTML prototype only (`docs/mock-loja.html`) — Blazor storefront not started
- **Admin**: staff-authenticated pages (role-protected)
  - ✅ Blazor Server admin area live in `WebShopABMATIC/Web/`

### 1.2 Layers (recommended)

| Layer | Project | Status |
|-------|---------|--------|
| **UI (Blazor Server)** | `WebShopABMATIC/Web/` | ✅ Admin pages — no EF in Razor |
| **Application** | `WebShopABMATIC/Application/` | ✅ DTOs, policies, ports |
| **Infrastructure** | `WebShopABMATIC/Infrastructure/` | ✅ EF services, Identity, seed |
| **Domain** | `WebShopABMATIC/Model/` + `Persistence/` | ✅ Legacy entities + `WebShopABMATICDbContext` |

- **UI (Blazor Server)**: pages/components only, no EF queries  
  ✅ Admin pages call ports (`IProductAdminPort`, etc.) only

- **Application**: DTOs + mapping + use-cases/services + ports/interfaces  
  ✅ `Application/Auth`, `Application/Admin/*`, `Application/Ports/IAdminPorts.cs`

- **Infrastructure**: EF Core DbContext + repositories/adapters + external integrations  
  ✅ `Infrastructure/Admin/*`, `Infrastructure/Identity/*`, `Infrastructure/Seeding/*`

- **Domain**: entities + value objects  
  ✅ Existing EF entities under `Model/Entities/`

### 1.3 Data flow (mock-first → real)

- **Static mocks** (`docs/`) define screen requirements and reference DTO fields  
  ✅ `docs/mock-admin.html`, `docs/mock-loja.html`, [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md)

- **DTOs** become the contract between UI and Application services  
  ✅ `ProductDto`, `CustomerDto`, `OrderSummaryDto`, `AdminDashboardDto`, etc.

- **Ports/services** provide a stable API the UI uses  
  ✅ `IAdminDashboardPort`, `IProductAdminPort`, `ICustomerAdminPort`, `IOrderAdminPort`, `IAdminHubPort`

- Infrastructure implements ports using EF Core  
  ✅ `ProductAdminService`, `CustomerAdminService`, `OrderAdminService`, `AdminDashboardService`

---

## 🗂️ 2. Repository conventions (folders)

Suggested organization (adapt to current solution layout):

- `docs/` — static prototypes (HTML mocks)  
  ✅ Entry: `docs/mock-loja.html` · Admin reference: `docs/mock-admin.html`

- `docs/images/` — storefront product images (`product1.png` … `product6.png`)  
  ✅ Used by the store mock (Hard drive 1–6)

- `readme/` — project documentation (this file lives here)  
  ✅

- `WebShopABMATIC/` solution folder:
  - `Web/` — Blazor UI (admin implemented)  
    ✅ `Components/Pages/Admin/*`, `Components/Layout/AdminLayout.razor`, `Components/Admin/*`
  - `Application/` — DTOs, ports, policies  
    ✅
  - `Infrastructure/` — EF, Identity migrations, admin services, dev seed  
    ✅
  - `Model/` — domain entities  
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
  ✅ `Components/Account/Pages/Login.razor`

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
  dotnet ef database update --project WebShopABMATIC/Infrastructure/WebShopABMATIC.Infrastructure.csproj --startup-project WebShopABMATIC/Web/WebShopABMATIC.Web.csproj --context ApplicationDbContext
  ```
  ✅ Auto-applied on startup in Development via `IdentitySeedHostedService`

- For CI/Prod: run migrations as part of release  
  ⏳ Pipeline not configured yet

### 4.3 Seeding (required for mock-first)

**Identity (Development — implemented)**

| User | Password | Roles |
|------|----------|-------|
| `admin@webshop.com` | `Admin@12345` | Admin, Manager |
| `manager@webshop.com` | `Manager@12345` | Manager |
| `customer@webshop.com` | `Customer@12345` | Customer |

✅ `Infrastructure/Seeding/IdentitySeed.cs` + `IdentitySeedHostedService`

**Domain / catalog seed (planned)**

- Products with image URLs and stock values  
  ⏳ Store mock uses local images (`docs/images/product*.png`, Hard drive 1–6); domain seed pending
- Discounts with codes (e.g., `SUMMER10`, `FREESHIP`)  
  ⏳
- Customer profile linked to Identity  
  ⏳

Seed strategy:

- **Dev-only seed** on startup when DB is empty  
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
| `connWebShopABMATIC` | Domain entities (`WebShopABMATICDbContext`) |

---

## 🖥️ Admin implementation (completed — May 2026)

This section records everything delivered beyond the baseline items 1–5 above.

### Solution projects

| Project | Path |
|---------|------|
| WebShopABMATIC.Web | `WebShopABMATIC/Web/` |
| WebShopABMATIC.Application | `WebShopABMATIC/Application/` |
| WebShopABMATIC.Infrastructure | `WebShopABMATIC/Infrastructure/` |
| WebShopABMATIC.Data | `WebShopABMATIC/Model/` |
| WebShopABMATIC.Data.Persistence | `WebShopABMATIC/Persistence/` |

✅ All added to `WebShopABMATIC.sln`

### Admin UI (IMMO-style layout)

Matches `docs/mock-admin.html` and [UI_PATTERNS_QUICK_START.md](UI_PATTERNS_QUICK_START.md):

| Route | Screen | Status |
|-------|--------|--------|
| `/admin` | Dashboard — portfolio KPI cards | ✅ |
| `/admin/hub/{id}` | Hub — entity cards (Webshop, Catalog, …) | ✅ |
| `/admin/products` | Product list — filters, Apply/Clear, `table-dark` | ✅ |
| `/admin/products/new`, `/admin/products/{id}` | Product create/edit form | ✅ |
| `/admin/customers` | Customer list | ✅ |
| `/admin/orders` | Order list | ✅ |
| `/admin/profile` | Staff profile | ✅ |
| `/admin/webshop-structures`, … | Other hub entities | ✅ Placeholder pages (routes ready) |

**Shell components:** `AdminLayout`, `AdminSidebar`, `AdminTopBar`, `wwwroot/css/admin.css`  
✅ Dark sidebar, top bar (Hello + Logout), footer date/version

### Application ports & DTOs

✅ `IAdminDashboardPort`, `IAdminHubPort`, `IProductAdminPort`, `ICustomerAdminPort`, `IOrderAdminPort`  
✅ Hub registry mirrors mock-admin menus (Webshop, Catalog, Customers, Sales, Stock, Settings, Profile)

### Run the admin app

```bash
cd WebShopABMATIC/Web
dotnet run
```

1. Open the URL shown in the console (HTTPS from `launchSettings.json`)
2. Sign in: `admin@webshop.com` / `Admin@12345`
3. You are redirected to `/admin`

> **Note:** Product/Customer/Order lists read from the legacy domain schema. Run `scripts/WebShopABMATIC-create-local.sql` (or point `connWebShopABMATIC` at an existing ABMATIC database) for live data. Dashboard KPIs safely return `0` if tables are missing.

### HTML prototypes (store — not Blazor yet)

✅ `docs/mock-loja.html` — light-blue storefront, 6 products (Hard drive 1–6), images in `docs/images/`  
✅ Admin entry from store mock via **Admin Panel** (StaffUser.Admin demo)  
✅ [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md) — screen reference with `readme/images/*_screen.png`

---

## 🖼️ 6. Media (product images)

### 6.1 Store `ImageUrl` as external URL (fastest)

- Keep images on a CDN/Unsplash during development  
  ⏳ Replaced in store mock by local assets

- Works with current mocks  
  ✅ `docs/images/product1.png` … `product6.png` in `mock-loja.html`

### 6.2 First-class media storage (production-ready)

- Azure Blob Storage or local filesystem  
  ⏳
- Persist `ImageUrl` or `BlobKey` in DB  
  ⏳

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

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements
- 🖥️ [Mock Prototype Guide](MOCK_PROTOTYPE_GUIDE.md) — HTML mocks, menus, entities, validation
- 📋 [UI Patterns Quick Start](UI_PATTERNS_QUICK_START.md) — UI conventions and templates
- 📋 [Code Patterns](CODE_PATTERNS_AND_INFRASTRUCTURE.md) — Engineering patterns used in the solution

---

**© 2026 AdminSense. All rights reserved.**
