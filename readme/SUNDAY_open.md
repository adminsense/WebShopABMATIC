# Demo seed inventory — `seeds.sql`

![Status](https://img.shields.io/badge/Status-Admin%20screens%20seeded-28a745?style=flat-square) ![Script](https://img.shields.io/badge/Source-scripts%2Fseeds.sql-0d47a1?style=flat-square)

> **Purpose:** Living checklist of what `scripts/seeds.sql` populates vs admin screens still empty.  
> **Merged summary (rows + screens):** [DATA_SUMMARY.md](./DATA_SUMMARY.md)  
> **Run seed:** [`scripts/README.md`](../scripts/README.md) · [`scripts/seed-demo.ps1`](../scripts/seed-demo.ps1) · [DATA_DEMO_SEED.md](./DATA_DEMO_SEED.md)

**Identity (login):** [`scripts/seed-identity.ps1`](../scripts/seed-identity.ps1) — not SQL (`Infrastructure/Seeding/IdentitySeed.cs`).

---

## O que já está no `seeds.sql` (e na tua BD)

| Área | Tabelas | Rows (demo) | Ecrã admin / uso |
|------|---------|-------------|------------------|
| **Lookups** | Country, City, VatTypes, DeliveryTypes, PaymentTerms, OrderStatuses, PaymentMethods, CustomerTypes, CustomerStatuses, OrderProcessingTypes | 1–3 each | Formulários encomenda / lookups |
| **Settings** | UserGroups, BaseCompany, BaseCompanyVatNumber | 3 / 1 / 1 | `/admin/user-groups`, accounting |
| **Accounting** | DocumentTypes, AccountingDocuments | 2 / 1 | Spec / demo invoice (sem lista admin) |
| **CRM** | Manufacturer, Supplier, CustomerProductDiscounts | 1 / 1 / 3 | `/admin/customer-discounts` |
| **Clientes** | Customers, CustomerDeliveryAddresses, Contact, CustomerContacts | 4 / 5 / 3 / 3 | `/admin/customers`, checkout, contactos |
| **Catálogo** | Product, ProductPrices, WebshopStructures, WebshopProductStructures, PriceListCategories | 12 / 12 / 12 / 11 / 3 | Produtos, categorias, preços |
| **Catálogo extra** | ProductOptions, ProductOptionValue, ProductQuantityTiers | 3 / 7 / 4 | `/admin/product-options`, `/admin/product-tiers` |
| **Imagens** | AzureFileFolders, AzureFiles | 1 / 10 | Loja + form produto |
| **Stock** | StockLocations, ProductStockLocations, StockMovements, StockOrder + lines, StockOrderDeliveries | 1 / 12 / 8 / 1 PO (3 lines) / 1 GRN | Stock hub, movimentos, PO demo |
| **Vendas** | Project, Orders, OrderLines, OrderAdvancePayments | 4 / ~34 / many / 3 | `/admin/orders`, Mollie demo |
| **Alertas** | StockLowAlerts | 3 unread | Dashboard low-stock |
| **Audit** | AuditLogs | 12 | `/admin/audit-logs` |
| **Email** | EmailQueues, EmailMessages | 2 / 2 | Fila demo (envio real = prod) |
| **Staff (domínio)** | StaffUsers | 3 | `/admin/staff-users` |

**Produtos webshop** (`ShowOnWebshop = 1`): 10 — com `ProductStructureId` ligado a `WebshopProductStructures`.

---

## Estado por ecrã admin (última atualização)

| Tabela | Ecrã admin | Seed | Notas |
|--------|------------|------|-------|
| `WebshopProductStructures` | `/admin/webshop-product-structures` | ✅ 11 rows | Árvore NL/FR/EN + `Product.ProductStructureId` |
| `ProductOptions` / `ProductOptionValue` | `/admin/product-options` | ✅ 3 + 7 | HDD-001 + cabo |
| `ProductQuantityTier` | `/admin/product-tiers` | ✅ 4 | SKUs 1–3 |
| `PriceListCategory` | `/admin/price-list-categories` | ✅ 3 | Storage, Accessories, Services |
| `UserGroup` | `/admin/user-groups` | ✅ 3 | Sales, Warehouse, Installation |
| `CustomerProductDiscount` | `/admin/customer-discounts` | ✅ 3 | Clientes 1, 2, 4 |
| `StaffUser` | `/admin/staff-users` | ✅ 3 | Legacy domain users (≠ Identity) |
| `StockOrderDeliveries` | *(Phase E UI ⬜)* | ✅ 1 GRN | Dados demo; CRUD admin ainda por fazer |
| `AccountingDocuments` | sem lista admin | ✅ 1 | Fatura paga ligada à order `2026009` |
| `EmailMessages` | sem lista admin | ✅ 2 | Fila `LowStockAlerts` — **worker SMTP = prod** |
| `Contact` / `CustomerContacts` | sem lista dedicada | ✅ 3 + 3 | Dados para CRM / futuro ecrã |

---

## Fora do SQL

| O quê | Como |
|-------|------|
| Login `admin@` / `customer@` | `.\scripts\seed-identity.ps1` |
| Setup completo | `.\scripts\apply-local-database.ps1` |

---

## ⬜ Ainda não seedado (sem ecrã ou fase futura)

| Item | Motivo |
|------|--------|
| `AccountingDocumentLines` | Sem UI; documento cabeçalho basta para demo |
| `WebshopProductStructures` ↔ produto na loja | Filtro storefront por categoria — Phase E / store |
| PO / GRN / transfer **CRUD** | Phase E — dados demo existem, UI ⬜ |
| Reserva stock no checkout | Phase E — `ReservedQuantity` só display |
| Envio real de email | Prod — mock in-app ✅ |

---

## Resumo

- **Um ficheiro:** `scripts/seeds.sql` (idempotente).
- **Re-seed:** `.\scripts\seed-demo.ps1`
- **Admin lists:** todas as tabelas da secção “Estado por ecrã” com ✅ têm dados após seed.

---

## Documentation

- 🏠 [Main Documentation](../README.md)
- 📋 [Scripts index](../scripts/README.md)
- 📋 [Demo seed runbook](./DATA_DEMO_SEED.md)

---

**© 2026 AdminSense. All rights reserved.**
