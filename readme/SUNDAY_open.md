# Stock & payments — implementation tracker

![Status](https://img.shields.io/badge/Status-Phases%202–6%20%2B%20C-22c55e?style=flat-square) ![Scope](https://img.shields.io/badge/Scope-Stock%20%2B%20Mollie-512BD4?style=flat-square) ![Related](https://img.shields.io/badge/See-SPEC__STOCK__OPERATIONS__PROPOSAL-informational?style=flat-square)

> **Purpose:** Track stock decrement on sales, manual stock entry, Mollie go-live, and `StockAdjust` audit.  
> **Related:** [SPEC_STOCK_OPERATIONS_PROPOSAL.md](./SPEC_STOCK_OPERATIONS_PROPOSAL.md) · [AUDITS_open.md](./AUDITS_open.md) · [IMPLEMENTATION_ROADMAP_open.md](./IMPLEMENTATION_ROADMAP_open.md)  
> **Mark ✅ when done · ⬜ when pending.**

| Phase | Scope | Status |
|-------|--------|--------|
| **Phase 1** | Business rules (PrePay / PostPay / manual entry) | ✅ Decided |
| **Phase 2** | `IStockMovementService` + sale hooks | ✅ Done |
| **Phase 3** | Manager manual adjustment UI/API | ✅ Done |
| **Phase 3b** | Low stock alerts, filter, dashboard, storefront | ✅ Done |
| **Phase 4** | Real-time UI (SignalR) — optional | ⬜ Pending |
| **Phase 5** | Mollie config + E2E test | ⬜ Pending (ops) |
| **Phase 6** | Audit `StockAdjust` badge | ✅ Done |
| **Phase C** | Store/admin order visibility | ✅ Done |
| **Phase E** | PO CRUD, transfers, stock reservation | ⬜ Pending |

**NOTES — Phase E (what each item means)**

| Item | Meaning | Today in the app |
|------|---------|------------------|
| **PO CRUD** | Create/edit **purchase orders** to suppliers (`StockOrder` + lines) | Demo seed + read-only in stock overview — no admin CRUD |
| **Transfer** | Move stock **between warehouses** (`StockLocations`) via paired movements | Not implemented (single default location in demo) |
| **GRN** | **Goods received** — register supplier delivery against a PO; stock goes up | Tables exist in model; no receive UI |
| **Reserva** | On checkout, **block** qty in `ReservedQuantity` (available = on hand − reserved) | Grid shows reserved/available; checkout **decrements directly** on pay (no reserve step) |

Phase E is **optional for the current webshop** (catalog → checkout → Mollie → stock down). Needed only if you want full ERP-style inbound stock and reservations.

**Current focus:** _Phase 5 Mollie E2E · Phase 3b SMTP worker · Phase E stock ops_

---

## Current state (what already exists)

| Area | Status |
|------|--------|
| **Mollie** | ✅ Integrated: adapter, webhook, payment-return fallback, audit `PaymentPaid` |
| **Mollie config** | ⬜ Set `Mollie:ApiKey` + public webhook — [PAYMENTS.md](./PAYMENTS.md) |
| **Checkout** | ✅ PostPay + PrePay paths; stock on pay / post-pay checkout |
| **Order visibility** | ✅ Store `/orders`, `/orders/{id}` · Admin payment + Mollie columns + advance payments |
| **Stock** | ✅ Movement service, adjustment UI, low-stock in-app + email queue |
| **Audit `StockAdjust`** | ✅ Badge + logging from `StockMovementService` (sale + manual) |
| **ReservedQuantity** | ✅ Display + seeds in grid · ⬜ auto-reserve on checkout (Phase E) |

---

## Phase 3b — Low stock alerts ✅ (email send pending)

- ✅ In-app `StockLowAlerts` + dashboard
- ✅ Email rows queued in `Emails.EmailMessages` (`EmailQueueId = LowStockAlerts`) — `Notifications:LowStock` in config
- ⬜ **SMTP worker** — send queued low-stock messages (enqueue only today)

---

## Phase 5 — Mollie go-live ⬜

- ⬜ Set `Mollie:ApiKey` (test, then live)
- ⬜ Public webhook URL (ngrok / deploy)
- ⬜ Run E2E checklist in [PAYMENTS.md](./PAYMENTS.md)
- ✅ Document config keys ([PAYMENTS.md](./PAYMENTS.md))

---

## Phase 6 — Audit `StockAdjust` ✅

- ✅ `StockAdjust` in `AuditActions` + orange badge in audit grid
- ✅ One event per sale / manual adjustment from `StockMovementService`
- ✅ Interceptor suppressed for `StockMovement` / `ProductStockLocation` on those writes

---

## Phase C — Order visibility ✅

- ✅ Customer `/orders` (own orders from DB)
- ✅ Customer `/orders/{id}` with payment badge + Mollie id
- ✅ Admin `/admin/orders`: payment status + Mollie id columns
- ✅ Admin edit form: advance payments read-only table
- ✅ Seeds: demo `OrderAdvancePayments`

---

## Phase E — Stock ops (later) ⬜

Deferred until Phase 5 stable. Full breakdown: [IMPLEMENTATION_ROADMAP_open.md § Phase E](./IMPLEMENTATION_ROADMAP_open.md).

- ⬜ **Transfer between locations** (UI + API + movements)
- ⬜ **Purchase order CRUD** (open PO today is read-only + seed)
- ⬜ **Receive delivery (GRN)** — linked to PO
- ⬜ **Reservation on checkout** — increment `ReservedQuantity` (display + seeds done; workflow not wired)

---

## Progress

```
Phase 1  [██████████] decided
Phase 2  [██████████] done (E2E via Phase 5)
Phase 3  [██████████] done
Phase 3b [█████████░] ⬜ SMTP worker
Phase 4  [░░░░░░░░░░] optional SignalR
Phase 5  [███░░░░░░░] ⬜ ApiKey + webhook + E2E
Phase 6  [██████████] done
Phase C  [██████████] done
Phase E  [░░░░░░░░░░] ⬜ PO / transfer / reservation
```
