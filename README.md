# WebShop vNext — Storefront + Admin (template)

![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?style=flat-square&logo=blazor&logoColor=white) ![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet&logoColor=white) ![C#](https://img.shields.io/badge/C%23-13.0-239120?style=flat-square&logo=csharp&logoColor=white) ![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white) ![License](https://img.shields.io/badge/License-MIT-green?style=flat-square&logo=opensourceinitiative&logoColor=white)

This repository will become the **new version** of the WebShop experience:
- **Storefront** (catalog → product detail → cart → checkout → orders)
- **Admin** (dashboard → catalog management → sales management → access control)

We are taking the current live site (`adminsenceweb.azurewebsites.net`) as the source of truth for **data and flows**, and rewriting it into:
- a **mock-first UI** (static/prototype that mirrors the DTOs/Entities)
- then a **working template** for the WebShop solution (Admin + Site)

---

## ✅ 1. Goals (vNext)

- **Keep the domain stable** (products, categories, discounts, orders, customers, users/roles)
- **Modernize UI/UX** (new layout, components, responsive design)
- **Separate concerns**:
  - Storefront: customer-facing browsing & purchase flow
  - Admin: staff-facing management & access control
- **Mock-first workflow**: every screen has a mock + DTO contract + service endpoints plan.

---

## 🧩 2. Information sources in this repo

- **UI prototypes**: `docs/index.html`
  - **Admin mock**: `docs/mock-admin.html`
  - **Store/cart mock**: `docs/mock-shopcart.html`

These mocks already map UI labels to the underlying entities (example tags like `Product.StockQuantity`, `OrderItem`, `ApplicationUser`).

---

## 🔐 3. Authentication & Authorization (required)

### 3.1 Authentication
- **Storefront users** sign in / register.
- **Admin users** sign in and must have staff roles.

Recommended approach (aligned to current stack):
- ASP.NET Core Identity (cookie auth for Blazor Server)
- Optional: add JWT later only if we split into separate API + SPA.

### 3.2 Roles (minimum)
- **Admin**: full access
- **Manager**: catalog + orders (no user/role management)
- **Customer**: storefront only

### 3.3 Permission model (resource-based)
Resource permissions to model in code (initially via roles, upgradeable to claims):
- **Products**: read/write
- **Categories**: read/write
- **Discounts**: read/write
- **Orders**: read + update status
- **Customers**: read (support)
- **Users & roles**: read/write (admins only)

---

## 📦 4. Core domain & DTOs (vNext contracts)

The UI and endpoints should be driven by DTOs. Below is the initial contract list (field names are indicative and will be aligned to the EF models).

### 4.1 Catalog
- **ProductDto**
  - `Id`
  - `Name`
  - `Brand`
  - `Description`
  - `Price`
  - `StockQuantity`
  - `CategoryId`
  - `ImageUrl`
  - `IsActive` (optional)
- **CategoryDto**
  - `CategoryId`
  - `Name`
  - `Description`
- **DiscountDto**
  - `Id`
  - `Name`
  - `Code`
  - `Percent` (nullable)
  - `Amount` (nullable)
  - `StartDate`
  - `EndDate`
  - `IsActive`

### 4.2 Cart & checkout
- **CartDto**
  - `CartId`
  - `CustomerId`
  - `Items: List<CartItemDto>`
  - computed totals (server-side)
- **CartItemDto**
  - `CartItemId` (optional)
  - `ProductId`
  - `Quantity`
  - `UnitPrice` (snapshot)

### 4.3 Orders
- **OrderDto**
  - `OrderId`
  - `OrderDate`
  - `CustomerId`
  - `Status`
  - `TotalAmount`
  - `Items: List<OrderItemDto>`
- **OrderItemDto**
  - `OrderItemId`
  - `OrderId`
  - `ProductId`
  - `Quantity`
  - `UnitPrice`

Order status (initial):
- `Processing`, `Shipped`, `Delivered`, `Failed`, `Returned`

### 4.4 Customer & identity
- **CustomerDto**
  - `CustomerId`
  - `UserId` (Identity)
  - `FirstName`
  - `LastName`
  - `DateOfBirth`
  - `Gender`
  - `Phone`
  - `Address`
- **UserDto**
  - `UserId`
  - `Email`
  - `FullName`
  - `Roles: string[]`
  - `IsActive`

### 4.5 Dashboard aggregates (admin)
- **AdminDashboardDto**
  - `ProductsCount`, `OrdersCount`, `CustomersCount`, `Revenue`
  - `RecentOrders: List<OrderSummaryDto>`
  - `LowStock: List<LowStockProductDto>`

---

## 🖥️ 5. Screens (what we will build)

### 5.1 Storefront (customer)
- **Catalog**
  - search by text
  - filter by category
  - product cards (name/brand/price/stock)
- **Product detail**
  - gallery/image, description, qty selector, add to cart
  - reviews (optional v2)
- **Cart**
  - update quantities, remove items
  - apply discount code
  - summary totals
- **Checkout**
  - address selection/entry
  - create order
- **Auth**
  - sign in
  - register (creates Identity user + customer profile)
- **My orders**
  - list orders for logged-in customer

### 5.2 Admin (staff)
- **Dashboard**
  - KPIs + recent orders + low stock
- **Products**
  - list + create/edit + delete
- **Categories**
  - list + create/edit + delete
- **Discounts**
  - list + create/edit + activate/deactivate
- **Orders**
  - list + filter by status
  - update status
- **Customers**
  - list/search + view details (optional v2)
- **Users**
  - list + invite/edit + status
- **Roles & permissions**
  - roles list
  - permissions matrix
  - assign roles to users

---

## 🔌 6. Endpoints / services (initial API surface)

Even if this remains Blazor Server, we should still define a clean service surface (ports) to keep UI independent.

### 6.1 Storefront services
- `CatalogService`: get products, product details, categories
- `CartService`: get cart, add/update/remove items, apply discount
- `OrderService`: create order, list my orders
- `AuthService`: register, login, logout

### 6.2 Admin services
- `AdminDashboardService`
- `ProductAdminService`
- `CategoryAdminService`
- `DiscountAdminService`
- `OrderAdminService`
- `CustomerAdminService`
- `UserAdminService` + `RoleAdminService`

---

## 🧪 7. Mock-first workflow (how we will execute)

- **Mock**: each screen exists as a static prototype that references DTO fields (source: `docs/`)
- **DTOs**: create DTOs + mapping layer (AutoMapper) early
- **Ports/services**: define interfaces first, then implement with EF Core
- **UI**: build pages against ports with loading/error states
- **Seed**: create seed data so the mock data matches real DB data

---

## 🚀 8. Local setup (current stack)

### 8.1 Prerequisites
- Visual Studio / VS Code
- SQL Server
- .NET SDK (target framework in solution)

### 8.2 Database
Set `ConnectionStrings:connWebshop` in `appsettings.json`, then run:

```bash
dotnet ef database update
```

### 8.3 Run

```bash
cd WebSHop
dotnet run
```

---

## 🧭 9. Next steps (immediate)

- Extract the **real data contracts** and navigation from the live site (or API behind it).
- Replace the current mocks with a **vNext mock** driven by the DTOs above.
- Create a template structure for:
  - `Admin` area (authz-protected)
  - `Storefront` area (public + customer auth)

---

## Documentation

- 📋 `docs/index.html` - Entry point for UI prototypes
- 📋 `docs/mock-admin.html` - Admin prototype
- 📋 `docs/mock-shopcart.html` - Storefront + cart prototype

---

**© 2026 AdminSense. All rights reserved.**

