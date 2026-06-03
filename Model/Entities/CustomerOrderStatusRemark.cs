#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerOrderStatusRemarks] (legacy: [Crm].[KlantDossierStatusOpmerking]).</summary>
public class CustomerOrderStatusRemark
{
    public int Id { get; set; }
    public int OrderStatusId { get; set; }
    public string Notes { get; set; }
    public int CustomerId { get; set; }
}

