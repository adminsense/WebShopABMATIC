---
paths:
  - "WebShopABMATIC/Infrastructure/**"
  - "WebShopABMATIC/Application/**"
  - "WebShopABMATIC/Program.cs"
  - "WebShopABMATIC/Endpoints/**"
  - "WebShopABMATIC/Persistence/**"
  - "WebShopABMATIC/Model/**"
  - "WebShopABMATIC.Client/Components/Account/**"
---

# Infrastructure / Application rules

- Spec: `docs/SPEC_INFRASTRUCTURE.md` §4 (DB-first **global**). Mapping: `docs/DATA_DUTCH_ENGLISH_MODEL.md`.
- **Database first (whole solution):** live `abmatic_test` is authoritative. **Never invent** columns, tables, EF migrations, `Migrate()`, `EnsureCreated()`, or schema scripts for the ERP — store, admin, freight, Mollie, stock, etc.
- Map English entities to **existing** Dutch names via `WebShopABMATICModelBuilder`. Do not invent columns/tables in code or docs.
- Hexagonal: UI → ports/use cases → Infrastructure adapters. Register in `DependencyInjection` / `Program.cs`.
- Auth: `LegacySignInService` + cookie — not ASP.NET Identity at runtime.
- Payments: **`Mollie:UseMock` until client API keys** — mock is the approved path; do not enable real Mollie or invent keys. Runbook: `docs/SPEC_MOLLIE_PAYMENTS_open.md`. Encode PSP data in existing ERP fields only.
- After behaviour changes: update the matching SPEC/`DATA_*` and `AMENDMENTS.md` when needed.
