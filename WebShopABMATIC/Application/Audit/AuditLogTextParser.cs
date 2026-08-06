namespace WebShopABMATIC.Application.Audit;

/// <summary>
/// Parses action / entity id / success from <c>[Logging].[Error].Exception</c> text
/// written by <c>LegacyAuditWriter</c> / <c>LegacyAuditService</c>.
/// </summary>
public static class AuditLogTextParser
{
    private static readonly string[] ActionTokens =
        AuditActions.All.OrderByDescending(a => a.Length).ToArray();

    public static string ParseAction(string? exception, string? className)
    {
        var text = exception ?? string.Empty;
        if (text.StartsWith("Failed ", StringComparison.OrdinalIgnoreCase))
        {
            text = text["Failed ".Length..].TrimStart();
        }

        foreach (var action in ActionTokens)
        {
            if (text.StartsWith(action, StringComparison.OrdinalIgnoreCase))
            {
                return action;
            }
        }

        foreach (var action in ActionTokens)
        {
            if (string.Equals(className, action, StringComparison.OrdinalIgnoreCase))
            {
                return action;
            }
        }

        return string.IsNullOrWhiteSpace(className) ? "Unknown" : className!;
    }

    public static string? ParseEntityId(string? exception)
    {
        if (string.IsNullOrWhiteSpace(exception))
        {
            return null;
        }

        const string marker = " id=";
        var idx = exception.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
        {
            return null;
        }

        var start = idx + marker.Length;
        var end = start;
        while (end < exception.Length && (char.IsLetterOrDigit(exception[end]) || exception[end] is '-' or '_'))
        {
            end++;
        }

        return end > start ? exception[start..end] : null;
    }

    public static bool IsSuccess(string? exception) =>
        exception is null || !exception.Contains("failed", StringComparison.OrdinalIgnoreCase);
}
