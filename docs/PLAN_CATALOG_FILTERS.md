# Provisional analysis — Coolblue-style catalog filters (faceted search)

![Status](https://img.shields.io/badge/Status-Pilot%20implemented-28a745?style=flat-square) ![Scope](https://img.shields.io/badge/Scope-Store%20catalog-0ea5e9?style=flat-square)

> **Purpose:** Working note for Coolblue-style catalog filters.  
> **Reference:** [Coolblue — Windows all-in-one PCs (sale)](https://www.coolblue.be/nl/desktops/windows/all-in-one-pcs/solden).  
> **Runtime (2026-07-20):** Pilot **S.7** shipped — Merk + Voorraad + Prijs on whitelisted leaf categories (`StoreCatalogFilters`, default id **54**). `ProductProperty` facets appear when ERP rows exist; otherwise a muted placeholder. Promote lasting behaviour into `SPEC_WEB_STORE.md` §4.1 (done); keep this file for decision history until client expands the IT whitelist / admin property CRUD.

**Process followed:** [AGENTS.md](../AGENTS.md) → `SPEC_WEB_STORE.md`, `DATA_*`, live code (`StoreCatalogService`, `Catalog.razor`).

---

## 1. What the client is asking for

On Coolblue, a **category product list** has:

| UI element | Example (Dutch) | Behaviour |
|------------|-----------------|-----------|
| Left **filter sidebar** | Processor, Videokaart, Werkgeheugen, Merk, Totale opslagcapaciteit, Prijs, … | Checkbox groups with **live counts** `(n)` |
| **Recommended** badge | “Aanbevolen” on some values | Editorial / merchandising hint |
| **Explanation** link | “Uitleg” per group | Help text per attribute |
| Result header | “11 desktops” + sort | List updates when filters change |
| Product cards | Spec chips in subtitle | e.g. “Intel Core Ultra 7 \| Shared videokaart \| 32 GB RAM” |

This is **faceted navigation** (e-commerce attribute filters), not the same as our **category tree** or **product options at checkout**.

---

## 2. What WebShopABMATIC has today

| Capability | Status | Implementation |
|------------|--------|----------------|
| Category tree (sidebar) | ✅ | `ProductStructure` + optional `WebshopStructure`; leaf → product grid (`Catalog.razor`, `StoreCategorySidebar`) |
| Text search | ✅ | `StoreSearchModal` — loads catalog slice, filters client-side |
| Sort | 🔶 | Limited; no dedicated server sort API |
| **Attribute / spec filters** | ✅ Pilot | Whitelisted leaves only — `StoreFacetSidebar` + `StoreCatalogFilters` (default category **54**) |
| Manufacturer on product | ✅ | DB + store facet **Merk**; `Product.ManufacturerId` → `Crm.Manufacturer` |
| Per-product **order options** | ✅ | `ProductOption` + `ProductOptionValue` — **checkout configurator**, not catalog filters |
| Product **spec properties** | 🔶 | EF mapped; store facets when `ProductPropertyItem` rows exist; no admin CRUD yet |

**DB-first rule:** we must not invent new ERP tables. Any filter dimension must map to **existing** columns/tables (`AGENTS.md`, `SPEC_INFRASTRUCTURE` §4).

---

## 3. ERP building blocks (relevant)

| Dutch / table | C# entity | Role for filters |
|---------------|-----------|------------------|
| `[Products].[ProductStructuur]` | `ProductStructure` | **Navigation tree** — which products belong to which category (`Product.ProductStructureId`) |
| `[Products].[WebshopStructuur]` | `WebshopStructure` | Optional **store menu** overlay |
| `[Products].[ProductProperty]` | `ProductProperty` | **Dictionary** of attribute names (NL/FR/EN), `SortOrder` |
| `[Products].[ProductPropertieItem]` | `ProductPropertyItem` | **Per-product value** — `ProductId` + `ProductPropertyId` + free-text `Value` (max 250) |
| `[Crm].[Manufacturer]` | `Manufacturer` | **Brand** filter via `Product.ManufacturerId` |
| `[Products].[ProductPrices]` | `ProductPrice` | **Price range** filter (list / customer price) |
| `[Products].[ProductOptions]` | `ProductOption` | ❌ **Not for faceted search** — buyer choices on product detail / order lines |

**Important distinction**

| | `ProductProperty` | `ProductOption` |
|---|-------------------|-----------------|
| **Purpose** | Spec sheet / discovery (“32 GB RAM”, “Intel Core i7”) | Configuration when ordering (cable length, mandatory choices) |
| **Scope** | Comparable across products in a category | Often unique per SKU |
| **Store today** | Not exposed | `StoreProductOptionsForm` on product detail |
| **Coolblue analogue** | Sidebar filters + spec line on card | Accessories / warranties at buy time |

There is **no** ERP column linking `ProductProperty` to a `ProductStructure` node (which properties apply to “Laptops” vs “Cables”). That scoping must live in **app configuration** or be **derived dynamically** from data present in the current category.

---

## 4. Why “all categories” is not viable

The client’s reference is **informatics** (desktops / all-in-one PCs) with a **stable, comparable attribute set**:

- Processor, GPU, RAM, storage, screen size, brand, price tier, use case, …

ABMATIC’s `ProductStructure` tree can cover **many unrelated domains** (storage, accessories, services, cables, paint-shop articles, etc.). For those:

| Problem | Effect |
|---------|--------|
| Different attribute sets per domain | A laptop filter sidebar on “Network cable” is meaningless |
| Free-text `ProductPropertyItem.Value` | Without normalization, facets break (“16GB”, “16 GB”, “16384 MB”) |
| Data entry cost | Every filterable SKU needs properties maintained — **no admin screen yet** |
| Empty facets | Coolblue hides or disables groups with 0 results; we’d show noise without data discipline |
| “Recommended” / “Uitleg” | Not in `ProductProperty` schema — needs editorial rules or manual tagging |

**Conclusion (draft):** Faceted filters should be **scoped to an agreed IT / informatics branch** of `ProductStructure` (leaf categories only), not every webshop category.

---

## 5. Proposed scope boundary

### 5.1 In scope (pilot)

Define with the client a **whitelist of leaf `ProductStructure` nodes** (or one subtree), e.g.:

- Desktops / workstations  
- All-in-one PCs  
- Laptops / notebooks  
- (Optional later) Monitors, servers, components  

For those leaves only:

- Show **filter sidebar** on the product grid (`Catalog.razor` when `categoryId` is a configured leaf).
- Other categories keep **today’s UX**: tree navigation + search, **no** spec sidebar.

### 5.2 Out of scope (initial release)

- Facets on non-IT categories  
- Review scores, “speed class”, marketing “recommended for use” (unless client adds data elsewhere)  
- Full Coolblue parity (connections, hybrid GPU taxonomy, etc.) in v1  
- Using `ProductOption` as catalog filters  

### 5.3 Category tree vs filters (both)

Keep the **left category tree** (`StoreCategorySidebar`). Add a **second column** or collapsible panel **only on IT product-list pages** — same information architecture as Coolblue: **category = context**, **facets = refinement within context**.

---

## 6. Technical approach options

### Option A — **Recommended:** `ProductProperty` + dynamic facets (DB-first)

**How it works**

1. Staff maintain `ProductProperty` definitions (global list) and `ProductPropertyItem` rows per product (admin UI to build).
2. On `GET` catalog for leaf category `C`:
   - Load product IDs where `ProductStructureId == C` and `ShowOnWebshop`.
   - Aggregate facets: for each `ProductPropertyId`, group distinct `Value` → count.
   - Add **Brand** from `ManufacturerId`, **Price** buckets from `ProductPrices`.
3. User selects checkboxes → server returns filtered product list + **updated facet counts** (Coolblue behaviour).
4. App config (`appsettings` or small JSON, not ERP migration) maps **structure node id → ordered list of property ids** to show (hide irrelevant groups).

**Pros:** Uses existing ERP tables; facets appear only when data exists; same model legacy ABMATIC may already use for price lists / exports.  
**Cons:** Needs **admin CRUD** for properties; needs **data quality** rules; no “recommended” badge without extra convention.

### Option B — Manufacturer + price only (minimal)

Filters: **Merk** + **Prijs** + stock. No `ProductProperty`.

**Pros:** Fast; `ManufacturerId` already on `Product`.  
**Cons:** Far from Coolblue reference; weak for “Core i7 / 32 GB” shopping.

### Option C — Parse product title / description

Regex or NLP on `NameNl` / `WebshopDescriptionNl`.

**Pros:** No new admin data entry.  
**Cons:** Fragile, not DB-first truth, unmaintainable for B2B — **reject**.

### Option D — New ERP tables / search engine

Elasticsearch, dedicated attribute tables, PIM integration.

**Pros:** Coolblue-scale.  
**Cons:** Violates current **DB-first / no invented schema** rule unless DBA delivers ERP change — **out of scope** unless client funds ERP work.

---

## 7. Recommended delivery plan

| Phase | Deliverable | Status |
|-------|-------------|--------|
| **0 — Decision** | IT subtree / pilot leaf | ✅ Pilot leaf **54** (Handzenders); expand whitelist via config |
| **1 — Data** | Populate `ProductProperty` + items | ⬜ Client/staff — sidebar shows muted placeholder until data exists |
| **2 — Admin** | `/admin/product-properties` | ⬜ Later |
| **3 — API** | `GetCategoryFacetsAsync` + filtered `GetCatalogAsync` | ✅ Public for guests (login only at checkout) |
| **4 — Store UI** | `StoreFacetSidebar` + query `?merk=&voorraad=&prijs=` | ✅ |
| **5 — Polish** | Sort, mobile drawer, card spec chips | ⬜ |

**Architecture (hexagonal):**

```text
Catalog.razor
  → IStoreCatalogPort.GetCategoryFacetsAsync / GetCatalogAsync(filters)
  → StoreCatalogService
  → ProductPropertyItems + Product + Manufacturer + ProductPrices
```

No `DbContext` in Razor; facet logic in Application/Infrastructure.

---

## 8. Coolblue → ERP mapping (pilot)

| Coolblue filter | ERP source | Notes |
|-----------------|------------|-------|
| Merk | `Product.ManufacturerId` | Ready |
| Prijs | `ProductPrices.GrossSalesPrice` | Range slider / buckets |
| Processor (CPU) | `ProductProperty` e.g. “Processor” | Client defines property id + allowed values |
| Videokaart | `ProductProperty` “GPU” | |
| Werkgeheugen | `ProductProperty` “RAM” | Normalize units |
| Totale opslagcapaciteit | `ProductProperty` “Storage” | |
| Schermdiagonaal | `ProductProperty` “Screen size” | IT categories only |
| Aanbevolen voor gebruik | — | **Not in ERP** — defer or manual tag property |
| Reviewscore | — | **Not in ERP** — defer |
| Aanbevolen badge on value | — | **Editorial** — defer v1 |

Product card subtitle chips can mirror Coolblue by reading the same `ProductPropertyItem` rows used for filters.

---

## 9. Risks and operational cost

| Risk | Mitigation |
|------|------------|
| Properties empty on live catalog | Pilot category only; hide sidebar if &lt; 1 facet group has data |
| Inconsistent values | Admin dropdown per property (reuse `ProductOptionValue` pattern?) **or** strict pick-list in admin — **confirm with DBA** if pick-lists must be ERP-wide |
| Performance (counts on every click) | Cache facet snapshot per category (short TTL); SQL `GROUP BY` on indexed keys |
| Client expects filters everywhere | Document **IT-only** rule in proposal; tree navigation for rest |
| Legacy ERP already uses `ProductProperty` differently | **Validate on `abmatic_test`** with client before building UI |

---

## 10. Open questions for the client

1. Which **`ProductStructure` nodes** (names/ids) are “informatics” for v1?  
2. Does legacy ABMATIC already maintain **`ProductProperty` / `ProductPropertyItems`** for those SKUs? Can we see production samples?  
3. Is **Dutch-only** filter labels OK (ERP `NameNl`), or EN/FR store toggle required?  
4. Are **“Aanbevolen”** badges and **use-case** filters mandatory for go-live, or acceptable in a later phase?  
5. Who **owns data entry** — client staff in admin, or import from supplier feeds?  
6. Should non-IT categories **stay as today** (tree only), or is simple **brand + price** enough there?

---

## 11. Draft decision (for Marco / client sign-off)

| Topic | Proposal |
|-------|----------|
| **Do we build Coolblue-style facets?** | **Yes**, but **not globally** |
| **Scope** | **IT / informatics leaf categories only** (whitelist) |
| **Data model** | Existing **`ProductProperty` + `ProductPropertyItem` + `Manufacturer` + `ProductPrices`** |
| **Do not use** | `ProductOption` as catalog filters |
| **Pilot** | One leaf category (e.g. all-in-one / desktops), 5–8 property groups + brand + price |
| **Prerequisite** | Confirm ERP data + build admin property maintenance |
| **Reject for now** | New tables, search engine, parsing titles, facets on all categories |

---

## 12. Related docs and code

| Item | Path |
|------|------|
| Store spec | [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.1 |
| Dutch ↔ English map | [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md) — `ProductProperty`, `ProductStructure` |
| Demo row counts | [DATA_SUMMARY.md](./DATA_SUMMARY.md) — 12 webshop structures, **0** property demo rows |
| Catalog service | `WebShopABMATIC/Infrastructure/Store/StoreCatalogService.cs` |
| Catalog page | `WebShopABMATIC.Client/Components/Pages/Store/Catalog.razor` |
| Order options (not facets) | `StoreProductOptionsForm.razor` |

---

**© 2026 — provisional; not product truth until promoted to SPEC.**
