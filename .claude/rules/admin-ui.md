---
paths:
  - "WebShopABMATIC.Client/Components/Pages/Admin/**"
  - "WebShopABMATIC.Client/Components/Admin/**"
  - "WebShopABMATIC.Client/Components/Layout/Admin*.razor"
  - "WebShopABMATIC.Client/wwwroot/css/admin.css"
  - "WebShopABMATIC/wwwroot/css/admin.css"
  - "WebShopABMATIC/Infrastructure/Admin/**"
---

# Admin UI rules

- Spec first: `docs/SPEC_ADMIN.md` (auth §2 — legacy `StaffUsers`, not Identity).
- Visual model: match [Adminsence.Shop](https://adminsenceweb.azurewebsites.net/) chrome + `docs/mocks/mock-admin.html` (hub / list / form). Do not invent a parallel admin skin.
- Patterns: `docs/PATTERNS_UI_QUICK_START.md` — OpenIconic for nav, Bootstrap Icons for actions; match hub/list/form shells.
- Admin roles: `Admin` / `Manager` only on `/admin/*`. Customers never use admin for their own orders.
- Login: `/admin/login` → `POST /account/admin-login`. Prefer existing `AdminLogin` / layout patterns.
- No DbContext in Razor — use admin ports / use cases already registered in DI.
- After behaviour changes: update `SPEC_ADMIN` (and `AMENDMENTS.md` if runtime-facing).
