#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerProductDiscounts] (legacy: [Crm].[KlantProductKorting]).</summary>
public class CustomerProductDiscount
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public int ProductId { get; set; }
    public string? Notes { get; set; }
    public DateTime FromAddress { get; set; }
    public DateTime? ValidTo { get; set; }
    public int? CustomerTypeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public decimal? Margin { get; set; }
    public decimal? InstallationDiscountPercentage { get; set; }
    public int? InstallationCustomerTypeId { get; set; }
}

