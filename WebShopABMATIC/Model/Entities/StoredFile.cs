#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Files].[StoredFiles] (legacy: [Bestanden].[Bestand]).</summary>
public class StoredFile
{
    public int StoredFileId { get; set; }
    public string FileName { get; set; }
    public int FileType { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public int CreatedBy { get; set; }
    public byte[] Data { get; set; }
}

