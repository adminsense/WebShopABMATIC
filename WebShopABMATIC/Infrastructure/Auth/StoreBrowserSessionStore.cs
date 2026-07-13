using System.Collections.Concurrent;

namespace WebShopABMATIC.Infrastructure.Auth;

/// <summary>
/// Server-side storefront browser session. Survives Chrome cookie/sessionStorage restore
/// only while this process still holds the id — closing the browser suspends then ends it.
/// </summary>
public interface IStoreBrowserSessionStore
{
    string Start();
    bool IsActive(string sessionId);
    void Touch(string sessionId);
    void Suspend(string sessionId);
    void Resume(string sessionId);
    void End(string sessionId);
}

public sealed class StoreBrowserSessionStore : IStoreBrowserSessionStore
{
    /// <summary>Grace after last tab unload so same-tab forceLoad navigations can resume.</summary>
    private static readonly TimeSpan SuspendGrace = TimeSpan.FromSeconds(45);

    /// <summary>Hard idle without any touch/ping (aligned with cookie idle).</summary>
    private static readonly TimeSpan AbsoluteIdle = TimeSpan.FromMinutes(15);

    private readonly ConcurrentDictionary<string, Entry> _sessions = new(StringComparer.Ordinal);

    public string Start()
    {
        var id = Guid.NewGuid().ToString("N");
        _sessions[id] = new Entry { LastTouchUtc = DateTime.UtcNow };
        return id;
    }

    public bool IsActive(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId) || !_sessions.TryGetValue(sessionId, out var entry))
        {
            return false;
        }

        if (DateTime.UtcNow - entry.LastTouchUtc > AbsoluteIdle)
        {
            End(sessionId);
            return false;
        }

        return true;
    }

    public void Touch(string sessionId)
    {
        if (!_sessions.TryGetValue(sessionId, out var entry))
        {
            return;
        }

        entry.LastTouchUtc = DateTime.UtcNow;
    }

    public void Suspend(string sessionId)
    {
        if (!_sessions.TryGetValue(sessionId, out var entry))
        {
            return;
        }

        entry.SuspendCts?.Cancel();
        entry.SuspendCts?.Dispose();
        var cts = new CancellationTokenSource();
        entry.SuspendCts = cts;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(SuspendGrace, cts.Token).ConfigureAwait(false);
                End(sessionId);
            }
            catch (OperationCanceledException)
            {
                // Resumed or replaced.
            }
            finally
            {
                cts.Dispose();
            }
        });
    }

    public void Resume(string sessionId)
    {
        if (!_sessions.TryGetValue(sessionId, out var entry))
        {
            return;
        }

        if (entry.SuspendCts is not null)
        {
            entry.SuspendCts.Cancel();
            entry.SuspendCts.Dispose();
            entry.SuspendCts = null;
        }

        entry.LastTouchUtc = DateTime.UtcNow;
    }

    public void End(string sessionId)
    {
        if (!_sessions.TryRemove(sessionId, out var entry))
        {
            return;
        }

        entry.SuspendCts?.Cancel();
        entry.SuspendCts?.Dispose();
    }

    private sealed class Entry
    {
        public DateTime LastTouchUtc { get; set; }
        public CancellationTokenSource? SuspendCts { get; set; }
    }
}
