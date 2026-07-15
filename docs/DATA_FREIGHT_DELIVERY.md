# Freight / delivery — ERP mapping (Dutch DB → English code)

![Status](https://img.shields.io/badge/Status-Analysed%20from%20abmatic__test-0ea5e9?style=flat-square) ![Rule](https://img.shields.io/badge/Default%20fee-%E2%82%AC0-64748b?style=flat-square)

> **Source of truth:** live Azure SQL `abmatic_test` (queried 2026-07-14).  
> Product/UI labels stay **Dutch** as stored in ERP; **code identifiers stay English**.  
> Schema rules: `AGENTS.md` / `db-first` (not restated here).

---

## Language / mapping conventions (repo-wide)

| Layer | Language | Rule |
|-------|----------|------|
| **Database** | Mostly **Dutch** schemas/tables/columns (`Projecten`, `Klanten`, `ProductPrijzen`…). Some names already look English (`Products`, `Emails`) but treat physical names as ERP truth. | Never rename live tables from the app |
| **C# entities / ports / use cases** | **English** | `DeliveryType`, `OrderDeliveryTypeProduct`, `GrossSalesPrice` |
| **EF ModelBuilder** | English prop → Dutch column | e.g. `DeliveryTypeId` → `LeverigsType` on `Klant` (note ERP typo) |
| **XML / comments on entities** | **English**, with DE-PARA | `/// Entity for [Projects].[DeliveryTypes] (legacy: [Projecten].[LeveringType]).` |
| **Customer-facing labels** | As in ERP (often Dutch `ProdName` / `Naam`) | Show DB text; do not invent EN product names for freight SKUs |
| **Store UI chrome** (buttons) | English app copy today | Separate from ERP product names |

Full table map: [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md).

**Implemented** in store checkout (2026-07-14): mock €9 removed; cart selects a freight product for the customer’s `DeliveryTypeId`; fee from `ProductPrices` or **€0**.

---

## How frete actually works in AB-MATIC

`DeliveryType` (**NL** `LeveringType`) has **no price column**.  
Frete = **product(s)** linked to a delivery type, priced via **product prices**.

```text
Customer.DeliveryTypeId          (NL Klanten.Klant.LeverigsType)
        │
        ▼
DeliveryType                     (NL Projecten.LeveringType)
        │
        ▼
OrderDeliveryTypeProduct[]       (NL Projecten.DossierLeveringsTypeProduct)
        │   LeveringTypeId + ProductProdId
        ▼
Product + ProductPrice           (NL Products.Product + Products.ProductPrijzen.BrutoVerkoop)
        │
        ▼
Store checkout DeliveryFee       (English code) — €0 if no usable link/price
```

### DE-PARA (freight)

| English (code) | Dutch (DB) | Role |
|----------------|------------|------|
| `DeliveryType` / `DeliveryTypes` | `Projecten.LeveringType` | Lookup: Levering, Montage, Afhaling, Verzending |
| `DeliveryType.Name` | `Naam` | Dutch label |
| `DeliveryType.IncludeInstallationCost` | `MontageKostTellen` | Flag only |
| `DeliveryType.IsDefault` | `IsDefault` | Default type (Montage in current DB) |
| `OrderDeliveryTypeProduct` | `Projecten.DossierLeveringsTypeProduct` | Type → freight/service **ProductId** |
| `OrderDeliveryTypeProduct.LeveringTypeId` | `LeveringTypeId` | FK type |
| `OrderDeliveryTypeProduct.ProductId` | `ProductProdId` | Freight/montage/shipping SKU |
| `Customer.DeliveryTypeId` | `Klanten.Klant.LeverigsType` | Customer’s usual type (**typo** `Leverigs` in ERP) |
| `CustomerType.DeliveryTypeId` | `Klanten.KlantType.LeveringsType` | Default per customer type |
| `Order.DeliveryTypeId` | `LeveringsType` on order | Persisted on webshop order |
| `ProductPrice.GrossSalesPrice` | `ProductPrijzen.BrutoVerkoop` | List price used for fee |
| `ProductPrice.FromAddress` / `ValidTo` | `Van` / `Tot` | Validity window |
| `OrderLine.IsLeveringsTypeProduct` | `IsLeveringsTypeProduct` | Marks delivery line on dossier |

---

## What we saw on `abmatic_test` (counts)

| `DeliveryType` Id | Dutch name | Mapped products | Typical price pattern |
|-------------------|------------|-----------------|------------------------|
| 1 | Levering | 2 | €/km rates (`0,90`, `1,30`) — **not** a flat fee |
| 2 | Montage | many | Mostly €0 notes / hours; some lines e.g. logistics €20 |
| 3 | Afhaling | **0** | → store fee **€0** (no rows) |
| 4 | Verzending | many GLS/Vengo | Flat pallet/parcel fees (€17…€135); some expired/0 |

Customer distribution (approx.): Montage ≫ Levering ≈ Afhaling > Verzending — all four used.

---

## Webshop rules (decision — owner-confirmed direction)

1. **Remove** any hardcoded delivery fee (`9.00m`) from Application / Infrastructure / Client.
2. **Default fee = €0** when:
   - no `OrderDeliveryTypeProduct` rows for the customer’s `DeliveryTypeId`, or
   - no selected freight product, or
   - selected product has **no valid** `ProductPrice` / price resolves to null → treat as **0** (admin can fix prices later).
3. Fee comes **only** from ERP: map type → product → `IProductPricingPort` / `ProductPrices` (same validity rules as catalog).
4. **Do not auto-sum** all products under a type (would charge every montage note/hour SKU). Store picks **one** freight product (or none → €0).
5. **Per-km / formula** products stay at list unit price only if used as qty=1 placeholder, or remain **€0 until** a later qty/km feature — prefer showing Dutch `ProdName` and fee **0** until there is a clear flat `BrutoVerkoop` selection (Verzending-style).
6. Persist chosen type on `Order.DeliveryTypeId`; when fee > 0, add order line with `IsLeveringsTypeProduct` and real `ProductId` when known (improve over null ProductId “Standard delivery”).
7. Admin later adjusts zero/wrong freights via existing product prices + `DossierLeveringsTypeProduct` / delivery-types screens.

### Cart UX (implementation target)

- Use customer `DeliveryTypeId` as starting type (English code / Dutch DB).
- List linked freight products for that type (Dutch names + resolved price or “€0”).
- Allow picking one option or “No delivery charge” (explicit €0).
- Quote/totals: `DeliveryFee` from selection only.

---

## Deferred product behaviour

- Auto-adding every Montage accessory line under a type (ERP lists many related SKUs; store picks **one** freight product or €0).
- Real Mollie (blocked until client keys).

### Note on “€/km” products in ERP

Some linked freight products are named like *“Transport … 0,9€ per km”* and have `BrutoVerkoop` ≈ `0.90`. That is **ERP catalogue text / unit list price**, not a webshop distance engine.

The store **does not** know trip km (no geocoding, no route table wired for checkout). So today those options only show the **unit price from `ProductPrices`** if selected — they are **not** multiplied by kilometres. Until the client defines an existing ERP source for distance, treat €/km SKUs like any other selectable product row (or leave fee at €0 by not selecting them).

---

## Related

- Store behaviour: [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) §4.4 / frete  
- Roadmap item **S.5**: [SPEC_IMPLEMENTATION_ROADMAP_open.md](./SPEC_IMPLEMENTATION_ROADMAP_open.md)  
- NL↔EN map: [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md)
- DB-first hard rule: `AGENTS.md` / `.cursor/rules/db-first.mdc` (not repeated here)