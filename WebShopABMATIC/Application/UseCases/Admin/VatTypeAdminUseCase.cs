using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.VatTypes;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class VatTypeAdminUseCase : IVatTypeAdminPort
{
    private readonly IVatTypeRepository _repository;

    public VatTypeAdminUseCase(IVatTypeRepository repository) => _repository = repository;

    public Task<PagedResult<VatTypeDto>> GetVatTypesAsync(VatTypeListFilter filter, CancellationToken cancellationToken = default) => _repository.GetVatTypesAsync(filter, cancellationToken);
    public Task<VatTypeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(VatTypeEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}