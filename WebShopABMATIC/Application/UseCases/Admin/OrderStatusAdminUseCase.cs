using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.OrderStatuses;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class OrderStatusAdminUseCase : IOrderStatusAdminPort
{
    private readonly IOrderStatusRepository _repository;

    public OrderStatusAdminUseCase(IOrderStatusRepository repository) => _repository = repository;

    public Task<PagedResult<OrderStatusDto>> GetOrderStatusesAsync(OrderStatusListFilter filter, CancellationToken cancellationToken = default) => _repository.GetOrderStatusesAsync(filter, cancellationToken);
    public Task<OrderStatusEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(OrderStatusEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}