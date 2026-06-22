#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Logging].[AppErrors] (legacy: [Logging].[Error]).</summary>
public class AppError
{
    public DateTime DateTime { get; set; }
    public string ModuleName { get; set; }
    public string Exception { get; set; }
    public string InnerExceptionMessage { get; set; }
    public string UserName { get; set; }
    public string ClassName { get; set; }
    public long Id { get; set; }
}

