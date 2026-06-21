namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ILegacyCustomerPasswordPort
{
    Task<(bool Succeeded, string? Error)> ChangePasswordAsync(
        int customerId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default);
}
