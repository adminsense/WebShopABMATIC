namespace WebShopABMATIC.Web.Services;

public sealed class GridExportRequest
{
    public required string FileBaseName { get; init; }
    public required string Title { get; init; }
    public required IReadOnlyList<string> Headers { get; init; }
    public required IReadOnlyList<IReadOnlyList<string>> Rows { get; init; }
}

public static class GridExportBuilder
{
    public static GridExportRequest? FromRows(
        string fileBaseName,
        string title,
        IReadOnlyList<string> headers,
        IEnumerable<IReadOnlyList<string>>? rows)
    {
        var materialized = rows?.ToList();
        if (materialized is null || materialized.Count == 0)
        {
            return null;
        }

        return new GridExportRequest
        {
            FileBaseName = fileBaseName,
            Title = title,
            Headers = headers,
            Rows = materialized
        };
    }

    public static string Cell(object? value) => value switch
    {
        null => "",
        DateTime dt => dt.ToString("yyyy-MM-dd HH:mm:ss"),
        DateOnly d => d.ToString("yyyy-MM-dd"),
        bool b => b ? "Yes" : "No",
        _ => value.ToString() ?? ""
    };
}
