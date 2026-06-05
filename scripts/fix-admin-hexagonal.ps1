$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot

$entityUsings = @{
    WebshopStructure = 'WebShopABMATIC.Application.Admin.WebshopStructures'
    WebshopProductStructure = 'WebShopABMATIC.Application.Admin.WebshopProductStructures'
    ProductPrice = 'WebShopABMATIC.Application.Admin.ProductPrices'
    ProductQuantityTier = 'WebShopABMATIC.Application.Admin.ProductQuantityTiers'
    ProductOption = 'WebShopABMATIC.Application.Admin.ProductOptions'
    PriceListCategory = 'WebShopABMATIC.Application.Admin.PriceListCategories'
    Manufacturer = 'WebShopABMATIC.Application.Admin.Manufacturers'
    Supplier = 'WebShopABMATIC.Application.Admin.Suppliers'
    CustomerDeliveryAddress = 'WebShopABMATIC.Application.Admin.CustomerDeliveryAddresses'
    CustomerProductDiscount = 'WebShopABMATIC.Application.Admin.CustomerProductDiscounts'
    CustomerType = 'WebShopABMATIC.Application.Admin.CustomerTypes'
    Customer = 'WebShopABMATIC.Application.Admin.Customers'
    OrderStatus = 'WebShopABMATIC.Application.Admin.OrderStatuses'
    DeliveryType = 'WebShopABMATIC.Application.Admin.DeliveryTypes'
    Order = 'WebShopABMATIC.Application.Admin.Orders'
    ProductStockLocation = 'WebShopABMATIC.Application.Admin.ProductStockLocations'
    StockLocation = 'WebShopABMATIC.Application.Admin.StockLocations'
    PaymentMethod = 'WebShopABMATIC.Application.Admin.PaymentMethods'
    StaffUser = 'WebShopABMATIC.Application.Admin.StaffUsers'
    UserGroup = 'WebShopABMATIC.Application.Admin.UserGroups'
    VatType = 'WebShopABMATIC.Application.Admin.VatTypes'
}

$services = @(
    @{ Port = 'IWebshopStructureAdminPort'; Entity = 'WebshopStructure' },
    @{ Port = 'IWebshopProductStructureAdminPort'; Entity = 'WebshopProductStructure' },
    @{ Port = 'IProductPriceAdminPort'; Entity = 'ProductPrice' },
    @{ Port = 'IProductQuantityTierAdminPort'; Entity = 'ProductQuantityTier' },
    @{ Port = 'IProductOptionAdminPort'; Entity = 'ProductOption' },
    @{ Port = 'IPriceListCategoryAdminPort'; Entity = 'PriceListCategory' },
    @{ Port = 'IManufacturerAdminPort'; Entity = 'Manufacturer' },
    @{ Port = 'ISupplierAdminPort'; Entity = 'Supplier' },
    @{ Port = 'ICustomerDeliveryAddressAdminPort'; Entity = 'CustomerDeliveryAddress' },
    @{ Port = 'ICustomerProductDiscountAdminPort'; Entity = 'CustomerProductDiscount' },
    @{ Port = 'ICustomerTypeAdminPort'; Entity = 'CustomerType' },
    @{ Port = 'ICustomerAdminPort'; Entity = 'Customer' },
    @{ Port = 'IOrderStatusAdminPort'; Entity = 'OrderStatus' },
    @{ Port = 'IDeliveryTypeAdminPort'; Entity = 'DeliveryType' },
    @{ Port = 'IOrderAdminPort'; Entity = 'Order' },
    @{ Port = 'IProductStockLocationAdminPort'; Entity = 'ProductStockLocation' },
    @{ Port = 'IStockLocationAdminPort'; Entity = 'StockLocation' },
    @{ Port = 'IPaymentMethodAdminPort'; Entity = 'PaymentMethod' },
    @{ Port = 'IStaffUserAdminPort'; Entity = 'StaffUser' },
    @{ Port = 'IUserGroupAdminPort'; Entity = 'UserGroup' },
    @{ Port = 'IVatTypeAdminPort'; Entity = 'VatType' }
)

$portsFile = Get-Content (Join-Path $root 'Application\Ports\IAdminPorts.cs') -Raw
$outboundDir = Join-Path $root 'Application\Ports\Outbound'
$useCaseDir = Join-Path $root 'Application\UseCases\Admin'

function Get-ParamNames([string]$params) {
    if ([string]::IsNullOrWhiteSpace($params)) { return @() }
    return ($params -split ',') | ForEach-Object {
        $p = $_.Trim()
        if ($p -match '\s(\w+)\s*(?:=\s*.+)?$') { $Matches[1] }
    }
}

foreach ($s in $services) {
    $repoInterface = 'I' + $s.Entity + 'Repository'
    $useCaseClass = $s.Entity + 'AdminUseCase'
    $usingNs = $entityUsings[$s.Entity]

    $portPattern = "public interface $($s.Port)\r?\n\{([\s\S]*?)\r?\n\}"
    if ($portsFile -notmatch $portPattern) { throw "Port not found: $($s.Port)" }
    $portBody = $Matches[1]

    $repoInterfaceContent = @"
using WebShopABMATIC.Application.Common;
using $usingNs;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface $repoInterface
{$portBody
}
"@
    Set-Content -Path (Join-Path $outboundDir "$repoInterface.cs") -Value $repoInterfaceContent -NoNewline

    $methodLines = ($portBody -split "`r?`n") | Where-Object { $_ -match '^\s*Task' }
    $delegations = ($methodLines | ForEach-Object {
        $line = $_.Trim()
        if ($line -match '^(.+)\s+(\w+)\((.*)\);\s*$') {
            $returnType = $Matches[1].Trim()
            $methodName = $Matches[2]
            $params = $Matches[3]
            $names = Get-ParamNames $params
            $callArgs = $names -join ', '
            "    public $returnType $methodName($params) => _repository.$methodName($callArgs);"
        }
    }) -join "`r`n"

    $useCaseContent = @"
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using $usingNs;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class $useCaseClass : $($s.Port)
{
    private readonly $repoInterface _repository;

    public $useCaseClass($repoInterface repository) => _repository = repository;

$delegations
}
"@
    Set-Content -Path (Join-Path $useCaseDir "$useCaseClass.cs") -Value $useCaseContent -NoNewline
    Write-Host "Fixed $($useCaseClass)"
}

Write-Host 'Done.'
