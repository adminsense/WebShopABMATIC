using WebShopABMATIC.Data.Entities;

namespace WebShopABMATIC.Infrastructure.Persistence;

internal static class AdminCrudDefaults
{
    public static object Create(string routeKey) => routeKey.ToLowerInvariant() switch
    {
        "webshop-structures" => new WebshopStructure { NameNl = string.Empty, SortOrder = 1 },
        "webshop-product-structures" => new WebshopProductStructure { NameEn = string.Empty, NameNl = string.Empty, NameFr = string.Empty },
        "product-prices" => new ProductPrice
        {
            ProductId = 1,
            FromAddress = DateTime.UtcNow.Date,
            GrossSalesPrice = 0,
            GrossPurchasePrice = 0,
            NetPurchasePrice = 0,
            BasePrice = 0,
            AssemblyPrice = 0,
            InstallationPrice = 0,
            StartupCost = 0,
            PurchaseDiscountPercentage = 0,
            CalculationType = 1
        },
        "product-tiers" => new ProductQuantityTier { ProductId = 1, MinimumQuantity = 1, Discount = 0 },
        "product-options" => new ProductOption
        {
            ProductId = 1,
            Name = string.Empty,
            NameEn = string.Empty,
            NameFr = string.Empty,
            ValueType = "Text",
            Tag = string.Empty,
            IsRequired = false,
            CalculatePrice = false,
            SortOrder = 1,
            ProductionNotesFlag = false,
            QuoteNotesFlag = false
        },
        "price-list-categories" => new PriceListCategory { Name = string.Empty, NameFr = string.Empty, SortOrder = 1 },
        "manufacturers" => new Manufacturer { Name = string.Empty },
        "suppliers" => new Supplier { Name = string.Empty, LanguageId = 1, GeneralLedgerRevenueAccount = 700000, IsInactive = false },
        "delivery-addresses" => new CustomerDeliveryAddress
        {
            Name = string.Empty,
            Straat = string.Empty,
            Number = string.Empty,
            Bus = string.Empty,
            CityId = 1,
            Notes = string.Empty
        },
        "customer-discounts" => new CustomerProductDiscount
        {
            CustomerId = 1,
            ProductId = 1,
            FromAddress = DateTime.UtcNow.Date,
            UserId = 1,
            CreatedAt = DateTime.UtcNow
        },
        "customer-types" => new CustomerType
        {
            CustomerTypeName = string.Empty,
            CustomerTypeNameFr = string.Empty,
            RequiresVatNumber = true,
            PaymentTermId = 1,
            VatSystemId = 1,
            BaseDiscount = 0,
            DeliveryTypeId = 1,
            SortOrder = 1
        },
        "customers" => new Customer
        {
            CustomerName = string.Empty,
            CustomerStreet = string.Empty,
            CustomerHouseNumber = string.Empty,
            CustomerBox = string.Empty,
            CustomerTypeId = 1,
            CustomerPhone = string.Empty,
            CustomerFax = string.Empty,
            CustomerEmail = string.Empty,
            CustomerVatSystemId = 1,
            CustomerStatusId = 1,
            CustomerCityId = 1,
            AccountManagerUserId = 1,
            Locked = false,
            LockedBy = string.Empty,
            DeliveryTypeId = 1,
            CustomerLanguageId = 1,
            CustomerGroup = "Demo",
            BetaaltermijnId = 1,
            DeliveryCustomerTypeId = 1,
            CustomerVatNumber = string.Empty,
            QuoteContactId = 0,
            OrderConfirmationContactId = 0,
            PlanningContactId = 0,
            DeliveryCompleteContactId = 0,
            BillingContactId = 0
        },
        "order-statuses" => new OrderStatus
        {
            Name = string.Empty,
            NameFr = string.Empty,
            ScreenMode = "Order",
            ReserveStock = false,
            AffectsStock = false
        },
        "delivery-types" => new DeliveryType { Name = string.Empty, NameFr = string.Empty, IncludeInstallationCost = false },
        "orders" => new Order
        {
            ProjectId = 1,
            IsAccepted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = 1,
            DeliveryTypeId = 1,
            CustomerTypeId = 1,
            VatTypeId = 1,
            OrderProcessingTypeId = 1,
            PriceListTypeId = 1,
            BetaaltermijnId = 1,
            GeneralDiscount = 0,
            CommissionAmount = 0,
            QuoteValidDays = 30,
            BaseCompanyVatNumberId = 1,
            IsUrgent = false,
            AllowPartialDelivery = true
        },
        "product-stock" => new ProductStockLocation
        {
            StockLocationId = 1,
            ProductId = 1,
            Quantity = 0,
            MinQuantity = 0,
            MaxQuantity = 100,
            ReservedQuantity = 0,
            IsDefault = false
        },
        "stock-locations" => new StockLocation { Name = string.Empty, IsWarehouse = true },
        "payment-methods" => new PaymentMethod { NameEn = string.Empty, NameNl = string.Empty, NameFr = string.Empty, IsPrePay = true, IsPostPay = false },
        "staff-users" => new StaffUser
        {
            Login = string.Empty,
            Password = "ChangeMe!",
            FirstName = string.Empty,
            LastName = string.Empty,
            Address = string.Empty,
            BaseCompaniesId = 1,
            TaalId = 1,
            HiredAt = DateTime.UtcNow.Date,
            Color = 0,
            TextColor = 0
        },
        "user-groups" => new UserGroup { Name = string.Empty, IsInstallationTeam = false, IsServiceTeam = false, IsTransportTeam = false },
        "vat-types" => new VatType
        {
            Name = string.Empty,
            Percentage = 21,
            InvoiceText = string.Empty,
            InvoiceTextEn = string.Empty,
            InvoiceTextFr = string.Empty,
            ExplanationNl = string.Empty,
            ExplanationFr = string.Empty,
            ExplanationEn = string.Empty
        },
        _ => throw new InvalidOperationException($"Unknown entity route: {routeKey}")
    };
}
