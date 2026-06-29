# Demo data on Azure SQL

![Status](https://img.shields.io/badge/Status-Ready-28a745?style=flat-square) ![Database](https://img.shields.io/badge/Database-abmatic__test-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white) ![Schemas](https://img.shields.io/badge/Demo%20schemas-8-512BD4?style=flat-square)

**Demo dataset** on `abmatic_test` for testing the admin dashboard, product/customer/order lists, and webshop-related KPIs.

---

## Executive summary

| Item | Value |
|------|--------|
| **Target database** | `abmatic_test` |
| **SQL Server instance** | `abmatic.database.windows.net` (Azure SQL) |
| **Prerequisite** | EF schema: `InitialDomain` + `OrderAdvancePaymentMollieColumns` on `WebShopABMATICDbContext` |
| **Schemas with demo data** | `Crm`, `Customers`, `Accounting`, `Projects`, `Products`, `Files`, `Settings`, `Emails` |
| **Schemas without demo data** | `Tasks`, `Logging`, and most optional `Projects` / `Products` tables |
| **Auth** | `Settings.StaffUsers` + `Klanten.Klant` (`LoginWebshop` / `PasswordWebshop`) |
| **Full ERP export** | `ABMATIC.bacpac` (~3 GB) — offline; see [section 8](#8-full-erp-database-abmaticbacpac) |

---

## 1. Connection strings

The application reads **`connWebShopABMATIC`** from:

| File | Database |
|------|----------|
| `WebShopABMATIC/appsettings.json` | `abmatic_test` on `abmatic.database.windows.net` |
| `WebShopABMATIC/appsettings.Development.json` | Same as above (or User Secrets) |

Example (credentials in `appsettings.json` / User Secrets):

```text
Server=tcp:abmatic.database.windows.net,1433;Database=abmatic_test;User Id=<user>;Password=<password>;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=False;
```

Single `WebShopABMATICDbContext` against the ABMATIC legacy database.

---

## 2. Schema prerequisites

Apply EF migrations when the model changes:

```powershell
$c = "Server=tcp:abmatic.database.windows.net,1433;Database=abmatic_test;User Id=<user>;Password=<password>;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=False;"

dotnet ef database update `
  --project WebShopABMATIC\Persistence\WebShopABMATIC.Data.Persistence.csproj `
  --startup-project WebShopABMATIC\WebShopABMATIC.csproj `
  --context WebShopABMATICDbContext `
  --connection $c
```

Demo rows live in the shared Azure database — maintained outside this repository.

---

## 3. Schemas with demo data

The EF model defines **11 schemas** and **139 tables**. Demo content covers **eight** schema areas.

### 3.1 Included

| Schema | Tables | Purpose |
|--------|--------|---------|
| **Crm** | `Country`, `City`, `CustomerStatuses`, `Manufacturer`, `Supplier`, `PaymentTerms`, `CustomerProductDiscounts` | Lookups + discounts |
| **Customers** | `CustomerTypes`, `Customers`, `Contact`, `CustomerContacts`, `CustomerDeliveryAddresses` | B2B + contacts + addresses |
| **Accounting** | `VatTypes`, `DocumentTypes`, `AccountingDocuments` | VAT + demo invoice |
| **Projects** | `DeliveryTypes`, `OrderStatuses`, `OrderProcessingTypes`, `Project`, `Orders`, `OrderLines`, `OrderAdvancePayments` | Sales + Mollie demo |
| **Products** | `StockLocations`, `Product`, `ProductPrices`, `ProductStockLocations`, `WebshopStructures`, `WebshopProductStructures`, `ProductOptions`, `ProductOptionValue`, `ProductQuantityTiers`, `PriceListCategories`, `StockOrder` (+ lines + GRN) | Catalog + stock admin |
| **Files** | `AzureFileFolders`, `AzureFiles` | Product images |
| **Settings** | `PaymentMethods`, `UserGroups`, `StaffUsers`, `BaseCompany`, `BaseCompanyVatNumber` | Checkout + staff |
| **Emails** | `EmailQueues`, `EmailMessages` | Queued low-stock demo |

Detail: [DATA_SUMMARY.md](./DATA_SUMMARY.md).

### 3.2 Not populated

| Schema / area | Examples | Why empty |
|---------------|----------|-----------|
| **Tasks** | `TaskItems`, `TaskTemplates` | No admin screens yet |
| **Logging** | `AppErrors`, `ProjectActivities` | Operational only |
| **Settings** (other) | `AppSettings`, `DocumentTemplates` | Defaults sufficient |
| **Accounting** (other) | `AccountingDocumentLines` | Header-only demo invoice |

---

## 4. Data relationships

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
```

1. **Crm** country/city → supplier + manufacturer  
2. **Customers** — four customers (incl. `customer@webshop.com`)  
3. **Products** — 12 SKUs (10 with `ShowOnWebshop = 1`), stock rows, 12 `WebshopStructures` nodes  
4. **Projects** — one project per customer → 34 orders → order lines (YTD revenue ~**29,384**)

---

## 5. Demo volumes (dashboard targets)

| KPI / list | Count | Source |
|------------|-------|--------|
| Products on webshop | 10 | `Products.Product` where `ShowOnWebshop = 1` |
| Webshop structure nodes | 12 | `Products.WebshopStructures` |
| Customers | 4 | `Customers.Customers` |
| Orders this month | 24 | `Projects.Orders` (current UTC month) |
| Pending orders | 8 | `Projects.Orders` where `IsAccepted = 0` |
| Low stock (product rows) | 5 | `Products.ProductStockLocations` where `Quantity <= MinQuantity` |
| Order advance payments | 3 | `Projects.OrderAdvancePayments` |
| Revenue YTD | ~29,384 | Sum of accepted `OrderLines.TotalExclVat` (UTC year) |

Admin dashboard: `IAdminDashboardPort` / `AdminDashboardUseCase`.

---

## 6. Login accounts (demo)

| Portal | Table | Demo login | Password |
|--------|-------|------------|----------|
| Admin | `Settings.StaffUsers` | `admin@webshop.com` | `demo` |
| Admin | `Settings.StaffUsers` | `manager@webshop.com` | `demo` |
| Store | `Customers.Customers` | `customer@webshop.com` | `demo` |

Staff: `Password` on `StaffUsers`. Store: `LoginWebshop` / `PasswordWebshop` / `SaltWebshop`.

**Blazor:** `/sign-in`, `/sign-up`, `/admin/login` → `/account/store-login` or `/account/admin-login`.

**Production ERP data:** use credentials already in those tables.

---

## 7. Product catalog (demo SKUs)

| Part number | Name (EN) | On webshop | Notes |
|-------------|-----------|------------|--------|
| HDD-001 … HDD-006 | Hard drive 1–6 | Yes | Catalog demo |
| ACC-001, ACC-002 | Rack kit, SATA cables | Yes | Accessories |
| SRV-001, SRV-002 | Installation, warranty | Yes | Services |
| INT-001, INT-002 | Internal spare, adapter | No | Internal-only |

---

## 8. Troubleshooting

| Symptom | Likely cause | Action |
|---------|--------------|--------|
| Invalid object name | Schema not migrated | Run EF `database update` (section 2) |
| Cannot open database | DB missing | Verify `abmatic_test` on Azure SQL |
| App shows zeros | Wrong connection string | Check `appsettings.Development.json` |
| SSMS wrong data | Wrong server | Connect to `abmatic.database.windows.net` |
| Login failed | Firewall | Add client IP in Azure SQL firewall |

---

## 9. Full ERP database (`ABMATIC.bacpac`)

| Aspect | Detail |
|--------|--------|
| **File** | `ABMATIC.bacpac` (~3 GB) |
| **Contents** | Full ABMATIC schema + production-like data (Dutch names) |
| **In repo** | Not committed — offline / shared storage |

Day-to-day development uses **`abmatic_test`** on Azure with the demo subset above — not a local bacpac restore.

For integration dry-runs: import bacpac to a **dedicated Azure SQL** instance via SSMS or `SqlPackage.exe`.

---

## Documentation

- 🏠 [Main Documentation](../README.md)
- 📊 [Data summary](./DATA_SUMMARY.md)

---

**© 2026 AdminSense. All rights reserved.**
