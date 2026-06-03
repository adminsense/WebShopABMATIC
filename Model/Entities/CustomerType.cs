#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Customers].[CustomerTypes] (legacy: [Klanten].[KlantType]).</summary>
public class CustomerType
{
    public int KlantTypeId { get; set; }
    public string CustomerTypeName { get; set; }
    public bool RequiresVatNumber { get; set; }
    public int PaymentTermId { get; set; }
    public int VatSystemId { get; set; }
    public decimal BaseDiscount { get; set; }
    public int DeliveryTypeId { get; set; }
    public string CustomerTypeNameFr { get; set; }
    public int SortOrder { get; set; }
    public bool? IsDefault { get; set; }
}

