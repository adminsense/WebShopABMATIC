using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.DeliveryTypes;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class DeliveryTypeAdminUseCase : IDeliveryTypeAdminPort
{
    private readonly IDeliveryTypeRepository _repository;

    public DeliveryTypeAdminUseCase(IDeliveryTypeRepository repository) => _repository = repository;

    public Task<PagedResult<DeliveryTypeDto>> GetDeliveryTypesAsync(DeliveryTypeListFilter filter, CancellationToken cancellationToken = default) => _repository.GetDeliveryTypesAsync(filter, cancellationToken);
    public Task<DeliveryTypeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(DeliveryTypeEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}