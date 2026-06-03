#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Emails].[EmailAttachments] (legacy: [Emails].[Bijlage]).</summary>
public class EmailAttachment
{
    public int StoredFileId { get; set; }
    public int EmailId { get; set; }
    public int Id { get; set; }
    public string EmailFileName { get; set; }
    public bool IsEmailOnlyFile { get; set; }
}

