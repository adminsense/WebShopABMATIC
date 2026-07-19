# Mollie payments — mock-first + go-live runbook

![Status](https://img.shields.io/badge/Default-Mock%20until%20client%20keys-0ea5e9?style=flat-square) ![Scope](https://img.shields.io/badge/Scope-Mollie%20PrePay-512BD4?style=flat-square)

> **Hard rule (owner):** Keep **`Mollie:UseMock = true`** and do **not** switch to real Mollie until the **client delivers API keys**. Agents must not invent keys, commit secrets, or treat real Mollie as a current sprint goal.
> **Purpose of this doc:** (1) document the **approved mock** payment path used now; (2) ops checklist for **B.9** only after keys arrive.
> **Scope:** PrePay checkout (iDEAL / card). PostPay (invoice) does not use Mollie.
> **Storefront UX** (cart layout, freight selector, CTA labels, confirmation screen) is owned by [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) — this file is **provider / ops only**.
> **Mark ✅ when done · ⬜ when pending.**

| Step | Status | Notes |
|------|--------|-------|
| Dev / current runtime: `Mollie:UseMock` | ✅ **Required until client keys** | `MollieMockPaymentAdapter` |
| Client delivers `Mollie:ApiKey` (test, then live) | ⬜ Waiting on client | Do not unblock B.9 without this |
| Public webhook URL + E2E checklist (§3) | ⬜ After keys | |
| Production keys + webhook | ⬜ After keys | Owner configures Azure / Key Vault |

**Related tracker:** [SPEC_IMPLEMENTATION_ROADMAP_open.md](./SPEC_IMPLEMENTATION_ROADMAP_open.md) — **B.9** stays ⬜ and **last** until keys + go-live.

---

## Current rule: mock only

Until the client sends Mollie keys:

- Default config: **`Mollie:UseMock=true`** (mock adapter registered).
- PrePay creates payments with ids `tr_mock_*` and redirects to in-app **`/checkout/mollie-mock`**.
- Mock adapter treats valid mock ids as **paid** when status is queried (payment-return / order summary).
- Do **not** set a real `Mollie:ApiKey` in shared/repo settings; when keys arrive later → User Secrets / Azure App Settings only (never git).

### Provider behaviour (mock)

- `CheckoutUseCase` PrePay path + `MollieMockPaymentAdapter`
- Webhook endpoint exists: `POST /api/webhooks/mollie/payments` (for real Mollie later; mock path does **not** require webhook)
- On successful paid status: audit + `ApplySaleFromOrderAsync`; on cancel/expire/fail: reservation release (see [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §5 and [SPEC_STOCK_OPERATIONS_PROPOSAL.md](./SPEC_STOCK_OPERATIONS_PROPOSAL.md))

### Mock checkout URL

- Redirect to **`/checkout/mollie-mock`** → customer clicks **Pay** → `/orders/{id}/payment-return`
- Settlement happens when order summary **refreshes payment status once** (not continuous polling); mock ids resolve as paid

**Demo card (pre-filled on mock page):** `4111 1111 1111 1111` · Demo Customer · `12/28` · CVC `123`

**Static HTML:** [docs/mocks/mock-payments.html](./mocks/mock-payments.html) remains a visual prototype. The Blazor mock checkout and confirmation now follow that approved visual direction while using dynamic amount/order data and ERP freight (missing price → €0). Runtime details: [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.4.

---

## After client keys (B.9) — do not start until keys exist

### Prerequisites

- Connected to `abmatic_test` (DB-first — **no** EF migrate)
- At least one ERP `PaymentMethods` row that the storefront can select as PrePay online/Mollie (name heuristic or PrePay fallback — storefront may label it **iDEAL / card (Mollie)**; see [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.4)
- Public HTTPS URL reachable by Mollie
- **`Mollie:ApiKey` from client** + set `Mollie:UseMock` to `false` in that environment only

### 1. API key (owner)

```powershell
cd WebShopABMATIC
dotnet user-secrets set "Mollie:ApiKey" "test_xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
```

Prefer User Secrets / Azure App Settings. Never commit keys.

### 2. Webhook URL

Mollie calls `POST /api/webhooks/mollie/payments`.

| Environment | Example |
|-------------|---------|
| ngrok | `https://….ngrok-free.app` |
| Azure | `https://….azurewebsites.net` |

**Handler:** `ProcessMollieWebhookUseCase` → paid → audit `PaymentPaid` → `ApplySaleFromOrderAsync`.

### 3. Manual E2E checklist (B.9)

1. Sign in as store customer.
2. Cart → **Place order & pay** with the selectable Mollie PrePay method.
3. Redirect to **real** Mollie hosted checkout.
4. Pay with Mollie test method.
5. Verify webhook and/or payment-return: paid status, stock sale, admin order column, audit.
6. Repeat for **canceled**, **expired**, and **failed** statuses → reservation release (already coded).

### 4. Payment return fallback

If webhook is delayed, `/orders/{id}/payment-return` (and order summary load) performs a **one-shot status refresh** against Mollie and applies the same paid + stock logic. This is not continuous polling.

### 5. Production

- Live key in Azure / Key Vault only.
- Register production webhook in Mollie dashboard.
- HTTPS only; webhook idempotency already implemented.

---

## Related docs

- [SPEC_IMPLEMENTATION_ROADMAP_open.md](./SPEC_IMPLEMENTATION_ROADMAP_open.md) — B.9 last
- [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) — cart / checkout / confirmation UX
- [SPEC_STOCK_OPERATIONS_PROPOSAL.md](./SPEC_STOCK_OPERATIONS_PROPOSAL.md) — sequence diagrams
- [AGENTS.md](../AGENTS.md) — hard constraints for agents
