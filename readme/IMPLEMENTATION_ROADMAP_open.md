# Implementation roadmap — stock, checkout, Mollie & open backlog

![Status](https://img.shields.io/badge/Status-Core%20done%20%2B%20open%20backlog-22c55e?style=flat-square) ![Scope](https://img.shields.io/badge/Scope-0–E%20%2B%20media%20%2B%20alerts-512BD4?style=flat-square)

> **Purpose:** Single delivery tracker for WebShopABMATIC — phased checkboxes, open backlog, and **dev-first** priority.  
> **Mark ✅ when done · ⬜ when pending · 🔶 partial.**  
> **Analysis:** [SPEC_STOCK_OPERATIONS_PROPOSAL.md](./SPEC_STOCK_OPERATIONS_PROPOSAL.md)  
> **Related:** [PAYMENTS_open.md](./PAYMENTS_open.md) · [AZUREBLOB.md](./AZUREBLOB.md)

### Delivery model (owner rule)

| Track | Goal | When |
|-------|------|------|
| **Dev 100%** | All features working locally with **mocks** (`Mollie:UseMock`, `Notifications:LowStock:UseMock`, local file media) | **Now — finish this first** |
| **Prod go-live** | Real Mollie, SMTP worker, Azure Blob — needs client credentials / infra | **Last — only after dev is 100%** |

**Mocks count as ✅ for dev.** Prod adapters are a separate go-live track — never block dev priority.

### Master phase map

| Phase | Focus | Dev status | Prod go-live |
|-------|--------|------------|--------------|
| **Auth** | Legacy cookie (`StaffUsers` + `Klanten.Klant`) | ✅ Done | — |
| **0** | Seeds + pricing foundation | ✅ Done | — |
| **A** | Stock admin (read-only) | ✅ Done | — |
| **B** | Checkout + Mollie | ✅ Done (mock) | ⬜ **B.9** real E2E — last |
| **C** | Store & admin order visibility | ✅ Done | — |
| **D** | Stock writes + low-stock in-app | ✅ Done | — |
| **3b** | Low stock email | ✅ Done (mock + in-app KPI from stock grid) | ⬜ SMTP worker — last |
| **M** | Product images | ✅ Done | ✅ Azure Blob (`files`) |
| **E** | PO / GRN / transfer / reservation | ⬜ Pending | — |
| **F** | SignalR real-time stock (optional) | ⬜ Pending | — |
| **G** | Audit hooks (no-op in legacy DB) | ✅ Done | — |

**Historical build order:** **0 → A ∥ B → C → D → …**

### Dev priority — finish dev 100% first

1. **E** — PO → GRN → transfer → reserve on checkout *(if in scope)*  
2. **F** — SignalR *(optional)*  
3. **E.8** — Refresh [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md)

### Prod go-live — last (after dev 100%)

Do **not** start until dev track above is complete:

1. **3b** — SMTP / background worker for low-stock queue  
2. **B.9** — Mollie real E2E — [PAYMENTS_open.md](./PAYMENTS_open.md)

---

## ⬜ Open backlog — dev (finish 100% first)

### E — Stock ops

Optional for minimal webshop; needed for ERP-style inbound stock.

| Item | Status | Today |
|------|--------|-------|
| **E.2 PO CRUD** | ⬜ | Seed + read-only KPI — no admin CRUD |
| **E.3 GRN** | ⬜ | Model only — no receive UI |
| **E.1 Transfer** | ⬜ | No UI/API / paired movements |
| **D.7 / E reservation** | ⬜ | Grid + seeds show reserved; checkout does not increment |

### F — SignalR (optional)

- ⬜ Real-time stock refresh — not started

### Later (E extras — not MVP)

- ⬜ **E.4** `AccountingDocument` on payment  
- ⬜ **E.5** Mollie refunds in admin  
- ⬜ **E.6** Retry payment / expired session UX  
- ⬜ **E.8** Refresh [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) (outdated status text)

---

## ⬜ Prod go-live — last (after dev 100%)

### 3b — Low stock email send (production)

| Item | Dev | Prod |
|------|-----|------|
| In-app alerts + dashboard | ✅ | ✅ |
| `MockLowStockEmailNotifier` (`UseMock=true`) | ✅ | n/a |
| `LowStockEmailNotifier` → `Emails.EmailMessages` | ✅ | ✅ |
| **SMTP / background worker** | n/a | ⬜ |
| Production SMTP settings | n/a | ⬜ |

See [DATA_DEMO_SEED.md](./DATA_DEMO_SEED.md) (email queue rows only).

### B.9 — Mollie E2E

| Item | Dev | Prod |
|------|-----|------|
| Code + `MollieMockPaymentAdapter` | ✅ | n/a |
| `Mollie:ApiKey`, webhook, E2E checklist | n/a | ⬜ — [PAYMENTS_open.md](./PAYMENTS_open.md) |

### M.5 — Azure Blob (production) ✅

- ✅ Real Azure Blob storage adapter — [AZUREBLOB.md](./AZUREBLOB.md) (container `files`, SAS URLs)

---

## ✅ Done (summary)

| Area | Status |
|------|--------|
| Foundation 0, stock read A, checkout B.1–B.8, visibility C | ✅ |
| Stock writes D.1–D.4, D.6; manual adjustment; sale hooks | ✅ |
| Low stock KPI + email mock (Phase 3b dev) | ✅ Grid KPI; `NullLowStockAlertService` |
| Audit `StockAdjust` calls (Phase G) | ✅ Logged via `IAuditService` → `NullAuditService` (no `dbo.AuditLogs`) |
| Mollie integration + dev mock (B.9a) | ✅ |
| Product media + seeds (Phase M dev) | ✅ |
| Azure Blob production adapter (M.5) | ✅ |
| `ReservedQuantity` display + available calc | ✅ display only |

---

## Phase 0 — Foundation ✅

- ✅ **0.1** Seed `ProductPrices` in `Sql/seeds.sql`
- ✅ **0.2** Seed `PaymentMethods` + `PaymentTerms`
- ✅ **0.3** Seed `OrderStatuses` with `ReserveStock` / `AffectsStock` flags
- ✅ **0.4** `IProductPricingPort` + repository
- ✅ **0.5** Wire `StoreCatalogService` to real prices
- ✅ **0.6** EF migration: Mollie columns on `OrderAdvancePayments`
- ✅ **0.7** NuGet `Mollie.Api` + DI (`MollieDependencyInjection`)
- ✅ **0.8** `IMolliePaymentPort` + `MolliePaymentAdapter`
- ✅ **0.9** Update [DATA_DEMO_SEED.md](./DATA_DEMO_SEED.md)

---

## Phase A — Stock admin (read-only) ✅

- ✅ **A.1** Hub cards: overview + movement journal
- ✅ **A.2** `IStockOverviewPort` + `/admin/stock/overview`
- ✅ **A.3** Movement journal `/admin/stock/movements`
- ✅ **A.4** Dashboard KPIs: movements (7d), open POs
- ✅ **A.5** Seed: `StockMovements` + open `StockOrder` demo
- ✅ **A.6** Update [SPEC_ADMIN.md](./SPEC_ADMIN.md) routes

---

## Phase B — Checkout + Mollie

- ✅ **B.1** `ICheckoutPort` + `CheckoutUseCase` + `IStoreOrderRepository`
- ✅ **B.2** Persist `Order` + `OrderLine` + `OrderAdvancePayment`
- ✅ **B.3** Cart: `PaymentMethods` + delivery addresses from DB
- ✅ **B.4** Server-side totals + stock validation
- ✅ **B.5** Pre-pay: Mollie payment → hosted checkout URL
- ✅ **B.6** Webhook + idempotent handler → paid + stock
- ✅ **B.7** `/orders/{id}/payment-return` + confirmation
- ✅ **B.8** Post-pay path (no Mollie)
- ✅ **B.9a** `MollieMockPaymentAdapter` when `Mollie:UseMock=true` — **dev done**
- ⬜ **B.9** Real Mollie test key + public webhook + E2E — [PAYMENTS_open.md](./PAYMENTS_open.md) — **prod go-live (last)**

---

## Phase C — Visibility ✅

- ✅ **C.1** Customer `/orders` list
- ✅ **C.2** Customer `/orders/{id}` + payment badge
- ✅ **C.3** Admin orders: payment status + Mollie id
- ✅ **C.4** Admin: `OrderAdvancePayments` read-only
- ✅ **C.5** Seed demo advance payments

---

## Phase D — Stock integration ✅ (core)

- ✅ **D.1** Manual adjustment — `/admin/stock/adjustment` + API
- ✅ **D.2** PrePay paid + PostPay checkout → `ApplySaleFromOrderAsync`
- ✅ **D.3** Movement journal: `OrderLineId` on sales
- ✅ **D.4** Negative stock blocked
- ✅ **D.5** Low stock KPI on dashboard + product-stock grid (`ILowStockReadRepository`); storefront hints where implemented
- ✅ **D.6** `ReservedQuantity` / available on product-stock grid + seeds
- ⬜ **D.7** Reservation on checkout (`ReservedQuantity` increment) — see Phase E

---

## Phase 3b — Low stock email ✅ (dev) · ⬜ (prod)

- ✅ **3b.1** `LowStockEmailNotifier` queues to `Emails.EmailMessages`
- ✅ **3b.2** `MockLowStockEmailNotifier` for Development
- ✅ **3b.3** Dev mock: `Notifications:LowStock:UseMock=true` — **dev done**
- ✅ **3b.4** Dashboard low-stock widget from legacy `ProductStockLocations` (not `dbo.StockLowAlerts`)
- ⬜ **3b.5** Background worker / SMTP sender — **prod go-live (last)**
- ⬜ **3b.6** Production SMTP configuration — **prod go-live (last)**

---

## Phase M — Product media ✅

Detail: [AZUREBLOB.md](./AZUREBLOB.md)

- ✅ **M.1** `IProductMediaPort` + `LocalProductMediaService`
- ✅ **M.2** Admin product upload
- ✅ **M.3** Store catalog reads `AzureFiles` (fallback images)
- ✅ **M.4** Seed `AzureFileFolders` + `AzureFiles` in `seeds.sql` (all `ShowOnWebshop` products)
- ✅ **M.5** Real Azure Blob storage adapter (account `abmatic`, container `files`, SAS URLs)

---

## Phase E — Stock ops & extras ⬜

### Stock operations
- ⬜ **E.1** Transfer between locations (UI + API + paired movements)
- ⬜ **E.2** Purchase order CRUD (`StockOrder` + lines)
- ⬜ **E.3** Receive delivery (GRN) linked to PO

### Payments & accounting (later)
- ⬜ **E.4** `AccountingDocument` on payment
- ⬜ **E.5** Mollie refunds in admin
- ⬜ **E.6** Retry payment / expired session UX

### Docs
- ✅ **E.7** [PAYMENTS_open.md](./PAYMENTS_open.md) written
- ⬜ **E.8** Update [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) to match current Blazor store

---

## Phase F — SignalR (optional) ⬜

- ⬜ **F.1** Push stock updates to admin / store UI

---

## Phase G — Audit hooks ✅ (no persistent table)

- ✅ **G.1** `StockAdjust` in `AuditActions` + UI badge styling (when audit UI returns)
- ✅ **G.2** `StockMovementService` calls `IAuditService.LogAsync` (sale + manual)
- ✅ **G.3** `NullAuditService` — legacy-only DB has no `dbo.AuditLogs`
- ✅ **G.4** `AuditSuppressionContext` on stock movement writes

> Retired: `ApplicationDbContext`, `/admin/audit-logs`, `AuditLogRepository`. Optional future: write audit to legacy `Logging` tables.

---

## Definition of done (per checkbox)

| Criterion | Applies to |
|-----------|------------|
| Builds with 0 errors | All |
| Manual test documented in PR / commit message | B, D, E |
| Hexagonal ports + use cases (no logic only in Razor) | A–E |
| `seeds.sql` runnable after change | 0, A, C, M |

---

## Progress

```
DEV (finish first)
Phase 0   [██████████] 9/9
Phase A   [██████████] 6/6
Phase B   [██████████] dev mock ✅
Phase C   [██████████] 5/5
Phase D   [█████████░] D.7 → Phase E
Phase 3b  [██████████] dev mock ✅
Phase M   [██████████] M.1–M.5 ✅
Phase E   [░░░░░░░░░░] ⬜ PO / GRN / transfer / reserve
Phase F   [░░░░░░░░░░] ⬜ optional SignalR
Phase G   [██████████] hooks only (NullAuditService) ✅

PROD GO-LIVE (last — after dev 100%)
B.9 Mollie E2E     ⬜
3b SMTP worker     ⬜
M.5 Azure Blob     ✅
```

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements
- 📋 [Analysis proposal](./SPEC_STOCK_OPERATIONS_PROPOSAL.md) — diagrams & decisions
- 🗂️ [Data model](./DATA_DUTCH_ENGLISH_MODEL.md) — legacy auth + Mollie columns

---

**© 2026 AdminSense. All rights reserved.**
