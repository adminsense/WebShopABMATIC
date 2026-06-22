#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[StockOrderDeliveries] (legacy: [Products].[StockOrderLevering]).</summary>
public class StockOrderDelivery
{
    public int Id { get; set; }
    public int StockOrderDetail { get; set; }
    public string DeliveryDocumentNumber { get; set; }
    public DateTime Date { get; set; }
    public decimal Quantity { get; set; }
    public decimal QuantityInvoiced { get; set; }
}

