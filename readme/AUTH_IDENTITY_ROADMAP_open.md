# Authentication & identity roadmap

![Status](https://img.shields.io/badge/Status-Auth--4%20next-f59e0b?style=flat-square)

> **Purpose:** Unify ASP.NET Identity with the legacy domain model — admin users, store customers, real user IDs on writes, password reset. Mark ✅ when done.  
> **Context:** [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) §3 · [SPEC_ADMIN.md](./SPEC_ADMIN.md) §2  
> **Suggested order:** **Auth-1 → Auth-2 → Auth-3 → Auth-4 → Auth-5 → Auth-6 → Auth-7** (Auth-7 = audit logs, after IDs are stable).

| Phase | Focus | Status |
|-------|--------|--------|
| **Baseline** | Identity tables + login (partial) | 🟡 Partial |
| **Auth-1** | Link Identity ↔ Customer | ✅ Done |
| **Auth-2** | Admin system users (roles) | ✅ Done |
| **Auth-3** | Store sign-up | ✅ Done |
| **Auth-4** | Admin password reset | ✅ Done |
| **Auth-5** | Simplify Settings hub | ✅ Done |
| **Auth-6** | Persist real user IDs on writes | ✅ Done |
| **Auth-7** | Audit logs | ✅ Done (Phase 3 optional) |

**Current focus:** _Auth-7 complete — see [AUDITS_open.md](./AUDITS_open.md) (only `StockAdjust` badge deferred)_

---

## Context — two user worlds today

| System | Tables | Login today? | Linked to domain? |
|--------|--------|--------------|-------------------|
| **ASP.NET Identity** | `AspNetUsers`, `AspNetRoles`, … | ⏳ Not runtime | Migrations in repo; login not wired in `Program.cs` |
| **Legacy staff** | `Settings.StaffUsers` | ✅ Admin login | Plaintext `Login` + `Password` |
| **Legacy customer** | `Customers.Customers` | ✅ Store login | `WebshopLogin` + `PasswordWebshop` / `SaltWebshop` |

**AspNet tables:** created by this project (`Infrastructure/Identity/Migrations/InitialIdentity`), **not** from the legacy ERP schema. Same SQL Server database, separate concern from domain tables.

**Target model:**

```text
AspNetUsers
  ├── AspNetUserRoles → Admin | Manager | Customer
  ├── CustomerId (FK) → Customers  [store buyers]
  └── StaffUserId (FK, optional) → StaffUsers  [HR profile, optional]

Orders.CreatedByUserId / audit fields → resolved via ICurrentUserContext (Identity + legacy bridge)
```

**Strategy for legacy `int UserId` columns:** add `IdentityUserId` / link table where needed (option **B + bridge**); migrate writes in Auth-6 before full audit log (Auth-7).

---

## Roles & authorization policies

Defined in `Application/Auth/AppRoles.cs` and seeded via `IdentitySeed.cs` into `AspNetRoles`.

### Application roles

| Role | Constant | Who | Typical access |
|------|----------|-----|----------------|
| **Admin** | `AppRoles.Admin` | Back-office staff | Full admin panel; user management (`/admin/users`), audit log, customer webshop password reset |
| **Manager** | `AppRoles.Manager` | Back-office staff | Most admin screens; **not** Admin-only actions (e.g. `/admin/users`, audit log) |
| **Customer** | `AppRoles.Customer` | Individual webstore buyer (B2C) | Catalog, cart, orders, **`/my-account`** (name, address, phone, password). Payment method is chosen **at checkout**. Role is fixed after sign-up — **only Admin** can change it in `/admin/users`. |

A user may hold **more than one role** (e.g. Admin + Manager, or Admin + Customer for dual portal access).

### Authorization policies (`AppPolicies`)

| Policy | Requirement | Used for |
|--------|-------------|----------|
| `AdminOnly` | Role **Admin** | `/admin/users`, audit log, some customer password resets |
| `AdminOrManager` | Role **Admin** or **Manager** | Most `/admin/*` CRUD pages |
| `CustomerOnly` | Role **Customer** | Store **`/my-account`** |

### Where each user type manages their account

| User type | Self-service | Admin-managed |
|-----------|--------------|---------------|
| **Customer** (webstore) | `/my-account` — link in store header | `/admin/users` (Admin only) |
| **Admin / Manager** (staff) | `/admin/profile` — sidebar **My profile** | `/admin/users` (Admin only) |

**Staff-only login redirect:** users with Admin/Manager roles **without** Customer are sent to **`/admin`** after sign-in (not the store homepage).

### Demo logins (`seeds.sql` on `abmatic_test`)

| Login | Password | Access |
|-------|----------|--------|
| `admin@webshop.com` | `demo` | Admin + Manager (`StaffUsers`) |
| `manager@webshop.com` | `demo` | Manager |
| `customer@webshop.com` | `demo` | Store customer |

> **Not valid:** `Admin@12345`, `Manager@12345`, `Customer@12345` — those were AspNet Identity demo passwords. Runtime auth uses legacy tables above.

> **Azure ERP data:** when `abmatic_test` has real client rows, use credentials from `[Settings].[StaffUsers]` and `[Customers].[Customers]` in the database.

---

## Baseline — already in place (do not re-check)

- ✅ **B.1** `AspNetUsers` + roles migration (`InitialIdentity`)
- ✅ **B.2** Roles `Admin`, `Manager`, `Customer` (`AppRoles`)
- ✅ **B.3** Dev seed users (`IdentitySeed.cs`)
- ✅ **B.4** Admin login `/Account/Login` + policies on `/admin/*`
- ✅ **B.5** Store sign-in `/sign-in` (Customer role gate)
- ✅ **B.6** `/admin/profile` (edit name, phone, password — inside AdminLayout)
- ✅ **B.7** Checkout customer bridge via `WebshopLogin` only _(replaced in Auth-1 — IdentityUserId + fallback)_

---

## Auth-1 — Link Identity ↔ Customer

- ✅ **1.1** EF: `Customers.IdentityUserId` (nullable, indexed) + optional `ApplicationUser.CustomerId`
- ✅ **1.2** Domain migration + update `WebShopABMATICModelBuilder`
- ✅ **1.3** `IdentitySeed`: after creating `customer@webshop.com`, set `Customers.IdentityUserId` for matching row
- ✅ **1.4** `IStoreCustomerRepository`: resolve by `IdentityUserId` first, fallback `WebshopLogin`
- ✅ **1.5** Update [DATA_DEMO_SEED.md](./DATA_DEMO_SEED.md) — document link column
- ✅ **1.6** Manual test: sign-in as customer → checkout still resolves customer #4

---

## Auth-2 — Admin system users (replace StaffUsers for auth)

- ✅ **2.1** Hub card **Users** → `/admin/users` (Admin-only policy; `/admin/system-users` alias)
- ✅ **2.2** `ISystemUserAdminPort` + use case + list/create/edit via `UserManager` / `RoleManager`
- ✅ **2.3** UI: email, first/last name, roles (Admin / Manager checkboxes), active/lockout
- ✅ **2.4** Create user with temporary password or invite flow
- ✅ **2.5** Deprecate **Staff user** hub card for auth _(keep table for HR later or hide)_
- ✅ **2.6** Update [SPEC_ADMIN.md](./SPEC_ADMIN.md) — system users vs legacy StaffUsers

---

## Auth-3 — Store sign-up

- ✅ **3.1** `/sign-up` page (StoreLayout) — email, name, phone, home address, password (individual consumer; **Customer** role only)
- ✅ **3.2** `ICustomerRegistrationPort` + use case: create `ApplicationUser` (Customer role) + `Customers` row (person name, address) + link IDs
- ✅ **3.3** Auto sign-in after registration → redirect to catalog or cart
- ✅ **3.4** Link from `/sign-in` to sign-up
- ✅ **3.5** Seed: do not duplicate if email exists

---

## Auth-4 — Admin password reset

- ✅ **4.1** System users: **Reset password** action (Admin-only) → set new password via `UserManager`
- ✅ **4.2** Customers admin (`/admin/customers`): **Reset webshop password** for linked Identity user
- ✅ **4.3** Optional: generate random temp password + show once (no email in dev)
- ✅ **4.4** Document dev flow in [SPEC_ADMIN.md](./SPEC_ADMIN.md)

---

## Auth-5 — Simplify Settings hub

- ✅ **5.1** Remove or hide **User group** card from `AdminHubRegistry` (legacy teams — not needed for webshop MVP)
- ✅ **5.2** Re-order Settings: Users, Payment methods, VAT, _(StaffUsers HR optional)_
- ✅ **5.3** Update sidebar active routes

---

## Auth-6 — Persist real user IDs on writes

- ✅ **6.1** `ICurrentUserContext` port: `IdentityUserId`, `CustomerId?`, `StaffUserId?`, display name
- ✅ **6.2** Checkout / `StoreOrderRepository`: `CreatedByUserId` from context (not hardcoded `1`)
- ✅ **6.3** Bridge table or mapping: Identity user → legacy `StaffUsers.Id` where int FK required _(email match on `StaffUsers.Login`)_
- ✅ **6.4** Admin CRUD saves: `CreatedBy` / `ModifiedBy` → Identity id (replace varchar where touched)
- ✅ **6.5** List tables to migrate (priority): `Orders`, `OrderLogs`, `AzureFiles`, `Customers`

---

## Auth-7 — Audit logs

> **Live tracker:** [AUDITS_open.md](./AUDITS_open.md) — Phase 1 + 2 **done**; Phase 3 deferred.

- ✅ **7.1** `AuditLog` entity + EF migration (`ApplicationDbContext.AuditLogs`)
- ✅ **7.2** `IAuditService` + domain `AuditSaveChangesInterceptor` + auth/export hooks
- ✅ **7.3** Admin read-only journal `/admin/audit-logs` (filters, legend, detail modal, export)

---

## Definition of done (per checkbox)

| Criterion | Applies to |
|-----------|------------|
| Builds with 0 errors | All |
| Identity + domain migration applied locally | 1.x, 6.x |
| Manual test documented | 2, 3, 4 |
| Uses hexagonal ports + use cases | 2, 3, 4, 6 |
| Admin-only for user/role management | 2, 4 |

---

## Quick progress

```
Baseline [███████░░░] 7/7
Auth-1   [██████████] 6/6
Auth-2   [██████████] 6/6
Auth-3   [██████████] 5/5
Auth-4   [██████████] 4/4
Auth-5   [██████████] 3/3
Auth-6   [██████████] 5/5
Auth-7   [█████████░] 3/3  (Phase 3 optional)
```

---

## Documentation

- 🏠 [Main Documentation](../README.md)
- 🏗️ [Infrastructure — Identity §3](./SPEC_INFRASTRUCTURE.md)
- 🖥️ [Admin — auth §2](./SPEC_ADMIN.md)
- ✅ [Stock & checkout roadmap](./IMPLEMENTATION_ROADMAP_open.md)

---

**© 2026 AdminSense. All rights reserved.**
