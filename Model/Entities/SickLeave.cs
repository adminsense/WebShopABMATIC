#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Users].[SickLeaves] (legacy: [Users].[Ziekte]).</summary>
public class SickLeave
{
    public int Id { get; set; }
    public DateTime FromAddress { get; set; }
    public DateTime ValidTo { get; set; }
    public long? SickNoteAzureFileId { get; set; }
    public int UserId { get; set; }
    public int? CalendarEntryId { get; set; }
}

