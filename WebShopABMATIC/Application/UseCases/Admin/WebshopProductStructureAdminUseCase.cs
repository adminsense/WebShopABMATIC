using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.WebshopProductStructures;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class WebshopProductStructureAdminUseCase : IWebshopProductStructureAdminPort
{
    private readonly IWebshopProductStructureRepository _repository;

    public WebshopProductStructureAdminUseCase(IWebshopProductStructureRepository repository) => _repository = repository;

    public Task<PagedResult<WebshopProductStructureDto>> GetWebshopProductStructuresAsync(WebshopProductStructureListFilter filter, CancellationToken cancellationToken = default) => _repository.GetWebshopProductStructuresAsync(filter, cancellationToken);
    public Task<WebshopProductStructureEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(WebshopProductStructureEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}