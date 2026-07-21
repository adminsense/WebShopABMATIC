namespace WebShopABMATIC.Infrastructure.Store;

/// <summary>
/// Blazor Server shares one scoped DbContext per circuit. Sidebar, cart, checkout and
/// catalog can query concurrently — serialize all store DB work through this gate.
/// Re-entrant on the same async flow so catalog→pricing nested calls do not deadlock.
/// </summary>
public sealed class StoreDbGate
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly AsyncLocal<int> _depth = new();

    public async Task<T> RunAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
    {
        var acquired = false;
        if (_depth.Value == 0)
        {
            // Stay on the Blazor sync context — ConfigureAwait(false) caused closed-connection races on the scoped DbContext.
            await _gate.WaitAsync(cancellationToken);
            acquired = true;
        }

        _depth.Value++;
        try
        {
            return await operation();
        }
        finally
        {
            _depth.Value--;
            if (acquired)
            {
                _gate.Release();
            }
        }
    }

    public async Task RunAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        var acquired = false;
        if (_depth.Value == 0)
        {
            await _gate.WaitAsync(cancellationToken);
            acquired = true;
        }

        _depth.Value++;
        try
        {
            await operation();
        }
        finally
        {
            _depth.Value--;
            if (acquired)
            {
                _gate.Release();
            }
        }
    }
}
