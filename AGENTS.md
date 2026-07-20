# AGENTS.md — how to work in this repo

This file is the **default contract** for Cursor, Claude Code, and any coding agent.  
Do **not** wait for the user to restate process, docs, or patterns.

## Start every non-trivial task here

1. Open **[`docs/README.md`](docs/README.md)** — pick the right SPEC for the task.
2. Follow **[`.claude/CLAUDE.md`](.claude/CLAUDE.md)** — hard product/auth/Azure constraints.
3. For UI/code style, open **[`docs/PATTERNS_UI_QUICK_START.md`](docs/PATTERNS_UI_QUICK_START.md)** and/or **[`docs/PATTERNS_CODE_AND_INFRASTRUCTURE.md`](docs/PATTERNS_CODE_AND_INFRASTRUCTURE.md)** *before* inventing a new pattern.
4. After behaviour changes: update the matching `docs/SPEC_*` (or checklist on `SPEC_*_open.md`) and add a line to **[`docs/AMENDMENTS.md`](docs/AMENDMENTS.md)** when runtime notes matter. Rule: [`.cursor/rules/docs-sync.mdc`](.cursor/rules/docs-sync.mdc).

## What to read (quick map)

| If the task touches… | Read first |
|----------------------|------------|
| Store / catalog / cart / checkout / customer | `docs/SPEC_WEB_STORE.md` |
| Admin / staff login / roles | `docs/SPEC_ADMIN.md` |
| Hosting, Blazor, Azure, DI, cookie auth ops | `docs/SPEC_INFRASTRUCTURE.md` |
| Payments / Mollie go-live checklist | `docs/SPEC_MOLLIE_PAYMENTS_open.md` |
| Delivery checklist | `docs/SPEC_IMPLEMENTATION_ROADMAP_open.md` |
| DB / Dutch↔English / Blob images | `docs/DATA_*.md` |
| Buttons, grids, forms, Blazor conventions | `docs/PATTERNS_*.md` |

Do **not** load `docs/archive/` unless the user asks for history.

## Doc naming (do not invent new schemes)

| Pattern | Meaning |
|---------|---------|
| `SPEC_<Topic>.md` | Stable product truth |
| `SPEC_<Topic>_open.md` | Spec still in development + ✅/⬜ checklist |
| `DATA_*` | Schema / mapping / storage reference |
| `PATTERNS_*` | How we implement (copy existing patterns) |
| `AMENDMENTS.md` | Short dated runtime changelog |

## Default engineering rules

- **Database first:** The live ERP database (`abmatic_test` on Azure SQL) is the **source of truth**. Physical schema is **mostly Dutch**; C# uses **English** names with DE-PARA in `WebShopABMATICModelBuilder` / entity comments ([`docs/DATA_DUTCH_ENGLISH_MODEL.md`](docs/DATA_DUTCH_ENGLISH_MODEL.md)). ERP labels/product names stay as stored (often Dutch).
  - **Never invent** columns, tables, EF migrations, `Migrate()` / `EnsureCreated()`, or schema scripts for the ERP — in **code or docs**, for **any** feature (store, admin, freight, Mollie, stock, …).
  - Map and encode behaviour onto **existing** Dutch tables/columns. If a real schema change is unavoidable, it is an explicit **DBA/ERP** change on the database — not from this app.
- **Freight:** no hardcoded delivery fee. Resolve from `OrderDeliveryTypeProduct` + `ProductPrices` for the customer’s `DeliveryTypeId`; **€0** when no usable row/price. See [`docs/DATA_FREIGHT_DELIVERY.md`](docs/DATA_FREIGHT_DELIVERY.md).
- **Hexagonal:** UI → Application (use cases/ports) → Infrastructure. No DbContext in Razor pages.
- **Auth:** legacy cookies (`LegacySignInService`), **not** ASP.NET Identity at runtime. Staff ≠ customer identities.
- **ERP:** do not invent columns/tables in code or docs that are not in the database.
- **Store UX:** guests see list price (or Out of stock / Price on request). Guests may **add to cart**; **login/register** is required to **place order & pay** (not to browse or view list price). See `SPEC_WEB_STORE.md` §9.1–9.2.
- **Payments:** **`Mollie:UseMock = true`** until the **client delivers API keys**. Do not switch to real Mollie, invent keys, or prioritize B.9 go-live before that. See `docs/SPEC_MOLLIE_PAYMENTS_open.md`.
- **Azure Blazor:** WebSockets On in App Service; sticky ARR affinity assumed.
- Prefer matching existing components/CSS over new visual systems.

## Done means

Code **and** matching docs updated. Saying “user didn’t ask for docs” is not a reason to skip `docs-sync`.

## Git & publish (owner only)

- **Never** create git commits, amend, push, force-push, or open PRs unless Marco **explicitly** asks in that message.
- **Never** publish/deploy to Azure (Web Deploy, `dotnet publish` for release, Portal deploy) unless Marco **explicitly** asks.
- Staging/local `dotnet build` / `dotnet run` is fine. Preparing files for commit is fine; **running** `git commit` / `git push` / publish is not.
- Default when work is ready: summarize what changed and wait — Marco commits and publishes.

## Pointers

- Product overview (humans): [`README.md`](README.md)  
- Docs index: [`docs/README.md`](docs/README.md)  
- Claude: [`CLAUDE.md`](CLAUDE.md) → [`.claude/CLAUDE.md`](.claude/CLAUDE.md)  
- Path rules: [`.claude/rules/`](.claude/rules/) (`store-ui`, `admin-ui`, `infrastructure`, `docs-sync`, `db-first`, `owner-only-git-publish`)
- Docs workflow skill: [`.cursor/skills/docs-governance/SKILL.md`](.cursor/skills/docs-governance/SKILL.md)
- Format hook (automation only): [`.claude/hooks/format-csharp.ps1`](.claude/hooks/format-csharp.ps1) — not for product/DB/UI policy
