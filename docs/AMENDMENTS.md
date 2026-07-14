# Runtime amendments & open UI notes

![Status](https://img.shields.io/badge/Status-Living%20changelog-0ea5e9?style=flat-square)

Short **current-runtime** notes. Historical store-layout migration dump (phases A–D, unified login study, long checklists):

→ [`archive/AMENDMENTS_store_layout_migration.md`](./archive/AMENDMENTS_store_layout_migration.md)

Stable behaviour lives in the SPECs (`SPEC_WEB_STORE.md`, `SPEC_ADMIN.md`, `SPEC_INFRASTRUCTURE.md`). Prefer updating those; use this file only for dated “runtime diverged / interim decision” lines.

---

## Amendments (newest first)

> **2026-07-14 — DB-first = global hard rule:** Never invent columns/tables/migrations/schema scripts for **any** feature. Reinforced in `AGENTS.md`, `SPEC_INFRASTRUCTURE` §4, `docs/README`, `.cursor/rules/db-first.mdc`, `.claude/rules/db-first.md` (not frete-only).

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
