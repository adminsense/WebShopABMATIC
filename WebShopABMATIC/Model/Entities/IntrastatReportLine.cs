#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Accounting].[IntrastatReportLines] (legacy: [Boekhouding].[IntrastatReportLine]).</summary>
public class IntrastatReportLine
{
    public int Id { get; set; }
    public int? DocumentDocId { get; set; }
    public string ProductName { get; set; }
    public string PartnerLand { get; set; }
    public string TransactieCode { get; set; }
    public string Gewest { get; set; }
    public string GoodsCode { get; set; }
    public decimal Weight { get; set; }
    public decimal AanvullendeEenheden { get; set; }
    public decimal WaardeInEur { get; set; }
    public string? Vervoer { get; set; }
    public string? Incoterm { get; set; }
    public string LandVanOorsprong { get; set; }
    public string BtwNrTegenpartij { get; set; }
}

