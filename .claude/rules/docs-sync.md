# Docs are part of the delivery

Do **not** wait for the user to ask. After behavioural or architectural code changes, sync the markdown in `docs/`.

Mirror of `.cursor/rules/docs-sync.mdc`. Index: `docs/README.md`.

Naming: `SPEC_<Topic>.md` (stable) · `SPEC_<Topic>_open.md` (checklist in progress) · `AMENDMENTS.md` (runtime changelog).

## Before implementing

1. Identify which specs apply (`SPEC_WEB_STORE.md`, `SPEC_ADMIN.md`, `SPEC_INFRASTRUCTURE.md`, `SPEC_*_open.md`, etc.).
2. Skim the relevant sections so the change matches documented intent (or update the docs if intent changed).

## After implementing

| Change | Docs to touch |
|--------|----------------|
| Store auth / cookie / session / login routes | `SPEC_WEB_STORE.md`, `SPEC_ADMIN.md`, `SPEC_INFRASTRUCTURE.md`, `AMENDMENTS.md` |
| Admin auth / roles / staff vs customer | `SPEC_ADMIN.md`, `SPEC_INFRASTRUCTURE.md` |
| Azure / Blazor / WebSockets / deploy | `SPEC_INFRASTRUCTURE.md` |
| Checkout / Mollie / stock | `SPEC_WEB_STORE.md`, `SPEC_MOLLIE_PAYMENTS_open.md`, `SPEC_IMPLEMENTATION_ROADMAP_open.md` |
| UI / nav / store chrome decisions | `AMENDMENTS.md`, `SPEC_WEB_STORE.md` |

## How to update

- Change **current-state** tables/sections; mark obsolete claims clearly.
- Prefer short factual amendments; add a dated line to `AMENDMENTS.md` when needed; archive bulky history under `docs/archive/`.
- **DB-first (global):** never invent ERP columns, tables, migrations, or schema scripts in docs or imply they are planned app work — any feature. Adapt to the live schema (`AGENTS.md`, `SPEC_INFRASTRUCTURE` §4).
- Do not document features/columns that are not in `abmatic_test`.
- Freight/Mollie/etc. examples of mapping onto **existing** tables belong in `DATA_*`; they do not invent schema.

Code without matching doc updates for the areas above = incomplete.
