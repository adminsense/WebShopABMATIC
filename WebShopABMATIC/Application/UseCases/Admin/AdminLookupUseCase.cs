using WebShopABMATIC.Application.Admin.Lookups;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class AdminLookupUseCase : IAdminLookupPort
{
    private readonly IAdminLookupRepository _repository;

    public AdminLookupUseCase(IAdminLookupRepository repository) => _repository = repository;

    public Task<PagedResult<CityLookupDto>> SearchCitiesAsync(CityListFilter filter, CancellationToken cancellationToken = default) =>
        _repository.SearchCitiesAsync(filter, cancellationToken);

    public Task<CityLookupDto?> GetCityAsync(int cityId, CancellationToken cancellationToken = default) =>
        _repository.GetCityAsync(cityId, cancellationToken);

    public Task<IReadOnlyList<LanguageLookupDto>> GetLanguagesAsync(CancellationToken cancellationToken = default) =>
        _repository.GetLanguagesAsync(cancellationToken);

    public Task<IReadOnlyList<NamedLookupDto>> GetDeliveryTypesAsync(CancellationToken cancellationToken = default) =>
        _repository.GetDeliveryTypesAsync(cancellationToken);

    public Task<IReadOnlyList<NamedLookupDto>> GetCustomerTypesAsync(CancellationToken cancellationToken = default) =>
        _repository.GetCustomerTypesAsync(cancellationToken);

    public Task<IReadOnlyList<NamedLookupDto>> GetSuppliersAsync(CancellationToken cancellationToken = default) =>
        _repository.GetSuppliersAsync(cancellationToken);

    public Task<IReadOnlyList<NamedLookupDto>> GetManufacturersAsync(CancellationToken cancellationToken = default) =>
        _repository.GetManufacturersAsync(cancellationToken);

    public Task<IReadOnlyList<NamedLookupDto>> GetStockLocationsAsync(CancellationToken cancellationToken = default) =>
        _repository.GetStockLocationsAsync(cancellationToken);
}
