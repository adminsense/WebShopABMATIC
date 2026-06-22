using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopABMATIC.Data.Persistence.DomainMigrations
{
    /// <inheritdoc />
    public partial class InitialDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accounting");

            migrationBuilder.EnsureSchema(
                name: "Crm");

            migrationBuilder.EnsureSchema(
                name: "Logging");

            migrationBuilder.EnsureSchema(
                name: "Settings");

            migrationBuilder.EnsureSchema(
                name: "Files");

            migrationBuilder.EnsureSchema(
                name: "Projects");

            migrationBuilder.EnsureSchema(
                name: "Customers");

            migrationBuilder.EnsureSchema(
                name: "Products");

            migrationBuilder.EnsureSchema(
                name: "Emails");

            migrationBuilder.EnsureSchema(
                name: "Users");

            migrationBuilder.EnsureSchema(
                name: "Tasks");

            migrationBuilder.CreateTable(
                name: "AccountingDocumentLines",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GateId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ProductOmschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    EenheidsPrijs = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Subtotaal = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BtwPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    InstallationPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AssemblyPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsOption = table.Column<bool>(type: "bit", nullable: false),
                    BtwBedrag = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    KortingBedrag = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    PrijslijsType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GroepNaam = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsOptieVanBestellingDetailId = table.Column<int>(type: "int", nullable: true),
                    ProductType = table.Column<int>(type: "int", nullable: true),
                    OrderLineId = table.Column<int>(type: "int", nullable: true),
                    GateComponentId = table.Column<int>(type: "int", nullable: true),
                    LeveringAfhalingOkOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NettoCommisieEenheidsPrijs = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    BestelNummer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KortingType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    BebatProductId = table.Column<int>(type: "int", nullable: true),
                    RecupelProductId = table.Column<int>(type: "int", nullable: true),
                    BebatNaam = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RecupelNaam = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BebatStukPrijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BebatAantal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BebatTotaal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecupelStukPrijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecupelAantal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecupelTotaal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontageStukPrijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssemblageStukPrijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsProducedCompositeProduct = table.Column<bool>(type: "bit", nullable: true),
                    IsVoorschot = table.Column<bool>(type: "bit", nullable: true),
                    IsTextOnly = table.Column<bool>(type: "bit", nullable: true),
                    KlaarVoorVerzendingOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NietTellenWegensVoorschot = table.Column<bool>(type: "bit", nullable: false),
                    IsGarantie = table.Column<bool>(type: "bit", nullable: false),
                    IsPopUpRow = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasisPrijsTotaal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartupCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OpstartKostTotaal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatTypeId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    DocumentDetailMasterId = table.Column<int>(type: "int", nullable: false),
                    DetailVanMasterId = table.Column<int>(type: "int", nullable: true),
                    AankoopStukPrijs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Goederen = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Diensten = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GoodsCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    AanvullendeEenheden = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    LandVanOorsprong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingDocumentLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountingDocuments",
                schema: "Accounting",
                columns: table => new
                {
                    AccountingDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentVatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DocumentNetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DocumentTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinal = table.Column<bool>(type: "bit", nullable: true),
                    DocumentCustomerBox = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DocumentCustomerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DocumentCustomerNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentCustomerPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentCustomerStreet = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DocumentCustomerCity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    DocumentCustomerCompanyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DocumentCustomerVatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentCustomerCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentCustomerHouseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Vervaldatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GecrediteerdeFactuur = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocGecrediteerdeFactuurDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectContactNaam = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ProjectContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectContactMobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectContactEmail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LeverAdresContactNaam = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LeverAdresContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LeverAdresContactMobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LeverAdresContactEmail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EindklantContactNaam = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EindklantContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EindklantContactMobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EindklantContactEmail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: true),
                    DocumentCustomerLanguageId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    LeverAdresStraat = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LeverAdresNr = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LeverAdresBus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LeverAdresStad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LeverAdresPostcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeverAdresLand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsVoorschotFactuur = table.Column<bool>(type: "bit", nullable: true),
                    ReedsGefactureerdVoorschot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VoorschotNaam = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HeeftCommisie = table.Column<bool>(type: "bit", nullable: true),
                    VoorschotPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    VerzondenVia = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DossierBeheerder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectManagerUserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountManagerUserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BetaaldOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BetalingswijzeId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToelichtingVoorschotten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseCompanyVatNumberId = table.Column<int>(type: "int", nullable: false),
                    DocKlantGebouwNaam = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    DocumentOpmerking = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    PeppolVerzondenOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EasypostId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PeppolStatus = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingDocuments", x => x.AccountingDocumentId);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppErrors",
                schema: "Logging",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    InnerExceptionMessage = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppErrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutoNumberings",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoNumberings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AzureFileFolders",
                schema: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsForCrm = table.Column<bool>(type: "bit", nullable: false),
                    IsForOrder = table.Column<bool>(type: "bit", nullable: false),
                    IsForProject = table.Column<bool>(type: "bit", nullable: false),
                    IsForProduct = table.Column<bool>(type: "bit", nullable: false),
                    IsForUser = table.Column<bool>(type: "bit", nullable: false),
                    IsForGeneralUse = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureFileFolders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AzureFiles",
                schema: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AzureFileFolderId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BlobRef = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ThumbRef = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    EmailId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedByUserId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    IsPrimaryImage = table.Column<bool>(type: "bit", nullable: true),
                    PublishToWeb = table.Column<bool>(type: "bit", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IsGeneral = table.Column<bool>(type: "bit", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true),
                    OrderLineId = table.Column<int>(type: "int", nullable: true),
                    IsLinkedRef = table.Column<bool>(type: "bit", nullable: true),
                    SendToCustomer = table.Column<bool>(type: "bit", nullable: false),
                    SendOnSupplierOrder = table.Column<bool>(type: "bit", nullable: false),
                    StockOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseCompany",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StreetNr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StreetBox = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    VatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FaxTemplate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BIC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Slogan = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AccountingDocumentFooter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MsGraphApiApplicationId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MsGraphApiSecretId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MsGraphApiDomain = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MsGraphApiTenantId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    FileShareComputername = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    FileShareUser = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    FileSharePassword = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    FileShareShare = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    FileShareAccountName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Eori = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AccentColor = table.Column<int>(type: "int", nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EmailTemplate = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AllowAddNewBlobFiles = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseCompanyAccess",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseCompanyAccess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseCompanyVatNumber",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: false),
                    VatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EoriNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bank1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Bank2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Bank1Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Bank2Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Bank1Bic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Bank2Bic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseCompanyVatNumber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillingAgreements",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    VatPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingAgreements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarEntries",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAllDay = table.Column<bool>(type: "bit", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(650)", maxLength: 650, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    LabelId = table.Column<int>(type: "int", nullable: true),
                    ReminderInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecurrenceInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutlookId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    IsSyncedToExchange = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    IsOnHold = table.Column<bool>(type: "bit", nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: true),
                    OnHoldOrCancelledReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubjectUserText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContactContactId = table.Column<int>(type: "int", nullable: true),
                    SystemDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLeave = table.Column<bool>(type: "bit", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarLabels",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    IsType = table.Column<bool>(type: "bit", nullable: false),
                    IsCategory = table.Column<bool>(type: "bit", nullable: false),
                    RestrictEditing = table.Column<bool>(type: "bit", nullable: true),
                    IsForLeave = table.Column<bool>(type: "bit", nullable: true),
                    IsInternalService = table.Column<bool>(type: "bit", nullable: true),
                    IsExternalService = table.Column<bool>(type: "bit", nullable: true),
                    IsProjectPlanning = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarLabels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarLogs",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(650)", maxLength: 650, nullable: true),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChangeAction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    ContactContactId = table.Column<int>(type: "int", nullable: true),
                    IsLeave = table.Column<bool>(type: "bit", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CalendarEntryId = table.Column<int>(type: "int", nullable: false),
                    ChangedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarStatuses",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<int>(type: "int", nullable: true),
                    ShowInTodo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                schema: "Crm",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CountryIsoCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                schema: "Customers",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactBox = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ContactFax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactHouseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactMobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactStreet = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactFirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsInstallerContact = table.Column<bool>(type: "bit", nullable: false),
                    ContactCityId = table.Column<int>(type: "int", nullable: false),
                    IsInternalUserContact = table.Column<bool>(type: "bit", nullable: false),
                    SalutationId = table.Column<int>(type: "int", nullable: true),
                    ContactLanguageId = table.Column<int>(type: "int", nullable: false),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: true),
                    InstallerDisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactJobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailQuote = table.Column<bool>(type: "bit", nullable: false),
                    EmailOrderConfirmation = table.Column<bool>(type: "bit", nullable: false),
                    EmailPlanning = table.Column<bool>(type: "bit", nullable: false),
                    EmailDeliveryReady = table.Column<bool>(type: "bit", nullable: false),
                    EmailDelivered = table.Column<bool>(type: "bit", nullable: false),
                    EmailBilling = table.Column<bool>(type: "bit", nullable: false),
                    LeftAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactBuilding = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactId);
                });

            migrationBuilder.CreateTable(
                name: "ContactProjectRoles",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameNl = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DeactivateAfterOrderClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactProjectRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEu = table.Column<bool>(type: "bit", nullable: true),
                    IsoCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerContacts",
                schema: "Customers",
                columns: table => new
                {
                    CustomerContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsDefaultContact = table.Column<bool>(type: "bit", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerContacts", x => x.CustomerContactId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCustomProductLines",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RawMaterialId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductEenheidId = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CustomerCustomProductId = table.Column<int>(type: "int", nullable: false),
                    ArticleNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCustomProductLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCustomProducts",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductEenheidId = table.Column<int>(type: "int", nullable: false),
                    ArticleNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SupplierArticleNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCustomProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCustomProductTiers",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCustomProductId = table.Column<int>(type: "int", nullable: false),
                    MinQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PieceUnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCustomProductTiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDeliveredProducts",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedByUserId = table.Column<int>(type: "int", nullable: false),
                    BroughtBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PickedUpAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PickedUpBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SentToSupplierAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ReturnedFromSupplierAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IrreparableAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDeliveredProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDeliveryAddresses",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Straat = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ContactId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDeliveryAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerExtraDiscounts",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerTypeFilterId = table.Column<int>(type: "int", nullable: false),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerExtraDiscounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFollowUps",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFollowUps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerJobCodeRates",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    JobCodeId = table.Column<int>(type: "int", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerJobCodeRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerNotes",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderStatusRemarks",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderStatusRemarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProductDiscounts",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FromAddress = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Margin = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    InstallationDiscountPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    InstallationCustomerTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProductDiscounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerVatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerBox = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerHouseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CustomerStreet = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CustomerTypeId = table.Column<int>(type: "int", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerFax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerNotes = table.Column<string>(type: "nvarchar(2024)", maxLength: 2024, nullable: true),
                    CustomerEmail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CustomerVatSystemId = table.Column<int>(type: "int", nullable: false),
                    CustomerStatusId = table.Column<int>(type: "int", nullable: false),
                    CustomerCityId = table.Column<int>(type: "int", nullable: false),
                    CustomerActivityId = table.Column<int>(type: "int", nullable: true),
                    CustomerNumber = table.Column<int>(type: "int", nullable: true),
                    AccountManagerUserId = table.Column<int>(type: "int", nullable: false),
                    FirstContactName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LockedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    LockedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RevenueLast12Months = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SendPriceListByEmail = table.Column<bool>(type: "bit", nullable: false),
                    PromotionalMailing = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryTypeId = table.Column<int>(type: "int", nullable: false),
                    InvoicesByMail = table.Column<bool>(type: "bit", nullable: false),
                    CustomerInternalNotes = table.Column<string>(type: "nvarchar(2024)", maxLength: 2024, nullable: true),
                    DigitalInvoicing = table.Column<bool>(type: "bit", nullable: false),
                    CElabelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CElabelNr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerPaymentStatus = table.Column<int>(type: "int", nullable: true),
                    BaseCompaniesId = table.Column<int>(type: "int", nullable: true),
                    IsInternalCompany = table.Column<bool>(type: "bit", nullable: true),
                    CustomerLanguageId = table.Column<int>(type: "int", nullable: false),
                    PriceListResContractor = table.Column<bool>(type: "bit", nullable: true),
                    PriceListResDealer = table.Column<bool>(type: "bit", nullable: true),
                    PriceListResConsumer = table.Column<bool>(type: "bit", nullable: true),
                    PriceListIndContractor = table.Column<bool>(type: "bit", nullable: true),
                    PriceListIndDealer = table.Column<bool>(type: "bit", nullable: true),
                    CEemail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CustomerGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true),
                    QuoteContactId = table.Column<int>(type: "int", nullable: false),
                    OrderConfirmationContactId = table.Column<int>(type: "int", nullable: false),
                    PlanningContactId = table.Column<int>(type: "int", nullable: false),
                    DeliveryCompleteContactId = table.Column<int>(type: "int", nullable: false),
                    BillingContactId = table.Column<int>(type: "int", nullable: false),
                    CommissionRecipientId = table.Column<int>(type: "int", nullable: true),
                    RequestedCommission = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BetaaltermijnId = table.Column<int>(type: "int", nullable: false),
                    WebshopLogin = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    WebshopPasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    WebshopPasswordSalt = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CustomerBuildingName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LaatsteFollowUp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryCustomerTypeId = table.Column<int>(type: "int", nullable: false),
                    PeppolIdSchema = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    PeppolId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerStatuses",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSupplierDiscounts",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FromAddress = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InstallationCustomerTypeId = table.Column<int>(type: "int", nullable: true),
                    InstallationDiscountPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSupplierDiscounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                schema: "Customers",
                columns: table => new
                {
                    KlantTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequiresVatNumber = table.Column<bool>(type: "bit", nullable: false),
                    PaymentTermId = table.Column<int>(type: "int", nullable: false),
                    VatSystemId = table.Column<int>(type: "int", nullable: false),
                    BaseDiscount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DeliveryTypeId = table.Column<int>(type: "int", nullable: false),
                    CustomerTypeNameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.KlantTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTypes",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IncludeInstallationCost = table.Column<bool>(type: "bit", nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplates",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EmailTemplate = table.Column<bool>(type: "bit", nullable: false),
                    FaxTemplate = table.Column<bool>(type: "bit", nullable: false),
                    LetterTemplate = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: true),
                    TaalId = table.Column<int>(type: "int", nullable: false),
                    AzureFileId = table.Column<long>(type: "bigint", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ParameterId = table.Column<int>(type: "int", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrawGroup",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailAttachments",
                schema: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoredFileId = table.Column<int>(type: "int", nullable: false),
                    EmailId = table.Column<int>(type: "int", nullable: false),
                    EmailFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsEmailOnlyFile = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailMessages",
                schema: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    FromAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ToAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Cc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Bcc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactId = table.Column<int>(type: "int", nullable: true),
                    RelatedSupplierId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    PreviewText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TaskItemId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true),
                    OrderLineId = table.Column<int>(type: "int", nullable: true),
                    RequiresAction = table.Column<bool>(type: "bit", nullable: true),
                    EmailQueueId = table.Column<int>(type: "int", nullable: true),
                    StockOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailQueues",
                schema: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GridLayout",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LayoutXml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UsrId = table.Column<int>(type: "int", nullable: true),
                    IsPivot = table.Column<bool>(type: "bit", nullable: true),
                    PivotName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridLayout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntrastatCode",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainGroup = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubGroup = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntrastatCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntrastatReportLines",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentDocId = table.Column<int>(type: "int", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PartnerLand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactieCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gewest = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GoodsCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    AanvullendeEenheden = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WaardeInEur = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Vervoer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Incoterm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LandVanOorsprong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BtwNrTegenpartij = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntrastatReportLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCode",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NameNl = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsBillable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobSites",
                schema: "Projects",
                columns: table => new
                {
                    JobSiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteStreet = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SiteBox = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SiteHouseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SiteCityId = table.Column<int>(type: "int", nullable: false),
                    EndCustomerEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EndCustomerMobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EndCustomerName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EndCustomerPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SiteNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSites", x => x.JobSiteId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageTags",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceKey = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    En = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceContractLines",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OnderhoudsContractId = table.Column<int>(type: "int", nullable: false),
                    ProjectInstallatieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceContractLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceContracts",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FromAddress = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AutoRenew = table.Column<bool>(type: "bit", nullable: false),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CycleDays = table.Column<int>(type: "int", nullable: false),
                    ReminderDaysInAdvance = table.Column<int>(type: "int", nullable: false),
                    VatTypeId = table.Column<int>(type: "int", nullable: false),
                    SuccessorUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceContracts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                schema: "Crm",
                columns: table => new
                {
                    ManufacturerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CompanyRegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.ManufacturerId);
                });

            migrationBuilder.CreateTable(
                name: "MiscellaneousProducts",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ArticleNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StockLocationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AantalInStock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastCountedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscellaneousProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderAdvancePayments",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Percent = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsFinalInvoice = table.Column<bool>(type: "bit", nullable: false),
                    InvoicedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    AdvancePaymentVisibility = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAdvancePayments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderDeliveryTypeProducts",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeveringTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDeliveryTypeProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderDevelopmentLines",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArticleNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    MasterRowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    ProductEenheidId = table.Column<int>(type: "int", nullable: true),
                    ReservedQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityTakenFromStock = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MustOrder = table.Column<bool>(type: "bit", nullable: false),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPurchasePrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    DrawingCreated = table.Column<bool>(type: "bit", nullable: false),
                    MaterialType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Processing = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Finish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockOrderDetailId = table.Column<int>(type: "int", nullable: true),
                    RequiresPainting = table.Column<bool>(type: "bit", nullable: true),
                    PaintWorkSupplierId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDevelopmentLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderFeedbacks",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Bucket = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderFeedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderFileLinks",
                schema: "Files",
                columns: table => new
                {
                    OrderFileLinkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoredFileId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderFileLinks", x => x.OrderFileLinkId);
                });

            migrationBuilder.CreateTable(
                name: "OrderInstallationLines",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Material = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityInInstallation = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Processing = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    SupplierArticleNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Treatment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInstallationLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    GateId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AssemblyPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InstallationPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalExclVat = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalInclVat = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsOption = table.Column<bool>(type: "bit", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Btw = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UpliftType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentDisplayName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Lengte = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Hoogte = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Breedte = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Assemblage = table.Column<bool>(type: "bit", nullable: false),
                    Montage = table.Column<bool>(type: "bit", nullable: false),
                    QuoteNotesHeader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MontageStukprijs = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AssemblageStukprijs = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductOptionId = table.Column<int>(type: "int", nullable: true),
                    ProductOptionHoofdDetaillId = table.Column<int>(type: "int", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProductionGroupId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AantalProductieVerwerkt = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    AantalAfgehaaldOfGeleverd = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    QuantityInvoiced = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Afgehandeld = table.Column<bool>(type: "bit", nullable: true),
                    PurchaseOrderNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PopUpDataXml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    InProductie = table.Column<bool>(type: "bit", nullable: true),
                    Controleren = table.Column<bool>(type: "bit", nullable: true),
                    ProductieKlaar = table.Column<bool>(type: "bit", nullable: true),
                    LeveringOfAfhalingKlaar = table.Column<bool>(type: "bit", nullable: true),
                    MontageKlaar = table.Column<bool>(type: "bit", nullable: true),
                    Gefactureerd = table.Column<bool>(type: "bit", nullable: true),
                    Tefactureren = table.Column<bool>(type: "bit", nullable: true),
                    ProductieVerwerkOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadyForBillingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BtwBedrag = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Tecrediteren = table.Column<bool>(type: "bit", nullable: true),
                    GecrediteerdOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToOrder = table.Column<bool>(type: "bit", nullable: true),
                    Ordered = table.Column<bool>(type: "bit", nullable: true),
                    OrderedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentDisplayNameFr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NettoCommisieEenheidsPrijs = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    IsSubProduct = table.Column<bool>(type: "bit", nullable: true),
                    AantalSubProduct = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BestelNummer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductEenheidId = table.Column<int>(type: "int", nullable: true),
                    InvoicedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrijsControleren = table.Column<bool>(type: "bit", nullable: true),
                    AantalTeBestellen = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductStockLocatieId = table.Column<int>(type: "int", nullable: true),
                    AantalInStockGereserveerd = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    AantalDefinitiefUitStock = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    NoPriceCacl = table.Column<bool>(type: "bit", nullable: true),
                    AantalBeschikbaarInStock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsSamenGesteldProduct = table.Column<bool>(type: "bit", nullable: true),
                    ReportingGroupId = table.Column<int>(type: "int", nullable: false),
                    BrutoAankoopprijs = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    NettoAankoopPrijs = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    WinstPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StockOrderDetailId = table.Column<int>(type: "int", nullable: true),
                    QuantityOrdered = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArticleNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OrigineleKorting = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OverRuleLosseLijnAutoKorting = table.Column<bool>(type: "bit", nullable: true),
                    IsTextOnly = table.Column<bool>(type: "bit", nullable: true),
                    UpliftTypeOrigineel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BebatNaam = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BebatProductId = table.Column<int>(type: "int", nullable: true),
                    RecupelNaam = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RecupelProductId = table.Column<int>(type: "int", nullable: true),
                    BebatAantal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BebatStukPrijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BebatTotaal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecupelAantal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    RecupelStukPrijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecupelTotaal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsProducedCompositeProduct = table.Column<bool>(type: "bit", nullable: true),
                    IsLeveringsTypeProduct = table.Column<bool>(type: "bit", nullable: true),
                    StandaardKortingsType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StandaardKortingsPecentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsGarantie = table.Column<bool>(type: "bit", nullable: false),
                    TotaalExclVoorCommissie = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PopUpRow = table.Column<bool>(type: "bit", nullable: false),
                    NodeLevel = table.Column<int>(type: "int", nullable: true),
                    AfgehandeldOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Selected = table.Column<bool>(type: "bit", nullable: false),
                    IsExtraKortingRow = table.Column<bool>(type: "bit", nullable: true),
                    BestellingIsBinnenGekomen = table.Column<bool>(type: "bit", nullable: true),
                    UnitParameter = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BasisPrijsTellen = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnQuote = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnOrderConfirmation = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnInvoice = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnPackingSlip = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnDeliveryNote = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnProductionOrder = table.Column<bool>(type: "bit", nullable: false),
                    ToonOpLakBon = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnInstallationOrder = table.Column<bool>(type: "bit", nullable: false),
                    Ral = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ToonOmschrijvingOpFactuur = table.Column<bool>(type: "bit", nullable: false),
                    ToonOmschrijvingOpPakbon = table.Column<bool>(type: "bit", nullable: false),
                    ToonOmschrijvingOpLeverbon = table.Column<bool>(type: "bit", nullable: false),
                    ToonOmschrijvingOpProductiebon = table.Column<bool>(type: "bit", nullable: false),
                    ToonOmschrijvingOpLakBon = table.Column<bool>(type: "bit", nullable: false),
                    ToonOmschrijvingOpOfferte = table.Column<bool>(type: "bit", nullable: false),
                    ToonOpVrachtbrief = table.Column<bool>(type: "bit", nullable: false),
                    ToonOmschrijvingOpVrachtbrief = table.Column<bool>(type: "bit", nullable: false),
                    ToonOmschrijvingOpOrderbevestiging = table.Column<bool>(type: "bit", nullable: false),
                    StartupCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpstartKostTotaal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BasisPrijsTotaal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatTypeId = table.Column<int>(type: "int", nullable: false),
                    OrderTemplateId = table.Column<int>(type: "int", nullable: true),
                    OrderTemplateDetailId = table.Column<int>(type: "int", nullable: true),
                    PieceUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerCustomProductId = table.Column<int>(type: "int", nullable: true),
                    TaxproductenGecontroleerd = table.Column<bool>(type: "bit", nullable: true),
                    ReportRecupel = table.Column<bool>(type: "bit", nullable: false),
                    ReportBebat = table.Column<bool>(type: "bit", nullable: false),
                    ToonGeenDetailPrijzen = table.Column<bool>(type: "bit", nullable: true),
                    OfferteDocumentDocId = table.Column<int>(type: "int", nullable: true),
                    OrderBevestigingDocumentDocId = table.Column<int>(type: "int", nullable: true),
                    PakBonDocumentDocId = table.Column<int>(type: "int", nullable: true),
                    LeveringsBonDocId = table.Column<int>(type: "int", nullable: true),
                    FactuurDocId = table.Column<int>(type: "int", nullable: true),
                    IsProduction = table.Column<bool>(type: "bit", nullable: true),
                    OmschrijvingProbleem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KlantBinnenGebrachtOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KlantTerugOpgehaaldOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VanKlantInOntvangstGenomenDoor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OmschrijvingHerstelling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentToSupplierAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OntvangenVanLeverancierOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrackingHerstelling = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsHerstelling = table.Column<bool>(type: "bit", nullable: true),
                    NieuwVervangToestelGegevenOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HerstelUren = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HerstellingAfgerondOp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HerstellingsKostMateriaal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HerstellingUurtarief = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HerstellingsKostTotaal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KortingOverride = table.Column<bool>(type: "bit", nullable: true),
                    PrijsTypeOverride = table.Column<bool>(type: "bit", nullable: true),
                    InterneHerstelling = table.Column<bool>(type: "bit", nullable: true),
                    AdvancePaymentVisibility = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Goederen = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Diensten = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GoodsCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    AanvullendeEenheden = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    LandVanOorsprong = table.Column<int>(type: "int", nullable: true),
                    IntrastatReported = table.Column<bool>(type: "bit", nullable: true),
                    IntrastatReportedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderLineTexts",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderBevestiging = table.Column<bool>(type: "bit", nullable: false),
                    Leveringsbon = table.Column<bool>(type: "bit", nullable: false),
                    Factuur = table.Column<bool>(type: "bit", nullable: false),
                    AkOrder = table.Column<bool>(type: "bit", nullable: false),
                    Offerte = table.Column<bool>(type: "bit", nullable: false),
                    DossierDetailsId = table.Column<int>(type: "int", nullable: true),
                    MontageBon = table.Column<bool>(type: "bit", nullable: false),
                    Lakbon = table.Column<bool>(type: "bit", nullable: false),
                    ProductieBon = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderLogs",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProcessingTypes",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CalculatePrice = table.Column<bool>(type: "bit", nullable: false),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProcessingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProjectLines",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArticleNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    MasterRowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    ProductEenheidId = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityTakenFromStock = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MustOrder = table.Column<bool>(type: "bit", nullable: false),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPurchasePrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    DrawingCreated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProjectLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderRemarks",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CustomerNoteId = table.Column<int>(type: "int", nullable: true),
                    ProductionCategories = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionCategoryId = table.Column<int>(type: "int", nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRemarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    GeneralDiscount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DeliveryTypeId = table.Column<int>(type: "int", nullable: false),
                    PriceListTypeId = table.Column<int>(type: "int", nullable: false),
                    QuoteId = table.Column<int>(type: "int", nullable: true),
                    OrderNumber = table.Column<int>(type: "int", nullable: true),
                    CommissionAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InstallerContactId = table.Column<int>(type: "int", nullable: true),
                    RequestedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InstallationDays = table.Column<int>(type: "int", nullable: true),
                    ProductionDays = table.Column<int>(type: "int", nullable: true),
                    MasterOrderId = table.Column<int>(type: "int", nullable: true),
                    VatTypeId = table.Column<int>(type: "int", nullable: false),
                    OrderProcessingTypeId = table.Column<int>(type: "int", nullable: false),
                    CustomerTypeId = table.Column<int>(type: "int", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RalColor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AdvanceInvoiceEnabled = table.Column<bool>(type: "bit", nullable: true),
                    ExtraDiscount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CustomerNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstallerNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalStaffNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowPartialDelivery = table.Column<bool>(type: "bit", nullable: false),
                    CommissionSalesUserId = table.Column<int>(type: "int", nullable: true),
                    IsCommissionInvoiced = table.Column<bool>(type: "bit", nullable: true),
                    CommissionToInvoice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeveradresId = table.Column<int>(type: "int", nullable: true),
                    BetaaltermijnId = table.Column<int>(type: "int", nullable: false),
                    PopupMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteValidDays = table.Column<int>(type: "int", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    PriceListDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BaseCompanyVatNumberId = table.Column<int>(type: "int", nullable: false),
                    AdvancePaymentsByAmount = table.Column<bool>(type: "bit", nullable: true),
                    HasCloudFolder = table.Column<bool>(type: "bit", nullable: true),
                    IsClosingVerified = table.Column<bool>(type: "bit", nullable: true),
                    InvoiceNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteNotesHeader = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusAccesses",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusAccesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    IncludeInSalesReporting = table.Column<bool>(type: "bit", nullable: true),
                    NameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReportInProgress = table.Column<bool>(type: "bit", nullable: true),
                    ScreenMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderStatusGroupId = table.Column<int>(type: "int", nullable: true),
                    ReserveStock = table.Column<bool>(type: "bit", nullable: false),
                    AffectsStock = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusGroups",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStructures",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MasterRowId = table.Column<int>(type: "int", nullable: true),
                    OrderTypeId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CustomerReference = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    OrderConfirmationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadyForDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadyForBillingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuoteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalInvoiced = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TeamsId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OrderConfirmedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlannedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReadyForDeliveryBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QuoteBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReadyForBillingBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HasPaperFile = table.Column<bool>(type: "bit", nullable: false),
                    QueuedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStructures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTemplate",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTemplateDetail",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityFormula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OrderTemplateId = table.Column<int>(type: "int", nullable: false),
                    PriceFormula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductEenheidId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTemplateDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTypes",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SerialNumberSuffix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderProcessingTypeId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameNl = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsPrePay = table.Column<bool>(type: "bit", nullable: false),
                    IsPostPay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTerms",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AantalDagen = table.Column<int>(type: "int", nullable: false),
                    EndOfMonth = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceListCategories",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    HasOptions = table.Column<bool>(type: "bit", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NameFr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceListTexts",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextFr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameNl = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescriptionNl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderPartNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StockNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: true),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    UnitsPerSale = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitsPerPurchase = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PriceListSortOrder = table.Column<int>(type: "int", nullable: true),
                    ShowOnPriceList = table.Column<bool>(type: "bit", nullable: false),
                    ShortNotesNl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProdToonOmschrijvingPrijslijst = table.Column<bool>(type: "bit", nullable: false),
                    RecupelAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    BebatAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ProdRalKleur = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ProdKleurPoedercode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MinimumQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomWorkPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    RecupelProductId = table.Column<int>(type: "int", nullable: true),
                    BebatProductId = table.Column<int>(type: "int", nullable: true),
                    IsQuickLooseSaleOption = table.Column<bool>(type: "bit", nullable: true),
                    NameFr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescriptionFr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortNotesFr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TaskTemplateId = table.Column<int>(type: "int", nullable: true),
                    PurchaseUnitId = table.Column<int>(type: "int", nullable: true),
                    SalesUnitId = table.Column<int>(type: "int", nullable: true),
                    AdsolutId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortNotesEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsCompositeProduct = table.Column<bool>(type: "bit", nullable: true),
                    ProductStructureId = table.Column<int>(type: "int", nullable: true),
                    TemporaryDiscount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    TemporaryNetPurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReportingGroupId = table.Column<int>(type: "int", nullable: true),
                    SalesStockTriggerQuantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    ExtraPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ExtraAssemblyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtraInstallationPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsProducedCompositeProduct = table.Column<bool>(type: "bit", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true),
                    HasNoPrice = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnQuote = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnOrderConfirmation = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnInvoice = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnPackingSlip = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnDeliveryNote = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnProductionOrder = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnPaintShopOrder = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnInstallationOrder = table.Column<bool>(type: "bit", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    InternalDocumentNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExternalInstallerCost = table.Column<bool>(type: "bit", nullable: false),
                    ReportRecupel = table.Column<bool>(type: "bit", nullable: false),
                    ReportBebat = table.Column<bool>(type: "bit", nullable: false),
                    HasTierPricing = table.Column<bool>(type: "bit", nullable: true),
                    HideDetailPrice = table.Column<bool>(type: "bit", nullable: true),
                    ShowOnWebshop = table.Column<bool>(type: "bit", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsNew = table.Column<bool>(type: "bit", nullable: true),
                    EanCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PopupMessage = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    WebshopDescriptionNl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoodsCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IntrastatCodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ProductDiscountSuggestionLines",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductKortingSuggestieId = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscountSuggestionLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductDiscountSuggestions",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrossCorrection = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Pro1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Pro2 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Pro3 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Aan1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Aan2 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Par1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DiscountCap = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Ond1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscountSuggestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductManuals",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ShowOnWeb = table.Column<bool>(type: "bit", nullable: true),
                    SendAutomatically = table.Column<bool>(type: "bit", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductManuals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptions",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ValueType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductionNotesFlag = table.Column<bool>(type: "bit", nullable: false),
                    QuoteNotesFlag = table.Column<bool>(type: "bit", nullable: false),
                    CalculatePrice = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsQuantityLine = table.Column<bool>(type: "bit", nullable: true),
                    NameFr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DefaultValueFormula = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    QuantityFormula = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ExtraPriceFormula = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    UnitParameterFormula = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionValue",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptieProduct = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductOptionId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ValueFr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValueEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPopupReturnColumns",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameNl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LooseSaleColumn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GateComponentColumn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPopupReturnColumns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPopupTemplate",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPopupTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPopupTemplateLines",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameNl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransferToQuantityFormula = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TransferToQuantity = table.Column<bool>(type: "bit", nullable: false),
                    IncludePrice = table.Column<bool>(type: "bit", nullable: false),
                    WriteToLineColumn = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPopupTemplateLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPopupValueTypes",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DescriptionFr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPopupValueTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromAddress = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssemblyPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InstallationPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    GrossSalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossPurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductAankoopKortingenId = table.Column<int>(type: "int", nullable: true),
                    OverrideBruto = table.Column<bool>(type: "bit", nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CorrectedGrossPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceCalculationFormula = table.Column<string>(type: "nvarchar(2024)", maxLength: 2024, nullable: true),
                    UsePriceCalculationFormula = table.Column<bool>(type: "bit", nullable: true),
                    BasePriceCalculationFormula = table.Column<string>(type: "nvarchar(2024)", maxLength: 2024, nullable: true),
                    StartupCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Pro1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Pro2 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Pro3 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Aan1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Aan2 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Par1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Ond = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ExtraPurchaseCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtraPurchaseCostNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PurchaseDiscountPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    GrossCorrectionPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CalculationType = table.Column<int>(type: "int", nullable: false),
                    SupplierUsesDifferentGrossSalesPrice = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPriceSalesDiscounts",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlantTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductPrijzenId = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPriceSalesDiscounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductionGroup",
                schema: "Products",
                columns: table => new
                {
                    ProductProductionGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductionGroup", x => x.ProductProductionGroupId);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductionGroupLinks",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductionGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductionGroupLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductProperty",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NameNl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProperty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPropertyItems",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductPropertyId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPropertyItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPurchaseDiscounts",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Percentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Pro1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Pro2 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Pro3 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Aan1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Aan2 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Par1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FromAddress = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ond = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPurchaseDiscounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductQuantityTiers",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinimumQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQuantityTiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductStockLocations",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockLocationId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsInactive = table.Column<bool>(type: "bit", nullable: true),
                    MaxQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    MinQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ReservedQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LastCountedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountedQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStockLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductStructures",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ParentTaskId = table.Column<int>(type: "int", nullable: true),
                    NameNl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IntroPriceListTextId = table.Column<int>(type: "int", nullable: true),
                    OutroPriceListTextId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: true),
                    ShowOnPriceList = table.Column<bool>(type: "bit", nullable: true),
                    Icon = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PageBreakAfter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStructures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductSubProduct",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterProductId = table.Column<int>(type: "int", nullable: false),
                    SubProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false),
                    ExtraBasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductType",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsStockItem = table.Column<bool>(type: "bit", nullable: false),
                    IsProduction = table.Column<bool>(type: "bit", nullable: false),
                    IsPurchaseItem = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductUnits",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameNl = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UnitParameter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectNumber = table.Column<int>(type: "int", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProjectManagerUserId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    JobSiteId = table.Column<int>(type: "int", nullable: true),
                    ProjectTypeId = table.Column<int>(type: "int", nullable: false),
                    ProjectCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectInternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: true),
                    ProductionLabelReference = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    IsTemplate = table.Column<bool>(type: "bit", nullable: true),
                    ProjectNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsStandardProject = table.Column<bool>(type: "bit", nullable: false),
                    TeamsId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PopupMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectActivities",
                schema: "Logging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ActionCode = table.Column<int>(type: "int", nullable: false),
                    LoggedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectContacts",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ContactProjectRolId = table.Column<int>(type: "int", nullable: false),
                    LinkedContactId = table.Column<int>(type: "int", nullable: false),
                    ContactContactId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectInstallations",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstallationOrderId = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Specifications = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectInstallations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectLog",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ActionCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectParties",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ProjectPartijGroepId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BillingPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectParties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPartyContacts",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerContactId = table.Column<int>(type: "int", nullable: false),
                    ProjectPartijId = table.Column<int>(type: "int", nullable: false),
                    EmailQuote = table.Column<bool>(type: "bit", nullable: false),
                    EmailOrderConfirmation = table.Column<bool>(type: "bit", nullable: false),
                    EmailPlanning = table.Column<bool>(type: "bit", nullable: false),
                    EmailDeliveryReady = table.Column<bool>(type: "bit", nullable: false),
                    EmailDelivered = table.Column<bool>(type: "bit", nullable: false),
                    EmailBilling = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPartyContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPartyGroups",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameNl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsForSupplier = table.Column<bool>(type: "bit", nullable: false),
                    IsForCustomer = table.Column<bool>(type: "bit", nullable: false),
                    IsForManufacturer = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPartyGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RawMaterials",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairCostPrices",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FromAddress = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairCostPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportingGroups",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportingGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salutations",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalutationText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsMale = table.Column<bool>(type: "bit", nullable: false),
                    IsFemale = table.Column<bool>(type: "bit", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salutations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRates",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SickLeaves",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromAddress = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SickNoteAzureFileId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CalendarEntryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SickLeaves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffUsers",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Crm = table.Column<bool>(type: "bit", nullable: false),
                    Offerte = table.Column<bool>(type: "bit", nullable: false),
                    Bestellingen = table.Column<bool>(type: "bit", nullable: false),
                    Productie = table.Column<bool>(type: "bit", nullable: false),
                    Boekhouding = table.Column<bool>(type: "bit", nullable: false),
                    Planning = table.Column<bool>(type: "bit", nullable: false),
                    Admin = table.Column<bool>(type: "bit", nullable: false),
                    Verkoper = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    ExchangeLastWatermark = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DoSyncExchange = table.Column<bool>(type: "bit", nullable: false),
                    RechtstreeksGsmNrtonen = table.Column<bool>(type: "bit", nullable: false),
                    RechtstreeksTelefoonNrTonen = table.Column<bool>(type: "bit", nullable: false),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserGroupId = table.Column<int>(type: "int", nullable: true),
                    DirecteLosseVerkoop = table.Column<bool>(type: "bit", nullable: false),
                    ReportSales = table.Column<bool>(type: "bit", nullable: true),
                    DefaultCeLabelPrinter = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DefaultProductionLabelPrinter = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailTemplate = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FaxTemplate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Gsm = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BaseCompaniesId = table.Column<int>(type: "int", nullable: false),
                    TaalId = table.Column<int>(type: "int", nullable: false),
                    Ice1Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ice1Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Ice2Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ice2Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PrivateEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Hr = table.Column<bool>(type: "bit", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    HiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeftAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductBeheer = table.Column<bool>(type: "bit", nullable: false),
                    PlanningBinnenDienst = table.Column<bool>(type: "bit", nullable: false),
                    PlanningBuitendienst = table.Column<bool>(type: "bit", nullable: false),
                    PlanningProjecten = table.Column<bool>(type: "bit", nullable: false),
                    CompanyPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyMobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ToonInPrijslijst = table.Column<bool>(type: "bit", nullable: true),
                    SelectForInternalPlanning = table.Column<bool>(type: "bit", nullable: true),
                    SelectForExternalPlanning = table.Column<bool>(type: "bit", nullable: true),
                    SelectForProjectPlanning = table.Column<bool>(type: "bit", nullable: true),
                    SelectForLeavePlanning = table.Column<bool>(type: "bit", nullable: true),
                    DefaultPlanningLabelId = table.Column<int>(type: "int", nullable: true),
                    TextColor = table.Column<int>(type: "int", nullable: false),
                    CanAccessRevenueReports = table.Column<bool>(type: "bit", nullable: true),
                    CanAccessProfitReports = table.Column<bool>(type: "bit", nullable: true),
                    CanAccessDmsSpecial = table.Column<bool>(type: "bit", nullable: true),
                    CanAccessBulkOrders = table.Column<bool>(type: "bit", nullable: true),
                    CanAccessStockManagement = table.Column<bool>(type: "bit", nullable: true),
                    CanOrderFromOrderScreen = table.Column<bool>(type: "bit", nullable: true),
                    ShowProjects = table.Column<bool>(type: "bit", nullable: true),
                    CanAccessBilling = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StandardBillingTermLines",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StdFacturatieVoorwaardenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardBillingTermLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StandardBillingTerms",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardBillingTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockLocations",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsWarehouse = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockMovements",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OrderLineId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsReservation = table.Column<bool>(type: "bit", nullable: true),
                    ProductStockLocatieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockOrder",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderConfirmationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockOrderDeliveries",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockOrderDetail = table.Column<int>(type: "int", nullable: false),
                    DeliveryDocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityInvoiced = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOrderDeliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockOrderLines",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    QuantityOrdered = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LijnOK = table.Column<bool>(type: "bit", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PackSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PurchaseUnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PurchaseTotalPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InternalReference = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DeliveryNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OrderNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Besteld = table.Column<bool>(type: "bit", nullable: true),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Geleverd = table.Column<bool>(type: "bit", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductTypeId = table.Column<int>(type: "int", nullable: true),
                    QuantityDelivered = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityProcessedToStock = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOrderLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoredFiles",
                schema: "Files",
                columns: table => new
                {
                    StoredFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFiles", x => x.StoredFileId);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "Crm",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CompanyRegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SupplierOrderEmail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    GeneralLedgerRevenueAccount = table.Column<int>(type: "int", nullable: false),
                    IsMainSupplier = table.Column<bool>(type: "bit", nullable: true),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true),
                    PriceListSortOrder = table.Column<int>(type: "int", nullable: true),
                    PriceListName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "SupplierContacts",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true),
                    ContactFunctionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskActions",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskItemId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskDependencies",
                schema: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DependentTaskId = table.Column<int>(type: "int", nullable: true),
                    ParentTaskId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDependencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AssignedUserId = table.Column<int>(type: "int", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    OrderLineId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    BaseCompanyId = table.Column<int>(type: "int", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PercentComplete = table.Column<int>(type: "int", nullable: true),
                    UserGroupId = table.Column<int>(type: "int", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedByCreatorAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true),
                    PopupShown = table.Column<bool>(type: "bit", nullable: true),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRejectionRead = table.Column<bool>(type: "bit", nullable: true),
                    IsRejected = table.Column<bool>(type: "bit", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskTemplateDependencies",
                schema: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentTaskId = table.Column<int>(type: "int", nullable: true),
                    DependsOnTaskTemplateTaskId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTemplateDependencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskTemplates",
                schema: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameFr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NameNl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskTemplateTasks",
                schema: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    DueWithinTicks = table.Column<long>(type: "bigint", nullable: false),
                    TaakTypeId = table.Column<int>(type: "int", nullable: false),
                    DefaultUserId = table.Column<int>(type: "int", nullable: true),
                    UseProjectOwnerAsDefault = table.Column<int>(type: "int", nullable: true),
                    DefaultUserGroupId = table.Column<int>(type: "int", nullable: true),
                    LockUntilPreviousComplete = table.Column<bool>(type: "bit", nullable: false),
                    TaakTemplateId = table.Column<int>(type: "int", nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTemplateTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypes",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompleteWithinDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: true),
                    ProductionWarning = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryInstallationWarning = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateType",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentTypeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timesheet",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    StartDt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StopDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMinutes = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    DurationHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    JobCodeId = table.Column<int>(type: "int", nullable: false),
                    IsBillable = table.Column<bool>(type: "bit", nullable: false),
                    BillableMinutes = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BillableHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrestantUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsTimerRunning = table.Column<bool>(type: "bit", nullable: false),
                    TimerStartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimerLastValue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsInstallationTeam = table.Column<bool>(type: "bit", nullable: false),
                    IsServiceTeam = table.Column<bool>(type: "bit", nullable: false),
                    IsTransportTeam = table.Column<bool>(type: "bit", nullable: false),
                    OrderStatusGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VatTypes",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    InvoiceText = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceTextEn = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    InvoiceTextFr = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ExplanationNl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExplanationFr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExplanationEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true),
                    TaxExemptionReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxExemptionReasonCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PeppolCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebshopProductStructures",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NameFr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NameNl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ParentTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebshopProductStructures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebshopStructures",
                schema: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameNl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentTaskId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebshopStructures", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingDocumentLines",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "AccountingDocuments",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Activities",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "AppErrors",
                schema: "Logging");

            migrationBuilder.DropTable(
                name: "AppSettings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "AutoNumberings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "AzureFileFolders",
                schema: "Files");

            migrationBuilder.DropTable(
                name: "AzureFiles",
                schema: "Files");

            migrationBuilder.DropTable(
                name: "BaseCompany",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "BaseCompanyAccess",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "BaseCompanyVatNumber",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "BillingAgreements",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "CalendarEntries",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CalendarLabels",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CalendarLogs",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CalendarStatuses",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "City",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "Contact",
                schema: "Customers");

            migrationBuilder.DropTable(
                name: "ContactProjectRoles",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerContacts",
                schema: "Customers");

            migrationBuilder.DropTable(
                name: "CustomerCustomProductLines",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerCustomProducts",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerCustomProductTiers",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerDeliveredProducts",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "CustomerDeliveryAddresses",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerExtraDiscounts",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "CustomerFollowUps",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerJobCodeRates",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerNotes",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerOrderStatusRemarks",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerProductDiscounts",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "Customers");

            migrationBuilder.DropTable(
                name: "CustomerStatuses",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerSupplierDiscounts",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "CustomerTypes",
                schema: "Customers");

            migrationBuilder.DropTable(
                name: "DeliveryTypes",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "DocumentTemplates",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "DocumentTypes",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "DrawGroup",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "EmailAttachments",
                schema: "Emails");

            migrationBuilder.DropTable(
                name: "EmailMessages",
                schema: "Emails");

            migrationBuilder.DropTable(
                name: "EmailQueues",
                schema: "Emails");

            migrationBuilder.DropTable(
                name: "GridLayout",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "IntrastatCode",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "IntrastatReportLines",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "JobCode",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "JobSites",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "LanguageTags",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "MaintenanceContractLines",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "MaintenanceContracts",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "Manufacturer",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "MiscellaneousProducts",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "OrderAdvancePayments",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderDeliveryTypeProducts",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderDevelopmentLines",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderFeedbacks",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderFileLinks",
                schema: "Files");

            migrationBuilder.DropTable(
                name: "OrderInstallationLines",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderLines",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderLineTexts",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderLogs",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderProcessingTypes",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderProjectLines",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderRemarks",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderStatusAccesses",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderStatuses",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderStatusGroups",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderStructures",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "OrderTemplate",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "OrderTemplateDetail",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "OrderTypes",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "PaymentTerms",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "PriceListCategories",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "PriceListTexts",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductDiscountSuggestionLines",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "ProductDiscountSuggestions",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "ProductManuals",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductOptions",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductOptionValue",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductPopupReturnColumns",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductPopupTemplate",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductPopupTemplateLines",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductPopupValueTypes",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductPrices",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductPriceSalesDiscounts",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductProductionGroup",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductProductionGroupLinks",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductProperty",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductPropertyItems",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductPurchaseDiscounts",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductQuantityTiers",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductStockLocations",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductStructures",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductSubProduct",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductType",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "ProductUnits",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectActivities",
                schema: "Logging");

            migrationBuilder.DropTable(
                name: "ProjectContacts",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "ProjectInstallations",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectLog",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectParties",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectPartyContacts",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectPartyGroups",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "RawMaterials",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "RepairCostPrices",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "ReportingGroups",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "Salutations",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "ServiceRates",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "SickLeaves",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "StaffUsers",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "StandardBillingTermLines",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "StandardBillingTerms",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "StockLocations",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "StockMovements",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "StockOrder",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "StockOrderDeliveries",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "StockOrderLines",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "StoredFiles",
                schema: "Files");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "SupplierContacts",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "TaskActions",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "TaskDependencies",
                schema: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskItems",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "TaskTemplateDependencies",
                schema: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskTemplates",
                schema: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskTemplateTasks",
                schema: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskTypes",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "TemplateType",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Timesheet",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "UserGroups",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "VatTypes",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "WebshopProductStructures",
                schema: "Products");

            migrationBuilder.DropTable(
                name: "WebshopStructures",
                schema: "Products");
        }
    }
}
