#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Accounting].[CustomerExtraDiscounts] (legacy: [Boekhouding].[KlantExtraKortingen]).</summary>
public class CustomerExtraDiscount
{
    public int Id { get; set; }
    public decimal Discount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal MinAmount { get; set; }
    public int CustomerTypeFilterId { get; set; }
    public int? BaseCompanyId { get; set; }
}

