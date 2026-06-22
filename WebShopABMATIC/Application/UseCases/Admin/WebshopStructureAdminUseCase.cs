using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.WebshopStructures;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class WebshopStructureAdminUseCase : IWebshopStructureAdminPort
{
    private readonly IWebshopStructureRepository _repository;

    public WebshopStructureAdminUseCase(IWebshopStructureRepository repository) => _repository = repository;

    public Task<PagedResult<WebshopStructureDto>> GetWebshopStructuresAsync(WebshopStructureListFilter filter, CancellationToken cancellationToken = default) => _repository.GetWebshopStructuresAsync(filter, cancellationToken);
    public Task<WebshopStructureEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(WebshopStructureEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}