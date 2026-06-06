using System.Globalization;
using System.Text;
using Microsoft.JSInterop;

namespace WebShopABMATIC.Web.Services;

public sealed class GridExportService : IGridExportService
{
    private readonly IJSRuntime _js;

    public GridExportService(IJSRuntime js) => _js = js;

    public async Task ExportAsync(string format, GridExportRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(format);
        var normalized = format.Trim().ToLowerInvariant();

        if (normalized is "csv")
        {
            var csv = BuildCsv(request);
            var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray();
            var base64 = Convert.ToBase64String(bytes);
            await _js.InvokeVoidAsync("adminExport.downloadCsv", cancellationToken, $"{request.FileBaseName}.csv", base64);
            return;
        }

        if (normalized is "pdf")
        {
            var html = BuildPrintHtml(request);
            await _js.InvokeVoidAsync("adminExport.printPdf", cancellationToken, request.Title, html);
            return;
        }

        throw new ArgumentException("Format must be csv or pdf.", nameof(format));
    }

    private static string BuildCsv(GridExportRequest request)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", request.Headers.Select(EscapeCsv)));
        foreach (var row in request.Rows)
        {
            sb.AppendLine(string.Join(",", row.Select(EscapeCsv)));
        }

        return sb.ToString();
    }

    private static string EscapeCsv(string value)
    {
        if (value.Contains('"') || value.Contains(',') || value.Contains('\n') || value.Contains('\r'))
        {
            return $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
        }

        return value;
    }

    private static string BuildPrintHtml(GridExportRequest request)
    {
        var sb = new StringBuilder();
        sb.Append("<html><head><meta charset=\"utf-8\"><title>");
        sb.Append(System.Net.WebUtility.HtmlEncode(request.Title));
        sb.Append("</title><style>");
        sb.Append("body{font-family:Segoe UI,Arial,sans-serif;margin:24px;color:#1e2a3a;}");
        sb.Append("h1{font-size:18px;margin:0 0 16px;}");
        sb.Append("table{border-collapse:collapse;width:100%;font-size:12px;}");
        sb.Append("th,td{border:1px solid #dee2e6;padding:6px 8px;text-align:left;}");
        sb.Append("th{background:#212529;color:#fff;}");
        sb.Append("tr:nth-child(even){background:#f8f9fa;}");
        sb.Append("@media print{body{margin:12px;}}");
        sb.Append("</style></head><body>");
        sb.Append("<h1>").Append(System.Net.WebUtility.HtmlEncode(request.Title)).Append("</h1>");
        sb.Append("<table><thead><tr>");
        foreach (var header in request.Headers)
        {
            sb.Append("<th>").Append(System.Net.WebUtility.HtmlEncode(header)).Append("</th>");
        }

        sb.Append("</tr></thead><tbody>");
        foreach (var row in request.Rows)
        {
            sb.Append("<tr>");
            foreach (var cell in row)
            {
                sb.Append("<td>").Append(System.Net.WebUtility.HtmlEncode(cell)).Append("</td>");
            }

            sb.Append("</tr>");
        }

        sb.Append("</tbody></table></body></html>");
        return sb.ToString();
    }
}
