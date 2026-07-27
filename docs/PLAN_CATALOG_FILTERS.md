# Catalog filters â€” ProductAttribuut (client model)

<p style="display:flex;flex-wrap:nowrap;gap:0.35rem;align-items:center;overflow-x:auto;margin:0.5rem 0 0;"><img alt="Status" src="https://img.shields.io/badge/Status-Implemented-28a745?style=flat-square" /><img alt="Scope" src="https://img.shields.io/badge/Scope-Store%20+%20Admin-512BD4?style=flat-square" /></p>

**Canonical working spec for catalog attribute filters.**  
S.7 pilot removed. Runtime uses `ProductAttribuut` / `ProductAttribuutItem` only. Apply SQL script on `abmatic_test` before first use.

---

## 1. Client requirements (mandatory)

| Rule | Detail |
|------|--------|
| Attributes on **products only** | Not on categories |
| Admin maintains dictionary + values | Staff create/seed attributes; per product choose attribute and fill value; client can update later |
| Store filters | User opens a **leaf** category â†’ system loads attributes present on products in that category â†’ shows **distinct** values as **checkboxes** â†’ check/uncheck filters the grid |
| Applies to all products/categories | No Handzenders-only whitelist; any leaf with product attribute data shows filters |
| Layout | **Unchanged** â€” keep current `Catalog.razor` + facet checkbox sidebar chrome |

**Do not use** for these filters: `ProductOption` (checkout), legacy `ProductProperty` / `ProductPropertieItem` (old ERP property sheet â€” not this feature).

**Replaces** the S.7 pilot entirely (Merk / Voorraad / Prijs / `ProductProperty` facets / `StoreCatalogFilters` whitelist). Pilot code has been deleted.

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

## 3. Schema (Dutch SQL â†’ English C#)

**Explicit exception to â€œnever invent ERP tablesâ€:** client asked for new tables for this feature. Delivery = **SQL script** applied by Marco/DBA on `abmatic_test`, then EF mapping only. **No** `Migrate()` / `EnsureCreated()`.

### `[Products].[ProductAttribuut]` â†’ `ProductAttribute`

| SQL (Dutch) | C# |
|-------------|-----|
| `Id` | `Id` |
| `NaamEn` | `NameEn` |
| `NaamNl` | `NameNl` |
| `NaamFr` | `NameFr` |
| `Volgorde` | `SortOrder` |

### `[Products].[ProductAttribuutItem]` â†’ `ProductAttributeValue`

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

1. **`/admin/attributes`** â€” dictionary CRUD for `ProductAttribuut` (seeded 18 + future rows).  
2. **`/admin/product-attributes`** â€” dedicated assignment: search `[Products].[Product]` by **NameNl / NameEn / NameFr** (+ part number / EAN), select `ProdId`, then add/edit/delete `Waarde` rows. Deep-link: `/admin/product-attributes/{ProductId}`. Optional shortcut from Product list.  
3. One product may have many attributes; values are free text (distinct facet keys = exact `Waarde` strings).

### 4.2 Store

1. User navigates to a **leaf** `ProductStructure` category (`Catalog.razor`).  
2. Load products in that leaf (`ShowOnWebshop`).  
3. Load `ProductAttribuutItem` for those products.  
4. Build facet groups: each attribute that has â‰¥1 value â†’ distinct `Waarde` + counts.  
5. Same checkbox sidebar layout as today; selections filter the product grid.  
6. Guests may browse and filter; login only for place order / pay / account (unchanged Â§9.1â€“9.2).

### 4.3 Hexagonal (target)

```text
Catalog.razor + StoreFacetSidebar (layout kept)
  â†’ IStoreCatalogPort (facets + filtered catalog)
  â†’ StoreCatalogService
  â†’ ProductAttribuut / ProductAttribuutItem

/admin/attributes â†’ IProductAttributeAdminPort
/admin/product-attributes â†’ IProductAttributeAssignmentAdminPort
  â†’ repositories â†’ same tables
```

No `DbContext` in Razor.

---

## 5. Delivery phases

| Phase | Deliverable | Status |
|-------|-------------|--------|
| **D0 â€” Docs** | This PLAN + SPEC/AMENDMENTS/DATA sync | âœ… |
| **D1 â€” SQL + EF** | `scripts/ProductAttribuut_create_and_seed.sql` + ModelBuilder map | âœ… (DBA must run script on `abmatic_test`) |
| **D2 â€” Delete pilot** | Removed S.7 filter code / `StoreCatalogFilters` / ProductProperty catalog facets | âœ… |
| **D3 â€” Admin** | `/admin/attributes` + `/admin/product-attributes` | âœ… |
| **D4 â€” Store** | Facets from `ProductAttribuutItem` (layout unchanged) | âœ… |
| **D5 â€” Docs** | PLAN / SPEC / AMENDMENTS sync | âœ… |

---

## 6. Related docs

| Doc | Role |
|-----|------|
| [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) Â§4.1 | Store facet behaviour (target) |
| [SPEC_ADMIN.md](./SPEC_ADMIN.md) | Admin surfaces (update when UI lands) |
| [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) | Config â€” old `StoreCatalogFilters` removed from target |
| [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md) | DE-PARA |
| [SPEC_IMPLEMENTATION_ROADMAP_open.md](./SPEC_IMPLEMENTATION_ROADMAP_open.md) | S.7 status |
| [AMENDMENTS.md](./AMENDMENTS.md) | Dated runtime notes |

Obsolete: Coolblue / Handzenders pilot analysis; `docs/mocks/mock-store-filters*.html` as live intent (illustrative only until replaced).

---

**Â© 2026 AdminSense. All rights reserved.**
