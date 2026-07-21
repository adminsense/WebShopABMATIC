# Product media — Azure Blob (legacy-aligned)

![Status](https://img.shields.io/badge/Status-Complete-28a745?style=flat-square) ![Storage](https://img.shields.io/badge/Storage-Azure%20Blob%20%60files%60-0dcaf0?style=flat-square) ![Tables](https://img.shields.io/badge/Tables-Bestanden.AzureFile-512BD4?style=flat-square) ![Legacy](https://img.shields.io/badge/Model-ABMATIC%20aligned-28a745?style=flat-square)

> [!IMPORTANT]
> **Executive Summary:** Product images follow the **legacy ABMATIC pattern**: metadata in `[Bestanden].[AzureFile]`, linked to `[Products].[Product]` via `ProductId`. Bytes live in Azure Blob Storage (account `abmatic`, container `files`). The storefront resolves `BlobRef` to **SAS URLs** for a private container. When `AzureStorage:ConnectionString` is empty, the app falls back to `LocalProductMediaService` (local `wwwroot/media/`).

### Coverage statistics

<table>
<colgroup>
<col style="width:24%">
<col style="width:8%">
<col style="width:14%">
<col style="width:54%">
</colgroup>
<thead>
<tr><th>Category</th><th>Count</th><th>Status</th><th>Notes</th></tr>
</thead>
<tbody>
<tr><td><strong>Legacy tables</strong></td><td>2</td><td>✅</td><td><code>AzureFiles</code>, <code>AzureFileFolders</code> (EF → <code>Bestanden</code>)</td></tr>
<tr><td><strong>Product link</strong></td><td>1</td><td>✅</td><td><code>AzureFiles.ProductId</code> (logical, no FK)</td></tr>
<tr><td><strong>Admin form upload</strong></td><td>1</td><td>✅</td><td><code>ProductForm</code> + <code>IProductMediaPort</code></td></tr>
<tr><td><strong>Store image source</strong></td><td>1</td><td>✅</td><td><code>StoreCatalogService</code> via SAS blob URLs</td></tr>
<tr><td><strong>Azure Blob adapter</strong></td><td>1</td><td>✅</td><td><code>AzureBlobProductMediaService</code></td></tr>
<tr><td><strong>HTTP verification</strong></td><td>11/12</td><td>✅</td><td>Sample catalog products on <code>abmatic_test</code> (1 SKU without <code>AzureFile</code> row)</td></tr>
</tbody>
</table>

### Implementation quality

<table>
<colgroup>
<col style="width:32%">
<col style="width:14%">
<col style="width:54%">
</colgroup>
<thead>
<tr><th>Aspect</th><th>Status</th><th>Details</th></tr>
</thead>
<tbody>
<tr><td><strong>EF entities</strong></td><td>✅</td><td><code>AzureFile</code>, <code>AzureFileFolder</code></td></tr>
<tr><td><strong>Production storage</strong></td><td>✅</td><td>Account <code>abmatic</code>, container <code>files</code></td></tr>
<tr><td><strong>Blob name resolution</strong></td><td>✅</td><td><code>BlobRef</code> as-is (no appended <code>Extension</code>)</td></tr>
<tr><td><strong>Private container</strong></td><td>✅</td><td>SAS read URLs, 12h cache</td></tr>
<tr><td><strong>Dev fallback</strong></td><td>✅</td><td><code>LocalProductMediaService</code> when no connection string</td></tr>
<tr><td><strong>Catalog performance</strong></td><td>✅</td><td>Home page loads 12 products; existence check before SAS</td></tr>
</tbody>
</table>

---

## 🖼️ 1. Legacy Model (How ABMATIC Linked Products to Files)

### 🔗 1.1 Relationship Direction

```text
Products.Product (ProdId)
        ↑
        │  ProductId (nullable int)
        │
Bestanden.AzureFile
  ├── BlobRef          → blob object name in container (no extension in key)
  ├── Extension        → .jpg / .PNG (metadata only)
  ├── ThumbRef         → thumbnails/{BlobRef} (optional)
  ├── IsPrimaryImage   → catalog hero image
  ├── PublishToWeb     → visible on storefront
  └── AzureFileFolderId
```

<table>
<colgroup>
<col style="width:38%">
<col style="width:62%">
</colgroup>
<thead>
<tr><th>Design choice</th><th>Legacy behaviour</th></tr>
</thead>
<tbody>
<tr><td>Column on <code>Product</code> for image</td><td><strong>No</strong></td></tr>
<tr><td>Foreign key <code>AzureFiles → Product</code></td><td><strong>No</strong> — logical link only</td></tr>
<tr><td>Storefront image</td><td><code>IsPrimaryImage = 1</code> and preferably <code>PublishToWeb = 1</code> (fallback without flag if missing)</td></tr>
</tbody>
</table>

### 1.2 Storage types (do not confuse)

<table>
<colgroup>
<col style="width:32%">
<col style="width:38%">
<col style="width:30%">
</colgroup>
<thead>
<tr><th>Resource</th><th>Role</th><th>Catalog images?</th></tr>
</thead>
<tbody>
<tr><td><strong>Blob</strong> <code>abmatic</code> / <code>files</code></td><td>Product image bytes</td><td>✅ Yes</td></tr>
<tr><td><strong>File Share</strong> <code>abmaticprojecten</code> / <code>projecten</code></td><td>ERP project files</td><td>❌ No</td></tr>
<tr><td><code>Bestanden.StoredFiles</code></td><td>Binary in SQL</td><td>❌ Attachments</td></tr>
</tbody>
</table>

> Client message “blobstorage” meant **Blob Storage** (not File Share), not a container named <code>blobstorage</code>. On account <code>abmatic</code> the real containers are <code>files</code> and <code>productdata</code>; product images are in <code>files</code>.

---

## ☁️ 2. Production Integration (ABMATIC — Verified)

<table>
<colgroup>
<col style="width:28%">
<col style="width:72%">
</colgroup>
<thead>
<tr><th>Item</th><th>Value</th></tr>
</thead>
<tbody>
<tr><td><strong>SQL database</strong></td><td><code>abmatic_test</code> on <code>abmatic.database.windows.net</code></td></tr>
<tr><td><strong>Image metadata</strong></td><td><code>[Bestanden].[AzureFile]</code></td></tr>
<tr><td><strong>Storage account</strong></td><td><code>abmatic</code></td></tr>
<tr><td><strong>Blob container</strong></td><td><code>files</code></td></tr>
<tr><td><strong>Blob endpoint</strong></td><td><code>https://abmatic.blob.core.windows.net</code></td></tr>
</tbody>
</table>

### 📝 `BlobRef` Format (Verified)

<table>
<colgroup>
<col style="width:18%">
<col style="width:42%">
<col style="width:40%">
</colgroup>
<thead>
<tr><th>ProductId</th><th>BlobRef (object name)</th><th>Extension (DB)</th></tr>
</thead>
<tbody>
<tr><td>11234</td><td><code>19-10-2022-10-58-4567322bc1-30f9-4f2e-afb7-aa07c44ffda5</code></td><td><code>.jpg</code></td></tr>
<tr><td>23729</td><td><code>4-7-2025-03-27-298b89a764-338e-41c4-b338-9e71d54842be</code></td><td><code>.png</code></td></tr>
</tbody>
</table>

- Blob object name = **`BlobRef` exactly** (extension is **not** part of the blob key).
- `Extension` column is metadata only.
- Storefront URL pattern: <code>https://abmatic.blob.core.windows.net/files/{BlobRef}?sv=…</code> (SAS).

---

## 🏛️ 3. Application Architecture

```text
IProductMediaPort
  ├── AzureBlobProductMediaService   ← when AzureStorage:ConnectionString is set
  └── LocalProductMediaService       ← dev fallback (/media/...)

ProductMediaDependencyInjection
  └── registers Azure SDK + BlobServiceClient when configured

BlobReferenceResolver
  └── ResolvePreferredBlobName(blobRef) → blobRef.Trim() (legacy rule)
```

### ⚙️ Configuration

**Azure App Service** (never commit secrets):

```json
"AzureStorage": {
  "ConnectionString": "<DefaultEndpointsProtocol=https;AccountName=abmatic;AccountKey=…>",
  "ContainerName": "files",
  "UseSasUrls": true,
  "SasValidityHours": 12
}
```

### 💻 IIS / Local Run

- `web.config` → `.\bin\Debug\net10.0\WebShopABMATIC.dll`
- Port **5090** (`launchSettings.json`)
- After code changes: rebuild; do not commit DLLs from `Web/` root (see `.gitignore`)

---

## 🛍️ 4. Storefront Behaviour (Implemented)

<table>
<colgroup>
<col style="width:28%">
<col style="width:72%">
</colgroup>
<thead>
<tr><th>Behaviour</th><th>Detail</th></tr>
</thead>
<tbody>
<tr><td><strong>Catalog load</strong></td><td>12 products on home page (<code>GetCatalogAsync(take: 12)</code>)</td></tr>
<tr><td><strong>Image URL</strong></td><td>SAS from <code>AzureBlobProductMediaService</code>; static <code>/images/productN.png</code> only if no blob row</td></tr>
<tr><td><strong>Publish filter</strong></td><td><code>PublishToWeb = 1</code> first; falls back to any primary image</td></tr>
<tr><td><strong>Blob existence</strong></td><td><code>ExistsAsync</code> before generating SAS (avoids broken URLs)</td></tr>
</tbody>
</table>

Conceptual query:

```sql
SELECT BlobRef, Extension
FROM Bestanden.AzureFile
WHERE ProductId = @id
  AND IsPrimaryImage = 1
  AND (Deleted IS NULL OR Deleted = 0)
ORDER BY PublishToWeb DESC, Created DESC
```

---

## ✅ 5. Implementation Checklist (Complete)

| # | Task | Status |
|---|------|--------|
| 1 | Document legacy model + client storage facts | ✅ |
| 2 | `AzureFileFolders` + `AzureFiles` rows on `abmatic_test` | ✅ |
| 3 | `IProductMediaPort` + local adapter | ✅ |
| 4 | Admin upload + preview | ✅ |
| 5 | Store catalog/detail read via port | ✅ |
| 6 | Azure SDK adapter + `AzureStorage` config | ✅ |
| 7 | Container `files` + `BlobRef` naming verified | ✅ |
| 8 | SAS URLs + existence check | ✅ |
| 9 | HTTP smoke test (11/12 sample SKUs) | ✅ |

---

## 🔮 6. Out of Scope (Future)

| Item | Notes |
|------|--------|
| Multi-image gallery UI | Single primary image only |
| Thumbnail generation | `ThumbRef` exists in DB; storefront uses full `BlobRef` |
| Azure CDN | SAS direct to blob endpoint |
| Image crop / virus scan | Admin upload only |
| Products without `AzureFile` row | Show placeholder; fix data in ERP |

---

## 📚 7. Key Source Files

| File | Role |
|------|------|
| `Infrastructure/Media/AzureBlobProductMediaService.cs` | Blob read/write + SAS |
| `Infrastructure/Media/BlobReferenceResolver.cs` | Legacy blob key rules |
| `Infrastructure/Media/ProductMediaDependencyInjection.cs` | Azure vs local registration |
| `Infrastructure/Media/AzureStorageOptions.cs` | Config section `AzureStorage` |
| `Infrastructure/Store/StoreCatalogService.cs` | Catalog calls `IProductMediaPort` |
| `Web/appsettings.json` | `ContainerName: files` (secrets local only) |

---

## Documentation

- 🏠 [Main Documentation](../README.md)

---

**© 2026 AdminSense. All rights reserved.**
