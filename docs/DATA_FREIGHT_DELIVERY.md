# ðŸšš Freight / Delivery â€” ERP Mapping (Dutch DB â†’ English Code)

<p style="display:flex;flex-wrap:nowrap;gap:0.35rem;align-items:center;overflow-x:auto;margin:0.5rem 0 0;"><img alt="Status" src="https://img.shields.io/badge/Status-Analysed%20from%20abmatic__test-0ea5e9?style=flat-square" /><img alt="Rule" src="https://img.shields.io/badge/Default%20fee-%E2%82%AC0-64748b?style=flat-square" /><img alt="Type" src="https://img.shields.io/badge/Type-Data%20Reference-512BD4?style=flat-square" /></p>

**Freight and delivery fee mapping from live ERP database**

---> **Source of truth:** live Azure SQL `abmatic_test` (queried 2026-07-14).  
> Product/UI labels stay **Dutch** as stored in ERP; **code identifiers stay English**.  
> Schema rules: `AGENTS.md` / `db-first` (not restated here).

---

## ðŸŒ Language / Mapping Conventions (Repo-Wide)

| Layer | Language | Rule |
|-------|----------|------|
| **Database** | Mostly **Dutch** schemas/tables/columns (`Projecten`, `Klanten`, `ProductPrijzen`â€¦). Some names already look English (`Products`, `Emails`) but treat physical names as ERP truth. | Never rename live tables from the app |
| **C# entities / ports / use cases** | **English** | `DeliveryType`, `OrderDeliveryTypeProduct`, `GrossSalesPrice` |
| **EF ModelBuilder** | English prop â†’ Dutch column | e.g. `DeliveryTypeId` â†’ `LeverigsType` on `Klant` (note ERP typo) |
| **XML / comments on entities** | **English**, with mapping notes | `/// Entity for [Projects].[DeliveryTypes] (legacy: [Projecten].[LeveringType]).` |
| **Customer-facing labels** | As in ERP (often Dutch `ProdName` / `Naam`) | Show DB text; do not invent EN product names for freight SKUs |
| **Store UI chrome** (buttons) | English app copy today | Separate from ERP product names |

Full table map: [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md).

**Implemented** in store checkout (2026-07-14): mock â‚¬9 removed; cart selects a freight product for the customerâ€™s `DeliveryTypeId`; fee from `ProductPrices` or **â‚¬0**.

---

## ðŸ”§ How Freight Actually Works in AB-MATIC

`DeliveryType` (**NL** `LeveringType`) has **no price column**.  
Freight = **product(s)** linked to a delivery type, priced via **product prices**.

```text
Customer.DeliveryTypeId          (NL Klanten.Klant.LeverigsType)
        â”‚
        â–¼
DeliveryType                     (NL Projecten.LeveringType)
        â”‚
        â–¼
OrderDeliveryTypeProduct[]       (NL Projecten.DossierLeveringsTypeProduct)
        â”‚   LeveringTypeId + ProductProdId
        â–¼
Product + ProductPrice           (NL Products.Product + Products.ProductPrijzen.BrutoVerkoop)
        â”‚
        â–¼
Store checkout DeliveryFee       (English code) â€” â‚¬0 if no usable link/price
```

### ðŸ—ºï¸ Mapping (Freight)

| English (code) | Dutch (DB) | Role |
|----------------|------------|------|
| `DeliveryType` / `DeliveryTypes` | `Projecten.LeveringType` | Lookup: Levering, Montage, Afhaling, Verzending |
| `DeliveryType.Name` | `Naam` | Dutch label |
| `DeliveryType.IncludeInstallationCost` | `MontageKostTellen` | Flag only |
| `DeliveryType.IsDefault` | `IsDefault` | Default type (Montage in current DB) |
| `OrderDeliveryTypeProduct` | `Projecten.DossierLeveringsTypeProduct` | Type â†’ freight/service **ProductId** |
| `OrderDeliveryTypeProduct.LeveringTypeId` | `LeveringTypeId` | FK type |
| `OrderDeliveryTypeProduct.ProductId` | `ProductProdId` | Freight/montage/shipping SKU |
| `Customer.DeliveryTypeId` | `Klanten.Klant.LeverigsType` | Customerâ€™s usual type (**typo** `Leverigs` in ERP) |
| `CustomerType.DeliveryTypeId` | `Klanten.KlantType.LeveringsType` | Default per customer type |
| `Order.DeliveryTypeId` | `LeveringsType` on order | Persisted on webshop order |
| `ProductPrice.GrossSalesPrice` | `ProductPrijzen.BrutoVerkoop` | List price used for fee |
| `ProductPrice.FromAddress` / `ValidTo` | `Van` / `Tot` | Validity window |
| `OrderLine.IsLeveringsTypeProduct` | `IsLeveringsTypeProduct` | Marks delivery line on dossier |

---

## ðŸ“Š What We Saw on `abmatic_test` (Counts)

| `DeliveryType` Id | Dutch name | Mapped products | Typical price pattern |
|-------------------|------------|-----------------|------------------------|
| 1 | Levering | 2 | â‚¬/km rates (`0,90`, `1,30`) â€” **not** a flat fee |
| 2 | Montage | many | Mostly â‚¬0 notes / hours; some lines e.g. logistics â‚¬20 |
| 3 | Afhaling | **0** | â†’ store fee **â‚¬0** (no rows) |
| 4 | Verzending | many GLS/Vengo | Flat pallet/parcel fees (â‚¬17â€¦â‚¬135); some expired/0 |

Customer distribution (approx.): Montage â‰« Levering â‰ˆ Afhaling > Verzending â€” all four used.

---

## Webshop rules (decision â€” owner-confirmed direction)

1. **Remove** any hardcoded delivery fee (`9.00m`) from Application / Infrastructure / Client.
2. **Default fee = â‚¬0** when:
   - no `OrderDeliveryTypeProduct` rows for the customerâ€™s `DeliveryTypeId`, or
   - no selected freight product, or
   - selected product has **no valid** `ProductPrice` / price resolves to null â†’ treat as **0** (admin can fix prices later).
3. Fee comes **only** from ERP: map type â†’ product â†’ `IProductPricingPort` / `ProductPrices` (same validity rules as catalog).
4. **Do not auto-sum** all products under a type (would charge every montage note/hour SKU). Store picks **one** freight product (or none â†’ â‚¬0).
5. **Per-km / formula** products stay at list unit price only if used as qty=1 placeholder, or remain **â‚¬0 until** a later qty/km feature â€” prefer showing Dutch `ProdName` and fee **0** until there is a clear flat `BrutoVerkoop` selection (Verzending-style).
6. Persist chosen type on `Order.DeliveryTypeId`; when fee > 0, add order line with `IsLeveringsTypeProduct` and real `ProductId` when known (improve over null ProductId â€œStandard deliveryâ€).
7. Admin later adjusts zero/wrong freights via existing product prices + `DossierLeveringsTypeProduct` / delivery-types screens.

### Cart UX (implementation target)

- Use customer `DeliveryTypeId` as starting type (English code / Dutch DB).
- List linked freight products for that type (Dutch names + resolved price or â€œâ‚¬0â€).
- Allow picking one option or â€œNo delivery chargeâ€ (explicit â‚¬0).
- Quote/totals: `DeliveryFee` from selection only.

---

## Deferred product behaviour

- Auto-adding every Montage accessory line under a type (ERP lists many related SKUs; store picks **one** freight product or â‚¬0).
- Real Mollie (blocked until client keys).

### Note on â€œâ‚¬/kmâ€ products in ERP

Some linked freight products are named like *â€œTransport â€¦ 0,9â‚¬ per kmâ€* and have `BrutoVerkoop` â‰ˆ `0.90`. That is **ERP catalogue text / unit list price**, not a webshop distance engine.

The store **does not** know trip km (no geocoding, no route table wired for checkout). So today those options only show the **unit price from `ProductPrices`** if selected â€” they are **not** multiplied by kilometres. Until the client defines an existing ERP source for distance, treat â‚¬/km SKUs like any other selectable product row (or leave fee at â‚¬0 by not selecting them).

---

## Related

- Store behaviour: [SPEC_WEB_STORE.md](./SPEC_WEB_STORE.md) Â§4.4 / freight  
- Roadmap item **S.5**: [SPEC_IMPLEMENTATION_ROADMAP_open.md](./SPEC_IMPLEMENTATION_ROADMAP_open.md)  
- NLâ†”EN map: [DATA_DUTCH_ENGLISH_MODEL.md](./DATA_DUTCH_ENGLISH_MODEL.md)
- DB-first hard rule: `AGENTS.md` / `.cursor/rules/db-first.mdc` (not repeated here)
