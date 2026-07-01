using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Audit;

public sealed class AuditSuppressionContext : IAuditSuppressionContext
{
    private readonly AsyncLocal<HashSet<string>?> _suppressed = new();

    public IDisposable SuppressEntityTypes(params string[] entityTypeNames)
    {
        var current = _suppressed.Value ?? new HashSet<string>(StringComparer.Ordinal);
        var next = new HashSet<string>(current, StringComparer.Ordinal);
        foreach (var name in entityTypeNames)
        {
            next.Add(name);
        }

        _suppressed.Value = next;
        return new Scope(this, current);
    }

    public bool IsSuppressed(string entityTypeName) =>
        _suppressed.Value?.Contains(entityTypeName) == true;

    private sealed class Scope : IDisposable
    {
        private readonly AuditSuppressionContext _context;
        private readonly HashSet<string>? _previous;
        private bool _disposed;

        public Scope(AuditSuppressionContext context, HashSet<string>? previous)
        {
            _context = context;
            _previous = previous;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _context._suppressed.Value = _previous;
            _disposed = true;
        }
    }
}
