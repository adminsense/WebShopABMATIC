using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Infrastructure.Auth;
using WebShopABMATIC.Web.Components;

namespace WebShopABMATIC.Web.Components.Account;

/// <summary>
/// Flows legacy cookie auth into Blazor interactive circuits (SSR + CSR).
/// </summary>
public sealed class LegacyAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider, IDisposable
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStoreBrowserSessionStore _browserSessions;
    private readonly PersistentComponentState _persistentState;
    private readonly PersistingComponentStateSubscription _subscription;
    private Task<AuthenticationState>? _authenticationStateTask;

    public LegacyAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        IStoreBrowserSessionStore browserSessions,
        PersistentComponentState persistentComponentState)
        : base(loggerFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _browserSessions = browserSessions;
        _persistentState = persistentComponentState;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        _subscription = _persistentState.RegisterOnPersisting(
            OnPersistingAsync,
            AppRenderModes.InteractiveServer);
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(20);

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var task = GetAuthenticationStateCoreAsync();
        _authenticationStateTask = task;
        return task;
    }

    private Task<AuthenticationState> GetAuthenticationStateCoreAsync()
    {
        var httpUser = _httpContextAccessor.HttpContext?.User;
        if (httpUser?.Identity?.IsAuthenticated == true)
        {
            if (!IsStoreBrowserSessionValid(httpUser))
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            return Task.FromResult(new AuthenticationState(httpUser));
        }

        // Do not revive a session from persisted prerender state when the auth cookie is gone.
        if (_httpContextAccessor.HttpContext is not null)
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        if (_persistentState.TryTakeFromJson(nameof(UserInfo), out UserInfo? userInfo)
            && userInfo is not null
            && !string.IsNullOrEmpty(userInfo.UserId))
        {
            var principal = CreatePrincipal(userInfo);
            if (!IsStoreBrowserSessionValid(principal))
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            return Task.FromResult(new AuthenticationState(principal));
        }

        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
    }

    protected override Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState,
        CancellationToken cancellationToken)
    {
        var user = authenticationState.User;
        if (user.Identity?.IsAuthenticated != true)
        {
            return Task.FromResult(false);
        }

        // When HttpContext is available, the cookie must still be valid.
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is not null && httpContext.User.Identity?.IsAuthenticated != true)
        {
            return Task.FromResult(false);
        }

        if (!IsStoreBrowserSessionValid(user))
        {
            return Task.FromResult(false);
        }

        var hasLegacyId = user.HasClaim(c =>
            c.Type is ClaimTypes.NameIdentifier or "legacy_staff_id" or "legacy_customer_id");

        return Task.FromResult(hasLegacyId);
    }

    private bool IsStoreBrowserSessionValid(ClaimsPrincipal user)
    {
        if (!user.IsInRole(AppRoles.Customer))
        {
            return true;
        }

        var sessionId = user.FindFirstValue(LegacyAuthClaims.StoreBrowserSession);
        return !string.IsNullOrEmpty(sessionId) && _browserSessions.IsActive(sessionId);
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> task) =>
        _authenticationStateTask = task;

    private async Task OnPersistingAsync()
    {
        var authenticationState = _authenticationStateTask is not null
            ? await _authenticationStateTask
            : await GetAuthenticationStateCoreAsync();

        var principal = authenticationState.User;
        if (principal.Identity?.IsAuthenticated != true)
        {
            return;
        }

        var userInfo = new UserInfo
        {
            UserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            Name = principal.FindFirst(ClaimTypes.Name)?.Value,
            DisplayName = principal.FindFirst(LegacyAuthClaims.DisplayName)?.Value,
            StoreBrowserSession = principal.FindFirst(LegacyAuthClaims.StoreBrowserSession)?.Value,
            Roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray()
        };

        _persistentState.PersistAsJson(nameof(UserInfo), userInfo);
    }

    private static ClaimsPrincipal CreatePrincipal(UserInfo userInfo)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userInfo.UserId!),
            new(ClaimTypes.Name, userInfo.Name ?? userInfo.UserId!)
        };

        if (!string.IsNullOrWhiteSpace(userInfo.DisplayName))
        {
            claims.Add(new Claim(LegacyAuthClaims.DisplayName, userInfo.DisplayName));
        }

        if (!string.IsNullOrWhiteSpace(userInfo.StoreBrowserSession))
        {
            claims.Add(new Claim(LegacyAuthClaims.StoreBrowserSession, userInfo.StoreBrowserSession));
        }

        claims.AddRange(userInfo.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        if (userInfo.UserId!.StartsWith("staff:", StringComparison.Ordinal))
        {
            claims.Add(new Claim("legacy_staff_id", userInfo.UserId["staff:".Length..]));
        }
        else if (userInfo.UserId.StartsWith("customer:", StringComparison.Ordinal))
        {
            claims.Add(new Claim("legacy_customer_id", userInfo.UserId["customer:".Length..]));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Legacy"));
    }

    public void Dispose()
    {
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        _subscription.Dispose();
    }
}

internal sealed class UserInfo
{
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? StoreBrowserSession { get; set; }
    public string[] Roles { get; set; } = [];
}
