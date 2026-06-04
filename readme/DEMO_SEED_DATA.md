# Demo seed data (SQL)

![Status](https://img.shields.io/badge/Status-Ready-28a745?style=flat-square) ![Script](https://img.shields.io/badge/Script-seeds.sql-0d47a1?style=flat-square) ![Database](https://img.shields.io/badge/Database-WebShopABMATIC-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white) ![Schemas](https://img.shields.io/badge/Schemas%20seeded-5-512BD4?style=flat-square)

**Idempotent demo dataset** for local testing of the admin dashboard, product/customer/order lists, and webshop-related KPIs.

**Script:** [`scripts/seeds.sql`](../scripts/seeds.sql)

---

## Executive summary

| Item | Value |
|------|--------|
| **Target database** | `WebShopABMATIC` |
| **SQL Server instance** | `MULLER` (Windows authentication) |
| **Seed script** | `scripts/seeds.sql` |
| **Prerequisite** | EF schema applied (`InitialIdentity` + `InitialDomain` migrations) |
| **Schemas with data** | `Crm`, `Customers`, `Accounting`, `Projects`, `Products` |
| **Schemas not seeded** | `Tasks`, `Emails`, `Files`, `Logging`, `Settings`, and most optional `Projects` / `Products` tables |
| **Identity users** | Not in `seeds.sql` — created on first dev run via `IdentitySeed` (`admin@webshop.com`, etc.) |

---

## 1. Connection strings

The application reads **`DefaultConnection`** (Identity) and **`connWebShopABMATIC`** (domain) from:

| File | Database |
|------|----------|
| `Web/appsettings.json` | `WebShopABMATIC` on `MULLER` |
| `Web/appsettings.Development.json` | Same as above (Development overrides LocalDB) |

Example:

```text
Server=MULLER;Database=WebShopABMATIC;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```

Both contexts use the **same database** in development so admin lists and Identity login share one server catalog.

---

## 2. Prerequisites (schema before seed)

`seeds.sql` only **INSERT**s demo rows. Tables must already exist.

Apply migrations once against `WebShopABMATIC` on `MULLER`:

```powershell
$c = "Server=MULLER;Database=WebShopABMATIC;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

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
sqlcmd -S MULLER -E -d WebShopABMATIC -i "scripts\seeds.sql"
```

The script is **idempotent**: it deletes prior demo rows in dependency order, reseeds identities, then inserts fresh data. Safe to re-run before a demo.

On success, `sqlcmd` prints a summary table (products on webshop, orders this month, YTD revenue, etc.) and `Demo seed completed successfully.`

---

## 4. SQL schemas populated (domains)

The full EF model defines **11 schemas** and **139+ tables** ([DUTCH_ENGLISH_DATA_MODEL.md](DUTCH_ENGLISH_DATA_MODEL.md)). The demo seed touches **five** of them.

### 4.1 Included schemas

| Schema | Tables seeded | Purpose |
|--------|---------------|---------|
| **Crm** | `Country`, `City`, `CustomerStatuses`, `Manufacturer`, `Supplier` | Lookups for addresses and product master data |
| **Customers** | `CustomerTypes`, `Customers` | B2B customers and `WebshopLogin` for storefront testing |
| **Accounting** | `VatTypes` | 21% VAT on order lines |
| **Projects** | `DeliveryTypes`, `OrderProcessingTypes`, `Project`, `Orders`, `OrderLines` | Sales pipeline and admin dashboard order KPIs |
| **Products** | `StockLocations`, `Product`, `ProductStockLocations`, `WebshopStructures` | Catalog, low-stock alerts, webshop tree nodes |

### 4.2 Not populated (empty after seed)

| Schema / area | Examples | Why omitted |
|---------------|----------|-------------|
| **Tasks** | `TaskItems`, `TaskTemplates`, `TaskDependencies` | No admin screens wired yet |
| **Emails** | `EmailMessages`, `EmailQueues` | Out of scope for dashboard demo |
| **Files** | `AzureFiles`, `StoredFiles` | No file-upload flows in current UI |
| **Logging** | `AppErrors`, `ProjectActivities` | Operational data, not demo content |
| **Settings** | `AppSettings`, `BaseCompany`, `DocumentTemplates` | Uses defaults / not required for lists |
| **Projects** (other) | `OrderStatuses`, `OrderRemarks`, `OrderLogs`, … | Only orders + lines needed for KPIs |
| **Products** (other) | `WebshopProductStructures`, `ProductPrices`, `ProductOptions`, … | Catalog list uses `Product` + flags |
| **Customers** (other) | `Contact`, `CustomerContacts`, `CustomerDeliveryAddresses` | List uses `Customers` only |

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
| Low stock alerts | 5–7 | `Products.ProductStockLocations` where `Quantity <= MinQuantity` |
| Revenue YTD | ~29,384 | Sum of `OrderLines.TotalExclVat` on accepted orders in current UTC year |

Admin dashboard reads these via `IAdminDashboardPort` / `AdminDashboardService` ([INFRASTRUCTURE.md](INFRASTRUCTURE.md)).

---

## 7. Sample identities (not in SQL)

Login accounts are **not** inserted by `seeds.sql`. In Development, `IdentitySeedHostedService` creates:

| Email | Password | Roles |
|-------|----------|-------|
| `admin@webshop.com` | `Admin@12345` | Admin, Manager |
| `manager@webshop.com` | `Manager@12345` | Manager |
| `customer@webshop.com` | `Customer@12345` | Customer |

Domain row for storefront: `Customers.Customers.WebshopLogin = customer@webshop.com` (seeded in SQL).

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
| Cannot open database | DB missing on `MULLER` | `CREATE DATABASE [WebShopABMATIC]` |
| App shows zeros | Wrong connection string | Confirm `appsettings.Development.json` → `MULLER` / `WebShopABMATIC` |
| FK / duplicate key on re-run | Partial failed run | Re-run full `seeds.sql` (deletes demo tables first) |
| SSMS shows old DB only | Wrong instance | Connect to **MULLER**, not `(localdb)` |

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements
- 🏗️ [Infrastructure](INFRASTRUCTURE.md) — Connection strings, migrations, admin services
- 📊 [Data model](DUTCH_ENGLISH_DATA_MODEL.md) — All schemas and table inventory
- 🖥️ [Admin panel](ADMIN.md) — Dashboard and hub screens fed by this data
- 📋 [Code Patterns](CODE_PATTERNS_AND_INFRASTRUCTURE.md) — Readme and documentation standards

---

**© 2026 AdminSense. All rights reserved.**
