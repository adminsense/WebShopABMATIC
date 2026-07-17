# Documentation index

Source of truth for **WebShopABMATIC** behaviour and delivery.  
**Agents:** start at [../AGENTS.md](../AGENTS.md) · [../CLAUDE.md](../CLAUDE.md) · [../.claude/CLAUDE.md](../.claude/CLAUDE.md).

> **DB-first (whole project):** live Azure SQL `abmatic_test` is authoritative.  
> **Never invent** columns, tables, EF migrations, or schema scripts for the ERP — **any** feature. Adapt the app to what already exists (`AGENTS.md`, [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) §4).

## Read first (by task)

| Task | Document |
|------|----------|
| Storefront (catalog, cart, checkout, customer account) | [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) |
| Admin panel, staff vs customer auth | [SPEC_ADMIN.md](./SPEC_ADMIN.md) |
| Hexagonal layers, Azure, Blazor, cookie auth ops | [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) |
| Dated runtime / UI amendments | [AMENDMENTS.md](./AMENDMENTS.md) |
| Mollie go-live checklist (provider ops + ✅/⬜) | [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md) — **mock required until client keys**; cart UX → [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) |
| Delivery roadmap (spec + ✅/⬜) | [SPEC_IMPLEMENTATION_ROADMAP_open.md](./SPEC_IMPLEMENTATION_ROADMAP_open.md) |

## Reference (on demand)

| Topic | Document |
|-------|----------|
| Stock / checkout proposal history | [SPEC_STOCK_OPERATIONS_PROPOSAL.md](./SPEC_STOCK_OPERATIONS_PROPOSAL.md) |
| DB summary | [DATA_SUMMARY.md](./DATA_SUMMARY.md) |
| Dutch ↔ English table map | [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md) |
| Freight / delivery fee (ERP products) | [DATA_FREIGHT_DELIVERY.md](./DATA_FREIGHT_DELIVERY.md) |
| Azure Blob product images | [DATA_AZUREBLOB.md](./DATA_AZUREBLOB.md) |
| UI copy-paste patterns | [PATTERNS_UI_QUICK_START.md](./PATTERNS_UI_QUICK_START.md) |
| Code / infra patterns + **naming rules** | [PATTERNS_CODE_AND_INFRASTRUCTURE.md](./PATTERNS_CODE_AND_INFRASTRUCTURE.md) |
| HTML mock walkthrough | [MOCK_PROTOTYPE_GUIDE.md](./MOCK_PROTOTYPE_GUIDE.md) |
| Historical layout-migration dump | [archive/AMENDMENTS_store_layout_migration.md](./archive/AMENDMENTS_store_layout_migration.md) |

## Folders

| Path | Contents |
|------|----------|
| `docs/mocks/` | Static HTML prototypes — **not** live UI unless a SPEC says so. Includes `mock-admin.html`, `mock-payments.html` (conceptual Mollie), `mock-store-filters.html` (+ `mock-store.css` / standalone) |
| `docs/images/` | Screenshots (some illustrative / from mocks) |
| `docs/archive/` | Bulky / closed notes (keep SPECs lean) |
| `.cursor/skills/` | Project Agent Skills (e.g. `docs-governance`) — workflows, not product truth |
| `publish/` | App Service publish profiles (`*.publishsettings` gitignored) — **not** documentation |

## Naming convention

| Pattern | Meaning |
|---------|---------|
| `SPEC_<Topic>.md` | Stable product / architecture specification (current truth) |
| `SPEC_<Topic>_open.md` | Spec **still in development** with internal ✅/⬜ checklist |
| `AMENDMENTS.md` | Short dated runtime changelog (not a feature checklist) |
| `DATA_` / `PATTERNS_` / `MOCK_` | DB maps, coding patterns, mock walkthroughs |

When a `_open` checklist is done: rename to `SPEC_<Topic>.md` (drop `_open`) or move to `archive/`.

When behaviour changes: update the matching `SPEC_*`, then add a one-liner to `AMENDMENTS.md` if needed.
