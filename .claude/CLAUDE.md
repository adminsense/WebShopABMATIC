# WebShopABMATIC — Claude Code guide

B2B webshop + admin. **.NET 10** · **Blazor Web App (Interactive Server)** · hexagonal · Azure SQL ERP `abmatic_test` (Dutch schema, English EF names).

| Contract | Path |
|----------|------|
| Process (Cursor + Claude) | [`AGENTS.md`](../AGENTS.md) |
| Specs index | [`docs/README.md`](../docs/README.md) |
| Path-scoped rules | [`.claude/rules/`](rules/) |
| Personal overrides | `CLAUDE.local.md` (gitignored) |

Do **not** duplicate SPECs into `.claude/`.

---

## Stack & layout

| Area | Location |
|------|----------|
| Host / `Program.cs` / Infrastructure / Application | `WebShopABMATIC/` |
| Blazor UI (store + admin) | `WebShopABMATIC.Client/` |
| Solution | `WebShopABMATIC.sln` |
| Docs | `docs/` |
| Publish profiles | `publish/` (not documentation) |

---

## Commands

```powershell
# Restore + build (from repo root) — primary smoke check
dotnet build WebShopABMATIC.sln

# Run host (HTTPS profile; connection string via User Secrets / local appsettings — gitignored)
dotnet run --project WebShopABMATIC/WebShopABMATIC.csproj --launch-profile WebShopABMATIC

# Typical URLs (see Properties/launchSettings.json)
# https://localhost:44357
```

### Testing

Project: **`WebShopABMATIC.Tests`** (xUnit + NSubstitute + FluentAssertions + WebApplicationFactory).

```powershell
dotnet test WebShopABMATIC.sln
```

- **Unit:** Application use cases (store checkout/webhook/profile, admin product/order/stock/dashboard) + `LegacySignInService` (EF InMemory) + `StoreProductDescription`.
- **API integration:** login endpoints, Mollie webhook, stock adjustment auth (no live SQL — ports/DbContext replaced in test host).
- Still useful: manual store guest cart → checkout login; admin `/admin/login` with `StaffUsers`.

Do **not** add `dotnet ef database update` (DB-first). SQL opt-in / bUnit / Playwright = later phases.

### Hooks (automation — not product rules)

After file edits, Claude/Cursor run `.claude/hooks/format-csharp.ps1` (`dotnet format whitespace` on `.cs` only).  
DB-first, Adminsence/mocks, Mollie, store price/stock stay in **AGENTS.md / SPECs / path rules** — hooks do not enforce those.

---

## Always follow

1. Read the right **SPEC** (`docs/README.md`) before coding. Map to the **existing database** only.
2. Sync docs after behaviour changes (`rules/docs-sync.md`, Cursor `docs-sync.mdc`). Use **PATTERNS_*** before inventing UI/code style.
3. **Auth:** cookie `LegacySignInService` — **not** Identity `AspNetUsers`. Staff = `StaffUsers`; customers = `Customers`. Customer history = store `/orders`, never `/admin`.
4. **Azure Blazor:** App Service **Web sockets = On**. Cookie alone authorizes (no in-memory session gate).
5. **Catalog:** guests see **list price** (or Out of stock / Price on request). Guests may add to cart; login/register only to **place order & pay** (`SPEC_WEB_STORE` §9.2).
6. **Mollie:** keep **`Mollie:UseMock`** until the **client sends API keys**. Real Mollie (B.9) is blocked on the client — see `docs/SPEC_MOLLIE_PAYMENTS_open.md`. Do not put keys in git.

### Specs by task

| Task | Read first |
|------|------------|
| Storefront | `docs/SPEC_WEB_STORE.md` |
| Admin / staff | `docs/SPEC_ADMIN.md` §2 |
| Azure / Blazor / DI | `docs/SPEC_INFRASTRUCTURE.md` |
| Runtime notes | `docs/AMENDMENTS.md` |
| Mollie | `docs/SPEC_MOLLIE_PAYMENTS_open.md` |
| Roadmap checklist | `docs/SPEC_IMPLEMENTATION_ROADMAP_open.md` |

Optional: `docs/DATA_*`, `docs/PATTERNS_*`, `docs/MOCK_*`. Avoid `docs/archive/` unless asked.

---

## Hard constraints (“never do”)

- **Git / publish — owner only:** Never `git commit`, amend, push, PR, or Azure/publish deploy unless Marco **explicitly** asks in that chat. Leave changes ready; he commits and publishes.
- **DB-first (global):** `abmatic_test` is authoritative. **Never invent** columns, tables, EF migrations, `Migrate()`, `EnsureCreated()`, or schema scripts for the ERP — any feature. Adapt code/docs to existing columns (encode when documented).
- **No** invented ERP tables/columns in code or docs.
- **No** DbContext in Razor pages — ports / use cases / Infrastructure only.
- Prefer existing store/admin components and CSS over a new visual system.
- After shipping: update matching `SPEC_*` / `SPEC_*_open` checklists; one line in `AMENDMENTS.md` when useful.

---

## Path rules

When editing matching files, Claude also loads:

| Rule | When |
|------|------|
| `rules/db-first.md` | Always — never invent ERP schema |
| `rules/store-ui.md` | Store Blazor / store CSS/JS |
| `rules/admin-ui.md` | Admin Blazor / admin CSS |
| `rules/infrastructure.md` | Host Infrastructure / Program / Application |
| `rules/docs-sync.md` | Always (docs sync) |
| `rules/owner-only-git-publish.md` | Always (no commit/push/publish unless asked) |
