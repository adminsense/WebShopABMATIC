# ðŸ“Š Demo data summary â€” Azure + admin screens

<p style="display:flex;flex-wrap:nowrap;gap:0.35rem;align-items:center;overflow-x:auto;margin:0.5rem 0 0;"><img alt="Status" src="https://img.shields.io/badge/Status-Live%20on%20Azure%20SQL-28a745?style=flat-square" /><img alt="Tables" src="https://img.shields.io/badge/Demo%20tables-40%2B-512BD4?style=flat-square" /><img alt="Database" src="https://img.shields.io/badge/Database-abmatic__test-CC2927?style=flat-square&amp;logo=microsoftsqlserver&amp;logoColor=white" /></p>

> **Purpose:** Demo tables on `abmatic.database.windows.net` / `abmatic_test`, related admin screens, and approximate row counts.  
---

## Executive summary

| Item | Value |
|------|--------|
| **Target database** | `abmatic_test` on Azure SQL `abmatic.database.windows.net` |
| **Login** | `StaffUsers` + `Klanten.Klant` on `abmatic_test` |
| **Schemas with data** | `Crm`, `Customers`, `Accounting`, `Projects`, `Products`, `Files`, `Settings`, `Emails` |
| **Admin coverage** | Orders, stock, CRM, catalog extras, accounting demo, email queue |

### ðŸ“ˆ Key metrics (live counts)

| KPI | Rows | Screen | Status |
|-----|-----:|--------|--------|
| ðŸ“¦ **Admin products (all SKUs)** | 12 | `/admin/products` | âœ… |
| ðŸ‘¥ **Customers** | 4 | `/admin/customers` | âœ… |
| ðŸ§¾ **Orders** | 34 | `/admin/orders` | âœ… |
| ðŸ“‹ **Order lines** | 35 | `/admin/orders` | âœ… |
| âš ï¸ **Low-stock product rows** | 5 | `/admin` dashboard Â· `/admin/product-stock` | âœ… |
| ðŸ’° **Revenue YTD (accepted)** | ~29 384 | `/admin` dashboard | âœ… |

### âœ… Demo coverage quality

| Aspect | Status | Details |
|--------|--------|---------|
| **Core lookups** | âœ… Complete | Country, VAT, order statuses, payment methods |
| **Catalog & media** | âœ… Complete | Products, prices, webshop structures, `AzureFiles` |
| **CRM & discounts** | âœ… Complete | Customers, addresses, contacts, product discounts |
| **Sales & payments** | âœ… Complete | Orders, lines, advance payments (Mollie mock) |
| **Stock & PO** | ðŸŸ¢ Present | Locations, movements, open PO + partial GRN row |
| **Email queue** | ðŸ”· Demo only | Queued rows âœ… â€” SMTP worker = **prod** |
| **Login** | âœ… In SQL | `StaffUsers` + `LoginWebshop` / `PasswordWebshop` on `Klanten.Klant` |

### ðŸ“‹ Categories summary

| Category | Tables | Rows (approx.) | Admin screens | Demo |
|----------|--------|----------------|---------------|------|
| ðŸ·ï¸ **Lookups** | 9 | 12 | VAT, delivery types, payment methods, â€¦ | âœ… |
| âš™ï¸ **Settings** | 4 | 8 | User groups, staff users, base company | âœ… |
| ðŸ‘¤ **Customers & CRM** | 7 | 19 | Customers, addresses, discounts, suppliers | âœ… |
| ðŸ“¦ **Catalog** | 10 | 74 | Products, prices, structures, options, tiers | âœ… |
| ðŸ–¼ï¸ **Media** | 2 | 11 | Product images (Azure Blob `files` or local fallback) | âœ… |
| ðŸ“Š **Stock** | 6 | 27 | Locations, movements, PO, GRN demo | âœ… |
| ðŸ›ï¸ **Sales** | 4 | 76 | Orders, lines, advance payments | âœ… |
| âœ‰ï¸ **Email** | 2 | 4 | Infra only (no admin list) | âœ… |

---

## ðŸ“Š 1. Master Table â€” Schema, Rows, Screen

| Area | Table (schema) | Rows | Screen / usage | Demo | Notes |
|------|------------------|-----:|----------------|------|-------|
| **Lookups** | `Crm.Country` | 1 | Lookups / forms | âœ… | Belgium |
| | `Crm.City` | 1 | Lookups / forms | âœ… | Brussels |
| | `Accounting.VatTypes` | 1 | `/admin/vat-types` | âœ… | 21% VAT |
| | `Projects.DeliveryTypes` | 1 | `/admin/delivery-types` | âœ… | Standard delivery |
| | `Crm.PaymentTerms` | 1 | Orders | âœ… | 30 days net |
| | `Projects.OrderStatuses` | 3 | `/admin/order-statuses` | âœ… | Pending / Paid / Accepted |
| | `Settings.PaymentMethods` | 2 | `/admin/payment-methods` + checkout | âœ… | Mollie PrePay + invoice PostPay |
| | `Customers.CustomerTypes` | 1 | `/admin/customer-types` | âœ… | B2B Dealer |
| | `Crm.CustomerStatuses` | 1 | Lookups | âœ… | Active |
| | `Projects.OrderProcessingTypes` | 1 | Lookups | âœ… | Webshop |
| **Settings** | `Settings.UserGroups` | 3 | `/admin/user-groups` | âœ… | Sales, Warehouse, Installation |
| | `Settings.BaseCompany` | 1 | Accounting / company | âœ… | Demo BV |
| | `Settings.BaseCompanyVatNumber` | 1 | Accounting | âœ… | Linked to company |
| | `Settings.StaffUsers` | 3 | `/admin/staff-users` + **admin login** | âœ… | `Login` / `Password` (plaintext) |
| **Accounting** | `Accounting.DocumentTypes` | 2 | Spec only | âœ… | Invoice + credit note |
| | `Accounting.AccountingDocuments` | 1 | Spec only (no admin list) | âœ… | Paid invoice â†’ order `2026009` |
| **CRM** | `Crm.Manufacturer` | 1 | `/admin/manufacturers` | âœ… | Demo Manufacturer |
| | `Crm.Supplier` | 1 | `/admin/suppliers` | âœ… | Demo Supplier |
| | `Crm.CustomerProductDiscounts` | 3 | `/admin/customer-discounts` | âœ… | Customers 1, 2, 4 |
| **Customers** | `Customers.Customers` | 4 | `/admin/customers` | âœ… | `LoginWebshop` / `PasswordWebshop` |
| | `Crm.CustomerDeliveryAddresses` | 5 | `/admin/delivery-addresses` + checkout | âœ… | 2 addresses for customer 4 |
| | `Customers.Contact` | 3 | CRM (no dedicated list) | âœ… | Buyers + supplier contact |
| | `Customers.CustomerContacts` | 3 | CRM (no dedicated list) | âœ… | Linked to customers |
| **Catalog** | `Products.Product` | 12 | `/admin/products` | âœ… | 10 webshop + 2 internal |
| | `Products.Product` *(webshop)* | 10 | Store catalog (separate app area) | âœ… | `ShowOnWebshop = 1` |
| | `Products.ProductPrices` | 12 | `/admin/product-prices` | âœ… | 1 row per product |
| | `Products.WebshopStructures` | 12 | `/admin/webshop-structures` | âœ… | Navigation tree |
| | `Products.WebshopProductStructures` | 11 | `/admin/webshop-product-structures` | âœ… | NL/FR/EN + `ProductStructureId` |
| | `Products.PriceListCategories` | 3 | `/admin/price-list-categories` | âœ… | Storage, Accessories, Services |
| **Catalog extra** | `Products.ProductOptions` | 3 | `/admin/product-options` | âœ… | HDD-001 + cable pack |
| | `Products.ProductOptionValue` | 7 | `/admin/product-options` | âœ… | Capacity / interface / length |
| | `Products.ProductQuantityTiers` | 4 | `/admin/product-tiers` | âœ… | Products 1â€“3 |
| **Media** | `Files.AzureFileFolders` | 1 | Product media | âœ… | Folder â€œProductsâ€ |
| | `Files.AzureFiles` *(primary web)* | 10 | `/admin/products` (+ store when enabled) | âœ… | `BlobRef` â†’ `/images/productN.png` |
| **Stock** | `Products.StockLocations` | 1 | `/admin/stock-locations` | âœ… | Main warehouse |
| | `Products.ProductStockLocations` | 12 | `/admin/product-stock` | âœ… | Low-stock + reserved demo |
| | `Products.StockMovements` | 8 | `/admin/stock/movements` | âœ… | In/out/reservation mix |
| | `Products.StockOrder` *(open)* | 1 | `/admin/stock/overview` KPI | âœ… | Demo open PO |
| | `Products.StockOrderLines` | 3 | PO demo | âœ… | HDD 1â€“3 |
| | `Products.StockOrderDeliveries` | 1 | GRN demo *(Phase E UI â¬œ)* | âœ… | Partial receive on line 1 |
| **Sales** | `Projects.Project` | 4 | Orders (1 per customer) | âœ… | Webshop projects |
| | `Projects.Orders` | 34 | `/admin/orders` | âœ… | 24 this month, 8 pending |
| | `Projects.OrderLines` | 35 | `/admin/orders` | âœ… | incl. YTD top-up line |
| | `Projects.OrderAdvancePayments` | 3 | `/admin/orders` + Mollie | âœ… | paid / open / post-pay |
| **Email** | `Emails.EmailQueues` | 2 | Infra | âœ… | Outbound + LowStockAlerts |
| | `Emails.EmailMessages` | 2 | Queue demo *(no admin list)* | âœ… | Worker SMTP = **prod** |

---

## 2. Catalog & webshop flags (admin)

| Concept | Rows | Admin screen |
|---------|-----:|--------------|
| SKUs with `ShowOnWebshop = 1` | 10 | `/admin/products` filter |
| Primary published images | 10 | `AzureFiles` on product edit |
| Webshop navigation nodes | 12 | `/admin/webshop-structures` |
| Product category labels | 11 | `/admin/webshop-product-structures` |
| Configurable options | 3 options / 7 values | `/admin/product-options` |

---

## 3. Authentication

| Portal | Table | Fields |
|--------|-------|--------|
| Admin | `Settings.StaffUsers` | `Login`, `Password` |
| Store | `Klanten.Klant` | `LoginWebshop`, `PasswordWebshop`, `SaltWebshop` |

Credentials come from the connected `abmatic_test` database only. Use `/admin/customers` or `/admin/staff-users` to find logins; reset webshop passwords in admin when needed.

---

## 4. Not in scope (UI gaps)

| Item | Reason |
|------|--------|
| `AccountingDocumentLines` | Header demo only; no UI |
| PO / GRN / transfer **CRUD** | Phase E â€” demo data âœ…, UI â¬œ |
| Stock reservation at checkout | âœ… PrePay â€” `ApplyReservationFromOrderAsync` (D.7) |
| Real email send (SMTP) | Prod â€” queue demo âœ…, worker â¬œ |
| `Tasks.*` | No admin screens |

---

## 5. Schema policy (DB-first)

> [!IMPORTANT]
> **`abmatic_test` is authoritative.** Do **not** run `dotnet ef database update`, EF migrations, or schema scripts from this repository to alter the ERP database. Change mapping code to match the live schema; schema changes (if ever needed) are DBA/ERP outside this app.

---

## Documentation

- ðŸ  [Main Documentation](../README.md) â€” Project overview and requirements

---

**Â© 2026 AdminSense. All rights reserved.**
