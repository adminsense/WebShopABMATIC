-- ABMATIC local: banco + schemas + tabelas (do zero)
-- Servidor: MULLER (Windows: Muller\Muller)
-- sqlcmd -S MULLER -E -i ABMATIC-create-local.sql

USE [master];
GO

IF DB_ID(N'ABMATIC') IS NOT NULL
BEGIN
    DECLARE @kill NVARCHAR(MAX) = N'';
    SELECT @kill += N'KILL ' + CAST(session_id AS NVARCHAR(10)) + N';'
    FROM sys.dm_exec_sessions
    WHERE database_id = DB_ID(N'ABMATIC') AND session_id <> @@SPID;
    IF LEN(@kill) > 0 EXEC sp_executesql @kill;

    ALTER DATABASE [ABMATIC] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [ABMATIC];
END
GO

CREATE DATABASE [ABMATIC] COLLATE Latin1_General_CI_AS;
GO

USE [ABMATIC];
GO

CREATE SCHEMA [Bestanden] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Boekhouding] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Crm] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Emails] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Instellingen] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Klanten] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Logging] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Products] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Projecten] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Taken] AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Users] AUTHORIZATION [dbo];
GO

CREATE TABLE [Bestanden].[AzureFile]
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
    [DossierId] int,
    [ProjectProjectId] int,
    [EmailId] int,
    [KlantKlantId] int,
    [Deleted] bit,
    [DeletedByUserId] int,
    [ProductId] int,
    [IsPrimaryImage] bit,
    [PublishToWeb] bit,
    [UserId] int,
    [IsGeneral] bit,
    [SupplierSupplierId] int,
    [ManufacturerManufacturerId] int,
    [DossierDetailId] int,
    [IsLinkedRef] bit,
    [VerzendenNaarKlant] bit NOT NULL,
    [VerzendenBijBestellingLeverancier] bit NOT NULL,
    [StockOrderId] int,
    CONSTRAINT [PK_Bestanden_AzureFile] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Bestanden].[AzureFileFolder]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [IsForCrm] bit NOT NULL,
    [IsForDossier] bit NOT NULL,
    [IsForProject] bit NOT NULL,
    [IsForProduct] bit NOT NULL,
    [IsForUser] bit NOT NULL,
    [IsForGeneralUse] bit NOT NULL,
    [Volgorde] decimal(10,2) NOT NULL,
    CONSTRAINT [PK_Bestanden_AzureFileFolder] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Bestanden].[Bestand]
(
    [BestandId] int IDENTITY(1,1) NOT NULL,
    [BestandNaam] nvarchar(250) NOT NULL,
    [BestandType] int NOT NULL,
    [Created] datetime NOT NULL,
    [Updated] datetime,
    [AangemaaktDoor] int NOT NULL,
    [Data] varbinary(max) NOT NULL,
    CONSTRAINT [PK_Bestanden_Bestand] PRIMARY KEY CLUSTERED ([BestandId])
);
GO

CREATE TABLE [Bestanden].[DossierBestanden]
(
    [DosBestandBestandId] int NOT NULL,
    [DosBestandDossierId] int NOT NULL,
    [DosBestandId] int IDENTITY(1,1) NOT NULL,
    CONSTRAINT [PK_Bestanden_DossierBestanden] PRIMARY KEY CLUSTERED ([DosBestandId])
);
GO

CREATE TABLE [Boekhouding].[BtwType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(150) NOT NULL,
    [FactuurTekst] nvarchar(512) NOT NULL,
    [Percentage] decimal(18,2) NOT NULL,
    [FactuurTekstEn] nvarchar(512) NOT NULL,
    [FactuurTekstFr] nvarchar(512) NOT NULL,
    [ToelichtingNl] nvarchar(max) NOT NULL,
    [ToelichtingFr] nvarchar(max) NOT NULL,
    [ToelichtingEn] nvarchar(max) NOT NULL,
    [IsDefault] bit,
    [TaxExemptionReason] nvarchar(50),
    [TaxExemptionReasonCode] nvarchar(50),
    [PeppolCode] nvarchar(50),
    CONSTRAINT [PK_Boekhouding_BtwType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Boekhouding].[DocumentDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Groep] nvarchar(50) NOT NULL,
    [PoortId] int,
    [ProjectId] int,
    [ProductId] int,
    [ProductNaam] nvarchar(512) NOT NULL,
    [ProductOmschrijving] nvarchar(max) NOT NULL,
    [Aantal] decimal(18,4) NOT NULL,
    [EenheidsPrijs] decimal(18,4) NOT NULL,
    [Eenheid] nvarchar(50) NOT NULL,
    [KortingPercentage] decimal(18,4) NOT NULL,
    [Subtotaal] decimal(18,4) NOT NULL,
    [BtwPercentage] decimal(18,4) NOT NULL,
    [Totaal] decimal(18,4) NOT NULL,
    [DocumentId] int NOT NULL,
    [MontagePrijs] decimal(18,4) NOT NULL,
    [AssemblagePrijs] decimal(18,4) NOT NULL,
    [IsOptie] bit NOT NULL,
    [BtwBedrag] decimal(18,4) NOT NULL,
    [KortingBedrag] decimal(18,4) NOT NULL,
    [Volgorde] int NOT NULL,
    [PrijslijsType] nvarchar(20),
    [GroepNaam] nvarchar(500) NOT NULL,
    [IsOptieVanBestellingDetailId] int,
    [ProductType] int,
    [BestellingDetailId] int,
    [PoortOnderdeelId] int,
    [LeveringAfhalingOkOp] date,
    [NettoCommisieEenheidsPrijs] decimal(18,4),
    [DossierId] int NOT NULL,
    [BestelNummer] nvarchar(150),
    [BasisPrijs] decimal(18,2) NOT NULL,
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
    [GeproduceerdSamenGesteldproduct] bit,
    [IsVoorschot] bit,
    [IsTextOnly] bit,
    [KlaarVoorVerzendingOp] datetime,
    [GeleverdOp] datetime,
    [NietTellenWegensVoorschot] bit NOT NULL,
    [IsGarantie] bit NOT NULL,
    [IsPopUpRow] bit NOT NULL,
    [Opmerking] nvarchar(max),
    [BasisPrijsTotaal] decimal(18,2) NOT NULL,
    [Opstartkost] decimal(18,4) NOT NULL,
    [OpstartKostTotaal] decimal(18,2) NOT NULL,
    [BtwTypeId] int NOT NULL,
    [SupplierSupplierId] int,
    [DocumentDetailMasterId] int NOT NULL,
    [DetailVanMasterId] int,
    [AankoopStukPrijs] decimal(18,2),
    [Goederen] decimal(18,2),
    [Diensten] decimal(18,2),
    [GoederenCode] nvarchar(50),
    [Gewicht] decimal(18,3),
    [AanvullendeEenheden] decimal(18),
    [LandVanOorsprong] int,
    CONSTRAINT [PK_Boekhouding_DocumentDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Boekhouding].[DocumentDetailold]
(
    [Id] int NOT NULL,
    [Groep] nvarchar(50) NOT NULL,
    [PoortId] int,
    [ProjectId] int,
    [ProductId] int,
    [ProductNaam] nvarchar(512) NOT NULL,
    [ProductOmschrijving] nvarchar(max) NOT NULL,
    [Aantal] decimal(18,4) NOT NULL,
    [EenheidsPrijs] decimal(18,4) NOT NULL,
    [Eenheid] nvarchar(50) NOT NULL,
    [KortingPercentage] decimal(18,4) NOT NULL,
    [Subtotaal] decimal(18,4) NOT NULL,
    [BtwPercentage] decimal(18,4) NOT NULL,
    [Totaal] decimal(18,4) NOT NULL,
    [DocumentId] int NOT NULL,
    [MontagePrijs] decimal(18,4) NOT NULL,
    [AssemblagePrijs] decimal(18,4) NOT NULL,
    [IsOptie] bit NOT NULL,
    [BtwBedrag] decimal(18,4) NOT NULL,
    [KortingBedrag] decimal(18,4) NOT NULL,
    [Volgorde] int NOT NULL,
    [PrijslijsType] nvarchar(20),
    [GroepNaam] nvarchar(150) NOT NULL,
    [IsOptieVanBestellingDetailId] int,
    [ProductType] int,
    [BestellingDetailId] int,
    [PoortOnderdeelId] int,
    [LeveringAfhalingOkOp] date,
    [NettoCommisieEenheidsPrijs] decimal(18,4),
    [DossierId] int NOT NULL,
    [BestelNummer] nvarchar(150),
    [BasisPrijs] decimal(18,2) NOT NULL,
    [KortingType] nvarchar(2) NOT NULL,
    [BebatProductId] int,
    [RecupelProductId] int,
    [BebatNaam] nvarchar(150),
    [RecupelNaam] nvarchar(150),
    [BebatStukPrijs] decimal(18,2) NOT NULL,
    [BebatAantal] decimal(18,2) NOT NULL,
    [BebatTotaal] decimal(18,2) NOT NULL,
    [RecupelStukPrijs] decimal(18,2) NOT NULL,
    [RecupelAantal] decimal(18,2) NOT NULL,
    [RecupelTotaal] decimal(18,2) NOT NULL,
    [MontageStukPrijs] decimal(18,2) NOT NULL,
    [AssemblageStukPrijs] decimal(18,2) NOT NULL,
    [GeproduceerdSamenGesteldproduct] bit,
    [IsVoorschot] bit,
    [IsTextOnly] bit,
    [KlaarVoorVerzendingOp] datetime,
    [GeleverdOp] datetime,
    [NietTellenWegensVoorschot] bit NOT NULL,
    [IsGarantie] bit NOT NULL,
    [IsPopUpRow] bit NOT NULL,
    [Opmerking] nvarchar(max)
);
GO

CREATE TABLE [Boekhouding].[Documenten]
(
    [DocAanmaakDatum] datetime NOT NULL,
    [DocBedragBtw] decimal(18,2) NOT NULL,
    [DocBedragNetto] decimal(18,2) NOT NULL,
    [DocBedragTotaal] decimal(18,2) NOT NULL,
    [DocDatum] datetime NOT NULL,
    [DocDefinitef] bit,
    [DocId] int IDENTITY(1,1) NOT NULL,
    [DocKlantBus] nvarchar(50) NOT NULL,
    [DocKlantId] int NOT NULL,
    [DocKlantNaam] nvarchar(150) NOT NULL,
    [DocKlantNr] nvarchar(50) NOT NULL,
    [DocKlantPostcode] nvarchar(50) NOT NULL,
    [DocKlantStraat] nvarchar(150) NOT NULL,
    [DocKlantWoonplaats] nvarchar(150) NOT NULL,
    [DocNummer] nvarchar(50),
    [DocType] int NOT NULL,
    [AangemaaktDoor] int NOT NULL,
    [DocBestellingId] int,
    [DocKlantBedrijfsnaam] nvarchar(150) NOT NULL,
    [DocKlantBtwnr] nvarchar(50) NOT NULL,
    [DocKlantLand] nvarchar(50) NOT NULL,
    [DocKlantHuisNr] nvarchar(50) NOT NULL,
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
    [Leveringsdatum] datetime,
    [BaseCompanyId] int,
    [DocKlantTaal] int,
    [ProjectProjectId] int NOT NULL,
    [LeverAdresStraat] nvarchar(150),
    [LeverAdresNr] nvarchar(20),
    [LeverAdresBus] nvarchar(20),
    [LeverAdresStad] nvarchar(150),
    [LeverAdresPostcode] nvarchar(20),
    [Opmerking] nvarchar(max),
    [LeverAdresLand] nvarchar(100),
    [IsVoorschotFactuur] bit,
    [ReedsGefactureerdVoorschot] decimal(18,2),
    [VoorschotNaam] nvarchar(500),
    [HeeftCommisie] bit,
    [VoorschotPercentage] decimal(18,4),
    [VerzondenVia] nvarchar(500),
    [DossierBeheerder] nvarchar(50),
    [ProjectBeheerder] nvarchar(50),
    [KlantVerantwoordelijke] nvarchar(50),
    [BetaaldOp] datetime,
    [BetalingswijzeId] int,
    [Reden] nvarchar(max),
    [ToelichtingVoorschotten] nvarchar(max),
    [BaseCompanyVatNumberId] int NOT NULL,
    [DocKlantGebouwNaam] nvarchar(400),
    [DocumentOpmerking] nvarchar(max),
    [CountryId] int,
    [PeppolVerzondenOp] datetime,
    [EasypostId] nvarchar(250),
    [PeppolStatus] nvarchar(250),
    CONSTRAINT [PK_Boekhouding_Documenten] PRIMARY KEY CLUSTERED ([DocId])
);
GO

CREATE TABLE [Boekhouding].[DocumentType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(150) NOT NULL,
    [NaamFr] nvarchar(150) NOT NULL,
    [ParameterId] int NOT NULL,
    [NaamEn] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_Boekhouding_DocumentType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Boekhouding].[IntrastatReportLine]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [DocumentDocId] int,
    [ProductNaam] nvarchar(250) NOT NULL,
    [PartnerLand] nvarchar(50) NOT NULL,
    [TransactieCode] nvarchar(50) NOT NULL,
    [Gewest] nvarchar(50) NOT NULL,
    [GoederenCode] nvarchar(50) NOT NULL,
    [Gewicht] decimal(18,3) NOT NULL,
    [AanvullendeEenheden] decimal(18,2) NOT NULL,
    [WaardeInEur] decimal(18,2) NOT NULL,
    [Vervoer] nvarchar(50),
    [Incoterm] nvarchar(50),
    [LandVanOorsprong] nvarchar(50) NOT NULL,
    [BtwNrTegenpartij] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Boekhouding_IntrastatReportLine] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Boekhouding].[KlantExtraKortingen]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Korting] decimal(18,2) NOT NULL,
    [TotBedrag] decimal(18,2) NOT NULL,
    [VanBedrag] decimal(18,2) NOT NULL,
    [TypeKlant] int NOT NULL,
    [BaseCompanyId] int,
    CONSTRAINT [PK_Boekhouding_KlantExtraKortingen] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Aanspreking]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [AansprekingTekst] nvarchar(50) NOT NULL,
    [Man] bit NOT NULL,
    [Vrouw] bit NOT NULL,
    [Taal] int NOT NULL,
    CONSTRAINT [PK_Crm_Aanspreking] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Activiteiten]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_Crm_Activiteiten] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Agenda]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] int,
    [StarteDate] datetime,
    [EndDate] datetime,
    [AllDay] bit,
    [Onderwerp] nvarchar(650),
    [Locatie] nvarchar(150),
    [Omschrijving] nvarchar(max),
    [Status] int,
    [Label] int,
    [ReminderInfo] nvarchar(max),
    [RecurrenceInfo] nvarchar(max),
    [OutlookId] nvarchar(500),
    [UserId] int,
    [KlantId] int,
    [ProjectId] int,
    [SyncedToExchange] bit NOT NULL,
    [DossierId] int,
    [OnHold] bit,
    [Cancelled] bit,
    [RedenOnHoldAndCancelled] nvarchar(500),
    [OnderwerpUserText] nvarchar(500),
    [ContactContactId] int,
    [OmschrijvingSysteem] nvarchar(max),
    [IsVerlof] bit,
    [GemaaktDoorUserId] int NOT NULL,
    [GemaaktOp] datetime NOT NULL,
    CONSTRAINT [PK_Crm_Agenda] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[AgendaLabel]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [Kleur] int NOT NULL,
    [IsType] bit NOT NULL,
    [IsCategorie] bit NOT NULL,
    [BeperktBewerken] bit,
    [IsVoorVerlof] bit,
    [IsBinnenDienst] bit,
    [IsBuitenDienst] bit,
    [IsProjectPlanning] bit,
    CONSTRAINT [PK_Crm_AgendaLabel] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[AgendaLog]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(650),
    [Start] datetime,
    [End] datetime,
    [ChangeAction] nvarchar(50) NOT NULL,
    [DossierId] int,
    [ProjectProjectId] int,
    [KlantKlantId] int,
    [ContactContactId] int,
    [IsVerlof] bit,
    [ChangeTime] datetime NOT NULL,
    [UserId] int NOT NULL,
    [AgendaId] int NOT NULL,
    [ChangedByUserId] int,
    CONSTRAINT [PK_Crm_AgendaLog] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[AgendaStatus]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [Kleur] int,
    [ToonInTodo] bit NOT NULL,
    CONSTRAINT [PK_Crm_AgendaStatus] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Betaaltermijn]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(150) NOT NULL,
    [AantalDagen] int NOT NULL,
    [Eindemaand] bit NOT NULL,
    [IsDefault] bit,
    CONSTRAINT [PK_Crm_Betaaltermijn] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[City]
(
    [CityId] int IDENTITY(1,1) NOT NULL,
    [CityName] nvarchar(150) NOT NULL,
    [CityZip] nvarchar(50) NOT NULL,
    [CityCountry] nvarchar(150) NOT NULL,
    [CountryIsoCode] nvarchar(50),
    [CountryId] int,
    CONSTRAINT [PK_Crm_City] PRIMARY KEY CLUSTERED ([CityId])
);
GO

CREATE TABLE [Crm].[ContactProjectRol]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NaamNl] nvarchar(150) NOT NULL,
    [NaamFr] nvarchar(150) NOT NULL,
    [NaamEn] nvarchar(150) NOT NULL,
    [SetNonActiveNaDossierAfgehandeld] bit NOT NULL,
    CONSTRAINT [PK_Crm_ContactProjectRol] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Country]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsEu] bit,
    [Isocode] nvarchar(10) NOT NULL,
    [Naam] nvarchar(100),
    CONSTRAINT [PK_Crm_Country] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantDossierStatusOpmerking]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [BestellingStatusId] int NOT NULL,
    [Opmerking] nvarchar(250) NOT NULL,
    [KlantKlantId] int NOT NULL,
    CONSTRAINT [PK_Crm_KlantDossierStatusOpmerking] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantFollowUp]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tekst] nvarchar(max) NOT NULL,
    [KlantKlantId] int NOT NULL,
    [Datum] datetime NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Crm_KlantFollowUp] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantJobcodeTarief]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantKlantId] int NOT NULL,
    [JobCodeId] int NOT NULL,
    [Uurtarief] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Crm_KlantJobcodeTarief] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantLeveradres]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Straat] nvarchar(250) NOT NULL,
    [Nummer] nvarchar(50) NOT NULL,
    [Bus] nvarchar(50) NOT NULL,
    [Stad] int NOT NULL,
    [Opmerking] nvarchar(2000) NOT NULL,
    [Contact] int,
    [KlantKlantId] int,
    [Naam] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Crm_KlantLeveradres] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantLeverancierKorting]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantKlantId] int NOT NULL,
    [KortingPercentage] decimal(18,4) NOT NULL,
    [SupplierSupplierId] int NOT NULL,
    [Opmerking] nvarchar(500),
    [Van] datetime NOT NULL,
    [Tot] datetime,
    [KlantTypeKlantTypeId] int,
    [AangemaaktOp] datetime NOT NULL,
    [UserId] int NOT NULL,
    [MontageKlantTypeId] int,
    [MontageKortingPercentage] decimal(18,4),
    CONSTRAINT [PK_Crm_KlantLeverancierKorting] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantMaatProduct]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantKlantId] int NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Omschrijvig] nvarchar(max) NOT NULL,
    [ProductEenheidId] int NOT NULL,
    [Artikelnummer] nvarchar(250),
    [Opmerking] nvarchar(1024),
    [Actief] bit NOT NULL,
    [ArtikelNummerLeverancier] nvarchar(250),
    CONSTRAINT [PK_Crm_KlantMaatProduct] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantMaatProductDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [GrondstofId] int,
    [SupplierId] int,
    [Aantal] decimal(18,4) NOT NULL,
    [ProductEenheidId] int NOT NULL,
    [AankoopPrijs] decimal(18,4),
    [KlantMaatProductId] int NOT NULL,
    [Artikelnummer] nvarchar(250),
    CONSTRAINT [PK_Crm_KlantMaatProductDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantMaatproductStaffel]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantMaatProductId] int NOT NULL,
    [MinAantal] decimal(18,2) NOT NULL,
    [MaxAantal] decimal(18,2) NOT NULL,
    [StukPrijs] decimal(18,4) NOT NULL,
    [VanDatum] datetime NOT NULL,
    [TotDatum] datetime NOT NULL,
    CONSTRAINT [PK_Crm_KlantMaatproductStaffel] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantOpmerkingen]
(
    [KlantId] int NOT NULL,
    [DocumentType] int,
    [Opmerking] nvarchar(max) NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    CONSTRAINT [PK_Crm_KlantOpmerkingen] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantProductKorting]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantKlantId] int NOT NULL,
    [KortingPercentage] decimal(18,4),
    [ProductProdId] int NOT NULL,
    [Opmerking] nvarchar(500),
    [Van] datetime NOT NULL,
    [Tot] datetime,
    [KlantTypeKlantTypeId] int,
    [AangemaaktOp] datetime NOT NULL,
    [UserId] int NOT NULL,
    [Marge] decimal(18,4),
    [KortingsPercentageMontage] decimal(18,4),
    [MontageKlantTypeId] int,
    CONSTRAINT [PK_Crm_KlantProductKorting] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[KlantStatus]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [Kleur] int NOT NULL,
    [Default] bit,
    CONSTRAINT [PK_Crm_KlantStatus] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Manufacturer]
(
    [ManufacturerId] int IDENTITY(1,1) NOT NULL,
    [ManufacturerName] nvarchar(150) NOT NULL,
    [ManufacturerPhone] nvarchar(50),
    [ManufacturerMobile] nvarchar(50),
    [ManufacturerFax] nvarchar(50),
    [ManufacturerEmail] nvarchar(150),
    [ManufacturerAddress] nvarchar(250),
    [ManufacturerCity] int,
    [ManufacturerKBO] nvarchar(50),
    [ManufacturerVAT] nvarchar(50),
    CONSTRAINT [PK_Crm_Manufacturer] PRIMARY KEY CLUSTERED ([ManufacturerId])
);
GO

CREATE TABLE [Crm].[ProjectContact]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectProjectId] int NOT NULL,
    [DossierId] int,
    [ContactProjectRolId] int NOT NULL,
    [ProjectContactId] int NOT NULL,
    [ContactContactId] int NOT NULL,
    CONSTRAINT [PK_Crm_ProjectContact] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Supplier]
(
    [SupplierId] int IDENTITY(1,1) NOT NULL,
    [SupplierName] nvarchar(150) NOT NULL,
    [SupplierPhone] nvarchar(50),
    [SupplierMobile] nvarchar(50),
    [SupplierFax] nvarchar(50),
    [SupplierEmail] nvarchar(150),
    [SupplierAddress] nvarchar(250),
    [SupplierCity] int,
    [SupplierKBO] nvarchar(50),
    [SupplierVAT] nvarchar(50),
    [SupplierOrderMail] nvarchar(250),
    [SupplierLangId] int NOT NULL,
    [GrootboekOmzetRekening] int NOT NULL,
    [MainSupplier] bit,
    [NietActief] bit NOT NULL,
    [Gecontroleerd] bit,
    [PrijsLijstVolgorde] int,
    [PrijslijstNaam] nvarchar(100),
    [Opmerking] nvarchar(max),
    CONSTRAINT [PK_Crm_Supplier] PRIMARY KEY CLUSTERED ([SupplierId])
);
GO

CREATE TABLE [Crm].[SupplierConact]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierId] int NOT NULL,
    [ContactId] int NOT NULL,
    [IsDefault] bit,
    [ContactFunction] int,
    CONSTRAINT [PK_Crm_SupplierConact] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[TaakActies]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [TakenId] int NOT NULL,
    [Datum] datetime NOT NULL,
    [Toelichting] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Crm_TaakActies] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[TaakType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(100) NOT NULL,
    [BinnenAantalDagenCompleet] decimal(18,2) NOT NULL,
    [Kleur] int,
    [WaarschuwingProductie] bit NOT NULL,
    [WaarschuwingLeveringMontage] bit NOT NULL,
    CONSTRAINT [PK_Crm_TaakType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Crm].[Taken]
(
    [DatumRappel] datetime NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [Klant] int,
    [Omschrijving] nvarchar(2000) NOT NULL,
    [Type] int NOT NULL,
    [User] int,
    [Voltooid] bit NOT NULL,
    [AanmaakDatum] datetime NOT NULL,
    [Project] int,
    [DossierDetailId] int,
    [DossierId] int,
    [BaseCompanyId] int,
    [EindDatum] datetime NOT NULL,
    [PercentComplete] int,
    [UsrGroepId] int,
    [VoltooidOp] datetime,
    [AangemaaktDoorUsrId] int NOT NULL,
    [Geannuleerd] bit,
    [GeannuleerdOp] datetime,
    [CheckedByCreaterOn] datetime,
    [Gelezen] bit,
    [PopupShown] bit,
    [Dringend] bit NOT NULL,
    [Weigeringsreden] nvarchar(max),
    [WeigereningGelezen] bit,
    [Geweigerd] bit,
    [GelezenOp] datetime,
    [GeweigerdOp] datetime,
    CONSTRAINT [PK_Crm_Taken] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [dbo].[abm$]
(
    [Id] float,
    [Naam] nvarchar(255),
    [ArtNrLev] nvarchar(255),
    [Art ABM] nvarchar(255),
    [Omschrijving] nvarchar(max),
    [EAN code] nvarchar(255),
    [Gewicht] float,
    [Recupel product] nvarchar(255),
    [Recupel Art] float,
    [Recupel Artnr am] nvarchar(255)
);
GO

CREATE TABLE [dbo].[imp_Intrastat2]
(
    [DocId] float,
    [DocDetId] float,
    [DocNummer] nvarchar(255),
    [GroepNaam] nvarchar(255),
    [DocDatum] datetime,
    [DocKlantNaam] nvarchar(255),
    [DocKlantBtwnr] nvarchar(255),
    [DocKlantLand] nvarchar(255),
    [LeverAdresLand] nvarchar(255),
    [ProductNaam] nvarchar(255),
    [Subtotaal] float,
    [Aanvullende Eenheden] float,
    [ProductGewicht] float,
    [KG] float,
    [Goederen] float,
    [Diensten] float,
    [Goederencode] nvarchar(255),
    [Gewicht] float,
    [LandVanOorsprong] float,
    [F20] nvarchar(255)
);
GO

CREATE TABLE [dbo].[impStockPlaatsen]
(
    [MG-O-REK1-01#A] nvarchar(255)
);
GO

CREATE TABLE [dbo].[instrastatimp]
(
    [DocId] float,
    [DocDetId] float,
    [DocNummer] nvarchar(255),
    [GroepNaam] nvarchar(255),
    [DocDatum] datetime,
    [DocKlantNaam] nvarchar(255),
    [DocKlantBtwnr] nvarchar(255),
    [DocKlantLand] nvarchar(255),
    [LeverAdresLand] nvarchar(255),
    [ProductNaam] nvarchar(255),
    [Subtotaal] float,
    [Diensten] nvarchar(255),
    [Goederen] float,
    [Goederencode] nvarchar(255),
    [Gewicht] float,
    [AanvullendeEenheden] float,
    [LandVanOorsprong] nvarchar(255)
);
GO

CREATE TABLE [dbo].[instrastatproductenupdateimport]
(
    [ProdId] float,
    [ProdName] nvarchar(255),
    [ProdDescription] nvarchar(max),
    [ProdOrderPartNumber] nvarchar(255),
    [ProdStockNumber] nvarchar(255),
    [SupplierName] nvarchar(255),
    [CityCountry] nvarchar(255),
    [ManufacturerName] nvarchar(255),
    [GoederenCode] nvarchar(255),
    [IntrastatCodeId] nvarchar(255),
    [Gewicht in gramme] float,
    [F12] nvarchar(255),
    [F13] nvarchar(255),
    [F14] nvarchar(255)
);
GO

CREATE TABLE [Emails].[Bijlage]
(
    [BestandId] int NOT NULL,
    [EmailId] int NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmailBestandsNaam] nvarchar(500) NOT NULL,
    [EmailOnlyFile] bit NOT NULL,
    CONSTRAINT [PK_Emails_Bijlage] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Emails].[Email]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantId] int,
    [ProjectId] int,
    [Van] nvarchar(500),
    [Aan] nvarchar(500),
    [Cc] nvarchar(500),
    [Bcc] nvarchar(500),
    [Onderwerp] nvarchar(1000) NOT NULL,
    [Inhoud] nvarchar(max) NOT NULL,
    [Verzonden] datetime NOT NULL,
    [Ontvangen] datetime NOT NULL,
    [ContactId] int,
    [SupplierId] int,
    [DossierId] int,
    [PreviewText] nvarchar(500) NOT NULL,
    [TakenId] int,
    [UserId] int,
    [IsPrivate] bit,
    [SupplierSupplierId] int,
    [ManufacturerManufacturerId] int,
    [DossierDetailId] int,
    [TeBehandelen] bit,
    [EmailQueueId] int,
    [StockOrderId] int,
    CONSTRAINT [PK_Emails_Email] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Emails].[EmailQueue]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Emails_EmailQueue] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[AutoNummering]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nummer] int NOT NULL,
    [Omschrijving] nvarchar(150) NOT NULL,
    [Prefix] nvarchar(50) NOT NULL,
    [Tag] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Instellingen_AutoNummering] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[BaseCompany]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250) NOT NULL,
    [Street] nvarchar(250) NOT NULL,
    [StreetNr] nvarchar(50) NOT NULL,
    [StreetBox] nvarchar(50) NOT NULL,
    [City] nvarchar(250) NOT NULL,
    [Zip] nvarchar(50) NOT NULL,
    [Country] nvarchar(50) NOT NULL,
    [KlantId] int,
    [Logo] varbinary(max),
    [VatNumber] nvarchar(50) NOT NULL,
    [Tel] nvarchar(50) NOT NULL,
    [Fax] nvarchar(50) NOT NULL,
    [IBAN] nvarchar(50) NOT NULL,
    [BIC] nvarchar(50) NOT NULL,
    [Slogan] nvarchar(250) NOT NULL,
    [BhDocumentFooter] nvarchar(max) NOT NULL,
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
    [EMail] nvarchar(150),
    [AllowAddNewFilesBlob] bit,
    CONSTRAINT [PK_Instellingen_BaseCompany] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[BaseCompanyAccess]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [BaseCompanyId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Instellingen_BaseCompanyAccess] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[BaseCompanyVatNumber]
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
    CONSTRAINT [PK_Instellingen_BaseCompanyVatNumber] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[Betalingswijze]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NaamNl] nvarchar(150) NOT NULL,
    [NaamFr] nvarchar(150) NOT NULL,
    [NaamEn] nvarchar(150) NOT NULL,
    [IsPrePay] bit NOT NULL,
    [IsPostPay] bit NOT NULL,
    CONSTRAINT [PK_Instellingen_Betalingswijze] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[GridLayout]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ObjectName] nvarchar(500) NOT NULL,
    [LayoutXml] nvarchar(max) NOT NULL,
    [Opmerking] nvarchar(150),
    [UsrId] int,
    [IsPivot] bit,
    [PivotName] nvarchar(50),
    CONSTRAINT [PK_Instellingen_GridLayout] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[HerstellingKostPrijs]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Prijs] decimal(18,2) NOT NULL,
    [Tot] datetime,
    [Van] datetime NOT NULL,
    CONSTRAINT [PK_Instellingen_HerstellingKostPrijs] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[LangTag]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [FieldName] nvarchar(250) NOT NULL,
    [NL] nvarchar(max) NOT NULL,
    [FR] nvarchar(max) NOT NULL,
    [From] nvarchar(250) NOT NULL,
    [En] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Instellingen_LangTag] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[Parameter]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(150) NOT NULL,
    [Waarde] nvarchar(max) NOT NULL,
    [Type] nvarchar(50) NOT NULL,
    [BaseCompanyId] int,
    CONSTRAINT [PK_Instellingen_Parameter] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[ProductKortingSuggestie]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [CorrectieBruto] decimal(18,4) NOT NULL,
    [Korting] decimal(18,4) NOT NULL,
    [Pro1] decimal(18,4) NOT NULL,
    [Pro2] decimal(18,4) NOT NULL,
    [Pro3] decimal(18,4) NOT NULL,
    [Aan1] decimal(18,4) NOT NULL,
    [Aan2] decimal(18,4) NOT NULL,
    [Par1] decimal(18,4) NOT NULL,
    [KortingTot] decimal(18,4) NOT NULL,
    [Ond1] decimal(18,4) NOT NULL,
    CONSTRAINT [PK_Instellingen_ProductKortingSuggestie] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[ProductKortingSuggestieDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantTypeKlantTypeId] int NOT NULL,
    [ProductKortingSuggestieId] int NOT NULL,
    [Korting] decimal(18,4) NOT NULL,
    CONSTRAINT [PK_Instellingen_ProductKortingSuggestieDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[StdFacturatieVoorwaarden]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Instellingen_StdFacturatieVoorwaarden] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[StdFacturatieVoorwaardenDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Percentage] decimal(18,4) NOT NULL,
    [StdFacturatieVoorwaardenId] int NOT NULL,
    CONSTRAINT [PK_Instellingen_StdFacturatieVoorwaardenDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[Taal]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [Tag] nvarchar(3) NOT NULL,
    [NaamFR] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Instellingen_Taal] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[Templates]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(500) NOT NULL,
    [Email] bit NOT NULL,
    [Fax] bit NOT NULL,
    [Brief] bit NOT NULL,
    [Default] bit NOT NULL,
    [Type] int NOT NULL,
    [BaseCompanyId] int,
    [TaalId] int NOT NULL,
    [AzureFileId] bigint,
    [Onderwerp] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Instellingen_Templates] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[TemplateType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(150) NOT NULL,
    [Tag] nvarchar(50) NOT NULL,
    [DocType] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Instellingen_TemplateType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[User]
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
    [Kleur] int NOT NULL,
    [ExchangeLastWatermark] nvarchar(1000),
    [DoSyncExchange] bit NOT NULL,
    [RechtstreeksGsmNrtonen] bit NOT NULL,
    [RechtstreeksTelefoonNrTonen] bit NOT NULL,
    [LinkedinUrl] nvarchar(500),
    [UsrGroepId] int,
    [DirecteLosseVerkoop] bit NOT NULL,
    [ReportSales] bit,
    [StdCElabelPrinter] nvarchar(255),
    [StdProductieLabelPrinter] nvarchar(255),
    [Login] nvarchar(50) NOT NULL,
    [Password] nvarchar(50) NOT NULL,
    [Email] nvarchar(150),
    [Tel] nvarchar(20),
    [Fax] nvarchar(20),
    [Gsm] nvarchar(20),
    [Voornaam] nvarchar(100) NOT NULL,
    [Achternaam] nvarchar(100) NOT NULL,
    [BaseCompaniesId] int NOT NULL,
    [TaalId] int NOT NULL,
    [Ice1Nr] nvarchar(50),
    [Ice1Naam] nvarchar(100),
    [Ice2Nr] nvarchar(50),
    [Ice2Naam] nvarchar(100),
    [EmaiPrive] nvarchar(150),
    [Hr] bit,
    [Adres] nvarchar(150) NOT NULL,
    [Indienst] datetime NOT NULL,
    [Uitdienst] datetime,
    [ProductBeheer] bit NOT NULL,
    [PlanningBinnenDienst] bit NOT NULL,
    [PlanningBuitendienst] bit NOT NULL,
    [PlanningProjecten] bit NOT NULL,
    [TelAbm] nvarchar(50),
    [GsmAbm] nvarchar(50),
    [Functie] nvarchar(50),
    [ToonInPrijslijst] bit,
    [SelecteerBijBinnendienstPlanning] bit,
    [SelecteerBijBuitendienstPlanning] bit,
    [SelecteerBijProjectPlanning] bit,
    [SelecteerBijVerlofPlanning] bit,
    [StandaardPlanningLabel] int,
    [KleurText] int NOT NULL,
    [ToegangOmzetRapporten] bit,
    [ToegangWinstRapporten] bit,
    [ToegangDmsspecial] bit,
    [ToegangBulkBestellingen] bit,
    [ToegangStockBeheer] bit,
    [VanuitDossierBestellingenDoen] bit,
    [ToonProjecten] bit,
    [Facturatie] bit,
    CONSTRAINT [PK_Instellingen_User] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Instellingen].[UsrGroep]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [IsMontage] bit NOT NULL,
    [IsService] bit NOT NULL,
    [IsTransport] bit NOT NULL,
    [DossierStatusGroepId] int,
    CONSTRAINT [PK_Instellingen_UsrGroep] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Klanten].[Contact]
(
    [ContactId] int IDENTITY(1,1) NOT NULL,
    [ContactBus] nvarchar(50) NOT NULL,
    [ContactEmail] nvarchar(150) NOT NULL,
    [ContactFax] nvarchar(50) NOT NULL,
    [ContactHuisnr] nvarchar(50) NOT NULL,
    [ContactLogin] nvarchar(50) NOT NULL,
    [ContactMobiel] nvarchar(50) NOT NULL,
    [ContactNaam] nvarchar(50) NOT NULL,
    [ContactPaswoord] nvarchar(50) NOT NULL,
    [ContactStraat] nvarchar(150) NOT NULL,
    [ContactTel] nvarchar(50) NOT NULL,
    [ContactVoornaam] nvarchar(150) NOT NULL,
    [ContactIsMonteur] bit NOT NULL,
    [ContactCity] int NOT NULL,
    [ContactIsInternUsr] bit NOT NULL,
    [ContactAanspreking] int,
    [ContactTaal] int NOT NULL,
    [BaseCompanyId] int,
    [ContactMonteurDisplayName] nvarchar(50),
    [ContactFunctie] nvarchar(100),
    [MailOfferte] bit NOT NULL,
    [MailOrderbevestiging] bit NOT NULL,
    [MailPlanning] bit NOT NULL,
    [MailLeveringKlaar] bit NOT NULL,
    [MailGeleverd] bit NOT NULL,
    [MailFacturatie] bit NOT NULL,
    [Uitdienst] datetime,
    [ContactGebouw] nvarchar(250),
    CONSTRAINT [PK_Klanten_Contact] PRIMARY KEY CLUSTERED ([ContactId])
);
GO

CREATE TABLE [Klanten].[Klant]
(
    [KlantId] int IDENTITY(1,1) NOT NULL,
    [KlantBtw] nvarchar(50) NOT NULL,
    [KlantBus] nvarchar(50) NOT NULL,
    [KlantHuisnr] nvarchar(50) NOT NULL,
    [KlantNaam] nvarchar(150) NOT NULL,
    [KlantStraat] nvarchar(150) NOT NULL,
    [KlantType] int NOT NULL,
    [KlantAlgTel] nvarchar(50) NOT NULL,
    [KlantAlgFax] nvarchar(50) NOT NULL,
    [KlantOpmerking] nvarchar(2024),
    [KlantEmail] nvarchar(250) NOT NULL,
    [KlantBtwsysteem] int NOT NULL,
    [KlantStatusId] int NOT NULL,
    [KlantCity] int NOT NULL,
    [KlantActiviteit] int,
    [KlantNummer] int,
    [KlantVerantwoordelijke] int NOT NULL,
    [EersteContact] nvarchar(150),
    [LockedTime] datetime,
    [Locked] bit NOT NULL,
    [LockedBy] nvarchar(100) NOT NULL,
    [OmzetLaatste12Maanden] decimal(18,2) NOT NULL,
    [PrijslijstViaEmail] bit NOT NULL,
    [PromotieMailing] bit NOT NULL,
    [LeverigsType] int NOT NULL,
    [FacturenPerPost] bit NOT NULL,
    [KlantInterneOpmerking] nvarchar(2024),
    [DigitaleFacturatie] bit NOT NULL,
    [CElabelName] nvarchar(50),
    [CElabelNr] nvarchar(50),
    [KlantBetalingStatus] int,
    [BaseCompaniesId] int,
    [IsInternalCompany] bit,
    [KlantTaal] int NOT NULL,
    [PrijslijstResAannemer] bit,
    [PrijslijstResDealer] bit,
    [PrijslijstResParticulier] bit,
    [PrijslijstIndaannemer] bit,
    [PrijslijstIndDealer] bit,
    [CEemail] nvarchar(250),
    [Logo] varbinary(max),
    [KlantGroep] nvarchar(50) NOT NULL,
    [AangemaaktDoor] nvarchar(50),
    [AangemaaktOp] datetime,
    [AangepastDoor] nvarchar(50),
    [AangepastOp] datetime,
    [Gecontroleerd] bit,
    [ContactOfferteContactId] int NOT NULL,
    [ContactOrderbevestigingContactId] int NOT NULL,
    [ContactPlanningContactId] int NOT NULL,
    [ContactLeveringCompleetContactId] int NOT NULL,
    [ContactFacturatieContactId] int NOT NULL,
    [CommisieOntvanger] int,
    [GevraagdeCommisie] decimal(18,4) NOT NULL,
    [BetaaltermijnId] int NOT NULL,
    [LoginWebShopABMATIC] nvarchar(150),
    [PasswordWebShopABMATIC] nvarchar(512),
    [SaltWebShopABMATIC] nvarchar(512),
    [KlantAdresBuilding] nvarchar(250),
    [LaatsteFollowUp] datetime,
    [LeveringKlantTypeId] int NOT NULL,
    [PeppolIdSchema] nvarchar(8),
    [PeppolId] nvarchar(250),
    CONSTRAINT [PK_Klanten_Klant] PRIMARY KEY CLUSTERED ([KlantId])
);
GO

CREATE TABLE [Klanten].[KlantContact]
(
    [KlantContactId] int IDENTITY(1,1) NOT NULL,
    [KlantContactKlantId] int,
    [KlantContactContactId] int NOT NULL,
    [KlantContactOpmerking] nvarchar(250),
    [DefaultConact] bit,
    [Functie] nvarchar(100),
    [SupplierSupplierId] int,
    [ManufacturerManufacturerId] int,
    CONSTRAINT [PK_Klanten_KlantContact] PRIMARY KEY CLUSTERED ([KlantContactId])
);
GO

CREATE TABLE [Klanten].[KlantType]
(
    [KlantTypeId] int IDENTITY(1,1) NOT NULL,
    [KlantTypeNaam] nvarchar(50) NOT NULL,
    [KlantTypeBtwNrVerplicht] bit NOT NULL,
    [Betaaltermijn] int NOT NULL,
    [BtwSysteem] int NOT NULL,
    [BasisKorting] decimal(18,4) NOT NULL,
    [LeveringsType] int NOT NULL,
    [KlantTypeNaamFr] nvarchar(50) NOT NULL,
    [Volgorde] int NOT NULL,
    [IsDefault] bit,
    CONSTRAINT [PK_Klanten_KlantType] PRIMARY KEY CLUSTERED ([KlantTypeId])
);
GO

CREATE TABLE [Logging].[Error]
(
    [DateTime] datetime NOT NULL,
    [ModuleName] nvarchar(50) NOT NULL,
    [Exception] nvarchar(1024) NOT NULL,
    [InnerException] nvarchar(1024) NOT NULL,
    [UserName] nvarchar(50) NOT NULL,
    [ClassName] nvarchar(50) NOT NULL,
    [Id] bigint NOT NULL
);
GO

CREATE TABLE [Logging].[ProjectActiviteit]
(
    [ProjectProjectId] int NOT NULL,
    [Actie] int NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [LogDate] datetime NOT NULL,
    CONSTRAINT [PK_Logging_ProjectActiviteit] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[DrawGroup]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Products_DrawGroup] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[Grondstof]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Omschrijving] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Products_Grondstof] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[IntrastatCode]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50) NOT NULL,
    [Name] nvarchar(250),
    [Text] nvarchar(max),
    [Hoofdgroep] nvarchar(500),
    [SubGroep] nvarchar(500),
    CONSTRAINT [PK_Products_IntrastatCode] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[LosseProducten]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [ArtikelNr] nvarchar(100),
    [Stockplaats] nvarchar(100),
    [Opmerking] nvarchar(250),
    [AankoopPrijs] decimal(18,2),
    [Leverancier] nvarchar(100),
    [AantalInStock] decimal(18,2),
    [LaatsteKeerGeteld] datetime,
    [Groep] nvarchar(150),
    CONSTRAINT [PK_Products_LosseProducten] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[OrderTemplate]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Active] bit NOT NULL,
    [KlantKlantId] int,
    CONSTRAINT [PK_Products_OrderTemplate] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[OrderTemplateDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductProdId] int,
    [SupplierSupplierId] int NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Groep] nvarchar(50) NOT NULL,
    [Aantal] decimal(18,4) NOT NULL,
    [AantalFormule] nvarchar(max) NOT NULL,
    [PrijsPerStuk] decimal(18,4) NOT NULL,
    [Totaal] decimal(18,4) NOT NULL,
    [OrderTemplateId] int NOT NULL,
    [PrijsFormule] nvarchar(max) NOT NULL,
    [ProductEenheidId] int NOT NULL,
    CONSTRAINT [PK_Products_OrderTemplateDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[PrestatieTarief]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(150) NOT NULL,
    [Tarief] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Products_PrestatieTarief] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[PrijslijstCategorie]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Volgorde] int NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Opties] bit,
    [Kleur] nvarchar(50),
    [NaamFr] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Products_PrijslijstCategorie] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[PrijslijstTeksten]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [BaseCompanyId] int NOT NULL,
    [Tekst] nvarchar(max) NOT NULL,
    [TekstFr] nvarchar(max) NOT NULL,
    [TekstEn] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Products_PrijslijstTeksten] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[Product]
(
    [ProdId] int IDENTITY(1,1) NOT NULL,
    [ProdName] nvarchar(150) NOT NULL,
    [ProdDescription] nvarchar(max) NOT NULL,
    [ProdOrderPartNumber] nvarchar(150) NOT NULL,
    [ProdStockNumber] nvarchar(50) NOT NULL,
    [ProdSupplier] int NOT NULL,
    [ProdManufacturer] int NOT NULL,
    [ProductTypeId] int,
    [ProdNonActive] bit NOT NULL,
    [ProdVerpaktPerVerkoop] decimal(18,4) NOT NULL,
    [ProdVerpaktPerAankoop] decimal(18,4) NOT NULL,
    [ProdPriceListOrder] int,
    [ProdPrijsLijst] bit NOT NULL,
    [ProdKorteOpmerking] nvarchar(500),
    [ProdToonOmschrijvingPrijslijst] bit NOT NULL,
    [ProdRecupel] decimal(18,4),
    [PredBebat] decimal(18,4),
    [ProdRalKleur] nvarchar(10),
    [ProdKleurPoedercode] nvarchar(50),
    [ProdMinAantal] decimal(18,2) NOT NULL,
    [ProdMaatwerkPercentage] decimal(18,4) NOT NULL,
    [ProdRecupelProduct] int,
    [ProdBebatProduct] int,
    [IsSnelleOptieLosseVk] bit,
    [ProdNameFr] nvarchar(150) NOT NULL,
    [ProdDescriptionFr] nvarchar(max) NOT NULL,
    [ProdKorteOpmerkingFr] nvarchar(500) NOT NULL,
    [TaakTemplate] int,
    [AankoopProductEenheidId] int,
    [VerkoopProductEenheid1Id] int,
    [AdsolutId] int,
    [ProdNameEN] nvarchar(150) NOT NULL,
    [ProdDescriptionEN] nvarchar(max) NOT NULL,
    [ProdKorteOpmerkingEN] nvarchar(500) NOT NULL,
    [ProdIsSamengesteldProduct] bit,
    [ProductStructuurId] int,
    [TempKorting] decimal(18,4),
    [TempNettoAankoop] decimal(18,2),
    [ReportingGroep1Id] int,
    [VerkoopAantalStockTrigger] decimal(18,3) NOT NULL,
    [MeerPrijs] decimal(18,4),
    [MeerprijsAssemblage] decimal(18,2),
    [MeerprijsMontage] decimal(18,2),
    [GeproduceerdSamengesteldProduct] bit,
    [Gecontroleerd] bit,
    [ProdZonderPrijs] bit NOT NULL,
    [ToonOpOfferte] bit NOT NULL,
    [ToonOpOrderbevestiging] bit NOT NULL,
    [ToonOpFactuur] bit NOT NULL,
    [ToonOpPakbon] bit NOT NULL,
    [ToonOpLeveringsBon] bit NOT NULL,
    [ToonOpProductieBon] bit NOT NULL,
    [ToonOpLakkerijBon] bit NOT NULL,
    [ToonOpMontageBon] bit NOT NULL,
    [Gewicht] decimal(18,3) NOT NULL,
    [OpmerkingInterneBonnen] nvarchar(1000),
    [ExterneMonteurKost] bit NOT NULL,
    [RecupelRapporteren] bit NOT NULL,
    [BebatRapporteren] bit NOT NULL,
    [HeeftStaffelprijzen] bit,
    [ToonGeenDetailprijs] bit,
    [WebShopABMATIC] bit,
    [LaatsteAanpassing] datetime,
    [LaatsteAanpassingDoor] nvarchar(50),
    [IsNieuw] bit,
    [EanCode] nvarchar(50),
    [PopUpMelding] nvarchar(512),
    [WebShopABMATICDescriptionNl] nvarchar(max) NOT NULL,
    [GoederenCode] nvarchar(50),
    [IntrastatCodeId] int,
    CONSTRAINT [PK_Products_Product] PRIMARY KEY CLUSTERED ([ProdId])
);
GO

CREATE TABLE [Products].[ProductAankoopKortingen]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Percentage] decimal(18,4) NOT NULL,
    [SupplierId] int NOT NULL,
    [Naam] nvarchar(150) NOT NULL,
    [Bruto] decimal(18,4) NOT NULL,
    [Pro1] decimal(18,4) NOT NULL,
    [Pro2] decimal(18,4) NOT NULL,
    [Pro3] decimal(18,4) NOT NULL,
    [Aan1] decimal(18,4) NOT NULL,
    [Aan2] decimal(18,4) NOT NULL,
    [Par1] decimal(18,4) NOT NULL,
    [Tot] datetime,
    [Van] datetime NOT NULL,
    [Ond] decimal(18,4) NOT NULL,
    CONSTRAINT [PK_Products_ProductAankoopKortingen] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductEenheid]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NaamEn] nvarchar(150) NOT NULL,
    [NaamFr] nvarchar(150) NOT NULL,
    [NaamNl] nvarchar(150) NOT NULL,
    [EenheidsParameter] bit NOT NULL,
    CONSTRAINT [PK_Products_ProductEenheid] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductHandleiding]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Path] nvarchar(1000) NOT NULL,
    [ProductProdId] int NOT NULL,
    [Web] bit,
    [AutomMeesturen] bit,
    [Extentie] nvarchar(25) NOT NULL,
    CONSTRAINT [PK_Products_ProductHandleiding] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductOptions]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [WaardeType] nvarchar(50) NOT NULL,
    [Verplicht] bit NOT NULL,
    [ProductId] int NOT NULL,
    [ProductieOpmerking] bit NOT NULL,
    [OfferteOpmerking] bit NOT NULL,
    [BerekenPrijs] bit NOT NULL,
    [Volgorde] int NOT NULL,
    [IsAantalLijn] bit,
    [NaamFR] nvarchar(250) NOT NULL,
    [StandaardWaardeFormule] nvarchar(4000),
    [Tag] nvarchar(250) NOT NULL,
    [FormuleAantal] nvarchar(4000),
    [NaamEn] nvarchar(250) NOT NULL,
    [ExtraPriceFormula] nvarchar(4000),
    [EenheidsparameterFormule] nvarchar(4000),
    CONSTRAINT [PK_Products_ProductOptions] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductOptionValue]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OptieProduct] int,
    [Waarde] nvarchar(100),
    [ProductOptionId] int NOT NULL,
    [Volgorde] int NOT NULL,
    [WaardeFr] nvarchar(100),
    [WaardeEn] nvarchar(100),
    CONSTRAINT [PK_Products_ProductOptionValue] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPopupRetourKolom]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NaamFr] nvarchar(50) NOT NULL,
    [NaamNl] nvarchar(50) NOT NULL,
    [RowLosseVkkolom] nvarchar(150) NOT NULL,
    [RowPoortOnderdeelKolom] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_Products_ProductPopupRetourKolom] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPopupTemplate]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [NaamFr] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Products_ProductPopupTemplate] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPopupTemplateDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NaamFr] nvarchar(50) NOT NULL,
    [NaamNl] nvarchar(50) NOT NULL,
    [OverbrengenNaanAantalFormule] nvarchar(250) NOT NULL,
    [OverbrengenNaarAantal] bit NOT NULL,
    [PrijsOpnemen] bit NOT NULL,
    [SchrijfNaarLijnKolom] int NOT NULL,
    [Verplicht] bit NOT NULL,
    [Volgorde] int NOT NULL,
    CONSTRAINT [PK_Products_ProductPopupTemplateDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPopupWaardeType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [NaamFr] nvarchar(50) NOT NULL,
    [Omschrijving] nvarchar(250) NOT NULL,
    [OmschrijvingFr] nvarchar(250) NOT NULL,
    [Tag] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Products_ProductPopupWaardeType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPrijzen]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Van] date NOT NULL,
    [Tot] date,
    [AssemblagePrijs] decimal(18,4) NOT NULL,
    [MontagePrijs] decimal(18,4) NOT NULL,
    [ProductId] int NOT NULL,
    [BrutoVerkoop] decimal(18,2) NOT NULL,
    [BrutoAankoop] decimal(18,2) NOT NULL,
    [NettoAankoop] decimal(18,2) NOT NULL,
    [ProductAankoopKortingenId] int,
    [OverrideBruto] bit,
    [Basisprijs] decimal(18,4) NOT NULL,
    [GecorrigeerdeBrutoPrijs] decimal(18,2) NOT NULL,
    [PrijsBerekenFormule] nvarchar(2024),
    [GebruikPrijsBerekenFormule] bit,
    [BasisPrijsBerekenFormule] nvarchar(2024),
    [OpstartKost] decimal(18,4) NOT NULL,
    [Pro1] decimal(18,4) NOT NULL,
    [Pro2] decimal(18,4) NOT NULL,
    [Pro3] decimal(18,4) NOT NULL,
    [Aan1] decimal(18,4) NOT NULL,
    [Aan2] decimal(18,4) NOT NULL,
    [Par1] decimal(18,4) NOT NULL,
    [Ond] decimal(18,4) NOT NULL,
    [ExtraAankoopKost] decimal(18,2),
    [ExtraAankoopKostUitleg] nvarchar(500),
    [AankoopKortingPercentage] decimal(18,4) NOT NULL,
    [BrutoCorrectiePercentage] decimal(18,4) NOT NULL,
    [TypeBerekening] int NOT NULL,
    [LeverancierHanteerdAndereBrutoVerkoop] bit NOT NULL,
    CONSTRAINT [PK_Products_ProductPrijzen] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPrijzenVerkoopKorting]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantTypeId] int NOT NULL,
    [ProductPrijzenId] int NOT NULL,
    [Korting] decimal(18,4) NOT NULL,
    CONSTRAINT [PK_Products_ProductPrijzenVerkoopKorting] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductProductionGroup]
(
    [ProdProdGroId] int IDENTITY(1,1) NOT NULL,
    [ProdProdGroName] nvarchar(150) NOT NULL,
    [ProdProdGroOrder] int NOT NULL,
    [Kleur] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Products_ProductProductionGroup] PRIMARY KEY CLUSTERED ([ProdProdGroId])
);
GO

CREATE TABLE [Products].[ProductProductionsGroepen]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int NOT NULL,
    [ProductieGroep] int NOT NULL,
    CONSTRAINT [PK_Products_ProductProductionsGroepen] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductPropertieItem]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductPropertyId] int NOT NULL,
    [Value] nvarchar(250) NOT NULL,
    [ProductProdId] int NOT NULL,
    CONSTRAINT [PK_Products_ProductPropertieItem] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductProperty]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NameEn] nvarchar(250) NOT NULL,
    [NameFr] nvarchar(250) NOT NULL,
    [NameNl] nvarchar(250) NOT NULL,
    [Volgorde] int NOT NULL,
    CONSTRAINT [PK_Products_ProductProperty] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductStaffel]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Minimum] decimal(18,2) NOT NULL,
    [Korting] decimal(18,4) NOT NULL,
    [ProductProdId] int NOT NULL,
    CONSTRAINT [PK_Products_ProductStaffel] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductStockLocatie]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [StockId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Aantal] decimal(18,4) NOT NULL,
    [NietMeerActief] bit,
    [MaxAantal] decimal(18,4) NOT NULL,
    [IsStandaard] bit NOT NULL,
    [MinAantal] decimal(18,4) NOT NULL,
    [Gereserveerd] decimal(18,4) NOT NULL,
    [LaatsteKeerGeteld] datetime,
    [AantalGeteld] decimal(18,4),
    CONSTRAINT [PK_Products_ProductStockLocatie] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductStructuur]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Level] int NOT NULL,
    [ParentId] int,
    [NaamNl] nvarchar(250) NOT NULL,
    [NaamEn] nvarchar(250) NOT NULL,
    [NaamFr] nvarchar(250) NOT NULL,
    [IntoPrijslijstTekstenId] int,
    [OutroPrijslijstTekstenOutroId] int,
    [Order] int NOT NULL,
    [Kleur] int,
    [TonenOpPrijslijst] bit,
    [Icon] varbinary(max),
    [PageBreakAfter] bit NOT NULL,
    CONSTRAINT [PK_Products_ProductStructuur] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductStructuurWebShopABMATIC]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NaamEn] nvarchar(250) NOT NULL,
    [NaamFr] nvarchar(250) NOT NULL,
    [NaamNl] nvarchar(250) NOT NULL,
    [ParentId] int,
    CONSTRAINT [PK_Products_ProductStructuurWebShopABMATIC] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductSubProduct]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [MasterProductProdId] int NOT NULL,
    [SubProductProdId] int NOT NULL,
    [Aantal] decimal(18,4) NOT NULL,
    [Optioneel] bit NOT NULL,
    [ExrtaBasisPrijs] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Products_ProductSubProduct] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ProductType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [NameFr] nvarchar(150) NOT NULL,
    [StockItem] bit NOT NULL,
    [IsProductie] bit NOT NULL,
    [BestelArtikel] bit NOT NULL,
    CONSTRAINT [PK_Products_ProductType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[ReportingGroep1]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [Volgorde] int NOT NULL,
    CONSTRAINT [PK_Products_ReportingGroep1] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockBeweging]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductProdId] int NOT NULL,
    [DossierDetailId] int,
    [Aantal] decimal(18,4) NOT NULL,
    [DatumEnTijd] datetime NOT NULL,
    [Opmerking] nvarchar(150),
    [IsReservering] bit,
    [ProductStockLocatieId] int NOT NULL,
    CONSTRAINT [PK_Products_StockBeweging] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockLocatie]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(150) NOT NULL,
    [IsMagazijn] bit,
    CONSTRAINT [PK_Products_StockLocatie] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockOrder]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [LeverancierId] int NOT NULL,
    [CreateDatum] datetime NOT NULL,
    [OrderDatum] datetime,
    [OrderBevestigingDatum] datetime,
    [LeveringsDatum] datetime,
    [FactuurDatum] datetime,
    [Afgewerkt] bit NOT NULL,
    [Opmerking] nvarchar(max),
    [UserId] int NOT NULL,
    [VerwachteLeveringsDatum] datetime,
    [InterneOpmerking] nvarchar(max),
    [TotaalBedrag] decimal(18,2),
    CONSTRAINT [PK_Products_StockOrder] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockOrderDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [StockOrderId] int NOT NULL,
    [ProductId] int,
    [AantalBesteld] decimal(18,2) NOT NULL,
    [LijnOK] bit NOT NULL,
    [Productnaam] nvarchar(500) NOT NULL,
    [Ordernummer] nvarchar(150) NOT NULL,
    [VerpaktPer] nvarchar(50) NOT NULL,
    [AankoopPrijsStuk] decimal(18,4) NOT NULL,
    [AankoopPrijsTotaal] decimal(18,4) NOT NULL,
    [Eenheid] nvarchar(50) NOT NULL,
    [InterneReferentie] nvarchar(150),
    [OpermerkingLevering] nvarchar(2000),
    [OpmerkingBestelling] nvarchar(2000),
    [Besteld] bit,
    [BesteldOp] datetime,
    [Geleverd] bit,
    [GeleverdOp] datetime,
    [ProductTypeId] int,
    [AantalGeleverd] decimal(18,2) NOT NULL,
    [AantalVerwerktInStock] decimal(18,2),
    CONSTRAINT [PK_Products_StockOrderDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[StockOrderLevering]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [StockOrderDetail] int NOT NULL,
    [LeveringsDocNummer] nvarchar(100) NOT NULL,
    [Datum] datetime NOT NULL,
    [Aantal] decimal(18,2) NOT NULL,
    [AantalGefactureerd] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Products_StockOrderLevering] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Products].[stockreserveringbackup]
(
    [id] int IDENTITY(1,1) NOT NULL,
    [AantalDefinitiefUitStock] decimal(18,4),
    [AantalInStockGereserveerd] decimal(18,4),
    [AantalAfgehaaldOfGeleverd] decimal(18,4),
    CONSTRAINT [PK_Products_stockreserveringbackup] PRIMARY KEY CLUSTERED ([id])
);
GO

CREATE TABLE [Products].[WebShopABMATICStructuur]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NaamNl] nvarchar(50) NOT NULL,
    [ParentId] int,
    [Volgorde] int NOT NULL,
    CONSTRAINT [PK_Products_WebShopABMATICStructuur] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[Bestelling]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Geaccepteerd] bit NOT NULL,
    [GemaaktOp] datetime NOT NULL,
    [GemaaktDoorUsrId] int NOT NULL,
    [ProjectId] int NOT NULL,
    [AlgemeneKorting] decimal(18,4) NOT NULL,
    [LeveringsType] int NOT NULL,
    [PrijslijstType] int NOT NULL,
    [OfferteId] int,
    [Dossiernummer] int,
    [Commissiebedrag] decimal(18,4) NOT NULL,
    [MonteurContactId] int,
    [GewensteOpleveringsdatum] datetime,
    [MontageDuurInDagen] int,
    [ProductieDuurInDagen] int,
    [MasterDossier] int,
    [BtwTypeId] int NOT NULL,
    [DossierVerwerkingsTypeId] int NOT NULL,
    [KlantTypeKlantTypeId] int NOT NULL,
    [InterneNota] nvarchar(max),
    [RalKleur] nvarchar(150),
    [VoorschotFacturatie] bit,
    [ExtraKorting] decimal(18,4),
    [KlantOpmerking] nvarchar(max),
    [MonteurOpmerking] nvarchar(max),
    [AbmaticOpmerking] nvarchar(max),
    [DeelsLeveringToestaan] bit NOT NULL,
    [CommissieVerkoper] int,
    [CommissieGefactureerd] bit,
    [TefacturerenCommissie] decimal(18,2),
    [LeveradresId] int,
    [BetaaltermijnId] int NOT NULL,
    [PopupMelding] nvarchar(max),
    [OfferteAantalDagenGeldig] int NOT NULL,
    [IsDringend] bit NOT NULL,
    [PrijslijstDatum] datetime,
    [BaseCompanyVatNumberId] int NOT NULL,
    [VoorschottenOpBasisVanBedragen] bit,
    [HeeftCloudMap] bit,
    [AfsluitingGecontroleerd] bit,
    [OpmerkingFactuur] nvarchar(max),
    [OpmerkingOfferte] nvarchar(max),
    CONSTRAINT [PK_Projecten_Bestelling] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[BestellingDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [BestellingId] int NOT NULL,
    [PoortId] int,
    [ProductId] int,
    [Aantal] decimal(18,4) NOT NULL,
    [PrijsPerEenheid] decimal(18,4) NOT NULL,
    [AssemblagePrijs] decimal(18,4) NOT NULL,
    [MontagePrijs] decimal(18,4) NOT NULL,
    [TotaalExcl] decimal(18,4) NOT NULL,
    [TotaalIncl] decimal(18,4) NOT NULL,
    [IsOptie] bit NOT NULL,
    [Korting] decimal(18,4) NOT NULL,
    [Btw] decimal(18,4) NOT NULL,
    [UpliftType] nvarchar(50) NOT NULL,
    [NaamVoorDocument] nvarchar(500) NOT NULL,
    [Lengte] decimal(18,4) NOT NULL,
    [Hoogte] decimal(18,4) NOT NULL,
    [Breedte] decimal(18,4) NOT NULL,
    [Assemblage] bit NOT NULL,
    [Montage] bit NOT NULL,
    [OpmerkingOfferte] nvarchar(max) NOT NULL,
    [OpmerkingProductie] nvarchar(max),
    [MontageStukprijs] decimal(18,4) NOT NULL,
    [AssemblageStukprijs] decimal(18,4) NOT NULL,
    [ProductOptionId] int,
    [ProductOptionHoofdDetaillId] int,
    [Serienummer] nvarchar(150),
    [ProductieGroep] nvarchar(50) NOT NULL,
    [AantalProductieVerwerkt] decimal(18,4),
    [AantalAfgehaaldOfGeleverd] decimal(18,4),
    [AantalGefactureerd] decimal(18,4),
    [Afgehandeld] bit,
    [OpmerkingBestelBon] nvarchar(max) NOT NULL,
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
    [DatumKlaarvoorFacturatie] datetime,
    [BtwBedrag] decimal(18,4) NOT NULL,
    [Volgorde] int NOT NULL,
    [Tecrediteren] bit,
    [GecrediteerdOp] datetime,
    [ToOrder] bit,
    [Ordered] bit,
    [OrderedDate] datetime,
    [NaamVoorDocumentFR] nvarchar(500) NOT NULL,
    [NettoCommisieEenheidsPrijs] decimal(18,4),
    [IsSubProduct] bit,
    [AantalSubProduct] decimal(18,4) NOT NULL,
    [BestelNummer] nvarchar(100),
    [SupplierSupplierId] int,
    [AankoopPrijs] decimal(18,2),
    [ProductEenheidId] int,
    [GefactureerdOp] datetime,
    [PrijsControleren] bit,
    [AantalTeBestellen] decimal(18,4) NOT NULL,
    [ProductStockLocatieId] int,
    [AantalInStockGereserveerd] decimal(18,4),
    [AantalDefinitiefUitStock] decimal(18,4),
    [NoPriceCacl] bit,
    [AantalBeschikbaarInStock] decimal(18,2),
    [IsSamenGesteldProduct] bit,
    [ReportingGroep1Id] int NOT NULL,
    [BrutoAankoopprijs] decimal(18,4) NOT NULL,
    [NettoAankoopPrijs] decimal(18,4) NOT NULL,
    [WinstPercentage] decimal(18,4) NOT NULL,
    [StockOrderDetailId] int,
    [AantalBesteld] decimal(18,4),
    [BesteldOp] datetime,
    [ArtikelNr] nvarchar(250),
    [OrigineleKorting] decimal(18,4) NOT NULL,
    [OverRuleLosseLijnAutoKorting] bit,
    [IsTextOnly] bit,
    [UpliftTypeOrigineel] nvarchar(50) NOT NULL,
    [BasisPrijs] decimal(18,2) NOT NULL,
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
    [GeproduceerdSamenGesteldproduct] bit,
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
    [EenheidsParameter] decimal(18,4) NOT NULL,
    [BasisPrijsTellen] bit NOT NULL,
    [ToonOpOfferte] bit NOT NULL,
    [ToonOpOrderbevestiging] bit NOT NULL,
    [ToonOpFactuur] bit NOT NULL,
    [ToonOpPakbon] bit NOT NULL,
    [ToonOpLeveringsbon] bit NOT NULL,
    [ToonOpProductieBon] bit NOT NULL,
    [ToonOpLakBon] bit NOT NULL,
    [ToonOpMontageBon] bit NOT NULL,
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
    [OpstartKost] decimal(18,2) NOT NULL,
    [OpstartKostTotaal] decimal(18,2) NOT NULL,
    [BasisPrijsTotaal] decimal(18,2) NOT NULL,
    [BtwTypeId] int NOT NULL,
    [OrderTemplateId] int,
    [OrderTemplateDetailId] int,
    [StukPrijs] decimal(18,2) NOT NULL,
    [KlantMaatProductId] int,
    [TaxproductenGecontroleerd] bit,
    [RecupelRapporteren] bit NOT NULL,
    [BebatRapporteren] bit NOT NULL,
    [ToonGeenDetailPrijzen] bit,
    [OfferteDocumentDocId] int,
    [OrderBevestigingDocumentDocId] int,
    [PakBonDocumentDocId] int,
    [LeveringsBonDocId] int,
    [FactuurDocId] int,
    [IsProductie] bit,
    [OmschrijvingProbleem] nvarchar(max),
    [KlantBinnenGebrachtOp] datetime,
    [KlantTerugOpgehaaldOp] datetime,
    [VanKlantInOntvangstGenomenDoor] nvarchar(50),
    [OmschrijvingHerstelling] nvarchar(max),
    [VerzondenNaarLeverancierOp] datetime,
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
    [Voorschotzichtbaarheid] nvarchar(50) NOT NULL,
    [Goederen] decimal(18,2),
    [Diensten] decimal(18,2),
    [GoederenCode] nvarchar(50),
    [Gewicht] decimal(18,3),
    [AanvullendeEenheden] decimal(18),
    [LandVanOorsprong] int,
    [IntrastatReported] bit,
    [IntrastatReportedOn] datetime,
    CONSTRAINT [PK_Projecten_BestellingDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[BestellingStatus]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [Sequence] int,
    [IsForSalesReporting] bit,
    [NaamFr] nvarchar(50) NOT NULL,
    [ReportInBehandeling] bit,
    [ScreenModus] nvarchar(50) NOT NULL,
    [DossierStatusGroepId] int,
    [DoStockReservering] bit NOT NULL,
    [DoStock] bit NOT NULL,
    CONSTRAINT [PK_Projecten_BestellingStatus] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[BestellingStatusToegangen]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [BesellingStatusId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Projecten_BestellingStatusToegangen] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[BestellingType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [SerienrSuffix] nvarchar(50) NOT NULL,
    [DossierVerwerkingsTypeId] int NOT NULL,
    [Volgorde] int NOT NULL,
    [NaamFr] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Projecten_BestellingType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[BinnengebrachtProduct]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [DossierId] int NOT NULL,
    [Naam] nvarchar(1024) NOT NULL,
    [Toelichting] nvarchar(max) NOT NULL,
    [OntvangenOp] datetime NOT NULL,
    [OntvangenDoorUserId] int NOT NULL,
    [GebrachtDoor] nvarchar(150) NOT NULL,
    [OpgehaaldDatum] datetime,
    [OpgehaaldDoor] nvarchar(150),
    [VerzondenNaarLeverancierOp] datetime,
    [TrackingNr] nvarchar(150),
    [TerugOntvangenVanLeverancierOp] datetime,
    [OnherstelbaarDatum] datetime,
    CONSTRAINT [PK_Projecten_BinnengebrachtProduct] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[DossierDetailText]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    [OrderBevestiging] bit NOT NULL,
    [Leveringsbon] bit NOT NULL,
    [Factuur] bit NOT NULL,
    [AkOrder] bit NOT NULL,
    [Offerte] bit NOT NULL,
    [DossierDetailsId] int,
    [MontageBon] bit NOT NULL,
    [Lakbon] bit NOT NULL,
    [ProductieBon] bit NOT NULL,
    CONSTRAINT [PK_Projecten_DossierDetailText] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[DossierDevelopmentDetail]
(
    [Guid] uniqueidentifier NOT NULL,
    [ArtikelNr] nvarchar(250) NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Omschrijving] nvarchar(max),
    [ProductProdId] int,
    [MasterRowGuid] uniqueidentifier,
    [Aantal] decimal(18,3),
    [ProductEenheidId] int,
    [AantalGereserveerd] decimal(18,4) NOT NULL,
    [AantalUitstock] decimal(18,4) NOT NULL,
    [TeBestellen] bit NOT NULL,
    [BesteldOp] datetime,
    [GeleverdOp] datetime,
    [SupplierSupplierId] int,
    [AankoopPrijs] decimal(18,2) NOT NULL,
    [TotaalAankoop] decimal(18) NOT NULL,
    [TekeningGemaakt] bit NOT NULL,
    [TypeMateriaal] nvarchar(150),
    [Bewerking] nvarchar(150),
    [DossierId] int NOT NULL,
    [Volgorde] int NOT NULL,
    [Gewicht] decimal(18,2),
    [Afwerking] nvarchar(max),
    [StockOrderDetailId] int,
    [Lakken] bit,
    [LakwerkSupplierSupplierId] int
);
GO

CREATE TABLE [Projecten].[DossierFeedback]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [DossierId] int NOT NULL,
    [Bucket] nvarchar(150) NOT NULL,
    [Datum] datetime NOT NULL,
    [UserId] int NOT NULL,
    [Tekst] nvarchar(max),
    CONSTRAINT [PK_Projecten_DossierFeedback] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[DossierInstallatieDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ItemNr] nvarchar(50) NOT NULL,
    [OnderdeelNummer] nvarchar(250) NOT NULL,
    [Omschrijving] nvarchar(500),
    [Materiaal] nvarchar(150),
    [Aantal] decimal(18,4) NOT NULL,
    [AantalInInstallatie] decimal(18,4),
    [Bewerking] nvarchar(150),
    [LeverancierId] int,
    [LeverancierArtikelNr] nvarchar(150),
    [Behandeling] nvarchar(50),
    [Besteldatum] datetime,
    [Leverdatum] datetime,
    [PrijsPerStuk] decimal(18,4),
    [PrijsTotaal] decimal(18,4),
    [Opmerking] nvarchar(1000),
    [DossierId] int NOT NULL,
    CONSTRAINT [PK_Projecten_DossierInstallatieDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[DossierLeveringsTypeProduct]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [LeveringTypeId] int NOT NULL,
    [ProductProdId] int NOT NULL,
    CONSTRAINT [PK_Projecten_DossierLeveringsTypeProduct] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[DossierLog]
(
    [Id] bigint NOT NULL,
    [DossierId] int NOT NULL,
    [UserId] int NOT NULL,
    [Omschrijving] nvarchar(500) NOT NULL
);
GO

CREATE TABLE [Projecten].[DossierOpmerking]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [DossierId] int NOT NULL,
    [KlantopmerkingId] int,
    [ProductieCategories] nvarchar(150),
    [Opmerking] nvarchar(max) NOT NULL,
    [ProductieCategory] int,
    [DocumentType] int,
    CONSTRAINT [PK_Projecten_DossierOpmerking] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[DossierProjectDetail]
(
    [Guid] uniqueidentifier NOT NULL,
    [ArtikelNr] nvarchar(250) NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Omschrijving] nvarchar(max) NOT NULL,
    [ProductProdId] int,
    [MasterRowGuid] uniqueidentifier,
    [Aantal] decimal(18,3) NOT NULL,
    [ProductEenheidId] int NOT NULL,
    [AantalGereserveerd] decimal(18,4) NOT NULL,
    [AantalUitstock] decimal(18,4) NOT NULL,
    [TeBestellen] bit NOT NULL,
    [BesteldOp] datetime NOT NULL,
    [GeleverdOp] datetime NOT NULL,
    [SupplierSupplierId] int,
    [AankoopPrijs] decimal(18,2) NOT NULL,
    [TotaalAankoop] decimal(18) NOT NULL,
    [TekeningGemaakt] bit NOT NULL
);
GO

CREATE TABLE [Projecten].[DossierStatusGroep]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Projecten_DossierStatusGroep] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[DossierStructuur]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Nummer] nvarchar(1000) NOT NULL,
    [MasterRowId] int,
    [BestellingsTypeId] int,
    [DossierId] int,
    [Sequence] int NOT NULL,
    [ProjectProjectId] int NOT NULL,
    [GemaaktOp] datetime NOT NULL,
    [GemaaktDoor] int NOT NULL,
    [StatusId] int NOT NULL,
    [ReferentieKlant] nvarchar(1000) NOT NULL,
    [DossierNummer] nvarchar(50),
    [Path] nvarchar(1024),
    [DatumOrderBevestiging] datetime,
    [DatumGepland] datetime,
    [DatumLeveringsKlaar] datetime,
    [DatumKlaarVoorFacturatie] datetime,
    [DatumAfgehandeld] datetime,
    [DatumOfferte] datetime,
    [Totaal] decimal(18,2),
    [TotaalGefactureerd] decimal(18,2),
    [TeamsId] nvarchar(250),
    [OrderbevestigingDoor] nvarchar(50),
    [GeplandDoor] nvarchar(50),
    [LeveringsklaarDoor] nvarchar(50),
    [AfgehandeldDoor] nvarchar(50),
    [OfferteDoor] nvarchar(50),
    [KlaarvoorFacturatieDoor] nvarchar(50),
    [HeeftPapierenDossier] bit NOT NULL,
    [KlaargezetOp] datetime,
    CONSTRAINT [PK_Projecten_DossierStructuur] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[DossierVerwerkingsType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [PrijsCalculatie] bit NOT NULL,
    [BestellingStatusId] int NOT NULL,
    CONSTRAINT [PK_Projecten_DossierVerwerkingsType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[DossierVoorschot]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [DossierId] int NOT NULL,
    [Naam] nvarchar(250) NOT NULL,
    [Percent] decimal(18,4) NOT NULL,
    [IsEindFactuur] bit NOT NULL,
    [GefactureerdOp] datetime,
    [Volgorde] int NOT NULL,
    [Bedrag] decimal(18,6),
    [Voorschotzichtbaarheid] nvarchar(50),
    CONSTRAINT [PK_Projecten_DossierVoorschot] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[FacturatieAfspraak]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantId] int NOT NULL,
    [DossierId] int NOT NULL,
    [Percentage] decimal(18,4) NOT NULL,
    [Klantnaam] nvarchar(250) NOT NULL,
    [Btwperecentage] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Projecten_FacturatieAfspraak] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[JobCode]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(10) NOT NULL,
    [NaamNl] nvarchar(150) NOT NULL,
    [NaamFr] nvarchar(150) NOT NULL,
    [NaamEn] nvarchar(150) NOT NULL,
    [UurPrijs] decimal(18,2) NOT NULL,
    [Factureerbaar] bit NOT NULL,
    CONSTRAINT [PK_Projecten_JobCode] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[LeveringType]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(50) NOT NULL,
    [MontageKostTellen] bit NOT NULL,
    [NaamFr] nvarchar(50) NOT NULL,
    [IsDefault] bit,
    CONSTRAINT [PK_Projecten_LeveringType] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[OnderhoudsContract]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naam] nvarchar(500) NOT NULL,
    [Van] datetime NOT NULL,
    [Tot] datetime,
    [Bedrag] decimal(18,2) NOT NULL,
    [AutomatischVerlengen] bit NOT NULL,
    [OpgezegdOp] datetime,
    [RappelDatum] datetime,
    [ProjectProjectId] int NOT NULL,
    [CyclusDagen] int NOT NULL,
    [AantaldagenOpvoorhandRappelleren] int NOT NULL,
    [BtwTypeId] int NOT NULL,
    [OndershoudsContractOpvolgerUserId] int,
    CONSTRAINT [PK_Projecten_OnderhoudsContract] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[OnderhoudsContractDetail]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [OnderhoudsContractId] int NOT NULL,
    [ProjectInstallatieId] int NOT NULL,
    CONSTRAINT [PK_Projecten_OnderhoudsContractDetail] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[Project]
(
    [ProjectId] int IDENTITY(1,1) NOT NULL,
    [ProjectNummer] int NOT NULL,
    [ProjectNaam] nvarchar(250) NOT NULL,
    [ProjectBeheerder] int NOT NULL,
    [ProjectKlant] int,
    [ProjectWerf] int,
    [ProjectTypeId] int NOT NULL,
    [ProjectAanmaakDatum] datetime,
    [ProjectInterneOpmerking] nvarchar(max),
    [BaseCompanyId] int,
    [ProductionLabelReference] nvarchar(60),
    [IsTemplate] bit,
    [ProjectOpmerking] nvarchar(max),
    [IsStandaardProject] bit NOT NULL,
    [TeamsId] nvarchar(250),
    [PopupMelding] nvarchar(max),
    CONSTRAINT [PK_Projecten_Project] PRIMARY KEY CLUSTERED ([ProjectId])
);
GO

CREATE TABLE [Projecten].[ProjectInstallatie]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [InstallatieDossierId] int,
    [Locatie] nvarchar(250),
    [Naam] nvarchar(250),
    [ProjectId] int NOT NULL,
    [Serienummer] nvarchar(250),
    [Specificaties] nvarchar(max),
    CONSTRAINT [PK_Projecten_ProjectInstallatie] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[ProjectLog]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectId] int NOT NULL,
    [Actie] nvarchar(150) NOT NULL,
    [Datum] datetime NOT NULL,
    CONSTRAINT [PK_Projecten_ProjectLog] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[ProjectPartij]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantKlantId] int NOT NULL,
    [ProjectPartijGroepId] int,
    [Commentaar] nvarchar(500),
    [FacturatiePercentage] decimal(18,4) NOT NULL,
    [ProjectProjectId] int NOT NULL,
    CONSTRAINT [PK_Projecten_ProjectPartij] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[ProjectPartijContact]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [KlantContactKlantContactId] int NOT NULL,
    [ProjectPartijId] int NOT NULL,
    [MailOfferte] bit NOT NULL,
    [MailOrderbevestiging] bit NOT NULL,
    [MailPlanning] bit NOT NULL,
    [MailLeveringKlaar] bit NOT NULL,
    [MailGeleverd] bit NOT NULL,
    [MailFacturatie] bit NOT NULL,
    [Notitie] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_Projecten_ProjectPartijContact] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[ProjectPartijGroep]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NaamEn] nvarchar(50) NOT NULL,
    [NaamFr] nvarchar(50) NOT NULL,
    [NaamNl] nvarchar(50) NOT NULL,
    [IsVoorLeverancier] bit NOT NULL,
    [IsVoorKlant] bit NOT NULL,
    [IsVoorFabrikant] bit NOT NULL,
    CONSTRAINT [PK_Projecten_ProjectPartijGroep] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[Timesheet]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [DossierId] int,
    [StartDt] datetime NOT NULL,
    [StopDt] datetime,
    [DuurInMin] decimal(18),
    [DuurInUur] decimal(18,2),
    [JobCodeId] int NOT NULL,
    [Factureerbaar] bit NOT NULL,
    [TeFacturerenTijdInMin] decimal(18,2),
    [TeFacturerenTijdInUur] decimal(18,2),
    [PrestantUserId] int NOT NULL,
    [AangemaaktOp] datetime NOT NULL,
    [AangemaaktUserId] int NOT NULL,
    [Omschrijving] nvarchar(500) NOT NULL,
    [TimerOn] bit NOT NULL,
    [TimerStart] datetime,
    [TimerLastValue] int,
    CONSTRAINT [PK_Projecten_Timesheet] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Projecten].[Werf]
(
    [WerfAdres] nvarchar(150) NOT NULL,
    [WerfBus] nvarchar(50) NOT NULL,
    [WerfHuisnr] nvarchar(50) NOT NULL,
    [WerfId] int IDENTITY(1,1) NOT NULL,
    [WerfPlaats] int NOT NULL,
    [WerfEindKlantEmail] nvarchar(150) NOT NULL,
    [WerfEindKlantGsm] nvarchar(50) NOT NULL,
    [WerfEindKlantNaam] nvarchar(250) NOT NULL,
    [WerfEindKlantTel] nvarchar(50) NOT NULL,
    [WerfOpmerking] nvarchar(2000),
    CONSTRAINT [PK_Projecten_Werf] PRIMARY KEY CLUSTERED ([WerfId])
);
GO

CREATE TABLE [Taken].[TaakDependency]
(
    [DependentId] int,
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentId] int,
    [Type] int NOT NULL,
    CONSTRAINT [PK_Taken_TaakDependency] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Taken].[TaakTemplate]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [NaamFr] nvarchar(250) NOT NULL,
    [NaamNl] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Taken_TaakTemplate] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Taken].[TaakTemplateDependencie]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentId] int,
    [DependentOnId] int,
    [Type] int NOT NULL,
    CONSTRAINT [PK_Taken_TaakTemplateDependencie] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Taken].[TaakTemplateTaak]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Volgorde] int NOT NULL,
    [BinnenTermijnVan] bigint NOT NULL,
    [TaakTypeId] int NOT NULL,
    [IsDefaultUser] int,
    [IsDefaultProjectVerantwoordelijke] int,
    [IsDefaultGroep] int,
    [LockAlsVorigeNietCompleetZijn] bit NOT NULL,
    [TaakTemplateId] int NOT NULL,
    [Taaknaam] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Taken_TaakTemplateTaak] PRIMARY KEY CLUSTERED ([Id])
);
GO

CREATE TABLE [Users].[Ziekte]
(
    [Id] int IDENTITY(1,1) NOT NULL,
    [Van] datetime NOT NULL,
    [Tot] datetime NOT NULL,
    [ZiekteBriefAzureFileId] bigint,
    [UserId] int NOT NULL,
    [AgendaId] int,
    CONSTRAINT [PK_Users_Ziekte] PRIMARY KEY CLUSTERED ([Id])
);
GO

