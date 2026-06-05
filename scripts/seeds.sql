/* WebShopABMATIC — demo seed data (idempotent)
   Database: WebShopABMATIC on SQL Server MULLER
   Run: sqlcmd -S MULLER -E -d WebShopABMATIC -i scripts\seeds.sql
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

USE [WebShopABMATIC];
GO

-- Remove previous demo rows (child → parent)
DELETE FROM [Files].[AzureFiles];
DELETE FROM [Files].[AzureFileFolders];
DELETE FROM [Projects].[OrderLines];
DELETE FROM [Projects].[Orders];
DELETE FROM [Projects].[Project];
DELETE FROM [Products].[ProductStockLocations];
DELETE FROM [Products].[Product];
DELETE FROM [Products].[WebshopStructures];
DELETE FROM [Products].[StockLocations];
DELETE FROM [Customers].[Customers];
DELETE FROM [Customers].[CustomerTypes];
DELETE FROM [Projects].[OrderProcessingTypes];
DELETE FROM [Projects].[DeliveryTypes];
DELETE FROM [Accounting].[VatTypes];
DELETE FROM [Crm].[CustomerStatuses];
DELETE FROM [Crm].[Manufacturer];
DELETE FROM [Crm].[Supplier];
DELETE FROM [Crm].[City];
DELETE FROM [Crm].[Country];
GO

DBCC CHECKIDENT ('[Files].[AzureFiles]', RESEED, 0);
DBCC CHECKIDENT ('[Files].[AzureFileFolders]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[OrderLines]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[Orders]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[Project]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[ProductStockLocations]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[Product]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[WebshopStructures]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[StockLocations]', RESEED, 0);
DBCC CHECKIDENT ('[Customers].[Customers]', RESEED, 0);
DBCC CHECKIDENT ('[Customers].[CustomerTypes]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[OrderProcessingTypes]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[DeliveryTypes]', RESEED, 0);
DBCC CHECKIDENT ('[Accounting].[VatTypes]', RESEED, 0);
DBCC CHECKIDENT ('[Crm].[CustomerStatuses]', RESEED, 0);
DBCC CHECKIDENT ('[Crm].[Manufacturer]', RESEED, 0);
DBCC CHECKIDENT ('[Crm].[Supplier]', RESEED, 0);
DBCC CHECKIDENT ('[Crm].[City]', RESEED, 0);
DBCC CHECKIDENT ('[Crm].[Country]', RESEED, 0);
GO

BEGIN TRANSACTION;

-- Lookups
SET IDENTITY_INSERT [Crm].[Country] ON;
INSERT INTO [Crm].[Country] ([Id], [IsEu], [IsoCode], [Name]) VALUES (1, 1, N'BE', N'Belgium');
SET IDENTITY_INSERT [Crm].[Country] OFF;

SET IDENTITY_INSERT [Crm].[City] ON;
INSERT INTO [Crm].[City] ([CityId], [CityName], [PostalCode], [CountryName], [CountryIsoCode], [CountryId])
VALUES (1, N'Brussels', N'1000', N'Belgium', N'BE', 1);
SET IDENTITY_INSERT [Crm].[City] OFF;

SET IDENTITY_INSERT [Projects].[DeliveryTypes] ON;
INSERT INTO [Projects].[DeliveryTypes] ([Id], [Name], [IncludeInstallationCost], [NameFr], [IsDefault])
VALUES (1, N'Standard delivery', 0, N'Livraison standard', 1);
SET IDENTITY_INSERT [Projects].[DeliveryTypes] OFF;

SET IDENTITY_INSERT [Customers].[CustomerTypes] ON;
INSERT INTO [Customers].[CustomerTypes]
    ([KlantTypeId], [CustomerTypeName], [RequiresVatNumber], [PaymentTermId], [VatSystemId], [BaseDiscount], [DeliveryTypeId], [CustomerTypeNameFr], [SortOrder], [IsDefault])
VALUES (1, N'B2B Dealer', 1, 1, 1, 0, 1, N'Revendeur B2B', 1, 1);
SET IDENTITY_INSERT [Customers].[CustomerTypes] OFF;

SET IDENTITY_INSERT [Crm].[CustomerStatuses] ON;
INSERT INTO [Crm].[CustomerStatuses] ([Id], [Name], [Color], [IsDefault]) VALUES (1, N'Active', 2263842, 1);
SET IDENTITY_INSERT [Crm].[CustomerStatuses] OFF;

SET IDENTITY_INSERT [Accounting].[VatTypes] ON;
INSERT INTO [Accounting].[VatTypes]
    ([Id], [Name], [InvoiceText], [Percentage], [InvoiceTextEn], [InvoiceTextFr], [ExplanationNl], [ExplanationFr], [ExplanationEn], [IsDefault])
VALUES (1, N'21% VAT', N'BTW 21%', 21.00, N'VAT 21%', N'TVA 21%', N'Standard', N'Standard', N'Standard', 1);
SET IDENTITY_INSERT [Accounting].[VatTypes] OFF;

SET IDENTITY_INSERT [Projects].[OrderProcessingTypes] ON;
INSERT INTO [Projects].[OrderProcessingTypes] ([Id], [Name], [CalculatePrice], [OrderStatusId]) VALUES (1, N'Webshop', 1, 1);
SET IDENTITY_INSERT [Projects].[OrderProcessingTypes] OFF;

SET IDENTITY_INSERT [Crm].[Manufacturer] ON;
INSERT INTO [Crm].[Manufacturer] ([ManufacturerId], [Name], [CityId]) VALUES (1, N'Demo Manufacturer', 1);
SET IDENTITY_INSERT [Crm].[Manufacturer] OFF;

SET IDENTITY_INSERT [Crm].[Supplier] ON;
INSERT INTO [Crm].[Supplier]
    ([SupplierId], [Name], [CityId], [LanguageId], [GeneralLedgerRevenueAccount], [IsInactive])
VALUES (1, N'Demo Supplier', 1, 1, 700000, 0);
SET IDENTITY_INSERT [Crm].[Supplier] OFF;

-- Customers (4)
SET IDENTITY_INSERT [Customers].[Customers] ON;
INSERT INTO [Customers].[Customers] (
    [CustomerId], [CustomerVatNumber], [CustomerBox], [CustomerHouseNumber], [CustomerName], [CustomerStreet],
    [CustomerTypeId], [CustomerPhone], [CustomerFax], [CustomerEmail], [CustomerVatSystemId], [CustomerStatusId], [CustomerCityId],
    [AccountManagerUserId], [Locked], [LockedBy], [RevenueLast12Months], [SendPriceListByEmail], [PromotionalMailing],
    [DeliveryTypeId], [InvoicesByMail], [DigitalInvoicing], [CustomerLanguageId], [CustomerGroup],
    [QuoteContactId], [OrderConfirmationContactId], [PlanningContactId], [DeliveryCompleteContactId], [BillingContactId],
    [RequestedCommission], [BetaaltermijnId], [WebshopLogin], [DeliveryCustomerTypeId]
) VALUES
(1, N'BE0123456789', N'', N'1', N'Northwind Trading', N'Demo Street 1', 1, N'+32 2 111 0001', N'', N'northwind@demo.webshop', 1, 1, 1, 1, 0, N'', 12000, 0, 0, 1, 0, 1, 1, N'Demo', 0, 0, 0, 0, 0, 0, 1, N'northwind@demo.webshop', 1),
(2, N'BE0123456790', N'', N'2', N'Contoso Industries', N'Demo Street 2', 1, N'+32 2 111 0002', N'', N'contoso@demo.webshop', 1, 1, 1, 1, 0, N'', 18500, 0, 0, 1, 0, 1, 1, N'Demo', 0, 0, 0, 0, 0, 0, 1, N'contoso@demo.webshop', 1),
(3, N'BE0123456791', N'', N'3', N'Fabrikam Logistics', N'Demo Street 3', 1, N'+32 2 111 0003', N'', N'fabrikam@demo.webshop', 1, 1, 1, 1, 0, N'', 9200, 0, 0, 1, 0, 1, 1, N'Demo', 0, 0, 0, 0, 0, 0, 1, N'fabrikam@demo.webshop', 1),
(4, N'BE0123456792', N'', N'4', N'Tailspin Toys', N'Demo Street 4', 1, N'+32 2 111 0004', N'', N'customer@webshop.com', 1, 1, 1, 1, 0, N'', 6400, 0, 0, 1, 0, 1, 1, N'Demo', 0, 0, 0, 0, 0, 0, 1, N'customer@webshop.com', 1);
SET IDENTITY_INSERT [Customers].[Customers] OFF;

SET IDENTITY_INSERT [Products].[StockLocations] ON;
INSERT INTO [Products].[StockLocations] ([Id], [Name], [IsWarehouse]) VALUES (1, N'Main warehouse', 1);
SET IDENTITY_INSERT [Products].[StockLocations] OFF;

-- Products (12): 10 on webshop
SET IDENTITY_INSERT [Products].[Product] ON;
INSERT INTO [Products].[Product] (
    [ProductId], [NameNl], [DescriptionNl], [OrderPartNumber], [StockNumber], [SupplierId], [ManufacturerId],
    [IsInactive], [UnitsPerSale], [UnitsPerPurchase], [ShowOnPriceList], [ProdToonOmschrijvingPrijslijst],
    [MinimumQuantity], [CustomWorkPercentage], [NameFr], [DescriptionFr], [ShortNotesFr],
    [NameEn], [DescriptionEn], [ShortNotesEn], [SalesStockTriggerQuantity],
    [HasNoPrice], [ShowOnQuote], [ShowOnOrderConfirmation], [ShowOnInvoice], [ShowOnPackingSlip],
    [ShowOnDeliveryNote], [ShowOnProductionOrder], [ShowOnPaintShopOrder], [ShowOnInstallationOrder],
    [Weight], [ExternalInstallerCost], [ReportRecupel], [ReportBebat], [ShowOnWebshop], [WebshopDescriptionNl], [EanCode]
) VALUES
(1,  N'Hard drive 1', N'Reliable storage.', N'HDD-001', N'HDD-001', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Hard drive 1', N'Reliable storage.', N'', N'Hard drive 1', N'Reliable storage for everyday workloads.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'Reliable storage for everyday workloads.', N'EANHDD001'),
(2,  N'Hard drive 2', N'Expanded capacity.', N'HDD-002', N'HDD-002', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Hard drive 2', N'Expanded capacity.', N'', N'Hard drive 2', N'Expanded capacity for growing teams.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'Expanded capacity for growing teams.', N'EANHDD002'),
(3,  N'Hard drive 3', N'Fast SSD.', N'HDD-003', N'HDD-003', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Hard drive 3', N'Fast SSD.', N'', N'Hard drive 3', N'Fast SSD performance for critical apps.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'Fast SSD performance for critical apps.', N'EANHDD003'),
(4,  N'Hard drive 4', N'High-speed SSD.', N'HDD-004', N'HDD-004', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Hard drive 4', N'High-speed SSD.', N'', N'Hard drive 4', N'High-speed SSD with balanced endurance.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'High-speed SSD with balanced endurance.', N'EANHDD004'),
(5,  N'Hard drive 5', N'Enterprise HDD.', N'HDD-005', N'HDD-005', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Hard drive 5', N'Enterprise HDD.', N'', N'Hard drive 5', N'Enterprise HDD for heavy read/write cycles.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'Enterprise HDD for heavy read/write cycles.', N'EANHDD005'),
(6,  N'Hard drive 6', N'Premium HDD.', N'HDD-006', N'HDD-006', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Hard drive 6', N'Premium HDD.', N'', N'Hard drive 6', N'Premium HDD with maximum capacity.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'Premium HDD with maximum capacity.', N'EANHDD006'),
(7,  N'Rack mounting kit', N'19-inch rack kit.', N'ACC-001', N'ACC-001', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Rack mounting kit', N'19-inch rack kit.', N'', N'Rack mounting kit', N'Standard 19-inch rack kit.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'Standard 19-inch rack kit.', N'EANACC001'),
(8,  N'SATA cable pack', N'10 SATA III cables.', N'ACC-002', N'ACC-002', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'SATA cable pack', N'10 SATA III cables.', N'', N'SATA cable pack', N'Pack of 10 SATA III cables.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'Pack of 10 SATA III cables.', N'EANACC002'),
(9,  N'Installation service', N'On-site installation.', N'SRV-001', N'SRV-001', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Installation service', N'On-site installation.', N'', N'Installation service', N'On-site installation (per hour).', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'On-site installation (per hour).', N'EANSRV001'),
(10, N'Extended warranty', N'3-year warranty.', N'SRV-002', N'SRV-002', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Extended warranty', N'3-year warranty.', N'', N'Extended warranty', N'3-year extended warranty.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 1, N'3-year extended warranty.', N'EANSRV002'),
(11, N'Internal spare part', N'Not on webshop.', N'INT-001', N'INT-001', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Internal spare part', N'Not on webshop.', N'', N'Internal spare part', N'Not shown on webshop.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 0, N'Not shown on webshop.', N'EANINT001'),
(12, N'Legacy adapter', N'Legacy stock.', N'INT-002', N'INT-002', 1, 1, 0, 1, 1, 1, 0, 1, 0, N'Legacy adapter', N'Legacy stock.', N'', N'Legacy adapter', N'Legacy stock item.', N'', 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0.5, 0, 0, 0, 0, N'Legacy stock item.', N'EANINT002');
SET IDENTITY_INSERT [Products].[Product] OFF;

-- Azure file folder (product images — fictitious blob metadata)
SET IDENTITY_INSERT [Files].[AzureFileFolders] ON;
INSERT INTO [Files].[AzureFileFolders]
    ([Id], [Name], [IsForCrm], [IsForOrder], [IsForProject], [IsForProduct], [IsForUser], [IsForGeneralUse], [SortOrder])
VALUES (1, N'Products', 0, 0, 0, 1, 0, 0, 1);
SET IDENTITY_INSERT [Files].[AzureFileFolders] OFF;

-- Primary webshop images (BlobRef → static mock assets; uploads use /media/products/{id}/)
INSERT INTO [Files].[AzureFiles]
    ([Name], [Extension], [AzureFileFolderId], [Created], [CreatedByUserId], [Description], [BlobRef], [ThumbRef],
     [ProductId], [IsPrimaryImage], [PublishToWeb], [SendToCustomer], [SendOnSupplierOrder])
VALUES
(N'product1.png', N'.png', 1, GETUTCDATE(), 1, N'Hard drive 1 catalog image', N'/images/product1.png', N'/images/product1.png', 1, 1, 1, 0, 0),
(N'product2.png', N'.png', 1, GETUTCDATE(), 1, N'Hard drive 2 catalog image', N'/images/product2.png', N'/images/product2.png', 2, 1, 1, 0, 0),
(N'product3.png', N'.png', 1, GETUTCDATE(), 1, N'Hard drive 3 catalog image', N'/images/product3.png', N'/images/product3.png', 3, 1, 1, 0, 0),
(N'product4.png', N'.png', 1, GETUTCDATE(), 1, N'Hard drive 4 catalog image', N'/images/product4.png', N'/images/product4.png', 4, 1, 1, 0, 0),
(N'product5.png', N'.png', 1, GETUTCDATE(), 1, N'Hard drive 5 catalog image', N'/images/product5.png', N'/images/product5.png', 5, 1, 1, 0, 0),
(N'product6.png', N'.png', 1, GETUTCDATE(), 1, N'Hard drive 6 catalog image', N'/images/product6.png', N'/images/product6.png', 6, 1, 1, 0, 0);

-- Stock (7 low-stock alerts: Quantity <= MinQuantity)
INSERT INTO [Products].[ProductStockLocations] ([StockLocationId], [ProductId], [Quantity], [MaxQuantity], [IsDefault], [MinQuantity], [ReservedQuantity]) VALUES
(1, 1, 24, 500, 1, 5, 0),
(1, 2, 18, 500, 1, 3, 0),
(1, 3, 32, 500, 1, 8, 0),
(1, 4, 15, 500, 1, 2, 0),
(1, 5,  9, 500, 1, 1, 0),
(1, 6, 41, 500, 1, 10, 0),
(1, 7,  8, 500, 1, 15, 0),
(1, 8,  6, 500, 1, 25, 0),
(1, 9,  0, 500, 1, 0, 0),
(1, 10, 50, 500, 1, 5, 0),
(1, 11,  4, 500, 1, 5, 0),
(1, 12,  2, 500, 1, 2, 0);

-- Webshop structure (12 nodes)
SET IDENTITY_INSERT [Products].[WebshopStructures] ON;
INSERT INTO [Products].[WebshopStructures] ([Id], [NameNl], [ParentTaskId], [SortOrder]) VALUES
(1,  N'Storage',  NULL, 1),
(2,  N'SSD',      1,    2),
(3,  N'HDD',      1,    3),
(4,  N'Accessories', NULL, 4),
(5,  N'Cables',   4,    5),
(6,  N'Services', NULL, 6),
(7,  N'Rack',     4,    7),
(8,  N'Warranty', 6,    8),
(9,  N'Enterprise', 3,  9),
(10, N'Consumer', 3,   10),
(11, N'Spare parts', NULL, 11),
(12, N'Archive',  NULL, 12);
SET IDENTITY_INSERT [Products].[WebshopStructures] OFF;

-- Projects (1 per customer)
SET IDENTITY_INSERT [Projects].[Project] ON;
INSERT INTO [Projects].[Project] ([ProjectId], [ProjectNumber], [ProjectName], [ProjectManagerUserId], [CustomerId], [ProjectTypeId], [IsStandardProject]) VALUES
(1, 1001, N'Webshop — Northwind Trading', 1, 1, 1, 1),
(2, 1002, N'Webshop — Contoso Industries', 1, 2, 1, 1),
(3, 1003, N'Webshop — Fabrikam Logistics', 1, 3, 1, 1),
(4, 1004, N'Webshop — Tailspin Toys', 1, 4, 1, 1);
SET IDENTITY_INSERT [Projects].[Project] OFF;

-- Orders + lines (24 this month, 8 pending; YTD revenue ~29.384)
DECLARE @i int = 1;
DECLARE @MonthStart datetime2 = DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1);
DECLARE @YearStart datetime2 = DATEFROMPARTS(YEAR(GETUTCDATE()), 1, 1);
DECLARE @OrderId int;
DECLARE @ProjectId int;
DECLARE @ProductId int;
DECLARE @IsAccepted bit;
DECLARE @CreatedAt datetime2;
DECLARE @Qty decimal(18,4);
DECLARE @UnitPrice decimal(18,4);
DECLARE @LineTotal decimal(18,4);
DECLARE @VatAmount decimal(18,4);
DECLARE @CostUnit decimal(18,4);
DECLARE @DisplayName nvarchar(500);

WHILE @i <= 24
BEGIN
    SET @IsAccepted = CASE WHEN @i <= 8 THEN 0 ELSE 1 END;
    SET @CreatedAt = DATEADD(day, (@i - 1) % 28, @MonthStart);
    SET @ProjectId = ((@i - 1) % 4) + 1;
    SET @ProductId = ((@i - 1) % 10) + 1;
    SET @Qty = 1 + ((@i - 1) % 4);
    SET @UnitPrice = 40 + ((@i - 1) * 7.5);
    SET @LineTotal = ROUND(@UnitPrice * @Qty, 2);
    SET @VatAmount = ROUND(@LineTotal * 0.21, 2);
    SET @CostUnit = ROUND(@UnitPrice * 0.32, 2);
    SELECT @DisplayName = [NameEn] FROM [Products].[Product] WHERE [ProductId] = @ProductId;

    INSERT INTO [Projects].[Orders] (
        [IsAccepted], [CreatedAt], [CreatedByUserId], [ProjectId], [GeneralDiscount], [DeliveryTypeId], [PriceListTypeId],
        [CommissionAmount], [VatTypeId], [OrderProcessingTypeId], [CustomerTypeId], [AllowPartialDelivery],
        [BetaaltermijnId], [QuoteValidDays], [IsUrgent], [BaseCompanyVatNumberId], [OrderNumber]
    ) VALUES (
        @IsAccepted, @CreatedAt, 1, @ProjectId, 0, 1, 1, 0, 1, 1, 1, 1, 1, 30, 0, 1, 2026000 + @i
    );
    SET @OrderId = SCOPE_IDENTITY();

    INSERT INTO [Projects].[OrderLines] (
        [OrderId], [ProductId], [Quantity], [UnitPrice], [AssemblyPrice], [InstallationPrice],
        [TotalExclVat], [TotalInclVat], [IsOption], [Discount], [Btw], [UpliftType], [DocumentDisplayName],
        [Lengte], [Hoogte], [Breedte], [Assemblage], [Montage], [QuoteNotesHeader], [MontageStukprijs], [AssemblageStukprijs],
        [ProductionGroupId], [PurchaseOrderNotes], [ProductType], [BtwBedrag], [SortOrder], [DocumentDisplayNameFr],
        [AantalSubProduct], [AantalTeBestellen], [ReportingGroupId], [BrutoAankoopprijs], [NettoAankoopPrijs], [WinstPercentage],
        [OrigineleKorting], [UpliftTypeOrigineel], [BasePrice], [BebatAantal], [BebatStukPrijs], [BebatTotaal],
        [RecupelAantal], [RecupelStukPrijs], [RecupelTotaal], [IsGarantie], [TotaalExclVoorCommissie], [PopUpRow], [Selected],
        [UnitParameter], [BasisPrijsTellen], [ShowOnQuote], [ShowOnOrderConfirmation], [ShowOnInvoice], [ShowOnPackingSlip],
        [ShowOnDeliveryNote], [ShowOnProductionOrder], [ToonOpLakBon], [ShowOnInstallationOrder],
        [ToonOmschrijvingOpFactuur], [ToonOmschrijvingOpPakbon], [ToonOmschrijvingOpLeverbon], [ToonOmschrijvingOpProductiebon],
        [ToonOmschrijvingOpLakBon], [ToonOmschrijvingOpOfferte], [ToonOpVrachtbrief], [ToonOmschrijvingOpVrachtbrief],
        [ToonOmschrijvingOpOrderbevestiging], [StartupCost], [OpstartKostTotaal], [BasisPrijsTotaal], [VatTypeId], [PieceUnitPrice],
        [ReportRecupel], [ReportBebat], [AdvancePaymentVisibility], [Goederen], [Diensten]
    ) VALUES (
        @OrderId, @ProductId, @Qty, @UnitPrice, 0, 0, @LineTotal, @LineTotal + @VatAmount, 0, 0, 21, N'Standard', @DisplayName,
        0, 0, 0, 0, 0, N'', 0, 0, N'WEB', N'', 0, @VatAmount, 1, @DisplayName, 0, 0, 1, @CostUnit, @CostUnit, 30,
        0, N'Standard', @UnitPrice, 0, 0, 0, 0, 0, 0, 0, @LineTotal, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
        1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, @LineTotal, 1, @UnitPrice, 0, 0, N'Default', @LineTotal, 0
    );

    SET @i = @i + 1;
END;

-- Extra accepted orders earlier in the year (YTD financials)
SET @i = 1;
WHILE @i <= 10
BEGIN
    SET @CreatedAt = DATEADD(day, 14 + (@i * 11), @YearStart);
    IF @CreatedAt < @MonthStart
    BEGIN

    SET @ProjectId = ((@i - 1) % 4) + 1;
    SET @ProductId = ((@i - 1) % 6) + 1;
    SET @Qty = 2;
    SET @UnitPrice = 180 + (@i * 25);
    SET @LineTotal = ROUND(@UnitPrice * @Qty, 2);
    SET @VatAmount = ROUND(@LineTotal * 0.21, 2);
    SET @CostUnit = ROUND(@UnitPrice * 0.28, 2);
    SELECT @DisplayName = [NameEn] FROM [Products].[Product] WHERE [ProductId] = @ProductId;

    INSERT INTO [Projects].[Orders] (
        [IsAccepted], [CreatedAt], [CreatedByUserId], [ProjectId], [GeneralDiscount], [DeliveryTypeId], [PriceListTypeId],
        [CommissionAmount], [VatTypeId], [OrderProcessingTypeId], [CustomerTypeId], [AllowPartialDelivery],
        [BetaaltermijnId], [QuoteValidDays], [IsUrgent], [BaseCompanyVatNumberId], [OrderNumber]
    ) VALUES (
        1, @CreatedAt, 1, @ProjectId, 0, 1, 1, 0, 1, 1, 1, 1, 1, 30, 0, 1, 2025100 + @i
    );
    SET @OrderId = SCOPE_IDENTITY();

    INSERT INTO [Projects].[OrderLines] (
        [OrderId], [ProductId], [Quantity], [UnitPrice], [AssemblyPrice], [InstallationPrice],
        [TotalExclVat], [TotalInclVat], [IsOption], [Discount], [Btw], [UpliftType], [DocumentDisplayName],
        [Lengte], [Hoogte], [Breedte], [Assemblage], [Montage], [QuoteNotesHeader], [MontageStukprijs], [AssemblageStukprijs],
        [ProductionGroupId], [PurchaseOrderNotes], [ProductType], [BtwBedrag], [SortOrder], [DocumentDisplayNameFr],
        [AantalSubProduct], [AantalTeBestellen], [ReportingGroupId], [BrutoAankoopprijs], [NettoAankoopPrijs], [WinstPercentage],
        [OrigineleKorting], [UpliftTypeOrigineel], [BasePrice], [BebatAantal], [BebatStukPrijs], [BebatTotaal],
        [RecupelAantal], [RecupelStukPrijs], [RecupelTotaal], [IsGarantie], [TotaalExclVoorCommissie], [PopUpRow], [Selected],
        [UnitParameter], [BasisPrijsTellen], [ShowOnQuote], [ShowOnOrderConfirmation], [ShowOnInvoice], [ShowOnPackingSlip],
        [ShowOnDeliveryNote], [ShowOnProductionOrder], [ToonOpLakBon], [ShowOnInstallationOrder],
        [ToonOmschrijvingOpFactuur], [ToonOmschrijvingOpPakbon], [ToonOmschrijvingOpLeverbon], [ToonOmschrijvingOpProductiebon],
        [ToonOmschrijvingOpLakBon], [ToonOmschrijvingOpOfferte], [ToonOpVrachtbrief], [ToonOmschrijvingOpVrachtbrief],
        [ToonOmschrijvingOpOrderbevestiging], [StartupCost], [OpstartKostTotaal], [BasisPrijsTotaal], [VatTypeId], [PieceUnitPrice],
        [ReportRecupel], [ReportBebat], [AdvancePaymentVisibility], [Goederen], [Diensten]
    ) VALUES (
        @OrderId, @ProductId, @Qty, @UnitPrice, 0, 0, @LineTotal, @LineTotal + @VatAmount, 0, 0, 21, N'Standard', @DisplayName,
        0, 0, 0, 0, 0, N'', 0, 0, N'WEB', N'', 0, @VatAmount, 1, @DisplayName, 0, 0, 1, @CostUnit, @CostUnit, 30,
        0, N'Standard', @UnitPrice, 0, 0, 0, 0, 0, 0, 0, @LineTotal, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
        1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, @LineTotal, 1, @UnitPrice, 0, 0, N'Default', @LineTotal, 0
    );

    END;

    SET @i = @i + 1;
END;

-- Top-up line to reach ~29.384 YTD revenue on accepted orders
DECLARE @YtdRevenue decimal(18,2);
SELECT @YtdRevenue = SUM(ol.[TotalExclVat])
FROM [Projects].[OrderLines] ol
INNER JOIN [Projects].[Orders] o ON o.[Id] = ol.[OrderId]
WHERE o.[IsAccepted] = 1 AND o.[CreatedAt] >= @YearStart;

IF @YtdRevenue < 29384
BEGIN
    DECLARE @TopUp decimal(18,2) = 29384 - ISNULL(@YtdRevenue, 0);
    SELECT TOP 1 @OrderId = o.[Id] FROM [Projects].[Orders] o WHERE o.[IsAccepted] = 1 AND o.[CreatedAt] >= @YearStart ORDER BY o.[Id];
    SET @VatAmount = ROUND(@TopUp * 0.21, 2);

    INSERT INTO [Projects].[OrderLines] (
        [OrderId], [ProductId], [Quantity], [UnitPrice], [AssemblyPrice], [InstallationPrice],
        [TotalExclVat], [TotalInclVat], [IsOption], [Discount], [Btw], [UpliftType], [DocumentDisplayName],
        [Lengte], [Hoogte], [Breedte], [Assemblage], [Montage], [QuoteNotesHeader], [MontageStukprijs], [AssemblageStukprijs],
        [ProductionGroupId], [PurchaseOrderNotes], [ProductType], [BtwBedrag], [SortOrder], [DocumentDisplayNameFr],
        [AantalSubProduct], [AantalTeBestellen], [ReportingGroupId], [BrutoAankoopprijs], [NettoAankoopPrijs], [WinstPercentage],
        [OrigineleKorting], [UpliftTypeOrigineel], [BasePrice], [BebatAantal], [BebatStukPrijs], [BebatTotaal],
        [RecupelAantal], [RecupelStukPrijs], [RecupelTotaal], [IsGarantie], [TotaalExclVoorCommissie], [PopUpRow], [Selected],
        [UnitParameter], [BasisPrijsTellen], [ShowOnQuote], [ShowOnOrderConfirmation], [ShowOnInvoice], [ShowOnPackingSlip],
        [ShowOnDeliveryNote], [ShowOnProductionOrder], [ToonOpLakBon], [ShowOnInstallationOrder],
        [ToonOmschrijvingOpFactuur], [ToonOmschrijvingOpPakbon], [ToonOmschrijvingOpLeverbon], [ToonOmschrijvingOpProductiebon],
        [ToonOmschrijvingOpLakBon], [ToonOmschrijvingOpOfferte], [ToonOpVrachtbrief], [ToonOmschrijvingOpVrachtbrief],
        [ToonOmschrijvingOpOrderbevestiging], [StartupCost], [OpstartKostTotaal], [BasisPrijsTotaal], [VatTypeId], [PieceUnitPrice],
        [ReportRecupel], [ReportBebat], [AdvancePaymentVisibility], [Goederen], [Diensten]
    ) VALUES (
        @OrderId, 1, 1, @TopUp, 0, 0, @TopUp, @TopUp + @VatAmount, 0, 0, 21, N'Standard', N'YTD revenue adjustment',
        0, 0, 0, 0, 0, N'', 0, 0, N'WEB', N'', 0, @VatAmount, 99, N'YTD revenue adjustment', 0, 0, 1, @TopUp * 0.3, @TopUp * 0.3, 30,
        0, N'Standard', @TopUp, 0, 0, 0, 0, 0, 0, 0, @TopUp, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0,
        1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, @TopUp, 1, @TopUp, 0, 0, N'Default', @TopUp, 0
    );
END;

COMMIT TRANSACTION;
GO

-- Summary
SELECT N'Products on webshop' AS [Metric], COUNT(*) AS [Value] FROM [Products].[Product] WHERE [ShowOnWebshop] = 1
UNION ALL SELECT N'Webshop structure nodes', COUNT(*) FROM [Products].[WebshopStructures]
UNION ALL SELECT N'Customers', COUNT(*) FROM [Customers].[Customers]
UNION ALL SELECT N'Orders this month', COUNT(*) FROM [Projects].[Orders] WHERE [CreatedAt] >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
UNION ALL SELECT N'Pending orders', COUNT(*) FROM [Projects].[Orders] WHERE [IsAccepted] = 0
UNION ALL SELECT N'Low stock alerts', COUNT(*) FROM [Products].[ProductStockLocations] WHERE [Quantity] <= [MinQuantity]
UNION ALL SELECT N'Revenue YTD', ISNULL(SUM(ol.[TotalExclVat]), 0)
    FROM [Projects].[OrderLines] ol INNER JOIN [Projects].[Orders] o ON o.[Id] = ol.[OrderId]
    WHERE o.[IsAccepted] = 1 AND o.[CreatedAt] >= DATEFROMPARTS(YEAR(GETUTCDATE()), 1, 1);
GO

PRINT N'Demo seed completed successfully.';
GO
