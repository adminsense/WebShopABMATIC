#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[JobSites] (legacy: [Projecten].[Werf]).</summary>
public class JobSite
{
    public string SiteStreet { get; set; }
    public string SiteBox { get; set; }
    public string SiteHouseNumber { get; set; }
    public int JobSiteId { get; set; }
    public int SiteCityId { get; set; }
    public string EndCustomerEmail { get; set; }
    public string EndCustomerMobile { get; set; }
    public string EndCustomerName { get; set; }
    public string EndCustomerPhone { get; set; }
    public string? SiteNotes { get; set; }
}

