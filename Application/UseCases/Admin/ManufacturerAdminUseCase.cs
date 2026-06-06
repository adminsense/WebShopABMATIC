using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.Manufacturers;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class ManufacturerAdminUseCase : IManufacturerAdminPort
{
    private readonly IManufacturerRepository _repository;

    public ManufacturerAdminUseCase(IManufacturerRepository repository) => _repository = repository;

    public Task<PagedResult<ManufacturerDto>> GetManufacturersAsync(ManufacturerListFilter filter, CancellationToken cancellationToken = default) => _repository.GetManufacturersAsync(filter, cancellationToken);
    public Task<ManufacturerEditDto?> GetForEditAsync(int manufacturerId, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(manufacturerId, cancellationToken);
    public Task<int> SaveAsync(ManufacturerEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int manufacturerId, CancellationToken cancellationToken = default) => _repository.DeleteAsync(manufacturerId, cancellationToken);
}