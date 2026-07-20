# Runtime amendments & open UI notes

![Status](https://img.shields.io/badge/Status-Living%20changelog-0ea5e9?style=flat-square)

Short **current-runtime** notes. Historical store-layout migration dump (phases A–D, unified login study, long checklists):

→ [`archive/AMENDMENTS_store_layout_migration.md`](./archive/AMENDMENTS_store_layout_migration.md)

Stable behaviour lives in the SPECs (`SPEC_WEB_STORE.md`, `SPEC_ADMIN.md`, `SPEC_INFRASTRUCTURE.md`). Prefer updating those; use this file only for dated “runtime diverged / interim decision” lines.

---

## Amendments (newest first)

> **2026-07-20 — Auth/cart must not survive logout or browser close:** Removed prerender `PersistentComponentState` auth revival (root cause of “still logged in” on Blazor circuits). Cart is **session storage only** (no localStorage). Sign out clears session cart keys + auth cookie. Store login deletes legacy `.WebShopABMATIC.Auth` cookie on sign-in. See [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.3 / §9.2, [SPEC_ADMIN.md](./SPEC_ADMIN.md) §2.1.

> **2026-07-20 — Checkout DB race + add-to-cart UX + logout cookie:** Fixed `InvalidOperationException` (“connection is closed”) on checkout options by serializing remaining `StoreOrderRepository` calls through `StoreDbGate`, removing `ConfigureAwait(false)`, and retrying closed-connection once. Add to cart **stays on the product/catalog** (badge updates; no empty-cart flash). Store **Sign out** maps `/account/logout` **before** Blazor. See [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §3.1 / §9.2, [SPEC_ADMIN.md](./SPEC_ADMIN.md) §2.1.

> **2026-07-20 — Guest cart → checkout login (rule §9.2):** Guests may add/edit/remove cart lines (session soft hold). Login or register required only to place order & pay; guest lines merge into the customer cart. Closing the browser clears the guest cart (no ERP order / no ERP reservation). ERP `ReservedQuantity` still only on PrePay place-order. See [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.3, §5.2, §9.1–9.2. *(Supersedes the same-day “pending add / buy gate at Add to cart” note below.)*

> **2026-07-20 — Payment-return redirect + cart add reliability:** `OrderPaymentReturn` redirects after first interactive render with `forceLoad` (avoids Blazor `NavigationException` during prerender). `AddProductAsync` returns success/failure and retries browser storage.

> **2026-07-20 — S.7 catalog facet filters (pilot):** Leaf categories in `StoreCatalogFilters:EnabledCategoryIds` (default **54** Handzenders) show Coolblue-style sidebar: Merk (`Manufacturer`), Voorraad, Prijs; `ProductProperty` groups when ERP data exists (placeholder when empty). Not used: `ProductOption`. API: `GetCategoryFacetsAsync` + filtered `GetCatalogAsync`. UI: `StoreFacetSidebar` on `Catalog.razor`. See [PLAN_CATALOG_FILTERS.md](./PLAN_CATALOG_FILTERS.md), [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.1.

> **2026-07-17 — Store screenshots + payment UI aligned:** README now uses the current Categories + Deals storefront screenshot (old Hard drive/$ image removed). Blazor Mollie mock follows the approved hosted-checkout visual; order confirmation now shows the Payment received layout with real order lines, calculated VAT and the persisted ERP freight product/price (`OrderDeliveryTypeProduct` → `ProductPrices`, missing price → €0). Static payment mock no longer contains a fixed €9 freight.

> **2026-07-17 — Docs ownership realigned:** Root `README.md` slimmed (human pitch + screenshots + pointers). `SPEC_MOLLIE_PAYMENTS_open.md` = Mollie provider/ops runbook only; store cart/confirmation UX = `SPEC_WEB_STORE.md` §4.3–4.4. `mock-payments.html` labeled conceptual (not live UI). Project skill: `.cursor/skills/docs-governance/`.

> **2026-07-16 — S.4 server required options:** `CheckoutUseCase.BuildQuoteAsync` loads catalog options and rejects missing/invalid required options (and unknown option ids). Cart quote now sends line `Options` (same as place-order). CTA: “Cannot place order — fix stock or options”.

> **2026-07-16 — E.12 SPEC_WEB_STORE refresh:** Removed Identity leftovers and mock “Hard drive” SKUs; documented live ERP catalog, legacy login, freight, server option validation.

> **2026-07-14 — DB-first = global hard rule:** Never invent columns/tables/migrations/schema scripts for **any** feature. Reinforced in `AGENTS.md`, `SPEC_INFRASTRUCTURE` §4, `docs/README`, `.cursor/rules/db-first.mdc`, `.claude/rules/db-first.md` (not freight-only).

> **2026-07-14 — Freight from ERP (S.5):** Removed hardcoded €9. Fee from `OrderDeliveryTypeProduct` → `ProductPrices` (Dutch `DossierLeveringsTypeProduct` / `ProductPrijzen`); default **€0**. Dutch ERP labels in UI; English code DE-PARA. See [DATA_FREIGHT_DELIVERY.md](./DATA_FREIGHT_DELIVERY.md).

> **2026-07-14 — Mollie mock until client keys:** Hard rule — keep `Mollie:UseMock`; do not start real Mollie (B.9) until the client delivers API keys. Documented in [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md), `AGENTS.md`, roadmap B.9.

> **2026-07-14 — Cart stock blocking UX:** Stale cart lines with insufficient stock stay in the cart but checkout is clearly blocked (danger alert, line Out of stock / “only N left”, disabled CTA “Cannot place order — fix stock”). Server quote/place-order already rejected; UI made blocking obvious. See [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §5.2.

> **2026-07-14 — Format hook:** `.claude/hooks/format-csharp.ps1` runs after agent edits (Claude `PostToolUse` + Cursor `afterFileEdit`) — `dotnet format whitespace` on `.cs` only. Product rules (DB / Adminsence / Mollie) stay in SPECs & path rules, not hooks.

> **2026-07-14 — Store price/stock (AGENTS workflow):** Aligned search + detail + cards on `StorePriceFormatter.FormatListPrice` (guest list price; OOS label wins). CLAUDE.md documents build-as-smoke (no test project yet). Owner still enables Azure WebSockets manually.

> **2026-07-14 — Owner-only git/publish:** Agents never commit, push, or publish/deploy unless Marco explicitly asks. See `AGENTS.md` § Git & publish and `.cursor/rules/owner-only-git-publish.mdc`.

> **2026-07-14 — Claude Code structure:** Root `CLAUDE.md` pointer; enriched `.claude/CLAUDE.md` (commands + constraints); path rules `store-ui` / `admin-ui` / `infrastructure`; `.claude/settings.json` (deny editing appsettings/publish); `CLAUDE.local.md.example` + gitignore; `.claudeignore`.

> **2026-07-14 — DB-first rule:** Live `abmatic_test` is the source of truth. Removed migration/script workflows from SPECs/`DATA_*`. Agents must not use EF `Migrate` / `dotnet ef database update` / schema scripts for the ERP DB. See `AGENTS.md` and [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) §4.

> **2026-07-14 — Agent workflow:** Root [`AGENTS.md`](../AGENTS.md) is the default process contract (which SPEC/PATTERNS to read, docs sync). Cursor rule `agents.mdc` always applies.

> **2026-07-14 — Docs naming:** Living specs with checklists use `SPEC_<Topic>_open.md`. Runtime changelog is `AMENDMENTS.md` (no longer `open_*` prefix). Index: [README.md](./README.md).

> **2026-07-14 — Docs layout:** Renamed `readme/` to `docs/`; HTML mocks to `docs/mocks/`; bulky migration notes to `docs/archive/`; PublishSettings to `publish/`. Index: [README.md](./README.md).

> **2026-07-14 — Catalog price/stock UI:** Guests see **list price** (or **Out of stock** / **Price on request**). Removed “Meld u aan om uw prijs te zien” from cards. Login only when buying (add to cart). See [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §5.1 / §9.1.

> **2026-07-14 — Azure Blazor + store auth:** Removed in-memory `StoreBrowserSessionStore` (cookie alone authorizes customers). Interactive Server **prerender on**. Azure App Service must set **Web sockets = On** (Immo production already has this; see [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md)). Idle logout remains client-side 15 min → `/account/logout`.

> **2026-07 — My orders / Admin nav:** Spec visual notes that removed My orders were for matching Adminsence chrome only. Product need + [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.5 require discoverable **My orders**. Current `StoreHeader`: guest → Login + Admin (`/admin/login`); customer → **My orders** + account; staff → **Admin** (`/admin`). Auth tables remain split (Customer vs StaffUsers) — see [SPEC_ADMIN.md](./SPEC_ADMIN.md) §2. Unified login still open (see archive).

---

## Still open (pointers only)

| Topic | Where |
|-------|--------|
| Unified store login → admin redirect | Archive §2.2.2 |
| Store layout polish vs reference | Archive phases C–F |
| Mollie real key + webhook E2E | **Blocked on client keys** — mock required — [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md) |
| Implementation tracker | [SPEC_IMPLEMENTATION_ROADMAP_open.md](./SPEC_IMPLEMENTATION_ROADMAP_open.md) |

---

## Docs layout (2026-07-14)

| Path | Role |
|------|------|
| `docs/` | Product SPECs (`SPEC_*` / `SPEC_*_open`) + `AMENDMENTS.md` |
| `docs/mocks/` | Static HTML prototypes |
| `docs/images/` | Screenshots |
| `docs/archive/` | Closed / bulky historical notes |
| `publish/` | Local PublishSettings (gitignored `*.publishsettings`) |
