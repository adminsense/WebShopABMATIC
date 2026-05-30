#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Files].[AzureFiles] (legacy: [Bestanden].[AzureFile]).</summary>
public class AzureFile
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public int AzureFileFolderId { get; set; }
    public DateTime Created { get; set; }
    public int CreatedByUserId { get; set; }
    public string Description { get; set; }
    public string BlobRef { get; set; }
    public string? ThumbRef { get; set; }
    public DateTime? Modified { get; set; }
    public int? ModifiedByUserId { get; set; }
    public int? OrderId { get; set; }
    public int? ProjectId { get; set; }
    public int? EmailId { get; set; }
    public int? CustomerId { get; set; }
    public bool? Deleted { get; set; }
    public int? DeletedByUserId { get; set; }
    public int? ProductId { get; set; }
    public bool? IsPrimaryImage { get; set; }
    public bool? PublishToWeb { get; set; }
    public int? UserId { get; set; }
    public bool? IsGeneral { get; set; }
    public int? SupplierId { get; set; }
    public int? ManufacturerId { get; set; }
    public int? OrderLineId { get; set; }
    public bool? IsLinkedRef { get; set; }
    public bool SendToCustomer { get; set; }
    public bool SendOnSupplierOrder { get; set; }
    public int? StockOrderId { get; set; }
}

