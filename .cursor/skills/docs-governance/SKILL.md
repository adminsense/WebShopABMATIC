---
name: docs-governance
description: >-
  Audit and realign WebShopABMATIC documentation ownership (README, SPEC_*,
  AMENDMENTS, PATTERNS_*, DATA_*, mocks). Use when the user asks to reorganize
  docs, fix duplicated or outdated documentation, slim README, separate Mollie
  ops from store UX, label mocks vs live UI, or sync docs after behaviour changes.
---

# Docs governance (WebShopABMATIC)

Workflow for keeping docs clear. **Not** a second product spec — point to the canonical owner instead of copying truth into this skill.

## Before you start

1. Read [AGENTS.md](../../../AGENTS.md) and [docs/README.md](../../../docs/README.md).
2. Obey always-on rules: DB-first, owner-only git/publish, Mollie mock until client keys (do **not** restate them here as optional).

## Ownership map

| Content | Canonical owner |
|---------|-----------------|
| Human pitch, live URL, screenshots | Root `README.md` (slim) |
| Agent process / hard constraints | `AGENTS.md`, `.claude/CLAUDE.md`, `.cursor/rules/*` |
| Docs index | `docs/README.md` |
| Store layout, cart, checkout UX, confirmation | `docs/SPEC_WEB_STORE.md` |
| Admin / staff auth & roles | `docs/SPEC_ADMIN.md` |
| Hosting, DI, Blazor ops | `docs/SPEC_INFRASTRUCTURE.md` |
| Mollie adapter, webhook, keys, B.9 checklist | `docs/SPEC_MOLLIE_PAYMENTS_open.md` |
| Delivery checklist status | `docs/SPEC_IMPLEMENTATION_ROADMAP_open.md` |
| Schema / Dutch↔English / freight / Blob | `docs/DATA_*.md` |
| How to implement UI/code | `docs/PATTERNS_*.md` |
| Dated runtime notes | `docs/AMENDMENTS.md` |
| Static HTML | `docs/mocks/` — label **conceptual / not live UI** unless a SPEC says otherwise |
| History | `docs/archive/` — no current authority |

## Audit steps

1. List claims in the file under review (auth, layout, payment flow, schema, status badges).
2. Classify each claim with the table above.
3. For **behaviour** claims: verify against live Blazor / Application / Infrastructure code before calling them current truth.
4. For **schema** claims: verify against live DB / `DATA_*` — never invent columns or migrations.
5. Replace duplicates with a **link** to the canonical owner; do not keep two full copies.
6. Mark obsolete mock/screenshot material as illustrative, or move bulky history to `docs/archive/`.
7. Update `docs/README.md` if the index/ownership map changed; add a one-liner to `AMENDMENTS.md` when runtime or doc ownership changed.

## README rules

Keep root `README.md` as:

- Short product pitch + live URL + stack badges (no “Production Ready” while Mollie is mock-only).
- Store / Admin / Payments with brief bullets and optional screenshots (caption mocks as illustrative).
- Pointers: humans → `docs/README.md`; agents → `AGENTS.md`.

Do **not** put in README: RBAC matrices, Identity-as-runtime, CQRS/circuit-breaker lists, full SPEC catalog, agent hard rules.

## Mollie vs storefront

- **SPEC_MOLLIE** = provider ops (`UseMock`, adapter, webhook, secrets, B.9 E2E).
- **SPEC_WEB_STORE** = `/cart`, freight, selectable payment methods, CTA, route sequence, confirmation UI.
- Static `mock-payments.html` ≠ Blazor confirmation / freight / SKUs.

## Done output

Summarize for the user:

- **Kept** / **Moved** / **Archived** / **Removed**
- Broken links fixed
- Whether `AMENDMENTS.md` / `docs/README.md` were updated

Do **not** commit or publish unless Marco explicitly asks.
