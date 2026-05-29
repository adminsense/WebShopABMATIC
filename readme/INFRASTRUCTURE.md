# 🧱 Infrastructure (WebShopABMATIC vNext)

![Status](https://img.shields.io/badge/Status-Draft%20(vNext)-0dcaf0?style=flat-square) ![Runtime](https://img.shields.io/badge/Runtime-.NET%2010-512BD4?style=flat-square&logo=dotnet&logoColor=white) ![UI](https://img.shields.io/badge/UI-Blazor%20Server-512BD4?style=flat-square&logo=blazor&logoColor=white) ![DB](https://img.shields.io/badge/DB-SQL%20Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)

This document defines the **infrastructure and platform conventions** for WebShopABMATIC vNext (Storefront + Admin).

---

## ✅ 1. Architecture (target shape)

### 1.1 UI composition
- **Storefront**: public pages + customer-authenticated pages (cart, orders, profile)
- **Admin**: staff-authenticated pages (role-protected)

### 1.2 Layers (recommended)
- **UI (Blazor Server)**: pages/components only, no EF queries
- **Application**: DTOs + mapping + use-cases/services + ports/interfaces
- **Infrastructure**: EF Core DbContext + repositories/adapters + external integrations
- **Domain (optional)**: entities + value objects (if we want richer invariants)

### 1.3 Data flow (mock-first → real)
- **Static mocks** (`docs/`) define screen requirements and reference DTO fields
- **DTOs** become the contract between UI and Application services
- **Ports/services** provide a stable API the UI uses
- Infrastructure implements ports using EF Core

---

## 🗂️ 2. Repository conventions (folders)

Suggested organization (adapt to current solution layout):

- `docs/`: static prototypes (HTML mocks)
- `readme/`: project documentation (this file lives here)
- `WebShopABMATIC/` (or solution folder):
  - `Pages/` (or `Components/`): Blazor UI
  - `Application/`: DTOs, ports, mapping, validators
  - `Infrastructure/`: EF Core, migrations, external services

---

## 🔐 3. Identity, roles and access control

### 3.1 Authentication
- **ASP.NET Core Identity** (cookie auth) for Blazor Server.

### 3.2 Roles (minimum)
- `Admin`
- `Manager`
- `Customer`

### 3.3 Authorization rules (baseline)
- **Storefront**
  - Public: catalog, product detail
  - Customer-only: cart, checkout, my orders, profile
- **Admin**
  - Role required: `Admin` or `Manager`
  - User & role management: `Admin` only

### 3.4 Implementation notes
- Use `[Authorize]` on pages/components in admin area.
- Prefer **policy names** for readability (even if they map to roles initially):
  - `Policies.AdminOnly`
  - `Policies.AdminOrManager`
  - `Policies.CustomerOnly`

---

## 💾 4. Database & migrations

### 4.1 Database
- **SQL Server** as the primary persistence.
- EF Core migrations committed to repo.

### 4.2 Migrations workflow
- Local dev runs:
  - `dotnet ef database update`
- For CI/Prod:
  - run migrations as part of release (either startup migration or pipeline step).

### 4.3 Seeding (required for mock-first)
We need seed data consistent with the mocks:
- Categories (Electronics, Fashion, Home, Sports)
- Products with image URLs and stock values
- Discounts with codes (e.g., `SUMMER10`, `FREESHIP`)
- Users:
  - Admin user(s)
  - Manager user(s)
  - Customer user(s) + `Customer` profile

Seed strategy options:
- **Dev-only seed** on startup when DB is empty
- Optional: explicit `Seed` command/endpoint for dev environments only

---

## 🧰 5. Configuration & environments

### 5.1 Environments
- `Development`
- `Staging`
- `Production`

### 5.2 Configuration sources
- `appsettings.json`
- `appsettings.{Environment}.json`
- environment variables (preferred for secrets)
- Azure App Service configuration (for production secrets)

### 5.3 Secrets (do not commit)
- DB connection string
- Identity secrets (if any custom)
- External providers (if added later)

---

## 🖼️ 6. Media (product images)

We have two viable approaches:

### 6.1 Store `ImageUrl` as external URL (fastest)
- Keep images on a CDN/Unsplash during development
- Works with current mocks

### 6.2 First-class media storage (production-ready)
- Store uploads in:
  - Azure Blob Storage (recommended)
  - Or local filesystem for dev only
- Persist only references in DB:
  - `ImageUrl` or `BlobKey`

---

## 📈 7. Observability (logging + health)

### 7.1 Logging
- Use structured logging (built-in logging is fine initially)
- Add correlation id per request/circuit if needed

### 7.2 Health checks
- `/health` endpoint including DB connectivity check

### 7.3 Error handling
- Use Blazor `ErrorBoundary` for isolating UI crashes
- Standardize user-friendly error states on screens (loading/error/empty)

---

## 🚦 8. CI/CD (baseline)

Minimum pipeline gates:
- restore + build
- run tests (unit/integration if available)
- database migrations compile check (optional)

Recommended additions:
- formatting/linting
- publish artifacts
- deploy to staging

---

## ☁️ 9. Hosting (Azure target)

### 9.1 App hosting
- Azure App Service (Windows or Linux)
- .NET runtime pinned to expected version

### 9.2 Database hosting
- Azure SQL Database (or SQL Server VM for legacy constraints)

### 9.3 Deployment model
- One app containing both Storefront + Admin areas.
- Optionally split later:
  - API service + UI service (if moving to SPA or mobile clients).

---

## 🧪 10. “Get data from live site” (practical extraction checklist)

The live site may render as a SPA and can fail to load in some environments. We still need the **contracts** it uses.

### 10.1 What we need to extract
- **Routes/screens**: navigation structure (admin vs storefront)
- **Entities/DTOs**: field names, required vs optional, enums
- **API endpoints**: base URL + routes + HTTP methods
- **Auth flow**: login endpoint and token/cookie behavior

### 10.2 How to extract (recommended)
- Browser DevTools → Network tab:
  - filter by `fetch`/`xhr`
  - export HAR
  - identify JSON shapes
- If the API is separate:
  - capture base API host and reuse it for mock-data generation

### 10.3 Output artifacts (in-repo)
When we start extraction, we should commit:
- `docs/mock-data/*.json` with real payload samples (sanitized)
- a short mapping doc:
  - endpoint → DTO → screen usage

---

## Documentation

- 🏠 [Main Documentation](../README.md) - Project overview and requirements
- 📋 [UI Patterns Quick Start](UI_PATTERNS_QUICK_START.md) - UI conventions and templates
- 📋 [Code Patterns](CODE_PATTERNS_AND_INFRASTRUCTURE.md) - Engineering patterns used in the solution

---

**© 2026 AdminSense. All rights reserved.**

