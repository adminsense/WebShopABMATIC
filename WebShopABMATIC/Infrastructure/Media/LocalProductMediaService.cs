using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Media;

public sealed class LocalProductMediaService : IProductMediaPort
{
    private const string MediaRootSegment = "media/products";

    private readonly WebShopABMATICDbContext _db;
    private readonly IWebHostEnvironment _environment;

    public LocalProductMediaService(WebShopABMATICDbContext db, IWebHostEnvironment environment)
    {
        _db = db;
        _environment = environment;
    }

    public async Task<string?> GetPrimaryImageUrlAsync(int productId, bool webPublishedOnly = false, CancellationToken cancellationToken = default)
    {
        var blobRef = await QueryPrimaryBlobRefAsync(productId, webPublishedOnly, cancellationToken);
        return ResolvePublicUrl(blobRef);
    }

    public async Task<IReadOnlyDictionary<int, string>> GetPrimaryImageUrlsAsync(
        IReadOnlyList<int> productIds,
        bool webPublishedOnly = false,
        CancellationToken cancellationToken = default)
    {
        if (productIds.Count == 0)
        {
            return new Dictionary<int, string>();
        }

        var ids = productIds.Distinct().ToList();
        var rows = await _db.AzureFiles.AsNoTracking()
            .Where(f => f.ProductId != null
                        && ids.Contains(f.ProductId.Value)
                        && f.IsPrimaryImage == true
                        && (f.Deleted == null || f.Deleted == false))
            .Select(f => new
            {
                ProductId = f.ProductId!.Value,
                f.BlobRef,
                PublishToWeb = f.PublishToWeb == true,
                f.Created
            })
            .ToListAsync(cancellationToken);

        var result = new Dictionary<int, string>();
        foreach (var productId in ids)
        {
            var candidates = rows.Where(r => r.ProductId == productId).ToList();
            if (candidates.Count == 0)
            {
                continue;
            }

            var blobRef = webPublishedOnly
                ? candidates.Where(r => r.PublishToWeb).OrderByDescending(r => r.Created).Select(r => r.BlobRef).FirstOrDefault()
                  ?? candidates.OrderByDescending(r => r.Created).Select(r => r.BlobRef).FirstOrDefault()
                : candidates.OrderByDescending(r => r.Created).Select(r => r.BlobRef).FirstOrDefault();

            var url = ResolvePublicUrl(blobRef);
            if (!string.IsNullOrWhiteSpace(url))
            {
                result[productId] = url;
            }
        }

        return result;
    }

    private async Task<string?> QueryPrimaryBlobRefAsync(
        int productId,
        bool webPublishedOnly,
        CancellationToken cancellationToken)
    {
        var query = _db.AzureFiles.AsNoTracking()
            .Where(f => f.ProductId == productId
                        && f.IsPrimaryImage == true
                        && (f.Deleted == null || f.Deleted == false));

        if (webPublishedOnly)
        {
            query = query.Where(f => f.PublishToWeb == true);
        }

        return await query
            .OrderByDescending(f => f.Created)
            .Select(f => f.BlobRef)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task SavePrimaryImageAsync(
        int productId,
        ProductImageUpload upload,
        bool publishToWeb,
        int createdByUserId,
        CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(upload.FileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = ".png";
        }

        var safeFileName = $"primary{extension.ToLowerInvariant()}";
        var relativeUrl = $"/{MediaRootSegment}/{productId}/{safeFileName}";
        var logicalBlobRef = $"{MediaRootSegment}/{productId}/{safeFileName}";

        var directory = Path.Combine(_environment.WebRootPath, MediaRootSegment, productId.ToString());
        Directory.CreateDirectory(directory);

        var physicalPath = Path.Combine(directory, safeFileName);
        await using (var fileStream = File.Create(physicalPath))
        {
            await upload.Content.CopyToAsync(fileStream, cancellationToken);
        }

        var folderId = await EnsureProductFolderIdAsync(cancellationToken);
        var existing = await _db.AzureFiles
            .Where(f => f.ProductId == productId
                        && f.IsPrimaryImage == true
                        && (f.Deleted == null || f.Deleted == false))
            .OrderByDescending(f => f.Created)
            .FirstOrDefaultAsync(cancellationToken);

        var now = DateTime.UtcNow;
        if (existing is null)
        {
            _db.AzureFiles.Add(new AzureFile
            {
                Name = safeFileName,
                Extension = extension,
                AzureFileFolderId = folderId,
                Created = now,
                CreatedByUserId = createdByUserId,
                Description = "Primary product image",
                BlobRef = relativeUrl,
                ThumbRef = relativeUrl,
                ProductId = productId,
                IsPrimaryImage = true,
                PublishToWeb = publishToWeb,
                SendToCustomer = false,
                SendOnSupplierOrder = false
            });
        }
        else
        {
            existing.Name = safeFileName;
            existing.Extension = extension;
            existing.BlobRef = relativeUrl;
            existing.ThumbRef = relativeUrl;
            existing.Modified = now;
            existing.ModifiedByUserId = createdByUserId;
            existing.PublishToWeb = publishToWeb;
            existing.Deleted = false;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task SetPrimaryImagePublishToWebAsync(int productId, bool publishToWeb, CancellationToken cancellationToken = default)
    {
        var existing = await _db.AzureFiles
            .Where(f => f.ProductId == productId
                        && f.IsPrimaryImage == true
                        && (f.Deleted == null || f.Deleted == false))
            .ToListAsync(cancellationToken);

        if (existing.Count == 0)
        {
            return;
        }

        foreach (var file in existing)
        {
            file.PublishToWeb = publishToWeb;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> EnsureProductFolderIdAsync(CancellationToken cancellationToken)
    {
        var folder = await _db.AzureFileFolders
            .FirstOrDefaultAsync(f => f.IsForProduct, cancellationToken);

        if (folder is not null)
        {
            return folder.Id;
        }

        folder = new AzureFileFolder
        {
            Name = "Products",
            IsForCrm = false,
            IsForOrder = false,
            IsForProject = false,
            IsForProduct = true,
            IsForUser = false,
            IsForGeneralUse = false,
            SortOrder = 1
        };
        _db.AzureFileFolders.Add(folder);
        await _db.SaveChangesAsync(cancellationToken);
        return folder.Id;
    }

    private string? ResolvePublicUrl(string? blobRef)
    {
        if (string.IsNullOrWhiteSpace(blobRef))
        {
            return null;
        }

        if (blobRef.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || blobRef.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return blobRef;
        }

        var relativePath = blobRef.StartsWith('/')
            ? blobRef
            : $"/{blobRef.TrimStart('/')}";

        if (!relativePath.StartsWith("/media/", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var physicalPath = Path.Combine(
            _environment.WebRootPath,
            relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        return File.Exists(physicalPath) ? relativePath : null;
    }
}
