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
        var query = _db.AzureFiles.AsNoTracking()
            .Where(f => f.ProductId == productId
                        && f.IsPrimaryImage == true
                        && (f.Deleted == null || f.Deleted == false));

        if (webPublishedOnly)
        {
            query = query.Where(f => f.PublishToWeb == true);
        }

        var blobRef = await query
            .OrderByDescending(f => f.Created)
            .Select(f => f.BlobRef)
            .FirstOrDefaultAsync(cancellationToken);

        return ResolvePublicUrl(blobRef);
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

    private static string? ResolvePublicUrl(string? blobRef)
    {
        if (string.IsNullOrWhiteSpace(blobRef))
        {
            return null;
        }

        if (blobRef.StartsWith('/'))
        {
            return blobRef;
        }

        return $"/{blobRef.TrimStart('/')}";
    }
}
