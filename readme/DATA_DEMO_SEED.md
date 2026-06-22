# Demo seed data (SQL)

![Status](https://img.shields.io/badge/Status-Ready-28a745?style=flat-square) ![Script](https://img.shields.io/badge/Script-seeds.sql-0d47a1?style=flat-square) ![Database](https://img.shields.io/badge/Database-WebShopABMATIC-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white) ![Schemas](https://img.shields.io/badge/Schemas%20seeded-5-512BD4?style=flat-square)

**Idempotent demo dataset** for local testing of the admin dashboard, product/customer/order lists, and webshop-related KPIs.

**Script:** [`scripts/seeds.sql`](../scripts/seeds.sql)

---

## Executive summary

| Item | Value |
|------|--------|
| **Target database** | `abmatic_test` |
| **SQL Server instance** | `abmatic.database.windows.net` (Azure SQL, SQL authentication) |
| **Seed script** | `scripts/seeds.sql` |
| **Prerequisite** | EF schema applied (`InitialIdentity` + `InitialDomain` + `OrderAdvancePaymentMollieColumns` + `CustomerIdentityUserId` + `ApplicationUserCustomerId`) |
| **Schemas with data** | `Crm`, `Customers`, `Accounting`, `Projects`, `Products`, **`Files`**, **`Settings`**, **`Emails`** (queues + demo messages) |
| **Schemas not seeded** | `Tasks`, `Emails`, `Logging`, and most optional `Projects` / `Products` tables |
| **Identity users** | Not used for login — credentials in `StaffUsers` + `Customers` via `seeds.sql` |
| **Full legacy DB** | `ABMATIC.bacpac` exists (~3 GB) — **not** for local import; see [section 10](#10-full-legacy-database-abmaticbacpac) |

---

## 1. Connection strings

The application reads **`DefaultConnection`** (Identity) and **`connWebShopABMATIC`** (domain) from:

| File | Database |
|------|----------|
| `Web/appsettings.json` | `abmatic_test` on `abmatic.database.windows.net` (Azure SQL) |
| `Web/appsettings.Development.json` | Same as above |

Example (credentials not shown — see `appsettings.json` / User Secrets):

```text
Server=tcp:abmatic.database.windows.net,1433;Database=abmatic_test;User Id=<user>;Password=<password>;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=False;
```

Both contexts use the **same database** so admin lists and Identity login share one server catalog.

---

## 2. Prerequisites (schema before seed)

`seeds.sql` only **INSERT**s demo rows. Tables must already exist.

Apply migrations once against `abmatic_test` on `abmatic.database.windows.net`:

```powershell
sqlcmd -S abmatic.database.windows.net -d abmatic_test -U <user> -P <password> -i scripts\apply-pending-schema.sql
```

Or the all-in-one script:

```powershell
.\scripts\apply-local-database.ps1
```

Alternative (EF CLI — keeps `__EFMigrationsHistory` in sync if you prefer):

```powershell
$c = "Server=tcp:abmatic.database.windows.net,1433;Database=abmatic_test;User Id=<user>;Password=<password>;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=False;"

dotnet ef database update `
  --project Infrastructure\WebShopABMATIC.Infrastructure.csproj `
  --startup-project Web\WebShopABMATIC.Web.csproj `
  --context ApplicationDbContext `
  --connection $c

dotnet ef database update `
  --project Persistence\WebShopABMATIC.Data.Persistence.csproj `
  --startup-project Web\WebShopABMATIC.Web.csproj `
  --context WebShopABMATICDbContext `
  --connection $c
```

Create the database in SSMS or:

```sql
CREATE DATABASE [WebShopABMATIC] COLLATE Latin1_General_CI_AS;
```

---

## 3. Run the seed script

From the repository root:

```powershell
sqlcmd -S abmatic.database.windows.net -d abmatic_test -U <user> -P <password> -i "scripts\seeds.sql"
```

The script is **idempotent**: it deletes prior demo rows in dependency order, reseeds identities, then inserts fresh data. Safe to re-run before a demo.

On success, `sqlcmd` prints a summary table (products on webshop, orders this month, YTD revenue, etc.) and `Demo seed completed successfully.`

---

## 4. SQL schemas populated (domains)

The full EF model defines **11 schemas** and **139+ tables**. The demo seed touches **five** of them.

### 4.1 Included schemas

| Schema | Tables seeded | Purpose |
|--------|---------------|---------|
| **Crm** | `Country`, `City`, `CustomerStatuses`, `Manufacturer`, `Supplier`, **`PaymentTerms`**, **`CustomerProductDiscounts`** | Lookups + customer discounts |
| **Customers** | `CustomerTypes`, `Customers`, **`Contact`**, **`CustomerContacts`**, `CustomerDeliveryAddresses` | B2B + contacts + checkout addresses |
| **Accounting** | `VatTypes`, **`DocumentTypes`**, **`AccountingDocuments`** | VAT + demo invoice |
| **Projects** | `DeliveryTypes`, **`OrderStatuses`**, `OrderProcessingTypes`, `Project`, `Orders`, `OrderLines`, **`OrderAdvancePayments`** | Sales + Mollie demo |
| **Products** | `StockLocations`, `Product`, **`ProductPrices`**, `ProductStockLocations`, `WebshopStructures`, **`WebshopProductStructures`**, **`ProductOptions`**, **`ProductOptionValue`**, **`ProductQuantityTiers`**, **`PriceListCategories`**, `StockOrder` (+ lines + GRN) | Full catalog admin |
| **Files** | **`AzureFileFolders`**, **`AzureFiles`** | Storefront images |
| **Settings** | **`PaymentMethods`**, **`UserGroups`**, **`StaffUsers`**, **`BaseCompany`**, **`BaseCompanyVatNumber`** | Checkout + staff |
| **App (dbo)** | **`StockLowAlerts`**, **`AuditLogs`** | Dashboard alerts + audit |
| **Emails** | **`EmailQueues`**, **`EmailMessages`** (2 demo) | Queued low-stock demo |

Inventory detail: [DATA_SUMMARY.md](./DATA_SUMMARY.md) · [SUNDAY.md](./SUNDAY.md).

### 4.2 Not populated (empty after seed)

| Schema / area | Examples | Why omitted |
|---------------|----------|-------------|
| **Tasks** | `TaskItems`, `TaskTemplates`, `TaskDependencies` | No admin screens wired yet |
| **Emails** (send) | SMTP worker | Prod go-live — demo rows exist in DB |
| **Files** (other) | `StoredFiles` | Binary attachments — not used for catalog images |
| **Logging** | `AppErrors`, `ProjectActivities` | Operational data, not demo content |
| **Settings** (other) | `AppSettings`, `DocumentTemplates` | Uses defaults / not required for lists |
| **Projects** (other) | `OrderRemarks`, `OrderLogs`, … | Not required for lists |
| **Accounting** (other) | `AccountingDocumentLines` | Header-only demo invoice |

**Tasks** is part of the legacy model but is **not** used by `seeds.sql`.

---

## 5. Data relationships (seed graph)

```mermaid
flowchart TB
  subgraph Crm
    Country --> City
    City --> Manufacturer
    City --> Supplier
    CustomerStatuses
  end

  subgraph Customers
    DeliveryTypes --> CustomerTypes
    CustomerTypes --> Customers
  end

  subgraph Products
    Supplier --> Product
    Manufacturer --> Product
    Product --> ProductStockLocations
    StockLocations --> ProductStockLocations
    WebshopStructures
  end

  subgraph Projects
    DeliveryTypes
    VatTypes
    OrderProcessingTypes
    Customers --> Project
    Project --> Orders
    Orders --> OrderLines
    Product --> OrderLines
  end

  DeliveryTypes
  VatTypes
  OrderProcessingTypes
```

Logical chain in plain terms:

1. **Crm** country/city → supplier + manufacturer  
2. **Customers** type + four customers (including `customer@webshop.com`)  
3. **Products** — 12 SKUs (10 with `ShowOnWebshop = 1`, aligned with HDD mock names), stock rows, 12 `WebshopStructures` nodes  
4. **Projects** — one project per customer → orders (24 in current UTC month, 8 pending) → order lines (YTD revenue tuned to ~**29,384**)

---

## 6. Demo volumes (dashboard targets)

After a successful run, expect approximately:

| KPI / list | Count | Source |
|------------|-------|--------|
| Products on webshop | 10 | `Products.Product` where `ShowOnWebshop = 1` |
| Webshop structure nodes | 12 | `Products.WebshopStructures` |
| Customers | 4 | `Customers.Customers` |
| Orders this month | 24 | `Projects.Orders` (`CreatedAt` ≥ first day of current UTC month) |
| Pending orders | 8 | `Projects.Orders` where `IsAccepted = 0` |
| Low stock (product rows) | 5 | `Products.ProductStockLocations` where `Quantity <= MinQuantity` |
| In-app stock alerts (unread) | 3 | `dbo.StockLowAlerts` where `IsRead = 0` |
| Audit log rows | 12 | `dbo.AuditLogs` (incl. `CheckoutStarted`, `PaymentPaid`) |
| Order advance payments | 3 | `Projects.OrderAdvancePayments` (Mollie paid/open + post-pay) |
| Webshop product structures | 11 | `Products.WebshopProductStructures` |
| Product options / values | 3 / 7 | `ProductOptions` / `ProductOptionValue` |
| Product quantity tiers | 4 | `ProductQuantityTiers` |
| Price list categories | 3 | `PriceListCategories` |
| User groups / staff users | 3 / 3 | `UserGroups` / `StaffUsers` |
| Customer product discounts | 3 | `Crm.CustomerProductDiscounts` |
| Contacts / customer contacts | 3 / 3 | `Contact` / `CustomerContacts` |
| Stock GRN rows | 1 | `StockOrderDeliveries` |
| Accounting documents | 1 | `Accounting.AccountingDocuments` |
| Queued demo emails | 2 | `Emails.EmailMessages` |
| Revenue YTD | ~29,384 | Sum of `OrderLines.TotalExclVat` on accepted orders in current UTC year |

Full inventory: [SUNDAY.md](./SUNDAY.md).

Admin dashboard reads these via `IAdminDashboardPort` / `AdminDashboardUseCase`.

---

## 7. Login accounts (legacy — in `seeds.sql`)

Runtime auth uses **legacy ABMATIC cookie auth** (`LegacySignInService`) — **not** `AspNetUsers`.

| Portal | Table | Demo login (after `seeds.sql`) | Password |
|--------|-------|----------------------------------|----------|
| Admin | `Settings.StaffUsers` | `admin@webshop.com` | `demo` |
| Admin | `Settings.StaffUsers` | `manager@webshop.com` | `demo` |
| Store | `Customers.Customers` | `customer@webshop.com` | `demo` |

Staff passwords are **plaintext** in `StaffUsers.Password`. Store passwords use `PasswordWebshop` / `SaltWebshop` (demo customer uses plaintext hash with NULL salt).

**Azure with real ERP data:** use credentials that already exist in those tables — do not assume demo passwords.

`scripts/seed-identity.ps1` and `IdentitySeed.cs` are **deprecated** for login (not wired in current `Program.cs`).

**Audit demo rows** are in `seeds.sql` (`AuditLogs`); no separate identity step required.

---

## 8. Product catalog (seed SKUs)

| Part number | Name (EN) | On webshop | Notes |
|-------------|-----------|------------|--------|
| HDD-001 … HDD-006 | Hard drive 1–6 | Yes | Matches storefront mock catalog |
| ACC-001, ACC-002 | Rack kit, SATA cables | Yes | Accessories |
| SRV-001, SRV-002 | Installation, warranty | Yes | Services |
| INT-001, INT-002 | Internal spare, legacy adapter | No | Internal-only rows |

---

## 9. Troubleshooting

| Symptom | Likely cause | Action |
|---------|--------------|--------|
| Invalid object name | Schema not migrated | Run EF `database update` (section 2) |
| Cannot open database | DB missing on `abmatic.database.windows.net` | Create `abmatic_test` on the Azure SQL server |
| App shows zeros | Wrong connection string | Confirm `appsettings.Development.json` → `abmatic.database.windows.net` / `abmatic_test` |
| FK / duplicate key on re-run | Partial failed run | Re-run full `seeds.sql` (deletes demo tables first) |
| SSMS shows old DB only | Wrong instance | Connect to **abmatic.database.windows.net**, not a local instance |
| Login failed / firewall error | Azure SQL firewall | Add your client IP in the Azure SQL server firewall rules |

---

## 10. Full legacy database (`ABMATIC.bacpac`)

The repository also references a **full legacy ABMATIC database export**: **`ABMATIC.bacpac`**.

| Aspect | Detail |
|--------|--------|
| **File** | `ABMATIC.bacpac` |
| **Size** | ~**3 GB** (compressed export; restored database is larger) |
| **Contents** | Complete legacy ABMATIC schema and production-like data (Dutch names) |
| **In repo** | Usually **not** committed — too large for Git; kept offline or in shared storage |

### Why not import locally

Importing `ABMATIC.bacpac` into a **local SQL Server** instance (e.g. LocalDB) is **not recommended** for day-to-day development:

- The restored database consumes **substantial disk space** (often several times the `.bacpac` size).
- Most developers only need **admin lists, dashboard KPIs, and storefront smoke tests** — covered by `seeds.sql` on the English `WebShopABMATIC` schema.
- Local machines rarely have room for both the full legacy DB and the vNext `WebShopABMATIC` database comfortably.

For local work, use **`scripts/seeds.sql`** on `WebShopABMATIC` after EF migrations (sections 2–3 above).

### Production / Azure integration path

For **posterior integrations** with real legacy data (ETL, validation against live volumes, migration dry-runs):

1. Use a **dedicated Azure SQL Database** instance (or the **production database environment**), not a developer laptop.
2. Import `ABMATIC.bacpac` there via SSMS **Import Data-tier Application** or `SqlPackage.exe`.
3. Run ETL / mapping scripts against that Azure copy (English schema mapping is documented in the main README index).
4. Point integration jobs at Azure; keep local dev on `seeds.sql` + `WebShopABMATIC`.

> **Note:** Production imports affect cost, storage, and security. Restrict access to the Azure instance used for bacpac restore and treat it as **non-production** unless explicitly approved for live cutover.

Alternative without bacpac: `scripts/ABMATIC-create-local.sql` or `Bkp_Db/WebShopABMATIC-create-local.sql` for schema-only or smaller local setups.

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**