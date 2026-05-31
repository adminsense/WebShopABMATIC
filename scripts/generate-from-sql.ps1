# Generates English SQL + EF model from scripts/ABMATIC-create-local.sql
$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent
if (-not (Test-Path "$root\scripts\ABMATIC-create-local.sql")) { $root = "c:\Projects\WebShopABMATIC" }

$sqlPath = Join-Path $root "scripts\ABMATIC-create-local.sql"
$outSqlPath = Join-Path $root "scripts\WebShopABMATIC-create-local.sql"
$entitiesDir = Join-Path $root "Model\Entities"
$persistenceDir = Join-Path $root "Persistence"

$SchemaMap = @{
    Bestanden = "Files"; Boekhouding = "Accounting"; Crm = "Crm"; Emails = "Emails"
    Instellingen = "Settings"; Klanten = "Customers"; Logging = "Logging"
    Products = "Products"; Projecten = "Projects"; Taken = "Tasks"; Users = "Users"
}

$TableMap = @{
    AzureFile = "AzureFiles"; AzureFileFolder = "AzureFileFolders"; Bestand = "StoredFiles"
    DossierBestanden = "OrderFileLinks"; BtwType = "VatTypes"; DocumentDetail = "AccountingDocumentLines"
    Documenten = "AccountingDocuments"; DocumentType = "DocumentTypes"
    IntrastatReportLine = "IntrastatReportLines"; KlantExtraKortingen = "CustomerExtraDiscounts"
    Aanspreking = "Salutations"; Activiteiten = "Activities"; Agenda = "CalendarEntries"
    AgendaLabel = "CalendarLabels"; AgendaLog = "CalendarLogs"; AgendaStatus = "CalendarStatuses"
    Betaaltermijn = "PaymentTerms"; ContactProjectRol = "ContactProjectRoles"
    KlantDossierStatusOpmerking = "CustomerOrderStatusRemarks"; KlantFollowUp = "CustomerFollowUps"
    KlantJobcodeTarief = "CustomerJobCodeRates"; KlantLeveradres = "CustomerDeliveryAddresses"
    KlantLeverancierKorting = "CustomerSupplierDiscounts"; KlantMaatProduct = "CustomerCustomProducts"
    KlantMaatProductDetail = "CustomerCustomProductLines"; KlantMaatproductStaffel = "CustomerCustomProductTiers"
    KlantOpmerkingen = "CustomerNotes"; KlantProductKorting = "CustomerProductDiscounts"
    KlantStatus = "CustomerStatuses"; ProjectContact = "ProjectContacts"; SupplierConact = "SupplierContacts"
    TaakActies = "TaskActions"; TaakType = "TaskTypes"; Taken = "TaskItems"
    Bijlage = "EmailAttachments"; Email = "EmailMessages"; EmailQueue = "EmailQueues"
    AutoNummering = "AutoNumberings"; Betalingswijze = "PaymentMethods"
    HerstellingKostPrijs = "RepairCostPrices"; LangTag = "LanguageTags"; Parameter = "AppSettings"
    ProductKortingSuggestie = "ProductDiscountSuggestions"; ProductKortingSuggestieDetail = "ProductDiscountSuggestionLines"
    StdFacturatieVoorwaarden = "StandardBillingTerms"; StdFacturatieVoorwaardenDetail = "StandardBillingTermLines"
    Taal = "Languages"; Templates = "DocumentTemplates"; User = "StaffUsers"; UsrGroep = "UserGroups"
    Klant = "Customers"; KlantContact = "CustomerContacts"; KlantType = "CustomerTypes"
    Error = "AppErrors"; ProjectActiviteit = "ProjectActivities"; Grondstof = "RawMaterials"
    LosseProducten = "MiscellaneousProducts"; PrestatieTarief = "ServiceRates"
    PrijslijstCategorie = "PriceListCategories"; PrijslijstTeksten = "PriceListTexts"
    ProductAankoopKortingen = "ProductPurchaseDiscounts"; ProductEenheid = "ProductUnits"
    ProductHandleiding = "ProductManuals"; ProductOptions = "ProductOptions"
    ProductPopupRetourKolom = "ProductPopupReturnColumns"; ProductPopupTemplateDetail = "ProductPopupTemplateLines"
    ProductPopupWaardeType = "ProductPopupValueTypes"; ProductPrijzen = "ProductPrices"
    ProductPrijzenVerkoopKorting = "ProductPriceSalesDiscounts"; ProductProductionsGroepen = "ProductProductionGroupLinks"
    ProductPropertieItem = "ProductPropertyItems"; ProductStaffel = "ProductQuantityTiers"
    ProductStockLocatie = "ProductStockLocations"; ProductStructuur = "ProductStructures"
    ProductStructuurWebshop = "WebshopProductStructures"; WebshopStructuur = "WebshopStructures"
    StockBeweging = "StockMovements"; StockLocatie = "StockLocations"; StockOrderDetail = "StockOrderLines"
    StockOrderLevering = "StockOrderDeliveries"; Bestelling = "Orders"; BestellingDetail = "OrderLines"
    BestellingStatus = "OrderStatuses"; BestellingStatusToegangen = "OrderStatusAccesses"
    BestellingType = "OrderTypes"; BinnengebrachtProduct = "CustomerDeliveredProducts"
    DossierDetailText = "OrderLineTexts"; DossierDevelopmentDetail = "OrderDevelopmentLines"
    DossierFeedback = "OrderFeedbacks"; DossierInstallatieDetail = "OrderInstallationLines"
    DossierLeveringsTypeProduct = "OrderDeliveryTypeProducts"; DossierLog = "OrderLogs"
    DossierOpmerking = "OrderRemarks"; DossierProjectDetail = "OrderProjectLines"
    DossierStatusGroep = "OrderStatusGroups"; DossierStructuur = "OrderStructures"
    DossierVerwerkingsType = "OrderProcessingTypes"; DossierVoorschot = "OrderAdvancePayments"
    FacturatieAfspraak = "BillingAgreements"; LeveringType = "DeliveryTypes"
    OnderhoudsContract = "MaintenanceContracts"; OnderhoudsContractDetail = "MaintenanceContractLines"
    ProjectInstallatie = "ProjectInstallations"; ProjectPartij = "ProjectParties"
    ProjectPartijContact = "ProjectPartyContacts"; ProjectPartijGroep = "ProjectPartyGroups"
    TaakDependency = "TaskDependencies"; TaakTemplate = "TaskTemplates"
    TaakTemplateDependencie = "TaskTemplateDependencies"; TaakTemplateTaak = "TaskTemplateTasks"
    Ziekte = "SickLeaves"; Werf = "JobSites"; ReportingGroep1 = "ReportingGroups"
}

$ExcludeTables = @(
    "dbo.abm$", "dbo.imp_Intrastat2", "dbo.impStockPlaatsen", "dbo.instrastatimp",
    "dbo.instrastatproductenupdateimport", "Boekhouding.DocumentDetailold", "Products.stockreserveringbackup"
)

$EntityMap = @{
    AzureFiles = "AzureFile"; AzureFileFolders = "AzureFileFolder"; StoredFiles = "StoredFile"
    OrderFileLinks = "OrderFileLink"; VatTypes = "VatType"; AccountingDocumentLines = "AccountingDocumentLine"
    AccountingDocuments = "AccountingDocument"; CustomerExtraDiscounts = "CustomerExtraDiscount"
    Salutations = "Salutation"; Activities = "Activity"; CalendarEntries = "CalendarEntry"
    CalendarLabels = "CalendarLabel"; CalendarLogs = "CalendarLog"; CalendarStatuses = "CalendarStatus"
    PaymentTerms = "PaymentTerm"; ContactProjectRoles = "ContactProjectRole"
    CustomerOrderStatusRemarks = "CustomerOrderStatusRemark"; CustomerFollowUps = "CustomerFollowUp"
    CustomerJobCodeRates = "CustomerJobCodeRate"; CustomerDeliveryAddresses = "CustomerDeliveryAddress"
    CustomerSupplierDiscounts = "CustomerSupplierDiscount"; CustomerCustomProducts = "CustomerCustomProduct"
    CustomerCustomProductLines = "CustomerCustomProductLine"; CustomerCustomProductTiers = "CustomerCustomProductTier"
    CustomerNotes = "CustomerNote"; CustomerProductDiscounts = "CustomerProductDiscount"
    CustomerStatuses = "CustomerStatus"; SupplierContacts = "SupplierContact"
    TaskActions = "TaskAction"; TaskTypes = "TaskType"; TaskItems = "TaskItem"
    EmailAttachments = "EmailAttachment"; EmailMessages = "EmailMessage"
    AutoNumberings = "AutoNumbering"; PaymentMethods = "PaymentMethod"; RepairCostPrices = "RepairCostPrice"
    LanguageTags = "LanguageTag"; AppSettings = "AppSetting"
    ProductDiscountSuggestions = "ProductDiscountSuggestion"; ProductDiscountSuggestionLines = "ProductDiscountSuggestionLine"
    StandardBillingTerms = "StandardBillingTerm"; StandardBillingTermLines = "StandardBillingTermLine"
    Languages = "Language"; DocumentTemplates = "DocumentTemplate"; StaffUsers = "StaffUser"; UserGroups = "UserGroup"
    Customers = "Customer"; CustomerContacts = "CustomerContact"; CustomerTypes = "CustomerType"
    AppErrors = "AppError"; ProjectActivities = "ProjectActivity"; RawMaterials = "RawMaterial"
    MiscellaneousProducts = "MiscellaneousProduct"; ServiceRates = "ServiceRate"
    PriceListCategories = "PriceListCategory"; PriceListTexts = "PriceListText"
    ProductPurchaseDiscounts = "ProductPurchaseDiscount"; ProductUnits = "ProductUnit"
    ProductManuals = "ProductManual"; ProductPopupReturnColumns = "ProductPopupReturnColumn"
    ProductPopupTemplateLines = "ProductPopupTemplateLine"; ProductPopupValueTypes = "ProductPopupValueType"
    ProductPrices = "ProductPrice"; ProductPriceSalesDiscounts = "ProductPriceSalesDiscount"
    ProductProductionGroupLinks = "ProductProductionGroupLink"; ProductPropertyItems = "ProductPropertyItem"
    ProductQuantityTiers = "ProductQuantityTier"; ProductStockLocations = "ProductStockLocation"
    ProductStructures = "ProductStructure"; WebshopProductStructures = "WebshopProductStructure"
    WebshopStructures = "WebshopStructure"; StockMovements = "StockMovement"; StockLocations = "StockLocation"
    StockOrderLines = "StockOrderLine"; StockOrderDeliveries = "StockOrderDelivery"
    Orders = "Order"; OrderLines = "OrderLine"; OrderStatuses = "OrderStatus"
    OrderStatusAccesses = "OrderStatusAccess"; OrderTypes = "OrderType"
    CustomerDeliveredProducts = "CustomerDeliveredProduct"; OrderLineTexts = "OrderLineText"
    OrderDevelopmentLines = "OrderDevelopmentLine"; OrderFeedbacks = "OrderFeedback"
    OrderInstallationLines = "OrderInstallationLine"; OrderDeliveryTypeProducts = "OrderDeliveryTypeProduct"
    OrderLogs = "OrderLog"; OrderRemarks = "OrderRemark"; OrderProjectLines = "OrderProjectLine"
    OrderStatusGroups = "OrderStatusGroup"; OrderStructures = "OrderStructure"
    OrderProcessingTypes = "OrderProcessingType"; OrderAdvancePayments = "OrderAdvancePayment"
    BillingAgreements = "BillingAgreement"; DeliveryTypes = "DeliveryType"
    MaintenanceContracts = "MaintenanceContract"; MaintenanceContractLines = "MaintenanceContractLine"
    ProjectInstallations = "ProjectInstallation"; ProjectParties = "ProjectParty"
    ProjectPartyContacts = "ProjectPartyContact"; ProjectPartyGroups = "ProjectPartyGroup"
    TaskDependencies = "TaskDependency"; TaskTemplates = "TaskTemplate"
    TaskTemplateDependencies = "TaskTemplateDependency"; TaskTemplateTasks = "TaskTemplateTask"
    SickLeaves = "SickLeave"; JobSites = "JobSite"; ReportingGroups = "ReportingGroup"
}

function Translate-TableName([string]$name) {
    if ($TableMap.ContainsKey($name)) { return $TableMap[$name] }
    return $name
}

function Get-EntityName([string]$englishTable) {
    if ($EntityMap.ContainsKey($englishTable)) { return $EntityMap[$englishTable] }
    if ($englishTable.EndsWith("ies")) { return $englishTable.Substring(0, $englishTable.Length - 3) + "y" }
    if ($englishTable.EndsWith("s") -and $englishTable.Length -gt 1) { return $englishTable.Substring(0, $englishTable.Length - 1) }
    return $englishTable
}

$ColumnReplacements = @(
    @{From='KlantKlantId';To='CustomerId'}, @{From='SupplierSupplierId';To='SupplierId'}
    @{From='ManufacturerManufacturerId';To='ManufacturerId'}, @{From='ProjectProjectId';To='ProjectId'}
    @{From='Klant';To='CustomerId'}, @{From='User';To='AssignedUserId'}, @{From='Project';To='ProjectId'}
    @{From='AanmaakDatum';To='CreatedAt'}, @{From='UsrGroepId';To='UserGroupId'}
    @{From='AangemaaktDoorUsrId';To='CreatedByUserId'}, @{From='TakenId';To='TaskItemId'}
    @{From='KlantTypeKlantTypeId';To='CustomerTypeId'}, @{From='KlantContactKlantContactId';To='CustomerContactId'}
    @{From='ProductProdId';To='ProductId'}, @{From='ProdProdGroId';To='ProductProductionGroupId'}
    @{From='DossierDetailId';To='OrderLineId'}, @{From='DossierId';To='OrderId'}
    @{From='BestellingId';To='OrderId'}, @{From='BestellingDetailId';To='OrderLineId'}
    @{From='DocId';To='AccountingDocumentId'}, @{From='DosBestandBestandId';To='StoredFileId'}
    @{From='DosBestandDossierId';To='OrderId'}, @{From='DosBestandId';To='OrderFileLinkId'}
    @{From='BestandId';To='StoredFileId'}, @{From='BestandNaam';To='FileName'}, @{From='BestandType';To='FileType'}
    @{From='WerfId';To='JobSiteId'}, @{From='WerfAdres';To='SiteStreet'}, @{From='WerfBus';To='SiteBox'}
    @{From='WerfHuisnr';To='SiteHouseNumber'}, @{From='WerfPlaats';To='SiteCityId'}
    @{From='WerfEindKlantEmail';To='EndCustomerEmail'}, @{From='WerfEindKlantGsm';To='EndCustomerMobile'}
    @{From='WerfEindKlantNaam';To='EndCustomerName'}, @{From='WerfEindKlantTel';To='EndCustomerPhone'}
    @{From='WerfOpmerking';To='SiteNotes'}, @{From='KlantId';To='CustomerId'}, @{From='KlantNaam';To='CustomerName'}
    @{From='KlantNr';To='CustomerNumber'}, @{From='KlantBtw';To='CustomerVatNumber'}, @{From='KlantBus';To='CustomerBox'}
    @{From='KlantHuisnr';To='CustomerHouseNumber'}, @{From='KlantStraat';To='CustomerStreet'}
    @{From='KlantEmail';To='CustomerEmail'}, @{From='KlantCity';To='CustomerCityId'}, @{From='KlantType';To='CustomerTypeId'}
    @{From='KlantStatusId';To='CustomerStatusId'}, @{From='KlantTaal';To='CustomerLanguageId'}
    @{From='KlantOpmerking';To='CustomerNotes'}, @{From='KlantInterneOpmerking';To='CustomerInternalNotes'}
    @{From='KlantGroep';To='CustomerGroup'}, @{From='KlantVerantwoordelijke';To='AccountManagerUserId'}
    @{From='KlantAlgTel';To='CustomerPhone'}, @{From='KlantAlgFax';To='CustomerFax'}
    @{From='KlantBtwsysteem';To='CustomerVatSystemId'}, @{From='KlantBetalingStatus';To='CustomerPaymentStatus'}
    @{From='KlantActiviteit';To='CustomerActivityId'}, @{From='KlantNummer';To='CustomerNumber'}
    @{From='KlantAdresBuilding';To='CustomerBuildingName'}, @{From='KlantTypeNaam';To='CustomerTypeName'}
    @{From='KlantTypeNaamFr';To='CustomerTypeNameFr'}, @{From='KlantTypeBtwNrVerplicht';To='RequiresVatNumber'}
    @{From='KlantContactId';To='CustomerContactId'}, @{From='KlantContactKlantId';To='CustomerId'}
    @{From='KlantContactContactId';To='ContactId'}, @{From='KlantContactOpmerking';To='Notes'}
    @{From='KlantMaatProductId';To='CustomerCustomProductId'}, @{From='NaamNl';To='NameNl'}, @{From='NaamFr';To='NameFr'}
    @{From='NaamEn';To='NameEn'}, @{From='NaamFR';To='NameFr'}, @{From='NaamEN';To='NameEn'}
    @{From='Naam';To='Name'}, @{From='Omschrijving';To='Description'}, @{From='Opmerking';To='Notes'}
    @{From='Aantal';To='Quantity'}, @{From='Volgorde';To='SortOrder'}, @{From='GemaaktOp';To='CreatedAt'}
    @{From='AangemaaktOp';To='CreatedAt'}, @{From='AangemaaktDoor';To='CreatedBy'}
    @{From='AangemaaktUserId';To='CreatedByUserId'}, @{From='AangepastOp';To='ModifiedAt'}
    @{From='AangepastDoor';To='ModifiedBy'}, @{From='Geaccepteerd';To='IsAccepted'}
    @{From='GemaaktDoorUsrId';To='CreatedByUserId'}, @{From='LeveringsType';To='DeliveryTypeId'}
    @{From='LeverigsType';To='DeliveryTypeId'}, @{From='PrijslijstType';To='PriceListTypeId'}
    @{From='Dossiernummer';To='OrderNumber'}, @{From='DossierNummer';To='OrderNumber'}
    @{From='Commissiebedrag';To='CommissionAmount'}, @{From='MonteurContactId';To='InstallerContactId'}
    @{From='GewensteOpleveringsdatum';To='RequestedDeliveryDate'}, @{From='MontageDuurInDagen';To='InstallationDays'}
    @{From='ProductieDuurInDagen';To='ProductionDays'}, @{From='MasterDossier';To='MasterOrderId'}
    @{From='InterneNota';To='InternalNotes'}, @{From='RalKleur';To='RalColor'}, @{From='VoorschotFacturatie';To='AdvanceInvoiceEnabled'}
    @{From='ExtraKorting';To='ExtraDiscount'}, @{From='MonteurOpmerking';To='InstallerNotes'}
    @{From='AbmaticOpmerking';To='InternalStaffNotes'}, @{From='DeelsLeveringToestaan';To='AllowPartialDelivery'}
    @{From='PrijsPerEenheid';To='UnitPrice'}, @{From='AssemblagePrijs';To='AssemblyPrice'}
    @{From='MontagePrijs';To='InstallationPrice'}, @{From='TotaalExcl';To='TotalExclVat'}
    @{From='TotaalIncl';To='TotalInclVat'}, @{From='IsOptie';To='IsOption'}, @{From='Korting';To='Discount'}
    @{From='NaamVoorDocument';To='DocumentDisplayName'}, @{From='NaamVoorDocumentFR';To='DocumentDisplayNameFr'}
    @{From='OpmerkingOfferte';To='QuoteNotes'}, @{From='OpmerkingProductie';To='ProductionNotes'}
    @{From='OpmerkingBestelBon';To='PurchaseOrderNotes'}, @{From='ProductieGroep';To='ProductionGroup'}
    @{From='FactuurTekst';To='InvoiceText'}, @{From='FactuurTekstEn';To='InvoiceTextEn'}, @{From='FactuurTekstFr';To='InvoiceTextFr'}
    @{From='ToelichtingNl';To='ExplanationNl'}, @{From='ToelichtingFr';To='ExplanationFr'}, @{From='ToelichtingEn';To='ExplanationEn'}
    @{From='DocKlantNaam';To='DocumentCustomerName'}, @{From='DocKlantNr';To='DocumentCustomerNumber'}
    @{From='DocKlantStraat';To='DocumentCustomerStreet'}, @{From='DocKlantBus';To='DocumentCustomerBox'}
    @{From='DocKlantHuisNr';To='DocumentCustomerHouseNumber'}, @{From='DocKlantPostcode';To='DocumentCustomerPostalCode'}
    @{From='DocKlantWoonplaats';To='DocumentCustomerCity'}, @{From='DocKlantLand';To='DocumentCustomerCountry'}
    @{From='DocKlantBtwnr';To='DocumentCustomerVatNumber'}, @{From='DocKlantBedrijfsnaam';To='DocumentCustomerCompanyName'}
    @{From='DocAanmaakDatum';To='DocumentCreatedAt'}, @{From='DocDatum';To='DocumentDate'}
    @{From='DocBedragBtw';To='DocumentVatAmount'}, @{From='DocBedragNetto';To='DocumentNetAmount'}
    @{From='DocBedragTotaal';To='DocumentTotalAmount'}, @{From='DocBestellingId';To='OrderId'}
    @{From='DocDefinitef';To='IsFinal'}, @{From='DocNummer';To='DocumentNumber'}, @{From='DocType';To='DocumentTypeId'}
    @{From='DocKlantId';To='CustomerId'}, @{From='DocKlantTaal';To='DocumentCustomerLanguageId'}
    @{From='ProdName';To='NameNl'}, @{From='ProdDescription';To='DescriptionNl'}, @{From='ProdNameFr';To='NameFr'}
    @{From='ProdDescriptionFr';To='DescriptionFr'}, @{From='ProdNameEN';To='NameEn'}, @{From='ProdDescriptionEN';To='DescriptionEn'}
    @{From='ProdOrderPartNumber';To='OrderPartNumber'}, @{From='ProdStockNumber';To='StockNumber'}
    @{From='ProdSupplier';To='SupplierId'}, @{From='ProdManufacturer';To='ManufacturerId'}
    @{From='ProdNonActive';To='IsInactive'}, @{From='ProdPrijsLijst';To='ShowOnPriceList'}
    @{From='ProdId';To='ProductId'}, @{From='PredBebat';To='BebatAmount'}, @{From='ProdRecupel';To='RecupelAmount'}
    @{From='WebShop';To='ShowOnWebshop'}, @{From='WebshopDescriptionNl';To='WebshopDescriptionNl'}
    @{From='VerzendenNaarKlant';To='SendToCustomer'}, @{From='VerzendenBijBestellingLeverancier';To='SendOnSupplierOrder'}
    @{From='LeverancierId';To='SupplierId'}, @{From='CreateDatum';To='CreatedAt'}, @{From='OrderDatum';To='OrderDate'}
    @{From='LeveringsDatum';To='DeliveryDate'}, @{From='FactuurDatum';To='InvoiceDate'}, @{From='Afgewerkt';To='IsCompleted'}
    @{From='InterneOpmerking';To='InternalNotes'}, @{From='TotaalBedrag';To='TotalAmount'}
    @{From='AantalBesteld';To='QuantityOrdered'}, @{From='AantalGeleverd';To='QuantityDelivered'}
    @{From='Productnaam';To='ProductName'}, @{From='Ordernummer';To='OrderNumber'}, @{From='VerpaktPer';To='PackSize'}
    @{From='AankoopPrijsStuk';To='PurchaseUnitPrice'}, @{From='AankoopPrijsTotaal';To='PurchaseTotalPrice'}
    @{From='Eenheid';To='Unit'}, @{From='BesellingStatusId';To='OrderStatusId'}, @{From='Taaknaam';To='TaskName'}
    @{From='BinnenTermijnVan';To='DueWithinTicks'}, @{From='LockAlsVorigeNietCompleetZijn';To='LockUntilPreviousComplete'}
    @{From='EmailBestandsNaam';To='EmailFileName'}, @{From='EmailOnlyFile';To='IsEmailOnlyFile'}
    @{From='Onderwerp';To='Subject'}, @{From='Inhoud';To='Body'}, @{From='Verzonden';To='SentAt'}, @{From='Ontvangen';To='ReceivedAt'}
    @{From='Van';To='FromAddress'}, @{From='Aan';To='ToAddress'}, @{From='Voornaam';To='FirstName'}, @{From='Achternaam';To='LastName'}
    @{From='Indienst';To='HiredAt'}, @{From='Uitdienst';To='LeftAt'}, @{From='Functie';To='JobTitle'}
    @{From='Reden';To='Reason'}, @{From='PoortId';To='GateId'}, @{From='PoortOnderdeelId';To='GateComponentId'}
    @{From='ProductieCategories';To='ProductionCategories'}, @{From='Optioneel';To='IsOptional'}
    @{From='ExrtaBasisPrijs';To='ExtraBasePrice'}, @{From='ProjectContactId';To='LinkedContactId'}
    @{From='ContactOfferteContactId';To='QuoteContactId'}, @{From='ContactOrderbevestigingContactId';To='OrderConfirmationContactId'}
    @{From='ContactPlanningContactId';To='PlanningContactId'}, @{From='ContactLeveringCompleetContactId';To='DeliveryCompleteContactId'}
    @{From='ContactFacturatieContactId';To='BillingContactId'}, @{From='Pro1';To='Pro1'}, @{From='Pro2';To='Pro2'}
    @{From='Pro3';To='Pro3'}, @{From='Aan1';To='Aan1'}, @{From='Aan2';To='Aan2'}, @{From='Par1';To='Par1'}, @{From='Ond';To='Ond'}
    @{From='StarteDate';To='StartDate'}, @{From='EindDatum';To='EndDate'}, @{From='Onderwerp';To='Subject'}
    @{From='IsProductie';To='IsProduction'}, @{From='BestelArtikel';To='IsPurchaseItem'}, @{From='IsMagazijn';To='IsWarehouse'}
    @{From='Factureerbaar';To='IsBillable'}, @{From='DuurInMin';To='DurationMinutes'}, @{From='DuurInUur';To='DurationHours'}
    @{From='Omschrijvig';To='Description'}, @{From='DefaultConact';To='IsDefaultContact'}
    @{From='DossierVerwerkingsTypeId';To='OrderProcessingTypeId'}, @{From='BtwTypeId';To='VatTypeId'}
    @{From='AlgemeneKorting';To='GeneralDiscount'}, @{From='OfferteId';To='QuoteId'}
    @{From='CommissieVerkoper';To='CommissionSalesUserId'}, @{From='CommissieGefactureerd';To='IsCommissionInvoiced'}
    @{From='TefacturerenCommissie';To='CommissionToInvoice'}, @{From='PopupMelding';To='PopupMessage'}
    @{From='OfferteAantalDagenGeldig';To='QuoteValidDays'}, @{From='IsDringend';To='IsUrgent'}
    @{From='PrijslijstDatum';To='PriceListDate'}, @{From='VoorschottenOpBasisVanBedragen';To='AdvancePaymentsByAmount'}
    @{From='HeeftCloudMap';To='HasCloudFolder'}, @{From='AfsluitingGecontroleerd';To='IsClosingVerified'}
    @{From='OpmerkingFactuur';To='InvoiceNotes'}, @{From='OpmerkingOfferte';To='QuoteNotesHeader'}
    @{From='EersteContact';To='FirstContactName'}, @{From='OmzetLaatste12Maanden';To='RevenueLast12Months'}
    @{From='PrijslijstViaEmail';To='SendPriceListByEmail'}, @{From='PromotieMailing';To='PromotionalMailing'}
    @{From='FacturenPerPost';To='InvoicesByMail'}, @{From='DigitaleFacturatie';To='DigitalInvoicing'}
    @{From='Gecontroleerd';To='IsVerified'}, @{From='CommisieOntvanger';To='CommissionRecipientId'}
    @{From='GevraagdeCommisie';To='RequestedCommission'}, @{From='LoginWebshop';To='WebshopLogin'}
    @{From='PasswordWebshop';To='WebshopPasswordHash'}, @{From='SaltWebshop';To='WebshopPasswordSalt'}
    @{From='PrijslijstResAannemer';To='PriceListResContractor'}, @{From='PrijslijstResDealer';To='PriceListResDealer'}
    @{From='PrijslijstResParticulier';To='PriceListResConsumer'}, @{From='PrijslijstIndaannemer';To='PriceListIndContractor'}
    @{From='PrijslijstIndDealer';To='PriceListIndDealer'}, @{From='LeveringKlantTypeId';To='DeliveryCustomerTypeId'}
    @{From='MontageKlantTypeId';To='InstallationCustomerTypeId'}, @{From='DatumEnTijd';To='Timestamp'}
    @{From='IsReservering';To='IsReservation'}, @{From='IsMagazijn';To='IsWarehouse'}
    @{From='OrderBevestigingDatum';To='OrderConfirmationDate'}, @{From='VerwachteLeveringsDatum';To='ExpectedDeliveryDate'}
    @{From='OpermerkingLevering';To='DeliveryNotes'}, @{From='OpmerkingBestelling';To='OrderNotes'}
    @{From='InterneReferentie';To='InternalReference'}, @{From='AantalVerwerktInStock';To='QuantityProcessedToStock'}
    @{From='LeveringsDocNummer';To='DeliveryDocumentNumber'}, @{From='AantalGefactureerd';To='QuantityInvoiced'}
    @{From='Actie';To='ActionCode'}, @{From='LogDate';To='LoggedAt'}, @{From='ModuleName';To='ModuleName'}
    @{From='InnerException';To='InnerExceptionMessage'}, @{From='UserName';To='UserName'}, @{From='ClassName';To='ClassName'}
    @{From='Toelichting';To='Explanation'}, @{From='OntvangenOp';To='ReceivedAt'}, @{From='OntvangenDoorUserId';To='ReceivedByUserId'}
    @{From='GebrachtDoor';To='BroughtBy'}, @{From='OpgehaaldDatum';To='PickedUpAt'}, @{From='OpgehaaldDoor';To='PickedUpBy'}
    @{From='VerzondenNaarLeverancierOp';To='SentToSupplierAt'}, @{From='TrackingNr';To='TrackingNumber'}
    @{From='TerugOntvangenVanLeverancierOp';To='ReturnedFromSupplierAt'}, @{From='OnherstelbaarDatum';To='IrreparableAt'}
    @{From='IsForDossier';To='IsForOrder'}, @{From='IsForCrm';To='IsForCrm'}, @{From='IsForGeneralUse';To='IsForGeneralUse'}
    @{From='BhDocumentFooter';To='AccountingDocumentFooter'}, @{From='Tag';To='Tag'}
    @{From='IsPrePay';To='IsPrePay'}, @{From='IsPostPay';To='IsPostPay'}, @{From='IsPivot';To='IsPivot'}
    @{From='PivotName';To='PivotName'}, @{From='LayoutXml';To='LayoutXml'}, @{From='ObjectName';To='ObjectName'}
    @{From='FieldName';To='FieldName'}, @{From='From';To='SourceKey'}, @{From='Waarde';To='Value'}
    @{From='Type';To='Type'}, @{From='CorrectieBruto';To='GrossCorrection'}, @{From='Korting';To='Discount'}
    @{From='KortingTot';To='DiscountCap'}, @{From='Ond1';To='Ond1'}, @{From='Percentage';To='Percentage'}
    @{From='Eindemaand';To='EndOfMonth'}, @{From='Man';To='IsMale'}, @{From='Vrouw';To='IsFemale'}
    @{From='Taal';To='LanguageId'}, @{From='AansprekingTekst';To='SalutationText'}
    @{From='AllDay';To='IsAllDay'}, @{From='Locatie';To='Location'}, @{From='Status';To='StatusId'}
    @{From='Label';To='LabelId'}, @{From='ReminderInfo';To='ReminderInfo'}, @{From='RecurrenceInfo';To='RecurrenceInfo'}
    @{From='OutlookId';To='OutlookId'}, @{From='SyncedToExchange';To='IsSyncedToExchange'}
    @{From='OnHold';To='IsOnHold'}, @{From='Cancelled';To='IsCancelled'}, @{From='RedenOnHoldAndCancelled';To='OnHoldOrCancelledReason'}
    @{From='OnderwerpUserText';To='SubjectUserText'}, @{From='OmschrijvingSysteem';To='SystemDescription'}
    @{From='IsVerlof';To='IsLeave'}, @{From='GemaaktDoorUserId';To='CreatedByUserId'}, @{From='Subject';To='Subject'}
    @{From='ChangeAction';To='ChangeAction'}, @{From='ChangeTime';To='ChangedAt'}, @{From='ChangedByUserId';To='ChangedByUserId'}
    @{From='AgendaId';To='CalendarEntryId'}, @{From='ToonInTodo';To='ShowInTodo'}, @{From='Kleur';To='Color'}
    @{From='IsType';To='IsType'}, @{From='IsCategorie';To='IsCategory'}, @{From='BeperktBewerken';To='RestrictEditing'}
    @{From='IsVoorVerlof';To='IsForLeave'}, @{From='IsBinnenDienst';To='IsInternalService'}, @{From='IsBuitenDienst';To='IsExternalService'}
    @{From='IsProjectPlanning';To='IsProjectPlanning'}, @{From='SetNonActiveNaDossierAfgehandeld';To='DeactivateAfterOrderClosed'}
    @{From='BestellingStatusId';To='OrderStatusId'}, @{From='KortingPercentage';To='DiscountPercentage'}
    @{From='TotBedrag';To='MaxAmount'}, @{From='VanBedrag';To='MinAmount'}, @{From='TypeKlant';To='CustomerTypeFilterId'}
    @{From='Tekst';To='Text'}, @{From='Datum';To='Date'}, @{From='Uurtarief';To='HourlyRate'}
    @{From='Nummer';To='Number'}, @{From='Stad';To='CityId'}, @{From='Contact';To='ContactId'}
    @{From='Functie';To='JobTitle'}, @{From='DefaultConact';To='IsDefaultContact'}, @{From='Marge';To='Margin'}
    @{From='KortingsPercentageMontage';To='InstallationDiscountPercentage'}, @{From='MontageKortingPercentage';To='InstallationDiscountPercentage'}
    @{From='MontageKlantTypeId';To='InstallationCustomerTypeId'}, @{From='Artikelnummer';To='ArticleNumber'}
    @{From='ArtikelNummerLeverancier';To='SupplierArticleNumber'}, @{From='Actief';To='IsActive'}
    @{From='MinAantal';To='MinQuantity'}, @{From='MaxAantal';To='MaxQuantity'}, @{From='StukPrijs';To='PieceUnitPrice'}
    @{From='VanDatum';To='ValidFrom'}, @{From='TotDatum';To='ValidTo'}, @{From='DocumentType';To='DocumentTypeId'}
    @{From='SupplierId';To='SupplierId'}, @{From='GrondstofId';To='RawMaterialId'}, @{From='AankoopPrijs';To='PurchasePrice'}
    @{From='KlantMaatProductId';To='CustomerCustomProductId'}, @{From='Default';To='IsDefault'}
    @{From='KleurText';To='TextColor'}, @{From='LinkedinUrl';To='LinkedInUrl'}, @{From='StdCElabelPrinter';To='DefaultCeLabelPrinter'}
    @{From='StdProductieLabelPrinter';To='DefaultProductionLabelPrinter'}, @{From='TelAbm';To='CompanyPhone'}
    @{From='GsmAbm';To='CompanyMobile'}, @{From='EmaiPrive';To='PrivateEmail'}, @{From='Ice1Nr';To='Ice1Number'}
    @{From='Ice1Naam';To='Ice1Name'}, @{From='Ice2Nr';To='Ice2Number'}, @{From='Ice2Naam';To='Ice2Name'}
    @{From='Adres';To='Address'}, @{From='SelecteerBijBinnendienstPlanning';To='SelectForInternalPlanning'}
    @{From='SelecteerBijBuitendienstPlanning';To='SelectForExternalPlanning'}, @{From='SelecteerBijProjectPlanning';To='SelectForProjectPlanning'}
    @{From='SelecteerBijVerlofPlanning';To='SelectForLeavePlanning'}, @{From='StandaardPlanningLabel';To='DefaultPlanningLabelId'}
    @{From='ToegangOmzetRapporten';To='CanAccessRevenueReports'}, @{From='ToegangWinstRapporten';To='CanAccessProfitReports'}
    @{From='ToegangDmsspecial';To='CanAccessDmsSpecial'}, @{From='ToegangBulkBestellingen';To='CanAccessBulkOrders'}
    @{From='ToegangStockBeheer';To='CanAccessStockManagement'}, @{From='VanuitDossierBestellingenDoen';To='CanOrderFromOrderScreen'}
    @{From='ToonProjecten';To='ShowProjects'}, @{From='Facturatie';To='CanAccessBilling'}, @{From='IsMontage';To='IsInstallationTeam'}
    @{From='IsService';To='IsServiceTeam'}, @{From='IsTransport';To='IsTransportTeam'}, @{From='DossierStatusGroepId';To='OrderStatusGroupId'}
    @{From='ContactBus';To='ContactBox'}, @{From='ContactEmail';To='ContactEmail'}, @{From='ContactFax';To='ContactFax'}
    @{From='ContactHuisnr';To='ContactHouseNumber'}, @{From='ContactLogin';To='ContactLogin'}, @{From='ContactMobiel';To='ContactMobile'}
    @{From='ContactNaam';To='ContactLastName'}, @{From='ContactPaswoord';To='ContactPassword'}, @{From='ContactStraat';To='ContactStreet'}
    @{From='ContactTel';To='ContactPhone'}, @{From='ContactVoornaam';To='ContactFirstName'}, @{From='ContactIsMonteur';To='IsInstallerContact'}
    @{From='ContactCity';To='ContactCityId'}, @{From='ContactIsInternUsr';To='IsInternalUserContact'}, @{From='ContactAanspreking';To='SalutationId'}
    @{From='ContactTaal';To='ContactLanguageId'}, @{From='ContactMonteurDisplayName';To='InstallerDisplayName'}
    @{From='ContactFunctie';To='ContactJobTitle'}, @{From='MailOfferte';To='EmailQuote'}, @{From='MailOrderbevestiging';To='EmailOrderConfirmation'}
    @{From='MailPlanning';To='EmailPlanning'}, @{From='MailLeveringKlaar';To='EmailDeliveryReady'}, @{From='MailGeleverd';To='EmailDelivered'}
    @{From='MailFacturatie';To='EmailBilling'}, @{From='ContactGebouw';To='ContactBuilding'}
    @{From='BasisKorting';To='BaseDiscount'}, @{From='BtwSysteem';To='VatSystemId'}, @{From='LeveringsType';To='DeliveryTypeId'}
    @{From='Betaaltermijn';To='PaymentTermId'}, @{From='KlantTypeBtwNrVerplicht';To='RequiresVatNumber'}
    @{From='CityName';To='CityName'}, @{From='CityZip';To='PostalCode'}, @{From='CityCountry';To='CountryName'}
    @{From='CountryIsoCode';To='CountryIsoCode'}, @{From='IsEu';To='IsEu'}, @{From='Isocode';To='IsoCode'}
    @{From='Code';To='Code'}, @{From='Text';To='Text'}, @{From='Hoofdgroep';To='MainGroup'}, @{From='SubGroep';To='SubGroup'}
    @{From='ArtikelNr';To='ArticleNumber'}, @{From='Stockplaats';To='StockLocationCode'}, @{From='Leverancier';To='SupplierName'}
    @{From='LaatsteKeerGeteld';To='LastCountedAt'}, @{From='Groep';To='GroupName'}, @{From='Active';To='IsActive'}
    @{From='AantalFormule';To='QuantityFormula'}, @{From='PrijsPerStuk';To='UnitPrice'}, @{From='PrijsFormule';To='PriceFormula'}
    @{From='Tarief';To='Rate'}, @{From='Opties';To='HasOptions'}, @{From='TekstFr';To='TextFr'}, @{From='TekstEn';To='TextEn'}
    @{From='ProdKorteOpmerking';To='ShortNotesNl'}, @{From='ProdKorteOpmerkingFr';To='ShortNotesFr'}, @{From='ProdKorteOpmerkingEN';To='ShortNotesEn'}
    @{From='ProdVerpaktPerVerkoop';To='UnitsPerSale'}, @{From='ProdVerpaktPerAankoop';To='UnitsPerPurchase'}
    @{From='ProdPriceListOrder';To='PriceListSortOrder'}, @{From='ProdMinAantal';To='MinimumQuantity'}
    @{From='ProdMaatwerkPercentage';To='CustomWorkPercentage'}, @{From='ProdRecupelProduct';To='RecupelProductId'}
    @{From='ProdBebatProduct';To='BebatProductId'}, @{From='IsSnelleOptieLosseVk';To='IsQuickLooseSaleOption'}
    @{From='TaakTemplate';To='TaskTemplateId'}, @{From='AankoopProductEenheidId';To='PurchaseUnitId'}
    @{From='VerkoopProductEenheid1Id';To='SalesUnitId'}, @{From='AdsolutId';To='AdsolutId'}
    @{From='ProdIsSamengesteldProduct';To='IsCompositeProduct'}, @{From='ProductStructuurId';To='ProductStructureId'}
    @{From='TempKorting';To='TemporaryDiscount'}, @{From='TempNettoAankoop';To='TemporaryNetPurchasePrice'}
    @{From='ReportingGroep1Id';To='ReportingGroupId'}, @{From='VerkoopAantalStockTrigger';To='SalesStockTriggerQuantity'}
    @{From='MeerPrijs';To='ExtraPrice'}, @{From='MeerprijsAssemblage';To='ExtraAssemblyPrice'}, @{From='MeerprijsMontage';To='ExtraInstallationPrice'}
    @{From='GeproduceerdSamengesteldProduct';To='IsProducedCompositeProduct'}, @{From='ProdZonderPrijs';To='HasNoPrice'}
    @{From='ToonOpOfferte';To='ShowOnQuote'}, @{From='ToonOpOrderbevestiging';To='ShowOnOrderConfirmation'}
    @{From='ToonOpFactuur';To='ShowOnInvoice'}, @{From='ToonOpPakbon';To='ShowOnPackingSlip'}
    @{From='ToonOpLeveringsBon';To='ShowOnDeliveryNote'}, @{From='ToonOpProductieBon';To='ShowOnProductionOrder'}
    @{From='ToonOpLakkerijBon';To='ShowOnPaintShopOrder'}, @{From='ToonOpMontageBon';To='ShowOnInstallationOrder'}
    @{From='Gewicht';To='Weight'}, @{From='OpmerkingInterneBonnen';To='InternalDocumentNotes'}
    @{From='ExterneMonteurKost';To='ExternalInstallerCost'}, @{From='RecupelRapporteren';To='ReportRecupel'}
    @{From='BebatRapporteren';To='ReportBebat'}, @{From='HeeftStaffelprijzen';To='HasTierPricing'}
    @{From='ToonGeenDetailprijs';To='HideDetailPrice'}, @{From='LaatsteAanpassing';To='LastModifiedAt'}
    @{From='LaatsteAanpassingDoor';To='LastModifiedBy'}, @{From='IsNieuw';To='IsNew'}, @{From='EanCode';To='EanCode'}
    @{From='PopUpMelding';To='PopupMessage'}, @{From='GoederenCode';To='GoodsCode'}, @{From='IntrastatCodeId';To='IntrastatCodeId'}
    @{From='BrutoVerkoop';To='GrossSalesPrice'}, @{From='BrutoAankoop';To='GrossPurchasePrice'}, @{From='NettoAankoop';To='NetPurchasePrice'}
    @{From='Basisprijs';To='BasePrice'}, @{From='GecorrigeerdeBrutoPrijs';To='CorrectedGrossPrice'}
    @{From='PrijsBerekenFormule';To='PriceCalculationFormula'}, @{From='GebruikPrijsBerekenFormule';To='UsePriceCalculationFormula'}
    @{From='BasisPrijsBerekenFormule';To='BasePriceCalculationFormula'}, @{From='OpstartKost';To='StartupCost'}
    @{From='ExtraAankoopKost';To='ExtraPurchaseCost'}, @{From='ExtraAankoopKostUitleg';To='ExtraPurchaseCostNotes'}
    @{From='AankoopKortingPercentage';To='PurchaseDiscountPercentage'}, @{From='BrutoCorrectiePercentage';To='GrossCorrectionPercentage'}
    @{From='TypeBerekening';To='CalculationType'}, @{From='LeverancierHanteerdAndereBrutoVerkoop';To='SupplierUsesDifferentGrossSalesPrice'}
    @{From='WaardeType';To='ValueType'}, @{From='Verplicht';To='IsRequired'}, @{From='ProductieOpmerking';To='ProductionNotesFlag'}
    @{From='OfferteOpmerking';To='QuoteNotesFlag'}, @{From='BerekenPrijs';To='CalculatePrice'}, @{From='IsAantalLijn';To='IsQuantityLine'}
    @{From='StandaardWaardeFormule';To='DefaultValueFormula'}, @{From='FormuleAantal';To='QuantityFormula'}, @{From='ExtraPriceFormula';To='ExtraPriceFormula'}
    @{From='EenheidsparameterFormule';To='UnitParameterFormula'}, @{From='Waarde';To='Value'}, @{From='WaardeFr';To='ValueFr'}, @{From='WaardeEn';To='ValueEn'}
    @{From='RowLosseVkkolom';To='LooseSaleColumn'}, @{From='RowPoortOnderdeelKolom';To='GateComponentColumn'}
    @{From='OverbrengenNaanAantalFormule';To='TransferToQuantityFormula'}, @{From='OverbrengenNaarAantal';To='TransferToQuantity'}
    @{From='PrijsOpnemen';To='IncludePrice'}, @{From='SchrijfNaarLijnKolom';To='WriteToLineColumn'}, @{From='Omschrijving';To='Description'}
    @{From='OmschrijvingFr';To='DescriptionFr'}, @{From='Tag';To='Tag'}, @{From='Minimum';To='MinimumQuantity'}
    @{From='StockId';To='StockLocationId'}, @{From='NietMeerActief';To='IsInactive'}, @{From='MaxAantal';To='MaxQuantity'}
    @{From='IsStandaard';To='IsDefault'}, @{From='MinAantal';To='MinQuantity'}, @{From='Gereserveerd';To='ReservedQuantity'}
    @{From='AantalGeteld';To='CountedQuantity'}, @{From='Level';To='Level'}, @{From='ParentId';To='ParentId'}
    @{From='IntoPrijslijstTekstenId';To='IntroPriceListTextId'}, @{From='OutroPrijslijstTekstenOutroId';To='OutroPriceListTextId'}
    @{From='Order';To='SortOrder'}, @{From='TonenOpPrijslijst';To='ShowOnPriceList'}, @{From='Icon';To='Icon'}
    @{From='PageBreakAfter';To='PageBreakAfter'}, @{From='MasterProductProdId';To='MasterProductId'}, @{From='SubProductProdId';To='SubProductId'}
    @{From='StockItem';To='IsStockItem'}, @{From='IsProductie';To='IsProductionItem'}, @{From='BestelArtikel';To='IsPurchaseItem'}
    @{From='ProdProdGroName';To='Name'}, @{From='ProdProdGroOrder';To='SortOrder'}, @{From='ProductieGroep';To='ProductionGroupId'}
    @{From='Value';To='Value'}, @{From='NameEn';To='NameEn'}, @{From='NameFr';To='NameFr'}, @{From='NameNl';To='NameNl'}
    @{From='EenheidsParameter';To='UnitParameter'}, @{From='Bruto';To='GrossAmount'}, @{From='Tot';To='ValidTo'}, @{From='Van';To='ValidFrom'}
    @{From='Ond';To='Ond'}, @{From='Path';To='Path'}, @{From='Web';To='ShowOnWeb'}, @{From='AutomMeesturen';To='SendAutomatically'}
    @{From='Extentie';To='Extension'}, @{From='Bucket';To='Bucket'}, @{From='KlantopmerkingId';To='CustomerNoteId'}
    @{From='ProductieCategories';To='ProductionCategories'}, @{From='ProductieCategory';To='ProductionCategoryId'}
    @{From='ReferentieKlant';To='CustomerReference'}, @{From='Path';To='TreePath'}, @{From='DatumOrderBevestiging';To='OrderConfirmationDate'}
    @{From='DatumGepland';To='PlannedDate'}, @{From='DatumLeveringsKlaar';To='ReadyForDeliveryDate'}, @{From='DatumKlaarVoorFacturatie';To='ReadyForBillingDate'}
    @{From='DatumAfgehandeld';To='CompletedDate'}, @{From='DatumOfferte';To='QuoteDate'}, @{From='Totaal';To='TotalAmount'}
    @{From='TotaalGefactureerd';To='TotalInvoiced'}, @{From='TeamsId';To='TeamsId'}, @{From='OrderbevestigingDoor';To='OrderConfirmedBy'}
    @{From='GeplandDoor';To='PlannedBy'}, @{From='LeveringsklaarDoor';To='ReadyForDeliveryBy'}, @{From='AfgehandeldDoor';To='CompletedBy'}
    @{From='OfferteDoor';To='QuoteBy'}, @{From='KlaarvoorFacturatieDoor';To='ReadyForBillingBy'}, @{From='HeeftPapierenDossier';To='HasPaperFile'}
    @{From='KlaargezetOp';To='QueuedAt'}, @{From='PrijsCalculatie';To='CalculatePrice'}, @{From='Percent';To='Percent'}
    @{From='IsEindFactuur';To='IsFinalInvoice'}, @{From='GefactureerdOp';To='InvoicedAt'}, @{From='Bedrag';To='Amount'}
    @{From='Voorschotzichtbaarheid';To='AdvancePaymentVisibility'}, @{From='Klantnaam';To='CustomerName'}, @{From='Btwperecentage';To='VatPercentage'}
    @{From='Code';To='Code'}, @{From='NaamNl';To='NameNl'}, @{From='NaamFr';To='NameFr'}, @{From='NaamEn';To='NameEn'}, @{From='UurPrijs';To='HourlyRate'}
    @{From='MontageKostTellen';To='IncludeInstallationCost'}, @{From='CyclusDagen';To='CycleDays'}, @{From='AantaldagenOpvoorhandRappelleren';To='ReminderDaysInAdvance'}
    @{From='OndershoudsContractOpvolgerUserId';To='SuccessorUserId'}, @{From='AutomatischVerlengen';To='AutoRenew'}
    @{From='OpgezegdOp';To='CancelledAt'}, @{From='RappelDatum';To='ReminderDate'}, @{From='ProjectNummer';To='ProjectNumber'}
    @{From='ProjectNaam';To='ProjectName'}, @{From='ProjectBeheerder';To='ProjectManagerUserId'}, @{From='ProjectKlant';To='CustomerId'}
    @{From='ProjectWerf';To='JobSiteId'}, @{From='ProjectTypeId';To='ProjectTypeId'}, @{From='ProjectAanmaakDatum';To='ProjectCreatedAt'}
    @{From='ProjectInterneOpmerking';To='ProjectInternalNotes'}, @{From='ProductionLabelReference';To='ProductionLabelReference'}
    @{From='IsTemplate';To='IsTemplate'}, @{From='ProjectOpmerking';To='ProjectNotes'}, @{From='IsStandaardProject';To='IsStandardProject'}
    @{From='InstallatieDossierId';To='InstallationOrderId'}, @{From='Locatie';To='Location'}, @{From='Serienummer';To='SerialNumber'}
    @{From='Specificaties';To='Specifications'}, @{From='Commentaar';To='Comment'}, @{From='FacturatiePercentage';To='BillingPercentage'}
    @{From='Notitie';To='Note'}, @{From='IsVoorLeverancier';To='IsForSupplier'}, @{From='IsVoorKlant';To='IsForCustomer'}
    @{From='IsVoorFabrikant';To='IsForManufacturer'}, @{From='TeFacturerenTijdInMin';To='BillableMinutes'}, @{From='TeFacturerenTijdInUur';To='BillableHours'}
    @{From='TimerOn';To='IsTimerRunning'}, @{From='TimerStart';To='TimerStartedAt'}, @{From='TimerLastValue';To='TimerLastValue'}
    @{From='ItemNr';To='ItemNumber'}, @{From='OnderdeelNummer';To='PartNumber'}, @{From='Materiaal';To='Material'}, @{From='AantalInInstallatie';To='QuantityInInstallation'}
    @{From='Bewerking';To='Processing'}, @{From='LeverancierArtikelNr';To='SupplierArticleNumber'}, @{From='Behandeling';To='Treatment'}
    @{From='Besteldatum';To='OrderedAt'}, @{From='Leverdatum';To='DeliveredAt'}, @{From='PrijsPerStuk';To='UnitPrice'}, @{From='PrijsTotaal';To='TotalPrice'}
    @{From='Guid';To='Id'}, @{From='ArtikelNr';To='ArticleNumber'}, @{From='MasterRowGuid';To='MasterRowId'}, @{From='AantalGereserveerd';To='ReservedQuantity'}
    @{From='AantalUitstock';To='QuantityTakenFromStock'}, @{From='TeBestellen';To='MustOrder'}, @{From='BesteldOp';To='OrderedAt'}, @{From='GeleverdOp';To='DeliveredAt'}
    @{From='TotaalAankoop';To='TotalPurchasePrice'}, @{From='TekeningGemaakt';To='DrawingCreated'}, @{From='TypeMateriaal';To='MaterialType'}
    @{From='Afwerking';To='Finish'}, @{From='Lakken';To='RequiresPainting'}, @{From='LakwerkSupplierSupplierId';To='PaintWorkSupplierId'}
    @{From='DependentId';To='DependentTaskId'}, @{From='ParentId';To='ParentTaskId'}, @{From='DependentOnId';To='DependsOnTaskTemplateTaskId'}
    @{From='IsDefaultUser';To='DefaultUserId'}, @{From='IsDefaultProjectVerantwoordelijke';To='UseProjectOwnerAsDefault'}
    @{From='IsDefaultGroep';To='DefaultUserGroupId'}, @{From='ZiekteBriefAzureFileId';To='SickNoteAzureFileId'}
    @{From='SerienrSuffix';To='SerialNumberSuffix'}, @{From='BestellingsTypeId';To='OrderTypeId'}, @{From='MasterRowId';To='MasterRowId'}
    @{From='StatusId';To='StatusId'}, @{From='GemaaktDoor';To='CreatedByUserId'}, @{From='Sequence';To='SortOrder'}
    @{From='IsForSalesReporting';To='IncludeInSalesReporting'}, @{From='ReportInBehandeling';To='ReportInProgress'}, @{From='ScreenModus';To='ScreenMode'}
    @{From='DoStockReservering';To='ReserveStock'}, @{From='DoStock';To='AffectsStock'}, @{From='BinnenAantalDagenCompleet';To='CompleteWithinDays'}
    @{From='WaarschuwingProductie';To='ProductionWarning'}, @{From='WaarschuwingLeveringMontage';To='DeliveryInstallationWarning'}
    @{From='DatumRappel';To='ReminderDate'}, @{From='Omschrijving';To='Description'}, @{From='Voltooid';To='IsCompleted'}
    @{From='VoltooidOp';To='CompletedAt'}, @{From='PercentComplete';To='PercentComplete'}, @{From='Geannuleerd';To='IsCancelled'}
    @{From='GeannuleerdOp';To='CancelledAt'}, @{From='CheckedByCreaterOn';To='CheckedByCreatorAt'}, @{From='Gelezen';To='IsRead'}
    @{From='PopupShown';To='PopupShown'}, @{From='Dringend';To='IsUrgent'}, @{From='Weigeringsreden';To='RejectionReason'}
    @{From='WeigereningGelezen';To='IsRejectionRead'}, @{From='Geweigerd';To='IsRejected'}, @{From='GelezenOp';To='ReadAt'}, @{From='GeweigerdOp';To='RejectedAt'}
    @{From='PreviewText';To='PreviewText'}, @{From='TeBehandelen';To='RequiresAction'}, @{From='IsPrivate';To='IsPrivate'}
    @{From='EmailQueueId';To='EmailQueueId'}, @{From='StockOrderId';To='StockOrderId'}, @{From='SupplierOrderMail';To='SupplierOrderEmail'}
    @{From='GrootboekOmzetRekening';To='GeneralLedgerRevenueAccount'}, @{From='MainSupplier';To='IsMainSupplier'}, @{From='NietActief';To='IsInactive'}
    @{From='PrijsLijstVolgorde';To='PriceListSortOrder'}, @{From='PrijslijstNaam';To='PriceListName'}
    @{From='ManufacturerName';To='Name'}, @{From='ManufacturerPhone';To='Phone'}, @{From='ManufacturerMobile';To='Mobile'}
    @{From='ManufacturerFax';To='Fax'}, @{From='ManufacturerEmail';To='Email'}, @{From='ManufacturerAddress';To='Address'}
    @{From='ManufacturerCity';To='CityId'}, @{From='ManufacturerKBO';To='CompanyRegistrationNumber'}, @{From='ManufacturerVAT';To='VatNumber'}
    @{From='SupplierName';To='Name'}, @{From='SupplierPhone';To='Phone'}, @{From='SupplierMobile';To='Mobile'}, @{From='SupplierFax';To='Fax'}
    @{From='SupplierEmail';To='Email'}, @{From='SupplierAddress';To='Address'}, @{From='SupplierCity';To='CityId'}
    @{From='SupplierKBO';To='CompanyRegistrationNumber'}, @{From='SupplierVAT';To='VatNumber'}, @{From='SupplierLangId';To='LanguageId'}
    @{From='ContactFunction';To='ContactFunctionId'}, @{From='Email';To='EmailTemplate'}, @{From='Fax';To='FaxTemplate'}, @{From='Brief';To='LetterTemplate'}
    @{From='Onderwerp';To='Subject'}, @{From='DocType';To='DocumentTypeCode'}, @{From='NaamFR';To='NameFr'}, @{From='Slogan';To='Slogan'}
    @{From='AccentColor';To='AccentColor'}, @{From='AllowAddNewFilesBlob';To='AllowAddNewBlobFiles'}
)

function Translate-Column([string]$col) {
    $result = $col
    foreach ($r in ($ColumnReplacements | Sort-Object { $_.From.Length } -Descending)) {
        if ($result -eq $r.From) { return $r.To }
    }
    return $result
}

function Get-PropertyName([string]$columnName, [System.Collections.Generic.Dictionary[string,string]]$usedNames) {
    $preferred = @{
        SupplierSupplierId = 'SupplierId'
        KlantKlantId = 'CustomerId'
        ManufacturerManufacturerId = 'ManufacturerId'
        ProjectProjectId = 'ProjectId'
    }
    $prop = if ($preferred.ContainsKey($columnName)) { $preferred[$columnName] } else { Translate-Column $columnName }

    if (-not $usedNames.ContainsKey($prop)) {
        $usedNames[$prop] = $columnName
        return $prop
    }
    if ($usedNames[$prop] -eq $columnName) { return $prop }

    $fallback = switch ($columnName) {
        'SupplierId' { 'RelatedSupplierId' }
        'KlantId' { 'RelatedCustomerId' }
        default { "${prop}Legacy" }
    }
    if (-not $usedNames.ContainsKey($fallback)) {
        $usedNames[$fallback] = $columnName
        return $fallback
    }
    $fallback = "$prop$columnName"
    $usedNames[$fallback] = $columnName
    return $fallback
}

function Get-TablePropertyMap($columns) {
    $usedNames = New-Object 'System.Collections.Generic.Dictionary[string,string]'
    $map = @{}
    $sortedCols = @($columns | Sort-Object { if ($_.Name -match 'SupplierSupplierId|KlantKlantId|ManufacturerManufacturerId|ProjectProjectId') { 0 } else { 1 } }, { $_.Name })
    foreach ($c in $sortedCols) {
        $map[$c.Name] = Get-PropertyName $c.Name $usedNames
    }
    return $map
}

function Sql-ToClr([string]$sqlType, [bool]$nullable) {
    $t = ($sqlType -replace '\s+', ' ').Trim().ToLower()
    $clr = switch -regex ($t) {
        '^bigint' { 'long' }
        '^int' { 'int' }
        '^bit' { 'bool' }
        '^datetime' { 'DateTime' }
        '^date$' { 'DateTime' }
        '^uniqueidentifier' { 'Guid' }
        '^decimal' { 'decimal' }
        '^float' { 'double' }
        '^varbinary' { 'byte[]' }
        '^nvarchar' { 'string' }
        '^varchar' { 'string' }
        '^nchar' { 'string' }
        '^char' { 'string' }
        '^text' { 'string' }
        '^ntext' { 'string' }
        default { 'string' }
    }
    if ($nullable -and $clr -eq 'string') { return 'string?' }
    if ($nullable -and $clr -eq 'byte[]') { return 'byte[]?' }
    if ($nullable) { return "${clr}?" }
    return $clr
}

function Parse-SqlTables([string]$content) {
    $tables = @()
    $blocks = [regex]::Split($content, '(?=CREATE TABLE \[)')
    foreach ($block in $blocks) {
        if ($block -notmatch 'CREATE TABLE \[([^\]]+)\]\.\[([^\]]+)\]\s*\(') { continue }
        $schema = $Matches[1]; $table = $Matches[2]
        $full = "$schema.$table"
        if ($ExcludeTables -contains $full) { continue }

        $openIdx = $block.IndexOf('(')
        $closeIdx = $block.LastIndexOf(');')
        if ($openIdx -lt 0 -or $closeIdx -lt 0) { continue }
        $body = $block.Substring($openIdx + 1, $closeIdx - $openIdx - 1)

        $pkCol = 'Id'
        if ($body -match 'PRIMARY KEY CLUSTERED \(\[([^\]]+)\]\)') { $pkCol = $Matches[1] }

        $cols = @()
        foreach ($line in ($body -split "`r?`n")) {
            $line = $line.Trim().TrimEnd(',')
            if (-not $line -or $line -match '^\s*CONSTRAINT\s') { continue }
            if ($line -notmatch '^\[([^\]]+)\]\s+(\w+(?:\(\d+(?:,\d+)?\))?)') { continue }
            $cName = $Matches[1]
            $sqlType = $Matches[2]
            $isIdentity = $line -match 'IDENTITY\s*\('
            $notNull = $line -match 'NOT NULL'
            $cols += [pscustomobject]@{
                Name = $cName; SqlType = $sqlType; IsIdentity = $isIdentity; NotNull = $notNull
                IsPk = ($cName -eq $pkCol)
            }
        }
        if ($cols.Count -gt 0) {
            $tables += [pscustomobject]@{
                Schema = $schema; Table = $table; FullName = $full; PkColumn = $pkCol; Columns = $cols
            }
        }
    }
    return $tables
}

$sql = Get-Content $sqlPath -Raw
$tables = Parse-SqlTables $sql
Write-Host "Parsed $($tables.Count) tables from SQL"

# --- English SQL ---
$sqlOut = @"
-- WebShop local: database + schemas + tables (English names)
-- Generated from ABMATIC-create-local.sql
-- sqlcmd -S localhost -E -i WebShop-create-local.sql

USE [master];
GO

IF DB_ID(N'WebShop') IS NOT NULL
BEGIN
    ALTER DATABASE [WebShop] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [WebShop];
END
GO

CREATE DATABASE [WebShop] COLLATE Latin1_General_CI_AS;
GO

USE [WebShop];
GO

"@

foreach ($s in ($SchemaMap.Keys | Sort-Object)) {
    $en = $SchemaMap[$s]
    $sqlOut += "CREATE SCHEMA [$en] AUTHORIZATION [dbo];`r`nGO`r`n`r`n"
}

foreach ($t in $tables) {
    $enSchema = $SchemaMap[$t.Schema]
    $enTable = Translate-TableName $t.Table
    $sqlOut += "CREATE TABLE [$enSchema].[$enTable]`r`n(`r`n"
    $colLines = @()
    foreach ($c in $t.Columns) {
        $enCol = Translate-Column $c.Name
        $line = "    [$enCol] $($c.SqlType)"
        if ($c.IsIdentity) { $line += " IDENTITY(1,1)" }
        if ($c.NotNull) { $line += " NOT NULL" }
        $colLines += $line
    }
    $pkEn = Translate-Column $t.PkColumn
    $colLines += "    CONSTRAINT [PK_${enSchema}_${enTable}] PRIMARY KEY CLUSTERED ([$pkEn])"
    $sqlOut += ($colLines -join ",`r`n") + "`r`n);`r`nGO`r`n`r`n"
}
Set-Content -Path $outSqlPath -Value $sqlOut -Encoding UTF8

# --- Clean old model ---
$oldEntityDir = Join-Path $root "WebShop\Model\EntityClasses"
$oldQueryDir = Join-Path $root "WebShop\Model\TypedListClasses"
if (Test-Path $oldEntityDir) { Remove-Item $oldEntityDir -Recurse -Force }
if (Test-Path $oldQueryDir) { Remove-Item $oldQueryDir -Recurse -Force }
if (Test-Path $entitiesDir) { Remove-Item $entitiesDir -Recurse -Force }
New-Item -ItemType Directory -Path $entitiesDir -Force | Out-Null

# --- Generate entities ---
$entityNames = @{}
foreach ($t in $tables) {
    $enSchema = $SchemaMap[$t.Schema]
    $enTable = Translate-TableName $t.Table
    $entity = Get-EntityName $enTable
    $propMap = Get-TablePropertyMap $t.Columns
    $entityNames[$entity] = @{ Entity = $entity; EnSchema = $enSchema; EnTable = $enTable; Table = $t; PropMap = $propMap }

    $sb = New-Object System.Text.StringBuilder
    [void]$sb.AppendLine("#nullable enable")
    [void]$sb.AppendLine("using System;")
    [void]$sb.AppendLine("")
    [void]$sb.AppendLine("namespace WebShop.Data.Entities;")
    [void]$sb.AppendLine("")
    [void]$sb.AppendLine("/// <summary>Entity for [$enSchema].[$enTable] (legacy: [$($t.Schema)].[$($t.Table)]).</summary>")
    [void]$sb.AppendLine("public class $entity")
    [void]$sb.AppendLine("{")
    foreach ($c in $t.Columns) {
        $prop = $propMap[$c.Name]
        $nullable = -not $c.NotNull
        $clr = Sql-ToClr $c.SqlType $nullable
        [void]$sb.AppendLine("    public $clr $prop { get; set; }")
    }
    [void]$sb.AppendLine("}")
    Set-Content -Path (Join-Path $entitiesDir "$entity.cs") -Value $sb.ToString() -Encoding UTF8
}

# --- ModelBuilder ---
$mb = New-Object System.Text.StringBuilder
[void]$mb.AppendLine("using Microsoft.EntityFrameworkCore;")
[void]$mb.AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;")
[void]$mb.AppendLine("using WebShop.Data.Entities;")
[void]$mb.AppendLine("")
[void]$mb.AppendLine("namespace WebShop.Data.Persistence;")
[void]$mb.AppendLine("")
[void]$mb.AppendLine("public class WebShopModelBuilder")
[void]$mb.AppendLine("{")
[void]$mb.AppendLine("    public void BuildModel(ModelBuilder modelBuilder)")
[void]$mb.AppendLine("    {")
foreach ($info in ($entityNames.Values | Sort-Object { $_.Entity })) {
    $e = $info.Entity
    [void]$mb.AppendLine("        Map$e(modelBuilder.Entity<$e>());")
}
[void]$mb.AppendLine("    }")
[void]$mb.AppendLine("")

foreach ($info in ($entityNames.Values | Sort-Object { $_.Entity })) {
    $e = $info.Entity; $t = $info.Table
    $pk = Translate-Column $t.PkColumn
    [void]$mb.AppendLine("    private static void Map$e(EntityTypeBuilder<$e> config)")
    [void]$mb.AppendLine("    {")
    [void]$mb.AppendLine("        config.ToTable(`"$($info.EnTable)`", `"$($info.EnSchema)`");")
    [void]$mb.AppendLine("        config.HasKey(t => t.$pk);")
    foreach ($c in $t.Columns) {
        $prop = $info.PropMap[$c.Name]
        $line = "        config.Property(t => t.$prop)"
        if ($c.IsIdentity) { $line += ".ValueGeneratedOnAdd()" }
        elseif ($c.SqlType -match 'decimal') { $line += ".HasColumnType(`"$($c.SqlType)`")" }
        elseif ($c.SqlType -match 'nvarchar\((\d+)\)' -and $Matches) { $line += ".HasMaxLength($($Matches[1]))" }
        if ($c.NotNull -and $c.SqlType -match 'nvarchar') { $line += ".IsRequired()" }
        $line += ";"
        [void]$mb.AppendLine($line)
    }
    [void]$mb.AppendLine("    }")
    [void]$mb.AppendLine("")
}
[void]$mb.AppendLine("}")

# --- DbContext ---
$ctx = New-Object System.Text.StringBuilder
[void]$ctx.AppendLine("using Microsoft.EntityFrameworkCore;")
[void]$ctx.AppendLine("using WebShop.Data.Entities;")
[void]$ctx.AppendLine("")
[void]$ctx.AppendLine("namespace WebShop.Data.Persistence;")
[void]$ctx.AppendLine("")
[void]$ctx.AppendLine("public class WebShopDbContext : DbContext")
[void]$ctx.AppendLine("{")
[void]$ctx.AppendLine("    public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options) { }")
[void]$ctx.AppendLine("")
[void]$ctx.AppendLine("    protected override void OnModelCreating(ModelBuilder modelBuilder)")
[void]$ctx.AppendLine("    {")
[void]$ctx.AppendLine("        new WebShopModelBuilder().BuildModel(modelBuilder);")
[void]$ctx.AppendLine("    }")
[void]$ctx.AppendLine("")
foreach ($info in ($entityNames.Values | Sort-Object { $_.Entity })) {
    $e = $info.Entity
    $setName = if ($e.EndsWith('s')) { $e + "es" } elseif ($e.EndsWith('y')) { $e.Substring(0,$e.Length-1) + "ies" } else { $e + "s" }
    [void]$ctx.AppendLine("    public DbSet<$e> $setName { get; set; } = null!;")
}
[void]$ctx.AppendLine("}")

if (Test-Path $persistenceDir) {
    Get-ChildItem $persistenceDir -Filter "AdminsenseDB*" | Remove-Item -Force
}
Set-Content -Path (Join-Path $persistenceDir "WebShopModelBuilder.cs") -Value $mb.ToString() -Encoding UTF8
Set-Content -Path (Join-Path $persistenceDir "WebShopDbContext.cs") -Value $ctx.ToString() -Encoding UTF8

# --- csproj ---
@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <AssemblyName>WebShop.Data</AssemblyName>
    <RootNamespace>WebShop.Data</RootNamespace>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
'@ | Set-Content (Join-Path $root "WebShop\Model\WebShop.Data.csproj") -Encoding UTF8

@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <AssemblyName>WebShop.Data.Persistence</AssemblyName>
    <RootNamespace>WebShop.Data.Persistence</RootNamespace>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.7" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\Model\WebShop.Data.csproj" />
  </ItemGroup>
</Project>
'@ | Set-Content (Join-Path $persistenceDir "WebShop.Data.Persistence.csproj") -Encoding UTF8

Remove-Item (Join-Path $root "WebShop\Model\AdminsenseDB.Model.csproj") -Force -ErrorAction SilentlyContinue
Remove-Item (Join-Path $persistenceDir "AdminsenseDB.Persistence.csproj") -Force -ErrorAction SilentlyContinue

Write-Host "Generated $($entityNames.Count) entities"
Write-Host "SQL: $outSqlPath"
