# Catalog filters — ProductAttribuut (client model)

![Status](https://img.shields.io/badge/Status-Implemented-28a745?style=flat-square) ![Scope](https://img.shields.io/badge/Scope-Store%20+%20Admin-512BD4?style=flat-square)

**Canonical working spec for catalog attribute filters.**  
S.7 pilot removed. Runtime uses `ProductAttribuut` / `ProductAttribuutItem` only. Apply SQL script on `abmatic_test` before first use.

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

**Replaces** the S.7 pilot entirely (Merk / Voorraad / Prijs / `ProductProperty` facets / `StoreCatalogFilters` whitelist). Pilot code has been deleted.

---

## 2. Seed attribute list (18)

Dictionary rows in `[Products].[ProductAttribuut]` (`Naam` only; no product values until staff fill). Seed is in [`scripts/ProductAttribuut_create_and_seed.sql`](../scripts/ProductAttribuut_create_and_seed.sql) (idempotent `NOT EXISTS` on `Naam`):

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

Store/admin labels use `Naam` (C# `Name`). `Gegevenstype` / `Eenheid` stay NULL until client fills them.

---

## 3. Schema (Dutch SQL → English C#)

**Explicit exception to “never invent ERP tables”:** client asked for new tables for this feature. Delivery = **SQL script** applied by Marco/DBA on `abmatic_test`, then EF mapping only. **No** `Migrate()` / `EnsureCreated()`.

Script: [`scripts/ProductAttribuut_create_and_seed.sql`](../scripts/ProductAttribuut_create_and_seed.sql)

### `[Products].[ProductAttribuut]` → `ProductAttribute`

| SQL (Dutch) | C# |
|-------------|-----|
| `AttribuutId` | `Id` |
| `Naam` | `Name` |
| `Gegevenstype` | `DataType` |
| `Eenheid` | `Unit` |

### `[Products].[ProductAttribuutItem]` → `ProductAttributeValue`

| SQL (Dutch) | C# |
|-------------|-----|
| `Id` | `Id` |
| `ProductAttribuutId` | `ProductAttributeId` |
| `ProductProdId` | `ProductId` (FK → **`Product.ProdId`**) |
| `Waarde` (nvarchar 100) | `Value` |

Join for assignment screen (`/admin/product-attributes/{ProdId}`):

```sql
FROM Products.Product p
INNER JOIN Products.ProductAttribuutItem i ON i.ProductProdId = p.ProdId
INNER JOIN Products.ProductAttribuut a ON a.AttribuutId = i.ProductAttribuutId
WHERE p.ProdId = @ProdId
```

UI reference (do not alter PNG): [`docs/images/tela_atributos.png`](./images/tela_atributos.png)

DE-PARA also in [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md).

### How tables map to the store filter sidebar

```text
ProductAttribuut.Naam          → facet group title
ProductAttribuutItem.Waarde    → checkbox label (distinct values in the leaf)
Product.ProdId                 → product in the category grid (via ProductProdId)
```

`Gegevenstype` / `Eenheid` on `ProductAttribuut` stay NULL until the client fills them; they are **not** the checklist values.

**Target sidebar UI (leaf category only):**

```text
Power Supply
  [ ] 24 VDC
  [ ] 12-24 VACDC
  [ ] 230 VAC

Gate Type
  [ ] Sliding
  [ ] Swing
  [ ] …

Safety Features
  [ ] Photocells
  [ ] …
```

Demo seed for those `Waarde` rows: [`scripts/ProductAttribuutItem_demo_seed.sql`](../scripts/ProductAttribuutItem_demo_seed.sql).

**Important (CD4):** facets appear only on a **true leaf** (no child `ProductStructuur` rows). The seed resolves a leaf under the structure of ProdId `11742` that has `WebShop` products — **not** the parent Id itself when that Id has children (e.g. parent `17` shows tiles only). `Waarde` must be on products whose `ProductStructuurId` equals that leaf Id. After the script, open the printed `/?categoryId=<LeafId>`.

---

## 4. Behaviour

### 4.1 Admin

1. **`/admin/attributes`** — dictionary CRUD for `ProductAttribuut` (optional; out of current UI scope if route not shipped).
2. **`/admin/product-attributes`** — product-selection grid: search `[Products].[Product]` by **NameNl / NameEn / NameFr** (+ part number / EAN), then the **Attributes** action opens `ProductAttributeValuesModal` over the grid to add/edit/delete `Waarde` rows; closing returns to the grid. Same modal is reused by the Attributes action on `/admin/products`. Deep-link `/admin/product-attributes/{ProdId}` opens the modal directly. Content per [`tela_atributos.png`](./images/tela_atributos.png) (now rendered inside the modal, without the old *Change product* button).
3. One product may have many attributes; values are free text (distinct facet keys = exact `Waarde` strings). Dropdown labels = `Naam`.
### 4.2 Store

1. User navigates to a **leaf** `ProductStructure` category (`Catalog.razor`) — last tree level; parents show child tiles only (no facet sidebar).  
2. Load products in that leaf (`ShowOnWebshop` / column `WebShop`).  
3. Load `ProductAttribuutItem` for those products (**same** `ProductStructuurId` as the leaf).  
4. Build facet groups: each attribute that has ≥1 value → distinct `Waarde` + counts.  
5. Same checkbox sidebar layout as today; selections filter the product grid.  
6. Guests may browse and filter; login only for place order / pay / account (unchanged §9.1–9.2).

### 4.3 Hexagonal (target)

```text
Catalog.razor + StoreFacetSidebar (layout kept)
  → IStoreCatalogPort (facets + filtered catalog)
  → StoreCatalogService
  → ProductAttribuut / ProductAttribuutItem

/admin/attributes → IProductAttributeAdminPort
/admin/product-attributes → IProductAttributeAssignmentAdminPort
  → repositories → same tables
```

No `DbContext` in Razor.

---

## 5. Delivery phases

| Phase | Deliverable | Status |
|-------|-------------|--------|
| **D0 — Docs** | This PLAN + SPEC/AMENDMENTS/DATA sync | ✅ |
| **D1 — SQL + EF** | `scripts/ProductAttribuut_create_and_seed.sql` + ModelBuilder map | ✅ (DBA must run script on `abmatic_test`) |
| **D2 — Delete pilot** | Removed S.7 filter code / `StoreCatalogFilters` / ProductProperty catalog facets | ✅ |
| **D3 — Admin** | `/admin/attributes` + `/admin/product-attributes` | ✅ |
| **D4 — Store** | Facets from `ProductAttribuutItem` (layout unchanged) | ✅ |
| **D5 — Docs** | PLAN / SPEC / AMENDMENTS sync | ✅ |

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
