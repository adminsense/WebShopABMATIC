# Generates dedicated *List.razor pages (Product pattern) for admin entities.
$ErrorActionPreference = "Stop"
$root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
if (-not (Test-Path "$root\Web")) { $root = "c:\Projects\WebShopABMATIC" }
$outDir = Join-Path $root "Web\Components\Pages\Admin"

$entities = @(
    @{ File="WebshopStructureList.razor"; Route="webshop-structures"; Hub="/admin/hub/webshop"; Icon="bi-diagram-3"; Title="Webshop structure"; Desc="Catalog navigation tree nodes."; Entity="webshop structure"; Id="Id"; Port="IWebshopStructureAdminPort"; Dto="WebshopStructure"; EditId="Id"; Delete=$true; Cols=@("Id","NameNl","SortOrder"); Fields=@(
        @{L="NameNl";P="NameNl";T="text";C=6;R=$true;G=$true}
        @{L="ParentTaskId";P="ParentTaskId";T="int?";C=3;G=$false}
        @{L="SortOrder";P="SortOrder";T="int";C=3;R=$true;G=$true}
    )}
    @{ File="WebshopProductStructureList.razor"; Route="webshop-product-structures"; Hub="/admin/hub/webshop"; Icon="bi-list-nested"; Title="Product category"; Desc="Webshop product grouping."; Entity="product category"; Id="Id"; Port="IWebshopProductStructureAdminPort"; Dto="WebshopProductStructure"; EditId="Id"; Delete=$true; Cols=@("Id","NameEn","NameNl"); Fields=@(
        @{L="NameEn";P="NameEn";T="text";C=4;R=$true;G=$true}
        @{L="NameNl";P="NameNl";T="text";C=4;R=$true;G=$true}
        @{L="NameFr";P="NameFr";T="text";C=4;R=$true;G=$false}
        @{L="ParentTaskId";P="ParentTaskId";T="int?";C=4;G=$false}
    )}
    @{ File="ProductPriceList.razor"; Route="product-prices"; Hub="/admin/hub/catalog"; Icon="bi-currency-euro"; Title="Product price"; Desc="Gross and net prices per product."; Entity="product price"; Id="Id"; Port="IProductPriceAdminPort"; Dto="ProductPrice"; EditId="Id"; Delete=$true; Cols=@("Id","ProductId","GrossSalesPrice","GrossPurchasePrice"); Fields=@(
        @{L="ProductId";P="ProductId";T="int";C=3;R=$true;G=$true}
        @{L="Valid from";P="FromAddress";T="date";C=3;R=$true;G=$false}
        @{L="Valid to";P="ValidTo";T="date?";C=3;G=$false}
        @{L="Gross sales";P="GrossSalesPrice";T="decimal";C=3;R=$true;G=$true}
        @{L="Gross purchase";P="GrossPurchasePrice";T="decimal";C=3;R=$true;G=$true}
        @{L="Net purchase";P="NetPurchasePrice";T="decimal";C=3;G=$false}
        @{L="Base price";P="BasePrice";T="decimal";C=3;G=$false}
    )}
    @{ File="ProductTierList.razor"; Route="product-tiers"; Hub="/admin/hub/catalog"; Icon="bi-bar-chart-steps"; Title="Quantity tier"; Desc="Volume discount tiers."; Entity="quantity tier"; Id="Id"; Port="IProductQuantityTierAdminPort"; Dto="ProductQuantityTier"; EditId="Id"; Delete=$true; Cols=@("Id","ProductId","MinimumQuantity","Discount"); Fields=@(
        @{L="ProductId";P="ProductId";T="int";C=4;R=$true;G=$true}
        @{L="Minimum quantity";P="MinimumQuantity";T="decimal";C=4;R=$true;G=$true}
        @{L="Discount %";P="Discount";T="decimal";C=4;R=$true;G=$true}
    )}
    @{ File="ProductOptionList.razor"; Route="product-options"; Hub="/admin/hub/catalog"; Icon="bi-sliders"; Title="Product option"; Desc="Configurable product options."; Entity="product option"; Id="Id"; Port="IProductOptionAdminPort"; Dto="ProductOption"; EditId="Id"; Delete=$true; Cols=@("Id","ProductId","NameEn","SortOrder"); Fields=@(
        @{L="ProductId";P="ProductId";T="int";C=3;R=$true;G=$true}
        @{L="NameEn";P="NameEn";T="text";C=5;R=$true;G=$true}
        @{L="ValueType";P="ValueType";T="text";C=4;R=$true;G=$true}
        @{L="Tag";P="Tag";T="text";C=4;R=$true;G=$false}
        @{L="SortOrder";P="SortOrder";T="int";C=2;R=$true;G=$true}
        @{L="Required";P="IsRequired";T="bool";C=2;G=$false}
        @{L="Calculate price";P="CalculatePrice";T="bool";C=2;G=$false}
    )}
    @{ File="PriceListCategoryList.razor"; Route="price-list-categories"; Hub="/admin/hub/catalog"; Icon="bi-tags"; Title="Price list category"; Desc="Price list sections."; Entity="price list category"; Id="Id"; Port="IPriceListCategoryAdminPort"; Dto="PriceListCategory"; EditId="Id"; Delete=$true; Cols=@("Id","Name","SortOrder"); Fields=@(
        @{L="Name";P="Name";T="text";C=4;R=$true;G=$true}
        @{L="NameFr";P="NameFr";T="text";C=4;R=$true;G=$true}
        @{L="SortOrder";P="SortOrder";T="int";C=2;R=$true;G=$true}
        @{L="Color";P="Color";T="text";C=2;G=$false}
    )}
    @{ File="ManufacturerList.razor"; Route="manufacturers"; Hub="/admin/hub/catalog"; Icon="bi-building"; Title="Manufacturer"; Desc="Manufacturer master data."; Entity="manufacturer"; Id="ManufacturerId"; Port="IManufacturerAdminPort"; Dto="Manufacturer"; EditId="ManufacturerId"; Delete=$true; Cols=@("ManufacturerId","Name","Email"); Fields=@(
        @{L="Name";P="Name";T="text";C=6;R=$true;G=$true}
        @{L="CityId";P="CityId";T="int?";C=3;G=$true}
        @{L="Email";P="Email";T="text";C=6;G=$true}
        @{L="Phone";P="Phone";T="text";C=4;G=$false}
    )}
    @{ File="SupplierList.razor"; Route="suppliers"; Hub="/admin/hub/catalog"; Icon="bi-truck"; Title="Supplier"; Desc="Supplier master data."; Entity="supplier"; Id="SupplierId"; Port="ISupplierAdminPort"; Dto="Supplier"; EditId="SupplierId"; Delete=$true; Cols=@("SupplierId","Name","Email","IsInactive"); Fields=@(
        @{L="Name";P="Name";T="text";C=6;R=$true;G=$true}
        @{L="CityId";P="CityId";T="int?";C=3;G=$false}
        @{L="LanguageId";P="LanguageId";T="int";C=3;R=$true;G=$false}
        @{L="Email";P="Email";T="text";C=6;G=$true}
        @{L="Inactive";P="IsInactive";T="bool";C=2;G=$true}
    )}
    @{ File="DeliveryAddressList.razor"; Route="delivery-addresses"; Hub="/admin/hub/customers"; Icon="bi-geo-alt"; Title="Delivery address"; Desc="Customer shipping addresses."; Entity="delivery address"; Id="Id"; Port="ICustomerDeliveryAddressAdminPort"; Dto="CustomerDeliveryAddress"; EditId="Id"; Delete=$true; Cols=@("Id","CustomerId","Name","CityId"); Fields=@(
        @{L="CustomerId";P="CustomerId";T="int?";C=3;G=$true}
        @{L="Name";P="Name";T="text";C=5;R=$true;G=$true}
        @{L="Street";P="Straat";T="text";C=5;R=$true;G=$true}
        @{L="Number";P="Number";T="text";C=2;R=$true;G=$false}
        @{L="CityId";P="CityId";T="int";C=3;R=$true;G=$true}
    )}
    @{ File="CustomerDiscountList.razor"; Route="customer-discounts"; Hub="/admin/hub/customers"; Icon="bi-percent"; Title="Product discount"; Desc="Customer-specific product discounts."; Entity="product discount"; Id="Id"; Port="ICustomerProductDiscountAdminPort"; Dto="CustomerProductDiscount"; EditId="Id"; Delete=$true; Cols=@("Id","CustomerId","ProductId","DiscountPercentage"); Fields=@(
        @{L="CustomerId";P="CustomerId";T="int";C=3;R=$true;G=$true}
        @{L="ProductId";P="ProductId";T="int";C=3;R=$true;G=$true}
        @{L="Discount %";P="DiscountPercentage";T="decimal?";C=3;G=$true}
        @{L="Valid from";P="FromAddress";T="date";C=3;R=$true;G=$false}
    )}
    @{ File="CustomerTypeList.razor"; Route="customer-types"; Hub="/admin/hub/customers"; Icon="bi-layers"; Title="Customer type"; Desc="Dealer, contractor, consumer types."; Entity="customer type"; Id="KlantTypeId"; Port="ICustomerTypeAdminPort"; Dto="CustomerType"; EditId="KlantTypeId"; Delete=$true; Cols=@("KlantTypeId","CustomerTypeName","BaseDiscount"); Fields=@(
        @{L="Name";P="CustomerTypeName";T="text";C=4;R=$true;G=$true}
        @{L="NameFr";P="CustomerTypeNameFr";T="text";C=4;R=$true;G=$false}
        @{L="Base discount";P="BaseDiscount";T="decimal";C=2;R=$true;G=$true}
        @{L="SortOrder";P="SortOrder";T="int";C=2;R=$true;G=$true}
    )}
    @{ File="CustomerList.razor"; Route="customers"; Hub="/admin/hub/customers"; Icon="bi-person-badge"; Title="Customer"; Desc="B2B customer accounts."; Entity="customer"; Id="CustomerId"; Port="ICustomerAdminPort"; Dto="Customer"; EditId="CustomerId"; Delete=$true; Cols=@("CustomerId","CustomerName","WebshopLogin","CustomerEmail"); Fields=@(
        @{L="Name";P="CustomerName";T="text";C=6;R=$true;G=$true}
        @{L="Webshop login";P="WebshopLogin";T="text";C=4;G=$true}
        @{L="Email";P="CustomerEmail";T="text";C=6;R=$true;G=$true}
        @{L="Phone";P="CustomerPhone";T="text";C=4;R=$true;G=$false}
        @{L="CustomerTypeId";P="CustomerTypeId";T="int";C=3;R=$true;G=$true}
        @{L="CityId";P="CustomerCityId";T="int";C=3;R=$true;G=$false}
        @{L="Locked";P="Locked";T="bool";C=2;G=$false}
    )}
    @{ File="OrderStatusList.razor"; Route="order-statuses"; Hub="/admin/hub/sales"; Icon="bi-flag"; Title="Order status"; Desc="Order workflow statuses."; Entity="order status"; Id="Id"; Port="IOrderStatusAdminPort"; Dto="OrderStatus"; EditId="Id"; Delete=$true; Cols=@("Id","Name","ScreenMode"); Fields=@(
        @{L="Name";P="Name";T="text";C=4;R=$true;G=$true}
        @{L="NameFr";P="NameFr";T="text";C=4;R=$true;G=$false}
        @{L="ScreenMode";P="ScreenMode";T="text";C=4;R=$true;G=$true}
        @{L="SortOrder";P="SortOrder";T="int?";C=2;G=$true}
    )}
    @{ File="DeliveryTypeList.razor"; Route="delivery-types"; Hub="/admin/hub/sales"; Icon="bi-truck-flatbed"; Title="Delivery type"; Desc="Pickup, delivery, installation."; Entity="delivery type"; Id="Id"; Port="IDeliveryTypeAdminPort"; Dto="DeliveryType"; EditId="Id"; Delete=$true; Cols=@("Id","Name","IncludeInstallationCost"); Fields=@(
        @{L="Name";P="Name";T="text";C=5;R=$true;G=$true}
        @{L="NameFr";P="NameFr";T="text";C=5;R=$true;G=$false}
        @{L="Installation cost";P="IncludeInstallationCost";T="bool";C=3;G=$true}
    )}
    @{ File="OrderList.razor"; Route="orders"; Hub="/admin/hub/sales"; Icon="bi-receipt"; Title="Order"; Desc="Sales orders."; Entity="order"; Id="Id"; Port="IOrderAdminPort"; Dto="Order"; ListDto="OrderSummary"; EditId="Id"; Delete=$false; Cols=@("ProjectId","CreatedAt","IsAccepted"); Fields=@(
        @{L="ProjectId";P="ProjectId";T="int";C=3;R=$true;G=$true}
        @{L="Created at";P="CreatedAt";T="date";C=3;R=$true;G=$true}
        @{L="DeliveryTypeId";P="DeliveryTypeId";T="int";C=3;R=$true;G=$true}
        @{L="Accepted";P="IsAccepted";T="bool";C=2;G=$true}
    )}
    @{ File="ProductStockList.razor"; Route="product-stock"; Hub="/admin/hub/stock"; Icon="bi-boxes"; Title="Product stock"; Desc="Quantity per warehouse location."; Entity="product stock"; Id="Id"; Port="IProductStockLocationAdminPort"; Dto="ProductStockLocation"; EditId="Id"; Delete=$true; Cols=@("Id","ProductId","StockLocationId","Quantity"); Fields=@(
        @{L="StockLocationId";P="StockLocationId";T="int";C=3;R=$true;G=$true}
        @{L="ProductId";P="ProductId";T="int";C=3;R=$true;G=$true}
        @{L="Quantity";P="Quantity";T="decimal";C=3;R=$true;G=$true}
        @{L="Min quantity";P="MinQuantity";T="decimal";C=3;G=$false}
        @{L="Max quantity";P="MaxQuantity";T="decimal";C=3;G=$false}
    )}
    @{ File="StockLocationList.razor"; Route="stock-locations"; Hub="/admin/hub/stock"; Icon="bi-house-door"; Title="Stock location"; Desc="Warehouses and storage areas."; Entity="stock location"; Id="Id"; Port="IStockLocationAdminPort"; Dto="StockLocation"; EditId="Id"; Delete=$true; Cols=@("Id","Name","IsWarehouse"); Fields=@(
        @{L="Name";P="Name";T="text";C=8;R=$true;G=$true}
        @{L="Warehouse";P="IsWarehouse";T="bool";C=3;G=$true}
    )}
    @{ File="PaymentMethodList.razor"; Route="payment-methods"; Hub="/admin/hub/settings"; Icon="bi-credit-card"; Title="Payment method"; Desc="PrePay and PostPay methods."; Entity="payment method"; Id="Id"; Port="IPaymentMethodAdminPort"; Dto="PaymentMethod"; EditId="Id"; Delete=$true; Cols=@("Id","NameEn","IsPrePay"); Fields=@(
        @{L="NameEn";P="NameEn";T="text";C=4;R=$true;G=$true}
        @{L="NameNl";P="NameNl";T="text";C=4;R=$true;G=$false}
        @{L="NameFr";P="NameFr";T="text";C=4;R=$true;G=$false}
        @{L="PrePay";P="IsPrePay";T="bool";C=2;G=$true}
        @{L="PostPay";P="IsPostPay";T="bool";C=2;G=$false}
    )}
    @{ File="StaffUserList.razor"; Route="staff-users"; Hub="/admin/hub/settings"; Icon="bi-person-workspace"; Title="Staff user"; Desc="Internal staff accounts."; Entity="staff user"; Id="Id"; Port="IStaffUserAdminPort"; Dto="StaffUser"; EditId="Id"; Delete=$true; Cols=@("Id","Login","FirstName","LastName"); Fields=@(
        @{L="Login";P="Login";T="text";C=4;R=$true;G=$true}
        @{L="First name";P="FirstName";T="text";C=4;R=$true;G=$true}
        @{L="Last name";P="LastName";T="text";C=4;R=$true;G=$true}
        @{L="Job title";P="JobTitle";T="text";C=4;G=$false}
    )}
    @{ File="UserGroupList.razor"; Route="user-groups"; Hub="/admin/hub/settings"; Icon="bi-people"; Title="User group"; Desc="Staff teams."; Entity="user group"; Id="Id"; Port="IUserGroupAdminPort"; Dto="UserGroup"; EditId="Id"; Delete=$true; Cols=@("Id","Name","IsInstallationTeam"); Fields=@(
        @{L="Name";P="Name";T="text";C=6;R=$true;G=$true}
        @{L="Installation";P="IsInstallationTeam";T="bool";C=2;G=$true}
        @{L="Service";P="IsServiceTeam";T="bool";C=2;G=$false}
        @{L="Transport";P="IsTransportTeam";T="bool";C=2;G=$false}
    )}
    @{ File="VatTypeList.razor"; Route="vat-types"; Hub="/admin/hub/settings"; Icon="bi-file-earmark-text"; Title="VAT type"; Desc="VAT percentages and invoice text."; Entity="VAT type"; Id="Id"; Port="IVatTypeAdminPort"; Dto="VatType"; EditId="Id"; Delete=$true; Cols=@("Id","Name","Percentage"); Fields=@(
        @{L="Name";P="Name";T="text";C=4;R=$true;G=$true}
        @{L="Percentage";P="Percentage";T="decimal";C=3;R=$true;G=$true}
        @{L="Invoice text";P="InvoiceText";T="text";C=5;R=$true;G=$true}
    )}
)

function Get-DefaultModel($e) {
    switch ($e.Dto) {
        "WebshopStructure" { return "new WebshopStructureEditDto { SortOrder = 1 }" }
        "ProductPrice" { return "new ProductPriceEditDto { ProductId = 1, FromAddress = DateTime.UtcNow.Date }" }
        "ProductQuantityTier" { return "new ProductQuantityTierEditDto { ProductId = 1, MinimumQuantity = 1 }" }
        "ProductOption" { return "new ProductOptionEditDto { ProductId = 1, ValueType = `"Text`", Tag = `"`", SortOrder = 1 }" }
        "Supplier" { return "new SupplierEditDto { LanguageId = 1, IsInactive = false }" }
        "CustomerType" { return "new CustomerTypeEditDto { PaymentTermId = 1, VatSystemId = 1, DeliveryTypeId = 1, SortOrder = 1 }" }
        "Customer" { return "new CustomerEditDto { CustomerTypeId = 1, CustomerCityId = 1, DeliveryTypeId = 1, BetaaltermijnId = 1 }" }
        "Order" { return "new OrderEditDto { ProjectId = 1, DeliveryTypeId = 1, CreatedAt = DateTime.UtcNow.Date }" }
        "ProductStockLocation" { return "new ProductStockLocationEditDto { StockLocationId = 1, ProductId = 1, MaxQuantity = 100 }" }
        "VatType" { return "new VatTypeEditDto { Percentage = 21 }" }
        default { return "new $($e.Dto)EditDto()" }
    }
}

function Get-FormControl($f) {
    $id = "f-$($f.P)".ToLower()
    $req = if ($f.R) { " required" } else { "" }
    $label = "$($f.L)$(if ($f.R) { ' *' })"
    switch ($f.T) {
        "bool" {
            return @"
                        <div class="col-md-$($f.C) d-flex align-items-end">
                            <div class="form-check mb-1">
                                <InputCheckbox class="form-check-input" @bind-Value="_model.$($f.P)" id="$id" />
                                <label class="form-check-label" for="$id">$($f.L)</label>
                            </div>
                        </div>
"@
        }
        { $_ -in @("int","int?","decimal","decimal?") } {
            return @"
                        <div class="col-md-$($f.C)">
                            <label class="form-label" for="$id">$label</label>
                            <InputNumber id="$id" class="form-control form-control-sm" @bind-Value="_model.$($f.P)"$req />
                        </div>
"@
        }
        { $_ -in @("date","date?") } {
            return @"
                        <div class="col-md-$($f.C)">
                            <label class="form-label" for="$id">$label</label>
                            <InputDate id="$id" class="form-control form-control-sm" @bind-Value="_model.$($f.P)"$req />
                        </div>
"@
        }
        default {
            return @"
                        <div class="col-md-$($f.C)">
                            <label class="form-label" for="$id">$label</label>
                            <InputText id="$id" class="form-control form-control-sm" @bind-Value="_model.$($f.P)"$req />
                        </div>
"@
        }
    }
}

function Get-ListMethod($dto) {
    switch ($dto) {
        "WebshopStructure" { "GetWebshopStructuresAsync" }
        "WebshopProductStructure" { "GetWebshopProductStructuresAsync" }
        "ProductPrice" { "GetProductPricesAsync" }
        "ProductQuantityTier" { "GetProductQuantityTiersAsync" }
        "ProductOption" { "GetProductOptionsAsync" }
        "PriceListCategory" { "GetPriceListCategoriesAsync" }
        "Manufacturer" { "GetManufacturersAsync" }
        "Supplier" { "GetSuppliersAsync" }
        "CustomerDeliveryAddress" { "GetCustomerDeliveryAddressesAsync" }
        "CustomerProductDiscount" { "GetCustomerProductDiscountsAsync" }
        "CustomerType" { "GetCustomerTypesAsync" }
        "Customer" { "GetCustomersAsync" }
        "OrderStatus" { "GetOrderStatusesAsync" }
        "DeliveryType" { "GetDeliveryTypesAsync" }
        "Order" { "GetOrdersAsync" }
        "ProductStockLocation" { "GetProductStockLocationsAsync" }
        "StockLocation" { "GetStockLocationsAsync" }
        "PaymentMethod" { "GetPaymentMethodsAsync" }
        "StaffUser" { "GetStaffUsersAsync" }
        "UserGroup" { "GetUserGroupsAsync" }
        "VatType" { "GetVatTypesAsync" }
        default { throw "No list method for $dto" }
    }
}

function Get-GridCell($e, $col) {
    switch ($col) {
        "IsAccepted" { return "@(item.IsAccepted ? `"Yes`" : `"No`")" }
        "IsInactive" { return "@(item.IsInactive ? `"Yes`" : `"No`")" }
        "IsPrePay" { return "@(item.IsPrePay ? `"Yes`" : `"No`")" }
        "IsWarehouse" { return "@(item.IsWarehouse == true ? `"Yes`" : `"No`")" }
        "IncludeInstallationCost" { return "@(item.IncludeInstallationCost ? `"Yes`" : `"No`")" }
        "Locked" { return "@(item.Locked ? `"Yes`" : `"No`")" }
        "CreatedAt" { return "@item.CreatedAt.ToString(`"yyyy-MM-dd`")" }
        default { return "@item.$col" }
    }
}

foreach ($e in $entities) {
    $ns = switch ($e.Dto) {
        "CustomerDeliveryAddress" { "CustomerDeliveryAddresses" }
        "CustomerProductDiscount" { "CustomerProductDiscounts" }
        "CustomerType" { "CustomerTypes" }
        "Customer" { "Customers" }
        "OrderStatus" { "OrderStatuses" }
        "Order" { "Orders" }
        "ProductStockLocation" { "ProductStockLocations" }
        default { $e.Dto + "s" }
    }
    if ($e.Dto -eq "WebshopStructure") { $ns = "WebshopStructures" }
    if ($e.Dto -eq "WebshopProductStructure") { $ns = "WebshopProductStructures" }
    if ($e.Dto -eq "ProductPrice") { $ns = "ProductPrices" }
    if ($e.Dto -eq "ProductQuantityTier") { $ns = "ProductQuantityTiers" }
    if ($e.Dto -eq "ProductOption") { $ns = "ProductOptions" }
    if ($e.Dto -eq "PriceListCategory") { $ns = "PriceListCategories" }
    if ($e.Dto -eq "Manufacturer") { $ns = "Manufacturers" }
    if ($e.Dto -eq "Supplier") { $ns = "Suppliers" }
    if ($e.Dto -eq "DeliveryType") { $ns = "DeliveryTypes" }
    if ($e.Dto -eq "StockLocation") { $ns = "StockLocations" }
    if ($e.Dto -eq "PaymentMethod") { $ns = "PaymentMethods" }
    if ($e.Dto -eq "StaffUser") { $ns = "StaffUsers" }
    if ($e.Dto -eq "UserGroup") { $ns = "UserGroups" }
    if ($e.Dto -eq "VatType") { $ns = "VatTypes" }

    $listDto = if ($e.ListDto) { $e.ListDto } else { $e.Dto }
    $formFields = ($e.Fields | ForEach-Object { Get-FormControl $_ }) -join "`n"
    $displayCols = @($e.Cols | Where-Object { $_ -ne $e.Id })
    $colCount = $displayCols.Count + 2
    $headers = ($displayCols | ForEach-Object { "                        <th>$_</th>" }) -join "`n"
    $cells = ($displayCols | ForEach-Object { $c = Get-GridCell $e $_; "                                <td>$c</td>" }) -join "`n"
    $deleteBtn = if ($e.Delete) {
@'
                                        <button type="button" class="btn btn-sm btn-danger" @onclick="() => PromptDelete(item)" title="Delete">
                                            <i class="bi bi-trash"></i>
                                        </button>
'@
    } else { "" }
    $deleteModal = if ($e.Delete) { @"

@if (_showDeleteConfirm && _itemToDelete is not null)
{
    <div class="modal fade show d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header border-0"><h5 class="modal-title">Confirm deletion</h5><button type="button" class="btn-close" @onclick="CancelDelete" disabled="@_deleting"></button></div>
                <div class="modal-body"><p class="mb-0">Delete record <strong>#@_itemToDelete.$($e.Id)</strong>?</p></div>
                <div class="modal-footer border-0">
                    <button type="button" class="btn btn-secondary" @onclick="CancelDelete" disabled="@_deleting">Cancel</button>
                    <button type="button" class="btn btn-danger" @onclick="ConfirmDeleteAsync" disabled="@_deleting">Delete</button>
                </div>
            </div>
        </div>
    </div>
}
"@ } else { "" }

    $listMethod = Get-ListMethod $e.Dto
    $deleteFields = if ($e.Delete) { ", _deleting, _showDeleteConfirm" } else { "" }
    $deleteState = if ($e.Delete) { "`n    private $($listDto)Dto? _itemToDelete;" } else { "" }
    $deleteMethods = if ($e.Delete) { @"

    private void PromptDelete($($listDto)Dto item) { _itemToDelete = item; _showDeleteConfirm = true; }
    private void CancelDelete() { _showDeleteConfirm = false; _itemToDelete = null; }
    private async Task ConfirmDeleteAsync() {
        if (_itemToDelete is null) return;
        _deleting = true;
        try {
            if (!await AdminPort.DeleteAsync(_itemToDelete.$($e.Id), _cts?.Token ?? CancellationToken.None)) { _formError = "Record could not be deleted."; return; }
            if (_model.$($e.EditId) == _itemToDelete.$($e.Id)) await CancelFormAsync();
            CancelDelete(); await LoadGridAsync(_cts?.Token ?? CancellationToken.None);
        } finally { _deleting = false; }
    }
"@ } else { "" }

    $content = @"
@page "/admin/$($e.Route)"
@layout AdminLayout
@attribute [Authorize(Policy = AppPolicies.AdminOrManager)]
@using WebShopABMATIC.Application.Admin.$ns
@using WebShopABMATIC.Application.Common
@using WebShopABMATIC.Application.Auth
@using WebShopABMATIC.Application.Ports
@inject $($e.Port) AdminPort
@implements IAsyncDisposable

<PageTitle>$($e.Title)</PageTitle>

<div class="entity-page">
    <a class="btn btn-secondary btn-back mb-3" href="$($e.Hub)" data-enhance-nav="false"><span class="oi oi-arrow-left"></span> Back to list</a>
    <div class="entity-page-header page-header">
        <h1 class="h3 mb-1"><i class="bi $($e.Icon) text-primary"></i> $($e.Title)</h1>
        <p class="mb-0">$($e.Desc)</p>
    </div>
    <div class="card shadow-sm border entity-form-card admin-form-card">
        <div class="card-header card-header-catalog"><h2 class="h6 mb-0 fw-bold">@(_isEditing ? "Edit $($e.Entity)" : "Create $($e.Entity)")</h2></div>
        <div class="card-body">
            @if (_formLoading) { <p class="text-muted mb-0">Loading…</p> }
            else {
                <EditForm Model="_model" OnValidSubmit="SaveAsync">
                    <DataAnnotationsValidator />
                    <div class="entity-form-fields"><div class="row g-2">
$formFields
                        <div class="col-12 d-flex gap-2 pt-1">
                            <button type="submit" class="btn btn-primary" disabled="@_saving">@(_isEditing ? "Update" : "Create")</button>
                            <button type="button" class="btn btn-secondary" @onclick="CancelFormAsync" disabled="@_saving"><i class="bi bi-x me-1"></i>Cancel</button>
                        </div>
                    </div></div>
                </EditForm>
                @if (!string.IsNullOrEmpty(_formError)) { <div class="alert alert-danger mt-3 mb-0">@_formError</div> }
            }
        </div>
    </div>
    <div class="card shadow-sm border entity-grid-card">
        <div class="card-header bg-white border-bottom py-3 d-flex flex-wrap align-items-center justify-content-between gap-3">
            <h2 class="h6 mb-0 fw-bold">$($e.Title) list</h2>
            <div class="grid-search-wrap"><div class="input-group input-group-sm">
                <input type="search" class="form-control" placeholder="Search…" @bind="_searchDraft" @bind:event="oninput" @onkeydown="OnSearchKeyDown" />
                <span class="input-group-text"><i class="bi bi-search"></i></span>
            </div></div>
        </div>
        <div class="table-responsive">
            <table class="table table-striped table-hover mb-0">
                <thead class="table-dark"><tr><th>$($e.Id)</th>
$headers
                    <th class="text-center">Actions</th></tr></thead>
                <tbody>
                    @if (_loading) { <tr><td colspan="$colCount" class="text-center text-muted py-4">Loading…</td></tr> }
                    else if (!string.IsNullOrEmpty(_gridError)) { <tr><td colspan="$colCount" class="text-center text-danger py-4">@_gridError</td></tr> }
                    else if (_result is null || _result.Items.Count == 0) { <tr><td colspan="$colCount" class="text-center text-muted py-4">No records found.</td></tr> }
                    else { @foreach (var item in _result.Items) {
                        <tr @key="item.$($e.Id)" class="@(_model.$($e.EditId) == item.$($e.Id) ? "table-active" : null)">
                            <td>@item.$($e.Id)</td>
$cells
                            <td class="text-center"><div class="btn-group btn-group-sm">
                                <a class="btn btn-sm btn-primary" href="@(`$`"/admin/$($e.Route)?edit={item.$($e.Id)}`")" data-enhance-nav="false"><i class="bi bi-pencil"></i></a>
$deleteBtn
                            </div></td>
                        </tr> } }
                </tbody>
            </table>
        </div>
        @if (_result is not null && !_loading) { <div class="card-footer bg-white text-muted small py-2">@_result.TotalCount total · page @_result.Page</div> }
    </div>
</div>
$deleteModal

@code {
    [SupplyParameterFromQuery(Name = "edit")] public int? EditId { get; set; }
    private CancellationTokenSource? _cts;
    private $($e.Dto)ListFilter _filter = new();
    private $($e.Dto)EditDto _model = $(Get-DefaultModel $e);
    private string _searchDraft = string.Empty;
    private bool _loading = true, _formLoading, _saving$deleteFields, _disposed, _initialized;
    private string? _formError, _gridError;
    private PagedResult<$($listDto)Dto>? _result;$deleteState
    private bool _isEditing => _model.$($e.EditId) > 0;

    protected override async Task OnInitializedAsync() {
        _cts = new CancellationTokenSource();
        await LoadGridAsync(_cts.Token);
        if (EditId is > 0) await EditAsync(EditId.Value);
        _initialized = true;
    }
    protected override async Task OnParametersSetAsync() {
        if (!_initialized || _disposed || EditId is not > 0 || _model.$($e.EditId) == EditId) return;
        await EditAsync(EditId.Value);
    }
    private async Task LoadGridAsync(CancellationToken ct) {
        _loading = true; _gridError = null;
        try { _result = await AdminPort.$listMethod(_filter, ct); }
        catch (OperationCanceledException) { return; }
        catch (Exception ex) { _result = null; _gridError = ex.Message; }
        finally { _loading = false; await InvokeAsync(StateHasChanged); }
    }
    private async Task ApplySearchAsync() { _filter.Search = _searchDraft; _filter.Page = 1; _cts?.Cancel(); _cts = new CancellationTokenSource(); await LoadGridAsync(_cts.Token); }
    private async Task OnSearchKeyDown(KeyboardEventArgs e) { if (e.Key == "Enter") await ApplySearchAsync(); }
    private async Task EditAsync(int id) {
        _formLoading = true; _formError = null;
        try {
            var item = await AdminPort.GetForEditAsync(id, _cts?.Token ?? CancellationToken.None);
            if (item is null) { _formError = "Record not found."; return; }
            _model = item;
        } finally { _formLoading = false; }
    }
    private async Task SaveAsync() {
        _saving = true; _formError = null;
        try {
            var id = await AdminPort.SaveAsync(_model, _cts?.Token ?? CancellationToken.None);
            await LoadGridAsync(_cts?.Token ?? CancellationToken.None);
            await EditAsync(id);
        } catch (Exception ex) { _formError = ex.Message; }
        finally { _saving = false; }
    }
    private Task CancelFormAsync() { _formError = null; _model = $(Get-DefaultModel $e); return Task.CompletedTask; }$deleteMethods
    public async ValueTask DisposeAsync() { _disposed = true; if (_cts is not null) { await _cts.CancelAsync(); _cts.Dispose(); } }
}
"@

    $path = Join-Path $outDir $e.File
    Set-Content -Path $path -Value $content -Encoding UTF8
    Write-Host "Created $path"
}

Write-Host "Done. Generated $($entities.Count) razor pages."
