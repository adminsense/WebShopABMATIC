# Mollie payments — local E2E setup

> **Scope:** PrePay checkout (iDEAL / card via Mollie test mode). PostPay (invoice) does not use Mollie.

## Prerequisites

- SQL schema + seeds applied (`scripts/apply-local-database.ps1`)
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

1. Sign in as `customer@webshop.com` / `Customer@12345`.
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

- [IMPLEMENTATION_ROADMAP_open.md](./IMPLEMENTATION_ROADMAP_open.md) — Phase B.9
- [SUNDAY_open.md](./SUNDAY_open.md) — Phase 5
- [SPEC_STOCK_OPERATIONS_PROPOSAL.md](./SPEC_STOCK_OPERATIONS_PROPOSAL.md) — checkout sequence diagrams
