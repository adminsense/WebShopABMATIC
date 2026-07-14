# 📊 Demo data summary — Azure + admin screens

![Status](https://img.shields.io/badge/Status-Live%20on%20Azure%20SQL-28a745?style=flat-square) ![Tables](https://img.shields.io/badge/Demo%20tables-40%2B-512BD4?style=flat-square) ![Database](https://img.shields.io/badge/Database-abmatic__test-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)

> **Purpose:** Demo tables on `abmatic.database.windows.net` / `abmatic_test`, related admin screens, and approximate row counts.  
---

## Executive summary

| Item | Value |
|------|--------|
| **Target database** | `abmatic_test` on Azure SQL `abmatic.database.windows.net` |
| **Login** | `StaffUsers` + `Klanten.Klant` on `abmatic_test` |
| **Schemas with data** | `Crm`, `Customers`, `Accounting`, `Projects`, `Products`, `Files`, `Settings`, `Emails` |
| **Admin coverage** | Orders, stock, CRM, catalog extras, accounting demo, email queue |

### 📈 Key metrics (live counts)

| KPI | Rows | Screen | Status |
|-----|-----:|--------|--------|
| 📦 **Admin products (all SKUs)** | 12 | `/admin/products` | ✅ |
| 👥 **Customers** | 4 | `/admin/customers` | ✅ |
| 🧾 **Orders** | 34 | `/admin/orders` | ✅ |
| 📋 **Order lines** | 35 | `/admin/orders` | ✅ |
| ⚠️ **Low-stock product rows** | 5 | `/admin` dashboard · `/admin/product-stock` | ✅ |
| 💰 **Revenue YTD (accepted)** | ~29 384 | `/admin` dashboard | ✅ |

### ✅ Demo coverage quality

| Aspect | Status | Details |
|--------|--------|---------|
| **Core lookups** | ✅ Complete | Country, VAT, order statuses, payment methods |
| **Catalog & media** | ✅ Complete | Products, prices, webshop structures, `AzureFiles` |
| **CRM & discounts** | ✅ Complete | Customers, addresses, contacts, product discounts |
| **Sales & payments** | ✅ Complete | Orders, lines, advance payments (Mollie mock) |
| **Stock & PO** | 🟢 Present | Locations, movements, open PO + partial GRN row |
| **Email queue** | 🔷 Demo only | Queued rows ✅ — SMTP worker = **prod** |
| **Login** | ✅ In SQL | `StaffUsers` + `LoginWebshop` / `PasswordWebshop` on `Klanten.Klant` |

### 📋 Categories summary

| Category | Tables | Rows (approx.) | Admin screens | Demo |
|----------|--------|----------------|---------------|------|
| 🏷️ **Lookups** | 9 | 12 | VAT, delivery types, payment methods, … | ✅ |
| ⚙️ **Settings** | 4 | 8 | User groups, staff users, base company | ✅ |
| 👤 **Customers & CRM** | 7 | 19 | Customers, addresses, discounts, suppliers | ✅ |
| 📦 **Catalog** | 10 | 74 | Products, prices, structures, options, tiers | ✅ |
| 🖼️ **Media** | 2 | 11 | Product images (Azure Blob `files` or local fallback) | ✅ |
| 📊 **Stock** | 6 | 27 | Locations, movements, PO, GRN demo | ✅ |
| 🛍️ **Sales** | 4 | 76 | Orders, lines, advance payments | ✅ |
| ✉️ **Email** | 2 | 4 | Infra only (no admin list) | ✅ |

---

## 1. Master table — schema, rows, screen

| Area | Table (schema) | Rows | Screen / usage | Demo | Notes |
|------|------------------|-----:|----------------|------|-------|
| **Lookups** | `Crm.Country` | 1 | Lookups / forms | ✅ | Belgium |
| | `Crm.City` | 1 | Lookups / forms | ✅ | Brussels |
| | `Accounting.VatTypes` | 1 | `/admin/vat-types` | ✅ | 21% VAT |
| | `Projects.DeliveryTypes` | 1 | `/admin/delivery-types` | ✅ | Standard delivery |
| | `Crm.PaymentTerms` | 1 | Orders | ✅ | 30 days net |
| | `Projects.OrderStatuses` | 3 | `/admin/order-statuses` | ✅ | Pending / Paid / Accepted |
| | `Settings.PaymentMethods` | 2 | `/admin/payment-methods` + checkout | ✅ | Mollie PrePay + invoice PostPay |
| | `Customers.CustomerTypes` | 1 | `/admin/customer-types` | ✅ | B2B Dealer |
| | `Crm.CustomerStatuses` | 1 | Lookups | ✅ | Active |
| | `Projects.OrderProcessingTypes` | 1 | Lookups | ✅ | Webshop |
| **Settings** | `Settings.UserGroups` | 3 | `/admin/user-groups` | ✅ | Sales, Warehouse, Installation |
| | `Settings.BaseCompany` | 1 | Accounting / company | ✅ | Demo BV |
| | `Settings.BaseCompanyVatNumber` | 1 | Accounting | ✅ | Linked to company |
| | `Settings.StaffUsers` | 3 | `/admin/staff-users` + **admin login** | ✅ | `Login` / `Password` (plaintext) |
| **Accounting** | `Accounting.DocumentTypes` | 2 | Spec only | ✅ | Invoice + credit note |
| | `Accounting.AccountingDocuments` | 1 | Spec only (no admin list) | ✅ | Paid invoice → order `2026009` |
| **CRM** | `Crm.Manufacturer` | 1 | `/admin/manufacturers` | ✅ | Demo Manufacturer |
| | `Crm.Supplier` | 1 | `/admin/suppliers` | ✅ | Demo Supplier |
| | `Crm.CustomerProductDiscounts` | 3 | `/admin/customer-discounts` | ✅ | Customers 1, 2, 4 |
| **Customers** | `Customers.Customers` | 4 | `/admin/customers` | ✅ | `LoginWebshop` / `PasswordWebshop` |
| | `Crm.CustomerDeliveryAddresses` | 5 | `/admin/delivery-addresses` + checkout | ✅ | 2 addresses for customer 4 |
| | `Customers.Contact` | 3 | CRM (no dedicated list) | ✅ | Buyers + supplier contact |
| | `Customers.CustomerContacts` | 3 | CRM (no dedicated list) | ✅ | Linked to customers |
| **Catalog** | `Products.Product` | 12 | `/admin/products` | ✅ | 10 webshop + 2 internal |
| | `Products.Product` *(webshop)* | 10 | Store catalog (separate app area) | ✅ | `ShowOnWebshop = 1` |
| | `Products.ProductPrices` | 12 | `/admin/product-prices` | ✅ | 1 row per product |
| | `Products.WebshopStructures` | 12 | `/admin/webshop-structures` | ✅ | Navigation tree |
| | `Products.WebshopProductStructures` | 11 | `/admin/webshop-product-structures` | ✅ | NL/FR/EN + `ProductStructureId` |
| | `Products.PriceListCategories` | 3 | `/admin/price-list-categories` | ✅ | Storage, Accessories, Services |
| **Catalog extra** | `Products.ProductOptions` | 3 | `/admin/product-options` | ✅ | HDD-001 + cable pack |
| | `Products.ProductOptionValue` | 7 | `/admin/product-options` | ✅ | Capacity / interface / length |
| | `Products.ProductQuantityTiers` | 4 | `/admin/product-tiers` | ✅ | Products 1–3 |
| **Media** | `Files.AzureFileFolders` | 1 | Product media | ✅ | Folder “Products” |
| | `Files.AzureFiles` *(primary web)* | 10 | `/admin/products` (+ store when enabled) | ✅ | `BlobRef` → `/images/productN.png` |
| **Stock** | `Products.StockLocations` | 1 | `/admin/stock-locations` | ✅ | Main warehouse |
| | `Products.ProductStockLocations` | 12 | `/admin/product-stock` | ✅ | Low-stock + reserved demo |
| | `Products.StockMovements` | 8 | `/admin/stock/movements` | ✅ | In/out/reservation mix |
| | `Products.StockOrder` *(open)* | 1 | `/admin/stock/overview` KPI | ✅ | Demo open PO |
| | `Products.StockOrderLines` | 3 | PO demo | ✅ | HDD 1–3 |
| | `Products.StockOrderDeliveries` | 1 | GRN demo *(Phase E UI ⬜)* | ✅ | Partial receive on line 1 |
| **Sales** | `Projects.Project` | 4 | Orders (1 per customer) | ✅ | Webshop projects |
| | `Projects.Orders` | 34 | `/admin/orders` | ✅ | 24 this month, 8 pending |
| | `Projects.OrderLines` | 35 | `/admin/orders` | ✅ | incl. YTD top-up line |
| | `Projects.OrderAdvancePayments` | 3 | `/admin/orders` + Mollie | ✅ | paid / open / post-pay |
| **Email** | `Emails.EmailQueues` | 2 | Infra | ✅ | Outbound + LowStockAlerts |
| | `Emails.EmailMessages` | 2 | Queue demo *(no admin list)* | ✅ | Worker SMTP = **prod** |

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
| PO / GRN / transfer **CRUD** | Phase E — demo data ✅, UI ⬜ |
| Stock reservation at checkout | ✅ PrePay — `ApplyReservationFromOrderAsync` (D.7) |
| Real email send (SMTP) | Prod — queue demo ✅, worker ⬜ |
| `Tasks.*` | No admin screens |

---

## 5. Schema policy (DB-first)

> [!IMPORTANT]
> **`abmatic_test` is authoritative.** Do **not** run `dotnet ef database update`, EF migrations, or schema scripts from this repository to alter the ERP database. Change mapping code to match the live schema; schema changes (if ever needed) are DBA/ERP outside this app.

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**
