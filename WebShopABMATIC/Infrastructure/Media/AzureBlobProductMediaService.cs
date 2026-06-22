using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Media;

public sealed class AzureBlobProductMediaService : IProductMediaPort
{
    private readonly WebShopABMATICDbContext _db;
    private readonly BlobContainerClient _container;
    private readonly AzureStorageOptions _options;
    private readonly IWebHostEnvironment _environment;
    private readonly IMemoryCache _cache;

    public AzureBlobProductMediaService(
        WebShopABMATICDbContext db,
        BlobServiceClient blobServiceClient,
        IOptions<AzureStorageOptions> options,
        IWebHostEnvironment environment,
        IMemoryCache cache)
    {
        _db = db;
        _options = options.Value;
        _environment = environment;
        _cache = cache;
        _container = blobServiceClient.GetBlobContainerClient(_options.ContainerName);
    }

    public async Task<string?> GetPrimaryImageUrlAsync(
        int productId,
        bool webPublishedOnly = false,
        CancellationToken cancellationToken = default)
    {
        var file = await FindPrimaryFileAsync(productId, webPublishedOnly, cancellationToken)
            ?? (webPublishedOnly
                ? await FindPrimaryFileAsync(productId, webPublishedOnly: false, cancellationToken)
                : null);

        if (file is null || string.IsNullOrWhiteSpace(file.BlobRef))
        {
            return null;
        }

        if (BlobReferenceResolver.IsAbsoluteUrl(file.BlobRef))
        {
            return file.BlobRef;
        }

        if (BlobReferenceResolver.IsLocalMediaPath(file.BlobRef))
        {
            return ResolveLocalMediaUrl(file.BlobRef);
        }

        var blobName = await ResolveReadableBlobNameAsync(file.BlobRef, file.Extension, file.ThumbRef, cancellationToken);
        return blobName is null ? null : await BuildReadUrlAsync(blobName, cancellationToken);
    }

    private async Task<string?> ResolveReadableBlobNameAsync(
        string blobRef,
        string? extension,
        string? thumbRef,
        CancellationToken cancellationToken)
    {
        foreach (var candidate in BlobReferenceResolver.BuildBlobNameCandidates(blobRef, extension))
        {
            if ((await _container.GetBlobClient(candidate).ExistsAsync(cancellationToken)).Value)
            {
                return candidate;
            }
        }

        if (!string.IsNullOrWhiteSpace(thumbRef)
            && (await _container.GetBlobClient(thumbRef).ExistsAsync(cancellationToken)).Value)
        {
            return thumbRef;
        }

        return null;
    }

    private async Task<FileRef?> FindPrimaryFileAsync(
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
            .Select(f => new FileRef(f.BlobRef, f.Extension, f.ThumbRef))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private sealed record FileRef(string? BlobRef, string? Extension, string? ThumbRef);

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
            extension = ".jpg";
        }

        var blobKey = BlobReferenceResolver.BuildLegacyBlobKey(extension);
        var blobClient = _container.GetBlobClient(blobKey);

        upload.Content.Position = 0;
        await blobClient.UploadAsync(upload.Content, overwrite: true, cancellationToken);

        var folderId = await EnsureProductFolderIdAsync(cancellationToken);
        var existing = await _db.AzureFiles
            .Where(f => f.ProductId == productId
                        && f.IsPrimaryImage == true
                        && (f.Deleted == null || f.Deleted == false))
            .OrderByDescending(f => f.Created)
            .FirstOrDefaultAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var displayName = string.IsNullOrWhiteSpace(upload.FileName) ? $"primary{extension}" : upload.FileName;

        if (existing is null)
        {
            _db.AzureFiles.Add(new AzureFile
            {
                Name = displayName,
                Extension = extension,
                AzureFileFolderId = folderId,
                Created = now,
                CreatedByUserId = createdByUserId,
                Description = "Primary product image",
                BlobRef = blobKey,
                ThumbRef = $"thumbnails/{blobKey}",
                ProductId = productId,
                IsPrimaryImage = true,
                PublishToWeb = publishToWeb,
                SendToCustomer = false,
                SendOnSupplierOrder = false
            });
        }
        else
        {
            existing.Name = displayName;
            existing.Extension = extension;
            existing.BlobRef = blobKey;
            existing.ThumbRef = $"thumbnails/{blobKey}";
            existing.Modified = now;
            existing.ModifiedByUserId = createdByUserId;
            existing.PublishToWeb = publishToWeb;
            existing.Deleted = false;
        }

        await _db.SaveChangesAsync(cancellationToken);
        _cache.Remove(BuildCacheKey(blobKey));
    }

    public async Task SetPrimaryImagePublishToWebAsync(
        int productId,
        bool publishToWeb,
        CancellationToken cancellationToken = default)
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

    private async Task<string> BuildReadUrlAsync(string blobName, CancellationToken cancellationToken)
    {
        if (!_options.UseSasUrls)
        {
            return _container.GetBlobClient(blobName).Uri.ToString();
        }

        var cacheKey = BuildCacheKey(blobName);
        if (_cache.TryGetValue(cacheKey, out string? cached) && !string.IsNullOrEmpty(cached))
        {
            return cached;
        }

        var blobClient = _container.GetBlobClient(blobName);
        if (!blobClient.CanGenerateSasUri)
        {
            throw new InvalidOperationException(
                "Azure Storage account key is required to generate SAS URLs for private blobs.");
        }

        var expiresOn = DateTimeOffset.UtcNow.AddHours(Math.Max(1, _options.SasValidityHours));
        var sas = new BlobSasBuilder
        {
            BlobContainerName = _container.Name,
            BlobName = blobName,
            Resource = "b",
            ExpiresOn = expiresOn
        };
        sas.SetPermissions(BlobSasPermissions.Read);

        var url = blobClient.GenerateSasUri(sas).ToString();
        _cache.Set(cacheKey, url, expiresOn.AddMinutes(-5));
        return url;
    }

    private string? ResolveLocalMediaUrl(string blobRef)
    {
        var relativePath = blobRef.StartsWith('/')
            ? blobRef
            : $"/{blobRef.TrimStart('/')}";

        var physicalPath = Path.Combine(
            _environment.WebRootPath,
            relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        return File.Exists(physicalPath) ? relativePath : null;
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

    private static string BuildCacheKey(string blobName) => $"azure-blob-sas:{blobName}";
}
