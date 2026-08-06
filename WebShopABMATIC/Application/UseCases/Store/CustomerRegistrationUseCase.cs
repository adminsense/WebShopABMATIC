using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Registration;

namespace WebShopABMATIC.Application.UseCases.Store;

public sealed class CustomerRegistrationUseCase : ICustomerRegistrationPort
{
    private readonly ICustomerRegistrationRepository _repository;
    private readonly IAuditService _audit;

    public CustomerRegistrationUseCase(
        ICustomerRegistrationRepository repository,
        IAuditService audit)
    {
        _repository = repository;
        _audit = audit;
    }

    public async Task<CustomerRegistrationResult> RegisterAsync(
        CustomerRegistrationRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.RegisterAsync(request, cancellationToken);

        try
        {
            await _audit.LogAsync(new AuditLogWriteRequest
            {
                Action = AuditActions.Create,
                EntityName = "Customer",
                EntityId = result.CustomerId?.ToString(),
                UserDisplayName = request.Email,
                Success = result.Succeeded,
                ErrorMessage = result.Succeeded
                    ? null
                    : string.Join("; ", result.Errors)
            }, cancellationToken);
        }
        catch
        {
            // Registration outcome must not fail because audit write failed.
        }

        return result;
    }
}
