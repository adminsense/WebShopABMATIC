# Catalog filters — ProductAttribuut (client model)

![Status](https://img.shields.io/badge/Status-Spec%20agreed%20docs%20only-0ea5e9?style=flat-square)
![Scope](https://img.shields.io/badge/Scope-Store%20+%20Admin-512BD4?style=flat-square)

**Canonical working spec for catalog attribute filters.**  
Runtime code for the old S.7 pilot still exists until implementation; **this document is the target truth** — implement against it, then delete the pilot.

---

## 1. Client requirements (mandatory)

| Rule | Detail |
|------|--------|
| Attributes on **products only** | Not on categories |
| Admin maintains dictionary + values | Staff create/seed attributes; per product choose attribute and fill value; client can update later |
| Store filters | User opens a **leaf** category → system loads attributes present on products in that category → shows **distinct** values as **checkboxes** → check/uncheck filters the grid |
| Applies to all products/categories | No Handzenders-only whitelist; any leaf with product attribute data shows filters |
| Layout | **Unchanged** — keep current `Catalog.razor` + facet checkbox sidebar chrome |

**Do not use** for these filters: `ProductOption` (checkout), legacy `ProductProperty` / `ProductPropertieItem` (old ERP property sheet — not this feature).

**Replaces** the S.7 pilot entirely (Merk / Voorraad / Prijs / `ProductProperty` facets / `StoreCatalogFilters` whitelist). That pilot is obsolete for product intent; code removal is a later implementation step.

---

## 2. Seed attribute list (18)

Create these rows in `[Products].[ProductAttribuut]` (dictionary only; no product values until staff fill):

1. Power Supply  
2. Application Type  
3. Gate Type  
4. Maximum Gate Weight  
5. Maximum Gate Length  
6. Duty Cycle  
7. Motor Type  
8. Control Technology  
9. Access Control Method  
10. Communication Protocol  
11. Safety Features  
12. IP Protection Rating  
13. Frequency  
14. Battery Backup  
15. Smart Home Compatibility  
16. Installation Type  
17. Environment  
18. Certifications  

Store UI labels: prefer `NaamNl` when present; otherwise `NaamEn` (seed may copy EN into NL/FR until translations exist).

---

## 3. Schema (Dutch SQL → English C#)

**Explicit exception to “never invent ERP tables”:** client asked for new tables for this feature. Delivery = **SQL script** applied by Marco/DBA on `abmatic_test`, then EF mapping only. **No** `Migrate()` / `EnsureCreated()`.

### `[Products].[ProductAttribuut]` → `ProductAttribute`

| SQL (Dutch) | C# |
|-------------|-----|
| `Id` | `Id` |
| `NaamEn` | `NameEn` |
| `NaamNl` | `NameNl` |
| `NaamFr` | `NameFr` |
| `Volgorde` | `SortOrder` |

### `[Products].[ProductAttribuutItem]` → `ProductAttributeValue`

| SQL (Dutch) | C# |
|-------------|-----|
| `Id` | `Id` |
| `ProductAttribuutId` | `ProductAttributeId` |
| `ProductProdId` | `ProductId` |
| `Waarde` (nvarchar 250) | `Value` |

DE-PARA also in [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md).

---

## 4. Behaviour

### 4.1 Admin

1. Attribute dictionary page — list/edit the 18 (and future) `ProductAttribuut` rows.  
2. Per product — pick an attribute, enter `Waarde`, save; update/remove later.  
3. One product may have many attributes; values are free text (distinct facet keys = exact `Waarde` strings).

### 4.2 Store

1. User navigates to a **leaf** `ProductStructure` category (`Catalog.razor`).  
2. Load products in that leaf (`ShowOnWebshop`).  
3. Load `ProductAttribuutItem` for those products.  
4. Build facet groups: each attribute that has ≥1 value → distinct `Waarde` + counts.  
5. Same checkbox sidebar layout as today; selections filter the product grid.  
6. Guests may browse and filter; login only for place order / pay / account (unchanged §9.1–9.2).

### 4.3 Hexagonal (target)

```text
Catalog.razor + StoreFacetSidebar (layout kept)
  → IStoreCatalogPort (facets + filtered catalog)
  → StoreCatalogService
  → ProductAttribuut / ProductAttribuutItem

Admin pages
  → IProductAttributeAdminPort (names TBD at implement)
  → repositories → same tables
```

No `DbContext` in Razor.

---

## 5. Delivery phases (implementation — not started in this docs-only pass)

| Phase | Deliverable | Status |
|-------|-------------|--------|
| **D0 — Docs** | This PLAN + SPEC/AMENDMENTS/DATA sync | ✅ This pass |
| **D1 — SQL + EF** | Script + seed 18 + ModelBuilder map | ⬜ |
| **D2 — Delete pilot** | Remove S.7 filter code / `StoreCatalogFilters` / ProductProperty catalog facets | ⬜ |
| **D3 — Admin** | Attribute list + per-product values | ⬜ |
| **D4 — Store** | Wire facets to `ProductAttribuutItem` (layout unchanged) | ⬜ |
| **D5 — Tests** | One test file per SUT | ⬜ |

---

## 6. Related docs

| Doc | Role |
|-----|------|
| [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.1 | Store facet behaviour (target) |
| [SPEC_ADMIN.md](./SPEC_ADMIN.md) | Admin surfaces (update when UI lands) |
| [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) | Config — old `StoreCatalogFilters` removed from target |
| [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md) | DE-PARA |
| [SPEC_IMPLEMENTATION_ROADMAP_open.md](./SPEC_IMPLEMENTATION_ROADMAP_open.md) | S.7 status |
| [AMENDMENTS.md](./AMENDMENTS.md) | Dated runtime notes |

Obsolete: Coolblue / Handzenders pilot analysis; `docs/mocks/mock-store-filters*.html` as live intent (illustrative only until replaced).

---

**© 2026 AdminSense. All rights reserved.**
