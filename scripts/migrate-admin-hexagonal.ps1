$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot

$services = @(
    @{ Service = 'WebshopStructureAdminService'; Port = 'IWebshopStructureAdminPort'; Entity = 'WebshopStructure' },
    @{ Service = 'WebshopProductStructureAdminService'; Port = 'IWebshopProductStructureAdminPort'; Entity = 'WebshopProductStructure' },
    @{ Service = 'ProductPriceAdminService'; Port = 'IProductPriceAdminPort'; Entity = 'ProductPrice' },
    @{ Service = 'ProductQuantityTierAdminService'; Port = 'IProductQuantityTierAdminPort'; Entity = 'ProductQuantityTier' },
    @{ Service = 'ProductOptionAdminService'; Port = 'IProductOptionAdminPort'; Entity = 'ProductOption' },
    @{ Service = 'PriceListCategoryAdminService'; Port = 'IPriceListCategoryAdminPort'; Entity = 'PriceListCategory' },
    @{ Service = 'ManufacturerAdminService'; Port = 'IManufacturerAdminPort'; Entity = 'Manufacturer' },
    @{ Service = 'SupplierAdminService'; Port = 'ISupplierAdminPort'; Entity = 'Supplier' },
    @{ Service = 'CustomerDeliveryAddressAdminService'; Port = 'ICustomerDeliveryAddressAdminPort'; Entity = 'CustomerDeliveryAddress' },
    @{ Service = 'CustomerProductDiscountAdminService'; Port = 'ICustomerProductDiscountAdminPort'; Entity = 'CustomerProductDiscount' },
    @{ Service = 'CustomerTypeAdminService'; Port = 'ICustomerTypeAdminPort'; Entity = 'CustomerType' },
    @{ Service = 'CustomerAdminService'; Port = 'ICustomerAdminPort'; Entity = 'Customer' },
    @{ Service = 'OrderStatusAdminService'; Port = 'IOrderStatusAdminPort'; Entity = 'OrderStatus' },
    @{ Service = 'DeliveryTypeAdminService'; Port = 'IDeliveryTypeAdminPort'; Entity = 'DeliveryType' },
    @{ Service = 'OrderAdminService'; Port = 'IOrderAdminPort'; Entity = 'Order' },
    @{ Service = 'ProductStockLocationAdminService'; Port = 'IProductStockLocationAdminPort'; Entity = 'ProductStockLocation' },
    @{ Service = 'StockLocationAdminService'; Port = 'IStockLocationAdminPort'; Entity = 'StockLocation' },
    @{ Service = 'PaymentMethodAdminService'; Port = 'IPaymentMethodAdminPort'; Entity = 'PaymentMethod' },
    @{ Service = 'StaffUserAdminService'; Port = 'IStaffUserAdminPort'; Entity = 'StaffUser' },
    @{ Service = 'UserGroupAdminService'; Port = 'IUserGroupAdminPort'; Entity = 'UserGroup' },
    @{ Service = 'VatTypeAdminService'; Port = 'IVatTypeAdminPort'; Entity = 'VatType' }
)

$portsFile = Get-Content (Join-Path $root 'Application\Ports\IAdminPorts.cs') -Raw
$outboundDir = Join-Path $root 'Application\Ports\Outbound'
$repoDir = Join-Path $root 'Infrastructure\Persistence\Repositories'
$useCaseDir = Join-Path $root 'Application\UseCases\Admin'
New-Item -ItemType Directory -Force -Path $outboundDir, $repoDir, $useCaseDir | Out-Null

foreach ($s in $services) {
    $repoInterface = 'I' + $s.Entity + 'Repository'
    $repoClass = $s.Entity + 'Repository'
    $useCaseClass = $s.Entity + 'AdminUseCase'
    $servicePath = Join-Path $root "Infrastructure\Admin\$($s.Service).cs"

    # Extract port interface body from IAdminPorts.cs
    $portPattern = "public interface $($s.Port)\r?\n\{([\s\S]*?)\r?\n\}"
    if ($portsFile -notmatch $portPattern) { throw "Port not found: $($s.Port)" }
    $portBody = $Matches[1]

    # Create repository interface
    $repoInterfaceContent = @"
namespace WebShopABMATIC.Application.Ports.Outbound;

public interface $repoInterface
{$portBody
}
"@
    Set-Content -Path (Join-Path $outboundDir "$repoInterface.cs") -Value $repoInterfaceContent -NoNewline

    # Create repository from admin service
    $serviceContent = Get-Content $servicePath -Raw
    $repoContent = $serviceContent `
        -replace 'namespace WebShopABMATIC\.Infrastructure\.Admin;', 'namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;' `
        -replace 'using WebShopABMATIC\.Application\.Ports;', "using WebShopABMATIC.Application.Ports.Outbound;`r`nusing WebShopABMATIC.Infrastructure.Persistence;" `
        -replace "public sealed class $($s.Service) : $($s.Port)", "public sealed class $repoClass : $repoInterface" `
        -replace "$($s.Service)\(", "$repoClass("
    Set-Content -Path (Join-Path $repoDir "$repoClass.cs") -Value $repoContent -NoNewline

    # Create use case
    $methods = [regex]::Matches($portBody, '(?m)^\s*(Task<[^>]+>|Task<bool>|Task<int>)\s+(\w+)\(([^)]*)\);')
    $delegations = ($methods | ForEach-Object {
        $params = $_.Groups[3].Value
        "    public $($_.Groups[1].Value) $($_.Groups[2].Value)($params) => _repository.$($_.Groups[2].Value)($params);"
    }) -join "`r`n"

    $useCaseContent = @"
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class $useCaseClass : $($s.Port)
{
    private readonly $repoInterface _repository;

    public $useCaseClass($repoInterface repository) => _repository = repository;

$delegations
}
"@
    Set-Content -Path (Join-Path $useCaseDir "$useCaseClass.cs") -Value $useCaseContent -NoNewline

    Remove-Item $servicePath -Force
    Write-Host "Migrated $($s.Service)"
}

Remove-Item (Join-Path $root 'Infrastructure\Admin\AdminCrudDefaults.cs') -Force
Write-Host 'Done.'
