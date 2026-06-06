namespace WebShopABMATIC.Application.Auth;

public sealed class PasswordResetResult
{
    public required bool Succeeded { get; init; }
    public string? TemporaryPassword { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];
}
