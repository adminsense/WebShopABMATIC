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
- Do not invent ERP columns or features in docs that are not in the live database.
- **DB-first:** no EF migrations / schema scripts for `abmatic_test` — adapt the app to the existing ERP schema.

Code without matching doc updates for the areas above = incomplete.
