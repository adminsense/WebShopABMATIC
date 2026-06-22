/* WebShopABMATIC — demo seed data (idempotent)
   Database: WebShopABMATIC on SQL Server MULLER
   Run schema first: sqlcmd -S MULLER -E -d WebShopABMATIC -i scripts\apply-pending-schema.sql
   Then seed:        sqlcmd -S MULLER -E -d WebShopABMATIC -i scripts\seeds.sql
   Or all-in-one:    .\scripts\apply-local-database.ps1
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

USE [WebShopABMATIC];
GO

-- Remove previous demo rows (child → parent)
DELETE FROM [dbo].[StockLowAlerts];
DELETE FROM [dbo].[AuditLogs];
DELETE FROM [Emails].[EmailMessages];
DELETE FROM [Emails].[EmailQueues];
DELETE FROM [Accounting].[AccountingDocumentLines];
DELETE FROM [Accounting].[AccountingDocuments];
DELETE FROM [Accounting].[DocumentTypes];
DELETE FROM [Files].[AzureFiles];
DELETE FROM [Files].[AzureFileFolders];
DELETE FROM [Projects].[OrderAdvancePayments];
DELETE FROM [Projects].[OrderLines];
DELETE FROM [Projects].[Orders];
DELETE FROM [Projects].[Project];
DELETE FROM [Products].[StockOrderDeliveries];
DELETE FROM [Products].[StockOrderLines];
DELETE FROM [Products].[StockOrder];
DELETE FROM [Products].[StockMovements];
DELETE FROM [Products].[ProductOptionValue];
DELETE FROM [Products].[ProductOptions];
DELETE FROM [Products].[ProductQuantityTiers];
DELETE FROM [Products].[ProductPrices];
DELETE FROM [Products].[ProductStockLocations];
DELETE FROM [Products].[Product];
DELETE FROM [Products].[WebshopProductStructures];
DELETE FROM [Products].[WebshopStructures];
DELETE FROM [Products].[PriceListCategories];
DELETE FROM [Products].[StockLocations];
DELETE FROM [Crm].[CustomerProductDiscounts];
DELETE FROM [Crm].[CustomerDeliveryAddresses];
DELETE FROM [Customers].[CustomerContacts];
DELETE FROM [Customers].[Contact];
DELETE FROM [Customers].[Customers];
DELETE FROM [Customers].[CustomerTypes];
DELETE FROM [Projects].[OrderProcessingTypes];
DELETE FROM [Projects].[OrderStatuses];
DELETE FROM [Projects].[DeliveryTypes];
DELETE FROM [Settings].[StaffUsers];
DELETE FROM [Settings].[UserGroups];
DELETE FROM [Settings].[BaseCompanyAccess];
DELETE FROM [Settings].[BaseCompanyVatNumber];
DELETE FROM [Settings].[BaseCompany];
DELETE FROM [Settings].[PaymentMethods];
DELETE FROM [Crm].[PaymentTerms];
DELETE FROM [Accounting].[VatTypes];
DELETE FROM [Crm].[CustomerStatuses];
DELETE FROM [Crm].[Manufacturer];
DELETE FROM [Crm].[Supplier];
DELETE FROM [Crm].[City];
DELETE FROM [Crm].[Country];
GO

DBCC CHECKIDENT ('[Accounting].[AccountingDocumentLines]', RESEED, 0);
DBCC CHECKIDENT ('[Accounting].[AccountingDocuments]', RESEED, 0);
DBCC CHECKIDENT ('[Accounting].[DocumentTypes]', RESEED, 0);
DBCC CHECKIDENT ('[Files].[AzureFiles]', RESEED, 0);
DBCC CHECKIDENT ('[Files].[AzureFileFolders]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[ProductOptionValue]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[ProductOptions]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[ProductQuantityTiers]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[WebshopProductStructures]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[PriceListCategories]', RESEED, 0);
DBCC CHECKIDENT ('[Crm].[CustomerProductDiscounts]', RESEED, 0);
DBCC CHECKIDENT ('[Customers].[CustomerContacts]', RESEED, 0);
DBCC CHECKIDENT ('[Customers].[Contact]', RESEED, 0);
DBCC CHECKIDENT ('[Settings].[StaffUsers]', RESEED, 0);
DBCC CHECKIDENT ('[Settings].[UserGroups]', RESEED, 0);
DBCC CHECKIDENT ('[Settings].[BaseCompanyVatNumber]', RESEED, 0);
DBCC CHECKIDENT ('[Settings].[BaseCompany]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[OrderLines]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[Orders]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[OrderAdvancePayments]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[StockLowAlerts]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[AuditLogs]', RESEED, 0);
DBCC CHECKIDENT ('[Emails].[EmailQueues]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[Project]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[StockOrderLines]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[StockOrder]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[StockMovements]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[ProductStockLocations]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[Product]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[WebshopStructures]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[StockLocations]', RESEED, 0);
DBCC CHECKIDENT ('[Customers].[Customers]', RESEED, 0);
DBCC CHECKIDENT ('[Customers].[CustomerTypes]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[OrderProcessingTypes]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[OrderStatuses]', RESEED, 0);
DBCC CHECKIDENT ('[Projects].[DeliveryTypes]', RESEED, 0);
DBCC CHECKIDENT ('[Settings].[PaymentMethods]', RESEED, 0);
DBCC CHECKIDENT ('[Crm].[PaymentTerms]', RESEED, 0);
DBCC CHECKIDENT ('[Products].[ProductPrices]', RESEED, 0);
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

SET IDENTITY_INSERT [Crm].[PaymentTerms] ON;
INSERT INTO [Crm].[PaymentTerms] ([Id], [Name], [AantalDagen], [EndOfMonth], [IsDefault])
VALUES (1, N'30 days net', 30, 0, 1);
SET IDENTITY_INSERT [Crm].[PaymentTerms] OFF;

SET IDENTITY_INSERT [Projects].[OrderStatuses] ON;
INSERT INTO [Projects].[OrderStatuses]
    ([Id], [Name], [SortOrder], [IncludeInSalesReporting], [NameFr], [ReportInProgress], [ScreenMode], [OrderStatusGroupId], [ReserveStock], [AffectsStock])
VALUES
(1, N'Pending payment', 1, 1, N'En attente de paiement', 1, N'Order', NULL, 0, 0),
(2, N'Paid', 2, 1, N'Payé', 1, N'Order', NULL, 1, 0),
(3, N'Accepted', 3, 1, N'Accepté', 1, N'Order', NULL, 0, 1);
SET IDENTITY_INSERT [Projects].[OrderStatuses] OFF;

SET IDENTITY_INSERT [Settings].[PaymentMethods] ON;
INSERT INTO [Settings].[PaymentMethods] ([Id], [NameNl], [NameFr], [NameEn], [IsPrePay], [IsPostPay]) VALUES
(1, N'iDEAL / online', N'iDEAL / en ligne', N'iDEAL / card (Mollie)', 1, 0),
(2, N'Factuur 30 dagen', N'Facture 30 jours', N'Invoice 30 days', 0, 1);
SET IDENTITY_INSERT [Settings].[PaymentMethods] OFF;

SET IDENTITY_INSERT [Settings].[UserGroups] ON;
INSERT INTO [Settings].[UserGroups] ([Id], [Name], [IsInstallationTeam], [IsServiceTeam], [IsTransportTeam], [OrderStatusGroupId]) VALUES
(1, N'Sales', 0, 0, 0, NULL),
(2, N'Warehouse', 0, 0, 1, NULL),
(3, N'Installation', 1, 1, 0, NULL);
SET IDENTITY_INSERT [Settings].[UserGroups] OFF;

SET IDENTITY_INSERT [Settings].[BaseCompany] ON;
INSERT INTO [Settings].[BaseCompany]
    ([Id], [Name], [Street], [StreetNr], [StreetBox], [City], [Zip], [Country], [VatNumber], [Tel], [FaxTemplate],
     [IBAN], [BIC], [Slogan], [AccountingDocumentFooter], [Tag], [Bank])
VALUES
(1, N'WebShopABMATIC Demo BV', N'Demo Street', N'100', N'', N'Brussels', N'1000', N'Belgium', N'BE0123456789',
 N'+32 2 100 0000', N'', N'BE68539007547034', N'GEBABEBB', N'Demo webshop company', N'Thank you for your business.', N'DEMO', N'Demo Bank');
SET IDENTITY_INSERT [Settings].[BaseCompany] OFF;

SET IDENTITY_INSERT [Settings].[BaseCompanyVatNumber] ON;
INSERT INTO [Settings].[BaseCompanyVatNumber] ([Id], [BaseCompanyId], [VatNumber], [EoriNumber]) VALUES
(1, 1, N'BE0123456789', N'BE0123456789');
SET IDENTITY_INSERT [Settings].[BaseCompanyVatNumber] OFF;

SET IDENTITY_INSERT [Accounting].[DocumentTypes] ON;
INSERT INTO [Accounting].[DocumentTypes] ([Id], [Name], [NameFr], [ParameterId], [NameEn]) VALUES
(1, N'Factuur', N'Facture', 1, N'Invoice'),
(2, N'Creditnota', N'Note de crédit', 2, N'Credit note');
SET IDENTITY_INSERT [Accounting].[DocumentTypes] OFF;

SET IDENTITY_INSERT [Products].[PriceListCategories] ON;
INSERT INTO [Products].[PriceListCategories] ([Id], [SortOrder], [Name], [HasOptions], [Color], [NameFr]) VALUES
(1, 1, N'Storage', 1, N'#2563eb', N'Stockage'),
(2, 2, N'Accessories', 0, N'#7c6cf0', N'Accessoires'),
(3, 3, N'Services', 0, N'#059669', N'Services');
SET IDENTITY_INSERT [Products].[PriceListCategories] OFF;

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

-- Demo store password (legacy: plaintext in PasswordWebshop when SaltWebshop is NULL)
UPDATE [Customers].[Customers] SET [PasswordWebshop] = N'demo' WHERE [CustomerId] = 4;

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

-- Product prices (12 products — matches former store formula 49.99 + ((id-1) % 6) * 10)
INSERT INTO [Products].[ProductPrices] (
    [FromAddress], [ValidTo], [AssemblyPrice], [InstallationPrice], [ProductId],
    [GrossSalesPrice], [GrossPurchasePrice], [NetPurchasePrice], [BasePrice], [CorrectedGrossPrice],
    [StartupCost], [Pro1], [Pro2], [Pro3], [Aan1], [Aan2], [Par1], [Ond],
    [PurchaseDiscountPercentage], [GrossCorrectionPercentage], [CalculationType], [SupplierUsesDifferentGrossSalesPrice]
)
SELECT
    CAST(GETUTCDATE() AS date), NULL, 0, 0, p.[ProductId],
    CAST(49.99 + ((p.[ProductId] - 1) % 6) * 10 AS decimal(18,2)),
    CAST(ROUND((49.99 + ((p.[ProductId] - 1) % 6) * 10) * 0.32, 2) AS decimal(18,2)),
    CAST(ROUND((49.99 + ((p.[ProductId] - 1) % 6) * 10) * 0.32, 2) AS decimal(18,2)),
    CAST(49.99 + ((p.[ProductId] - 1) % 6) * 10 AS decimal(18,4)),
    CAST(49.99 + ((p.[ProductId] - 1) % 6) * 10 AS decimal(18,2)),
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0
FROM [Products].[Product] p;

-- Azure file folder (product images — fictitious blob metadata)
SET IDENTITY_INSERT [Files].[AzureFileFolders] ON;
INSERT INTO [Files].[AzureFileFolders]
    ([Id], [Name], [IsForCrm], [IsForOrder], [IsForProject], [IsForProduct], [IsForUser], [IsForGeneralUse], [SortOrder])
VALUES (1, N'Products', 0, 0, 0, 1, 0, 0, 1);
SET IDENTITY_INSERT [Files].[AzureFileFolders] OFF;

-- Primary webshop images (BlobRef → static mock assets; uploads use /media/products/{id}/)
-- All ShowOnWebshop products (1–10); image cycles product1–6 for accessories/services
INSERT INTO [Files].[AzureFiles]
    ([Name], [Extension], [AzureFileFolderId], [Created], [CreatedByUserId], [Description], [BlobRef], [ThumbRef],
     [ProductId], [IsPrimaryImage], [PublishToWeb], [SendToCustomer], [SendOnSupplierOrder])
SELECT
    CONCAT(N'product', ((p.[ProductId] - 1) % 6) + 1, N'.png'),
    N'.png',
    1,
    GETUTCDATE(),
    1,
    CONCAT(p.[NameEn], N' catalog image'),
    CONCAT(N'/images/product', ((p.[ProductId] - 1) % 6) + 1, N'.png'),
    CONCAT(N'/images/product', ((p.[ProductId] - 1) % 6) + 1, N'.png'),
    p.[ProductId],
    1,
    1,
    0,
    0
FROM [Products].[Product] p
WHERE p.[ShowOnWebshop] = 1;

-- Stock (7 low-stock alerts: Quantity <= MinQuantity; reserved qty on a few SKUs)
INSERT INTO [Products].[ProductStockLocations] ([StockLocationId], [ProductId], [Quantity], [MaxQuantity], [IsDefault], [MinQuantity], [ReservedQuantity]) VALUES
(1, 1, 24, 500, 1, 5, 3),
(1, 2, 18, 500, 1, 3, 0),
(1, 3, 32, 500, 1, 8, 0),
(1, 4, 15, 500, 1, 2, 2),
(1, 5,  9, 500, 1, 1, 0),
(1, 6, 41, 500, 1, 10, 0),
(1, 7,  8, 500, 1, 15, 1),
(1, 8,  6, 500, 1, 25, 0),
(1, 9,  0, 500, 1, 0, 0),
(1, 10, 50, 500, 1, 5, 4),
(1, 11,  4, 500, 1, 5, 0),
(1, 12,  2, 500, 1, 2, 0);

-- Customer delivery addresses (webshop checkout)
INSERT INTO [Crm].[CustomerDeliveryAddresses] ([CustomerId], [Name], [Straat], [Number], [Bus], [CityId], [Notes]) VALUES
(1, N'Northwind warehouse', N'Demo Street', N'1', N'', 1, N''),
(2, N'Contoso site', N'Demo Street', N'2', N'', 1, N''),
(3, N'Fabrikam depot', N'Demo Street', N'3', N'', 1, N''),
(4, N'Tailspin warehouse', N'Demo Street', N'4', N'', 1, N''),
(4, N'Tailspin site', N'Industrial Park', N'12', N'A', 1, N'');

-- Stock movements (demo journal — mix in/out/reservation)
INSERT INTO [Products].[StockMovements] ([ProductId], [Quantity], [Timestamp], [Notes], [IsReservation], [ProductStockLocatieId])
SELECT 1, 10, DATEADD(day, -6, GETUTCDATE()), N'Initial receipt', 0, psl.[Id] FROM [Products].[ProductStockLocations] psl WHERE psl.[ProductId] = 1 AND psl.[IsDefault] = 1
UNION ALL SELECT 2, -3, DATEADD(day, -5, GETUTCDATE()), N'Shipped to customer', 0, psl.[Id] FROM [Products].[ProductStockLocations] psl WHERE psl.[ProductId] = 2 AND psl.[IsDefault] = 1
UNION ALL SELECT 3, 5, DATEADD(day, -4, GETUTCDATE()), N'Cycle count adjustment', 0, psl.[Id] FROM [Products].[ProductStockLocations] psl WHERE psl.[ProductId] = 3 AND psl.[IsDefault] = 1
UNION ALL SELECT 4, -2, DATEADD(day, -3, GETUTCDATE()), N'Web order reservation', 1, psl.[Id] FROM [Products].[ProductStockLocations] psl WHERE psl.[ProductId] = 4 AND psl.[IsDefault] = 1
UNION ALL SELECT 5, 8, DATEADD(day, -2, GETUTCDATE()), N'PO delivery', 0, psl.[Id] FROM [Products].[ProductStockLocations] psl WHERE psl.[ProductId] = 5 AND psl.[IsDefault] = 1
UNION ALL SELECT 6, -1, DATEADD(day, -1, GETUTCDATE()), N'Damaged unit write-off', 0, psl.[Id] FROM [Products].[ProductStockLocations] psl WHERE psl.[ProductId] = 6 AND psl.[IsDefault] = 1
UNION ALL SELECT 7, 12, DATEADD(hour, -8, GETUTCDATE()), N'Accessory restock', 0, psl.[Id] FROM [Products].[ProductStockLocations] psl WHERE psl.[ProductId] = 7 AND psl.[IsDefault] = 1
UNION ALL SELECT 8, -4, DATEADD(hour, -2, GETUTCDATE()), N'Bulk shipment', 0, psl.[Id] FROM [Products].[ProductStockLocations] psl WHERE psl.[ProductId] = 8 AND psl.[IsDefault] = 1;

-- Open purchase order (demo)
INSERT INTO [Products].[StockOrder] ([SupplierId], [CreatedAt], [OrderDate], [IsCompleted], [UserId], [ExpectedDeliveryDate], [Notes], [TotalAmount])
VALUES (1, GETUTCDATE(), GETUTCDATE(), 0, 1, DATEADD(day, 7, GETUTCDATE()), N'Demo open PO — HDD restock', 1250.00);

DECLARE @StockOrderId int = SCOPE_IDENTITY();

INSERT INTO [Products].[StockOrderLines]
    ([StockOrderId], [ProductId], [QuantityOrdered], [LijnOK], [ProductName], [OrderNumber], [PackSize], [PurchaseUnitPrice], [PurchaseTotalPrice], [Unit], [Geleverd], [QuantityDelivered])
VALUES
(@StockOrderId, 1, 50, 1, N'Hard drive 1', N'PO-2026-001', N'1', 32.00, 1600.00, N'pcs', 0, 0),
(@StockOrderId, 2, 30, 1, N'Hard drive 2', N'PO-2026-001', N'1', 35.00, 1050.00, N'pcs', 0, 0),
(@StockOrderId, 3, 20, 1, N'Hard drive 3', N'PO-2026-001', N'1', 42.00, 840.00, N'pcs', 0, 0);

-- GRN — partial receive on first PO line (StockOrderDeliveries)
INSERT INTO [Products].[StockOrderDeliveries] ([StockOrderDetail], [DeliveryDocumentNumber], [Date], [Quantity], [QuantityInvoiced])
SELECT sol.[Id], N'GRN-2026-001', DATEADD(day, -1, GETUTCDATE()), 10, 10
FROM [Products].[StockOrderLines] sol
WHERE sol.[StockOrderId] = @StockOrderId AND sol.[ProductId] = 1;

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

-- Webshop product category labels (admin: /admin/webshop-product-structures)
SET IDENTITY_INSERT [Products].[WebshopProductStructures] ON;
INSERT INTO [Products].[WebshopProductStructures] ([Id], [NameEn], [NameFr], [NameNl], [ParentTaskId]) VALUES
(1,  N'Storage',       N'Stockage',     N'Opslag',       NULL),
(2,  N'HDD',           N'DD',           N'HDD',          1),
(3,  N'SSD',           N'SSD',          N'SSD',          1),
(4,  N'Enterprise HDD',N'DD entreprise',N'Enterprise HDD', 2),
(5,  N'Consumer HDD',  N'DD grand public', N'Consumer HDD', 2),
(6,  N'Accessories',   N'Accessoires',  N'Accessoires',  NULL),
(7,  N'Cables',        N'Câbles',       N'Kabels',       6),
(8,  N'Rack',          N'Rack',         N'Rack',         6),
(9,  N'Services',      N'Services',     N'Diensten',     NULL),
(10, N'Installation',  N'Installation', N'Installatie',  9),
(11, N'Warranty',      N'Garantie',     N'Garantie',     9);
SET IDENTITY_INSERT [Products].[WebshopProductStructures] OFF;

-- Link products to webshop product structure (ProductStructureId)
UPDATE [Products].[Product] SET [ProductStructureId] = 5 WHERE [ProductId] IN (1, 2);
UPDATE [Products].[Product] SET [ProductStructureId] = 3 WHERE [ProductId] IN (3, 4);
UPDATE [Products].[Product] SET [ProductStructureId] = 4 WHERE [ProductId] IN (5, 6);
UPDATE [Products].[Product] SET [ProductStructureId] = 7 WHERE [ProductId] = 8;
UPDATE [Products].[Product] SET [ProductStructureId] = 8 WHERE [ProductId] = 7;
UPDATE [Products].[Product] SET [ProductStructureId] = 10 WHERE [ProductId] = 9;
UPDATE [Products].[Product] SET [ProductStructureId] = 11 WHERE [ProductId] = 10;

-- Product options (admin: /admin/product-options)
INSERT INTO [Products].[ProductOptions]
    ([Name], [ValueType], [IsRequired], [ProductId], [ProductionNotesFlag], [QuoteNotesFlag], [CalculatePrice], [SortOrder],
     [NameFr], [Tag], [NameEn])
VALUES
(N'Capacity', N'List', 1, 1, 0, 0, 0, 1, N'Capacité', N'capacity', N'Capacity'),
(N'Interface', N'List', 0, 1, 0, 0, 0, 2, N'Interface', N'interface', N'Interface'),
(N'Cable length', N'List', 0, 8, 0, 0, 0, 1, N'Longueur câble', N'length', N'Cable length');

DECLARE @OptCapacity int;
DECLARE @OptInterface int;
DECLARE @OptCableLen int;

SELECT @OptCapacity = [Id] FROM [Products].[ProductOptions] WHERE [ProductId] = 1 AND [NameEn] = N'Capacity';
SELECT @OptInterface = [Id] FROM [Products].[ProductOptions] WHERE [ProductId] = 1 AND [NameEn] = N'Interface';
SELECT @OptCableLen = [Id] FROM [Products].[ProductOptions] WHERE [ProductId] = 8 AND [NameEn] = N'Cable length';

INSERT INTO [Products].[ProductOptionValue] ([ProductOptionId], [Value], [ValueEn], [ValueFr], [SortOrder]) VALUES
(@OptCapacity, N'500 GB', N'500 GB', N'500 Go', 1),
(@OptCapacity, N'1 TB', N'1 TB', N'1 To', 2),
(@OptCapacity, N'2 TB', N'2 TB', N'2 To', 3),
(@OptInterface, N'SATA III', N'SATA III', N'SATA III', 1),
(@OptInterface, N'NVMe', N'NVMe', N'NVMe', 2),
(@OptCableLen, N'0.5 m', N'0.5 m', N'0,5 m', 1),
(@OptCableLen, N'1 m', N'1 m', N'1 m', 2);

-- Quantity tiers (admin: /admin/product-tiers)
INSERT INTO [Products].[ProductQuantityTiers] ([MinimumQuantity], [Discount], [ProductId]) VALUES
(10, 5.00, 1),
(50, 10.00, 1),
(10, 3.00, 2),
(25, 7.50, 3);

-- Customer-specific product discounts (admin: /admin/customer-discounts)
INSERT INTO [Crm].[CustomerProductDiscounts]
    ([CustomerId], [DiscountPercentage], [ProductId], [Notes], [FromAddress], [ValidTo], [CustomerTypeId], [CreatedAt], [UserId])
VALUES
(1, 10.0000, 1, N'Northwind HDD volume deal', DATEADD(year, -1, GETUTCDATE()), NULL, 1, GETUTCDATE(), 1),
(2, 5.0000, 3, N'Contoso SSD promo', DATEADD(month, -3, GETUTCDATE()), DATEADD(month, 9, GETUTCDATE()), 1, GETUTCDATE(), 1),
(4, 15.0000, 2, N'Tailspin loyalty', DATEADD(month, -6, GETUTCDATE()), NULL, 1, GETUTCDATE(), 1);

-- Contacts + customer links
SET IDENTITY_INSERT [Customers].[Contact] ON;
INSERT INTO [Customers].[Contact] (
    [ContactId], [ContactBox], [ContactEmail], [ContactFax], [ContactHouseNumber], [ContactLogin], [ContactMobile],
    [ContactLastName], [ContactPassword], [ContactStreet], [ContactPhone], [ContactFirstName],
    [IsInstallerContact], [ContactCityId], [IsInternalUserContact], [ContactLanguageId],
    [EmailQuote], [EmailOrderConfirmation], [EmailPlanning], [EmailDeliveryReady], [EmailDelivered], [EmailBilling],
    [ContactJobTitle]
) VALUES
(1, N'', N'john.buyer@northwind.demo', N'', N'10', N'john.buyer', N'+32 470 111 001', N'Buyer', N'demo', N'Buyer Lane', N'+32 2 111 0001', N'John',
 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, N'Purchasing'),
(2, N'', N'mary.smith@tailspin.demo', N'', N'4', N'mary.smith', N'+32 470 222 002', N'Smith', N'demo', N'Demo Street', N'+32 2 111 0004', N'Mary',
 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, N'Operations'),
(3, N'', N'warehouse@demo-supplier.local', N'', N'1', N'supplier.contact', N'', N'Peeters', N'demo', N'Industrial Zone', N'+32 2 555 0100', N'Jan',
 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, N'Logistics');
SET IDENTITY_INSERT [Customers].[Contact] OFF;

SET IDENTITY_INSERT [Customers].[CustomerContacts] ON;
INSERT INTO [Customers].[CustomerContacts]
    ([CustomerContactId], [CustomerId], [ContactId], [Notes], [IsDefaultContact], [JobTitle]) VALUES
(1, 1, 1, N'Primary buyer', 1, N'Purchasing manager'),
(2, 4, 2, N'Storefront login contact', 1, N'Buyer'),
(3, NULL, 3, N'Supplier logistics', 0, N'Warehouse');
SET IDENTITY_INSERT [Customers].[CustomerContacts] OFF;

-- Legacy staff users (admin: /admin/staff-users) — parallel to Identity, not AspNetUsers
INSERT INTO [Settings].[StaffUsers] (
    [Crm], [Offerte], [Bestellingen], [Productie], [Boekhouding], [Planning], [Admin], [Verkoper],
    [Color], [DoSyncExchange], [RechtstreeksGsmNrtonen], [RechtstreeksTelefoonNrTonen], [UserGroupId], [DirecteLosseVerkoop],
    [Login], [Password], [EmailTemplate], [Tel], [FirstName], [LastName], [BaseCompaniesId], [TaalId], [Address], [HiredAt],
    [ProductBeheer], [PlanningBinnenDienst], [PlanningBuitendienst], [PlanningProjecten], [TextColor], [JobTitle],
    [CanAccessStockManagement], [CanAccessBilling]
) VALUES
(1, 1, 1, 0, 1, 1, 1, 1, 2263842, 0, 0, 0, 1, 1, N'admin@webshop.com', N'demo', N'admin@webshop.com', N'+32 2 100 0001',
 N'Anna', N'Rodriguez', 1, 1, N'Brussels', DATEADD(year, -3, GETUTCDATE()), 1, 1, 0, 1, 16777215, N'Admin', 1, 1),
(1, 1, 1, 0, 0, 0, 0, 1, 3447003, 0, 0, 0, 1, 0, N'manager@webshop.com', N'demo', N'manager@webshop.com', N'+32 2 100 0002',
 N'John', N'Sales', 1, 1, N'Brussels', DATEADD(year, -2, GETUTCDATE()), 0, 0, 0, 0, 16777215, N'Sales manager', 1, 0),
(0, 0, 0, 0, 0, 0, 0, 0, 9803157, 0, 0, 0, 2, 0, N'warehouse.demo', N'demo', N'warehouse@webshop.local', N'+32 2 100 0003',
 N'Kim', N'Vermeulen', 1, 1, N'Brussels', DATEADD(year, -1, GETUTCDATE()), 0, 0, 0, 0, 16777215, N'Warehouse lead', 1, 0);

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

-- Email queue names (future low-stock email — IEmailSender is no-op; alerts are in-app today)
SET IDENTITY_INSERT [Emails].[EmailQueues] ON;
INSERT INTO [Emails].[EmailQueues] ([Id], [Name]) VALUES
(1, N'Outbound'),
(2, N'LowStockAlerts');
SET IDENTITY_INSERT [Emails].[EmailQueues] OFF;

-- Demo queued emails (LowStockAlerts queue — worker sends in prod only)
INSERT INTO [Emails].[EmailMessages]
    ([ToAddress], [FromAddress], [Subject], [Body], [PreviewText], [SentAt], [ReceivedAt], [EmailQueueId], [RequiresAction], [CustomerId])
VALUES
(N'admin@webshop.com', N'noreply@webshop.local', N'Low stock: Hard drive 5',
 N'Hard drive 5 at Main warehouse: 9 on hand (minimum 1).', N'Hard drive 5 at Main warehouse: 9 on hand (minimum 1).',
 DATEADD(hour, -2, GETUTCDATE()), DATEADD(hour, -2, GETUTCDATE()), 2, 1, NULL),
(N'admin@webshop.com', N'noreply@webshop.local', N'Out of stock: Installation service',
 N'Installation service is out of stock at Main warehouse.', N'Installation service is out of stock at Main warehouse.',
 DATEADD(hour, -1, GETUTCDATE()), DATEADD(hour, -1, GETUTCDATE()), 2, 1, NULL);

-- Demo accounting document (paid invoice linked to accepted order 2026009)
DECLARE @DemoOrderId int;
DECLARE @DemoProjectId int = 4;
SELECT @DemoOrderId = [Id] FROM [Projects].[Orders] WHERE [OrderNumber] = 2026009;

IF @DemoOrderId IS NOT NULL
BEGIN
    INSERT INTO [Accounting].[AccountingDocuments] (
        [DocumentCreatedAt], [DocumentVatAmount], [DocumentNetAmount], [DocumentTotalAmount], [DocumentDate], [IsFinal],
        [DocumentCustomerBox], [CustomerId], [DocumentCustomerName], [DocumentCustomerNumber], [DocumentCustomerPostalCode],
        [DocumentCustomerStreet], [DocumentCustomerCity], [DocumentNumber], [DocumentTypeId], [CreatedBy], [OrderId],
        [DocumentCustomerCompanyName], [DocumentCustomerVatNumber], [DocumentCustomerCountry], [DocumentCustomerHouseNumber],
        [ProjectId], [BaseCompanyVatNumberId], [BetaaldOp], [BetalingswijzeId]
    )
    SELECT
        GETUTCDATE(), ROUND(t.[Net] * 0.21, 2), t.[Net], ROUND(t.[Net] * 1.21, 2), CAST(GETUTCDATE() AS date), 1,
        c.[CustomerBox], c.[CustomerId], c.[CustomerName], CAST(c.[CustomerId] AS nvarchar(50)), N'1000',
        c.[CustomerStreet], N'Brussels', N'INV-2026-0009', 1, 1, @DemoOrderId,
        c.[CustomerName], c.[CustomerVatNumber], N'Belgium', c.[CustomerHouseNumber],
        @DemoProjectId, 1, DATEADD(day, -1, GETUTCDATE()), 1
    FROM [Customers].[Customers] c
    CROSS APPLY (
        SELECT SUM(ol.[TotalExclVat]) AS [Net]
        FROM [Projects].[OrderLines] ol
        WHERE ol.[OrderId] = @DemoOrderId
    ) t
    WHERE c.[CustomerId] = 4;
END;

-- PrePay / Mollie demo milestones on 3 accepted webshop orders (order numbers 2026009–2026011)
INSERT INTO [Projects].[OrderAdvancePayments]
    ([OrderId], [Name], [Percent], [IsFinalInvoice], [SortOrder], [Amount], [AdvancePaymentVisibility],
     [MolliePaymentId], [MolliePaymentStatus], [MolliePaidAt], [MollieCheckoutUrl])
SELECT
    o.[Id],
    N'Online payment',
    CAST(100 AS decimal(18,4)),
    0,
    1,
    totals.[AmountInclVat],
    N'Default',
    CASE o.[OrderNumber]
        WHEN 2026009 THEN N'tr_demo_paid_seed01'
        WHEN 2026010 THEN N'tr_demo_open_seed02'
        ELSE NULL
    END,
    CASE o.[OrderNumber]
        WHEN 2026009 THEN N'paid'
        WHEN 2026010 THEN N'open'
        ELSE NULL
    END,
    CASE o.[OrderNumber]
        WHEN 2026009 THEN DATEADD(day, -2, GETUTCDATE())
        ELSE NULL
    END,
    CASE o.[OrderNumber]
        WHEN 2026010 THEN N'https://www.mollie.com/checkout/test/demo-open'
        ELSE NULL
    END
FROM [Projects].[Orders] o
INNER JOIN (
    SELECT [OrderId], SUM([TotalInclVat]) AS [AmountInclVat]
    FROM [Projects].[OrderLines]
    GROUP BY [OrderId]
) totals ON totals.[OrderId] = o.[Id]
WHERE o.[OrderNumber] IN (2026009, 2026010, 2026011);

-- In-app low stock alerts (dashboard banner + unread count; email push not wired yet)
INSERT INTO [dbo].[StockLowAlerts]
    ([ProductStockLocationId], [ProductId], [ProductName], [StockLocationId], [StockLocationName],
     [Quantity], [MinQuantity], [CreatedAt], [IsRead], [ReadAt])
SELECT
    psl.[Id],
    psl.[ProductId],
    p.[NameEn],
    psl.[StockLocationId],
    sl.[Name],
    psl.[Quantity],
    psl.[MinQuantity],
    DATEADD(minute, -45 + (psl.[ProductId] * 3), GETUTCDATE()),
    CASE WHEN psl.[ProductId] IN (7, 8) THEN 1 ELSE 0 END,
    CASE WHEN psl.[ProductId] IN (7, 8) THEN DATEADD(minute, -30, GETUTCDATE()) ELSE NULL END
FROM [Products].[ProductStockLocations] psl
INNER JOIN [Products].[Product] p ON p.[ProductId] = psl.[ProductId]
INNER JOIN [Products].[StockLocations] sl ON sl.[Id] = psl.[StockLocationId]
WHERE psl.[Quantity] <= psl.[MinQuantity];

-- Audit log demo rows (audit grid + checkout/Mollie trail)
DECLARE @AuditNow datetime2 = GETUTCDATE();

INSERT INTO [dbo].[AuditLogs]
    ([Timestamp], [Action], [EntityName], [EntityId], [UserDisplayName], [Severity], [Success],
     [ErrorMessage], [IpAddress], [UserAgent], [OldValues], [NewValues])
VALUES
(@AuditNow, N'Login', N'ApplicationUser', N'seed-admin', N'admin@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', NULL, N'{"email":"admin@webshop.com","roles":["Admin","Manager"]}'),
(DATEADD(minute, -40, @AuditNow), N'Create', N'Product', N'101', N'admin@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', NULL, N'{"nameEn":"Demo pump","orderPartNumber":"PMP-101"}'),
(DATEADD(minute, -35, @AuditNow), N'Update', N'Product', N'101', N'admin@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', N'{"nameEn":"Demo pump","grossSalesPrice":120.00}', N'{"nameEn":"Demo pump XL","grossSalesPrice":135.50}'),
(DATEADD(minute, -30, @AuditNow), N'ReportExport', N'ProductsCatalogReport', NULL, N'admin@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', NULL, N'{"reportKey":"ProductsCatalogReport","format":"csv"}'),
(DATEADD(minute, -25, @AuditNow), N'Create', N'Customer', N'42', N'manager@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', NULL, N'{"name":"ACME BV","email":"acme@example.com"}'),
(DATEADD(minute, -20, @AuditNow), N'LoginFailed', N'ApplicationUser', NULL, N'unknown@test.com', N'Warning', 0,
 N'Invalid email or password', N'127.0.0.1', N'Seed', NULL, N'{"email":"unknown@test.com","reason":"InvalidPassword"}'),
(DATEADD(minute, -18, @AuditNow), N'CheckoutStarted', N'Order', N'9', N'customer@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', NULL, N'{"orderNumber":2026009,"paymentMethod":"PrePay","totalInclVat":null}'),
(DATEADD(minute, -16, @AuditNow), N'PaymentPaid', N'OrderAdvancePayment', N'1', N'system', N'Information', 1,
 NULL, N'127.0.0.1', N'MollieWebhook', NULL, N'{"molliePaymentId":"tr_demo_paid_seed01","status":"paid"}'),
(DATEADD(minute, -15, @AuditNow), N'Update', N'Order', N'5001', N'manager@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', N'{"statusId":2}', N'{"statusId":5}'),
(DATEADD(minute, -10, @AuditNow), N'Delete', N'ProductOption', N'7', N'admin@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', N'{"nameEn":"Color red"}', NULL),
(DATEADD(minute, -5, @AuditNow), N'Logout', N'ApplicationUser', N'seed-admin', N'admin@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', NULL, N'{"reason":"ManualLogout"}'),
(DATEADD(minute, -2, @AuditNow), N'ReportExport', N'StockMovementsReport', NULL, N'admin@webshop.com', N'Information', 1,
 NULL, N'127.0.0.1', N'Seed', NULL, N'{"reportKey":"StockMovementsReport","format":"pdf","filters":{"dateFrom":"2026-01-01"}}');

COMMIT TRANSACTION;
GO

-- Summary
SELECT N'Products on webshop' AS [Metric], COUNT(*) AS [Value] FROM [Products].[Product] WHERE [ShowOnWebshop] = 1
UNION ALL SELECT N'Webshop structure nodes', COUNT(*) FROM [Products].[WebshopStructures]
UNION ALL SELECT N'Customers', COUNT(*) FROM [Customers].[Customers]
UNION ALL SELECT N'Orders this month', COUNT(*) FROM [Projects].[Orders] WHERE [CreatedAt] >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
UNION ALL SELECT N'Pending orders', COUNT(*) FROM [Projects].[Orders] WHERE [IsAccepted] = 0
UNION ALL SELECT N'Product prices', COUNT(*) FROM [Products].[ProductPrices]
UNION ALL SELECT N'Azure file folders', COUNT(*) FROM [Files].[AzureFileFolders]
UNION ALL SELECT N'Product images (AzureFiles)', COUNT(*) FROM [Files].[AzureFiles] WHERE [ProductId] IS NOT NULL AND [IsPrimaryImage] = 1
UNION ALL SELECT N'Payment methods', COUNT(*) FROM [Settings].[PaymentMethods]
UNION ALL SELECT N'Stock movements', COUNT(*) FROM [Products].[StockMovements]
UNION ALL SELECT N'Open purchase orders', COUNT(*) FROM [Products].[StockOrder] WHERE [IsCompleted] = 0
UNION ALL SELECT N'Delivery addresses', COUNT(*) FROM [Crm].[CustomerDeliveryAddresses]
UNION ALL SELECT N'Low stock (product rows)', COUNT(*) FROM [Products].[ProductStockLocations] WHERE [Quantity] <= [MinQuantity]
UNION ALL SELECT N'In-app stock alerts (unread)', COUNT(*) FROM [dbo].[StockLowAlerts] WHERE [IsRead] = 0
UNION ALL SELECT N'Audit log rows', COUNT(*) FROM [dbo].[AuditLogs]
UNION ALL SELECT N'Order advance payments', COUNT(*) FROM [Projects].[OrderAdvancePayments]
UNION ALL SELECT N'Email queues', COUNT(*) FROM [Emails].[EmailQueues]
UNION ALL SELECT N'Queued emails (demo)', COUNT(*) FROM [Emails].[EmailMessages]
UNION ALL SELECT N'Webshop product structures', COUNT(*) FROM [Products].[WebshopProductStructures]
UNION ALL SELECT N'Product options', COUNT(*) FROM [Products].[ProductOptions]
UNION ALL SELECT N'Product quantity tiers', COUNT(*) FROM [Products].[ProductQuantityTiers]
UNION ALL SELECT N'Price list categories', COUNT(*) FROM [Products].[PriceListCategories]
UNION ALL SELECT N'User groups', COUNT(*) FROM [Settings].[UserGroups]
UNION ALL SELECT N'Staff users (domain)', COUNT(*) FROM [Settings].[StaffUsers]
UNION ALL SELECT N'Customer product discounts', COUNT(*) FROM [Crm].[CustomerProductDiscounts]
UNION ALL SELECT N'Contacts', COUNT(*) FROM [Customers].[Contact]
UNION ALL SELECT N'Customer contacts', COUNT(*) FROM [Customers].[CustomerContacts]
UNION ALL SELECT N'Stock order deliveries (GRN)', COUNT(*) FROM [Products].[StockOrderDeliveries]
UNION ALL SELECT N'Accounting documents', COUNT(*) FROM [Accounting].[AccountingDocuments]
UNION ALL SELECT N'Revenue YTD', ISNULL(SUM(ol.[TotalExclVat]), 0)
    FROM [Projects].[OrderLines] ol INNER JOIN [Projects].[Orders] o ON o.[Id] = ol.[OrderId]
    WHERE o.[IsAccepted] = 1 AND o.[CreatedAt] >= DATEFROMPARTS(YEAR(GETUTCDATE()), 1, 1);
GO

PRINT N'Demo seed completed successfully.';
GO
