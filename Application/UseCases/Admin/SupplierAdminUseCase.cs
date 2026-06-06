using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.Suppliers;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class SupplierAdminUseCase : ISupplierAdminPort
{
    private readonly ISupplierRepository _repository;

    public SupplierAdminUseCase(ISupplierRepository repository) => _repository = repository;

    public Task<PagedResult<SupplierDto>> GetSuppliersAsync(SupplierListFilter filter, CancellationToken cancellationToken = default) => _repository.GetSuppliersAsync(filter, cancellationToken);
    public Task<SupplierEditDto?> GetForEditAsync(int supplierId, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(supplierId, cancellationToken);
    public Task<int> SaveAsync(SupplierEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int supplierId, CancellationToken cancellationToken = default) => _repository.DeleteAsync(supplierId, cancellationToken);
}