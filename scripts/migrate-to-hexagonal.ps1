# Generates hexagonal Outbound ports, Repositories, and UseCases from legacy AdminServices.
$ErrorActionPreference = "Stop"
$root = "c:\Projects\WebShopABMATIC"

$entities = @(
    @{ Service="WebshopStructureAdminService"; Port="IWebshopStructureAdminPort"; Repo="IWebshopStructureRepository"; Entity="WebshopStructure"; UseCase="WebshopStructureAdminUseCase" }
    @{ Service="WebshopProductStructureAdminService"; Port="IWebshopProductStructureAdminPort"; Repo="IWebshopProductStructureRepository"; Entity="WebshopProductStructure"; UseCase="WebshopProductStructureAdminUseCase" }
    @{ Service="ProductPriceAdminService"; Port="IProductPriceAdminPort"; Repo="IProductPriceRepository"; Entity="ProductPrice"; UseCase="ProductPriceAdminUseCase" }
    @{ Service="ProductQuantityTierAdminService"; Port="IProductQuantityTierAdminPort"; Repo="IProductQuantityTierRepository"; Entity="ProductQuantityTier"; UseCase="ProductQuantityTierAdminUseCase" }
    @{ Service="ProductOptionAdminService"; Port="IProductOptionAdminPort"; Repo="IProductOptionRepository"; Entity="ProductOption"; UseCase="ProductOptionAdminUseCase" }
    @{ Service="PriceListCategoryAdminService"; Port="IPriceListCategoryAdminPort"; Repo="IPriceListCategoryRepository"; Entity="PriceListCategory"; UseCase="PriceListCategoryAdminUseCase" }
    @{ Service="ManufacturerAdminService"; Port="IManufacturerAdminPort"; Repo="IManufacturerRepository"; Entity="Manufacturer"; UseCase="ManufacturerAdminUseCase" }
    @{ Service="SupplierAdminService"; Port="ISupplierAdminPort"; Repo="ISupplierRepository"; Entity="Supplier"; UseCase="SupplierAdminUseCase" }
    @{ Service="CustomerDeliveryAddressAdminService"; Port="ICustomerDeliveryAddressAdminPort"; Repo="ICustomerDeliveryAddressRepository"; Entity="CustomerDeliveryAddress"; UseCase="CustomerDeliveryAddressAdminUseCase" }
    @{ Service="CustomerProductDiscountAdminService"; Port="ICustomerProductDiscountAdminPort"; Repo="ICustomerProductDiscountRepository"; Entity="CustomerProductDiscount"; UseCase="CustomerProductDiscountAdminUseCase" }
    @{ Service="CustomerTypeAdminService"; Port="ICustomerTypeAdminPort"; Repo="ICustomerTypeRepository"; Entity="CustomerType"; UseCase="CustomerTypeAdminUseCase" }
    @{ Service="CustomerAdminService"; Port="ICustomerAdminPort"; Repo="ICustomerRepository"; Entity="Customer"; UseCase="CustomerAdminUseCase" }
    @{ Service="OrderStatusAdminService"; Port="IOrderStatusAdminPort"; Repo="IOrderStatusRepository"; Entity="OrderStatus"; UseCase="OrderStatusAdminUseCase" }
    @{ Service="DeliveryTypeAdminService"; Port="IDeliveryTypeAdminPort"; Repo="IDeliveryTypeRepository"; Entity="DeliveryType"; UseCase="DeliveryTypeAdminUseCase" }
    @{ Service="OrderAdminService"; Port="IOrderAdminPort"; Repo="IOrderRepository"; Entity="Order"; UseCase="OrderAdminUseCase" }
    @{ Service="ProductStockLocationAdminService"; Port="IProductStockLocationAdminPort"; Repo="IProductStockLocationRepository"; Entity="ProductStockLocation"; UseCase="ProductStockLocationAdminUseCase" }
    @{ Service="StockLocationAdminService"; Port="IStockLocationAdminPort"; Repo="IStockLocationRepository"; Entity="StockLocation"; UseCase="StockLocationAdminUseCase" }
    @{ Service="PaymentMethodAdminService"; Port="IPaymentMethodAdminPort"; Repo="IPaymentMethodRepository"; Entity="PaymentMethod"; UseCase="PaymentMethodAdminUseCase" }
    @{ Service="StaffUserAdminService"; Port="IStaffUserAdminPort"; Repo="IStaffUserRepository"; Entity="StaffUser"; UseCase="StaffUserAdminUseCase" }
    @{ Service="UserGroupAdminService"; Port="IUserGroupAdminPort"; Repo="IUserGroupRepository"; Entity="UserGroup"; UseCase="UserGroupAdminUseCase" }
    @{ Service="VatTypeAdminService"; Port="IVatTypeAdminPort"; Repo="IVatTypeRepository"; Entity="VatType"; UseCase="VatTypeAdminUseCase" }
)

$outboundDir = Join-Path $root "Application\Ports\Outbound"
$useCaseDir = Join-Path $root "Application\UseCases\Admin"
$repoDir = Join-Path $root "Infrastructure\Persistence\Repositories"
New-Item -ItemType Directory -Force -Path $outboundDir, $useCaseDir, $repoDir | Out-Null

# Read IAdminPorts.cs for port method signatures
$portsFile = Get-Content (Join-Path $root "Application\Ports\IAdminPorts.cs") -Raw

foreach ($e in $entities) {
    $servicePath = Join-Path $root "Infrastructure\Admin\$($e.Service).cs"
    if (-not (Test-Path $servicePath)) { Write-Warning "Skip $($e.Service)"; continue }

    $serviceContent = Get-Content $servicePath -Raw
    $portName = $e.Port

    # Extract interface block from IAdminPorts
    $portPattern = "public interface $portName\s*\{([^}]+)\}"
    if ($portsFile -notmatch $portPattern) { Write-Warning "Port not found: $portName"; continue }
    $portMethods = $Matches[1].Trim()

    # Outbound port: same methods as inbound (persistence adapter contract)
    $repoInterface = @"
using WebShopABMATIC.Application.Admin.*;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface $($e.Repo)
{
$portMethods
}
"@
    # Fix usings - need specific namespaces from service file
    $nsMatch = [regex]::Match($serviceContent, 'using WebShopABMATIC\.Application\.Admin\.(\w+);')
    if ($nsMatch.Success) {
        $adminNs = $nsMatch.Groups[1].Value
        $repoInterface = $repoInterface -replace 'using WebShopABMATIC\.Application\.Admin\.\*;', "using WebShopABMATIC.Application.Admin.$adminNs;"
    }

    Set-Content (Join-Path $outboundDir "$($e.Repo).cs") $repoInterface.TrimEnd() -Encoding UTF8

    # Repository: rename class, implement repo interface, move namespace
    $repoClass = $e.Repo.Substring(1) # drop I
    $repoContent = $serviceContent `
        -replace 'namespace WebShopABMATIC\.Infrastructure\.Admin;', 'namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;' `
        -replace "public sealed class $($e.Service) : $($e.Port)", "public sealed class ${repoClass} : $($e.Repo)" `
        -replace 'using WebShopABMATIC\.Application\.Ports;', 'using WebShopABMATIC.Application.Ports.Outbound;' `
        -replace 'Infrastructure\.Admin', 'Infrastructure.Persistence'

    Set-Content (Join-Path $repoDir "${repoClass}.cs") $repoContent -Encoding UTF8

    # UseCase: thin delegate to repository
    $useCaseContent = @"
using WebShopABMATIC.Application.Admin.$adminNs;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class $($e.UseCase) : $($e.Port)
{
    private readonly $($e.Repo) _repository;

    public $($e.UseCase)($($e.Repo) repository) => _repository = repository;

$(
    ($portMethods -split "`n" | ForEach-Object {
        $line = $_.Trim()
        if ($line -match 'Task<(.+?)>\s+(\w+)Async\((.+?)\)') {
            $ret = $Matches[1]; $method = $Matches[2]; $params = $Matches[3]
            "    public Task<$ret> ${method}Async($params) => _repository.${method}Async($($params -replace 'CancellationToken cancellationToken = default', 'cancellationToken') -replace 'CancellationToken cancellationToken = default', 'cancellationToken');"
        } elseif ($line -match 'Task<(\w+)>\s+(\w+)Async\((.+?)\)') {
            $ret = $Matches[1]; $method = $Matches[2]; $params = $Matches[3]
            "    public Task<$ret> ${method}Async($params) => _repository.${method}Async($($params));"
        } elseif ($line -match 'Task\s+(\w+)Async\((.+?)\)') {
            $method = $Matches[1]; $params = $Matches[2]
            "    public Task ${method}Async($params) => _repository.${method}Async($($params));"
        }
    }) -join "`n"
)
}
"@
    Set-Content (Join-Path $useCaseDir "$($e.UseCase).cs") $useCaseContent -Encoding UTF8
    Write-Host "Generated $($e.Entity)"
}

Write-Host "Done."
