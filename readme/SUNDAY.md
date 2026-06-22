# Demo seed inventory — `seeds.sql`

![Status](https://img.shields.io/badge/Status-Complete-28a745?style=flat-square) ![Script](https://img.shields.io/badge/Source-scripts%2Fseeds.sql-0d47a1?style=flat-square) ![Tables](https://img.shields.io/badge/Seeded%20tables-40%2B-512BD4?style=flat-square)

> **Purpose:** Inventory of what `Sql/seeds.sql` populates and which admin screens receive demo data.  
> **Merged summary (rows + screens):** [DATA_SUMMARY.md](./DATA_SUMMARY.md)  
> **Run seed:** [`Sql/README.md`](../Sql/README.md) · [`Sql/seeds.sql`](../Sql/seeds.sql) · [DATA_DEMO_SEED.md](./DATA_DEMO_SEED.md)

**Login (legacy):** credentials in `seeds.sql` — `StaffUsers` + `Customers` (`PasswordWebshop`). See [DATA_DEMO_SEED.md](./DATA_DEMO_SEED.md) §7.

### Coverage statistics

<table>
<colgroup>
<col style="width:28%">
<col style="width:12%">
<col style="width:14%">
<col style="width:46%">
</colgroup>
<thead>
<tr><th>Area</th><th>Tables</th><th>Seed</th><th>Admin / usage</th></tr>
</thead>
<tbody>
<tr><td><strong>Lookups</strong></td><td>9</td><td>✅</td><td>Order forms, VAT, payment methods</td></tr>
<tr><td><strong>Settings</strong></td><td>3</td><td>✅</td><td>User groups, base company</td></tr>
<tr><td><strong>CRM</strong></td><td>3</td><td>✅</td><td>Manufacturers, suppliers, discounts</td></tr>
<tr><td><strong>Customers</strong></td><td>4</td><td>✅</td><td>Customers, addresses, contacts</td></tr>
<tr><td><strong>Catalog</strong></td><td>5</td><td>✅</td><td>Products, prices, webshop structures</td></tr>
<tr><td><strong>Catalog extra</strong></td><td>3</td><td>✅</td><td>Options, tiers, price-list categories</td></tr>
<tr><td><strong>Media</strong></td><td>2</td><td>✅</td><td><code>AzureFileFolders</code> + <code>AzureFiles</code> (all webshop SKUs)</td></tr>
<tr><td><strong>Stock</strong></td><td>5</td><td>✅</td><td>Locations, movements, PO + GRN demo</td></tr>
<tr><td><strong>Sales</strong></td><td>4</td><td>✅</td><td>Orders, lines, advance payments</td></tr>
<tr><td><strong>Alerts & audit</strong></td><td>2</td><td>✅</td><td>Dashboard low-stock, audit logs</td></tr>
<tr><td><strong>Email</strong></td><td>2</td><td>✅</td><td>Queue demo (SMTP send = prod)</td></tr>
<tr><td><strong>Staff</strong></td><td>1</td><td>✅</td><td>Legacy domain staff users</td></tr>
</tbody>
</table>

---

## O que está no `seeds.sql`

| Área | Tabelas | Rows (demo) | Ecrã admin / uso |
|------|---------|-------------|------------------|
| **Lookups** | Country, City, VatTypes, DeliveryTypes, PaymentTerms, OrderStatuses, PaymentMethods, CustomerTypes, CustomerStatuses, OrderProcessingTypes | 1–3 each | Formulários encomenda / lookups |
| **Settings** | UserGroups, BaseCompany, BaseCompanyVatNumber | 3 / 1 / 1 | `/admin/user-groups`, accounting |
| **Accounting** | DocumentTypes, AccountingDocuments | 2 / 1 | Spec / demo invoice (sem lista admin) |
| **CRM** | Manufacturer, Supplier, CustomerProductDiscounts | 1 / 1 / 3 | `/admin/customer-discounts` |
| **Clientes** | Customers, CustomerDeliveryAddresses, Contact, CustomerContacts | 4 / 5 / 3 / 3 | `/admin/customers`, checkout, contactos |
| **Catálogo** | Product, ProductPrices, WebshopStructures, WebshopProductStructures, PriceListCategories | 12 / 12 / 12 / 11 / 3 | Produtos, categorias, preços |
| **Catálogo extra** | ProductOptions, ProductOptionValue, ProductQuantityTiers | 3 / 7 / 4 | `/admin/product-options`, `/admin/product-tiers` |
| **Imagens** | AzureFileFolders, AzureFiles | 1 / 10 | Loja + form produto — ver [AZUREBLOB.md](./AZUREBLOB.md) |
| **Stock** | StockLocations, ProductStockLocations, StockMovements, StockOrder + lines, StockOrderDeliveries | 1 / 12 / 8 / 1 PO (3 lines) / 1 GRN | Stock hub, movimentos, PO demo |
| **Vendas** | Project, Orders, OrderLines, OrderAdvancePayments | 4 / ~34 / many / 3 | `/admin/orders`, Mollie demo |
| **Alertas** | StockLowAlerts | 3 unread | Dashboard low-stock |
| **Audit** | AuditLogs | 12 | `/admin/audit-logs` |
| **Email** | EmailQueues, EmailMessages | 2 / 2 | Fila demo (envio real = prod) |
| **Staff (domínio)** | StaffUsers | 3 | `/admin/staff-users` |

**Produtos webshop** (`ShowOnWebshop = 1`): **10** — cada um com linha `AzureFiles` primária; `ProductStructureId` ligado a `WebshopProductStructures` no ambiente demo.

> **BD cliente (`abmatic_test`):** categorias da loja vêm de `Products.ProductStructuur` (ERP), não de `WebshopProductStructures` (vazio em produção). O seed demo mantém `WebshopProductStructures` para testes locais.

---

## Estado por ecrã admin

| Tabela | Ecrã admin | Seed | Notas |
|--------|------------|------|-------|
| `WebshopProductStructures` | `/admin/webshop-product-structures` | ✅ 11 rows | Árvore NL/FR/EN + `Product.ProductStructureId` (demo) |
| `ProductOptions` / `ProductOptionValue` | `/admin/product-options` | ✅ 3 + 7 | HDD-001 + cabo |
| `ProductQuantityTier` | `/admin/product-tiers` | ✅ 4 | SKUs 1–3 |
| `PriceListCategory` | `/admin/price-list-categories` | ✅ 3 | Storage, Accessories, Services |
| `UserGroup` | `/admin/user-groups` | ✅ 3 | Sales, Warehouse, Installation |
| `CustomerProductDiscount` | `/admin/customer-discounts` | ✅ 3 | Clientes 1, 2, 4 |
| `StaffUser` | `/admin/staff-users` | ✅ 3 | Legacy domain users (≠ Identity) |
| `StockOrderDeliveries` | *(Phase E UI ⬜)* | ✅ 1 GRN | Dados demo; CRUD admin = Phase E |
| `AccountingDocuments` | sem lista admin | ✅ 1 | Fatura paga ligada à order `2026009` |
| `EmailMessages` | sem lista admin | ✅ 2 | Fila `LowStockAlerts` — **worker SMTP = prod** |
| `Contact` / `CustomerContacts` | sem lista dedicada | ✅ 3 + 3 | Dados para CRM / futuro ecrã |

Todas as listas admin da tabela acima com ✅ têm dados após correr `Sql\seeds.sql`.

---

## Fora do SQL

| O quê | Como |
|-------|------|
| Login `admin@` / store `customer@` | `StaffUsers` + `Customers` in `seeds.sql` (`demo` password) |
| Setup schema + seed | `sqlcmd … -i Sql\apply-pending-schema.sql` then `-i Sql\seeds.sql` |

---

## Fora do âmbito do seed (fases futuras — não bloqueia este doc)

| Item | Motivo |
|------|--------|
| `AccountingDocumentLines` | Sem UI; cabeçalho de documento basta para demo |
| Filtro loja por `ProductStructuurWebshop` | BD cliente usa `ProductStructuur` — já implementado na app |
| PO / GRN / transfer **CRUD** | Phase E — dados demo existem, UI ⬜ ([roadmap](./IMPLEMENTATION_ROADMAP_open.md)) |
| Reserva stock no checkout | Phase E — `ReservedQuantity` só display |
| Envio real de email | Prod — mock in-app ✅ |

---

## Checklist (complete)

| # | Task | Status |
|---|------|--------|
| 1 | Lookups + settings | ✅ |
| 2 | Customers + CRM | ✅ |
| 3 | Catalog (12 SKUs, 10 webshop) | ✅ |
| 4 | Options, tiers, price-list categories | ✅ |
| 5 | `AzureFiles` para todos os SKUs webshop | ✅ |
| 6 | Stock + PO + GRN demo | ✅ |
| 7 | Orders + Mollie advance payments | ✅ |
| 8 | Low-stock alerts + audit + email queue | ✅ |
| 9 | Staff users (domínio) | ✅ |
| 10 | Legacy login users in SQL (`StaffUsers` + customer `PasswordWebshop`) | ✅ |

---

## Resumo

- **Um ficheiro:** `Sql/seeds.sql` (idempotente).
- **Re-seed:** `sqlcmd … -i Sql\seeds.sql`
- **Inventário completo:** [DATA_SUMMARY.md](./DATA_SUMMARY.md)
- **Imagens em produção:** [AZUREBLOB.md](./AZUREBLOB.md)

---

## Documentation

- 🏠 [Main Documentation](../README.md)
- 📋 [Sql index](../Sql/README.md)
- 📋 [Demo seed runbook](./DATA_DEMO_SEED.md)
- 🗺️ [Implementation roadmap](./IMPLEMENTATION_ROADMAP_open.md)

---

**© 2026 AdminSense. All rights reserved.**
