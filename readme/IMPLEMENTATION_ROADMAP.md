# Implementation roadmap — stock, checkout & Mollie

![Status](https://img.shields.io/badge/Status-In%20progress-0d6efd?style=flat-square)

> **Purpose:** Track delivery in small parts. Check `[x]` when done.  
> **Analysis reference:** [STOCK_OPERATIONS_PROPOSAL.md](./STOCK_OPERATIONS_PROPOSAL.md)  
> **Suggested order:** **0 → A ∥ B → C → D → E** (A and B can run in parallel after 0).

| Phase | Focus | Status |
|-------|--------|--------|
| **0** | Seeds + pricing foundation | ⬜ Not started |
| **A** | Stock admin (read-only) | ⬜ Not started |
| **B** | Checkout + Mollie | ⬜ Not started |
| **C** | Store & admin order visibility | ⬜ Not started |
| **D** | Stock writes + reservation on pay | ⬜ Not started |
| **E** | PO / invoice / extras (later) | ⬜ Deferred |

**Current focus:** _Phase 0_

---

## Phase 0 — Foundation (do first)

- [ ] **0.1** Seed `ProductPrices` (1 row per webshop product) in `scripts/seeds.sql`
- [ ] **0.2** Seed `PaymentMethods` (pre-pay + post-pay) + `PaymentTerms`
- [ ] **0.3** Seed `OrderStatuses` with `ReserveStock` / `AffectsStock` flags
- [ ] **0.4** `IProductPricingPort` + repository (active price + discounts)
- [ ] **0.5** Wire `StoreCatalogService` to real prices (remove hardcoded `49.99 + …`)
- [ ] **0.6** EF migration: Mollie columns on `OrderAdvancePayments` (`MolliePaymentId`, status, paid date, checkout URL)
- [ ] **0.7** NuGet `Mollie.Api` + `AddMollieApi` in `Program.cs` (config in user secrets)
- [ ] **0.8** `IMolliePaymentPort` + `MolliePaymentAdapter` (create + get payment)
- [ ] **0.9** Update [DEMO_SEED_DATA.md](./DEMO_SEED_DATA.md) for new seed rows

---

## Phase A — Stock admin (read-only)

- [ ] **A.1** Hub cards: Stock overview + Movement journal in `AdminHubRegistry`
- [ ] **A.2** `IStockOverviewPort` / use case / repository + `/admin/stock/overview`
- [ ] **A.3** Movement journal: filters + grid `/admin/stock/movements`
- [ ] **A.4** Dashboard KPIs: movements (7d), open POs on `/admin` stock card
- [ ] **A.5** Seed: 5–10 `StockMovements` + 1 minimal open `StockOrder` (demo)
- [ ] **A.6** Update [ADMIN.md](./ADMIN.md) routes table

---

## Phase B — Checkout + Mollie (happy path)

- [ ] **B.1** `ICheckoutPort` + `CheckoutUseCase` + `IStoreOrderRepository`
- [ ] **B.2** Persist `Order` + `OrderLine` + `OrderAdvancePayment` on Place order
- [ ] **B.3** Cart: load `PaymentMethods` + delivery addresses from DB
- [ ] **B.4** Server-side totals from `IProductPricingPort` + stock validation
- [ ] **B.5** Pre-pay: create Mollie payment → redirect to hosted checkout
- [ ] **B.6** Webhook endpoint + idempotent handler → mark `OrderAdvancePayment` paid
- [ ] **B.7** `/orders/{id}/payment-return` + `/orders/{id}/confirmation`
- [ ] **B.8** Post-pay path: no Mollie, confirmation only
- [ ] **B.9** Test with Mollie test key + dev tunnel (ngrok / Azure)

---

## Phase C — Visibility (store + admin)

- [ ] **C.1** Customer `/orders` list (own orders only)
- [ ] **C.2** Customer `/orders/{id}` with payment status badge
- [ ] **C.3** Admin `/admin/orders`: columns payment status + Mollie id
- [ ] **C.4** Admin order detail: `OrderAdvancePayments` read-only section
- [ ] **C.5** Seed: `OrderAdvancePayments` on 2–3 demo orders

---

## Phase D — Stock integration (after B.6 works)

- [ ] **D.1** Manual stock adjustment form + use case (transaction)
- [ ] **D.2** On webhook paid: reserve stock if `OrderStatus.ReserveStock`
- [ ] **D.3** Movement journal shows `OrderLineId` link from web orders
- [ ] **D.4** Confirm negative-stock rule with business (document decision)

---

## Phase E — Later (defer until Phases 0–D stable)

### Stock operations
- [ ] **E.1** Transfer between locations
- [ ] **E.2** Purchase order CRUD
- [ ] **E.3** Receive delivery (GRN)

### Payments & accounting
- [ ] **E.4** `AccountingDocument` on payment (invoice parity)
- [ ] **E.5** Mollie refunds in admin
- [ ] **E.6** Retry payment / expired session UX

### Docs & ops
- [ ] **E.7** `readme/PAYMENTS.md` (keys, webhook URL, test vs live)
- [ ] **E.8** [WEB_STORE.md](./WEB_STORE.md) — mark checkout as implemented

---

## Definition of done (per checkbox)

| Criterion | Applies to |
|-----------|------------|
| Builds with 0 errors | All |
| Manual test documented in PR / commit message | B, D |
| Uses hexagonal ports + use cases (no logic only in Razor) | A–D |
| `seeds.sql` runnable after change | 0, A, C |

---

## Quick progress

```
Phase 0  [__________] 0/9
Phase A  [__________] 0/6
Phase B  [__________] 0/9
Phase C  [__________] 0/5
Phase D  [__________] 0/4
Phase E  [__________] 0/8  (deferred)
```

_Update the bars when closing tasks._

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements
- 📋 [Analysis proposal](./STOCK_OPERATIONS_PROPOSAL.md) — diagrams & decisions

---

**© 2026 AdminSense. All rights reserved.**
