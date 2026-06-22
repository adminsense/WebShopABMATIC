using System.Collections.Concurrent;

namespace WebShopABMATIC.Infrastructure.Audit;

/// <summary>Suppresses duplicate Logout audit when manual sign-out is followed by circuit close.</summary>
public interface IManualLogoutTracker
{
    void MarkManualLogout(string identityUserId);

    bool WasManualLogoutRecently(string identityUserId);
}

public sealed class ManualLogoutTracker : IManualLogoutTracker
{
    private static readonly TimeSpan SuppressWindow = TimeSpan.FromSeconds(20);
    private readonly ConcurrentDictionary<string, DateTime> _recent = new();

    public void MarkManualLogout(string identityUserId)
    {
        if (!string.IsNullOrWhiteSpace(identityUserId))
        {
            _recent[identityUserId] = DateTime.UtcNow;
        }
    }

    public bool WasManualLogoutRecently(string identityUserId)
    {
        if (string.IsNullOrWhiteSpace(identityUserId))
        {
            return false;
        }

        if (!_recent.TryGetValue(identityUserId, out var at))
        {
            return false;
        }

        if (DateTime.UtcNow - at > SuppressWindow)
        {
            _recent.TryRemove(identityUserId, out _);
            return false;
        }

        return true;
    }
}
