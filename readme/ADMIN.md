# Admin Panel — Functional Specification

![Status](https://img.shields.io/badge/Status-Specified%20%2B%20partial%20build-28a745?style=flat-square) ![Screens](https://img.shields.io/badge/Screens-3%20layout%20types-0d47a1?style=flat-square) ![Entities](https://img.shields.io/badge/Hub%20entities-22-512BD4?style=flat-square) ![UI](https://img.shields.io/badge/UI-IMMO%20shell-0dcaf0?style=flat-square)

> [!IMPORTANT]
> **Executive Summary:** The WebShopABMATIC **Admin Panel** is the staff-facing Blazor Server application for managing catalog, customers, orders, stock, and settings. It follows the IMMO-style shell (sidebar, dashboard, hub cards, filter grids, forms) documented in the reference screenshots below. Access requires **Admin** or **Manager** roles via ASP.NET Core Identity.

### Coverage statistics

| Category | Count | Status | Notes |
|----------|-------|--------|-------|
| **Layout screenshots** | 3 | ✅ Documented | `main_screen`, `menu_screen`, `forms_screen` |
| **Sidebar menus** | 8 | ✅ Documented | Start through My profile |
| **Hub entities** | 22 | ✅ Documented | Full registration map |
| **Blazor routes** | 15+ | 🟡 Partial | Product CRUD + lists; placeholders for rest |
| **Stock rules** | 6 | ✅ Documented | Min/max, reserve, low-stock KPI |

### Implementation quality

| Aspect | Status | Details |
|--------|--------|---------|
| **Shell + dashboard** | ✅ Complete | KPI cards wired to EF when DB available |
| **Hub navigation** | ✅ Complete | `AdminHubRegistry` mirrors sidebar |
| **List screens** | 🟡 Partial | Product, Customer, Order |
| **Forms** | 🟡 Partial | Product save; others planned |
| **Financial widgets** | ⏳ Planned | YTD returns zero until accounting |

---

## Overview

| Artifact | Path | Role |
|----------|------|------|
| **Blazor app** | `Web/` | Runnable admin UI (`admin@webshop.com / Admin@12345`) |
| **HTML prototype** | `docs/mock-admin.html` | Visual reference before Blazor |
| **Layout screenshots** | `readme/images/*_screen.png` | Approved shell patterns |
| **UI patterns** | [UI_PATTERNS_QUICK_START.md](UI_PATTERNS_QUICK_START.md) | Buttons, grids, forms |
| **Infrastructure** | [INFRASTRUCTURE.md](INFRASTRUCTURE.md) | Auth, DB, configuration |

### Implementation status

| Area | Status | Details |
|------|--------|---------|
| **Shell layout** | ✅ Implemented | Sidebar, top bar, logout, footer |
| **Dashboard KPIs** | ✅ Implemented | `IAdminDashboardPort` + portfolio cards |
| **Hub navigation** | ✅ Implemented | 7 sidebar menus, entity cards |
| **List + filters** | ✅ Product, Customer, Order | Apply/Clear, `table-dark`, edit icon |
| **Forms** | ✅ Product CRUD | Other entities: routes ready (placeholder) |
| **Stock alerts on dashboard** | ✅ Implemented | `Quantity <= MinQuantity` count |
| **Financial YTD widgets** | 🟡 Placeholder | Values return `0` until accounting wired |

---

## 🖥️ 1. Visual layout (reference screenshots)

The admin UI is defined by **three screen types**. These match the legacy IMMO reference app and our HTML/Blazor mocks.

### 1.1 Main dashboard — `main_screen.png`

![Admin dashboard layout](images/main_screen.png)

| Element | Behaviour |
|---------|-----------|
| **Sidebar** | Dark navigation; brand box **WS WEBSHOP ABMATIC**; menu items with Open Iconic icons |
| **Top bar** | Greeting **Hello, {STAFF NAME}**; red **Logout** button |
| **Content** | 2×2 **portfolio cards** with KPIs, progress indicators, and action pills |
| **Footer** | Current date + application version (`v1.0`) |

**Blazor route:** `/admin`   (admin@webshop.com / Admin@12345)
**Purpose:** Landing page after login. Read-only summary with drill-down links to hubs (e.g. Webshop catalog → **Manage** → `/admin/hub/webshop`).

#### Dashboard widgets (vNext)

| Card | Metrics (data source) | Staff actions |
|------|----------------------|---------------|
| **Webshop catalog** | `Product.ShowOnWebshop` count; `WebshopStructure` node count | Open Webshop hub |
| **Sales & orders** | Orders this month; acceptance rate; pending `Order.IsAccepted = false` | Open Sales hub / order list |
| **Stock operations** | Low-stock count: `ProductStockLocation` where `Quantity <= MinQuantity` | Open Stock hub |
| **Financial · YTD** | Revenue, costs, net (accounting integration planned) | Reporting (future) |

---

### 1.2 Sub-menu hub — `menu_screen.png`

![Admin hub layout](images/menu_screen.png)

| Element | Behaviour |
|---------|-----------|
| **Back to start** | `btn-outline-secondary btn-sm` + `oi-arrow-left` |
| **Title + subtitle** | Menu name and one-line scope description |
| **Entity cards** | Icon circle, entity tag, title, description, full-width **"{Entity} form"** button |

**Blazor route:** `/admin/hub/{webshop|catalog|customers|sales|stock|settings|profile}`  
**Purpose:** Second navigation level — staff choose which **registration (master data)** to maintain before opening list or form screens.

---

### 1.3 List and form — `forms_screen.png`

![Admin list and form layout](images/forms_screen.png)

| Element | Behaviour |
|---------|-----------|
| **List header** | Entity title; green **Refresh** (`btn-success btn-sm`) |
| **Filter panel** | Search, dropdowns, **Modified only** checkbox |
| **Apply Filters** | `btn btn-primary` + `bi-funnel-fill` |
| **Clear** | `btn btn-danger` + `bi-x-circle-fill` |
| **Grid** | `table-dark`, striped rows, icon-only **Edit** (`btn-sm btn-primary`) |
| **Form** | Card with **Save** / **Cancel**; field validation per UI patterns |

**Blazor routes:** e.g. `/admin/products`, `/admin/products/{id}`, `/admin/products/new`  
**Purpose:** Standard CRUD pattern for every hub entity.

---

## 🔐 2. Authentication and login

### 2.1 Technology

| Item | Specification |
|------|----------------|
| **Provider** | ASP.NET Core Identity (cookie authentication) |
| **User store** | `ApplicationUser` + `ApplicationDbContext` |
| **Login page** | `/Account/Login` |
| **Post-login redirect** | Admin/Manager → `/admin`; others → return URL or home |

### 2.2 Roles and policies

| Role | Policy | Admin panel access |
|------|--------|-------------------|
| **Admin** | `AppPolicies.AdminOnly` | Full access; future: user/role management |
| **Manager** | `AppPolicies.AdminOrManager` | All operational menus (catalog, sales, stock, …) |
| **Customer** | `AppPolicies.CustomerOnly` | No admin — storefront only |

> [!NOTE]
> Staff permissions in the legacy model map from `StaffUser` flags (`Admin`, `ProductBeheer`, `Bestellingen`, …) to these roles over time.

### 2.3 Development seed accounts

| Email | Password | Roles |
|-------|----------|-------|
| `admin@webshop.com` | `Admin@12345` | Admin, Manager |
| `manager@webshop.com` | `Manager@12345` | Manager |

Seeded automatically in **Development** (`IdentitySeedHostedService`).

### 2.4 Logout

- Red **Logout** in top bar (matches `main_screen.png`).
- Ends cookie session; returns to login or storefront mock.

---

## 📋 3. Sidebar menus and registrations (master data)

Each sidebar item opens a **hub** of entity cards. Below: what staff **register and maintain** (CRUD), and primary database tables.

### 3.1 Menu summary

| # | Menu | Purpose | Hub entities |
|---|------|---------|--------------|
| 1 | **Start** | Dashboard only | — |
| 2 | **Webshop** | Storefront navigation and product grouping | `WebshopStructure`, `WebshopProductStructure` |
| 3 | **Catalog** | Products, pricing, options, suppliers | `Product`, `ProductPrice`, `ProductQuantityTier`, `ProductOption`, `PriceListCategory`, `Manufacturer`, `Supplier` |
| 4 | **Customers** | B2B accounts, addresses, discounts | `Customer`, `CustomerDeliveryAddress`, `CustomerProductDiscount`, `CustomerType` |
| 5 | **Sales** | Orders and fulfilment configuration | `Order`, `OrderStatus`, `DeliveryType` |
| 6 | **Stock** | Warehouses and quantities | `ProductStockLocation`, `StockLocation` |
| 7 | **Settings** | Payments, staff, VAT | `PaymentMethod`, `StaffUser`, `UserGroup`, `VatType` |
| 8 | **My profile** | Logged-in staff user | `StaffUser` (profile form) |

---

### 3.2 Webshop — storefront structure

| Entity | Table | What staff registers |
|--------|-------|----------------------|
| **Webshop structure** | `WebshopStructure` | Hierarchical **catalog menu** on the public site (`NameNl`, parent, `SortOrder`) |
| **Webshop product structure** | `WebshopProductStructure` | **Category labels** for the shop in NL/FR/EN |

**Store impact:** Drives category navigation on the web store (`WebshopStructure` chips / tree).

---

### 3.3 Catalog — products and pricing

| Entity | Table | What staff registers |
|--------|-------|----------------------|
| **Product** | `Product` | Master product: names, part numbers, supplier/manufacturer, **`ShowOnWebshop`**, `WebshopDescriptionNl`, EAN |
| **Product price** | `ProductPrice` | Price rows: gross/net sales and purchase, validity dates, assembly/installation |
| **Product quantity tier** | `ProductQuantityTier` | Volume discounts (`MinimumQuantity`, `Discount`) |
| **Product option** | `ProductOption` | Configurable options (required flag, sort order, price formulas) |
| **Price list category** | `PriceListCategories` | Sections for exported price lists |
| **Manufacturer** | `Manufacturer` | Brand/manufacturer master |
| **Supplier** | `Supplier` | Supplier master and price list metadata |

**Store impact:** Only `Product` with `ShowOnWebshop = true` appears on the storefront. Prices and options drive cart line calculations.

**Blazor status:** ✅ Full list + form for **Product**; other catalog entities — list/form routes prepared.

---

### 3.4 Customers — B2B accounts

| Entity | Table | What staff registers |
|--------|-------|----------------------|
| **Customer** | `Customer` | Company account, VAT, address, **`WebshopLogin`** + password hash, `CustomerTypeId`, `DeliveryTypeId` |
| **Customer delivery address** | `CustomerDeliveryAddress` | Ship-to addresses per customer |
| **Customer product discount** | `CustomerProductDiscount` | Customer-specific % discount per product (validity dates) |
| **Customer type** | `CustomerType` | Segment (dealer, contractor, …), base discount, default delivery |

**Store impact:** `WebshopLogin` is the customer sign-in on the web store. Discounts apply at checkout when implemented.

**Blazor status:** ✅ Customer **list**; form CRUD planned.

---

### 3.5 Sales — orders and workflow

| Entity | Table | What staff registers |
|--------|-------|----------------------|
| **Order** | `Order` + `OrderLine` | Sales orders: project link, acceptance, delivery type, discounts, line items |
| **Order status** | `OrderStatus` | Workflow steps; **`ReserveStock`**, **`AffectsStock`** flags |
| **Delivery type** | `DeliveryType` | Pickup, delivery, installation; cost inclusion rules |

**Staff workflows (vNext):**

- Review orders placed from the web store.
- Accept or reject (`Order.IsAccepted`).
- Progress status (drives stock reservation/consumption via `OrderStatus`).

**Blazor status:** ✅ Order **list**; detail editor planned.

---

### 3.6 Stock — inventory

| Entity | Table | What staff registers |
|--------|-------|----------------------|
| **Stock location** | `StockLocation` | Warehouses and storage sites (`IsWarehouse`) |
| **Product stock location** | `ProductStockLocation` | Per product/location: `Quantity`, `ReservedQuantity`, **`MinQuantity`**, **`MaxQuantity`**, last count |

**Blazor status:** Hub + routes ready; full CRUD planned.

---

### 3.7 Settings — platform configuration

| Entity | Table | What staff registers |
|--------|-------|----------------------|
| **Payment method** | `PaymentMethod` | Pre-pay / post-pay methods (NL/FR/EN names) |
| **Staff user** | `StaffUser` | Internal users, module flags, link to Identity |
| **User group** | `UserGroup` | Teams (installation, service, transport) |
| **VAT type** | `VatType` | VAT rates and invoice text |

> [!WARNING]
> **Staff user** management and Identity user linking are **Admin-only** in the target security model. Managers use operational menus only.

---

## 📦 4. Stock validation and alerts

Stock rules connect **admin maintenance**, **order workflow**, and the **storefront display**.

### 4.1 Data model (registrations)

| Field | Entity | Meaning |
|-------|--------|---------|
| `Quantity` | `ProductStockLocation` | On-hand stock at a location |
| `ReservedQuantity` | `ProductStockLocation` | Allocated to orders not yet fulfilled |
| `MinQuantity` | `ProductStockLocation` | **Reorder / low-stock threshold** |
| `MaxQuantity` | `ProductStockLocation` | Upper storage guideline |
| `ReserveStock` | `OrderStatus` | When order enters this status, stock is reserved |
| `AffectsStock` | `OrderStatus` | When order reaches this status, stock quantities are updated |

### 4.2 Admin validations and alerts

| Rule | Where enforced | Behaviour |
|------|----------------|-----------|
| **Low stock alert** | Dashboard **Stock operations** card | Count rows where `Quantity <= MinQuantity` |
| **Low stock review** | `/admin/product-stock` (planned) | Filter and edit min/quantity per product/location |
| **Inactive location** | `ProductStockLocation.IsInactive` | Exclude from availability (planned UI filter) |
| **Negative quantity** | Form validation (planned) | Block save if `Quantity < 0` |
| **Reserved vs available** | Business logic (planned) | Available = `Quantity - ReservedQuantity` (conceptual) |

### 4.3 Storefront stock display (linked behaviour)

| Rule | Where | Behaviour |
|------|-------|-----------|
| **Show availability hint** | Product detail page | Display e.g. “24 in stock” from default `ProductStockLocation` |
| **Low stock styling** | Catalog cards | Highlight when below threshold (e.g. orange “3 in stock”) |
| **Block add-to-cart** | Cart / checkout (planned) | Prevent order line if insufficient available quantity |
| **ShowOnWebshop gate** | Catalog query | Product hidden unless `ShowOnWebshop = true` |

> [!TIP]
> Store stock display is **informational** in the HTML mock; hard validation at checkout will be enforced in the Blazor storefront service layer.

---

## 📊 5. Dashboards and reporting

### 5.1 Start dashboard (`/admin`)

Primary operational dashboard — see [§1.1](#11-main-dashboard--main_screenpng).

| KPI | Calculation (current implementation) |
|-----|--------------------------------------|
| Products on webshop | `COUNT(Product WHERE ShowOnWebshop = true)` |
| Webshop structure nodes | `COUNT(WebshopStructure)` |
| Orders this month | `COUNT(Order WHERE CreatedAt >= month start)` |
| Pending acceptance | `COUNT(Order WHERE IsAccepted = false)` |
| Low stock alerts | `COUNT(ProductStockLocation WHERE Quantity <= MinQuantity)` |
| Revenue / costs / net YTD | Placeholder `0` until accounting module connected |

### 5.2 Future dashboards (planned)

| Dashboard | Audience | Content |
|-----------|----------|---------|
| **Sales pipeline** | Manager | Orders by `OrderStatus`, acceptance SLA |
| **Stock overview** | Warehouse | Locations under min, reserved vs on-hand |
| **Customer revenue** | Admin | Top customers, discounts applied |
| **Webshop analytics** | Marketing | Popular products, conversion (requires telemetry) |

---

## 🔄 6. Functional flows (staff)

```mermaid
flowchart TD
  Login[/Account/Login] --> Admin[/admin Dashboard]
  Admin --> Hub[/admin/hub/catalog]
  Hub --> List[/admin/products]
  List --> Form[/admin/products/id]
  Form --> List
  List --> Hub
  Hub --> Admin
  Admin --> Logout[Logout]
```

### 6.1 Typical tasks

| Task | Navigation path |
|------|-----------------|
| Publish product on webshop | Catalog → Product → set **ShowOnWebshop** |
| Set customer web login | Customers → Customer → **WebshopLogin** |
| Review new store order | Sales → Order → filter pending acceptance |
| Fix low stock | Stock → Product stock → increase **Quantity** or adjust **MinQuantity** |
| Add delivery address | Customers → Delivery address |

---

## ⚙️ 7. Blazor implementation map

| Screen type | Prototype | Blazor |
|-------------|-----------|--------|
| Dashboard | `mock-admin.html` `#view-dashboard` | `Components/Pages/Admin/Dashboard.razor` |
| Hub | `#view-hub` | `Hub.razor` |
| List | `#view-list` | `ProductList.razor`, `CustomerList.razor`, `OrderList.razor` |
| Form | `#view-form` | `ProductForm.razor` |
| Shell | sidebar + top bar | `AdminLayout`, `AdminSidebar`, `AdminTopBar` |

### 7.1 Run locally

```bash
cd Web
dotnet run
```

1. Open the HTTPS URL from the console.
2. Sign in: `admin@webshop.com` / `Admin@12345`.
3. Land on `/admin`.

> [!NOTE]
> Domain data requires SQL Server with the ABMATIC schema (`connWebShopABMATIC`). Identity database is created via EF migrations on first run in Development.

---

## ✅ 8. Validation and UI standards

All admin screens MUST follow [UI_PATTERNS_QUICK_START.md](UI_PATTERNS_QUICK_START.md):

| Control | Rule |
|---------|------|
| Back navigation | `oi-arrow-left`, `btn-outline-secondary btn-sm` |
| Grid edit | Icon-only `btn-sm btn-primary` |
| Apply / Clear filters | Full-size `btn-primary` / `btn-danger` |
| Form save | `btn-primary` with spinner when saving |
| Tables | `table-dark`, `table-striped`, `@key` on rows |
| Required fields | `*` in label, validation on blur (Blazor forms) |

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**
