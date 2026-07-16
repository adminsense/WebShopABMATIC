---
paths:
  - "WebShopABMATIC.Client/Components/Store/**"
  - "WebShopABMATIC.Client/Components/Pages/Store/**"
  - "WebShopABMATIC.Client/Services/Store*.cs"
  - "WebShopABMATIC.Client/wwwroot/css/store.css"
  - "WebShopABMATIC.Client/wwwroot/js/store-*.js"
  - "WebShopABMATIC/wwwroot/css/store.css"
  - "WebShopABMATIC/wwwroot/js/store-*.js"
  - "WebShopABMATIC/wwwroot/js/mollie-*.js"
---

# Store UI rules

- Spec first: `docs/SPEC_WEB_STORE.md` (price/stock §5.1, guest vs login §9.1).
- Patterns: `docs/PATTERNS_UI_QUICK_START.md` — match existing store components (`StoreProductCard`, `StoreHeader`, `ProductCartButton`).
- Guests see **list price** or **Out of stock** / **Price on request**. Never “login to see price” on cards.
- Price/stock labels: use `StorePriceFormatter.FormatListPrice` / `OutOfStockLabel` / `OnRequestLabel` (do not hardcode strings in cards/search/detail/cart button).
- Login only when **buying** (add to cart / checkout), not to browse.
- Stock: `IsOutOfStock` → label **Out of stock** (not “Unavailable”); keep product visible.
- Cart stale stock: keep the line; blocking danger UI + disabled checkout (never look like “Place order” still works). See SPEC §5.2.
- CSS: prefer `store.css` variables/classes already in use; do not invent a parallel theme.
- After UI behaviour changes: update `SPEC_WEB_STORE` and a line in `docs/AMENDMENTS.md` if needed.
