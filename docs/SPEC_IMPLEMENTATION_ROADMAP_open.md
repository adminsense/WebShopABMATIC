# Implementation roadmap — stock, checkout, Mollie & open backlog

![Status](https://img.shields.io/badge/Status-Core%20done%20%2B%20open%20backlog-22c55e?style=flat-square) ![Scope](https://img.shields.io/badge/Scope-0–E%20%2B%20media%20%2B%20alerts-512BD4?style=flat-square)

> **Purpose:** Single delivery tracker for WebShopABMATIC — phased checkboxes, open backlog, and **dev-first** priority.  
> **Mark ✅ when done · ⬜ when pending · 🔶 partial.**  
> **Analysis:** [SPEC_STOCK_OPERATIONS_PROPOSAL.md](./SPEC_STOCK_OPERATIONS_PROPOSAL.md)  
> **Related:** [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md) · [AMENDMENTS.md](./AMENDMENTS.md) §0 (layout loja) · [DATA_AZUREBLOB.md](./DATA_AZUREBLOB.md)

### Delivery model (owner rule)

| Track | Goal | When |
|-------|------|------|
| **Dev 100%** | All features working locally with **mocks** (`Mollie:UseMock`, `Notifications:LowStock:UseMock`, local file media) | **Now — finish this first** |
| **Prod go-live** | Real Mollie (**only after client keys**), SMTP worker, etc. | **Last — never before client delivers Mollie keys** |

**Mocks count as ✅ for dev.** **Mollie stays on mock** until the client sends API keys — see [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md). Agents must not treat real Mollie as current work.

### Master phase map

| Phase | Focus | Dev status | Prod go-live |
|-------|--------|------------|--------------|
| **Auth** | Legacy cookie — `[Instellingen].[User]` + `Customers` | ✅ Done — login unificado §2.2.2 | — |
| **Store layout** | Referência adminsenceweb (sidebar, CD4, deals) | ✅ Done — [AMENDMENTS.md](./AMENDMENTS.md) | — |
| **0** | Pricing foundation on `abmatic_test` | ✅ Done | — |
| **A** | Stock admin (read-only) | ✅ Done | — |
| **B** | Checkout + Mollie | ✅ Done (**mock — required**) | ⬜ **B.9** real E2E — **blocked on client keys** |
| **C** | Store & admin order visibility | ✅ Done | — |
| **D** | Stock writes + low-stock in-app | ✅ Done | — |
| **3b** | Low stock email | ✅ Done (mock + in-app KPI from stock grid) | ⬜ SMTP worker — last |
| **M** | Product images | ✅ Done | ✅ Azure Blob (`files`) |
| **E** | PO / GRN / transfer / reservation | ✅ Done (core) | — |
| **G** | Stock movement logging + legacy audit | ✅ Done | — |

**Historical build order:** **0 → A ∥ B → C → D → …**

### Dev priority — next work (Jul/2026)

1. ~~**S.4**~~ ✅ — server-side required product options on checkout
2. ~~**E.12**~~ ✅ — [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) refreshed (legacy auth, live catalog, no mock SKUs)
3. **Quality** — smoke = `dotnet build`; no test project yet (see `.claude/CLAUDE.md`)

**Done recently:** **S.5** freight from ERP; **S.4** server option validation; **E.12** store SPEC sync.  
**Not in sprint:** real Mollie (**B.9**) until client keys.

### Prod go-live — last (after keys + remaining prod items)

1. **B.9** — Mollie real E2E — **waiting on client keys** — [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md)
2. **3b** — SMTP / background worker for low-stock queue

---

## ⬜ Open backlog — dev (finish 100% first)

### Storefront — ERP forms

Layout loja ✅. Options / category detail mostly shipped; checklist synced to code (Jul/2026):

| Item | Status | Notes |
|------|--------|-------|
| **S.1** `GetProductOptionsAsync` | ✅ | Port + `StoreCatalogService` (admin `/admin/product-options` ✅) |
| **S.2** `StoreProductOptionsForm` + `ProductDetail` | ✅ | Required options gated in UI before add |
| **S.3** `GetCategoryDetailAsync` + intro/outro | ✅ | EN→NL; RTF markup omitted on storefront |
| **S.4** Cart/checkout line options | ✅ | Persist + cart display; **server** validates required options on quote/`PlaceOrderAsync` (`CheckoutUseCase`) |
| **S.5** Delivery / freight from ERP | ✅ | No mock €9 — fee from `OrderDeliveryTypeProduct` + `ProductPrices`; default €0; cart freight select. [DATA_FREIGHT_DELIVERY.md](./DATA_FREIGHT_DELIVERY.md) |
| **S.6** Cart stock blocking UX | ✅ | Stale OOS lines kept; checkout CTA blocked — [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §5.2 |

### Auth — legacy database ✅

| Item | Status | Notes |
|------|--------|-------|
| **A.1** Unified login | ✅ | `POST /account/login` → `SignInAsync` — [AMENDMENTS.md](./AMENDMENTS.md) §2.2.2 |
| **A.2** Staff → admin | ✅ | `[Instellingen].[User]`; flags `Admin` / `Bestellingen` / `Productie` → roles |
| **A.3** Customer → store | ✅ | `Customers.WebshopLogin` + hash/salt |
| **A.4** No AspNet Identity | ✅ | Cookie legacy; policies `AdminOrManager` / `CustomerOnly` |

### G — Legacy audit logging ✅

| Item | Status | Legacy table |
|------|--------|--------------|
| **G.1** `StockAdjust` in `AuditActions` | ✅ | — |
| **G.2** `StockMovementService` → order audit | ✅ | `[Projecten].[DossierLog]` |
| **G.3** `AuditSuppressionContext` on stock writes | ✅ | — |
| **G.4** `LegacyAuditService` + `LegacyAuditWriter` | ✅ | DossierLog / Error / ProjectActiviteit |
| **G.5** `LegacyAuditSaveChangesInterceptor` (admin CRUD) | ✅ | `[Logging].[Error]` |
| **G.6** Auth login/logout/fail | ✅ | `[Logging].[Error]` (`Auth`) |
| **G.7** `LegacyExceptionLoggingMiddleware` | ✅ | `[Logging].[Error]` |
| **G.8** Admin `/admin/audit-logs` + order DossierLog UI | ✅ | — |

Detail: [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) §3.5.

### E — Stock ops

Optional for minimal webshop; needed for ERP-style inbound stock.

| Item | Status | Today |
|------|--------|-------|
| **E.2 PO CRUD** | ✅ | `/admin/stock/purchase-orders` — header + lines |
| **E.3 GRN** | ✅ | `/admin/stock/purchase-orders/{id}/receive` + `ApplyPurchaseOrderReceiveAsync` |
| **E.1 Transfer** | ✅ | `/admin/stock/transfers/new` + paired movements API |
| **D.7 / E reservation** | ✅ | `ApplyReservationFromOrderAsync` on PrePay checkout |
| **E.7 Release reservation** | ✅ | `ReleaseReservationAsync` — reverses reservation on cancel/expire |
| **E.8 Webhook expired/canceled** | ✅ | `ProcessMollieWebhookUseCase` detects terminal Mollie statuses → release |
| **E.9 Reservation expiration service** | ✅ | `ReservationExpirationService` — background job every 5 min, releases after 30 min |
| **E.10 Admin cancel order** | ✅ | `POST /api/admin/stock/orders/{id}/cancel` + button on `/admin/orders` |
| **E.11 Status workflow service** | ✅ | `OrderStockWorkflowService` — evaluates `OrderStatus.ReserveStock/AffectsStock` flags |

### Quality

- ✅ Smoke: `dotnet build WebShopABMATIC.sln` (no dedicated test project yet — `.claude/CLAUDE.md`)
- ⬜ Automated store/admin regression suite — when a test project is added

### Later (E extras — not MVP)

- ⬜ **E.4** `AccountingDocument` on payment  
- ⬜ **E.5** Mollie refunds in admin  
- ⬜ **E.6** Retry payment / expired session UX  
- ✅ **E.12** Refresh [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) to match Blazor store (Jul/2026)
- ⬜ **E.13** OrderStructure admin CRUD (to wire `OrderStockWorkflowService` on status change)

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

Email queue rows exist on `abmatic_test` — see [DATA_SUMMARY.md](./DATA_SUMMARY.md).

### B.9 — Mollie E2E

| Item | Dev | Prod |
|------|-----|------|
| Code + `MollieMockPaymentAdapter` | ✅ **Required until client keys** | n/a |
| Client delivers `Mollie:ApiKey` | n/a | ⬜ **Waiting on client** |
| `Mollie:ApiKey`, webhook, E2E checklist | n/a | ⬜ — [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md) |

Do **not** start B.9 until keys are received. Storefront cart/confirmation UX: [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.4 (not duplicated here).

### M.5 — Azure Blob (production) ✅

- ✅ Real Azure Blob storage adapter — [DATA_AZUREBLOB.md](./DATA_AZUREBLOB.md) (container `files`, SAS URLs)

---

## ✅ Done (summary)

| Area | Status |
|------|--------|
| Foundation 0, stock read A, checkout B.1–B.8, visibility C | ✅ |
| Stock writes D.1–D.4, D.6; manual adjustment; sale hooks | ✅ |
| Low stock KPI + email mock (Phase 3b dev) | ✅ |
| Stock movement audit hooks (Phase G) | ✅ |
| Mollie integration + dev mock (B.9a) | ✅ |
| Product media (Phase M) | ✅ |
| Azure Blob production adapter (M.5) | ✅ |
| `ReservedQuantity` display + available calc | ✅ display only |
| `ReservedQuantity` increment on PrePay checkout | ✅ D.7 |
| `ReleaseReservationAsync` + webhook expired/cancel/failed | ✅ E.7–E.8 |
| Reservation expiration background service | ✅ E.9 |
| Admin cancel order + release stock | ✅ E.10 |
| Legacy `OrderStatus` flag workflow service | ✅ E.11 |

---

## Phase 0 — Foundation ✅

- ✅ **0.1** `ProductPrices` demo rows on `abmatic_test`
- ✅ **0.2** `PaymentMethods` + `PaymentTerms` on `abmatic_test`
- ✅ **0.3** `OrderStatuses` with `ReserveStock` / `AffectsStock` flags
- ✅ **0.4** `IProductPricingPort` + repository
- ✅ **0.5** Wire `StoreCatalogService` to real prices
- ✅ **0.6** Checkout/advance-payment integration on existing ERP fields (DB-first; **no** EF schema migration)
- ✅ **0.7** NuGet `Mollie.Api` + DI (`MollieDependencyInjection`)
- ✅ **0.8** `IMolliePaymentPort` + `MolliePaymentAdapter`
- ✅ **0.9** Update [DATA_SUMMARY.md](./DATA_SUMMARY.md)

---

## Phase A — Stock admin (read-only) ✅

- ✅ **A.1** Hub cards: overview + movement journal
- ✅ **A.2** `IStockOverviewPort` + `/admin/stock/overview`
- ✅ **A.3** Movement journal `/admin/stock/movements`
- ✅ **A.4** Dashboard KPIs: movements (7d), open POs
- ✅ **A.5** `StockMovements` + open `StockOrder` on `abmatic_test`
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
- ⬜ **B.9** Real Mollie test key + public webhook + E2E — [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md) — **prod go-live (last)**

---

## Phase C — Visibility ✅

- ✅ **C.1** Customer `/orders` list
- ✅ **C.2** Customer `/orders/{id}` + payment badge
- ✅ **C.3** Admin orders: payment status + Mollie id
- ✅ **C.4** Admin: `OrderAdvancePayments` read-only
- ✅ **C.5** Advance payments on `abmatic_test`

---

## Phase D — Stock integration ✅ (core)

- ✅ **D.1** Manual adjustment — `/admin/stock/adjustment` + API
- ✅ **D.2** PrePay paid + PostPay checkout → `ApplySaleFromOrderAsync`
- ✅ **D.3** Movement journal: `OrderLineId` on sales
- ✅ **D.4** Negative stock blocked
- ✅ **D.5** Low stock KPI on dashboard + product-stock grid (`ILowStockReadRepository`); storefront hints where implemented
- ✅ **D.6** `ReservedQuantity` / available on product-stock grid
- ✅ **D.7** Reservation on checkout (`ReservedQuantity` increment) — PrePay via `ApplyReservationFromOrderAsync`

---

## Phase 3b — Low stock email ✅ (dev) · ⬜ (prod)

- ✅ **3b.1** `LowStockEmailNotifier` queues to `Emails.EmailMessages`
- ✅ **3b.2** `MockLowStockEmailNotifier` for Development
- ✅ **3b.3** Dev mock: `Notifications:LowStock:UseMock=true` — **dev done**
- ✅ **3b.4** Dashboard low-stock widget from `ProductStockLocations`
- ⬜ **3b.5** Background worker / SMTP sender — **prod go-live (last)**
- ⬜ **3b.6** Production SMTP configuration — **prod go-live (last)**

---

## Phase M — Product media ✅

Detail: [DATA_AZUREBLOB.md](./DATA_AZUREBLOB.md)

- ✅ **M.1** `IProductMediaPort` + `LocalProductMediaService`
- ✅ **M.2** Admin product upload
- ✅ **M.3** Store catalog reads `AzureFiles` (fallback images)
- ✅ **M.4** `AzureFileFolders` + `AzureFiles` demo rows (all `ShowOnWebshop` products)
- ✅ **M.5** Real Azure Blob storage adapter (account `abmatic`, container `files`, SAS URLs)

---

## Phase E — Stock ops & extras ✅ (core)

### Stock operations
- ✅ **E.1** Transfer between locations (UI + API + paired movements)
- ✅ **E.2** Purchase order CRUD (`StockOrder` + lines)
- ✅ **E.3** Receive delivery (GRN) linked to PO

### Payments & accounting (later)
- ⬜ **E.4** `AccountingDocument` on payment
- ⬜ **E.5** Mollie refunds in admin
- ⬜ **E.6** Retry payment / expired session UX

### Docs
- ✅ **E.14** [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md) written
- ✅ **E.12** [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) matched to current Blazor store (legacy auth, live ERP catalog, freight, options)

---

## Phase G — Stock movement logging & legacy audit ✅

- ✅ **G.1** `StockAdjust` in `AuditActions`
- ✅ **G.2** `StockMovementService` logs sale + manual adjustments → `DossierLog` when order-linked
- ✅ **G.3** `AuditSuppressionContext` on stock movement writes
- ✅ **G.4–G.8** Legacy audit — [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) §3.5

---

## Definition of done (per checkbox)

| Criterion | Applies to |
|-----------|------------|
| Builds with 0 errors | All |
| Manual test documented in PR / commit message | B, D, E |
| Hexagonal ports + use cases (no logic only in Razor) | A–E |
| Demo data on Azure updated when features need new rows | 0, A, C, M |

---

## Progress

```
DEV (finish first)
Phase 0   [██████████] 9/9
Phase A   [██████████] 6/6
Phase B   [██████████] dev mock ✅
Phase C   [██████████] 5/5
Phase D   [██████████] D.7 reservation ✅
Phase 3b  [██████████] dev mock ✅
Phase M   [██████████] M.1–M.5 ✅
Phase E   [██████████] E.1–E.11 transfer + PO + GRN + reservations ✅
Phase G   [██████████] Stock movement logging ✅

PROD GO-LIVE (last — after dev 100%)
B.9 Mollie E2E     ⬜
3b SMTP worker     ⬜
M.5 Azure Blob     ✅
```

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements
- 📋 [Analysis proposal](./SPEC_STOCK_OPERATIONS_PROPOSAL.md) — diagrams & decisions
- 🗂️ [Data model](./DATA_DUTCH_ENGLISH_MODEL.md) — Dutch → English mapping

---

**© 2026 AdminSense. All rights reserved.**
