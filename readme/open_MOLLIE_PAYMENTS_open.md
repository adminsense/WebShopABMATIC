# Mollie payments — local E2E setup

![Status](https://img.shields.io/badge/Phase%205-⬜%20Pending-64748b?style=flat-square) ![Scope](https://img.shields.io/badge/Scope-Mollie%20PrePay-512BD4?style=flat-square)

> **Purpose:** Ops runbook for Mollie **prod go-live** (API key, webhook, E2E). Dev checkout is ✅ via `MollieMockPaymentAdapter`. **Open items** until **B.9** prod is ✅ in [open_IMPLEMENTATION_ROADMAP.md](./open_IMPLEMENTATION_ROADMAP.md) — **last**, after dev 100%.  
> **Scope:** PrePay checkout (iDEAL / card via Mollie test mode). PostPay (invoice) does not use Mollie.  
> **Mark ✅ when done · ⬜ when pending.**

| Step | Status |
|------|--------|
| Dev mock (`Mollie:UseMock`) for local flow | ✅ Dev done |
| `Mollie:ApiKey` (test, then live) | ⬜ Pending |
| Public webhook URL + E2E checklist (§3) | ⬜ Pending |
| Production keys + webhook | ⬜ Pending |

---

## Dev mock (no API key)

### Checkout + Mollie (dev)

- Removed the block that prevented PrePay; Mollie flow restored in `CheckoutUseCase`
- `Mollie:UseMock=true` (default) → `MollieMockPaymentAdapter` — simulates payment without an API key
- Webhook registered: `POST /api/webhooks/mollie/payments`
- **D.7** — reserves stock (`ReservedQuantity`) when a PrePay order is created; decrements stock after payment

### UI `/cart`

- Guest sees line items; checkout requires **Sign in**
- Authenticated customer: delivery address + payment method (default **Mollie / iDEAL**)
- Button **Place order & pay**

When `Mollie:ApiKey` is empty and `Mollie:UseMock` is `true` (default in `appsettings.json`), the app registers `MollieMockPaymentAdapter`:

- PrePay checkout creates a `tr_mock_*` payment id and redirects straight to `/orders/{id}/payment-return` (no Mollie hosted page).
- Payment-return / webhook logic treats mock ids as **paid** so stock and audit run locally.

This completes **dev** checkout. **B.9 prod** remains ⬜ until you configure a real test key and run the checklist below — **after dev 100%**.

To switch to real Mollie: set `Mollie:ApiKey`, set `Mollie:UseMock` to `false`, expose a public webhook URL.

**UI prototype:** [docs/mock-payments.html](docs/mock-payments.html) — Mollie card checkout + confirmation after pay.

---

## Prerequisites

- SQL schema + seeds applied (`Sql/seeds.sql` on `abmatic_test`)
- `PaymentMethods` row **iDEAL / card (Mollie)** with `IsPrePay = 1` (in `seeds.sql`)
- Public HTTPS URL reachable by Mollie (ngrok, Cloudflare Tunnel, or deployed dev slot)

## 1. Mollie test API key

1. Create a [Mollie](https://www.mollie.com/) account and open **Developers → API keys**.
2. Copy the **Test** API key (`test_…`).
3. Store it locally (do not commit):

```powershell
cd Web
dotnet user-secrets set "Mollie:ApiKey" "test_xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
```

Or set in `appsettings.Development.json` (gitignored in your environment only — prefer user secrets).

## 2. Webhook URL

Mollie calls `POST /api/webhooks/mollie/payments` with the payment id.

| Environment | Webhook base URL example |
|-------------|--------------------------|
| ngrok | `https://abc123.ngrok-free.app` |
| Azure App Service | `https://your-app.azurewebsites.net` |

Checkout passes `WebhookBaseUrl` from the cart page host. For local dev:

```powershell
ngrok http https://localhost:7031
```

Use the ngrok **https** URL as the site you open in the browser when testing checkout (same origin for redirect + webhook path).

**Handler:** `ProcessMollieWebhookUseCase` → marks `OrderAdvancePayment` paid → audit `PaymentPaid` → `ApplySaleFromOrderAsync`.

## 3. Manual E2E checklist (B.9)

1. Sign in as `customer@webshop.com` / `demo` (after demo seed) or a real `WebshopLogin` from Azure.
2. Add products to cart → **Place order** with **iDEAL / card (Mollie)**.
3. Confirm redirect to Mollie hosted checkout.
4. Complete payment with Mollie test method (e.g. paid status in test dashboard).
5. Verify:
   - Webhook received (app log: payment processed).
   - `/orders/{id}` shows **Paid**.
   - `Projects.OrderAdvancePayments.MolliePaidAt` set.
   - `AuditLogs` contains `PaymentPaid`.
   - Stock decremented (`Products.StockMovements` with `OrderLineId`).
6. `/admin/orders` → payment column **paid**, Mollie id populated.
7. `/admin/audit-logs` → `PaymentPaid` + `StockAdjust` (sale).

## 4. Payment return fallback

If the webhook is delayed, `/orders/{id}/payment-return` and `/orders/{id}` poll Mollie via `GetOrderSummaryAsync` and apply the same paid + stock logic.

## 5. Production

- Replace `Mollie:ApiKey` with **Live** key in secure configuration (Azure App Settings / Key Vault).
- Register production webhook URL in Mollie dashboard.
- Use HTTPS only; validate webhook idempotency (already implemented).

## Related docs

- [open_IMPLEMENTATION_ROADMAP.md](./open_IMPLEMENTATION_ROADMAP.md) — Phase B.9
- [open_IMPLEMENTATION_ROADMAP.md](./open_IMPLEMENTATION_ROADMAP.md) — Phase B.9 (last priority)
- [SPEC_STOCK_OPERATIONS_PROPOSAL.md](./SPEC_STOCK_OPERATIONS_PROPOSAL.md) — checkout sequence diagrams
