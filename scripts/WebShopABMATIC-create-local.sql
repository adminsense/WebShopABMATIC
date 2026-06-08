-- WebShopABMATIC local: database + schemas + tables (English names)
-- Generated from ABMATIC-create-local.sql
-- sqlcmd -S localhost -E -i WebShopABMATIC-create-local.sql

USE [master];
GO

IF DB_ID(N'WebShopABMATIC') IS NOT NULL
BEGIN
    ALTER DATABASE [WebShopABMATIC] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [WebShopABMATIC];
END
GO

CREATE DATABASE [WebShopABMATIC] COLLATE Latin1_General_CI_AS;
GO

USE [WebShopABMATIC];
GO
CREATE SCHEMA [Files] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Accounting] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Crm] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Emails] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Settings] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Customers] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Logging] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Products] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Projects] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Tasks] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Users] AUTHORIZATION [dbo];
GO

CREATE TABLE [Files].[AzureFiles]
(
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Extension] nvarchar(50) NOT NULL,
    [AzureFileFolderId] int NOT NULL,
    [Created] datetime NOT NULL,
    [CreatedByUserId] int NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [BlobRef] nvarchar(2000) NOT NULL,
    [ThumbRef] nvarchar(2000),
    [Modified] datetime,
    [ModifiedByUserId] int,
    [OrderId] int,
    [ProjectId] int,
    [EmailId] int,
    [CustomerId] int,
    [Deleted] bit,
    [DeletedByUserId] int,
    [ProductId] int,
    [IsPrimaryImage] bit,
    [PublishToWeb] bit,
    [UserId] int,
    [IsGeneral] bit,
    [SupplierId] int,
    [ManufacturerId] int,
    [OrderLineId] int,
    [IsLinkedRef] bit,
    [SendToCustomer] bit NOT NULL,
    [SendOnSupplierOrder] bit NOT NULL,
    [StockOrderId] int,
    CONSTRAINT [PK_Files_AzureFiles] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Files].[AzureFileFolders]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [IsForCrm] bit NOT NULL,
    [IsForOrder] bit NOT NULL,
    [IsForProject] bit NOT NULL,
    [IsForProduct] bit NOT NULL,
    [IsForUser] bit NOT NULL,
    [IsForGeneralUse] bit NOT NULL,
    [SortOrder] decimal(10,2) NOT NULL,
    CONSTRAINT [PK_Files_AzureFileFolders] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Files].[StoredFiles]
(
    [StoredFileId] int IDENTITY(1,1) NOT NULL,
    [FileName] nvarchar(250) NOT NULL,
    [FileType] int NOT NULL,
    [Created] datetime NOT NULL,
    [Updated] datetime,
    [CreatedBy] int NOT NULL,
    [Data] varbinary NOT NULL,
    CONSTRAINT [PK_Files_StoredFiles] PRIMARY KEY CLUSTERED ([StoredFileId])
);
GO

CREATE TABLE [Files].[OrderFileLinks]
(
    [StoredFileId] int NOT NULL,
    [OrderId] int NOT NULL,
    [OrderFileLinkId] int IDENTITY(1,1) NOT NULL,
    CONSTRAINT [PK_Files_OrderFileLinks] PRIMARY KEY CLUSTERED ([OrderFileLinkId])
);
GO

CREATE TABLE [Accounting].[VatTypes]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [InvoiceText] nvarchar(512) NOT NULL,
    [Percentage] decimal(18,2) NOT NULL,
    [InvoiceTextEn] nvarchar(512) NOT NULL,
    [InvoiceTextFr] nvarchar(512) NOT NULL,
    [ExplanationNl] nvarchar NOT NULL,
    [ExplanationFr] nvarchar NOT NULL,
    [ExplanationEn] nvarchar NOT NULL,
    [IsDefault] bit,
    [TaxExemptionReason] nvarchar(50),
    [TaxExemptionReasonCode] nvarchar(50),
    [PeppolCode] nvarchar(50),
    CONSTRAINT [PK_Accounting_VatTypes] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Accounting].[AccountingDocumentLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupName] nvarchar(50) NOT NULL,
    [GateId] int,
    [ProjectId] int,
    [ProductId] int,
    [ProductName] nvarchar(512) NOT NULL,
    [ProductOmschrijving] nvarchar NOT NULL,
    [Quantity] decimal(18,4) NOT NULL,
    [EenheidsPrijs] decimal(18,4) NOT NULL,
    [Unit] nvarchar(50) NOT NULL,
    [DiscountPercentage] decimal(18,4) NOT NULL,
    [Subtotaal] decimal(18,4) NOT NULL,
    [BtwPercentage] decimal(18,4) NOT NULL,
    [TotalAmount] decimal(18,4) NOT NULL,
    [DocumentId] int NOT NULL,
    [InstallationPrice] decimal(18,4) NOT NULL,
    [AssemblyPrice] decimal(18,4) NOT NULL,
    [IsOption] bit NOT NULL,
    [BtwBedrag] decimal(18,4) NOT NULL,
    [KortingBedrag] decimal(18,4) NOT NULL,
    [SortOrder] int NOT NULL,
    [PrijslijsType] nvarchar(20),
    [GroepNaam] nvarchar(500) NOT NULL,
    [IsOptieVanBestellingDetailId] int,
    [ProductType] int,
    [OrderLineId] int,
    [GateComponentId] int,
    [LeveringAfhalingOkOp] date,
    [NettoCommisieEenheidsPrijs] decimal(18,4),
    [OrderId] int NOT NULL,
    [BestelNummer] nvarchar(150),
    [BasePrice] decimal(18,2) NOT NULL,
    [KortingType] nvarchar(2) NOT NULL,
    [BebatProductId] int,
    [RecupelProductId] int,
    [BebatNaam] nvarchar(250),
    [RecupelNaam] nvarchar(500),
    [BebatStukPrijs] decimal(18,2) NOT NULL,
    [BebatAantal] decimal(18,2) NOT NULL,
    [BebatTotaal] decimal(18,2) NOT NULL,
    [RecupelStukPrijs] decimal(18,2) NOT NULL,
    [RecupelAantal] decimal(18,2) NOT NULL,
    [RecupelTotaal] decimal(18,2) NOT NULL,
    [MontageStukPrijs] decimal(18,2) NOT NULL,
    [AssemblageStukPrijs] decimal(18,2) NOT NULL,
    [IsProducedCompositeProduct] bit,
    [IsVoorschot] bit,
    [IsTextOnly] bit,
    [KlaarVoorVerzendingOp] datetime,
    [DeliveredAt] datetime,
    [NietTellenWegensVoorschot] bit NOT NULL,
    [IsGarantie] bit NOT NULL,
    [IsPopUpRow] bit NOT NULL,
    [Notes] nvarchar,
    [BasisPrijsTotaal] decimal(18,2) NOT NULL,
    [StartupCost] decimal(18,4) NOT NULL,
    [OpstartKostTotaal] decimal(18,2) NOT NULL,
    [VatTypeId] int NOT NULL,
    [SupplierId] int,
    [DocumentDetailMasterId] int NOT NULL,
    [DetailVanMasterId] int,
    [AankoopStukPrijs] decimal(18,2),
    [Goederen] decimal(18,2),
    [Diensten] decimal(18,2),
    [GoodsCode] nvarchar(50),
    [Weight] decimal(18,3),
    [AanvullendeEenheden] decimal(18),
    [LandVanOorsprong] int,
    CONSTRAINT [PK_Accounting_AccountingDocumentLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Accounting].[AccountingDocuments]
(
    [DocumentCreatedAt] datetime NOT NULL,
    [DocumentVatAmount] decimal(18,2) NOT NULL,
    [DocumentNetAmount] decimal(18,2) NOT NULL,
    [DocumentTotalAmount] decimal(18,2) NOT NULL,
    [DocumentDate] datetime NOT NULL,
    [IsFinal] bit,
    [AccountingDocumentId] int IDENTITY(1,1) NOT NULL,
    [DocumentCustomerBox] nvarchar(50) NOT NULL,
    [CustomerId] int NOT NULL,
    [DocumentCustomerName] nvarchar(150) NOT NULL,
    [DocumentCustomerNumber] nvarchar(50) NOT NULL,
    [DocumentCustomerPostalCode] nvarchar(50) NOT NULL,
    [DocumentCustomerStreet] nvarchar(150) NOT NULL,
    [DocumentCustomerCity] nvarchar(150) NOT NULL,
    [DocumentNumber] nvarchar(50),
    [DocumentTypeId] int NOT NULL,
    [CreatedBy] int NOT NULL,
    [OrderId] int,
    [DocumentCustomerCompanyName] nvarchar(150) NOT NULL,
    [DocumentCustomerVatNumber] nvarchar(50) NOT NULL,
    [DocumentCustomerCountry] nvarchar(50) NOT NULL,
    [DocumentCustomerHouseNumber] nvarchar(50) NOT NULL,
    [Vervaldatum] datetime,
    [GecrediteerdeFactuur] nvarchar(50),
    [DocGecrediteerdeFactuurDatum] datetime,
    [ProjectContactNaam] nvarchar(250),
    [ProjectContactPhone] nvarchar(50),
    [ProjectContactMobile] nvarchar(50),
    [ProjectContactEmail] nvarchar(250),
    [LeverAdresContactNaam] nvarchar(250),
    [LeverAdresContactPhone] nvarchar(50),
    [LeverAdresContactMobile] nvarchar(50),
    [LeverAdresContactEmail] nvarchar(250),
    [EindklantContactNaam] nvarchar(250),
    [EindklantContactPhone] nvarchar(50),
    [EindklantContactMobile] nvarchar(50),
    [EindklantContactEmail] nvarchar(250),
    [DeliveryDate] datetime,
    [BaseCompanyId] int,
    [DocumentCustomerLanguageId] int,
    [ProjectId] int NOT NULL,
    [LeverAdresStraat] nvarchar(150),
    [LeverAdresNr] nvarchar(20),
    [LeverAdresBus] nvarchar(20),
    [LeverAdresStad] nvarchar(150),
    [LeverAdresPostcode] nvarchar(20),
    [Notes] nvarchar,
    [LeverAdresLand] nvarchar(100),
    [IsVoorschotFactuur] bit,
    [ReedsGefactureerdVoorschot] decimal(18,2),
    [VoorschotNaam] nvarchar(500),
    [HeeftCommisie] bit,
    [VoorschotPercentage] decimal(18,4),
    [VerzondenVia] nvarchar(500),
    [DossierBeheerder] nvarchar(50),
    [ProjectManagerUserId] nvarchar(50),
    [AccountManagerUserId] nvarchar(50),
    [BetaaldOp] datetime,
    [BetalingswijzeId] int,
    [Reason] nvarchar,
    [ToelichtingVoorschotten] nvarchar,
    [BaseCompanyVatNumberId] int NOT NULL,
    [DocKlantGebouwNaam] nvarchar(400),
    [DocumentOpmerking] nvarchar,
    [CountryId] int,
    [PeppolVerzondenOp] datetime,
    [EasypostId] nvarchar(250),
    [PeppolStatus] nvarchar(250),
    CONSTRAINT [PK_Accounting_AccountingDocuments] PRIMARY KEY CLUSTERED ([AccountingDocumentId])
);
GO

CREATE TABLE [Accounting].[DocumentTypes]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [NameFr] nvarchar(150) NOT NULL,
    [ParameterId] int NOT NULL,
    [NameEn] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_Accounting_DocumentTypes] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Accounting].[IntrastatReportLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [DocumentDocId] int,
    [ProductName] nvarchar(250) NOT NULL,
    [PartnerLand] nvarchar(50) NOT NULL,
    [TransactieCode] nvarchar(50) NOT NULL,
    [Gewest] nvarchar(50) NOT NULL,
    [GoodsCode] nvarchar(50) NOT NULL,
    [Weight] decimal(18,3) NOT NULL,
    [AanvullendeEenheden] decimal(18,2) NOT NULL,
    [WaardeInEur] decimal(18,2) NOT NULL,
    [Vervoer] nvarchar(50),
    [Incoterm] nvarchar(50),
    [LandVanOorsprong] nvarchar(50) NOT NULL,
    [BtwNrTegenpartij] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Accounting_IntrastatReportLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Accounting].[CustomerExtraDiscounts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Discount] decimal(18,2) NOT NULL,
    [MaxAmount] decimal(18,2) NOT NULL,
    [MinAmount] decimal(18,2) NOT NULL,
    [CustomerTypeFilterId] int NOT NULL,
    [BaseCompanyId] int,
    CONSTRAINT [PK_Accounting_CustomerExtraDiscounts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Salutations]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalutationText] nvarchar(50) NOT NULL,
    [IsMale] bit NOT NULL,
    [IsFemale] bit NOT NULL,
    [LanguageId] int NOT NULL,
    CONSTRAINT [PK_Crm_Salutations] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Activities]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_Crm_Activities] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CalendarEntries]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] int,
    [StartDate] datetime,
    [EndDate] datetime,
    [IsAllDay] bit,
    [Subject] nvarchar(650),
    [Location] nvarchar(150),
    [Description] nvarchar,
    [StatusId] int,
    [LabelId] int,
    [ReminderInfo] nvarchar,
    [RecurrenceInfo] nvarchar,
    [OutlookId] nvarchar(500),
    [UserId] int,
    [CustomerId] int,
    [ProjectId] int,
    [IsSyncedToExchange] bit NOT NULL,
    [OrderId] int,
    [IsOnHold] bit,
    [IsCancelled] bit,
    [OnHoldOrCancelledReason] nvarchar(500),
    [SubjectUserText] nvarchar(500),
    [ContactContactId] int,
    [SystemDescription] nvarchar,
    [IsLeave] bit,
    [CreatedByUserId] int NOT NULL,
    [CreatedAt] datetime NOT NULL,
    CONSTRAINT [PK_Crm_CalendarEntries] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CalendarLabels]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Color] int NOT NULL,
    [IsType] bit NOT NULL,
    [IsCategory] bit NOT NULL,
    [RestrictEditing] bit,
    [IsForLeave] bit,
    [IsInternalService] bit,
    [IsExternalService] bit,
    [IsProjectPlanning] bit,
    CONSTRAINT [PK_Crm_CalendarLabels] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CalendarLogs]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(650),
    [Start] datetime,
    [End] datetime,
    [ChangeAction] nvarchar(50) NOT NULL,
    [OrderId] int,
    [ProjectId] int,
    [CustomerId] int,
    [ContactContactId] int,
    [IsLeave] bit,
    [ChangedAt] datetime NOT NULL,
    [UserId] int NOT NULL,
    [CalendarEntryId] int NOT NULL,
    [ChangedByUserId] int,
    CONSTRAINT [PK_Crm_CalendarLogs] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CalendarStatuses]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Color] int,
    [ShowInTodo] bit NOT NULL,
    CONSTRAINT [PK_Crm_CalendarStatuses] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[PaymentTerms]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [AantalDagen] int NOT NULL,
    [EndOfMonth] bit NOT NULL,
    [IsDefault] bit,
    CONSTRAINT [PK_Crm_PaymentTerms] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[City]
(
    [CityId] int IDENTITY(1,1) NOT NULL,
    [CityName] nvarchar(150) NOT NULL,
    [PostalCode] nvarchar(50) NOT NULL,
    [CountryName] nvarchar(150) NOT NULL,
    [CountryIsoCode] nvarchar(50),
    [CountryId] int,
    CONSTRAINT [PK_Crm_City] PRIMARY KEY CLUSTERED ([CityId])
);
GO

CREATE TABLE [Crm].[ContactProjectRoles]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameNl] nvarchar(150) NOT NULL,
    [NameFr] nvarchar(150) NOT NULL,
    [NameEn] nvarchar(150) NOT NULL,
    [DeactivateAfterOrderClosed] bit NOT NULL,
    CONSTRAINT [PK_Crm_ContactProjectRoles] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Country]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsEu] bit,
    [IsoCode] nvarchar(10) NOT NULL,
    [Name] nvarchar(100),
    CONSTRAINT [PK_Crm_Country] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerOrderStatusRemarks]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderStatusId] int NOT NULL,
    [Notes] nvarchar(250) NOT NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_Crm_CustomerOrderStatusRemarks] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerFollowUps]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar NOT NULL,
    [CustomerId] int NOT NULL,
    [Date] datetime NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Crm_CustomerFollowUps] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerJobCodeRates]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int NOT NULL,
    [JobCodeId] int NOT NULL,
    [HourlyRate] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Crm_CustomerJobCodeRates] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerDeliveryAddresses]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Straat] nvarchar(250) NOT NULL,
    [Number] nvarchar(50) NOT NULL,
    [Bus] nvarchar(50) NOT NULL,
    [CityId] int NOT NULL,
    [Notes] nvarchar(2000) NOT NULL,
    [ContactId] int,
    [CustomerId] int,
    [Name] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Crm_CustomerDeliveryAddresses] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerSupplierDiscounts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int NOT NULL,
    [DiscountPercentage] decimal(18,4) NOT NULL,
    [SupplierId] int NOT NULL,
    [Notes] nvarchar(500),
    [FromAddress] datetime NOT NULL,
    [ValidTo] datetime,
    [CustomerTypeId] int,
    [CreatedAt] datetime NOT NULL,
    [UserId] int NOT NULL,
    [InstallationCustomerTypeId] int,
    [InstallationDiscountPercentage] decimal(18,4),
    CONSTRAINT [PK_Crm_CustomerSupplierDiscounts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerCustomProducts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Description] nvarchar NOT NULL,
    [ProductEenheidId] int NOT NULL,
    [ArticleNumber] nvarchar(250),
    [Notes] nvarchar(1024),
    [IsActive] bit NOT NULL,
    [SupplierArticleNumber] nvarchar(250),
    CONSTRAINT [PK_Crm_CustomerCustomProducts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerCustomProductLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [RawMaterialId] int,
    [SupplierId] int,
    [Quantity] decimal(18,4) NOT NULL,
    [ProductEenheidId] int NOT NULL,
    [PurchasePrice] decimal(18,4),
    [CustomerCustomProductId] int NOT NULL,
    [ArticleNumber] nvarchar(250),
    CONSTRAINT [PK_Crm_CustomerCustomProductLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerCustomProductTiers]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerCustomProductId] int NOT NULL,
    [MinQuantity] decimal(18,2) NOT NULL,
    [MaxQuantity] decimal(18,2) NOT NULL,
    [PieceUnitPrice] decimal(18,4) NOT NULL,
    [ValidFrom] datetime NOT NULL,
    [ValidTo] datetime NOT NULL,
    CONSTRAINT [PK_Crm_CustomerCustomProductTiers] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerNotes]
(
    [CustomerId] int NOT NULL,
    [DocumentTypeId] int,
    [Notes] nvarchar NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    CONSTRAINT [PK_Crm_CustomerNotes] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerProductDiscounts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int NOT NULL,
    [DiscountPercentage] decimal(18,4),
    [ProductId] int NOT NULL,
    [Notes] nvarchar(500),
    [FromAddress] datetime NOT NULL,
    [ValidTo] datetime,
    [CustomerTypeId] int,
    [CreatedAt] datetime NOT NULL,
    [UserId] int NOT NULL,
    [Margin] decimal(18,4),
    [InstallationDiscountPercentage] decimal(18,4),
    [InstallationCustomerTypeId] int,
    CONSTRAINT [PK_Crm_CustomerProductDiscounts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[CustomerStatuses]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Color] int NOT NULL,
    [IsDefault] bit,
    CONSTRAINT [PK_Crm_CustomerStatuses] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Manufacturer]
(
    [ManufacturerId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Phone] nvarchar(50),
    [Mobile] nvarchar(50),
    [Fax] nvarchar(50),
    [Email] nvarchar(150),
    [Address] nvarchar(250),
    [CityId] int,
    [CompanyRegistrationNumber] nvarchar(50),
    [VatNumber] nvarchar(50),
    CONSTRAINT [PK_Crm_Manufacturer] PRIMARY KEY CLUSTERED ([ManufacturerId])
);
GO

CREATE TABLE [Crm].[ProjectContacts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectId] int NOT NULL,
    [OrderId] int,
    [ContactProjectRolId] int NOT NULL,
    [LinkedContactId] int NOT NULL,
    [ContactContactId] int NOT NULL,
    CONSTRAINT [PK_Crm_ProjectContacts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Supplier]
(
    [SupplierId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Phone] nvarchar(50),
    [Mobile] nvarchar(50),
    [Fax] nvarchar(50),
    [Email] nvarchar(150),
    [Address] nvarchar(250),
    [CityId] int,
    [CompanyRegistrationNumber] nvarchar(50),
    [VatNumber] nvarchar(50),
    [SupplierOrderEmail] nvarchar(250),
    [LanguageId] int NOT NULL,
    [GeneralLedgerRevenueAccount] int NOT NULL,
    [IsMainSupplier] bit,
    [IsInactive] bit NOT NULL,
    [IsVerified] bit,
    [PriceListSortOrder] int,
    [PriceListName] nvarchar(100),
    [Notes] nvarchar,
    CONSTRAINT [PK_Crm_Supplier] PRIMARY KEY CLUSTERED ([SupplierId])
);
GO

CREATE TABLE [Crm].[SupplierContacts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierId] int NOT NULL,
    [ContactId] int NOT NULL,
    [IsDefault] bit,
    [ContactFunctionId] int,
    CONSTRAINT [PK_Crm_SupplierContacts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[TaskActions]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [TaskItemId] int NOT NULL,
    [Date] datetime NOT NULL,
    [Explanation] nvarchar NOT NULL,
    CONSTRAINT [PK_Crm_TaskActions] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[TaskTypes]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [CompleteWithinDays] decimal(18,2) NOT NULL,
    [Color] int,
    [ProductionWarning] bit NOT NULL,
    [DeliveryInstallationWarning] bit NOT NULL,
    CONSTRAINT [PK_Crm_TaskTypes] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[TaskItems]
(
    [ReminderDate] datetime NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int,
    [Description] nvarchar(2000) NOT NULL,
    [Type] int NOT NULL,
    [AssignedUserId] int,
    [IsCompleted] bit NOT NULL,
    [CreatedAt] datetime NOT NULL,
    [ProjectId] int,
    [OrderLineId] int,
    [OrderId] int,
    [BaseCompanyId] int,
    [EndDate] datetime NOT NULL,
    [PercentComplete] int,
    [UserGroupId] int,
    [CompletedAt] datetime,
    [CreatedByUserId] int NOT NULL,
    [IsCancelled] bit,
    [CancelledAt] datetime,
    [CheckedByCreatorAt] datetime,
    [IsRead] bit,
    [PopupShown] bit,
    [IsUrgent] bit NOT NULL,
    [RejectionReason] nvarchar,
    [IsRejectionRead] bit,
    [IsRejected] bit,
    [ReadAt] datetime,
    [RejectedAt] datetime,
    CONSTRAINT [PK_Crm_TaskItems] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Emails].[EmailAttachments]
(
    [StoredFileId] int NOT NULL,
    [EmailId] int NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmailFileName] nvarchar(500) NOT NULL,
    [IsEmailOnlyFile] bit NOT NULL,
    CONSTRAINT [PK_Emails_EmailAttachments] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Emails].[EmailMessages]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int,
    [ProjectId] int,
    [FromAddress] nvarchar(500),
    [ToAddress] nvarchar(500),
    [Cc] nvarchar(500),
    [Bcc] nvarchar(500),
    [Subject] nvarchar(1000) NOT NULL,
    [Body] nvarchar NOT NULL,
    [SentAt] datetime NOT NULL,
    [ReceivedAt] datetime NOT NULL,
    [ContactId] int,
    [SupplierId] int,
    [OrderId] int,
    [PreviewText] nvarchar(500) NOT NULL,
    [TaskItemId] int,
    [UserId] int,
    [IsPrivate] bit,
    [SupplierId] int,
    [ManufacturerId] int,
    [OrderLineId] int,
    [RequiresAction] bit,
    [EmailQueueId] int,
    [StockOrderId] int,
    CONSTRAINT [PK_Emails_EmailMessages] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Emails].[EmailQueues]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Emails_EmailQueues] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[AutoNumberings]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] int NOT NULL,
    [Description] nvarchar(150) NOT NULL,
    [Prefix] nvarchar(50) NOT NULL,
    [Tag] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Settings_AutoNumberings] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[BaseCompany]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Street] nvarchar(250) NOT NULL,
    [StreetNr] nvarchar(50) NOT NULL,
    [StreetBox] nvarchar(50) NOT NULL,
    [City] nvarchar(250) NOT NULL,
    [Zip] nvarchar(50) NOT NULL,
    [Country] nvarchar(50) NOT NULL,
    [CustomerId] int,
    [Logo] varbinary,
    [VatNumber] nvarchar(50) NOT NULL,
    [Tel] nvarchar(50) NOT NULL,
    [FaxTemplate] nvarchar(50) NOT NULL,
    [IBAN] nvarchar(50) NOT NULL,
    [BIC] nvarchar(50) NOT NULL,
    [Slogan] nvarchar(250) NOT NULL,
    [AccountingDocumentFooter] nvarchar NOT NULL,
    [Tag] nvarchar(50) NOT NULL,
    [MsGraphApiApplicationId] nvarchar(150),
    [MsGraphApiSecretId] nvarchar(150),
    [MsGraphApiDomain] nvarchar(150),
    [MsGraphApiTenantId] nvarchar(150),
    [FileShareComputername] nvarchar(512),
    [FileShareUser] nvarchar(512),
    [FileSharePassword] nvarchar(512),
    [FileShareShare] nvarchar(512),
    [FileShareAccountName] nvarchar(512),
    [Eori] nvarchar(150),
    [Website] nvarchar(250),
    [AccentColor] int,
    [Bank] nvarchar(250) NOT NULL,
    [EmailTemplate] nvarchar(150),
    [AllowAddNewBlobFiles] bit,
    CONSTRAINT [PK_Settings_BaseCompany] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[BaseCompanyAccess]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [BaseCompanyId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Settings_BaseCompanyAccess] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[BaseCompanyVatNumber]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [BaseCompanyId] int NOT NULL,
    [VatNumber] nvarchar(50) NOT NULL,
    [EoriNumber] nvarchar(50) NOT NULL,
    [Bank1] nvarchar(100),
    [Bank2] nvarchar(100),
    [Bank1Name] nvarchar(100),
    [Bank2Name] nvarchar(100),
    [Bank1Bic] nvarchar(50),
    [Bank2Bic] nvarchar(50),
    CONSTRAINT [PK_Settings_BaseCompanyVatNumber] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[PaymentMethods]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameNl] nvarchar(150) NOT NULL,
    [NameFr] nvarchar(150) NOT NULL,
    [NameEn] nvarchar(150) NOT NULL,
    [IsPrePay] bit NOT NULL,
    [IsPostPay] bit NOT NULL,
    CONSTRAINT [PK_Settings_PaymentMethods] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[GridLayout]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ObjectName] nvarchar(500) NOT NULL,
    [LayoutXml] nvarchar NOT NULL,
    [Notes] nvarchar(150),
    [UsrId] int,
    [IsPivot] bit,
    [PivotName] nvarchar(50),
    CONSTRAINT [PK_Settings_GridLayout] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[RepairCostPrices]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Prijs] decimal(18,2) NOT NULL,
    [ValidTo] datetime,
    [FromAddress] datetime NOT NULL,
    CONSTRAINT [PK_Settings_RepairCostPrices] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[LanguageTags]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [FieldName] nvarchar(250) NOT NULL,
    [NL] nvarchar NOT NULL,
    [FR] nvarchar NOT NULL,
    [SourceKey] nvarchar(250) NOT NULL,
    [En] nvarchar NOT NULL,
    CONSTRAINT [PK_Settings_LanguageTags] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[AppSettings]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Value] nvarchar NOT NULL,
    [Type] nvarchar(50) NOT NULL,
    [BaseCompanyId] int,
    CONSTRAINT [PK_Settings_AppSettings] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[ProductDiscountSuggestions]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [GrossCorrection] decimal(18,4) NOT NULL,
    [Discount] decimal(18,4) NOT NULL,
    [Pro1] decimal(18,4) NOT NULL,
    [Pro2] decimal(18,4) NOT NULL,
    [Pro3] decimal(18,4) NOT NULL,
    [Aan1] decimal(18,4) NOT NULL,
    [Aan2] decimal(18,4) NOT NULL,
    [Par1] decimal(18,4) NOT NULL,
    [DiscountCap] decimal(18,4) NOT NULL,
    [Ond1] decimal(18,4) NOT NULL,
    CONSTRAINT [PK_Settings_ProductDiscountSuggestions] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[ProductDiscountSuggestionLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerTypeId] int NOT NULL,
    [ProductKortingSuggestieId] int NOT NULL,
    [Discount] decimal(18,4) NOT NULL,
    CONSTRAINT [PK_Settings_ProductDiscountSuggestionLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[StandardBillingTerms]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Settings_StandardBillingTerms] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[StandardBillingTermLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Percentage] decimal(18,4) NOT NULL,
    [StdFacturatieVoorwaardenId] int NOT NULL,
    CONSTRAINT [PK_Settings_StandardBillingTermLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[Languages]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Tag] nvarchar(3) NOT NULL,
    [NameFr] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Settings_Languages] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[DocumentTemplates]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500) NOT NULL,
    [EmailTemplate] bit NOT NULL,
    [FaxTemplate] bit NOT NULL,
    [LetterTemplate] bit NOT NULL,
    [IsDefault] bit NOT NULL,
    [Type] int NOT NULL,
    [BaseCompanyId] int,
    [TaalId] int NOT NULL,
    [AzureFileId] bigint,
    [Subject] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Settings_DocumentTemplates] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[TemplateType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Tag] nvarchar(50) NOT NULL,
    [DocumentTypeId] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Settings_TemplateType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[StaffUsers]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Crm] bit NOT NULL,
    [Offerte] bit NOT NULL,
    [Bestellingen] bit NOT NULL,
    [Productie] bit NOT NULL,
    [Boekhouding] bit NOT NULL,
    [Planning] bit NOT NULL,
    [Admin] bit NOT NULL,
    [Verkoper] bit NOT NULL,
    [Color] int NOT NULL,
    [ExchangeLastWatermark] nvarchar(1000),
    [DoSyncExchange] bit NOT NULL,
    [RechtstreeksGsmNrtonen] bit NOT NULL,
    [RechtstreeksTelefoonNrTonen] bit NOT NULL,
    [LinkedInUrl] nvarchar(500),
    [UserGroupId] int,
    [DirecteLosseVerkoop] bit NOT NULL,
    [ReportSales] bit,
    [DefaultCeLabelPrinter] nvarchar(255),
    [DefaultProductionLabelPrinter] nvarchar(255),
    [Login] nvarchar(50) NOT NULL,
    [Password] nvarchar(50) NOT NULL,
    [EmailTemplate] nvarchar(150),
    [Tel] nvarchar(20),
    [FaxTemplate] nvarchar(20),
    [Gsm] nvarchar(20),
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [BaseCompaniesId] int NOT NULL,
    [TaalId] int NOT NULL,
    [Ice1Number] nvarchar(50),
    [Ice1Name] nvarchar(100),
    [Ice2Number] nvarchar(50),
    [Ice2Name] nvarchar(100),
    [PrivateEmail] nvarchar(150),
    [Hr] bit,
    [Address] nvarchar(150) NOT NULL,
    [HiredAt] datetime NOT NULL,
    [LeftAt] datetime,
    [ProductBeheer] bit NOT NULL,
    [PlanningBinnenDienst] bit NOT NULL,
    [PlanningBuitendienst] bit NOT NULL,
    [PlanningProjecten] bit NOT NULL,
    [CompanyPhone] nvarchar(50),
    [CompanyMobile] nvarchar(50),
    [JobTitle] nvarchar(50),
    [ToonInPrijslijst] bit,
    [SelectForInternalPlanning] bit,
    [SelectForExternalPlanning] bit,
    [SelectForProjectPlanning] bit,
    [SelectForLeavePlanning] bit,
    [DefaultPlanningLabelId] int,
    [TextColor] int NOT NULL,
    [CanAccessRevenueReports] bit,
    [CanAccessProfitReports] bit,
    [CanAccessDmsSpecial] bit,
    [CanAccessBulkOrders] bit,
    [CanAccessStockManagement] bit,
    [CanOrderFromOrderScreen] bit,
    [ShowProjects] bit,
    [CanAccessBilling] bit,
    CONSTRAINT [PK_Settings_StaffUsers] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Settings].[UserGroups]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [IsInstallationTeam] bit NOT NULL,
    [IsServiceTeam] bit NOT NULL,
    [IsTransportTeam] bit NOT NULL,
    [OrderStatusGroupId] int,
    CONSTRAINT [PK_Settings_UserGroups] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Customers].[Contact]
(
    [ContactId] int IDENTITY(1,1) NOT NULL,
    [ContactBox] nvarchar(50) NOT NULL,
    [ContactEmail] nvarchar(150) NOT NULL,
    [ContactFax] nvarchar(50) NOT NULL,
    [ContactHouseNumber] nvarchar(50) NOT NULL,
    [ContactLogin] nvarchar(50) NOT NULL,
    [ContactMobile] nvarchar(50) NOT NULL,
    [ContactLastName] nvarchar(50) NOT NULL,
    [ContactPassword] nvarchar(50) NOT NULL,
    [ContactStreet] nvarchar(150) NOT NULL,
    [ContactPhone] nvarchar(50) NOT NULL,
    [ContactFirstName] nvarchar(150) NOT NULL,
    [IsInstallerContact] bit NOT NULL,
    [ContactCityId] int NOT NULL,
    [IsInternalUserContact] bit NOT NULL,
    [SalutationId] int,
    [ContactLanguageId] int NOT NULL,
    [BaseCompanyId] int,
    [InstallerDisplayName] nvarchar(50),
    [ContactJobTitle] nvarchar(100),
    [EmailQuote] bit NOT NULL,
    [EmailOrderConfirmation] bit NOT NULL,
    [EmailPlanning] bit NOT NULL,
    [EmailDeliveryReady] bit NOT NULL,
    [EmailDelivered] bit NOT NULL,
    [EmailBilling] bit NOT NULL,
    [LeftAt] datetime,
    [ContactBuilding] nvarchar(250),
    CONSTRAINT [PK_Customers_Contact] PRIMARY KEY CLUSTERED ([ContactId])
);
GO

CREATE TABLE [Customers].[Customers]
(
    [CustomerId] int IDENTITY(1,1) NOT NULL,
    [CustomerVatNumber] nvarchar(50) NOT NULL,
    [CustomerBox] nvarchar(50) NOT NULL,
    [CustomerHouseNumber] nvarchar(50) NOT NULL,
    [CustomerName] nvarchar(150) NOT NULL,
    [CustomerStreet] nvarchar(150) NOT NULL,
    [CustomerTypeId] int NOT NULL,
    [CustomerPhone] nvarchar(50) NOT NULL,
    [CustomerFax] nvarchar(50) NOT NULL,
    [CustomerNotes] nvarchar(2024),
    [CustomerEmail] nvarchar(250) NOT NULL,
    [CustomerVatSystemId] int NOT NULL,
    [CustomerStatusId] int NOT NULL,
    [CustomerCityId] int NOT NULL,
    [CustomerActivityId] int,
    [CustomerNumber] int,
    [AccountManagerUserId] int NOT NULL,
    [FirstContactName] nvarchar(150),
    [LockedTime] datetime,
    [Locked] bit NOT NULL,
    [LockedBy] nvarchar(100) NOT NULL,
    [RevenueLast12Months] decimal(18,2) NOT NULL,
    [SendPriceListByEmail] bit NOT NULL,
    [PromotionalMailing] bit NOT NULL,
    [DeliveryTypeId] int NOT NULL,
    [InvoicesByMail] bit NOT NULL,
    [CustomerInternalNotes] nvarchar(2024),
    [DigitalInvoicing] bit NOT NULL,
    [CElabelName] nvarchar(50),
    [CElabelNr] nvarchar(50),
    [CustomerPaymentStatus] int,
    [BaseCompaniesId] int,
    [IsInternalCompany] bit,
    [CustomerLanguageId] int NOT NULL,
    [PriceListResContractor] bit,
    [PriceListResDealer] bit,
    [PriceListResConsumer] bit,
    [PriceListIndContractor] bit,
    [PriceListIndDealer] bit,
    [CEemail] nvarchar(250),
    [Logo] varbinary,
    [CustomerGroup] nvarchar(50) NOT NULL,
    [CreatedBy] nvarchar(50),
    [CreatedAt] datetime,
    [ModifiedBy] nvarchar(50),
    [ModifiedAt] datetime,
    [IsVerified] bit,
    [QuoteContactId] int NOT NULL,
    [OrderConfirmationContactId] int NOT NULL,
    [PlanningContactId] int NOT NULL,
    [DeliveryCompleteContactId] int NOT NULL,
    [BillingContactId] int NOT NULL,
    [CommissionRecipientId] int,
    [RequestedCommission] decimal(18,4) NOT NULL,
    [BetaaltermijnId] int NOT NULL,
    [WebShopABMATICLogin] nvarchar(150),
    [WebShopABMATICPasswordHash] nvarchar(512),
    [WebShopABMATICPasswordSalt] nvarchar(512),
    [CustomerBuildingName] nvarchar(250),
    [LaatsteFollowUp] datetime,
    [DeliveryCustomerTypeId] int NOT NULL,
    [PeppolIdSchema] nvarchar(8),
    [PeppolId] nvarchar(250),
    [IdentityUserId] nvarchar(450),
    CONSTRAINT [PK_Customers_Customers] PRIMARY KEY CLUSTERED ([CustomerId])
);
GO

CREATE TABLE [Customers].[CustomerContacts]
(
    [CustomerContactId] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int,
    [ContactId] int NOT NULL,
    [Notes] nvarchar(250),
    [IsDefaultContact] bit,
    [JobTitle] nvarchar(100),
    [SupplierId] int,
    [ManufacturerId] int,
    CONSTRAINT [PK_Customers_CustomerContacts] PRIMARY KEY CLUSTERED ([CustomerContactId])
);
GO

CREATE TABLE [Customers].[CustomerTypes]
(
    [KlantTypeId] int IDENTITY(1,1) NOT NULL,
    [CustomerTypeName] nvarchar(50) NOT NULL,
    [RequiresVatNumber] bit NOT NULL,
    [PaymentTermId] int NOT NULL,
    [VatSystemId] int NOT NULL,
    [BaseDiscount] decimal(18,4) NOT NULL,
    [DeliveryTypeId] int NOT NULL,
    [CustomerTypeNameFr] nvarchar(50) NOT NULL,
    [SortOrder] int NOT NULL,
    [IsDefault] bit,
    CONSTRAINT [PK_Customers_CustomerTypes] PRIMARY KEY CLUSTERED ([KlantTypeId])
);
GO

CREATE TABLE [Logging].[AppErrors]
(
    [DateTime] datetime NOT NULL,
    [ModuleName] nvarchar(50) NOT NULL,
    [Exception] nvarchar(1024) NOT NULL,
    [InnerExceptionMessage] nvarchar(1024) NOT NULL,
    [UserName] nvarchar(50) NOT NULL,
    [ClassName] nvarchar(50) NOT NULL,
    [Id] bigint NOT NULL,
    CONSTRAINT [PK_Logging_AppErrors] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Logging].[ProjectActivities]
(
    [ProjectId] int NOT NULL,
    [ActionCode] int NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [LoggedAt] datetime NOT NULL,
    CONSTRAINT [PK_Logging_ProjectActivities] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[DrawGroup]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Products_DrawGroup] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[RawMaterials]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Description] nvarchar NOT NULL,
    CONSTRAINT [PK_Products_RawMaterials] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[IntrastatCode]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50) NOT NULL,
    [Name] nvarchar(250),
    [Text] nvarchar,
    [MainGroup] nvarchar(500),
    [SubGroup] nvarchar(500),
    CONSTRAINT [PK_Products_IntrastatCode] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[MiscellaneousProducts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [ArticleNumber] nvarchar(100),
    [StockLocationCode] nvarchar(100),
    [Notes] nvarchar(250),
    [PurchasePrice] decimal(18,2),
    [SupplierName] nvarchar(100),
    [AantalInStock] decimal(18,2),
    [LastCountedAt] datetime,
    [GroupName] nvarchar(150),
    CONSTRAINT [PK_Products_MiscellaneousProducts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[OrderTemplate]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [IsActive] bit NOT NULL,
    [CustomerId] int,
    CONSTRAINT [PK_Products_OrderTemplate] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[OrderTemplateDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int,
    [SupplierId] int NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [GroupName] nvarchar(50) NOT NULL,
    [Quantity] decimal(18,4) NOT NULL,
    [QuantityFormula] nvarchar NOT NULL,
    [UnitPrice] decimal(18,4) NOT NULL,
    [TotalAmount] decimal(18,4) NOT NULL,
    [OrderTemplateId] int NOT NULL,
    [PriceFormula] nvarchar NOT NULL,
    [ProductEenheidId] int NOT NULL,
    CONSTRAINT [PK_Products_OrderTemplateDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ServiceRates]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Rate] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Products_ServiceRates] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[PriceListCategories]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [SortOrder] int NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [HasOptions] bit,
    [Color] nvarchar(50),
    [NameFr] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Products_PriceListCategories] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[PriceListTexts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [BaseCompanyId] int NOT NULL,
    [Text] nvarchar NOT NULL,
    [TextFr] nvarchar NOT NULL,
    [TextEn] nvarchar NOT NULL,
    CONSTRAINT [PK_Products_PriceListTexts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[Product]
(
    [ProductId] int IDENTITY(1,1) NOT NULL,
    [NameNl] nvarchar(150) NOT NULL,
    [DescriptionNl] nvarchar NOT NULL,
    [OrderPartNumber] nvarchar(150) NOT NULL,
    [StockNumber] nvarchar(50) NOT NULL,
    [SupplierId] int NOT NULL,
    [ManufacturerId] int NOT NULL,
    [ProductTypeId] int,
    [IsInactive] bit NOT NULL,
    [UnitsPerSale] decimal(18,4) NOT NULL,
    [UnitsPerPurchase] decimal(18,4) NOT NULL,
    [PriceListSortOrder] int,
    [ShowOnPriceList] bit NOT NULL,
    [ShortNotesNl] nvarchar(500),
    [ProdToonOmschrijvingPrijslijst] bit NOT NULL,
    [RecupelAmount] decimal(18,4),
    [BebatAmount] decimal(18,4),
    [ProdRalKleur] nvarchar(10),
    [ProdKleurPoedercode] nvarchar(50),
    [MinimumQuantity] decimal(18,2) NOT NULL,
    [CustomWorkPercentage] decimal(18,4) NOT NULL,
    [RecupelProductId] int,
    [BebatProductId] int,
    [IsQuickLooseSaleOption] bit,
    [NameFr] nvarchar(150) NOT NULL,
    [DescriptionFr] nvarchar NOT NULL,
    [ShortNotesFr] nvarchar(500) NOT NULL,
    [TaskTemplateId] int,
    [PurchaseUnitId] int,
    [SalesUnitId] int,
    [AdsolutId] int,
    [NameEn] nvarchar(150) NOT NULL,
    [DescriptionEn] nvarchar NOT NULL,
    [ShortNotesEn] nvarchar(500) NOT NULL,
    [IsCompositeProduct] bit,
    [ProductStructureId] int,
    [TemporaryDiscount] decimal(18,4),
    [TemporaryNetPurchasePrice] decimal(18,2),
    [ReportingGroupId] int,
    [SalesStockTriggerQuantity] decimal(18,3) NOT NULL,
    [ExtraPrice] decimal(18,4),
    [ExtraAssemblyPrice] decimal(18,2),
    [ExtraInstallationPrice] decimal(18,2),
    [IsProducedCompositeProduct] bit,
    [IsVerified] bit,
    [HasNoPrice] bit NOT NULL,
    [ShowOnQuote] bit NOT NULL,
    [ShowOnOrderConfirmation] bit NOT NULL,
    [ShowOnInvoice] bit NOT NULL,
    [ShowOnPackingSlip] bit NOT NULL,
    [ShowOnDeliveryNote] bit NOT NULL,
    [ShowOnProductionOrder] bit NOT NULL,
    [ShowOnPaintShopOrder] bit NOT NULL,
    [ShowOnInstallationOrder] bit NOT NULL,
    [Weight] decimal(18,3) NOT NULL,
    [InternalDocumentNotes] nvarchar(1000),
    [ExternalInstallerCost] bit NOT NULL,
    [ReportRecupel] bit NOT NULL,
    [ReportBebat] bit NOT NULL,
    [HasTierPricing] bit,
    [HideDetailPrice] bit,
    [ShowOnWebShopABMATIC] bit,
    [LastModifiedAt] datetime,
    [LastModifiedBy] nvarchar(50),
    [IsNew] bit,
    [EanCode] nvarchar(50),
    [PopupMessage] nvarchar(512),
    [WebShopABMATICDescriptionNl] nvarchar NOT NULL,
    [GoodsCode] nvarchar(50),
    [IntrastatCodeId] int,
    CONSTRAINT [PK_Products_Product] PRIMARY KEY CLUSTERED ([ProductId])
);
GO

CREATE TABLE [Products].[ProductPurchaseDiscounts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Percentage] decimal(18,4) NOT NULL,
    [SupplierId] int NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [GrossAmount] decimal(18,4) NOT NULL,
    [Pro1] decimal(18,4) NOT NULL,
    [Pro2] decimal(18,4) NOT NULL,
    [Pro3] decimal(18,4) NOT NULL,
    [Aan1] decimal(18,4) NOT NULL,
    [Aan2] decimal(18,4) NOT NULL,
    [Par1] decimal(18,4) NOT NULL,
    [ValidTo] datetime,
    [FromAddress] datetime NOT NULL,
    [Ond] decimal(18,4) NOT NULL,
    CONSTRAINT [PK_Products_ProductPurchaseDiscounts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductUnits]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameEn] nvarchar(150) NOT NULL,
    [NameFr] nvarchar(150) NOT NULL,
    [NameNl] nvarchar(150) NOT NULL,
    [UnitParameter] bit NOT NULL,
    CONSTRAINT [PK_Products_ProductUnits] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductManuals]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Path] nvarchar(1000) NOT NULL,
    [ProductId] int NOT NULL,
    [ShowOnWeb] bit,
    [SendAutomatically] bit,
    [Extension] nvarchar(25) NOT NULL,
    CONSTRAINT [PK_Products_ProductManuals] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductOptions]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [ValueType] nvarchar(50) NOT NULL,
    [IsRequired] bit NOT NULL,
    [ProductId] int NOT NULL,
    [ProductionNotesFlag] bit NOT NULL,
    [QuoteNotesFlag] bit NOT NULL,
    [CalculatePrice] bit NOT NULL,
    [SortOrder] int NOT NULL,
    [IsQuantityLine] bit,
    [NameFr] nvarchar(250) NOT NULL,
    [DefaultValueFormula] nvarchar(4000),
    [Tag] nvarchar(250) NOT NULL,
    [QuantityFormula] nvarchar(4000),
    [NameEn] nvarchar(250) NOT NULL,
    [ExtraPriceFormula] nvarchar(4000),
    [UnitParameterFormula] nvarchar(4000),
    CONSTRAINT [PK_Products_ProductOptions] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductOptionValue]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OptieProduct] int,
    [Value] nvarchar(100),
    [ProductOptionId] int NOT NULL,
    [SortOrder] int NOT NULL,
    [ValueFr] nvarchar(100),
    [ValueEn] nvarchar(100),
    CONSTRAINT [PK_Products_ProductOptionValue] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPopupReturnColumns]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameFr] nvarchar(50) NOT NULL,
    [NameNl] nvarchar(50) NOT NULL,
    [LooseSaleColumn] nvarchar(150) NOT NULL,
    [GateComponentColumn] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_Products_ProductPopupReturnColumns] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPopupTemplate]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [NameFr] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Products_ProductPopupTemplate] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPopupTemplateLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameFr] nvarchar(50) NOT NULL,
    [NameNl] nvarchar(50) NOT NULL,
    [TransferToQuantityFormula] nvarchar(250) NOT NULL,
    [TransferToQuantity] bit NOT NULL,
    [IncludePrice] bit NOT NULL,
    [WriteToLineColumn] int NOT NULL,
    [IsRequired] bit NOT NULL,
    [SortOrder] int NOT NULL,
    CONSTRAINT [PK_Products_ProductPopupTemplateLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPopupValueTypes]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [NameFr] nvarchar(50) NOT NULL,
    [Description] nvarchar(250) NOT NULL,
    [DescriptionFr] nvarchar(250) NOT NULL,
    [Tag] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Products_ProductPopupValueTypes] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPrices]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [FromAddress] date NOT NULL,
    [ValidTo] date,
    [AssemblyPrice] decimal(18,4) NOT NULL,
    [InstallationPrice] decimal(18,4) NOT NULL,
    [ProductId] int NOT NULL,
    [GrossSalesPrice] decimal(18,2) NOT NULL,
    [GrossPurchasePrice] decimal(18,2) NOT NULL,
    [NetPurchasePrice] decimal(18,2) NOT NULL,
    [ProductAankoopKortingenId] int,
    [OverrideBruto] bit,
    [BasePrice] decimal(18,4) NOT NULL,
    [CorrectedGrossPrice] decimal(18,2) NOT NULL,
    [PriceCalculationFormula] nvarchar(2024),
    [UsePriceCalculationFormula] bit,
    [BasePriceCalculationFormula] nvarchar(2024),
    [StartupCost] decimal(18,4) NOT NULL,
    [Pro1] decimal(18,4) NOT NULL,
    [Pro2] decimal(18,4) NOT NULL,
    [Pro3] decimal(18,4) NOT NULL,
    [Aan1] decimal(18,4) NOT NULL,
    [Aan2] decimal(18,4) NOT NULL,
    [Par1] decimal(18,4) NOT NULL,
    [Ond] decimal(18,4) NOT NULL,
    [ExtraPurchaseCost] decimal(18,2),
    [ExtraPurchaseCostNotes] nvarchar(500),
    [PurchaseDiscountPercentage] decimal(18,4) NOT NULL,
    [GrossCorrectionPercentage] decimal(18,4) NOT NULL,
    [CalculationType] int NOT NULL,
    [SupplierUsesDifferentGrossSalesPrice] bit NOT NULL,
    CONSTRAINT [PK_Products_ProductPrices] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPriceSalesDiscounts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantTypeId] int NOT NULL,
    [ProductPrijzenId] int NOT NULL,
    [Discount] decimal(18,4) NOT NULL,
    CONSTRAINT [PK_Products_ProductPriceSalesDiscounts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductProductionGroup]
(
    [ProductProductionGroupId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [SortOrder] int NOT NULL,
    [Color] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Products_ProductProductionGroup] PRIMARY KEY CLUSTERED ([ProductProductionGroupId])
);
GO

CREATE TABLE [Products].[ProductProductionGroupLinks]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int NOT NULL,
    [ProductionGroupId] int NOT NULL,
    CONSTRAINT [PK_Products_ProductProductionGroupLinks] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPropertyItems]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductPropertyId] int NOT NULL,
    [Value] nvarchar(250) NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_Products_ProductPropertyItems] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductProperty]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameEn] nvarchar(250) NOT NULL,
    [NameFr] nvarchar(250) NOT NULL,
    [NameNl] nvarchar(250) NOT NULL,
    [SortOrder] int NOT NULL,
    CONSTRAINT [PK_Products_ProductProperty] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductQuantityTiers]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [MinimumQuantity] decimal(18,2) NOT NULL,
    [Discount] decimal(18,4) NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_Products_ProductQuantityTiers] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductStockLocations]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [StockLocationId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] decimal(18,4) NOT NULL,
    [IsInactive] bit,
    [MaxQuantity] decimal(18,4) NOT NULL,
    [IsDefault] bit NOT NULL,
    [MinQuantity] decimal(18,4) NOT NULL,
    [ReservedQuantity] decimal(18,4) NOT NULL,
    [LastCountedAt] datetime,
    [CountedQuantity] decimal(18,4),
    CONSTRAINT [PK_Products_ProductStockLocations] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductStructures]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Level] int NOT NULL,
    [ParentTaskId] int,
    [NameNl] nvarchar(250) NOT NULL,
    [NameEn] nvarchar(250) NOT NULL,
    [NameFr] nvarchar(250) NOT NULL,
    [IntroPriceListTextId] int,
    [OutroPriceListTextId] int,
    [SortOrder] int NOT NULL,
    [Color] int,
    [ShowOnPriceList] bit,
    [Icon] varbinary,
    [PageBreakAfter] bit NOT NULL,
    CONSTRAINT [PK_Products_ProductStructures] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[WebShopABMATICProductStructures]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameEn] nvarchar(250) NOT NULL,
    [NameFr] nvarchar(250) NOT NULL,
    [NameNl] nvarchar(250) NOT NULL,
    [ParentTaskId] int,
    CONSTRAINT [PK_Products_WebShopABMATICProductStructures] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductSubProduct]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [MasterProductId] int NOT NULL,
    [SubProductId] int NOT NULL,
    [Quantity] decimal(18,4) NOT NULL,
    [IsOptional] bit NOT NULL,
    [ExtraBasePrice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Products_ProductSubProduct] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [NameFr] nvarchar(150) NOT NULL,
    [IsStockItem] bit NOT NULL,
    [IsProduction] bit NOT NULL,
    [IsPurchaseItem] bit NOT NULL,
    CONSTRAINT [PK_Products_ProductType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ReportingGroups]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [SortOrder] int NOT NULL,
    CONSTRAINT [PK_Products_ReportingGroups] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockMovements]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int NOT NULL,
    [OrderLineId] int,
    [Quantity] decimal(18,4) NOT NULL,
    [Timestamp] datetime NOT NULL,
    [Notes] nvarchar(150),
    [IsReservation] bit,
    [ProductStockLocatieId] int NOT NULL,
    CONSTRAINT [PK_Products_StockMovements] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockLocations]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [IsWarehouse] bit,
    CONSTRAINT [PK_Products_StockLocations] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockOrder]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierId] int NOT NULL,
    [CreatedAt] datetime NOT NULL,
    [OrderDate] datetime,
    [OrderConfirmationDate] datetime,
    [DeliveryDate] datetime,
    [InvoiceDate] datetime,
    [IsCompleted] bit NOT NULL,
    [Notes] nvarchar,
    [UserId] int NOT NULL,
    [ExpectedDeliveryDate] datetime,
    [InternalNotes] nvarchar,
    [TotalAmount] decimal(18,2),
    CONSTRAINT [PK_Products_StockOrder] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockOrderLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [StockOrderId] int NOT NULL,
    [ProductId] int,
    [QuantityOrdered] decimal(18,2) NOT NULL,
    [LijnOK] bit NOT NULL,
    [ProductName] nvarchar(500) NOT NULL,
    [OrderNumber] nvarchar(150) NOT NULL,
    [PackSize] nvarchar(50) NOT NULL,
    [PurchaseUnitPrice] decimal(18,4) NOT NULL,
    [PurchaseTotalPrice] decimal(18,4) NOT NULL,
    [Unit] nvarchar(50) NOT NULL,
    [InternalReference] nvarchar(150),
    [DeliveryNotes] nvarchar(2000),
    [OrderNotes] nvarchar(2000),
    [Besteld] bit,
    [OrderedAt] datetime,
    [Geleverd] bit,
    [DeliveredAt] datetime,
    [ProductTypeId] int,
    [QuantityDelivered] decimal(18,2) NOT NULL,
    [QuantityProcessedToStock] decimal(18,2),
    CONSTRAINT [PK_Products_StockOrderLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockOrderDeliveries]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [StockOrderDetail] int NOT NULL,
    [DeliveryDocumentNumber] nvarchar(100) NOT NULL,
    [Date] datetime NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [QuantityInvoiced] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Products_StockOrderDeliveries] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[WebShopABMATICStructures]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameNl] nvarchar(50) NOT NULL,
    [ParentTaskId] int,
    [SortOrder] int NOT NULL,
    CONSTRAINT [PK_Products_WebShopABMATICStructures] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[Orders]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsAccepted] bit NOT NULL,
    [CreatedAt] datetime NOT NULL,
    [CreatedByUserId] int NOT NULL,
    [ProjectId] int NOT NULL,
    [GeneralDiscount] decimal(18,4) NOT NULL,
    [DeliveryTypeId] int NOT NULL,
    [PriceListTypeId] int NOT NULL,
    [QuoteId] int,
    [OrderNumber] int,
    [CommissionAmount] decimal(18,4) NOT NULL,
    [InstallerContactId] int,
    [RequestedDeliveryDate] datetime,
    [InstallationDays] int,
    [ProductionDays] int,
    [MasterOrderId] int,
    [VatTypeId] int NOT NULL,
    [OrderProcessingTypeId] int NOT NULL,
    [CustomerTypeId] int NOT NULL,
    [InternalNotes] nvarchar,
    [RalColor] nvarchar(150),
    [AdvanceInvoiceEnabled] bit,
    [ExtraDiscount] decimal(18,4),
    [CustomerNotes] nvarchar,
    [InstallerNotes] nvarchar,
    [InternalStaffNotes] nvarchar,
    [AllowPartialDelivery] bit NOT NULL,
    [CommissionSalesUserId] int,
    [IsCommissionInvoiced] bit,
    [CommissionToInvoice] decimal(18,2),
    [LeveradresId] int,
    [BetaaltermijnId] int NOT NULL,
    [PopupMessage] nvarchar,
    [QuoteValidDays] int NOT NULL,
    [IsUrgent] bit NOT NULL,
    [PriceListDate] datetime,
    [BaseCompanyVatNumberId] int NOT NULL,
    [AdvancePaymentsByAmount] bit,
    [HasCloudFolder] bit,
    [IsClosingVerified] bit,
    [InvoiceNotes] nvarchar,
    [QuoteNotesHeader] nvarchar,
    CONSTRAINT [PK_Projects_Orders] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] int NOT NULL,
    [GateId] int,
    [ProductId] int,
    [Quantity] decimal(18,4) NOT NULL,
    [UnitPrice] decimal(18,4) NOT NULL,
    [AssemblyPrice] decimal(18,4) NOT NULL,
    [InstallationPrice] decimal(18,4) NOT NULL,
    [TotalExclVat] decimal(18,4) NOT NULL,
    [TotalInclVat] decimal(18,4) NOT NULL,
    [IsOption] bit NOT NULL,
    [Discount] decimal(18,4) NOT NULL,
    [Btw] decimal(18,4) NOT NULL,
    [UpliftType] nvarchar(50) NOT NULL,
    [DocumentDisplayName] nvarchar(500) NOT NULL,
    [Lengte] decimal(18,4) NOT NULL,
    [Hoogte] decimal(18,4) NOT NULL,
    [Breedte] decimal(18,4) NOT NULL,
    [Assemblage] bit NOT NULL,
    [Montage] bit NOT NULL,
    [QuoteNotesHeader] nvarchar NOT NULL,
    [ProductionNotes] nvarchar,
    [MontageStukprijs] decimal(18,4) NOT NULL,
    [AssemblageStukprijs] decimal(18,4) NOT NULL,
    [ProductOptionId] int,
    [ProductOptionHoofdDetaillId] int,
    [SerialNumber] nvarchar(150),
    [ProductionGroupId] nvarchar(50) NOT NULL,
    [AantalProductieVerwerkt] decimal(18,4),
    [AantalAfgehaaldOfGeleverd] decimal(18,4),
    [QuantityInvoiced] decimal(18,4),
    [Afgehandeld] bit,
    [PurchaseOrderNotes] nvarchar NOT NULL,
    [PopUpDataXml] xml,
    [ProductType] int NOT NULL,
    [InProductie] bit,
    [Controleren] bit,
    [ProductieKlaar] bit,
    [LeveringOfAfhalingKlaar] bit,
    [MontageKlaar] bit,
    [Gefactureerd] bit,
    [Tefactureren] bit,
    [ProductieVerwerkOp] datetime,
    [ReadyForBillingDate] datetime,
    [BtwBedrag] decimal(18,4) NOT NULL,
    [SortOrder] int NOT NULL,
    [Tecrediteren] bit,
    [GecrediteerdOp] datetime,
    [ToOrder] bit,
    [Ordered] bit,
    [OrderedDate] datetime,
    [DocumentDisplayNameFr] nvarchar(500) NOT NULL,
    [NettoCommisieEenheidsPrijs] decimal(18,4),
    [IsSubProduct] bit,
    [AantalSubProduct] decimal(18,4) NOT NULL,
    [BestelNummer] nvarchar(100),
    [SupplierId] int,
    [PurchasePrice] decimal(18,2),
    [ProductEenheidId] int,
    [InvoicedAt] datetime,
    [PrijsControleren] bit,
    [AantalTeBestellen] decimal(18,4) NOT NULL,
    [ProductStockLocatieId] int,
    [AantalInStockGereserveerd] decimal(18,4),
    [AantalDefinitiefUitStock] decimal(18,4),
    [NoPriceCacl] bit,
    [AantalBeschikbaarInStock] decimal(18,2),
    [IsSamenGesteldProduct] bit,
    [ReportingGroupId] int NOT NULL,
    [BrutoAankoopprijs] decimal(18,4) NOT NULL,
    [NettoAankoopPrijs] decimal(18,4) NOT NULL,
    [WinstPercentage] decimal(18,4) NOT NULL,
    [StockOrderDetailId] int,
    [QuantityOrdered] decimal(18,4),
    [OrderedAt] datetime,
    [ArticleNumber] nvarchar(250),
    [OrigineleKorting] decimal(18,4) NOT NULL,
    [OverRuleLosseLijnAutoKorting] bit,
    [IsTextOnly] bit,
    [UpliftTypeOrigineel] nvarchar(50) NOT NULL,
    [BasePrice] decimal(18,2) NOT NULL,
    [BebatNaam] nvarchar(250),
    [BebatProductId] int,
    [RecupelNaam] nvarchar(500),
    [RecupelProductId] int,
    [BebatAantal] decimal(10,2) NOT NULL,
    [BebatStukPrijs] decimal(18,2) NOT NULL,
    [BebatTotaal] decimal(18,2) NOT NULL,
    [RecupelAantal] decimal(10,2) NOT NULL,
    [RecupelStukPrijs] decimal(18,2) NOT NULL,
    [RecupelTotaal] decimal(18,2) NOT NULL,
    [IsProducedCompositeProduct] bit,
    [IsLeveringsTypeProduct] bit,
    [StandaardKortingsType] nvarchar(50),
    [StandaardKortingsPecentage] decimal(18,2),
    [IsGarantie] bit NOT NULL,
    [TotaalExclVoorCommissie] decimal(18,2) NOT NULL,
    [PopUpRow] bit NOT NULL,
    [NodeLevel] int,
    [AfgehandeldOp] datetime,
    [Selected] bit NOT NULL,
    [IsExtraKortingRow] bit,
    [BestellingIsBinnenGekomen] bit,
    [UnitParameter] decimal(18,4) NOT NULL,
    [BasisPrijsTellen] bit NOT NULL,
    [ShowOnQuote] bit NOT NULL,
    [ShowOnOrderConfirmation] bit NOT NULL,
    [ShowOnInvoice] bit NOT NULL,
    [ShowOnPackingSlip] bit NOT NULL,
    [ShowOnDeliveryNote] bit NOT NULL,
    [ShowOnProductionOrder] bit NOT NULL,
    [ToonOpLakBon] bit NOT NULL,
    [ShowOnInstallationOrder] bit NOT NULL,
    [Ral] nvarchar(50),
    [ToonOmschrijvingOpFactuur] bit NOT NULL,
    [ToonOmschrijvingOpPakbon] bit NOT NULL,
    [ToonOmschrijvingOpLeverbon] bit NOT NULL,
    [ToonOmschrijvingOpProductiebon] bit NOT NULL,
    [ToonOmschrijvingOpLakBon] bit NOT NULL,
    [ToonOmschrijvingOpOfferte] bit NOT NULL,
    [ToonOpVrachtbrief] bit NOT NULL,
    [ToonOmschrijvingOpVrachtbrief] bit NOT NULL,
    [ToonOmschrijvingOpOrderbevestiging] bit NOT NULL,
    [StartupCost] decimal(18,2) NOT NULL,
    [OpstartKostTotaal] decimal(18,2) NOT NULL,
    [BasisPrijsTotaal] decimal(18,2) NOT NULL,
    [VatTypeId] int NOT NULL,
    [OrderTemplateId] int,
    [OrderTemplateDetailId] int,
    [PieceUnitPrice] decimal(18,2) NOT NULL,
    [CustomerCustomProductId] int,
    [TaxproductenGecontroleerd] bit,
    [ReportRecupel] bit NOT NULL,
    [ReportBebat] bit NOT NULL,
    [ToonGeenDetailPrijzen] bit,
    [OfferteDocumentDocId] int,
    [OrderBevestigingDocumentDocId] int,
    [PakBonDocumentDocId] int,
    [LeveringsBonDocId] int,
    [FactuurDocId] int,
    [IsProduction] bit,
    [OmschrijvingProbleem] nvarchar,
    [KlantBinnenGebrachtOp] datetime,
    [KlantTerugOpgehaaldOp] datetime,
    [VanKlantInOntvangstGenomenDoor] nvarchar(50),
    [OmschrijvingHerstelling] nvarchar,
    [SentToSupplierAt] datetime,
    [OntvangenVanLeverancierOp] datetime,
    [TrackingHerstelling] nvarchar(500),
    [IsHerstelling] bit,
    [NieuwVervangToestelGegevenOp] datetime,
    [HerstelUren] decimal(18,2),
    [HerstellingAfgerondOp] datetime,
    [HerstellingsKostMateriaal] decimal(18,2),
    [HerstellingUurtarief] decimal(18,2),
    [HerstellingsKostTotaal] decimal(18,2),
    [KortingOverride] bit,
    [PrijsTypeOverride] bit,
    [InterneHerstelling] bit,
    [AdvancePaymentVisibility] nvarchar(50) NOT NULL,
    [Goederen] decimal(18,2),
    [Diensten] decimal(18,2),
    [GoodsCode] nvarchar(50),
    [Weight] decimal(18,3),
    [AanvullendeEenheden] decimal(18),
    [LandVanOorsprong] int,
    [IntrastatReported] bit,
    [IntrastatReportedOn] datetime,
    CONSTRAINT [PK_Projects_OrderLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderStatuses]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [SortOrder] int,
    [IncludeInSalesReporting] bit,
    [NameFr] nvarchar(50) NOT NULL,
    [ReportInProgress] bit,
    [ScreenMode] nvarchar(50) NOT NULL,
    [OrderStatusGroupId] int,
    [ReserveStock] bit NOT NULL,
    [AffectsStock] bit NOT NULL,
    CONSTRAINT [PK_Projects_OrderStatuses] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderStatusAccesses]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderStatusId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Projects_OrderStatusAccesses] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderTypes]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [SerialNumberSuffix] nvarchar(50) NOT NULL,
    [OrderProcessingTypeId] int NOT NULL,
    [SortOrder] int NOT NULL,
    [NameFr] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Projects_OrderTypes] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[CustomerDeliveredProducts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] int NOT NULL,
    [Name] nvarchar(1024) NOT NULL,
    [Explanation] nvarchar NOT NULL,
    [ReceivedAt] datetime NOT NULL,
    [ReceivedByUserId] int NOT NULL,
    [BroughtBy] nvarchar(150) NOT NULL,
    [PickedUpAt] datetime,
    [PickedUpBy] nvarchar(150),
    [SentToSupplierAt] datetime,
    [TrackingNumber] nvarchar(150),
    [ReturnedFromSupplierAt] datetime,
    [IrreparableAt] datetime,
    CONSTRAINT [PK_Projects_CustomerDeliveredProducts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderLineTexts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar NOT NULL,
    [OrderBevestiging] bit NOT NULL,
    [Leveringsbon] bit NOT NULL,
    [Factuur] bit NOT NULL,
    [AkOrder] bit NOT NULL,
    [Offerte] bit NOT NULL,
    [DossierDetailsId] int,
    [MontageBon] bit NOT NULL,
    [Lakbon] bit NOT NULL,
    [ProductieBon] bit NOT NULL,
    CONSTRAINT [PK_Projects_OrderLineTexts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderDevelopmentLines]
(
    [Id] uniqueidentifier NOT NULL,
    [ArticleNumber] nvarchar(250) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Description] nvarchar,
    [ProductId] int,
    [MasterRowId] uniqueidentifier,
    [Quantity] decimal(18,3),
    [ProductEenheidId] int,
    [ReservedQuantity] decimal(18,4) NOT NULL,
    [QuantityTakenFromStock] decimal(18,4) NOT NULL,
    [MustOrder] bit NOT NULL,
    [OrderedAt] datetime,
    [DeliveredAt] datetime,
    [SupplierId] int,
    [PurchasePrice] decimal(18,2) NOT NULL,
    [TotalPurchasePrice] decimal(18) NOT NULL,
    [DrawingCreated] bit NOT NULL,
    [MaterialType] nvarchar(150),
    [Processing] nvarchar(150),
    [OrderId] int NOT NULL,
    [SortOrder] int NOT NULL,
    [Weight] decimal(18,2),
    [Finish] nvarchar,
    [StockOrderDetailId] int,
    [RequiresPainting] bit,
    [PaintWorkSupplierId] int,
    CONSTRAINT [PK_Projects_OrderDevelopmentLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderFeedbacks]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] int NOT NULL,
    [Bucket] nvarchar(150) NOT NULL,
    [Date] datetime NOT NULL,
    [UserId] int NOT NULL,
    [Text] nvarchar,
    CONSTRAINT [PK_Projects_OrderFeedbacks] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderInstallationLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ItemNumber] nvarchar(50) NOT NULL,
    [PartNumber] nvarchar(250) NOT NULL,
    [Description] nvarchar(500),
    [Material] nvarchar(150),
    [Quantity] decimal(18,4) NOT NULL,
    [QuantityInInstallation] decimal(18,4),
    [Processing] nvarchar(150),
    [SupplierId] int,
    [SupplierArticleNumber] nvarchar(150),
    [Treatment] nvarchar(50),
    [OrderedAt] datetime,
    [DeliveredAt] datetime,
    [UnitPrice] decimal(18,4),
    [TotalPrice] decimal(18,4),
    [Notes] nvarchar(1000),
    [OrderId] int NOT NULL,
    CONSTRAINT [PK_Projects_OrderInstallationLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderDeliveryTypeProducts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [LeveringTypeId] int NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_Projects_OrderDeliveryTypeProducts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderLogs]
(
    [Id] bigint NOT NULL,
    [OrderId] int NOT NULL,
    [UserId] int NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_Projects_OrderLogs] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderRemarks]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] int NOT NULL,
    [CustomerNoteId] int,
    [ProductionCategories] nvarchar(150),
    [Notes] nvarchar NOT NULL,
    [ProductionCategoryId] int,
    [DocumentTypeId] int,
    CONSTRAINT [PK_Projects_OrderRemarks] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderProjectLines]
(
    [Id] uniqueidentifier NOT NULL,
    [ArticleNumber] nvarchar(250) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Description] nvarchar NOT NULL,
    [ProductId] int,
    [MasterRowId] uniqueidentifier,
    [Quantity] decimal(18,3) NOT NULL,
    [ProductEenheidId] int NOT NULL,
    [ReservedQuantity] decimal(18,4) NOT NULL,
    [QuantityTakenFromStock] decimal(18,4) NOT NULL,
    [MustOrder] bit NOT NULL,
    [OrderedAt] datetime NOT NULL,
    [DeliveredAt] datetime NOT NULL,
    [SupplierId] int,
    [PurchasePrice] decimal(18,2) NOT NULL,
    [TotalPurchasePrice] decimal(18) NOT NULL,
    [DrawingCreated] bit NOT NULL,
    CONSTRAINT [PK_Projects_OrderProjectLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderStatusGroups]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Projects_OrderStatusGroups] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderStructures]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Number] nvarchar(1000) NOT NULL,
    [MasterRowId] int,
    [OrderTypeId] int,
    [OrderId] int,
    [SortOrder] int NOT NULL,
    [ProjectId] int NOT NULL,
    [CreatedAt] datetime NOT NULL,
    [CreatedByUserId] int NOT NULL,
    [StatusId] int NOT NULL,
    [CustomerReference] nvarchar(1000) NOT NULL,
    [OrderNumber] nvarchar(50),
    [Path] nvarchar(1024),
    [OrderConfirmationDate] datetime,
    [PlannedDate] datetime,
    [ReadyForDeliveryDate] datetime,
    [ReadyForBillingDate] datetime,
    [CompletedDate] datetime,
    [QuoteDate] datetime,
    [TotalAmount] decimal(18,2),
    [TotalInvoiced] decimal(18,2),
    [TeamsId] nvarchar(250),
    [OrderConfirmedBy] nvarchar(50),
    [PlannedBy] nvarchar(50),
    [ReadyForDeliveryBy] nvarchar(50),
    [CompletedBy] nvarchar(50),
    [QuoteBy] nvarchar(50),
    [ReadyForBillingBy] nvarchar(50),
    [HasPaperFile] bit NOT NULL,
    [QueuedAt] datetime,
    CONSTRAINT [PK_Projects_OrderStructures] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderProcessingTypes]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [CalculatePrice] bit NOT NULL,
    [OrderStatusId] int NOT NULL,
    CONSTRAINT [PK_Projects_OrderProcessingTypes] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[OrderAdvancePayments]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] int NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Percent] decimal(18,4) NOT NULL,
    [IsFinalInvoice] bit NOT NULL,
    [InvoicedAt] datetime,
    [SortOrder] int NOT NULL,
    [Amount] decimal(18,6),
    [AdvancePaymentVisibility] nvarchar(50),
    [MollieCheckoutUrl] nvarchar(500),
    [MolliePaidAt] datetime2,
    [MolliePaymentId] nvarchar(50),
    [MolliePaymentStatus] nvarchar(30),
    CONSTRAINT [PK_Projects_OrderAdvancePayments] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[BillingAgreements]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    [Percentage] decimal(18,4) NOT NULL,
    [CustomerName] nvarchar(250) NOT NULL,
    [VatPercentage] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Projects_BillingAgreements] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[JobCode]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(10) NOT NULL,
    [NameNl] nvarchar(150) NOT NULL,
    [NameFr] nvarchar(150) NOT NULL,
    [NameEn] nvarchar(150) NOT NULL,
    [HourlyRate] decimal(18,2) NOT NULL,
    [IsBillable] bit NOT NULL,
    CONSTRAINT [PK_Projects_JobCode] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[DeliveryTypes]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [IncludeInstallationCost] bit NOT NULL,
    [NameFr] nvarchar(50) NOT NULL,
    [IsDefault] bit,
    CONSTRAINT [PK_Projects_DeliveryTypes] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[MaintenanceContracts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500) NOT NULL,
    [FromAddress] datetime NOT NULL,
    [ValidTo] datetime,
    [Amount] decimal(18,2) NOT NULL,
    [AutoRenew] bit NOT NULL,
    [CancelledAt] datetime,
    [ReminderDate] datetime,
    [ProjectId] int NOT NULL,
    [CycleDays] int NOT NULL,
    [ReminderDaysInAdvance] int NOT NULL,
    [VatTypeId] int NOT NULL,
    [SuccessorUserId] int,
    CONSTRAINT [PK_Projects_MaintenanceContracts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[MaintenanceContractLines]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OnderhoudsContractId] int NOT NULL,
    [ProjectInstallatieId] int NOT NULL,
    CONSTRAINT [PK_Projects_MaintenanceContractLines] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[Project]
(
    [ProjectId] int IDENTITY(1,1) NOT NULL,
    [ProjectNumber] int NOT NULL,
    [ProjectName] nvarchar(250) NOT NULL,
    [ProjectManagerUserId] int NOT NULL,
    [CustomerId] int,
    [JobSiteId] int,
    [ProjectTypeId] int NOT NULL,
    [ProjectCreatedAt] datetime,
    [ProjectInternalNotes] nvarchar,
    [BaseCompanyId] int,
    [ProductionLabelReference] nvarchar(60),
    [IsTemplate] bit,
    [ProjectNotes] nvarchar,
    [IsStandardProject] bit NOT NULL,
    [TeamsId] nvarchar(250),
    [PopupMessage] nvarchar,
    CONSTRAINT [PK_Projects_Project] PRIMARY KEY CLUSTERED ([ProjectId])
);
GO

CREATE TABLE [Projects].[ProjectInstallations]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [InstallationOrderId] int,
    [Location] nvarchar(250),
    [Name] nvarchar(250),
    [ProjectId] int NOT NULL,
    [SerialNumber] nvarchar(250),
    [Specifications] nvarchar,
    CONSTRAINT [PK_Projects_ProjectInstallations] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[ProjectLog]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectId] int NOT NULL,
    [ActionCode] nvarchar(150) NOT NULL,
    [Date] datetime NOT NULL,
    CONSTRAINT [PK_Projects_ProjectLog] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[ProjectParties]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int NOT NULL,
    [ProjectPartijGroepId] int,
    [Comment] nvarchar(500),
    [BillingPercentage] decimal(18,4) NOT NULL,
    [ProjectId] int NOT NULL,
    CONSTRAINT [PK_Projects_ProjectParties] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[ProjectPartyContacts]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerContactId] int NOT NULL,
    [ProjectPartijId] int NOT NULL,
    [EmailQuote] bit NOT NULL,
    [EmailOrderConfirmation] bit NOT NULL,
    [EmailPlanning] bit NOT NULL,
    [EmailDeliveryReady] bit NOT NULL,
    [EmailDelivered] bit NOT NULL,
    [EmailBilling] bit NOT NULL,
    [Note] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_Projects_ProjectPartyContacts] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[ProjectPartyGroups]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameEn] nvarchar(50) NOT NULL,
    [NameFr] nvarchar(50) NOT NULL,
    [NameNl] nvarchar(50) NOT NULL,
    [IsForSupplier] bit NOT NULL,
    [IsForCustomer] bit NOT NULL,
    [IsForManufacturer] bit NOT NULL,
    CONSTRAINT [PK_Projects_ProjectPartyGroups] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[Timesheet]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] int,
    [StartDt] datetime NOT NULL,
    [StopDt] datetime,
    [DurationMinutes] decimal(18),
    [DurationHours] decimal(18,2),
    [JobCodeId] int NOT NULL,
    [IsBillable] bit NOT NULL,
    [BillableMinutes] decimal(18,2),
    [BillableHours] decimal(18,2),
    [PrestantUserId] int NOT NULL,
    [CreatedAt] datetime NOT NULL,
    [CreatedByUserId] int NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [IsTimerRunning] bit NOT NULL,
    [TimerStartedAt] datetime,
    [TimerLastValue] int,
    CONSTRAINT [PK_Projects_Timesheet] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projects].[JobSites]
(
    [SiteStreet] nvarchar(150) NOT NULL,
    [SiteBox] nvarchar(50) NOT NULL,
    [SiteHouseNumber] nvarchar(50) NOT NULL,
    [JobSiteId] int IDENTITY(1,1) NOT NULL,
    [SiteCityId] int NOT NULL,
    [EndCustomerEmail] nvarchar(150) NOT NULL,
    [EndCustomerMobile] nvarchar(50) NOT NULL,
    [EndCustomerName] nvarchar(250) NOT NULL,
    [EndCustomerPhone] nvarchar(50) NOT NULL,
    [SiteNotes] nvarchar(2000),
    CONSTRAINT [PK_Projects_JobSites] PRIMARY KEY CLUSTERED ([JobSiteId])
);
GO

CREATE TABLE [Tasks].[TaskDependencies]
(
    [DependentTaskId] int,
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentTaskId] int,
    [Type] int NOT NULL,
    CONSTRAINT [PK_Tasks_TaskDependencies] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Tasks].[TaskTemplates]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameFr] nvarchar(250) NOT NULL,
    [NameNl] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Tasks_TaskTemplates] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Tasks].[TaskTemplateDependencies]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentTaskId] int,
    [DependsOnTaskTemplateTaskId] int,
    [Type] int NOT NULL,
    CONSTRAINT [PK_Tasks_TaskTemplateDependencies] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Tasks].[TaskTemplateTasks]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [SortOrder] int NOT NULL,
    [DueWithinTicks] bigint NOT NULL,
    [TaakTypeId] int NOT NULL,
    [DefaultUserId] int,
    [UseProjectOwnerAsDefault] int,
    [DefaultUserGroupId] int,
    [LockUntilPreviousComplete] bit NOT NULL,
    [TaakTemplateId] int NOT NULL,
    [TaskName] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Tasks_TaskTemplateTasks] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Users].[SickLeaves]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [FromAddress] datetime NOT NULL,
    [ValidTo] datetime NOT NULL,
    [SickNoteAzureFileId] bigint,
    [UserId] int NOT NULL,
    [CalendarEntryId] int,
    CONSTRAINT [PK_Users_SickLeaves] PRIMARY KEY CLUSTERED ([Id])
);
GO


