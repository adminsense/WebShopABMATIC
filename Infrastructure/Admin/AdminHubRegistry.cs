using WebShopABMATIC.Application.Admin.Hubs;
using WebShopABMATIC.Application.Ports;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class AdminHubRegistry : IAdminHubPort
{
    private static readonly IReadOnlyList<AdminHubDefinitionDto> Hubs =
    [
        new()
        {
            Id = "webshop",
            Title = "Webshop",
            Subtitle = "Storefront structure: navigation and product categories",
            IconClass = "oi-globe",
            Cards =
            [
                Card("WebshopStructure", "Webshop structure", "Manage catalog navigation tree", "/admin/webshop-structures", "oi-folder"),
                Card("WebshopProductStructure", "Product categories", "Webshop product grouping", "/admin/webshop-product-structures", "oi-list")
            ]
        },
        new()
        {
            Id = "catalog",
            Title = "Catalog",
            Subtitle = "Core catalog entities: products, prices, options, and suppliers",
            IconClass = "oi-cog",
            Cards =
            [
                Card("Product", "Product", "Manage products and webshop visibility", "/admin/products", "oi-box"),
                Card("ProductPrice", "Product price", "Gross and net prices", "/admin/product-prices", "oi-dollar"),
                Card("ProductQuantityTier", "Quantity tiers", "Volume discounts", "/admin/product-tiers", "oi-bar-chart"),
                Card("ProductOption", "Product options", "Configurable options", "/admin/product-options", "oi-wrench"),
                Card("PriceListCategory", "Price list category", "Price list sections", "/admin/price-list-categories", "oi-tag"),
                Card("Manufacturer", "Manufacturer", "Manufacturer master data", "/admin/manufacturers", "oi-people"),
                Card("Supplier", "Supplier", "Supplier master data", "/admin/suppliers", "oi-briefcase")
            ]
        },
        new()
        {
            Id = "customers",
            Title = "Customers",
            Subtitle = "Customer accounts, addresses, and discounts",
            IconClass = "oi-people",
            Cards =
            [
                Card("Customer", "Customer", "B2B customer accounts", "/admin/customers", "oi-person"),
                Card("CustomerDeliveryAddress", "Delivery address", "Shipping addresses", "/admin/delivery-addresses", "oi-map-marker"),
                Card("CustomerProductDiscount", "Product discount", "Customer-specific discounts", "/admin/customer-discounts", "oi-pie-chart"),
                Card("CustomerType", "Customer type", "Dealer, contractor, consumer", "/admin/customer-types", "oi-layers")
            ]
        },
        new()
        {
            Id = "sales",
            Title = "Sales",
            Subtitle = "Orders, statuses, and delivery types",
            IconClass = "oi-cart",
            Cards =
            [
                Card("Order", "Order", "Orders and order lines", "/admin/orders", "oi-clipboard"),
                Card("OrderStatus", "Order status", "Workflow statuses", "/admin/order-statuses", "oi-flag"),
                Card("DeliveryType", "Delivery type", "Pickup, delivery, installation", "/admin/delivery-types", "oi-transfer")
            ]
        },
        new()
        {
            Id = "stock",
            Title = "Stock",
            Subtitle = "Inventory locations and product stock levels",
            IconClass = "oi-box",
            Cards =
            [
                Card("ProductStockLocation", "Product stock", "Quantity per location", "/admin/product-stock", "oi-box"),
                Card("StockLocation", "Stock location", "Warehouses and storage", "/admin/stock-locations", "oi-home")
            ]
        },
        new()
        {
            Id = "settings",
            Title = "Settings",
            Subtitle = "Payment, users, groups, and VAT",
            IconClass = "oi-wrench",
            Cards =
            [
                Card("PaymentMethod", "Payment method", "PrePay and PostPay methods", "/admin/payment-methods", "oi-credit-card"),
                Card("StaffUser", "Staff user", "Internal users and flags", "/admin/staff-users", "oi-person"),
                Card("UserGroup", "User group", "Staff teams", "/admin/user-groups", "oi-people"),
                Card("VatType", "VAT type", "VAT percentages", "/admin/vat-types", "oi-document")
            ]
        },
        new()
        {
            Id = "profile",
            Title = "My profile",
            Subtitle = "Staff user profile and preferences",
            IconClass = "oi-person",
            Cards =
            [
                new AdminHubCardDto
                {
                    Entity = "StaffUser",
                    Title = "Profile",
                    Description = "Edit your staff user profile",
                    ListRoute = "/admin/profile",
                    FormRoute = "/admin/profile",
                    IconClass = "oi-person",
                    ColorHex = "#5b8def"
                }
            ]
        }
    ];

    public IReadOnlyList<AdminHubDefinitionDto> GetHubDefinitions() => Hubs;

    public AdminHubDefinitionDto? GetHub(string hubId) =>
        Hubs.FirstOrDefault(h => h.Id.Equals(hubId, StringComparison.OrdinalIgnoreCase));

    private static AdminHubCardDto Card(string entity, string title, string description, string route, string icon) =>
        new()
        {
            Entity = entity,
            Title = title,
            Description = description,
            ListRoute = route,
            IconClass = icon,
            ColorHex = "#5b8def"
        };
}
