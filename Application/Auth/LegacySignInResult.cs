using System.Security.Claims;

namespace WebShopABMATIC.Application.Auth;

public sealed class LegacySignInResult
{
    public bool Succeeded { get; init; }
    public string? Error { get; init; }
    public ClaimsPrincipal? Principal { get; init; }
}
