using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Data.Entities;

namespace WebShopABMATIC.Infrastructure.Audit;

internal static class AuditEntitySnapshot
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = false };

    public static string? SerializeCurrent(EntityEntry entry)
    {
        var data = CollectValues(entry, useOriginal: false);
        return data.Count == 0 ? null : JsonSerializer.Serialize(data, JsonOptions);
    }

    public static string? SerializeOriginal(EntityEntry entry)
    {
        var data = CollectValues(entry, useOriginal: true);
        return data.Count == 0 ? null : JsonSerializer.Serialize(data, JsonOptions);
    }

    public static string? SerializeModified(EntityEntry entry)
    {
        var data = new Dictionary<string, object?>();
        foreach (var prop in entry.Properties.Where(p => p.IsModified && !p.Metadata.IsShadowProperty()))
        {
            data[prop.Metadata.Name] = prop.CurrentValue;
        }

        return data.Count == 0 ? null : JsonSerializer.Serialize(data, JsonOptions);
    }

    public static string? SerializeModifiedOriginal(EntityEntry entry)
    {
        var data = new Dictionary<string, object?>();
        foreach (var prop in entry.Properties.Where(p => p.IsModified && !p.Metadata.IsShadowProperty()))
        {
            data[prop.Metadata.Name] = prop.OriginalValue;
        }

        return data.Count == 0 ? null : JsonSerializer.Serialize(data, JsonOptions);
    }

    public static string ResolveEntityId(EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();
        if (key is null)
        {
            return "";
        }

        var parts = key.Properties
            .Select(p => entry.Property(p.Name).CurrentValue?.ToString() ?? "")
            .Where(v => !string.IsNullOrEmpty(v));

        return string.Join("-", parts);
    }

    private static Dictionary<string, object?> CollectValues(EntityEntry entry, bool useOriginal)
    {
        var data = new Dictionary<string, object?>();
        foreach (var prop in entry.Properties.Where(p => !p.Metadata.IsShadowProperty()))
        {
            data[prop.Metadata.Name] = useOriginal ? prop.OriginalValue : prop.CurrentValue;
        }

        return data;
    }

    public static bool IsProductSoftDelete(EntityEntry entry) =>
        entry.Entity is Product
        && entry.State == EntityState.Modified
        && entry.Property(nameof(Product.IsInactive)).IsModified
        && entry.Entity is Product { IsInactive: true };
}
