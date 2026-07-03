using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class StockOrderAdminUseCase : IStockOrderAdminPort
{
    private readonly IStockOrderRepository _repository;
    private readonly ICurrentUserContext _currentUser;

    public StockOrderAdminUseCase(IStockOrderRepository repository, ICurrentUserContext currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public Task<PagedResult<StockOrderSummaryDto>> GetOrdersAsync(StockOrderListFilter filter, CancellationToken cancellationToken = default) =>
        _repository.GetOrdersAsync(filter, cancellationToken);

    public Task<StockOrderEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        _repository.GetForEditAsync(id, cancellationToken);

    public Task<StockOrderLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default) =>
        _repository.GetLookupsAsync(cancellationToken);

    public async Task<int> SaveAsync(StockOrderEditDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _currentUser.GetCurrentUserAsync(cancellationToken);
        return await _repository.SaveAsync(dto, user.ResolveLegacyUserId(), cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        _repository.DeleteAsync(id, cancellationToken);
}
