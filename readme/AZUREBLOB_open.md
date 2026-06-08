# Product media — Azure Files model (legacy-aligned)

![Status](https://img.shields.io/badge/Status-Phase%201%20dev%20done-28a745?style=flat-square) ![Storage](https://img.shields.io/badge/Phase%201-Local%20fictitious%20blob-0dcaf0?style=flat-square) ![Tables](https://img.shields.io/badge/Tables-AzureFiles%20%2B%20Product-512BD4?style=flat-square) ![Legacy](https://img.shields.io/badge/Model-ABMATIC%20aligned-28a745?style=flat-square)

> [!IMPORTANT]
> **Executive Summary:** Product images in WebShopABMATIC follow the **legacy ABMATIC pattern**: files are registered in `[Files].[AzureFiles]` and linked to `[Products].[Product]` via `ProductId` — not via a column on `Product`. Phase 1 uses a **fictitious Azure Blob** (local filesystem under `wwwroot/media/products/`) with the same `BlobRef` contract, so admin forms, seed data, and the storefront can be built now and swapped to real Azure Blob Storage later without changing the database model.

### Coverage statistics

| Category | Count | Status | Notes |
|----------|-------|--------|-------|
| **Legacy tables** | 2 | ✅ In schema | `AzureFiles`, `AzureFileFolders` |
| **Product link** | 1 | ✅ Designed | `AzureFiles.ProductId` (logical, no FK) |
| **Admin form upload** | 1 | ✅ Done | `ProductForm` + media port |
| **Store image source** | 1 | ✅ Done | `StoreCatalogService` via `IProductMediaPort` |
| **Seed demo rows** | 10 | ✅ Done | All `ShowOnWebshop` products in `seeds.sql` |

### Implementation quality

| Aspect | Status | Details |
|--------|--------|---------|
| **EF entities** | ✅ Complete | `AzureFile`, `AzureFileFolder` mapped |
| **DB on MULLER** | ✅ Seeded | `AzureFileFolders` + `AzureFiles` via `seeds.sql` |
| **Admin save** | ✅ Wired | `ProductAdminUseCase` + `IProductMediaPort` (local blob Phase 1) |
| **Store catalog** | ✅ Done | `StoreCatalogService` via `IProductMediaPort`; static fallback if no row |
| **Real Azure Blob** | ⏳ Phase 2 | Replace storage adapter only |

---

## 1. Legacy model (how ABMATIC linked products to files)

### 1.1 Relationship direction

The link is **from file → product**, not from product → file:

```text
Products.Product (ProductId)
        ↑
        │  ProductId (nullable int)
        │
Files.AzureFiles
  ├── BlobRef          → blob key or public URL
  ├── ThumbRef         → thumbnail (optional)
  ├── IsPrimaryImage   → catalog hero image
  ├── PublishToWeb     → visible on storefront
  └── AzureFileFolderId → folder (Files.AzureFileFolders)
```

| Design choice | Legacy behaviour |
|---------------|------------------|
| Column on `Product` for image | **No** — no `ImageUrl`, no `AzureFileId` on `Product` |
| Foreign key `AzureFiles → Product` | **No** — logical link only; app enforces integrity |
| One product, many files | **Yes** — multiple `AzureFiles` per `ProductId` |
| Storefront image | Row with `IsPrimaryImage = 1` and `PublishToWeb = 1` |

### 1.2 Other file tables (not used for catalog images)

| Table | Role | Product catalog? |
|-------|------|----------------|
| `Files.StoredFiles` | Binary `varbinary(max)` in SQL Server | ❌ Attachments (orders, emails) |
| `Products.ProductManuals` | `ProductId` + `Path` for PDFs/manuals | ❌ Manuals, not shop photos |
| `Files.AzureFileFolders` | Organises `AzureFiles` (`IsForProduct`, etc.) | ✅ Required parent for product files |

---

## 2. Current state in WebShopABMATIC vNext

### 2.1 What exists today

| Layer | Behaviour |
|-------|-----------|
| **Database** | `Files.AzureFiles` seeded for all webshop products (`ShowOnWebshop = 1`) |
| **Admin `ProductForm`** | Image upload + preview via `IProductMediaPort` |
| **`ProductEditDto`** | `PrimaryImageUrl` for preview |
| **Store `StoreCatalog`** | Reads primary image from `AzureFiles`; fallback `/images/productN.png` |
| **`seeds.sql`** | `AzureFileFolders` (id=1 Products) + `AzureFiles` per webshop SKU |

### 2.2 Why align with legacy instead of a new column

- Same contract as the original ABMATIC ERP and existing schema.
- Supports multiple files per product (gallery, manuals, datasheets) without schema churn.
- `BlobRef` already abstracts storage — local path today, Azure container tomorrow.

---

## 3. Fictitious Azure Blob (Phase 1)

No Azure subscription is required for the first implementation. **Behaviour and table shape match production**; only the storage backend is local.

### 3.1 Local storage layout

| Item | Value |
|------|--------|
| **Physical path** | `Web/wwwroot/media/products/{productId}/` |
| **Public URL** | `/media/products/{productId}/{fileName}` |
| **`BlobRef` value** | Logical key, e.g. `products/42/primary.png` or the public URL above |
| **`ThumbRef`** | Same file initially, or omitted until thumbnail generation exists |

### 3.2 Storage adapter (planned)

```text
IProductMediaPort
  ├── SavePrimaryImageAsync(productId, stream, fileName) → AzureFiles row + file on disk
  ├── GetPrimaryImageAsync(productId) → BlobRef / public URL
  └── DeletePrimaryImageAsync(productId) → soft-delete or replace row

LocalProductMediaStorage   ← Phase 1 (fictitious blob)
AzureBlobProductMediaStorage ← Phase 2 (real SDK)
```

Swapping Phase 1 → Phase 2 changes **only** the infrastructure adapter; `AzureFiles`, DTOs, and UI stay the same.

### 3.3 Minimum `AzureFiles` row per product image

| Column | Typical value |
|--------|----------------|
| `ProductId` | `Products.Product.ProductId` |
| `Name` | Original or display file name |
| `Extension` | `.jpg`, `.png`, `.webp` |
| `AzureFileFolderId` | Seed folder “Products” (`IsForProduct = 1`) |
| `BlobRef` | Fictitious blob key or `/media/products/...` URL |
| `ThumbRef` | Optional thumbnail key |
| `IsPrimaryImage` | `true` for catalog hero |
| `PublishToWeb` | `true` when shown on storefront |
| `Description` | Short label or empty string |
| `Created` | UTC timestamp |
| `CreatedByUserId` | Current staff user id |
| `SendToCustomer` / `SendOnSupplierOrder` | `false` for catalog images |

### 3.4 Seed folder (`AzureFileFolders`)

One demo folder satisfies `AzureFileFolderId` NOT NULL semantics:

| Field | Value |
|-------|--------|
| `Name` | `Products` |
| `IsForProduct` | `true` |
| Other `IsFor*` flags | `false` |
| `SortOrder` | `1` |

Demo seed can link HDD 1–6 `AzureFiles` rows to seeded `ProductId` values with `BlobRef` pointing at existing mock assets (`/images/product1.png`, etc.) or copied files under `/media/products/`.

---

## 4. Planned application changes

### 4.1 Admin — product form

| Step | Create product | Edit product |
|------|----------------|--------------|
| 1 | Save `Product` → obtain `ProductId` | Load `Product` + primary `AzureFiles` |
| 2 | If upload present → save file locally | Show image preview from `BlobRef` |
| 3 | Insert `AzureFiles` with `ProductId`, flags | Replace file → update or supersede row |
| 4 | — | Clear upload → optional delete / unpublish |

**UI additions:** `InputFile`, preview `<img>`, validation (size, extension).

### 4.2 Application layer

| Artifact | Purpose |
|----------|---------|
| `ProductEditDto.PrimaryImageUrl` | Read-only preview for form |
| `IProductMediaPort` | Upload, resolve URL, publish flag (outbound port) |
| `ProductAdminUseCase` | Orchestrate product domain + media on save |

### 4.3 Storefront

Replace hardcoded image paths in `StoreCatalogService`:

```sql
-- Primary storefront image (conceptual query)
SELECT BlobRef
FROM Files.AzureFiles
WHERE ProductId = @id
  AND IsPrimaryImage = 1
  AND PublishToWeb = 1
  AND (Deleted IS NULL OR Deleted = 0)
```

Map `BlobRef` to a browser URL (local `/media/...` or future SAS URL).

### 4.4 Seed script (`scripts/seeds.sql`)

After `Products` insert:

1. Insert `AzureFileFolders` (id = 1, Products).
2. For each webshop SKU (`ShowOnWebshop = 1`), insert `AzureFiles` with matching `ProductId`, `IsPrimaryImage = 1`, `PublishToWeb = 1`, `BlobRef = '/images/productN.png'` (cycles 1–6 for accessories/services).

---

## 5. Data flow (end-to-end)

```mermaid
flowchart LR
  subgraph Admin
    Form[ProductForm upload]
    Svc[ProductAdminUseCase]
    Media[LocalProductMediaStorage]
  end

  subgraph DB
    Product[Products.Product]
    Azure[Files.AzureFiles]
    Folder[Files.AzureFileFolders]
  end

  subgraph Store
    Catalog[Store catalog query]
    Img[img src from BlobRef]
  end

  Form --> Svc
  Svc --> Product
  Svc --> Media
  Media --> Azure
  Azure --> Product
  Azure --> Folder
  Catalog --> Azure
  Catalog --> Img
```

---

## 6. Phase 2 — real Azure Blob Storage

| Concern | Phase 1 (now) | Phase 2 (production) |
|---------|---------------|----------------------|
| Bytes on disk | `wwwroot/media/products/` | Azure Blob container |
| `BlobRef` | Local key or site URL | Container + blob name |
| Thumbnails | Optional copy / skip | Azure Functions or SDK resize |
| CDN | Static files middleware | Azure CDN + SAS or public container |
| Configuration | None | `AzureStorage:ConnectionString`, container name |

**No migration** of `AzureFiles` rows expected — only `BlobRef` values may be rewritten if blobs are uploaded to Azure.

---

## 7. Scope and limitations (Phase 1)

| In scope | Out of scope (later) |
|----------|----------------------|
| One **primary** image per product | Multi-image gallery UI |
| Local fictitious blob | Real Azure SDK upload |
| `AzureFiles` + `PublishToWeb` | `StoredFiles` binary-in-SQL for catalog |
| Admin create/edit + seed + store read | Image cropping, virus scan, CDN rules |

---

## 8. Implementation order (recommended)

1. **Document** — this file ✅  
2. **Seed** — `AzureFileFolders` + `AzureFiles` in `seeds.sql` for demo products ✅  
3. **Infrastructure** — `IProductMediaPort` + `LocalProductMediaService` ✅  
4. **Admin** — extend DTO, service, `ProductForm` upload ✅  
5. **Store** — catalog/detail read image from `AzureFiles` ✅  
6. **Docs** — update [DATA_DEMO_SEED.md](DATA_DEMO_SEED.md) and [SPEC_INFRASTRUCTURE.md](SPEC_INFRASTRUCTURE.md) media section ✅  
7. **Phase 2** — real Azure Blob adapter (prod go-live, last) ⬜

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**
