using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.Customers;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class CustomerAdminUseCase : ICustomerAdminPort
{
    private readonly ICustomerRepository _repository;

    public CustomerAdminUseCase(ICustomerRepository repository) => _repository = repository;

    public Task<PagedResult<CustomerDto>> GetCustomersAsync(CustomerListFilter filter, CancellationToken cancellationToken = default) => _repository.GetCustomersAsync(filter, cancellationToken);
    public Task<CustomerEditDto?> GetForEditAsync(int customerId, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(customerId, cancellationToken);
    public Task<int> SaveAsync(CustomerEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int customerId, CancellationToken cancellationToken = default) => _repository.DeleteAsync(customerId, cancellationToken);
    public Task<PasswordResetResult> ResetWebshopPasswordAsync(int customerId, string? newPassword = null, CancellationToken cancellationToken = default) =>
        _repository.ResetWebshopPasswordAsync(customerId, newPassword, cancellationToken);
}