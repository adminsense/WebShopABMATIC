using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.Orders;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class OrderAdminUseCase : IOrderAdminPort
{
    private readonly IOrderRepository _repository;

    public OrderAdminUseCase(IOrderRepository repository) => _repository = repository;

    public Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(OrderListFilter filter, CancellationToken cancellationToken = default) => _repository.GetOrdersAsync(filter, cancellationToken);
    public Task<OrderEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(OrderEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
}