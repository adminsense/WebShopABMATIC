#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Files].[OrderFileLinks] (legacy: [Bestanden].[DossierBestanden]).</summary>
public class OrderFileLink
{
    public int StoredFileId { get; set; }
    public int OrderId { get; set; }
    public int OrderFileLinkId { get; set; }
}

