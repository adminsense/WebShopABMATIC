# 📝 Runtime Amendments & Open UI Notes

![Status](https://img.shields.io/badge/Status-Living%20Changelog-0ea5e9?style=flat-square) ![Updates](https://img.shields.io/badge/Updates-Continuous-22c55e?style=flat-square) ![Type](https://img.shields.io/badge/Type-Runtime%20Notes-512BD4?style=flat-square)

**Short current-runtime notes and dated changelog**

---

> [!NOTE]
> **Historical Archive:** Store layout migration dump (phases A–D, unified login study, long checklists) moved to:  
> → [`archive/AMENDMENTS_store_layout_migration.md`](./archive/AMENDMENTS_store_layout_migration.md)

Stable behaviour lives in the SPECs (`SPEC_WEB_STORE.md`, `SPEC_ADMIN.md`, `SPEC_INFRASTRUCTURE.md`). Prefer updating those; use this file as a dated summary. Each calendar day has **one table** in plain GitHub Markdown (no HTML): the date is the single left-aligned column header (`|:---|`) and each completed change is a numbered row. Update today's existing table instead of creating another table for the same date.

---

## 📅 Amendments (newest first)

| 2026-07-30 |
|:---|
| 1. Manual functional verification — Webstore: public catalog access, category navigation, product search and product-detail opening. |
| 2. Manual functional verification — Webstore: list-price/stock presentation and add-to-cart flow for guest and authenticated customer. |
| 3. Manual functional verification — Customer login: sign-in, authenticated redirect, session-cookie persistence and sign-out. |
| 4. Manual functional verification — Staff login: admin sign-in, role-protected redirect to `/admin` and sign-out. |
| 5. Manual functional verification — Admin product attributes: Attributes action opens the modal over the calling grid and Close/X returns without losing grid context. |
| 6. Manual functional verification — Admin product attributes: add, inline edit and delete-confirmation flows for product attribute values. |

| 2026-07-29 |
|:---|
| 1. Admin CRUD and workflow forms now use the full grid width; login cards and modals remain intentionally narrow. |
| 2. Product attribute values moved to reusable `ProductAttributeValuesModal`, opened over `/admin/products` and `/admin/product-attributes`; closing preserves the calling grid. |
| 3. Attribute deep links open the modal only after the product-selection grid finishes loading, preventing overlapping queries on the scoped `DbContext`. |
| 4. The content-modal interaction is documented in [PATTERNS_UI_QUICK_START.md](./PATTERNS_UI_QUICK_START.md) §6.1 and enforced by `.cursor/rules/ui-patterns-first.mdc`. |
| 5. Admin dashboard EF metrics now run sequentially instead of using `Task.WhenAll` on one scoped `DbContext`; ANCM stdout logging is disabled by default and accumulated local logs were removed. |

| 2026-07-28 |
|:---|
| 1. Remapped `[Products].[ProductAttribuut]` to C# `Name`/`DataType`/`Unit` (`Naam`/`Gegevenstype`/`Eenheid`). |
| 2. Mapped `[Products].[ProductAttribuutItem]` using FK `ProductProdId` → `Product.ProdId` and value column `Waarde`. |
| 3. Added the DBA SQL and idempotent seed for 18 attributes: [`scripts/ProductAttribuut_create_and_seed.sql`](../scripts/ProductAttribuut_create_and_seed.sql). |
| 4. Implemented product attribute assignment based on the approved [`tela_atributos.png`](./images/tela_atributos.png). |
| 5. Expanded the Product admin form with `DescriptionNl`/`En`/`Fr` and `WebshopDescriptionNl`; aligned the form width and action-button colours. |
| 6. Adjusted staff-user groups/password behaviour and admin logout enhanced navigation. |

| 2026-07-27 |
|:---|
| 1. Documented staff-user password, group and flag behaviour in `SPEC_ADMIN` §3.7. |
| 2. Removed My profile and the obsolete plan file. |
| 3. Added type-to-search to `AdminGridSearch` and moved `StoreStaffRedirect` to after-render navigation. |
| 4. Authentication cookies are always session cookies; Remember me was removed. |

| 2026-07-26 |
|:---|
| 1. Implemented `ProductAttribuut` + `ProductAttribuutItem` mapping and the DBA creation/seed script. |
| 2. Removed the S.7 pilot based on `StoreCatalogFilterOptions`, Merk/Voorraad/Prijs and `ProductProperty`. |
| 3. Added attribute dictionary/assignment admin flows and store leaf facets from exact `Waarde` values. |
| 4. DBA must apply the SQL to `abmatic_test` before runtime; see [PLAN_CATALOG_FILTERS.md](./PLAN_CATALOG_FILTERS.md). |

| 2026-07-23 |
|:---|
| 1. Reset the catalog-filter specification: target Dutch tables are `ProductAttribuut` + `ProductAttribuutItem`, with admin values per product and store facets from distinct `Waarde`. |
| 2. Marked whitelist 54, Merk/Voorraad/Prijs, `ProductProperty` facets and the Coolblue analysis obsolete; see [PLAN_CATALOG_FILTERS.md](./PLAN_CATALOG_FILTERS.md). |

| 2026-07-21 |
|:---|
| 1. Planned the advanced catalog-filter development documented in [PLAN_CATALOG_FILTERS.md](./PLAN_CATALOG_FILTERS.md). |
| 2. Product descriptions now resolve `WebshopDescriptionNl` → `DescriptionNl` → `DescriptionEn` → `DescriptionFr`, with **No description** fallback. |
| 3. Reverted the facet login gate: guests may browse, filter and add to cart; login remains required for ordering, payment and account features. |
| 4. Order confirmation now uses `StoreLayout`; the obsolete `StorePaymentLayout` was removed. |

| 2026-07-20 |
|:---|
| 1. Removed prerender authentication revival through `PersistentComponentState`. |
| 2. Cart storage is session-only and sign-out clears cart state plus the authentication cookie. |
| 3. Store login deletes the legacy `.WebShopABMATIC.Auth` cookie before sign-in. |

| 2026-07-19 |
|:---|
| 1. Serialized remaining checkout repository calls through `StoreDbGate` and added one retry for a closed connection. |
| 2. Add to cart remains on the product/catalog page and updates the badge without an empty-cart flash. |
| 3. Store sign-out maps `/account/logout` before Blazor. |

| 2026-07-18 |
|:---|
| 1. Guests may add, edit and remove session cart lines; login or registration is required only to place an order and pay. |
| 2. Guest lines merge into the customer cart after login, and closing the browser clears the guest cart. |
| 3. ERP `ReservedQuantity` remains applied only when placing a PrePay order. |

| 2026-07-17 |
|:---|
| 1. `OrderPaymentReturn` redirects after the first interactive render with `forceLoad`, avoiding prerender `NavigationException`. |
| 2. `AddProductAsync` reports success/failure and retries browser storage. |

| 2026-07-16 |
|:---|
| 1. Added the S.7 pilot sidebar for whitelisted leaf categories using Merk, Voorraad and Prijs facets. |
| 2. Added `GetCategoryFacetsAsync`, filtered catalog loading and `StoreFacetSidebar`; `ProductOption` was not used. |
| 3. This pilot was later superseded by the `ProductAttribuut` model documented in [PLAN_CATALOG_FILTERS.md](./PLAN_CATALOG_FILTERS.md). |

| 2026-07-15 |
|:---|
| 1. Updated README storefront screenshots to the current Categories + Deals interface. |
| 2. Aligned the Blazor Mollie mock and order confirmation with real order lines, calculated VAT and ERP freight price. |
| 3. Removed the fixed €9 freight value from the static payment mock. |

| 2026-07-14 |
|:---|
| 1. Slimmed the root README to product presentation, screenshots and documentation pointers. |
| 2. Assigned Mollie provider operations to `SPEC_MOLLIE_PAYMENTS_open.md` and store cart/confirmation UX to `SPEC_WEB_STORE.md`. |
| 3. Marked `mock-payments.html` conceptual and added the project docs-governance skill. |

| 2026-07-13 |
|:---|
| 1. `CheckoutUseCase.BuildQuoteAsync` now rejects missing, invalid and unknown required options. |
| 2. Cart quotes send line `Options`, matching place-order behaviour. |
| 3. Blocking CTA is “Cannot place order — fix stock or options”. |

| 2026-07-12 |
|:---|
| 1. Removed Identity leftovers and obsolete mock “Hard drive” SKUs from `SPEC_WEB_STORE`. |
| 2. Documented the live ERP catalog, legacy login, freight and server-side option validation. |

| 2026-07-11 |
|:---|
| 1. Established DB-first as a global rule: never invent ERP columns, tables, migrations or schema scripts. |
| 2. Reinforced the rule in `AGENTS.md`, `SPEC_INFRASTRUCTURE` §4, `docs/README` and the Cursor/Claude DB-first rules. |

| 2026-07-10 |
|:---|
| 1. Removed the hardcoded €9 freight fee. |
| 2. Freight now resolves through `OrderDeliveryTypeProduct` → `ProductPrices`, defaulting to €0 without a usable ERP price. |
| 3. Kept Dutch ERP labels in the UI and English C# names with DE-PARA; see [DATA_FREIGHT_DELIVERY.md](./DATA_FREIGHT_DELIVERY.md). |

| 2026-07-09 |
|:---|
| 1. Established `Mollie:UseMock=true` until the client supplies API keys. |
| 2. Documented the go-live block in [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md), `AGENTS.md` and roadmap B.9. |

| 2026-07-08 |
|:---|
| 1. Stale cart lines with insufficient stock remain visible but clearly block checkout. |
| 2. Added danger messaging, per-line stock status and disabled CTA “Cannot place order — fix stock”. |

| 2026-07-07 |
|:---|
| 1. Added `.claude/hooks/format-csharp.ps1` after-file-edit formatting for C# whitespace. |
| 2. Kept product rules in SPECs and path rules rather than automation hooks. |

| 2026-07-06 |
|:---|
| 1. Aligned search, product detail and cards on `StorePriceFormatter.FormatListPrice`; out-of-stock status takes precedence. |
| 2. Azure WebSockets remains an owner-managed App Service setting. |

| 2026-07-05 |
|:---|
| 1. Established owner-only git: agents do not commit, amend, push or force-push without explicit authorization. |
| 2. Established owner-only publish/deploy; see `AGENTS.md` and `.cursor/rules/owner-only-git-publish.mdc`. |

| 2026-07-04 |
|:---|
| 1. Added the root `CLAUDE.md` pointer and expanded `.claude/CLAUDE.md`. |
| 2. Added path rules for store UI, admin UI and infrastructure. |
| 3. Added Claude settings, local configuration example, gitignore and `.claudeignore`. |

| 2026-07-03 |
|:---|
| 1. Declared live `abmatic_test` the database source of truth. |
| 2. Removed migration and schema-script workflows from SPECs and `DATA_*`. |
| 3. Prohibited application-driven `Migrate`, `dotnet ef database update` and ERP schema scripts. |

| 2026-07-02 |
|:---|
| 1. Established root [`AGENTS.md`](../AGENTS.md) as the default agent process contract. |
| 2. Added the always-applied Cursor `agents.mdc` rule. |

| 2026-07-01 |
|:---|
| 1. Living specs with checklists use `SPEC_<Topic>_open.md`. |
| 2. Runtime changelog renamed to `AMENDMENTS.md`, without an `open_*` prefix. |
| 3. Documentation index remains [README.md](./README.md). |

| 2026-06-30 |
|:---|
| 1. Renamed `readme/` to `docs/`. |
| 2. Moved HTML mocks to `docs/mocks/` and bulky migration notes to `docs/archive/`. |
| 3. Moved PublishSettings to `publish/`. |

| 2026-06-29 |
|:---|
| 1. Guests see list price, **Out of stock** or **Price on request**. |
| 2. Removed “Meld u aan om uw prijs te zien” from product cards. |
| 3. Login is required when buying, not for browsing or seeing list prices. |

| 2026-06-28 |
|:---|
| 1. Removed in-memory `StoreBrowserSessionStore`; the customer authentication cookie is authoritative. |
| 2. Interactive Server uses prerender and Azure App Service requires WebSockets enabled. |
| 3. Idle logout remains client-side after 15 minutes through `/account/logout`. |

| 2026-06-27 |
|:---|
| 1. Restored discoverable **My orders** according to [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.5. |
| 2. Header actions differ for guests, customers and staff; staff enter through `/admin`. |
| 3. Customer and staff authentication tables remain separate; see [SPEC_ADMIN.md](./SPEC_ADMIN.md) §2. |
| 4. Unified login remains open in the historical archive. |

| 2026-06-26 |
|:---|
| 1. Kept `AMENDMENTS.md` as the dated runtime changelog. |
| 2. Stable behaviour remains in the matching SPECs. |
| 3. Older bulky notes remain under `docs/archive/`. |

| 2026-06-25 |
|:---|
| 1. Kept live runtime notes in `docs/AMENDMENTS.md`. |
| 2. Kept historical bulk notes in `docs/archive/`. |
| 3. Kept publish settings under `publish/`. |

---

## ⏳ Still Open (pointers only)

| Topic | Where |
|-------|--------|
| Unified store login → admin redirect | Archive §2.2.2 |
| Store layout polish vs reference | Archive phases C–F |
| Mollie real key + webhook E2E | **Blocked on client keys** — mock required — [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md) |
| Implementation tracker | [SPEC_IMPLEMENTATION_ROADMAP_open.md](./SPEC_IMPLEMENTATION_ROADMAP_open.md) |

---

## Docs layout (2026-07-01)

| Path | Role |
|------|------|
| `docs/` | Product SPECs (`SPEC_*` / `SPEC_*_open`) + `AMENDMENTS.md` |
| `docs/mocks/` | Static HTML prototypes |
| `docs/images/` | Screenshots |
| `docs/archive/` | Closed / bulky historical notes |
| `publish/` | Local PublishSettings (gitignored `*.publishsettings`) |
