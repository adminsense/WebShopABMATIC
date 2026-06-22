#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderRemarks] (legacy: [Projecten].[DossierOpmerking]).</summary>
public class OrderRemark
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int? CustomerNoteId { get; set; }
    public string? ProductionCategories { get; set; }
    public string Notes { get; set; }
    public int? ProductionCategoryId { get; set; }
    public int? DocumentTypeId { get; set; }
}

