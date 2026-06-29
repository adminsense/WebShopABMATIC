using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebShopABMATIC.Data.Entities;

namespace WebShopABMATIC.Data.Persistence;

public class WebShopABMATICModelBuilder
{
    public void BuildModel(ModelBuilder modelBuilder)
    {
        MapAccountingDocument(modelBuilder.Entity<AccountingDocument>());
        MapAccountingDocumentLine(modelBuilder.Entity<AccountingDocumentLine>());
        MapActivity(modelBuilder.Entity<Activity>());
        MapAppError(modelBuilder.Entity<AppError>());
        MapAppSetting(modelBuilder.Entity<AppSetting>());
        MapAutoNumbering(modelBuilder.Entity<AutoNumbering>());
        MapAzureFile(modelBuilder.Entity<AzureFile>());
        MapAzureFileFolder(modelBuilder.Entity<AzureFileFolder>());
        MapBaseCompany(modelBuilder.Entity<BaseCompany>());
        MapBaseCompanyAcces(modelBuilder.Entity<BaseCompanyAcces>());
        MapBaseCompanyVatNumber(modelBuilder.Entity<BaseCompanyVatNumber>());
        MapBillingAgreement(modelBuilder.Entity<BillingAgreement>());
        MapCalendarEntry(modelBuilder.Entity<CalendarEntry>());
        MapCalendarLabel(modelBuilder.Entity<CalendarLabel>());
        MapCalendarLog(modelBuilder.Entity<CalendarLog>());
        MapCalendarStatus(modelBuilder.Entity<CalendarStatus>());
        MapCity(modelBuilder.Entity<City>());
        MapContact(modelBuilder.Entity<Contact>());
        MapContactProjectRole(modelBuilder.Entity<ContactProjectRole>());
        MapCountry(modelBuilder.Entity<Country>());
        MapCustomer(modelBuilder.Entity<Customer>());
        MapCustomerContact(modelBuilder.Entity<CustomerContact>());
        MapCustomerCustomProduct(modelBuilder.Entity<CustomerCustomProduct>());
        MapCustomerCustomProductLine(modelBuilder.Entity<CustomerCustomProductLine>());
        MapCustomerCustomProductTier(modelBuilder.Entity<CustomerCustomProductTier>());
        MapCustomerDeliveredProduct(modelBuilder.Entity<CustomerDeliveredProduct>());
        MapCustomerDeliveryAddress(modelBuilder.Entity<CustomerDeliveryAddress>());
        MapCustomerExtraDiscount(modelBuilder.Entity<CustomerExtraDiscount>());
        MapCustomerFollowUp(modelBuilder.Entity<CustomerFollowUp>());
        MapCustomerJobCodeRate(modelBuilder.Entity<CustomerJobCodeRate>());
        MapCustomerNote(modelBuilder.Entity<CustomerNote>());
        MapCustomerOrderStatusRemark(modelBuilder.Entity<CustomerOrderStatusRemark>());
        MapCustomerProductDiscount(modelBuilder.Entity<CustomerProductDiscount>());
        MapCustomerStatus(modelBuilder.Entity<CustomerStatus>());
        MapCustomerSupplierDiscount(modelBuilder.Entity<CustomerSupplierDiscount>());
        MapCustomerType(modelBuilder.Entity<CustomerType>());
        MapDeliveryType(modelBuilder.Entity<DeliveryType>());
        MapDocumentTemplate(modelBuilder.Entity<DocumentTemplate>());
        MapDocumentType(modelBuilder.Entity<DocumentType>());
        MapDrawGroup(modelBuilder.Entity<DrawGroup>());
        MapEmailAttachment(modelBuilder.Entity<EmailAttachment>());
        MapEmailMessage(modelBuilder.Entity<EmailMessage>());
        MapEmailQueue(modelBuilder.Entity<EmailQueue>());
        MapGridLayout(modelBuilder.Entity<GridLayout>());
        MapIntrastatCode(modelBuilder.Entity<IntrastatCode>());
        MapIntrastatReportLine(modelBuilder.Entity<IntrastatReportLine>());
        MapJobCode(modelBuilder.Entity<JobCode>());
        MapJobSite(modelBuilder.Entity<JobSite>());
        MapLanguage(modelBuilder.Entity<Language>());
        MapLanguageTag(modelBuilder.Entity<LanguageTag>());
        MapMaintenanceContract(modelBuilder.Entity<MaintenanceContract>());
        MapMaintenanceContractLine(modelBuilder.Entity<MaintenanceContractLine>());
        MapManufacturer(modelBuilder.Entity<Manufacturer>());
        MapMiscellaneousProduct(modelBuilder.Entity<MiscellaneousProduct>());
        MapOrder(modelBuilder.Entity<Order>());
        MapOrderAdvancePayment(modelBuilder.Entity<OrderAdvancePayment>());
        MapOrderDeliveryTypeProduct(modelBuilder.Entity<OrderDeliveryTypeProduct>());
        MapOrderDevelopmentLine(modelBuilder.Entity<OrderDevelopmentLine>());
        MapOrderFeedback(modelBuilder.Entity<OrderFeedback>());
        MapOrderFileLink(modelBuilder.Entity<OrderFileLink>());
        MapOrderInstallationLine(modelBuilder.Entity<OrderInstallationLine>());
        MapOrderLine(modelBuilder.Entity<OrderLine>());
        MapOrderLineText(modelBuilder.Entity<OrderLineText>());
        MapOrderLog(modelBuilder.Entity<OrderLog>());
        MapOrderProcessingType(modelBuilder.Entity<OrderProcessingType>());
        MapOrderProjectLine(modelBuilder.Entity<OrderProjectLine>());
        MapOrderRemark(modelBuilder.Entity<OrderRemark>());
        MapOrderStatus(modelBuilder.Entity<OrderStatus>());
        MapOrderStatusAccess(modelBuilder.Entity<OrderStatusAccess>());
        MapOrderStatusGroup(modelBuilder.Entity<OrderStatusGroup>());
        MapOrderStructure(modelBuilder.Entity<OrderStructure>());
        MapOrderTemplate(modelBuilder.Entity<OrderTemplate>());
        MapOrderTemplateDetail(modelBuilder.Entity<OrderTemplateDetail>());
        MapOrderType(modelBuilder.Entity<OrderType>());
        MapPaymentMethod(modelBuilder.Entity<PaymentMethod>());
        MapPaymentTerm(modelBuilder.Entity<PaymentTerm>());
        MapPriceListCategory(modelBuilder.Entity<PriceListCategory>());
        MapPriceListText(modelBuilder.Entity<PriceListText>());
        MapProduct(modelBuilder.Entity<Product>());
        MapProductDiscountSuggestion(modelBuilder.Entity<ProductDiscountSuggestion>());
        MapProductDiscountSuggestionLine(modelBuilder.Entity<ProductDiscountSuggestionLine>());
        MapProductManual(modelBuilder.Entity<ProductManual>());
        MapProductOption(modelBuilder.Entity<ProductOption>());
        MapProductOptionValue(modelBuilder.Entity<ProductOptionValue>());
        MapProductPopupReturnColumn(modelBuilder.Entity<ProductPopupReturnColumn>());
        MapProductPopupTemplate(modelBuilder.Entity<ProductPopupTemplate>());
        MapProductPopupTemplateLine(modelBuilder.Entity<ProductPopupTemplateLine>());
        MapProductPopupValueType(modelBuilder.Entity<ProductPopupValueType>());
        MapProductPrice(modelBuilder.Entity<ProductPrice>());
        MapProductPriceSalesDiscount(modelBuilder.Entity<ProductPriceSalesDiscount>());
        MapProductProductionGroup(modelBuilder.Entity<ProductProductionGroup>());
        MapProductProductionGroupLink(modelBuilder.Entity<ProductProductionGroupLink>());
        MapProductProperty(modelBuilder.Entity<ProductProperty>());
        MapProductPropertyItem(modelBuilder.Entity<ProductPropertyItem>());
        MapProductPurchaseDiscount(modelBuilder.Entity<ProductPurchaseDiscount>());
        MapProductQuantityTier(modelBuilder.Entity<ProductQuantityTier>());
        MapProductStockLocation(modelBuilder.Entity<ProductStockLocation>());
        MapProductStructure(modelBuilder.Entity<ProductStructure>());
        MapProductSubProduct(modelBuilder.Entity<ProductSubProduct>());
        MapProductType(modelBuilder.Entity<ProductType>());
        MapProductUnit(modelBuilder.Entity<ProductUnit>());
        MapProject(modelBuilder.Entity<Project>());
        MapProjectActivity(modelBuilder.Entity<ProjectActivity>());
        MapProjectContact(modelBuilder.Entity<ProjectContact>());
        MapProjectInstallation(modelBuilder.Entity<ProjectInstallation>());
        MapProjectLog(modelBuilder.Entity<ProjectLog>());
        MapProjectParty(modelBuilder.Entity<ProjectParty>());
        MapProjectPartyContact(modelBuilder.Entity<ProjectPartyContact>());
        MapProjectPartyGroup(modelBuilder.Entity<ProjectPartyGroup>());
        MapRawMaterial(modelBuilder.Entity<RawMaterial>());
        MapRepairCostPrice(modelBuilder.Entity<RepairCostPrice>());
        MapReportingGroup(modelBuilder.Entity<ReportingGroup>());
        MapSalutation(modelBuilder.Entity<Salutation>());
        MapServiceRate(modelBuilder.Entity<ServiceRate>());
        MapSickLeave(modelBuilder.Entity<SickLeave>());
        MapStaffUser(modelBuilder.Entity<StaffUser>());
        MapStandardBillingTerm(modelBuilder.Entity<StandardBillingTerm>());
        MapStandardBillingTermLine(modelBuilder.Entity<StandardBillingTermLine>());
        MapStockLocation(modelBuilder.Entity<StockLocation>());
        MapStockMovement(modelBuilder.Entity<StockMovement>());
        MapStockOrder(modelBuilder.Entity<StockOrder>());
        MapStockOrderDelivery(modelBuilder.Entity<StockOrderDelivery>());
        MapStockOrderLine(modelBuilder.Entity<StockOrderLine>());
        MapStoredFile(modelBuilder.Entity<StoredFile>());
        MapSupplier(modelBuilder.Entity<Supplier>());
        MapSupplierContact(modelBuilder.Entity<SupplierContact>());
        MapTaskAction(modelBuilder.Entity<TaskAction>());
        MapTaskDependency(modelBuilder.Entity<TaskDependency>());
        MapTaskItem(modelBuilder.Entity<TaskItem>());
        MapTaskTemplate(modelBuilder.Entity<TaskTemplate>());
        MapTaskTemplateDependency(modelBuilder.Entity<TaskTemplateDependency>());
        MapTaskTemplateTask(modelBuilder.Entity<TaskTemplateTask>());
        MapTaskType(modelBuilder.Entity<TaskType>());
        MapTemplateType(modelBuilder.Entity<TemplateType>());
        MapTimesheet(modelBuilder.Entity<Timesheet>());
        MapUserGroup(modelBuilder.Entity<UserGroup>());
        MapVatType(modelBuilder.Entity<VatType>());
        MapWebshopProductStructure(modelBuilder.Entity<WebshopProductStructure>());
        MapWebshopStructure(modelBuilder.Entity<WebshopStructure>());
    }

    private static void MapAccountingDocument(EntityTypeBuilder<AccountingDocument> config)
    {
        config.ToTable("Documenten", "Boekhouding");
        config.HasKey(t => t.AccountingDocumentId);
        config.Property(t => t.DocumentCreatedAt).HasColumnName("DocAanmaakDatum");
        config.Property(t => t.DocumentVatAmount).HasColumnType("decimal(18,2)").HasColumnName("DocBedragBtw");
        config.Property(t => t.DocumentNetAmount).HasColumnType("decimal(18,2)").HasColumnName("DocBedragNetto");
        config.Property(t => t.DocumentTotalAmount).HasColumnType("decimal(18,2)").HasColumnName("DocBedragTotaal");
        config.Property(t => t.DocumentDate).HasColumnName("DocDatum");
        config.Property(t => t.IsFinal).HasColumnName("DocDefinitef");
        config.Property(t => t.AccountingDocumentId).ValueGeneratedOnAdd().HasColumnName("DocId");
        config.Property(t => t.DocumentCustomerBox).HasMaxLength(50).IsRequired().HasColumnName("DocKlantBus");
        config.Property(t => t.CustomerId).HasColumnName("DocKlantId");
        config.Property(t => t.DocumentCustomerName).HasMaxLength(150).IsRequired().HasColumnName("DocKlantNaam");
        config.Property(t => t.DocumentCustomerNumber).HasMaxLength(50).IsRequired().HasColumnName("DocKlantNr");
        config.Property(t => t.DocumentCustomerPostalCode).HasMaxLength(50).IsRequired().HasColumnName("DocKlantPostcode");
        config.Property(t => t.DocumentCustomerStreet).HasMaxLength(150).IsRequired().HasColumnName("DocKlantStraat");
        config.Property(t => t.DocumentCustomerCity).HasMaxLength(150).IsRequired().HasColumnName("DocKlantWoonplaats");
        config.Property(t => t.DocumentNumber).HasMaxLength(50).HasColumnName("DocNummer");
        config.Property(t => t.DocumentTypeId).HasColumnName("DocType");
        config.Property(t => t.CreatedBy).HasColumnName("AangemaaktDoor");
        config.Property(t => t.OrderId).HasColumnName("DocBestellingId");
        config.Property(t => t.DocumentCustomerCompanyName).HasMaxLength(150).IsRequired().HasColumnName("DocKlantBedrijfsnaam");
        config.Property(t => t.DocumentCustomerVatNumber).HasMaxLength(50).IsRequired().HasColumnName("DocKlantBtwnr");
        config.Property(t => t.DocumentCustomerCountry).HasMaxLength(50).IsRequired().HasColumnName("DocKlantLand");
        config.Property(t => t.DocumentCustomerHouseNumber).HasMaxLength(50).IsRequired().HasColumnName("DocKlantHuisNr");
        config.Property(t => t.Vervaldatum);
        config.Property(t => t.GecrediteerdeFactuur).HasMaxLength(50);
        config.Property(t => t.DocGecrediteerdeFactuurDatum);
        config.Property(t => t.ProjectContactNaam).HasMaxLength(250);
        config.Property(t => t.ProjectContactPhone).HasMaxLength(50);
        config.Property(t => t.ProjectContactMobile).HasMaxLength(50);
        config.Property(t => t.ProjectContactEmail).HasMaxLength(250);
        config.Property(t => t.LeverAdresContactNaam).HasMaxLength(250);
        config.Property(t => t.LeverAdresContactPhone).HasMaxLength(50);
        config.Property(t => t.LeverAdresContactMobile).HasMaxLength(50);
        config.Property(t => t.LeverAdresContactEmail).HasMaxLength(250);
        config.Property(t => t.EindklantContactNaam).HasMaxLength(250);
        config.Property(t => t.EindklantContactPhone).HasMaxLength(50);
        config.Property(t => t.EindklantContactMobile).HasMaxLength(50);
        config.Property(t => t.EindklantContactEmail).HasMaxLength(250);
        config.Property(t => t.DeliveryDate).HasColumnName("Leveringsdatum");
        config.Property(t => t.BaseCompanyId);
        config.Property(t => t.DocumentCustomerLanguageId).HasColumnName("DocKlantTaal");
        config.Property(t => t.ProjectId).HasColumnName("ProjectProjectId");
        config.Property(t => t.LeverAdresStraat).HasMaxLength(150);
        config.Property(t => t.LeverAdresNr).HasMaxLength(20);
        config.Property(t => t.LeverAdresBus).HasMaxLength(20);
        config.Property(t => t.LeverAdresStad).HasMaxLength(150);
        config.Property(t => t.LeverAdresPostcode).HasMaxLength(20);
        config.Property(t => t.Notes).HasColumnName("Opmerking");
        config.Property(t => t.LeverAdresLand).HasMaxLength(100);
        config.Property(t => t.IsVoorschotFactuur);
        config.Property(t => t.ReedsGefactureerdVoorschot).HasColumnType("decimal(18,2)");
        config.Property(t => t.VoorschotNaam).HasMaxLength(500);
        config.Property(t => t.HeeftCommisie);
        config.Property(t => t.VoorschotPercentage).HasColumnType("decimal(18,4)");
        config.Property(t => t.VerzondenVia).HasMaxLength(500);
        config.Property(t => t.DossierBeheerder).HasMaxLength(50);
        config.Property(t => t.ProjectManagerUserId).HasMaxLength(50).HasColumnName("ProjectBeheerder");
        config.Property(t => t.AccountManagerUserId).HasMaxLength(50).HasColumnName("KlantVerantwoordelijke");
        config.Property(t => t.BetaaldOp);
        config.Property(t => t.BetalingswijzeId);
        config.Property(t => t.Reason).HasColumnName("Reden");
        config.Property(t => t.ToelichtingVoorschotten);
        config.Property(t => t.BaseCompanyVatNumberId);
        config.Property(t => t.DocKlantGebouwNaam).HasMaxLength(400);
        config.Property(t => t.DocumentOpmerking);
        config.Property(t => t.CountryId);
        config.Property(t => t.PeppolVerzondenOp);
        config.Property(t => t.EasypostId).HasMaxLength(250);
        config.Property(t => t.PeppolStatus).HasMaxLength(250);
    }

    private static void MapAccountingDocumentLine(EntityTypeBuilder<AccountingDocumentLine> config)
    {
        config.ToTable("DocumentDetail", "Boekhouding");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.GroupName).HasMaxLength(50).IsRequired().HasColumnName("Groep");
        config.Property(t => t.GateId).HasColumnName("PoortId");
        config.Property(t => t.ProjectId);
        config.Property(t => t.ProductId);
        config.Property(t => t.ProductName).HasMaxLength(512).IsRequired().HasColumnName("ProductNaam");
        config.Property(t => t.ProductOmschrijving).IsRequired();
        config.Property(t => t.Quantity).HasColumnType("decimal(18,4)").HasColumnName("Aantal");
        config.Property(t => t.EenheidsPrijs).HasColumnType("decimal(18,4)");
        config.Property(t => t.Unit).HasMaxLength(50).IsRequired().HasColumnName("Eenheid");
        config.Property(t => t.DiscountPercentage).HasColumnType("decimal(18,4)").HasColumnName("KortingPercentage");
        config.Property(t => t.Subtotaal).HasColumnType("decimal(18,4)");
        config.Property(t => t.BtwPercentage).HasColumnType("decimal(18,4)");
        config.Property(t => t.TotalAmount).HasColumnType("decimal(18,4)").HasColumnName("Totaal");
        config.Property(t => t.DocumentId);
        config.Property(t => t.InstallationPrice).HasColumnType("decimal(18,4)").HasColumnName("MontagePrijs");
        config.Property(t => t.AssemblyPrice).HasColumnType("decimal(18,4)").HasColumnName("AssemblagePrijs");
        config.Property(t => t.IsOption).HasColumnName("IsOptie");
        config.Property(t => t.BtwBedrag).HasColumnType("decimal(18,4)");
        config.Property(t => t.KortingBedrag).HasColumnType("decimal(18,4)");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.PrijslijsType).HasMaxLength(20);
        config.Property(t => t.GroepNaam).HasMaxLength(500).IsRequired();
        config.Property(t => t.IsOptieVanBestellingDetailId);
        config.Property(t => t.ProductType);
        config.Property(t => t.OrderLineId).HasColumnName("BestellingDetailId");
        config.Property(t => t.GateComponentId).HasColumnName("PoortOnderdeelId");
        config.Property(t => t.LeveringAfhalingOkOp);
        config.Property(t => t.NettoCommisieEenheidsPrijs).HasColumnType("decimal(18,4)");
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.BestelNummer).HasMaxLength(150);
        config.Property(t => t.BasePrice).HasColumnType("decimal(18,2)").HasColumnName("BasisPrijs");
        config.Property(t => t.KortingType).HasMaxLength(2).IsRequired();
        config.Property(t => t.BebatProductId);
        config.Property(t => t.RecupelProductId);
        config.Property(t => t.BebatNaam).HasMaxLength(250);
        config.Property(t => t.RecupelNaam).HasMaxLength(500);
        config.Property(t => t.BebatStukPrijs).HasColumnType("decimal(18,2)");
        config.Property(t => t.BebatAantal).HasColumnType("decimal(18,2)");
        config.Property(t => t.BebatTotaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.RecupelStukPrijs).HasColumnType("decimal(18,2)");
        config.Property(t => t.RecupelAantal).HasColumnType("decimal(18,2)");
        config.Property(t => t.RecupelTotaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.MontageStukPrijs).HasColumnType("decimal(18,2)");
        config.Property(t => t.AssemblageStukPrijs).HasColumnType("decimal(18,2)");
        config.Property(t => t.IsProducedCompositeProduct).HasColumnName("GeproduceerdSamenGesteldproduct");
        config.Property(t => t.IsVoorschot);
        config.Property(t => t.IsTextOnly);
        config.Property(t => t.KlaarVoorVerzendingOp);
        config.Property(t => t.DeliveredAt).HasColumnName("GeleverdOp");
        config.Property(t => t.NietTellenWegensVoorschot);
        config.Property(t => t.IsGarantie);
        config.Property(t => t.IsPopUpRow);
        config.Property(t => t.Notes).HasColumnName("Opmerking");
        config.Property(t => t.BasisPrijsTotaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.StartupCost).HasColumnType("decimal(18,4)").HasColumnName("Opstartkost");
        config.Property(t => t.OpstartKostTotaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.VatTypeId).HasColumnName("BtwTypeId");
        config.Property(t => t.SupplierId).HasColumnName("SupplierSupplierId");
        config.Property(t => t.DocumentDetailMasterId);
        config.Property(t => t.DetailVanMasterId);
        config.Property(t => t.AankoopStukPrijs).HasColumnType("decimal(18,2)");
        config.Property(t => t.Goederen).HasColumnType("decimal(18,2)");
        config.Property(t => t.Diensten).HasColumnType("decimal(18,2)");
        config.Property(t => t.GoodsCode).HasMaxLength(50).HasColumnName("GoederenCode");
        config.Property(t => t.Weight).HasColumnType("decimal(18,3)").HasColumnName("Gewicht");
        config.Property(t => t.AanvullendeEenheden).HasColumnType("decimal(18,0)");
        config.Property(t => t.LandVanOorsprong);
    }

    private static void MapActivity(EntityTypeBuilder<Activity> config)
    {
        config.ToTable("Activiteiten", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("Naam");
    }

    private static void MapAppError(EntityTypeBuilder<AppError> config)
    {
        config.ToTable("Error", "Logging");
        config.HasKey(t => t.Id);
        config.Property(t => t.DateTime);
        config.Property(t => t.ModuleName).HasMaxLength(50).IsRequired();
        config.Property(t => t.Exception).HasMaxLength(1024).IsRequired();
        config.Property(t => t.InnerExceptionMessage).HasMaxLength(1024).IsRequired().HasColumnName("InnerException");
        config.Property(t => t.UserName).HasMaxLength(50).IsRequired();
        config.Property(t => t.ClassName).HasMaxLength(50).IsRequired();
        config.Property(t => t.Id);
    }

    private static void MapAppSetting(EntityTypeBuilder<AppSetting> config)
    {
        config.ToTable("Parameter", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Value).IsRequired().HasColumnName("Waarde");
        config.Property(t => t.Type).HasMaxLength(50).IsRequired();
        config.Property(t => t.BaseCompanyId);
    }

    private static void MapAutoNumbering(EntityTypeBuilder<AutoNumbering> config)
    {
        config.ToTable("AutoNummering", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Number).HasColumnName("Nummer");
        config.Property(t => t.Description).HasMaxLength(150).IsRequired().HasColumnName("Omschrijving");
        config.Property(t => t.Prefix).HasMaxLength(50).IsRequired();
        config.Property(t => t.Tag).HasMaxLength(50).IsRequired();
    }

    private static void MapAzureFile(EntityTypeBuilder<AzureFile> config)
    {
        config.ToTable("AzureFile", "Bestanden");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(250).IsRequired();
        config.Property(t => t.Extension).HasMaxLength(50).IsRequired();
        config.Property(t => t.AzureFileFolderId);
        config.Property(t => t.Created);
        config.Property(t => t.CreatedByUserId);
        config.Property(t => t.Description).HasMaxLength(500).IsRequired();
        config.Property(t => t.BlobRef).HasMaxLength(2000).IsRequired();
        config.Property(t => t.ThumbRef).HasMaxLength(2000);
        config.Property(t => t.Modified);
        config.Property(t => t.ModifiedByUserId);
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.ProjectId).HasColumnName("ProjectProjectId");
        config.Property(t => t.EmailId);
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
        config.Property(t => t.Deleted);
        config.Property(t => t.DeletedByUserId);
        config.Property(t => t.ProductId);
        config.Property(t => t.IsPrimaryImage);
        config.Property(t => t.PublishToWeb);
        config.Property(t => t.UserId);
        config.Property(t => t.IsGeneral);
        config.Property(t => t.SupplierId).HasColumnName("SupplierSupplierId");
        config.Property(t => t.ManufacturerId).HasColumnName("ManufacturerManufacturerId");
        config.Property(t => t.OrderLineId).HasColumnName("DossierDetailId");
        config.Property(t => t.IsLinkedRef);
        config.Property(t => t.SendToCustomer).HasColumnName("VerzendenNaarKlant");
        config.Property(t => t.SendOnSupplierOrder).HasColumnName("VerzendenBijBestellingLeverancier");
        config.Property(t => t.StockOrderId);
    }

    private static void MapAzureFileFolder(EntityTypeBuilder<AzureFileFolder> config)
    {
        config.ToTable("AzureFileFolder", "Bestanden");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired();
        config.Property(t => t.IsForCrm);
        config.Property(t => t.IsForOrder).HasColumnName("IsForDossier");
        config.Property(t => t.IsForProject);
        config.Property(t => t.IsForProduct);
        config.Property(t => t.IsForUser);
        config.Property(t => t.IsForGeneralUse);
        config.Property(t => t.SortOrder).HasColumnType("decimal(10,2)").HasColumnName("Volgorde");
    }

    private static void MapBaseCompany(EntityTypeBuilder<BaseCompany> config)
    {
        config.ToTable("BaseCompany", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(250).IsRequired();
        config.Property(t => t.Street).HasMaxLength(250).IsRequired();
        config.Property(t => t.StreetNr).HasMaxLength(50).IsRequired();
        config.Property(t => t.StreetBox).HasMaxLength(50).IsRequired();
        config.Property(t => t.City).HasMaxLength(250).IsRequired();
        config.Property(t => t.Zip).HasMaxLength(50).IsRequired();
        config.Property(t => t.Country).HasMaxLength(50).IsRequired();
        config.Property(t => t.CustomerId).HasColumnName("KlantId");
        config.Property(t => t.Logo);
        config.Property(t => t.VatNumber).HasMaxLength(50).IsRequired();
        config.Property(t => t.Tel).HasMaxLength(50).IsRequired();
        config.Property(t => t.FaxTemplate).HasMaxLength(50).IsRequired().HasColumnName("Fax");
        config.Property(t => t.IBAN).HasMaxLength(50).IsRequired();
        config.Property(t => t.BIC).HasMaxLength(50).IsRequired();
        config.Property(t => t.Slogan).HasMaxLength(250).IsRequired();
        config.Property(t => t.AccountingDocumentFooter).IsRequired().HasColumnName("BhDocumentFooter");
        config.Property(t => t.Tag).HasMaxLength(50).IsRequired();
        config.Property(t => t.MsGraphApiApplicationId).HasMaxLength(150);
        config.Property(t => t.MsGraphApiSecretId).HasMaxLength(150);
        config.Property(t => t.MsGraphApiDomain).HasMaxLength(150);
        config.Property(t => t.MsGraphApiTenantId).HasMaxLength(150);
        config.Property(t => t.FileShareComputername).HasMaxLength(512);
        config.Property(t => t.FileShareUser).HasMaxLength(512);
        config.Property(t => t.FileSharePassword).HasMaxLength(512);
        config.Property(t => t.FileShareShare).HasMaxLength(512);
        config.Property(t => t.FileShareAccountName).HasMaxLength(512);
        config.Property(t => t.Eori).HasMaxLength(150);
        config.Property(t => t.Website).HasMaxLength(250);
        config.Property(t => t.AccentColor);
        config.Property(t => t.Bank).HasMaxLength(250).IsRequired();
        config.Property(t => t.EmailTemplate).HasMaxLength(150).HasColumnName("EMail");
        config.Property(t => t.AllowAddNewBlobFiles).HasColumnName("AllowAddNewFilesBlob");
    }

    private static void MapBaseCompanyAcces(EntityTypeBuilder<BaseCompanyAcces> config)
    {
        config.ToTable("BaseCompanyAccess", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.BaseCompanyId);
        config.Property(t => t.UserId);
    }

    private static void MapBaseCompanyVatNumber(EntityTypeBuilder<BaseCompanyVatNumber> config)
    {
        config.ToTable("BaseCompanyVatNumber", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.BaseCompanyId);
        config.Property(t => t.VatNumber).HasMaxLength(50).IsRequired();
        config.Property(t => t.EoriNumber).HasMaxLength(50).IsRequired();
        config.Property(t => t.Bank1).HasMaxLength(100);
        config.Property(t => t.Bank2).HasMaxLength(100);
        config.Property(t => t.Bank1Name).HasMaxLength(100);
        config.Property(t => t.Bank2Name).HasMaxLength(100);
        config.Property(t => t.Bank1Bic).HasMaxLength(50);
        config.Property(t => t.Bank2Bic).HasMaxLength(50);
    }

    private static void MapBillingAgreement(EntityTypeBuilder<BillingAgreement> config)
    {
        config.ToTable("FacturatieAfspraak", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerId).HasColumnName("KlantId");
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.Percentage).HasColumnType("decimal(18,4)");
        config.Property(t => t.CustomerName).HasMaxLength(250).IsRequired().HasColumnName("Klantnaam");
        config.Property(t => t.VatPercentage).HasColumnType("decimal(18,2)").HasColumnName("Btwperecentage");
    }

    private static void MapCalendarEntry(EntityTypeBuilder<CalendarEntry> config)
    {
        config.ToTable("Agenda", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Type);
        config.Property(t => t.StartDate).HasColumnName("StarteDate");
        config.Property(t => t.EndDate);
        config.Property(t => t.IsAllDay).HasColumnName("AllDay");
        config.Property(t => t.Subject).HasMaxLength(650).HasColumnName("Onderwerp");
        config.Property(t => t.Location).HasMaxLength(150).HasColumnName("Locatie");
        config.Property(t => t.Description).HasColumnName("Omschrijving");
        config.Property(t => t.StatusId).HasColumnName("Status");
        config.Property(t => t.LabelId).HasColumnName("Label");
        config.Property(t => t.ReminderInfo);
        config.Property(t => t.RecurrenceInfo);
        config.Property(t => t.OutlookId).HasMaxLength(500);
        config.Property(t => t.UserId);
        config.Property(t => t.CustomerId).HasColumnName("KlantId");
        config.Property(t => t.ProjectId);
        config.Property(t => t.IsSyncedToExchange).HasColumnName("SyncedToExchange");
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.IsOnHold).HasColumnName("OnHold");
        config.Property(t => t.IsCancelled).HasColumnName("Cancelled");
        config.Property(t => t.OnHoldOrCancelledReason).HasMaxLength(500).HasColumnName("RedenOnHoldAndCancelled");
        config.Property(t => t.SubjectUserText).HasMaxLength(500).HasColumnName("OnderwerpUserText");
        config.Property(t => t.ContactContactId);
        config.Property(t => t.SystemDescription).HasColumnName("OmschrijvingSysteem");
        config.Property(t => t.IsLeave).HasColumnName("IsVerlof");
        config.Property(t => t.CreatedByUserId).HasColumnName("GemaaktDoorUserId");
        config.Property(t => t.CreatedAt).HasColumnName("GemaaktOp");
    }

    private static void MapCalendarLabel(EntityTypeBuilder<CalendarLabel> config)
    {
        config.ToTable("AgendaLabel", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Color).HasColumnName("Kleur");
        config.Property(t => t.IsType);
        config.Property(t => t.IsCategory).HasColumnName("IsCategorie");
        config.Property(t => t.RestrictEditing).HasColumnName("BeperktBewerken");
        config.Property(t => t.IsForLeave).HasColumnName("IsVoorVerlof");
        config.Property(t => t.IsInternalService).HasColumnName("IsBinnenDienst");
        config.Property(t => t.IsExternalService).HasColumnName("IsBuitenDienst");
        config.Property(t => t.IsProjectPlanning);
    }

    private static void MapCalendarLog(EntityTypeBuilder<CalendarLog> config)
    {
        config.ToTable("AgendaLog", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Subject).HasMaxLength(650);
        config.Property(t => t.Start);
        config.Property(t => t.End);
        config.Property(t => t.ChangeAction).HasMaxLength(50).IsRequired();
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.ProjectId).HasColumnName("ProjectProjectId");
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
        config.Property(t => t.ContactContactId);
        config.Property(t => t.IsLeave).HasColumnName("IsVerlof");
        config.Property(t => t.ChangedAt).HasColumnName("ChangeTime");
        config.Property(t => t.UserId);
        config.Property(t => t.CalendarEntryId).HasColumnName("AgendaId");
        config.Property(t => t.ChangedByUserId);
    }

    private static void MapCalendarStatus(EntityTypeBuilder<CalendarStatus> config)
    {
        config.ToTable("AgendaStatus", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Color).HasColumnName("Kleur");
        config.Property(t => t.ShowInTodo).HasColumnName("ToonInTodo");
    }

    private static void MapCity(EntityTypeBuilder<City> config)
    {
        config.ToTable("City", "Crm");
        config.HasKey(t => t.CityId);
        config.Property(t => t.CityId).ValueGeneratedOnAdd();
        config.Property(t => t.CityName).HasMaxLength(150).IsRequired();
        config.Property(t => t.PostalCode).HasMaxLength(50).IsRequired().HasColumnName("CityZip");
        config.Property(t => t.CountryName).HasMaxLength(150).IsRequired().HasColumnName("CityCountry");
        config.Property(t => t.CountryIsoCode).HasMaxLength(50);
        config.Property(t => t.CountryId);
    }

    private static void MapContact(EntityTypeBuilder<Contact> config)
    {
        config.ToTable("Contact", "Klanten");
        config.HasKey(t => t.ContactId);
        config.Property(t => t.ContactId).ValueGeneratedOnAdd();
        config.Property(t => t.ContactBox).HasMaxLength(50).IsRequired().HasColumnName("ContactBus");
        config.Property(t => t.ContactEmail).HasMaxLength(150).IsRequired();
        config.Property(t => t.ContactFax).HasMaxLength(50).IsRequired();
        config.Property(t => t.ContactHouseNumber).HasMaxLength(50).IsRequired().HasColumnName("ContactHuisnr");
        config.Property(t => t.ContactLogin).HasMaxLength(50).IsRequired();
        config.Property(t => t.ContactMobile).HasMaxLength(50).IsRequired().HasColumnName("ContactMobiel");
        config.Property(t => t.ContactLastName).HasMaxLength(50).IsRequired().HasColumnName("ContactNaam");
        config.Property(t => t.ContactPassword).HasMaxLength(50).IsRequired().HasColumnName("ContactPaswoord");
        config.Property(t => t.ContactStreet).HasMaxLength(150).IsRequired().HasColumnName("ContactStraat");
        config.Property(t => t.ContactPhone).HasMaxLength(50).IsRequired().HasColumnName("ContactTel");
        config.Property(t => t.ContactFirstName).HasMaxLength(150).IsRequired().HasColumnName("ContactVoornaam");
        config.Property(t => t.IsInstallerContact).HasColumnName("ContactIsMonteur");
        config.Property(t => t.ContactCityId).HasColumnName("ContactCity");
        config.Property(t => t.IsInternalUserContact).HasColumnName("ContactIsInternUsr");
        config.Property(t => t.SalutationId).HasColumnName("ContactAanspreking");
        config.Property(t => t.ContactLanguageId).HasColumnName("ContactTaal");
        config.Property(t => t.BaseCompanyId);
        config.Property(t => t.InstallerDisplayName).HasMaxLength(50).HasColumnName("ContactMonteurDisplayName");
        config.Property(t => t.ContactJobTitle).HasMaxLength(100).HasColumnName("ContactFunctie");
        config.Property(t => t.EmailQuote).HasColumnName("MailOfferte");
        config.Property(t => t.EmailOrderConfirmation).HasColumnName("MailOrderbevestiging");
        config.Property(t => t.EmailPlanning).HasColumnName("MailPlanning");
        config.Property(t => t.EmailDeliveryReady).HasColumnName("MailLeveringKlaar");
        config.Property(t => t.EmailDelivered).HasColumnName("MailGeleverd");
        config.Property(t => t.EmailBilling).HasColumnName("MailFacturatie");
        config.Property(t => t.LeftAt).HasColumnName("Uitdienst");
        config.Property(t => t.ContactBuilding).HasMaxLength(250).HasColumnName("ContactGebouw");
    }

    private static void MapContactProjectRole(EntityTypeBuilder<ContactProjectRole> config)
    {
        config.ToTable("ContactProjectRol", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameNl).HasMaxLength(150).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.NameFr).HasMaxLength(150).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.NameEn).HasMaxLength(150).IsRequired().HasColumnName("NaamEn");
        config.Property(t => t.DeactivateAfterOrderClosed).HasColumnName("SetNonActiveNaDossierAfgehandeld");
    }

    private static void MapCountry(EntityTypeBuilder<Country> config)
    {
        config.ToTable("Country", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.IsEu);
        config.Property(t => t.IsoCode).HasMaxLength(10).IsRequired().HasColumnName("Isocode");
        config.Property(t => t.Name).HasMaxLength(100).HasColumnName("Naam");
    }

    private static void MapCustomer(EntityTypeBuilder<Customer> config)
    {
        config.ToTable("Klant", "Klanten");
        config.HasKey(t => t.CustomerId);
        config.Property(t => t.CustomerId).ValueGeneratedOnAdd().HasColumnName("KlantId");
        config.Property(t => t.CustomerVatNumber).HasMaxLength(50).IsRequired().HasColumnName("KlantBtw");
        config.Property(t => t.CustomerBox).HasMaxLength(50).IsRequired().HasColumnName("KlantBus");
        config.Property(t => t.CustomerHouseNumber).HasMaxLength(50).IsRequired().HasColumnName("KlantHuisnr");
        config.Property(t => t.CustomerName).HasMaxLength(150).IsRequired().HasColumnName("KlantNaam");
        config.Property(t => t.CustomerStreet).HasMaxLength(150).IsRequired().HasColumnName("KlantStraat");
        config.Property(t => t.CustomerTypeId).HasColumnName("KlantType");
        config.Property(t => t.CustomerPhone).HasMaxLength(50).IsRequired().HasColumnName("KlantAlgTel");
        config.Property(t => t.CustomerFax).HasMaxLength(50).IsRequired().HasColumnName("KlantAlgFax");
        config.Property(t => t.CustomerNotes).HasMaxLength(2024).HasColumnName("KlantOpmerking");
        config.Property(t => t.CustomerEmail).HasMaxLength(250).IsRequired().HasColumnName("KlantEmail");
        config.Property(t => t.CustomerVatSystemId).HasColumnName("KlantBtwsysteem");
        config.Property(t => t.CustomerStatusId).HasColumnName("KlantStatusId");
        config.Property(t => t.CustomerCityId).HasColumnName("KlantCity");
        config.Property(t => t.CustomerActivityId).HasColumnName("KlantActiviteit");
        config.Property(t => t.CustomerNumber).HasColumnName("KlantNummer");
        config.Property(t => t.AccountManagerUserId).HasColumnName("KlantVerantwoordelijke");
        config.Property(t => t.FirstContactName).HasMaxLength(150).HasColumnName("EersteContact");
        config.Property(t => t.LockedTime);
        config.Property(t => t.Locked);
        config.Property(t => t.LockedBy).HasMaxLength(100).IsRequired();
        config.Property(t => t.RevenueLast12Months).HasColumnType("decimal(18,2)").HasColumnName("OmzetLaatste12Maanden");
        config.Property(t => t.SendPriceListByEmail).HasColumnName("PrijslijstViaEmail");
        config.Property(t => t.PromotionalMailing).HasColumnName("PromotieMailing");
        config.Property(t => t.DeliveryTypeId).HasColumnName("LeverigsType");
        config.Property(t => t.InvoicesByMail).HasColumnName("FacturenPerPost");
        config.Property(t => t.CustomerInternalNotes).HasMaxLength(2024).HasColumnName("KlantInterneOpmerking");
        config.Property(t => t.DigitalInvoicing).HasColumnName("DigitaleFacturatie");
        config.Property(t => t.CElabelName).HasMaxLength(50);
        config.Property(t => t.CElabelNr).HasMaxLength(50);
        config.Property(t => t.CustomerPaymentStatus).HasColumnName("KlantBetalingStatus");
        config.Property(t => t.BaseCompaniesId);
        config.Property(t => t.IsInternalCompany);
        config.Property(t => t.CustomerLanguageId).HasColumnName("KlantTaal");
        config.Property(t => t.PriceListResContractor).HasColumnName("PrijslijstResAannemer");
        config.Property(t => t.PriceListResDealer).HasColumnName("PrijslijstResDealer");
        config.Property(t => t.PriceListResConsumer).HasColumnName("PrijslijstResParticulier");
        config.Property(t => t.PriceListIndContractor).HasColumnName("PrijslijstIndaannemer");
        config.Property(t => t.PriceListIndDealer).HasColumnName("PrijslijstIndDealer");
        config.Property(t => t.CEemail).HasMaxLength(250);
        config.Property(t => t.Logo);
        config.Property(t => t.CustomerGroup).HasMaxLength(50).IsRequired().HasColumnName("KlantGroep");
        config.Property(t => t.CreatedBy).HasMaxLength(50).HasColumnName("AangemaaktDoor");
        config.Property(t => t.CreatedAt).HasColumnName("AangemaaktOp");
        config.Property(t => t.ModifiedBy).HasMaxLength(50).HasColumnName("AangepastDoor");
        config.Property(t => t.ModifiedAt).HasColumnName("AangepastOp");
        config.Property(t => t.IsVerified).HasColumnName("Gecontroleerd");
        config.Property(t => t.QuoteContactId).HasColumnName("ContactOfferteContactId");
        config.Property(t => t.OrderConfirmationContactId).HasColumnName("ContactOrderbevestigingContactId");
        config.Property(t => t.PlanningContactId).HasColumnName("ContactPlanningContactId");
        config.Property(t => t.DeliveryCompleteContactId).HasColumnName("ContactLeveringCompleetContactId");
        config.Property(t => t.BillingContactId).HasColumnName("ContactFacturatieContactId");
        config.Property(t => t.CommissionRecipientId).HasColumnName("CommisieOntvanger");
        config.Property(t => t.RequestedCommission).HasColumnType("decimal(18,4)").HasColumnName("GevraagdeCommisie");
        config.Property(t => t.BetaaltermijnId);
        config.Property(t => t.WebshopLogin).HasMaxLength(150).HasColumnName("LoginWebshop");
        config.Property(t => t.WebshopPasswordHash).HasMaxLength(512).HasColumnName("PasswordWebshop");
        config.Property(t => t.WebshopPasswordSalt).HasMaxLength(512).HasColumnName("SaltWebshop");
        config.Property(t => t.CustomerBuildingName).HasMaxLength(250).HasColumnName("KlantAdresBuilding");
        config.Property(t => t.LaatsteFollowUp);
        config.Property(t => t.DeliveryCustomerTypeId).HasColumnName("LeveringKlantTypeId");
        config.Property(t => t.PeppolIdSchema).HasMaxLength(8);
        config.Property(t => t.PeppolId).HasMaxLength(250);
    }

    private static void MapCustomerContact(EntityTypeBuilder<CustomerContact> config)
    {
        config.ToTable("KlantContact", "Klanten");
        config.HasKey(t => t.CustomerContactId);
        config.Property(t => t.CustomerContactId).ValueGeneratedOnAdd().HasColumnName("KlantContactId");
        config.Property(t => t.CustomerId).HasColumnName("KlantContactKlantId");
        config.Property(t => t.ContactId).HasColumnName("KlantContactContactId");
        config.Property(t => t.Notes).HasMaxLength(250).HasColumnName("KlantContactOpmerking");
        config.Property(t => t.IsDefaultContact).HasColumnName("DefaultConact");
        config.Property(t => t.JobTitle).HasMaxLength(100).HasColumnName("Functie");
        config.Property(t => t.SupplierId).HasColumnName("SupplierSupplierId");
        config.Property(t => t.ManufacturerId).HasColumnName("ManufacturerManufacturerId");
    }

    private static void MapCustomerCustomProduct(EntityTypeBuilder<CustomerCustomProduct> config)
    {
        config.ToTable("KlantMaatProduct", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Description).IsRequired().HasColumnName("Omschrijvig");
        config.Property(t => t.ProductEenheidId);
        config.Property(t => t.ArticleNumber).HasMaxLength(250).HasColumnName("Artikelnummer");
        config.Property(t => t.Notes).HasMaxLength(1024).HasColumnName("Opmerking");
        config.Property(t => t.IsActive).HasColumnName("Actief");
        config.Property(t => t.SupplierArticleNumber).HasMaxLength(250).HasColumnName("ArtikelNummerLeverancier");
    }

    private static void MapCustomerCustomProductLine(EntityTypeBuilder<CustomerCustomProductLine> config)
    {
        config.ToTable("KlantMaatProductDetail", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(250).IsRequired();
        config.Property(t => t.RawMaterialId).HasColumnName("GrondstofId");
        config.Property(t => t.SupplierId);
        config.Property(t => t.Quantity).HasColumnType("decimal(18,4)").HasColumnName("Aantal");
        config.Property(t => t.ProductEenheidId);
        config.Property(t => t.PurchasePrice).HasColumnType("decimal(18,4)").HasColumnName("AankoopPrijs");
        config.Property(t => t.CustomerCustomProductId).HasColumnName("KlantMaatProductId");
        config.Property(t => t.ArticleNumber).HasMaxLength(250).HasColumnName("Artikelnummer");
    }

    private static void MapCustomerCustomProductTier(EntityTypeBuilder<CustomerCustomProductTier> config)
    {
        config.ToTable("KlantMaatproductStaffel", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerCustomProductId).HasColumnName("KlantMaatProductId");
        config.Property(t => t.MinQuantity).HasColumnType("decimal(18,2)").HasColumnName("MinAantal");
        config.Property(t => t.MaxQuantity).HasColumnType("decimal(18,2)").HasColumnName("MaxAantal");
        config.Property(t => t.PieceUnitPrice).HasColumnType("decimal(18,4)").HasColumnName("StukPrijs");
        config.Property(t => t.ValidFrom).HasColumnName("VanDatum");
        config.Property(t => t.ValidTo).HasColumnName("TotDatum");
    }

    private static void MapCustomerDeliveredProduct(EntityTypeBuilder<CustomerDeliveredProduct> config)
    {
        config.ToTable("BinnengebrachtProduct", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.Name).HasMaxLength(1024).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Explanation).IsRequired().HasColumnName("Toelichting");
        config.Property(t => t.ReceivedAt).HasColumnName("OntvangenOp");
        config.Property(t => t.ReceivedByUserId).HasColumnName("OntvangenDoorUserId");
        config.Property(t => t.BroughtBy).HasMaxLength(150).IsRequired().HasColumnName("GebrachtDoor");
        config.Property(t => t.PickedUpAt).HasColumnName("OpgehaaldDatum");
        config.Property(t => t.PickedUpBy).HasMaxLength(150).HasColumnName("OpgehaaldDoor");
        config.Property(t => t.SentToSupplierAt).HasColumnName("VerzondenNaarLeverancierOp");
        config.Property(t => t.TrackingNumber).HasMaxLength(150).HasColumnName("TrackingNr");
        config.Property(t => t.ReturnedFromSupplierAt).HasColumnName("TerugOntvangenVanLeverancierOp");
        config.Property(t => t.IrreparableAt).HasColumnName("OnherstelbaarDatum");
    }

    private static void MapCustomerDeliveryAddress(EntityTypeBuilder<CustomerDeliveryAddress> config)
    {
        config.ToTable("KlantLeveradres", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Straat).HasMaxLength(250).IsRequired();
        config.Property(t => t.Number).HasMaxLength(50).IsRequired().HasColumnName("Nummer");
        config.Property(t => t.Bus).HasMaxLength(50).IsRequired();
        config.Property(t => t.CityId).HasColumnName("Stad");
        config.Property(t => t.Notes).HasMaxLength(2000).IsRequired().HasColumnName("Opmerking");
        config.Property(t => t.ContactId).HasColumnName("Contact");
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
    }

    private static void MapCustomerExtraDiscount(EntityTypeBuilder<CustomerExtraDiscount> config)
    {
        config.ToTable("KlantExtraKortingen", "Boekhouding");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Discount).HasColumnType("decimal(18,2)").HasColumnName("Korting");
        config.Property(t => t.MaxAmount).HasColumnType("decimal(18,2)").HasColumnName("TotBedrag");
        config.Property(t => t.MinAmount).HasColumnType("decimal(18,2)").HasColumnName("VanBedrag");
        config.Property(t => t.CustomerTypeFilterId).HasColumnName("TypeKlant");
        config.Property(t => t.BaseCompanyId);
    }

    private static void MapCustomerFollowUp(EntityTypeBuilder<CustomerFollowUp> config)
    {
        config.ToTable("KlantFollowUp", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Text).IsRequired().HasColumnName("Tekst");
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
        config.Property(t => t.Date).HasColumnName("Datum");
        config.Property(t => t.UserId);
    }

    private static void MapCustomerJobCodeRate(EntityTypeBuilder<CustomerJobCodeRate> config)
    {
        config.ToTable("KlantJobcodeTarief", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
        config.Property(t => t.JobCodeId);
        config.Property(t => t.HourlyRate).HasColumnType("decimal(18,2)").HasColumnName("Uurtarief");
    }

    private static void MapCustomerNote(EntityTypeBuilder<CustomerNote> config)
    {
        config.ToTable("KlantOpmerkingen", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.CustomerId).HasColumnName("KlantId");
        config.Property(t => t.DocumentTypeId).HasColumnName("DocumentType");
        config.Property(t => t.Notes).IsRequired().HasColumnName("Opmerking");
        config.Property(t => t.Id).ValueGeneratedOnAdd();
    }

    private static void MapCustomerOrderStatusRemark(EntityTypeBuilder<CustomerOrderStatusRemark> config)
    {
        config.ToTable("KlantDossierStatusOpmerking", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OrderStatusId).HasColumnName("BestellingStatusId");
        config.Property(t => t.Notes).HasMaxLength(250).IsRequired().HasColumnName("Opmerking");
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
    }

    private static void MapCustomerProductDiscount(EntityTypeBuilder<CustomerProductDiscount> config)
    {
        config.ToTable("KlantProductKorting", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
        config.Property(t => t.DiscountPercentage).HasColumnType("decimal(18,4)").HasColumnName("KortingPercentage");
        config.Property(t => t.ProductId).HasColumnName("ProdId");
        config.Property(t => t.Notes).HasMaxLength(500).HasColumnName("Opmerking");
        config.Property(t => t.FromAddress).HasColumnName("Van");
        config.Property(t => t.ValidTo).HasColumnName("Tot");
        config.Property(t => t.CustomerTypeId).HasColumnName("KlantTypeKlantTypeId");
        config.Property(t => t.CreatedAt).HasColumnName("AangemaaktOp");
        config.Property(t => t.UserId);
        config.Property(t => t.Margin).HasColumnType("decimal(18,4)").HasColumnName("Marge");
        config.Property(t => t.InstallationDiscountPercentage).HasColumnType("decimal(18,4)").HasColumnName("KortingsPercentageMontage");
        config.Property(t => t.InstallationCustomerTypeId).HasColumnName("MontageKlantTypeId");
    }

    private static void MapCustomerStatus(EntityTypeBuilder<CustomerStatus> config)
    {
        config.ToTable("KlantStatus", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Color).HasColumnName("Kleur");
        config.Property(t => t.IsDefault).HasColumnName("Default");
    }

    private static void MapCustomerSupplierDiscount(EntityTypeBuilder<CustomerSupplierDiscount> config)
    {
        config.ToTable("KlantLeverancierKorting", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
        config.Property(t => t.DiscountPercentage).HasColumnType("decimal(18,4)").HasColumnName("KortingPercentage");
        config.Property(t => t.SupplierId).HasColumnName("SupplierSupplierId");
        config.Property(t => t.Notes).HasMaxLength(500).HasColumnName("Opmerking");
        config.Property(t => t.FromAddress).HasColumnName("Van");
        config.Property(t => t.ValidTo).HasColumnName("Tot");
        config.Property(t => t.CustomerTypeId).HasColumnName("KlantTypeKlantTypeId");
        config.Property(t => t.CreatedAt).HasColumnName("AangemaaktOp");
        config.Property(t => t.UserId);
        config.Property(t => t.InstallationCustomerTypeId).HasColumnName("MontageKlantTypeId");
        config.Property(t => t.InstallationDiscountPercentage).HasColumnType("decimal(18,4)").HasColumnName("MontageKortingPercentage");
    }

    private static void MapCustomerType(EntityTypeBuilder<CustomerType> config)
    {
        config.ToTable("KlantType", "Klanten");
        config.HasKey(t => t.KlantTypeId);
        config.Property(t => t.KlantTypeId).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerTypeName).HasMaxLength(50).IsRequired().HasColumnName("KlantTypeNaam");
        config.Property(t => t.RequiresVatNumber).HasColumnName("KlantTypeBtwNrVerplicht");
        config.Property(t => t.PaymentTermId).HasColumnName("Betaaltermijn");
        config.Property(t => t.VatSystemId).HasColumnName("BtwSysteem");
        config.Property(t => t.BaseDiscount).HasColumnType("decimal(18,4)").HasColumnName("BasisKorting");
        config.Property(t => t.DeliveryTypeId).HasColumnName("LeveringsType");
        config.Property(t => t.CustomerTypeNameFr).HasMaxLength(50).IsRequired().HasColumnName("KlantTypeNaamFr");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.IsDefault);
    }

    private static void MapDeliveryType(EntityTypeBuilder<DeliveryType> config)
    {
        config.ToTable("LeveringType", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.IncludeInstallationCost).HasColumnName("MontageKostTellen");
        config.Property(t => t.NameFr).HasMaxLength(50).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.IsDefault);
    }

    private static void MapDocumentTemplate(EntityTypeBuilder<DocumentTemplate> config)
    {
        config.ToTable("Templates", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(500).IsRequired().HasColumnName("Naam");
        config.Property(t => t.EmailTemplate).HasColumnName("Email");
        config.Property(t => t.FaxTemplate).HasColumnName("Fax");
        config.Property(t => t.LetterTemplate).HasColumnName("Brief");
        config.Property(t => t.IsDefault).HasColumnName("Default");
        config.Property(t => t.Type);
        config.Property(t => t.BaseCompanyId);
        config.Property(t => t.TaalId);
        config.Property(t => t.AzureFileId);
        config.Property(t => t.Subject).HasMaxLength(250).IsRequired().HasColumnName("Onderwerp");
    }

    private static void MapDocumentType(EntityTypeBuilder<DocumentType> config)
    {
        config.ToTable("DocumentType", "Boekhouding");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("Naam");
        config.Property(t => t.NameFr).HasMaxLength(150).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.ParameterId);
        config.Property(t => t.NameEn).HasMaxLength(150).IsRequired().HasColumnName("NaamEn");
    }

    private static void MapDrawGroup(EntityTypeBuilder<DrawGroup> config)
    {
        config.ToTable("DrawGroup", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired();
    }

    private static void MapEmailAttachment(EntityTypeBuilder<EmailAttachment> config)
    {
        config.ToTable("Bijlage", "Emails");
        config.HasKey(t => t.Id);
        config.Property(t => t.StoredFileId).HasColumnName("BestandId");
        config.Property(t => t.EmailId);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.EmailFileName).HasMaxLength(500).IsRequired().HasColumnName("EmailBestandsNaam");
        config.Property(t => t.IsEmailOnlyFile).HasColumnName("EmailOnlyFile");
    }

    private static void MapEmailMessage(EntityTypeBuilder<EmailMessage> config)
    {
        config.ToTable("Email", "Emails");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerId).HasColumnName("KlantId");
        config.Property(t => t.ProjectId);
        config.Property(t => t.FromAddress).HasMaxLength(500).HasColumnName("Van");
        config.Property(t => t.ToAddress).HasMaxLength(500).HasColumnName("Aan");
        config.Property(t => t.Cc).HasMaxLength(500);
        config.Property(t => t.Bcc).HasMaxLength(500);
        config.Property(t => t.Subject).HasMaxLength(1000).IsRequired().HasColumnName("Onderwerp");
        config.Property(t => t.Body).IsRequired().HasColumnName("Inhoud");
        config.Property(t => t.SentAt).HasColumnName("Verzonden");
        config.Property(t => t.ReceivedAt).HasColumnName("Ontvangen");
        config.Property(t => t.ContactId);
        config.Ignore(t => t.RelatedSupplierId);
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.PreviewText).HasMaxLength(500).IsRequired();
        config.Property(t => t.TaskItemId).HasColumnName("TakenId");
        config.Property(t => t.UserId);
        config.Property(t => t.IsPrivate);
        config.Property(t => t.SupplierId);
        config.Property(t => t.ManufacturerId).HasColumnName("ManufacturerManufacturerId");
        config.Property(t => t.OrderLineId).HasColumnName("DossierDetailId");
        config.Property(t => t.RequiresAction).HasColumnName("TeBehandelen");
        config.Property(t => t.EmailQueueId);
        config.Property(t => t.StockOrderId);
    }

    private static void MapEmailQueue(EntityTypeBuilder<EmailQueue> config)
    {
        config.ToTable("EmailQueue", "Emails");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(100).IsRequired().HasColumnName("Naam");
    }

    private static void MapGridLayout(EntityTypeBuilder<GridLayout> config)
    {
        config.ToTable("GridLayout", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ObjectName).HasMaxLength(500).IsRequired();
        config.Property(t => t.LayoutXml).IsRequired();
        config.Property(t => t.Notes).HasMaxLength(150).HasColumnName("Opmerking");
        config.Property(t => t.UsrId);
        config.Property(t => t.IsPivot);
        config.Property(t => t.PivotName).HasMaxLength(50);
    }

    private static void MapIntrastatCode(EntityTypeBuilder<IntrastatCode> config)
    {
        config.ToTable("IntrastatCode", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Code).HasMaxLength(50).IsRequired();
        config.Property(t => t.Name).HasMaxLength(250);
        config.Property(t => t.Text);
        config.Property(t => t.MainGroup).HasMaxLength(500).HasColumnName("Hoofdgroep");
        config.Property(t => t.SubGroup).HasMaxLength(500).HasColumnName("SubGroep");
    }

    private static void MapIntrastatReportLine(EntityTypeBuilder<IntrastatReportLine> config)
    {
        config.ToTable("IntrastatReportLine", "Boekhouding");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.DocumentDocId);
        config.Property(t => t.ProductName).HasMaxLength(250).IsRequired().HasColumnName("ProductNaam");
        config.Property(t => t.PartnerLand).HasMaxLength(50).IsRequired();
        config.Property(t => t.TransactieCode).HasMaxLength(50).IsRequired();
        config.Property(t => t.Gewest).HasMaxLength(50).IsRequired();
        config.Property(t => t.GoodsCode).HasMaxLength(50).IsRequired().HasColumnName("GoederenCode");
        config.Property(t => t.Weight).HasColumnType("decimal(18,3)").HasColumnName("Gewicht");
        config.Property(t => t.AanvullendeEenheden).HasColumnType("decimal(18,2)");
        config.Property(t => t.WaardeInEur).HasColumnType("decimal(18,2)");
        config.Property(t => t.Vervoer).HasMaxLength(50);
        config.Property(t => t.Incoterm).HasMaxLength(50);
        config.Property(t => t.LandVanOorsprong).HasMaxLength(50).IsRequired();
        config.Property(t => t.BtwNrTegenpartij).HasMaxLength(50).IsRequired();
    }

    private static void MapJobCode(EntityTypeBuilder<JobCode> config)
    {
        config.ToTable("JobCode", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Code).HasMaxLength(10).IsRequired();
        config.Property(t => t.NameNl).HasMaxLength(150).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.NameFr).HasMaxLength(150).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.NameEn).HasMaxLength(150).IsRequired().HasColumnName("NaamEn");
        config.Property(t => t.HourlyRate).HasColumnType("decimal(18,2)").HasColumnName("UurPrijs");
        config.Property(t => t.IsBillable).HasColumnName("Factureerbaar");
    }

    private static void MapJobSite(EntityTypeBuilder<JobSite> config)
    {
        config.ToTable("Werf", "Projecten");
        config.HasKey(t => t.JobSiteId);
        config.Property(t => t.SiteStreet).HasMaxLength(150).IsRequired().HasColumnName("WerfAdres");
        config.Property(t => t.SiteBox).HasMaxLength(50).IsRequired().HasColumnName("WerfBus");
        config.Property(t => t.SiteHouseNumber).HasMaxLength(50).IsRequired().HasColumnName("WerfHuisnr");
        config.Property(t => t.JobSiteId).ValueGeneratedOnAdd().HasColumnName("WerfId");
        config.Property(t => t.SiteCityId).HasColumnName("WerfPlaats");
        config.Property(t => t.EndCustomerEmail).HasMaxLength(150).IsRequired().HasColumnName("WerfEindKlantEmail");
        config.Property(t => t.EndCustomerMobile).HasMaxLength(50).IsRequired().HasColumnName("WerfEindKlantGsm");
        config.Property(t => t.EndCustomerName).HasMaxLength(250).IsRequired().HasColumnName("WerfEindKlantNaam");
        config.Property(t => t.EndCustomerPhone).HasMaxLength(50).IsRequired().HasColumnName("WerfEindKlantTel");
        config.Property(t => t.SiteNotes).HasMaxLength(2000).HasColumnName("WerfOpmerking");
    }

    private static void MapLanguage(EntityTypeBuilder<Language> config)
    {
        config.ToTable("Taal", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Tag).HasMaxLength(3).IsRequired();
        config.Property(t => t.NameFr).HasMaxLength(50).IsRequired().HasColumnName("NaamFR");
    }

    private static void MapLanguageTag(EntityTypeBuilder<LanguageTag> config)
    {
        config.ToTable("LangTag", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.FieldName).HasMaxLength(250).IsRequired();
        config.Property(t => t.NL).IsRequired();
        config.Property(t => t.FR).IsRequired();
        config.Property(t => t.SourceKey).HasMaxLength(250).IsRequired().HasColumnName("From");
        config.Property(t => t.En).IsRequired();
    }

    private static void MapMaintenanceContract(EntityTypeBuilder<MaintenanceContract> config)
    {
        config.ToTable("OnderhoudsContract", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(500).IsRequired().HasColumnName("Naam");
        config.Property(t => t.FromAddress).HasColumnName("Van");
        config.Property(t => t.ValidTo).HasColumnName("Tot");
        config.Property(t => t.Amount).HasColumnType("decimal(18,2)").HasColumnName("Bedrag");
        config.Property(t => t.AutoRenew).HasColumnName("AutomatischVerlengen");
        config.Property(t => t.CancelledAt).HasColumnName("OpgezegdOp");
        config.Property(t => t.ReminderDate).HasColumnName("RappelDatum");
        config.Property(t => t.ProjectId).HasColumnName("ProjectProjectId");
        config.Property(t => t.CycleDays).HasColumnName("CyclusDagen");
        config.Property(t => t.ReminderDaysInAdvance).HasColumnName("AantaldagenOpvoorhandRappelleren");
        config.Property(t => t.VatTypeId).HasColumnName("BtwTypeId");
        config.Property(t => t.SuccessorUserId).HasColumnName("OndershoudsContractOpvolgerUserId");
    }

    private static void MapMaintenanceContractLine(EntityTypeBuilder<MaintenanceContractLine> config)
    {
        config.ToTable("OnderhoudsContractDetail", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OnderhoudsContractId);
        config.Property(t => t.ProjectInstallatieId);
    }

    private static void MapManufacturer(EntityTypeBuilder<Manufacturer> config)
    {
        config.ToTable("Manufacturer", "Crm");
        config.HasKey(t => t.ManufacturerId);
        config.Property(t => t.ManufacturerId).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("ManufacturerName");
        config.Property(t => t.Phone).HasMaxLength(50).HasColumnName("ManufacturerPhone");
        config.Property(t => t.Mobile).HasMaxLength(50).HasColumnName("ManufacturerMobile");
        config.Property(t => t.Fax).HasMaxLength(50).HasColumnName("ManufacturerFax");
        config.Property(t => t.Email).HasMaxLength(150).HasColumnName("ManufacturerEmail");
        config.Property(t => t.Address).HasMaxLength(250).HasColumnName("ManufacturerAddress");
        config.Property(t => t.CityId).HasColumnName("ManufacturerCity");
        config.Property(t => t.CompanyRegistrationNumber).HasMaxLength(50).HasColumnName("ManufacturerKBO");
        config.Property(t => t.VatNumber).HasMaxLength(50).HasColumnName("ManufacturerVAT");
    }

    private static void MapMiscellaneousProduct(EntityTypeBuilder<MiscellaneousProduct> config)
    {
        config.ToTable("LosseProducten", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.ArticleNumber).HasMaxLength(100).HasColumnName("ArtikelNr");
        config.Property(t => t.StockLocationCode).HasMaxLength(100).HasColumnName("Stockplaats");
        config.Property(t => t.Notes).HasMaxLength(250).HasColumnName("Opmerking");
        config.Property(t => t.PurchasePrice).HasColumnType("decimal(18,2)").HasColumnName("AankoopPrijs");
        config.Property(t => t.SupplierName).HasMaxLength(100).HasColumnName("Leverancier");
        config.Property(t => t.AantalInStock).HasColumnType("decimal(18,2)");
        config.Property(t => t.LastCountedAt).HasColumnName("LaatsteKeerGeteld");
        config.Property(t => t.GroupName).HasMaxLength(150).HasColumnName("Groep");
    }

    private static void MapOrder(EntityTypeBuilder<Order> config)
    {
        config.ToTable("Bestelling", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.IsAccepted).HasColumnName("Geaccepteerd");
        config.Property(t => t.CreatedAt).HasColumnName("GemaaktOp");
        config.Property(t => t.CreatedByUserId).HasColumnName("GemaaktDoorUsrId");
        config.Property(t => t.ProjectId);
        config.Property(t => t.GeneralDiscount).HasColumnType("decimal(18,4)").HasColumnName("AlgemeneKorting");
        config.Property(t => t.DeliveryTypeId).HasColumnName("LeveringsType");
        config.Property(t => t.PriceListTypeId).HasColumnName("PrijslijstType");
        config.Property(t => t.QuoteId).HasColumnName("OfferteId");
        config.Property(t => t.OrderNumber).HasColumnName("Dossiernummer");
        config.Property(t => t.CommissionAmount).HasColumnType("decimal(18,4)").HasColumnName("Commissiebedrag");
        config.Property(t => t.InstallerContactId).HasColumnName("MonteurContactId");
        config.Property(t => t.RequestedDeliveryDate).HasColumnName("GewensteOpleveringsdatum");
        config.Property(t => t.InstallationDays).HasColumnName("MontageDuurInDagen");
        config.Property(t => t.ProductionDays).HasColumnName("ProductieDuurInDagen");
        config.Property(t => t.MasterOrderId).HasColumnName("MasterDossier");
        config.Property(t => t.VatTypeId).HasColumnName("BtwTypeId");
        config.Property(t => t.OrderProcessingTypeId).HasColumnName("DossierVerwerkingsTypeId");
        config.Property(t => t.CustomerTypeId).HasColumnName("KlantTypeKlantTypeId");
        config.Property(t => t.InternalNotes).HasColumnName("InterneNota");
        config.Property(t => t.RalColor).HasMaxLength(150).HasColumnName("RalKleur");
        config.Property(t => t.AdvanceInvoiceEnabled).HasColumnName("VoorschotFacturatie");
        config.Property(t => t.ExtraDiscount).HasColumnType("decimal(18,4)").HasColumnName("ExtraKorting");
        config.Property(t => t.CustomerNotes).HasColumnName("KlantOpmerking");
        config.Property(t => t.InstallerNotes).HasColumnName("MonteurOpmerking");
        config.Property(t => t.InternalStaffNotes).HasColumnName("AbmaticOpmerking");
        config.Property(t => t.AllowPartialDelivery).HasColumnName("DeelsLeveringToestaan");
        config.Property(t => t.CommissionSalesUserId).HasColumnName("CommissieVerkoper");
        config.Property(t => t.IsCommissionInvoiced).HasColumnName("CommissieGefactureerd");
        config.Property(t => t.CommissionToInvoice).HasColumnType("decimal(18,2)").HasColumnName("TefacturerenCommissie");
        config.Property(t => t.LeveradresId);
        config.Property(t => t.BetaaltermijnId);
        config.Property(t => t.PopupMessage).HasColumnName("PopupMelding");
        config.Property(t => t.QuoteValidDays).HasColumnName("OfferteAantalDagenGeldig");
        config.Property(t => t.IsUrgent).HasColumnName("IsDringend");
        config.Property(t => t.PriceListDate).HasColumnName("PrijslijstDatum");
        config.Property(t => t.BaseCompanyVatNumberId);
        config.Property(t => t.AdvancePaymentsByAmount).HasColumnName("VoorschottenOpBasisVanBedragen");
        config.Property(t => t.HasCloudFolder).HasColumnName("HeeftCloudMap");
        config.Property(t => t.IsClosingVerified).HasColumnName("AfsluitingGecontroleerd");
        config.Property(t => t.InvoiceNotes).HasColumnName("OpmerkingFactuur");
        config.Property(t => t.QuoteNotesHeader).HasColumnName("OpmerkingOfferte");
    }

    private static void MapOrderAdvancePayment(EntityTypeBuilder<OrderAdvancePayment> config)
    {
        config.ToTable("DossierVoorschot", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Percent).HasColumnType("decimal(18,4)");
        config.Property(t => t.IsFinalInvoice).HasColumnName("IsEindFactuur");
        config.Property(t => t.InvoicedAt).HasColumnName("GefactureerdOp");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.Amount).HasColumnType("decimal(18,6)").HasColumnName("Bedrag");
        config.Property(t => t.AdvancePaymentVisibility).HasMaxLength(50).HasColumnName("Voorschotzichtbaarheid");
    }

    private static void MapOrderDeliveryTypeProduct(EntityTypeBuilder<OrderDeliveryTypeProduct> config)
    {
        config.ToTable("DossierLeveringsTypeProduct", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.LeveringTypeId);
        config.Property(t => t.ProductId).HasColumnName("ProductProdId");
    }

    private static void MapOrderDevelopmentLine(EntityTypeBuilder<OrderDevelopmentLine> config)
    {
        config.ToTable("DossierDevelopmentDetail", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).HasColumnName("Guid");
        config.Property(t => t.ArticleNumber).HasMaxLength(250).IsRequired().HasColumnName("ArtikelNr");
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Description).HasColumnName("Omschrijving");
        config.Property(t => t.ProductId).HasColumnName("ProductProdId");
        config.Property(t => t.MasterRowId).HasColumnName("MasterRowGuid");
        config.Property(t => t.Quantity).HasColumnType("decimal(18,3)").HasColumnName("Aantal");
        config.Property(t => t.ProductEenheidId);
        config.Property(t => t.ReservedQuantity).HasColumnType("decimal(18,4)").HasColumnName("AantalGereserveerd");
        config.Property(t => t.QuantityTakenFromStock).HasColumnType("decimal(18,4)").HasColumnName("AantalUitstock");
        config.Property(t => t.MustOrder).HasColumnName("TeBestellen");
        config.Property(t => t.OrderedAt).HasColumnName("BesteldOp");
        config.Property(t => t.DeliveredAt).HasColumnName("GeleverdOp");
        config.Property(t => t.SupplierId).HasColumnName("SupplierSupplierId");
        config.Property(t => t.PurchasePrice).HasColumnType("decimal(18,2)").HasColumnName("AankoopPrijs");
        config.Property(t => t.TotalPurchasePrice).HasColumnType("decimal(18,0)").HasColumnName("TotaalAankoop");
        config.Property(t => t.DrawingCreated).HasColumnName("TekeningGemaakt");
        config.Property(t => t.MaterialType).HasMaxLength(150).HasColumnName("TypeMateriaal");
        config.Property(t => t.Processing).HasMaxLength(150).HasColumnName("Bewerking");
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.Weight).HasColumnType("decimal(18,2)").HasColumnName("Gewicht");
        config.Property(t => t.Finish).HasColumnName("Afwerking");
        config.Property(t => t.StockOrderDetailId);
        config.Property(t => t.RequiresPainting).HasColumnName("Lakken");
        config.Property(t => t.PaintWorkSupplierId).HasColumnName("LakwerkSupplierSupplierId");
    }

    private static void MapOrderFeedback(EntityTypeBuilder<OrderFeedback> config)
    {
        config.ToTable("DossierFeedback", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.Bucket).HasMaxLength(150).IsRequired();
        config.Property(t => t.Date).HasColumnName("Datum");
        config.Property(t => t.UserId);
        config.Property(t => t.Text).HasColumnName("Tekst");
    }

    private static void MapOrderFileLink(EntityTypeBuilder<OrderFileLink> config)
    {
        config.ToTable("DossierBestanden", "Bestanden");
        config.HasKey(t => t.OrderFileLinkId);
        config.Property(t => t.StoredFileId).HasColumnName("DosBestandBestandId");
        config.Property(t => t.OrderId).HasColumnName("DosBestandDossierId");
        config.Property(t => t.OrderFileLinkId).ValueGeneratedOnAdd().HasColumnName("DosBestandId");
    }

    private static void MapOrderInstallationLine(EntityTypeBuilder<OrderInstallationLine> config)
    {
        config.ToTable("DossierInstallatieDetail", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ItemNumber).HasMaxLength(50).IsRequired().HasColumnName("ItemNr");
        config.Property(t => t.PartNumber).HasMaxLength(250).IsRequired().HasColumnName("OnderdeelNummer");
        config.Property(t => t.Description).HasMaxLength(500).HasColumnName("Omschrijving");
        config.Property(t => t.Material).HasMaxLength(150).HasColumnName("Materiaal");
        config.Property(t => t.Quantity).HasColumnType("decimal(18,4)").HasColumnName("Aantal");
        config.Property(t => t.QuantityInInstallation).HasColumnType("decimal(18,4)").HasColumnName("AantalInInstallatie");
        config.Property(t => t.Processing).HasMaxLength(150).HasColumnName("Bewerking");
        config.Property(t => t.SupplierId).HasColumnName("LeverancierId");
        config.Property(t => t.SupplierArticleNumber).HasMaxLength(150).HasColumnName("LeverancierArtikelNr");
        config.Property(t => t.Treatment).HasMaxLength(50).HasColumnName("Behandeling");
        config.Property(t => t.OrderedAt).HasColumnName("Besteldatum");
        config.Property(t => t.DeliveredAt).HasColumnName("Leverdatum");
        config.Property(t => t.UnitPrice).HasColumnType("decimal(18,4)").HasColumnName("PrijsPerStuk");
        config.Property(t => t.TotalPrice).HasColumnType("decimal(18,4)").HasColumnName("PrijsTotaal");
        config.Property(t => t.Notes).HasMaxLength(1000).HasColumnName("Opmerking");
        config.Property(t => t.OrderId).HasColumnName("DossierId");
    }

    private static void MapOrderLine(EntityTypeBuilder<OrderLine> config)
    {
        config.ToTable("BestellingDetail", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OrderId).HasColumnName("BestellingId");
        config.Property(t => t.GateId).HasColumnName("PoortId");
        config.Property(t => t.ProductId);
        config.Property(t => t.Quantity).HasColumnType("decimal(18,4)").HasColumnName("Aantal");
        config.Property(t => t.UnitPrice).HasColumnType("decimal(18,4)").HasColumnName("PrijsPerEenheid");
        config.Property(t => t.AssemblyPrice).HasColumnType("decimal(18,4)").HasColumnName("AssemblagePrijs");
        config.Property(t => t.InstallationPrice).HasColumnType("decimal(18,4)").HasColumnName("MontagePrijs");
        config.Property(t => t.TotalExclVat).HasColumnType("decimal(18,4)").HasColumnName("TotaalExcl");
        config.Property(t => t.TotalInclVat).HasColumnType("decimal(18,4)").HasColumnName("TotaalIncl");
        config.Property(t => t.IsOption).HasColumnName("IsOptie");
        config.Property(t => t.Discount).HasColumnType("decimal(18,4)").HasColumnName("Korting");
        config.Property(t => t.Btw).HasColumnType("decimal(18,4)");
        config.Property(t => t.UpliftType).HasMaxLength(50).IsRequired();
        config.Property(t => t.DocumentDisplayName).HasMaxLength(500).IsRequired().HasColumnName("NaamVoorDocument");
        config.Property(t => t.Lengte).HasColumnType("decimal(18,4)");
        config.Property(t => t.Hoogte).HasColumnType("decimal(18,4)");
        config.Property(t => t.Breedte).HasColumnType("decimal(18,4)");
        config.Property(t => t.Assemblage);
        config.Property(t => t.Montage);
        config.Property(t => t.QuoteNotesHeader).IsRequired().HasColumnName("OpmerkingOfferte");
        config.Property(t => t.ProductionNotes).HasColumnName("OpmerkingProductie");
        config.Property(t => t.MontageStukprijs).HasColumnType("decimal(18,4)");
        config.Property(t => t.AssemblageStukprijs).HasColumnType("decimal(18,4)");
        config.Property(t => t.ProductOptionId);
        config.Property(t => t.ProductOptionHoofdDetaillId);
        config.Property(t => t.SerialNumber).HasMaxLength(150).HasColumnName("Serienummer");
        config.Property(t => t.ProductionGroupId).HasMaxLength(50).IsRequired().HasColumnName("ProductieGroep");
        config.Property(t => t.AantalProductieVerwerkt).HasColumnType("decimal(18,4)");
        config.Property(t => t.AantalAfgehaaldOfGeleverd).HasColumnType("decimal(18,4)");
        config.Property(t => t.QuantityInvoiced).HasColumnType("decimal(18,4)").HasColumnName("AantalGefactureerd");
        config.Property(t => t.Afgehandeld);
        config.Property(t => t.PurchaseOrderNotes).IsRequired().HasColumnName("OpmerkingBestelBon");
        config.Property(t => t.PopUpDataXml);
        config.Property(t => t.ProductType);
        config.Property(t => t.InProductie);
        config.Property(t => t.Controleren);
        config.Property(t => t.ProductieKlaar);
        config.Property(t => t.LeveringOfAfhalingKlaar);
        config.Property(t => t.MontageKlaar);
        config.Property(t => t.Gefactureerd);
        config.Property(t => t.Tefactureren);
        config.Property(t => t.ProductieVerwerkOp);
        config.Property(t => t.ReadyForBillingDate).HasColumnName("DatumKlaarvoorFacturatie");
        config.Property(t => t.BtwBedrag).HasColumnType("decimal(18,4)");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.Tecrediteren);
        config.Property(t => t.GecrediteerdOp);
        config.Property(t => t.ToOrder);
        config.Property(t => t.Ordered);
        config.Property(t => t.OrderedDate);
        config.Property(t => t.DocumentDisplayNameFr).HasMaxLength(500).IsRequired().HasColumnName("NaamVoorDocumentFR");
        config.Property(t => t.NettoCommisieEenheidsPrijs).HasColumnType("decimal(18,4)");
        config.Property(t => t.IsSubProduct);
        config.Property(t => t.AantalSubProduct).HasColumnType("decimal(18,4)");
        config.Property(t => t.BestelNummer).HasMaxLength(100);
        config.Property(t => t.SupplierId).HasColumnName("SupplierSupplierId");
        config.Property(t => t.PurchasePrice).HasColumnType("decimal(18,2)").HasColumnName("AankoopPrijs");
        config.Property(t => t.ProductEenheidId);
        config.Property(t => t.InvoicedAt).HasColumnName("GefactureerdOp");
        config.Property(t => t.PrijsControleren);
        config.Property(t => t.AantalTeBestellen).HasColumnType("decimal(18,4)");
        config.Property(t => t.ProductStockLocatieId);
        config.Property(t => t.AantalInStockGereserveerd).HasColumnType("decimal(18,4)");
        config.Property(t => t.AantalDefinitiefUitStock).HasColumnType("decimal(18,4)");
        config.Property(t => t.NoPriceCacl);
        config.Property(t => t.AantalBeschikbaarInStock).HasColumnType("decimal(18,2)");
        config.Property(t => t.IsSamenGesteldProduct);
        config.Property(t => t.ReportingGroupId).HasColumnName("ReportingGroep1Id");
        config.Property(t => t.BrutoAankoopprijs).HasColumnType("decimal(18,4)");
        config.Property(t => t.NettoAankoopPrijs).HasColumnType("decimal(18,4)");
        config.Property(t => t.WinstPercentage).HasColumnType("decimal(18,4)");
        config.Property(t => t.StockOrderDetailId);
        config.Property(t => t.QuantityOrdered).HasColumnType("decimal(18,4)").HasColumnName("AantalBesteld");
        config.Property(t => t.OrderedAt).HasColumnName("BesteldOp");
        config.Property(t => t.ArticleNumber).HasMaxLength(250).HasColumnName("ArtikelNr");
        config.Property(t => t.OrigineleKorting).HasColumnType("decimal(18,4)");
        config.Property(t => t.OverRuleLosseLijnAutoKorting);
        config.Property(t => t.IsTextOnly);
        config.Property(t => t.UpliftTypeOrigineel).HasMaxLength(50).IsRequired();
        config.Property(t => t.BasePrice).HasColumnType("decimal(18,2)").HasColumnName("BasisPrijs");
        config.Property(t => t.BebatNaam).HasMaxLength(250);
        config.Property(t => t.BebatProductId);
        config.Property(t => t.RecupelNaam).HasMaxLength(500);
        config.Property(t => t.RecupelProductId);
        config.Property(t => t.BebatAantal).HasColumnType("decimal(10,2)");
        config.Property(t => t.BebatStukPrijs).HasColumnType("decimal(18,2)");
        config.Property(t => t.BebatTotaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.RecupelAantal).HasColumnType("decimal(10,2)");
        config.Property(t => t.RecupelStukPrijs).HasColumnType("decimal(18,2)");
        config.Property(t => t.RecupelTotaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.IsProducedCompositeProduct).HasColumnName("GeproduceerdSamenGesteldproduct");
        config.Property(t => t.IsLeveringsTypeProduct);
        config.Property(t => t.StandaardKortingsType).HasMaxLength(50);
        config.Property(t => t.StandaardKortingsPecentage).HasColumnType("decimal(18,2)");
        config.Property(t => t.IsGarantie);
        config.Property(t => t.TotaalExclVoorCommissie).HasColumnType("decimal(18,2)");
        config.Property(t => t.PopUpRow);
        config.Property(t => t.NodeLevel);
        config.Property(t => t.AfgehandeldOp);
        config.Property(t => t.Selected);
        config.Property(t => t.IsExtraKortingRow);
        config.Property(t => t.BestellingIsBinnenGekomen);
        config.Property(t => t.UnitParameter).HasColumnType("decimal(18,4)").HasColumnName("EenheidsParameter");
        config.Property(t => t.BasisPrijsTellen);
        config.Property(t => t.ShowOnQuote).HasColumnName("ToonOpOfferte");
        config.Property(t => t.ShowOnOrderConfirmation).HasColumnName("ToonOpOrderbevestiging");
        config.Property(t => t.ShowOnInvoice).HasColumnName("ToonOpFactuur");
        config.Property(t => t.ShowOnPackingSlip).HasColumnName("ToonOpPakbon");
        config.Property(t => t.ShowOnDeliveryNote).HasColumnName("ToonOpLeveringsbon");
        config.Property(t => t.ShowOnProductionOrder).HasColumnName("ToonOpProductieBon");
        config.Property(t => t.ToonOpLakBon);
        config.Property(t => t.ShowOnInstallationOrder).HasColumnName("ToonOpMontageBon");
        config.Property(t => t.Ral).HasMaxLength(50);
        config.Property(t => t.ToonOmschrijvingOpFactuur);
        config.Property(t => t.ToonOmschrijvingOpPakbon);
        config.Property(t => t.ToonOmschrijvingOpLeverbon);
        config.Property(t => t.ToonOmschrijvingOpProductiebon);
        config.Property(t => t.ToonOmschrijvingOpLakBon);
        config.Property(t => t.ToonOmschrijvingOpOfferte);
        config.Property(t => t.ToonOpVrachtbrief);
        config.Property(t => t.ToonOmschrijvingOpVrachtbrief);
        config.Property(t => t.ToonOmschrijvingOpOrderbevestiging);
        config.Property(t => t.StartupCost).HasColumnType("decimal(18,2)").HasColumnName("OpstartKost");
        config.Property(t => t.OpstartKostTotaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.BasisPrijsTotaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.VatTypeId).HasColumnName("BtwTypeId");
        config.Property(t => t.OrderTemplateId);
        config.Property(t => t.OrderTemplateDetailId);
        config.Property(t => t.PieceUnitPrice).HasColumnType("decimal(18,2)").HasColumnName("StukPrijs");
        config.Property(t => t.CustomerCustomProductId).HasColumnName("KlantMaatProductId");
        config.Property(t => t.TaxproductenGecontroleerd);
        config.Property(t => t.ReportRecupel).HasColumnName("RecupelRapporteren");
        config.Property(t => t.ReportBebat).HasColumnName("BebatRapporteren");
        config.Property(t => t.ToonGeenDetailPrijzen);
        config.Property(t => t.OfferteDocumentDocId);
        config.Property(t => t.OrderBevestigingDocumentDocId);
        config.Property(t => t.PakBonDocumentDocId);
        config.Property(t => t.LeveringsBonDocId);
        config.Property(t => t.FactuurDocId);
        config.Property(t => t.IsProduction).HasColumnName("IsProductie");
        config.Property(t => t.OmschrijvingProbleem);
        config.Property(t => t.KlantBinnenGebrachtOp);
        config.Property(t => t.KlantTerugOpgehaaldOp);
        config.Property(t => t.VanKlantInOntvangstGenomenDoor).HasMaxLength(50);
        config.Property(t => t.OmschrijvingHerstelling);
        config.Property(t => t.SentToSupplierAt).HasColumnName("VerzondenNaarLeverancierOp");
        config.Property(t => t.OntvangenVanLeverancierOp);
        config.Property(t => t.TrackingHerstelling).HasMaxLength(500);
        config.Property(t => t.IsHerstelling);
        config.Property(t => t.NieuwVervangToestelGegevenOp);
        config.Property(t => t.HerstelUren).HasColumnType("decimal(18,2)");
        config.Property(t => t.HerstellingAfgerondOp);
        config.Property(t => t.HerstellingsKostMateriaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.HerstellingUurtarief).HasColumnType("decimal(18,2)");
        config.Property(t => t.HerstellingsKostTotaal).HasColumnType("decimal(18,2)");
        config.Property(t => t.KortingOverride);
        config.Property(t => t.PrijsTypeOverride);
        config.Property(t => t.InterneHerstelling);
        config.Property(t => t.AdvancePaymentVisibility).HasMaxLength(50).IsRequired().HasColumnName("Voorschotzichtbaarheid");
        config.Property(t => t.Goederen).HasColumnType("decimal(18,2)");
        config.Property(t => t.Diensten).HasColumnType("decimal(18,2)");
        config.Property(t => t.GoodsCode).HasMaxLength(50).HasColumnName("GoederenCode");
        config.Property(t => t.Weight).HasColumnType("decimal(18,3)").HasColumnName("Gewicht");
        config.Property(t => t.AanvullendeEenheden).HasColumnType("decimal(18,0)");
        config.Property(t => t.LandVanOorsprong);
        config.Property(t => t.IntrastatReported);
        config.Property(t => t.IntrastatReportedOn);
    }

    private static void MapOrderLineText(EntityTypeBuilder<OrderLineText> config)
    {
        config.ToTable("DossierDetailText", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Text).IsRequired();
        config.Property(t => t.OrderBevestiging);
        config.Property(t => t.Leveringsbon);
        config.Property(t => t.Factuur);
        config.Property(t => t.AkOrder);
        config.Property(t => t.Offerte);
        config.Property(t => t.DossierDetailsId);
        config.Property(t => t.MontageBon);
        config.Property(t => t.Lakbon);
        config.Property(t => t.ProductieBon);
    }

    private static void MapOrderLog(EntityTypeBuilder<OrderLog> config)
    {
        config.ToTable("DossierLog", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id);
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.UserId);
        config.Property(t => t.Description).HasMaxLength(500).IsRequired().HasColumnName("Omschrijving");
    }

    private static void MapOrderProcessingType(EntityTypeBuilder<OrderProcessingType> config)
    {
        config.ToTable("DossierVerwerkingsType", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.CalculatePrice).HasColumnName("PrijsCalculatie");
        config.Property(t => t.OrderStatusId).HasColumnName("BestellingStatusId");
    }

    private static void MapOrderProjectLine(EntityTypeBuilder<OrderProjectLine> config)
    {
        config.ToTable("DossierProjectDetail", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).HasColumnName("Guid");
        config.Property(t => t.ArticleNumber).HasMaxLength(250).IsRequired().HasColumnName("ArtikelNr");
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Description).IsRequired().HasColumnName("Omschrijving");
        config.Property(t => t.ProductId).HasColumnName("ProductProdId");
        config.Property(t => t.MasterRowId).HasColumnName("MasterRowGuid");
        config.Property(t => t.Quantity).HasColumnType("decimal(18,3)").HasColumnName("Aantal");
        config.Property(t => t.ProductEenheidId);
        config.Property(t => t.ReservedQuantity).HasColumnType("decimal(18,4)").HasColumnName("AantalGereserveerd");
        config.Property(t => t.QuantityTakenFromStock).HasColumnType("decimal(18,4)").HasColumnName("AantalUitstock");
        config.Property(t => t.MustOrder).HasColumnName("TeBestellen");
        config.Property(t => t.OrderedAt).HasColumnName("BesteldOp");
        config.Property(t => t.DeliveredAt).HasColumnName("GeleverdOp");
        config.Property(t => t.SupplierId).HasColumnName("SupplierSupplierId");
        config.Property(t => t.PurchasePrice).HasColumnType("decimal(18,2)").HasColumnName("AankoopPrijs");
        config.Property(t => t.TotalPurchasePrice).HasColumnType("decimal(18,0)").HasColumnName("TotaalAankoop");
        config.Property(t => t.DrawingCreated).HasColumnName("TekeningGemaakt");
    }

    private static void MapOrderRemark(EntityTypeBuilder<OrderRemark> config)
    {
        config.ToTable("DossierOpmerking", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.CustomerNoteId).HasColumnName("KlantopmerkingId");
        config.Property(t => t.ProductionCategories).HasMaxLength(150).HasColumnName("ProductieCategories");
        config.Property(t => t.Notes).IsRequired().HasColumnName("Opmerking");
        config.Property(t => t.ProductionCategoryId).HasColumnName("ProductieCategory");
        config.Property(t => t.DocumentTypeId).HasColumnName("DocumentType");
    }

    private static void MapOrderStatus(EntityTypeBuilder<OrderStatus> config)
    {
        config.ToTable("BestellingStatus", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.SortOrder).HasColumnName("Sequence");
        config.Property(t => t.IncludeInSalesReporting).HasColumnName("IsForSalesReporting");
        config.Property(t => t.NameFr).HasMaxLength(50).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.ReportInProgress).HasColumnName("ReportInBehandeling");
        config.Property(t => t.ScreenMode).HasMaxLength(50).IsRequired().HasColumnName("ScreenModus");
        config.Property(t => t.OrderStatusGroupId).HasColumnName("DossierStatusGroepId");
        config.Property(t => t.ReserveStock).HasColumnName("DoStockReservering");
        config.Property(t => t.AffectsStock).HasColumnName("DoStock");
    }

    private static void MapOrderStatusAccess(EntityTypeBuilder<OrderStatusAccess> config)
    {
        config.ToTable("BestellingStatusToegangen", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OrderStatusId).HasColumnName("BesellingStatusId");
        config.Property(t => t.UserId);
    }

    private static void MapOrderStatusGroup(EntityTypeBuilder<OrderStatusGroup> config)
    {
        config.ToTable("DossierStatusGroep", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
    }

    private static void MapOrderStructure(EntityTypeBuilder<OrderStructure> config)
    {
        config.ToTable("DossierStructuur", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Number).HasMaxLength(1000).IsRequired().HasColumnName("Nummer");
        config.Property(t => t.MasterRowId);
        config.Property(t => t.OrderTypeId).HasColumnName("BestellingsTypeId");
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.SortOrder).HasColumnName("Sequence");
        config.Property(t => t.ProjectId).HasColumnName("ProjectProjectId");
        config.Property(t => t.CreatedAt).HasColumnName("GemaaktOp");
        config.Property(t => t.CreatedByUserId).HasColumnName("GemaaktDoor");
        config.Property(t => t.StatusId);
        config.Property(t => t.CustomerReference).HasMaxLength(1000).IsRequired().HasColumnName("ReferentieKlant");
        config.Property(t => t.OrderNumber).HasMaxLength(50).HasColumnName("DossierNummer");
        config.Property(t => t.Path).HasMaxLength(1024);
        config.Property(t => t.OrderConfirmationDate).HasColumnName("DatumOrderBevestiging");
        config.Property(t => t.PlannedDate).HasColumnName("DatumGepland");
        config.Property(t => t.ReadyForDeliveryDate).HasColumnName("DatumLeveringsKlaar");
        config.Property(t => t.ReadyForBillingDate).HasColumnName("DatumKlaarVoorFacturatie");
        config.Property(t => t.CompletedDate).HasColumnName("DatumAfgehandeld");
        config.Property(t => t.QuoteDate).HasColumnName("DatumOfferte");
        config.Property(t => t.TotalAmount).HasColumnType("decimal(18,2)").HasColumnName("Totaal");
        config.Property(t => t.TotalInvoiced).HasColumnType("decimal(18,2)").HasColumnName("TotaalGefactureerd");
        config.Property(t => t.TeamsId).HasMaxLength(250);
        config.Property(t => t.OrderConfirmedBy).HasMaxLength(50).HasColumnName("OrderbevestigingDoor");
        config.Property(t => t.PlannedBy).HasMaxLength(50).HasColumnName("GeplandDoor");
        config.Property(t => t.ReadyForDeliveryBy).HasMaxLength(50).HasColumnName("LeveringsklaarDoor");
        config.Property(t => t.CompletedBy).HasMaxLength(50).HasColumnName("AfgehandeldDoor");
        config.Property(t => t.QuoteBy).HasMaxLength(50).HasColumnName("OfferteDoor");
        config.Property(t => t.ReadyForBillingBy).HasMaxLength(50).HasColumnName("KlaarvoorFacturatieDoor");
        config.Property(t => t.HasPaperFile).HasColumnName("HeeftPapierenDossier");
        config.Property(t => t.QueuedAt).HasColumnName("KlaargezetOp");
    }

    private static void MapOrderTemplate(EntityTypeBuilder<OrderTemplate> config)
    {
        config.ToTable("OrderTemplate", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired();
        config.Property(t => t.IsActive).HasColumnName("Active");
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
    }

    private static void MapOrderTemplateDetail(EntityTypeBuilder<OrderTemplateDetail> config)
    {
        config.ToTable("OrderTemplateDetail", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ProductId).HasColumnName("ProductProdId");
        config.Property(t => t.SupplierId).HasColumnName("SupplierSupplierId");
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.GroupName).HasMaxLength(50).IsRequired().HasColumnName("Groep");
        config.Property(t => t.Quantity).HasColumnType("decimal(18,4)").HasColumnName("Aantal");
        config.Property(t => t.QuantityFormula).IsRequired().HasColumnName("AantalFormule");
        config.Property(t => t.UnitPrice).HasColumnType("decimal(18,4)").HasColumnName("PrijsPerStuk");
        config.Property(t => t.TotalAmount).HasColumnType("decimal(18,4)").HasColumnName("Totaal");
        config.Property(t => t.OrderTemplateId);
        config.Property(t => t.PriceFormula).IsRequired().HasColumnName("PrijsFormule");
        config.Property(t => t.ProductEenheidId);
    }

    private static void MapOrderType(EntityTypeBuilder<OrderType> config)
    {
        config.ToTable("BestellingType", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.SerialNumberSuffix).HasMaxLength(50).IsRequired().HasColumnName("SerienrSuffix");
        config.Property(t => t.OrderProcessingTypeId).HasColumnName("DossierVerwerkingsTypeId");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.NameFr).HasMaxLength(50).IsRequired().HasColumnName("NaamFr");
    }

    private static void MapPaymentMethod(EntityTypeBuilder<PaymentMethod> config)
    {
        config.ToTable("Betalingswijze", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameNl).HasMaxLength(150).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.NameFr).HasMaxLength(150).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.NameEn).HasMaxLength(150).IsRequired().HasColumnName("NaamEn");
        config.Property(t => t.IsPrePay);
        config.Property(t => t.IsPostPay);
    }

    private static void MapPaymentTerm(EntityTypeBuilder<PaymentTerm> config)
    {
        config.ToTable("Betaaltermijn", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("Naam");
        config.Property(t => t.AantalDagen);
        config.Property(t => t.EndOfMonth).HasColumnName("Eindemaand");
        config.Property(t => t.IsDefault);
    }

    private static void MapPriceListCategory(EntityTypeBuilder<PriceListCategory> config)
    {
        config.ToTable("PrijslijstCategorie", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.HasOptions).HasColumnName("Opties");
        config.Property(t => t.Color).HasMaxLength(50).HasColumnName("Kleur");
        config.Property(t => t.NameFr).HasMaxLength(250).IsRequired().HasColumnName("NaamFr");
    }

    private static void MapPriceListText(EntityTypeBuilder<PriceListText> config)
    {
        config.ToTable("PrijslijstTeksten", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.BaseCompanyId);
        config.Property(t => t.Text).IsRequired().HasColumnName("Tekst");
        config.Property(t => t.TextFr).IsRequired().HasColumnName("TekstFr");
        config.Property(t => t.TextEn).IsRequired().HasColumnName("TekstEn");
    }

    private static void MapProduct(EntityTypeBuilder<Product> config)
    {
        config.ToTable("Product", "Products");
        config.HasKey(t => t.ProductId);
        config.Property(t => t.ProductId).ValueGeneratedOnAdd().HasColumnName("ProdId");
        config.Property(t => t.NameNl).HasMaxLength(150).IsRequired().HasColumnName("ProdName");
        config.Property(t => t.DescriptionNl).IsRequired().HasColumnName("ProdDescription");
        config.Property(t => t.OrderPartNumber).HasMaxLength(150).IsRequired().HasColumnName("ProdOrderPartNumber");
        config.Property(t => t.StockNumber).HasMaxLength(50).IsRequired().HasColumnName("ProdStockNumber");
        config.Property(t => t.SupplierId).HasColumnName("ProdSupplier");
        config.Property(t => t.ManufacturerId).HasColumnName("ProdManufacturer");
        config.Property(t => t.ProductTypeId);
        config.Property(t => t.IsInactive).HasColumnName("ProdNonActive");
        config.Property(t => t.UnitsPerSale).HasColumnType("decimal(18,4)").HasColumnName("ProdVerpaktPerVerkoop");
        config.Property(t => t.UnitsPerPurchase).HasColumnType("decimal(18,4)").HasColumnName("ProdVerpaktPerAankoop");
        config.Property(t => t.PriceListSortOrder).HasColumnName("ProdPriceListOrder");
        config.Property(t => t.ShowOnPriceList).HasColumnName("ProdPrijsLijst");
        config.Property(t => t.ShortNotesNl).HasMaxLength(500).HasColumnName("ProdKorteOpmerking");
        config.Property(t => t.ProdToonOmschrijvingPrijslijst);
        config.Property(t => t.RecupelAmount).HasColumnType("decimal(18,4)").HasColumnName("ProdRecupel");
        config.Property(t => t.BebatAmount).HasColumnType("decimal(18,4)").HasColumnName("PredBebat");
        config.Property(t => t.ProdRalKleur).HasMaxLength(10);
        config.Property(t => t.ProdKleurPoedercode).HasMaxLength(50);
        config.Property(t => t.MinimumQuantity).HasColumnType("decimal(18,2)").HasColumnName("ProdMinAantal");
        config.Property(t => t.CustomWorkPercentage).HasColumnType("decimal(18,4)").HasColumnName("ProdMaatwerkPercentage");
        config.Property(t => t.RecupelProductId).HasColumnName("ProdRecupelProduct");
        config.Property(t => t.BebatProductId).HasColumnName("ProdBebatProduct");
        config.Property(t => t.IsQuickLooseSaleOption).HasColumnName("IsSnelleOptieLosseVk");
        config.Property(t => t.NameFr).HasMaxLength(150).IsRequired().HasColumnName("ProdNameFr");
        config.Property(t => t.DescriptionFr).IsRequired().HasColumnName("ProdDescriptionFr");
        config.Property(t => t.ShortNotesFr).HasMaxLength(500).IsRequired().HasColumnName("ProdKorteOpmerkingFr");
        config.Property(t => t.TaskTemplateId).HasColumnName("TaakTemplate");
        config.Property(t => t.PurchaseUnitId).HasColumnName("AankoopProductEenheidId");
        config.Property(t => t.SalesUnitId).HasColumnName("VerkoopProductEenheid1Id");
        config.Property(t => t.AdsolutId);
        config.Property(t => t.NameEn).HasMaxLength(150).IsRequired().HasColumnName("ProdNameEN");
        config.Property(t => t.DescriptionEn).IsRequired().HasColumnName("ProdDescriptionEN");
        config.Property(t => t.ShortNotesEn).HasMaxLength(500).IsRequired().HasColumnName("ProdKorteOpmerkingEN");
        config.Property(t => t.IsCompositeProduct).HasColumnName("ProdIsSamengesteldProduct");
        config.Property(t => t.ProductStructureId).HasColumnName("ProductStructuurId");
        config.Property(t => t.TemporaryDiscount).HasColumnType("decimal(18,4)").HasColumnName("TempKorting");
        config.Property(t => t.TemporaryNetPurchasePrice).HasColumnType("decimal(18,2)").HasColumnName("TempNettoAankoop");
        config.Property(t => t.ReportingGroupId).HasColumnName("ReportingGroep1Id");
        config.Property(t => t.SalesStockTriggerQuantity).HasColumnType("decimal(18,3)").HasColumnName("VerkoopAantalStockTrigger");
        config.Property(t => t.ExtraPrice).HasColumnType("decimal(18,4)").HasColumnName("MeerPrijs");
        config.Property(t => t.ExtraAssemblyPrice).HasColumnType("decimal(18,2)").HasColumnName("MeerprijsAssemblage");
        config.Property(t => t.ExtraInstallationPrice).HasColumnType("decimal(18,2)").HasColumnName("MeerprijsMontage");
        config.Property(t => t.IsProducedCompositeProduct).HasColumnName("GeproduceerdSamengesteldProduct");
        config.Property(t => t.IsVerified).HasColumnName("Gecontroleerd");
        config.Property(t => t.HasNoPrice).HasColumnName("ProdZonderPrijs");
        config.Property(t => t.ShowOnQuote).HasColumnName("ToonOpOfferte");
        config.Property(t => t.ShowOnOrderConfirmation).HasColumnName("ToonOpOrderbevestiging");
        config.Property(t => t.ShowOnInvoice).HasColumnName("ToonOpFactuur");
        config.Property(t => t.ShowOnPackingSlip).HasColumnName("ToonOpPakbon");
        config.Property(t => t.ShowOnDeliveryNote).HasColumnName("ToonOpLeveringsBon");
        config.Property(t => t.ShowOnProductionOrder).HasColumnName("ToonOpProductieBon");
        config.Property(t => t.ShowOnPaintShopOrder).HasColumnName("ToonOpLakkerijBon");
        config.Property(t => t.ShowOnInstallationOrder).HasColumnName("ToonOpMontageBon");
        config.Property(t => t.Weight).HasColumnType("decimal(18,3)").HasColumnName("Gewicht");
        config.Property(t => t.InternalDocumentNotes).HasMaxLength(1000).HasColumnName("OpmerkingInterneBonnen");
        config.Property(t => t.ExternalInstallerCost).HasColumnName("ExterneMonteurKost");
        config.Property(t => t.ReportRecupel).HasColumnName("RecupelRapporteren");
        config.Property(t => t.ReportBebat).HasColumnName("BebatRapporteren");
        config.Property(t => t.HasTierPricing).HasColumnName("HeeftStaffelprijzen");
        config.Property(t => t.HideDetailPrice).HasColumnName("ToonGeenDetailprijs");
        config.Property(t => t.ShowOnWebshop).HasColumnName("WebShop");
        config.Property(t => t.LastModifiedAt).HasColumnName("LaatsteAanpassing");
        config.Property(t => t.LastModifiedBy).HasMaxLength(50).HasColumnName("LaatsteAanpassingDoor");
        config.Property(t => t.IsNew).HasColumnName("IsNieuw");
        config.Property(t => t.EanCode).HasMaxLength(50);
        config.Property(t => t.PopupMessage).HasMaxLength(512).HasColumnName("PopUpMelding");
        config.Property(t => t.WebshopDescriptionNl).IsRequired();
        config.Property(t => t.GoodsCode).HasMaxLength(50).HasColumnName("GoederenCode");
        config.Property(t => t.IntrastatCodeId);
    }

    private static void MapProductDiscountSuggestion(EntityTypeBuilder<ProductDiscountSuggestion> config)
    {
        config.ToTable("ProductKortingSuggestie", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.GrossCorrection).HasColumnType("decimal(18,4)").HasColumnName("CorrectieBruto");
        config.Property(t => t.Discount).HasColumnType("decimal(18,4)").HasColumnName("Korting");
        config.Property(t => t.Pro1).HasColumnType("decimal(18,4)");
        config.Property(t => t.Pro2).HasColumnType("decimal(18,4)");
        config.Property(t => t.Pro3).HasColumnType("decimal(18,4)");
        config.Property(t => t.Aan1).HasColumnType("decimal(18,4)");
        config.Property(t => t.Aan2).HasColumnType("decimal(18,4)");
        config.Property(t => t.Par1).HasColumnType("decimal(18,4)");
        config.Property(t => t.DiscountCap).HasColumnType("decimal(18,4)").HasColumnName("KortingTot");
        config.Property(t => t.Ond1).HasColumnType("decimal(18,4)");
    }

    private static void MapProductDiscountSuggestionLine(EntityTypeBuilder<ProductDiscountSuggestionLine> config)
    {
        config.ToTable("ProductKortingSuggestieDetail", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerTypeId).HasColumnName("KlantTypeKlantTypeId");
        config.Property(t => t.ProductKortingSuggestieId);
        config.Property(t => t.Discount).HasColumnType("decimal(18,4)").HasColumnName("Korting");
    }

    private static void MapProductManual(EntityTypeBuilder<ProductManual> config)
    {
        config.ToTable("ProductHandleiding", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Path).HasMaxLength(1000).IsRequired();
        config.Property(t => t.ProductId).HasColumnName("ProductProdId");
        config.Property(t => t.ShowOnWeb).HasColumnName("Web");
        config.Property(t => t.SendAutomatically).HasColumnName("AutomMeesturen");
        config.Property(t => t.Extension).HasMaxLength(25).IsRequired().HasColumnName("Extentie");
    }

    private static void MapProductOption(EntityTypeBuilder<ProductOption> config)
    {
        config.ToTable("ProductOptions", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.ValueType).HasMaxLength(50).IsRequired().HasColumnName("WaardeType");
        config.Property(t => t.IsRequired).HasColumnName("Verplicht");
        config.Property(t => t.ProductId);
        config.Property(t => t.ProductionNotesFlag).HasColumnName("ProductieOpmerking");
        config.Property(t => t.QuoteNotesFlag).HasColumnName("OfferteOpmerking");
        config.Property(t => t.CalculatePrice).HasColumnName("BerekenPrijs");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.IsQuantityLine).HasColumnName("IsAantalLijn");
        config.Property(t => t.NameFr).HasMaxLength(250).IsRequired().HasColumnName("NaamFR");
        config.Property(t => t.DefaultValueFormula).HasMaxLength(4000).HasColumnName("StandaardWaardeFormule");
        config.Property(t => t.Tag).HasMaxLength(250).IsRequired();
        config.Property(t => t.QuantityFormula).HasMaxLength(4000).HasColumnName("FormuleAantal");
        config.Property(t => t.NameEn).HasMaxLength(250).IsRequired().HasColumnName("NaamEn");
        config.Property(t => t.ExtraPriceFormula).HasMaxLength(4000);
        config.Property(t => t.UnitParameterFormula).HasMaxLength(4000).HasColumnName("EenheidsparameterFormule");
    }

    private static void MapProductOptionValue(EntityTypeBuilder<ProductOptionValue> config)
    {
        config.ToTable("ProductOptionValue", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OptieProduct);
        config.Property(t => t.Value).HasMaxLength(100).HasColumnName("Waarde");
        config.Property(t => t.ProductOptionId);
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.ValueFr).HasMaxLength(100).HasColumnName("WaardeFr");
        config.Property(t => t.ValueEn).HasMaxLength(100).HasColumnName("WaardeEn");
    }

    private static void MapProductPopupReturnColumn(EntityTypeBuilder<ProductPopupReturnColumn> config)
    {
        config.ToTable("ProductPopupRetourKolom", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameFr).HasMaxLength(50).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.NameNl).HasMaxLength(50).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.LooseSaleColumn).HasMaxLength(150).IsRequired().HasColumnName("RowLosseVkkolom");
        config.Property(t => t.GateComponentColumn).HasMaxLength(150).IsRequired().HasColumnName("RowPoortOnderdeelKolom");
    }

    private static void MapProductPopupTemplate(EntityTypeBuilder<ProductPopupTemplate> config)
    {
        config.ToTable("ProductPopupTemplate", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.NameFr).HasMaxLength(50).IsRequired().HasColumnName("NaamFr");
    }

    private static void MapProductPopupTemplateLine(EntityTypeBuilder<ProductPopupTemplateLine> config)
    {
        config.ToTable("ProductPopupTemplateDetail", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameFr).HasMaxLength(50).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.NameNl).HasMaxLength(50).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.TransferToQuantityFormula).HasMaxLength(250).IsRequired().HasColumnName("OverbrengenNaanAantalFormule");
        config.Property(t => t.TransferToQuantity).HasColumnName("OverbrengenNaarAantal");
        config.Property(t => t.IncludePrice).HasColumnName("PrijsOpnemen");
        config.Property(t => t.WriteToLineColumn).HasColumnName("SchrijfNaarLijnKolom");
        config.Property(t => t.IsRequired).HasColumnName("Verplicht");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
    }

    private static void MapProductPopupValueType(EntityTypeBuilder<ProductPopupValueType> config)
    {
        config.ToTable("ProductPopupWaardeType", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.NameFr).HasMaxLength(50).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.Description).HasMaxLength(250).IsRequired().HasColumnName("Omschrijving");
        config.Property(t => t.DescriptionFr).HasMaxLength(250).IsRequired().HasColumnName("OmschrijvingFr");
        config.Property(t => t.Tag).HasMaxLength(50).IsRequired();
    }

    private static void MapProductPrice(EntityTypeBuilder<ProductPrice> config)
    {
        config.ToTable("ProductPrijzen", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.FromAddress).HasColumnName("Van");
        config.Property(t => t.ValidTo).HasColumnName("Tot");
        config.Property(t => t.AssemblyPrice).HasColumnType("decimal(18,4)").HasColumnName("AssemblagePrijs");
        config.Property(t => t.InstallationPrice).HasColumnType("decimal(18,4)").HasColumnName("MontagePrijs");
        config.Property(t => t.ProductId).HasColumnName("ProductId");
        config.Property(t => t.GrossSalesPrice).HasColumnType("decimal(18,2)").HasColumnName("BrutoVerkoop");
        config.Property(t => t.GrossPurchasePrice).HasColumnType("decimal(18,2)").HasColumnName("BrutoAankoop");
        config.Property(t => t.NetPurchasePrice).HasColumnType("decimal(18,2)").HasColumnName("NettoAankoop");
        config.Property(t => t.ProductAankoopKortingenId);
        config.Property(t => t.OverrideBruto);
        config.Property(t => t.BasePrice).HasColumnType("decimal(18,4)").HasColumnName("Basisprijs");
        config.Property(t => t.CorrectedGrossPrice).HasColumnType("decimal(18,2)").HasColumnName("GecorrigeerdeBrutoPrijs");
        config.Property(t => t.PriceCalculationFormula).HasMaxLength(2024).HasColumnName("PrijsBerekenFormule");
        config.Property(t => t.UsePriceCalculationFormula).HasColumnName("GebruikPrijsBerekenFormule");
        config.Property(t => t.BasePriceCalculationFormula).HasMaxLength(2024).HasColumnName("BasisPrijsBerekenFormule");
        config.Property(t => t.StartupCost).HasColumnType("decimal(18,4)").HasColumnName("OpstartKost");
        config.Property(t => t.Pro1).HasColumnType("decimal(18,4)");
        config.Property(t => t.Pro2).HasColumnType("decimal(18,4)");
        config.Property(t => t.Pro3).HasColumnType("decimal(18,4)");
        config.Property(t => t.Aan1).HasColumnType("decimal(18,4)");
        config.Property(t => t.Aan2).HasColumnType("decimal(18,4)");
        config.Property(t => t.Par1).HasColumnType("decimal(18,4)");
        config.Property(t => t.Ond).HasColumnType("decimal(18,4)");
        config.Property(t => t.ExtraPurchaseCost).HasColumnType("decimal(18,2)").HasColumnName("ExtraAankoopKost");
        config.Property(t => t.ExtraPurchaseCostNotes).HasMaxLength(500).HasColumnName("ExtraAankoopKostUitleg");
        config.Property(t => t.PurchaseDiscountPercentage).HasColumnType("decimal(18,4)").HasColumnName("AankoopKortingPercentage");
        config.Property(t => t.GrossCorrectionPercentage).HasColumnType("decimal(18,4)").HasColumnName("BrutoCorrectiePercentage");
        config.Property(t => t.CalculationType).HasColumnName("TypeBerekening");
        config.Property(t => t.SupplierUsesDifferentGrossSalesPrice).HasColumnName("LeverancierHanteerdAndereBrutoVerkoop");
    }

    private static void MapProductPriceSalesDiscount(EntityTypeBuilder<ProductPriceSalesDiscount> config)
    {
        config.ToTable("ProductPrijzenVerkoopKorting", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.KlantTypeId);
        config.Property(t => t.ProductPrijzenId);
        config.Property(t => t.Discount).HasColumnType("decimal(18,4)").HasColumnName("Korting");
    }

    private static void MapProductProductionGroup(EntityTypeBuilder<ProductProductionGroup> config)
    {
        config.ToTable("ProductProductionGroup", "Products");
        config.HasKey(t => t.ProductProductionGroupId);
        config.Property(t => t.ProductProductionGroupId).ValueGeneratedOnAdd().HasColumnName("ProdProdGroId");
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("ProdProdGroName");
        config.Property(t => t.SortOrder).HasColumnName("ProdProdGroOrder");
        config.Property(t => t.Color).HasMaxLength(50).IsRequired().HasColumnName("Kleur");
    }

    private static void MapProductProductionGroupLink(EntityTypeBuilder<ProductProductionGroupLink> config)
    {
        config.ToTable("ProductProductionsGroepen", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ProductId);
        config.Property(t => t.ProductionGroupId).HasColumnName("ProductieGroep");
    }

    private static void MapProductProperty(EntityTypeBuilder<ProductProperty> config)
    {
        config.ToTable("ProductProperty", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameEn).HasMaxLength(250).IsRequired();
        config.Property(t => t.NameFr).HasMaxLength(250).IsRequired();
        config.Property(t => t.NameNl).HasMaxLength(250).IsRequired();
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
    }

    private static void MapProductPropertyItem(EntityTypeBuilder<ProductPropertyItem> config)
    {
        config.ToTable("ProductPropertieItem", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ProductPropertyId);
        config.Property(t => t.Value).HasMaxLength(250).IsRequired();
        config.Property(t => t.ProductId).HasColumnName("ProductProdId");
    }

    private static void MapProductPurchaseDiscount(EntityTypeBuilder<ProductPurchaseDiscount> config)
    {
        config.ToTable("ProductAankoopKortingen", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Percentage).HasColumnType("decimal(18,4)");
        config.Property(t => t.SupplierId);
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("Naam");
        config.Property(t => t.GrossAmount).HasColumnType("decimal(18,4)").HasColumnName("Bruto");
        config.Property(t => t.Pro1).HasColumnType("decimal(18,4)");
        config.Property(t => t.Pro2).HasColumnType("decimal(18,4)");
        config.Property(t => t.Pro3).HasColumnType("decimal(18,4)");
        config.Property(t => t.Aan1).HasColumnType("decimal(18,4)");
        config.Property(t => t.Aan2).HasColumnType("decimal(18,4)");
        config.Property(t => t.Par1).HasColumnType("decimal(18,4)");
        config.Property(t => t.ValidTo).HasColumnName("Tot");
        config.Property(t => t.FromAddress).HasColumnName("Van");
        config.Property(t => t.Ond).HasColumnType("decimal(18,4)");
    }

    private static void MapProductQuantityTier(EntityTypeBuilder<ProductQuantityTier> config)
    {
        config.ToTable("ProductStaffel", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.MinimumQuantity).HasColumnType("decimal(18,2)").HasColumnName("Minimum");
        config.Property(t => t.Discount).HasColumnType("decimal(18,4)").HasColumnName("Korting");
        config.Property(t => t.ProductId).HasColumnName("ProductProdId");
    }

    private static void MapProductStockLocation(EntityTypeBuilder<ProductStockLocation> config)
    {
        config.ToTable("ProductStockLocatie", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.StockLocationId).HasColumnName("StockId");
        config.Property(t => t.ProductId);
        config.Property(t => t.Quantity).HasColumnType("decimal(18,4)").HasColumnName("Aantal");
        config.Property(t => t.IsInactive).HasColumnName("NietMeerActief");
        config.Property(t => t.MaxQuantity).HasColumnType("decimal(18,4)").HasColumnName("MaxAantal");
        config.Property(t => t.IsDefault).HasColumnName("IsStandaard");
        config.Property(t => t.MinQuantity).HasColumnType("decimal(18,4)").HasColumnName("MinAantal");
        config.Property(t => t.ReservedQuantity).HasColumnType("decimal(18,4)").HasColumnName("Gereserveerd");
        config.Property(t => t.LastCountedAt).HasColumnName("LaatsteKeerGeteld");
        config.Property(t => t.CountedQuantity).HasColumnType("decimal(18,4)").HasColumnName("AantalGeteld");
    }

    private static void MapProductStructure(EntityTypeBuilder<ProductStructure> config)
    {
        config.ToTable("ProductStructuur", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Level);
        config.Property(t => t.ParentTaskId).HasColumnName("ParentId");
        config.Property(t => t.NameNl).HasMaxLength(250).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.NameEn).HasMaxLength(250).IsRequired().HasColumnName("NaamEn");
        config.Property(t => t.NameFr).HasMaxLength(250).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.IntroPriceListTextId).HasColumnName("IntoPrijslijstTekstenId");
        config.Property(t => t.OutroPriceListTextId).HasColumnName("OutroPrijslijstTekstenOutroId");
        config.Property(t => t.SortOrder).HasColumnName("Order");
        config.Property(t => t.Color).HasColumnName("Kleur");
        config.Property(t => t.ShowOnPriceList).HasColumnName("TonenOpPrijslijst");
        config.Property(t => t.Icon);
        config.Property(t => t.PageBreakAfter);
    }

    private static void MapProductSubProduct(EntityTypeBuilder<ProductSubProduct> config)
    {
        config.ToTable("ProductSubProduct", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.MasterProductId).HasColumnName("MasterProductProdId");
        config.Property(t => t.SubProductId).HasColumnName("SubProductProdId");
        config.Property(t => t.Quantity).HasColumnType("decimal(18,4)").HasColumnName("Aantal");
        config.Property(t => t.IsOptional).HasColumnName("Optioneel");
        config.Property(t => t.ExtraBasePrice).HasColumnType("decimal(18,2)").HasColumnName("ExrtaBasisPrijs");
    }

    private static void MapProductType(EntityTypeBuilder<ProductType> config)
    {
        config.ToTable("ProductType", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired();
        config.Property(t => t.NameFr).HasMaxLength(150).IsRequired();
        config.Property(t => t.IsStockItem).HasColumnName("StockItem");
        config.Property(t => t.IsProduction).HasColumnName("IsProductie");
        config.Property(t => t.IsPurchaseItem).HasColumnName("BestelArtikel");
    }

    private static void MapProductUnit(EntityTypeBuilder<ProductUnit> config)
    {
        config.ToTable("ProductEenheid", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameEn).HasMaxLength(150).IsRequired().HasColumnName("NaamEn");
        config.Property(t => t.NameFr).HasMaxLength(150).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.NameNl).HasMaxLength(150).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.UnitParameter).HasColumnName("EenheidsParameter");
    }

    private static void MapProject(EntityTypeBuilder<Project> config)
    {
        config.ToTable("Project", "Projecten");
        config.HasKey(t => t.ProjectId);
        config.Property(t => t.ProjectId).ValueGeneratedOnAdd();
        config.Property(t => t.ProjectNumber).HasColumnName("ProjectNummer");
        config.Property(t => t.ProjectName).HasMaxLength(250).IsRequired().HasColumnName("ProjectNaam");
        config.Property(t => t.ProjectManagerUserId).HasColumnName("ProjectBeheerder");
        config.Property(t => t.CustomerId).HasColumnName("ProjectKlant");
        config.Property(t => t.JobSiteId).HasColumnName("ProjectWerf");
        config.Property(t => t.ProjectTypeId);
        config.Property(t => t.ProjectCreatedAt).HasColumnName("ProjectAanmaakDatum");
        config.Property(t => t.ProjectInternalNotes).HasColumnName("ProjectInterneOpmerking");
        config.Property(t => t.BaseCompanyId);
        config.Property(t => t.ProductionLabelReference).HasMaxLength(60);
        config.Property(t => t.IsTemplate);
        config.Property(t => t.ProjectNotes).HasColumnName("ProjectOpmerking");
        config.Property(t => t.IsStandardProject).HasColumnName("IsStandaardProject");
        config.Property(t => t.TeamsId).HasMaxLength(250);
        config.Property(t => t.PopupMessage).HasColumnName("PopupMelding");
    }

    private static void MapProjectActivity(EntityTypeBuilder<ProjectActivity> config)
    {
        config.ToTable("ProjectActiviteit", "Logging");
        config.HasKey(t => t.Id);
        config.Property(t => t.ProjectId).HasColumnName("ProjectProjectId");
        config.Property(t => t.ActionCode).HasColumnName("Actie");
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.LoggedAt).HasColumnName("LogDate");
    }

    private static void MapProjectContact(EntityTypeBuilder<ProjectContact> config)
    {
        config.ToTable("ProjectContact", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ProjectId).HasColumnName("ProjectProjectId");
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.ContactProjectRolId);
        config.Property(t => t.LinkedContactId).HasColumnName("ProjectContactId");
        config.Property(t => t.ContactContactId);
    }

    private static void MapProjectInstallation(EntityTypeBuilder<ProjectInstallation> config)
    {
        config.ToTable("ProjectInstallatie", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.InstallationOrderId).HasColumnName("InstallatieDossierId");
        config.Property(t => t.Location).HasMaxLength(250).HasColumnName("Locatie");
        config.Property(t => t.Name).HasMaxLength(250).HasColumnName("Naam");
        config.Property(t => t.ProjectId);
        config.Property(t => t.SerialNumber).HasMaxLength(250).HasColumnName("Serienummer");
        config.Property(t => t.Specifications).HasColumnName("Specificaties");
    }

    private static void MapProjectLog(EntityTypeBuilder<ProjectLog> config)
    {
        config.ToTable("ProjectLog", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ProjectId);
        config.Property(t => t.ActionCode).HasMaxLength(150).IsRequired().HasColumnName("Actie");
        config.Property(t => t.Date).HasColumnName("Datum");
    }

    private static void MapProjectParty(EntityTypeBuilder<ProjectParty> config)
    {
        config.ToTable("ProjectPartij", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerId).HasColumnName("KlantKlantId");
        config.Property(t => t.ProjectPartijGroepId);
        config.Property(t => t.Comment).HasMaxLength(500).HasColumnName("Commentaar");
        config.Property(t => t.BillingPercentage).HasColumnType("decimal(18,4)").HasColumnName("FacturatiePercentage");
        config.Property(t => t.ProjectId).HasColumnName("ProjectProjectId");
    }

    private static void MapProjectPartyContact(EntityTypeBuilder<ProjectPartyContact> config)
    {
        config.ToTable("ProjectPartijContact", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerContactId).HasColumnName("KlantContactKlantContactId");
        config.Property(t => t.ProjectPartijId);
        config.Property(t => t.EmailQuote).HasColumnName("MailOfferte");
        config.Property(t => t.EmailOrderConfirmation).HasColumnName("MailOrderbevestiging");
        config.Property(t => t.EmailPlanning).HasColumnName("MailPlanning");
        config.Property(t => t.EmailDeliveryReady).HasColumnName("MailLeveringKlaar");
        config.Property(t => t.EmailDelivered).HasColumnName("MailGeleverd");
        config.Property(t => t.EmailBilling).HasColumnName("MailFacturatie");
        config.Property(t => t.Note).HasMaxLength(500).IsRequired().HasColumnName("Notitie");
    }

    private static void MapProjectPartyGroup(EntityTypeBuilder<ProjectPartyGroup> config)
    {
        config.ToTable("ProjectPartijGroep", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameEn).HasMaxLength(50).IsRequired().HasColumnName("NaamEn");
        config.Property(t => t.NameFr).HasMaxLength(50).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.NameNl).HasMaxLength(50).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.IsForSupplier).HasColumnName("IsVoorLeverancier");
        config.Property(t => t.IsForCustomer).HasColumnName("IsVoorKlant");
        config.Property(t => t.IsForManufacturer).HasColumnName("IsVoorFabrikant");
    }

    private static void MapRawMaterial(EntityTypeBuilder<RawMaterial> config)
    {
        config.ToTable("Grondstof", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Description).IsRequired().HasColumnName("Omschrijving");
    }

    private static void MapRepairCostPrice(EntityTypeBuilder<RepairCostPrice> config)
    {
        config.ToTable("HerstellingKostPrijs", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Prijs).HasColumnType("decimal(18,2)");
        config.Property(t => t.ValidTo).HasColumnName("Tot");
        config.Property(t => t.FromAddress).HasColumnName("Van");
    }

    private static void MapReportingGroup(EntityTypeBuilder<ReportingGroup> config)
    {
        config.ToTable("ReportingGroep1", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
    }

    private static void MapSalutation(EntityTypeBuilder<Salutation> config)
    {
        config.ToTable("Aanspreking", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.SalutationText).HasMaxLength(50).IsRequired().HasColumnName("AansprekingTekst");
        config.Property(t => t.IsMale).HasColumnName("Man");
        config.Property(t => t.IsFemale).HasColumnName("Vrouw");
        config.Property(t => t.LanguageId).HasColumnName("Taal");
    }

    private static void MapServiceRate(EntityTypeBuilder<ServiceRate> config)
    {
        config.ToTable("PrestatieTarief", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Rate).HasColumnType("decimal(18,2)").HasColumnName("Tarief");
    }

    private static void MapSickLeave(EntityTypeBuilder<SickLeave> config)
    {
        config.ToTable("Ziekte", "Users");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.FromAddress).HasColumnName("Van");
        config.Property(t => t.ValidTo).HasColumnName("Tot");
        config.Property(t => t.SickNoteAzureFileId).HasColumnName("ZiekteBriefAzureFileId");
        config.Property(t => t.UserId);
        config.Property(t => t.CalendarEntryId).HasColumnName("AgendaId");
    }

    private static void MapStaffUser(EntityTypeBuilder<StaffUser> config)
    {
        config.ToTable("User", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Crm);
        config.Property(t => t.Offerte);
        config.Property(t => t.Bestellingen);
        config.Property(t => t.Productie);
        config.Property(t => t.Boekhouding);
        config.Property(t => t.Planning);
        config.Property(t => t.Admin);
        config.Property(t => t.Verkoper);
        config.Property(t => t.Color).HasColumnName("Kleur");
        config.Property(t => t.ExchangeLastWatermark).HasMaxLength(1000);
        config.Property(t => t.DoSyncExchange);
        config.Property(t => t.RechtstreeksGsmNrtonen);
        config.Property(t => t.RechtstreeksTelefoonNrTonen);
        config.Property(t => t.LinkedInUrl).HasMaxLength(500).HasColumnName("LinkedinUrl");
        config.Property(t => t.UserGroupId).HasColumnName("UsrGroepId");
        config.Property(t => t.DirecteLosseVerkoop);
        config.Property(t => t.ReportSales);
        config.Property(t => t.DefaultCeLabelPrinter).HasMaxLength(255).HasColumnName("StdCElabelPrinter");
        config.Property(t => t.DefaultProductionLabelPrinter).HasMaxLength(255).HasColumnName("StdProductieLabelPrinter");
        config.Property(t => t.Login).HasMaxLength(50).IsRequired();
        config.Property(t => t.Password).HasMaxLength(50).IsRequired();
        config.Property(t => t.EmailTemplate).HasMaxLength(150).HasColumnName("Email");
        config.Property(t => t.Tel).HasMaxLength(20);
        config.Property(t => t.FaxTemplate).HasMaxLength(20).HasColumnName("Fax");
        config.Property(t => t.Gsm).HasMaxLength(20);
        config.Property(t => t.FirstName).HasMaxLength(100).IsRequired().HasColumnName("Voornaam");
        config.Property(t => t.LastName).HasMaxLength(100).IsRequired().HasColumnName("Achternaam");
        config.Property(t => t.BaseCompaniesId);
        config.Property(t => t.TaalId);
        config.Property(t => t.Ice1Number).HasMaxLength(50).HasColumnName("Ice1Nr");
        config.Property(t => t.Ice1Name).HasMaxLength(100).HasColumnName("Ice1Naam");
        config.Property(t => t.Ice2Number).HasMaxLength(50).HasColumnName("Ice2Nr");
        config.Property(t => t.Ice2Name).HasMaxLength(100).HasColumnName("Ice2Naam");
        config.Property(t => t.PrivateEmail).HasMaxLength(150).HasColumnName("EmaiPrive");
        config.Property(t => t.Hr);
        config.Property(t => t.Address).HasMaxLength(150).IsRequired().HasColumnName("Adres");
        config.Property(t => t.HiredAt).HasColumnName("Indienst");
        config.Property(t => t.LeftAt).HasColumnName("Uitdienst");
        config.Property(t => t.ProductBeheer);
        config.Property(t => t.PlanningBinnenDienst);
        config.Property(t => t.PlanningBuitendienst);
        config.Property(t => t.PlanningProjecten);
        config.Property(t => t.CompanyPhone).HasMaxLength(50).HasColumnName("TelAbm");
        config.Property(t => t.CompanyMobile).HasMaxLength(50).HasColumnName("GsmAbm");
        config.Property(t => t.JobTitle).HasMaxLength(50).HasColumnName("Functie");
        config.Property(t => t.ToonInPrijslijst);
        config.Property(t => t.SelectForInternalPlanning).HasColumnName("SelecteerBijBinnendienstPlanning");
        config.Property(t => t.SelectForExternalPlanning).HasColumnName("SelecteerBijBuitendienstPlanning");
        config.Property(t => t.SelectForProjectPlanning).HasColumnName("SelecteerBijProjectPlanning");
        config.Property(t => t.SelectForLeavePlanning).HasColumnName("SelecteerBijVerlofPlanning");
        config.Property(t => t.DefaultPlanningLabelId).HasColumnName("StandaardPlanningLabel");
        config.Property(t => t.TextColor).HasColumnName("KleurText");
        config.Property(t => t.CanAccessRevenueReports).HasColumnName("ToegangOmzetRapporten");
        config.Property(t => t.CanAccessProfitReports).HasColumnName("ToegangWinstRapporten");
        config.Property(t => t.CanAccessDmsSpecial).HasColumnName("ToegangDmsspecial");
        config.Property(t => t.CanAccessBulkOrders).HasColumnName("ToegangBulkBestellingen");
        config.Property(t => t.CanAccessStockManagement).HasColumnName("ToegangStockBeheer");
        config.Property(t => t.CanOrderFromOrderScreen).HasColumnName("VanuitDossierBestellingenDoen");
        config.Property(t => t.ShowProjects).HasColumnName("ToonProjecten");
        config.Property(t => t.CanAccessBilling).HasColumnName("Facturatie");
    }

    private static void MapStandardBillingTerm(EntityTypeBuilder<StandardBillingTerm> config)
    {
        config.ToTable("StdFacturatieVoorwaarden", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
    }

    private static void MapStandardBillingTermLine(EntityTypeBuilder<StandardBillingTermLine> config)
    {
        config.ToTable("StdFacturatieVoorwaardenDetail", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(250).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Percentage).HasColumnType("decimal(18,4)");
        config.Property(t => t.StdFacturatieVoorwaardenId);
    }

    private static void MapStockLocation(EntityTypeBuilder<StockLocation> config)
    {
        config.ToTable("StockLocatie", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("Naam");
        config.Property(t => t.IsWarehouse).HasColumnName("IsMagazijn");
    }

    private static void MapStockMovement(EntityTypeBuilder<StockMovement> config)
    {
        config.ToTable("StockBeweging", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ProductId).HasColumnName("ProductProdId");
        config.Property(t => t.OrderLineId).HasColumnName("DossierDetailId");
        config.Property(t => t.Quantity).HasColumnType("decimal(18,4)").HasColumnName("Aantal");
        config.Property(t => t.Timestamp).HasColumnName("DatumEnTijd");
        config.Property(t => t.Notes).HasMaxLength(150).HasColumnName("Opmerking");
        config.Property(t => t.IsReservation).HasColumnName("IsReservering");
        config.Property(t => t.ProductStockLocatieId);
    }

    private static void MapStockOrder(EntityTypeBuilder<StockOrder> config)
    {
        config.ToTable("StockOrder", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.SupplierId).HasColumnName("LeverancierId");
        config.Property(t => t.CreatedAt).HasColumnName("CreateDatum");
        config.Property(t => t.OrderDate).HasColumnName("OrderDatum");
        config.Property(t => t.OrderConfirmationDate).HasColumnName("OrderBevestigingDatum");
        config.Property(t => t.DeliveryDate).HasColumnName("LeveringsDatum");
        config.Property(t => t.InvoiceDate).HasColumnName("FactuurDatum");
        config.Property(t => t.IsCompleted).HasColumnName("Afgewerkt");
        config.Property(t => t.Notes).HasColumnName("Opmerking");
        config.Property(t => t.UserId);
        config.Property(t => t.ExpectedDeliveryDate).HasColumnName("VerwachteLeveringsDatum");
        config.Property(t => t.InternalNotes).HasColumnName("InterneOpmerking");
        config.Property(t => t.TotalAmount).HasColumnType("decimal(18,2)").HasColumnName("TotaalBedrag");
    }

    private static void MapStockOrderDelivery(EntityTypeBuilder<StockOrderDelivery> config)
    {
        config.ToTable("StockOrderLevering", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.StockOrderDetail);
        config.Property(t => t.DeliveryDocumentNumber).HasMaxLength(100).IsRequired().HasColumnName("LeveringsDocNummer");
        config.Property(t => t.Date).HasColumnName("Datum");
        config.Property(t => t.Quantity).HasColumnType("decimal(18,2)").HasColumnName("Aantal");
        config.Property(t => t.QuantityInvoiced).HasColumnType("decimal(18,2)").HasColumnName("AantalGefactureerd");
    }

    private static void MapStockOrderLine(EntityTypeBuilder<StockOrderLine> config)
    {
        config.ToTable("StockOrderDetail", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.StockOrderId);
        config.Property(t => t.ProductId);
        config.Property(t => t.QuantityOrdered).HasColumnType("decimal(18,2)").HasColumnName("AantalBesteld");
        config.Property(t => t.LijnOK);
        config.Property(t => t.ProductName).HasMaxLength(500).IsRequired().HasColumnName("Productnaam");
        config.Property(t => t.OrderNumber).HasMaxLength(150).IsRequired().HasColumnName("Ordernummer");
        config.Property(t => t.PackSize).HasMaxLength(50).IsRequired().HasColumnName("VerpaktPer");
        config.Property(t => t.PurchaseUnitPrice).HasColumnType("decimal(18,4)").HasColumnName("AankoopPrijsStuk");
        config.Property(t => t.PurchaseTotalPrice).HasColumnType("decimal(18,4)").HasColumnName("AankoopPrijsTotaal");
        config.Property(t => t.Unit).HasMaxLength(50).IsRequired().HasColumnName("Eenheid");
        config.Property(t => t.InternalReference).HasMaxLength(150).HasColumnName("InterneReferentie");
        config.Property(t => t.DeliveryNotes).HasMaxLength(2000).HasColumnName("OpermerkingLevering");
        config.Property(t => t.OrderNotes).HasMaxLength(2000).HasColumnName("OpmerkingBestelling");
        config.Property(t => t.Besteld);
        config.Property(t => t.OrderedAt).HasColumnName("BesteldOp");
        config.Property(t => t.Geleverd);
        config.Property(t => t.DeliveredAt).HasColumnName("GeleverdOp");
        config.Property(t => t.ProductTypeId);
        config.Property(t => t.QuantityDelivered).HasColumnType("decimal(18,2)").HasColumnName("AantalGeleverd");
        config.Property(t => t.QuantityProcessedToStock).HasColumnType("decimal(18,2)").HasColumnName("AantalVerwerktInStock");
    }

    private static void MapStoredFile(EntityTypeBuilder<StoredFile> config)
    {
        config.ToTable("Bestand", "Bestanden");
        config.HasKey(t => t.StoredFileId);
        config.Property(t => t.StoredFileId).ValueGeneratedOnAdd().HasColumnName("BestandId");
        config.Property(t => t.FileName).HasMaxLength(250).IsRequired().HasColumnName("BestandNaam");
        config.Property(t => t.FileType).HasColumnName("BestandType");
        config.Property(t => t.Created);
        config.Property(t => t.Updated);
        config.Property(t => t.CreatedBy).HasColumnName("AangemaaktDoor");
        config.Property(t => t.Data);
    }

    private static void MapSupplier(EntityTypeBuilder<Supplier> config)
    {
        config.ToTable("Supplier", "Crm");
        config.HasKey(t => t.SupplierId);
        config.Property(t => t.SupplierId).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("SupplierName");
        config.Property(t => t.Phone).HasMaxLength(50).HasColumnName("SupplierPhone");
        config.Property(t => t.Mobile).HasMaxLength(50).HasColumnName("SupplierMobile");
        config.Property(t => t.Fax).HasMaxLength(50).HasColumnName("SupplierFax");
        config.Property(t => t.Email).HasMaxLength(150).HasColumnName("SupplierEmail");
        config.Property(t => t.Address).HasMaxLength(250).HasColumnName("SupplierAddress");
        config.Property(t => t.CityId).HasColumnName("SupplierCity");
        config.Property(t => t.CompanyRegistrationNumber).HasMaxLength(50).HasColumnName("SupplierKBO");
        config.Property(t => t.VatNumber).HasMaxLength(50).HasColumnName("SupplierVAT");
        config.Property(t => t.SupplierOrderEmail).HasMaxLength(250).HasColumnName("SupplierOrderMail");
        config.Property(t => t.LanguageId).HasColumnName("SupplierLangId");
        config.Property(t => t.GeneralLedgerRevenueAccount).HasColumnName("GrootboekOmzetRekening");
        config.Property(t => t.IsMainSupplier).HasColumnName("MainSupplier");
        config.Property(t => t.IsInactive).HasColumnName("NietActief");
        config.Property(t => t.IsVerified).HasColumnName("Gecontroleerd");
        config.Property(t => t.PriceListSortOrder).HasColumnName("PrijsLijstVolgorde");
        config.Property(t => t.PriceListName).HasMaxLength(100).HasColumnName("PrijslijstNaam");
        config.Property(t => t.Notes).HasColumnName("Opmerking");
    }

    private static void MapSupplierContact(EntityTypeBuilder<SupplierContact> config)
    {
        config.ToTable("SupplierConact", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.SupplierId);
        config.Property(t => t.ContactId);
        config.Property(t => t.IsDefault);
        config.Property(t => t.ContactFunctionId).HasColumnName("ContactFunction");
    }

    private static void MapTaskAction(EntityTypeBuilder<TaskAction> config)
    {
        config.ToTable("TaakActies", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.TaskItemId).HasColumnName("TakenId");
        config.Property(t => t.Date).HasColumnName("Datum");
        config.Property(t => t.Explanation).IsRequired().HasColumnName("Toelichting");
    }

    private static void MapTaskDependency(EntityTypeBuilder<TaskDependency> config)
    {
        config.ToTable("TaakDependency", "Taken");
        config.HasKey(t => t.Id);
        config.Property(t => t.DependentTaskId).HasColumnName("DependentId");
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ParentTaskId).HasColumnName("ParentId");
        config.Property(t => t.Type);
    }

    private static void MapTaskItem(EntityTypeBuilder<TaskItem> config)
    {
        config.ToTable("Taken", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.ReminderDate).HasColumnName("DatumRappel");
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.CustomerId).HasColumnName("Klant");
        config.Property(t => t.Description).HasMaxLength(2000).IsRequired().HasColumnName("Omschrijving");
        config.Property(t => t.Type);
        config.Property(t => t.AssignedUserId).HasColumnName("User");
        config.Property(t => t.IsCompleted).HasColumnName("Voltooid");
        config.Property(t => t.CreatedAt).HasColumnName("AanmaakDatum");
        config.Property(t => t.ProjectId).HasColumnName("Project");
        config.Property(t => t.OrderLineId).HasColumnName("DossierDetailId");
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.BaseCompanyId);
        config.Property(t => t.EndDate).HasColumnName("EindDatum");
        config.Property(t => t.PercentComplete);
        config.Property(t => t.UserGroupId).HasColumnName("UsrGroepId");
        config.Property(t => t.CompletedAt).HasColumnName("VoltooidOp");
        config.Property(t => t.CreatedByUserId).HasColumnName("AangemaaktDoorUsrId");
        config.Property(t => t.IsCancelled).HasColumnName("Geannuleerd");
        config.Property(t => t.CancelledAt).HasColumnName("GeannuleerdOp");
        config.Property(t => t.CheckedByCreatorAt).HasColumnName("CheckedByCreaterOn");
        config.Property(t => t.IsRead).HasColumnName("Gelezen");
        config.Property(t => t.PopupShown);
        config.Property(t => t.IsUrgent).HasColumnName("Dringend");
        config.Property(t => t.RejectionReason).HasColumnName("Weigeringsreden");
        config.Property(t => t.IsRejectionRead).HasColumnName("WeigereningGelezen");
        config.Property(t => t.IsRejected).HasColumnName("Geweigerd");
        config.Property(t => t.ReadAt).HasColumnName("GelezenOp");
        config.Property(t => t.RejectedAt).HasColumnName("GeweigerdOp");
    }

    private static void MapTaskTemplate(EntityTypeBuilder<TaskTemplate> config)
    {
        config.ToTable("TaakTemplate", "Taken");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameFr).HasMaxLength(250).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.NameNl).HasMaxLength(250).IsRequired().HasColumnName("NaamNl");
    }

    private static void MapTaskTemplateDependency(EntityTypeBuilder<TaskTemplateDependency> config)
    {
        config.ToTable("TaakTemplateDependencie", "Taken");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.ParentTaskId).HasColumnName("ParentId");
        config.Property(t => t.DependsOnTaskTemplateTaskId).HasColumnName("DependentOnId");
        config.Property(t => t.Type);
    }

    private static void MapTaskTemplateTask(EntityTypeBuilder<TaskTemplateTask> config)
    {
        config.ToTable("TaakTemplateTaak", "Taken");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
        config.Property(t => t.DueWithinTicks).HasColumnName("BinnenTermijnVan");
        config.Property(t => t.TaakTypeId);
        config.Property(t => t.DefaultUserId).HasColumnName("IsDefaultUser");
        config.Property(t => t.UseProjectOwnerAsDefault).HasColumnName("IsDefaultProjectVerantwoordelijke");
        config.Property(t => t.DefaultUserGroupId).HasColumnName("IsDefaultGroep");
        config.Property(t => t.LockUntilPreviousComplete).HasColumnName("LockAlsVorigeNietCompleetZijn");
        config.Property(t => t.TaakTemplateId);
        config.Property(t => t.TaskName).HasMaxLength(250).IsRequired().HasColumnName("Taaknaam");
    }

    private static void MapTaskType(EntityTypeBuilder<TaskType> config)
    {
        config.ToTable("TaakType", "Crm");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(100).IsRequired().HasColumnName("Naam");
        config.Property(t => t.CompleteWithinDays).HasColumnType("decimal(18,2)").HasColumnName("BinnenAantalDagenCompleet");
        config.Property(t => t.Color).HasColumnName("Kleur");
        config.Property(t => t.ProductionWarning).HasColumnName("WaarschuwingProductie");
        config.Property(t => t.DeliveryInstallationWarning).HasColumnName("WaarschuwingLeveringMontage");
    }

    private static void MapTemplateType(EntityTypeBuilder<TemplateType> config)
    {
        config.ToTable("TemplateType", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("Naam");
        config.Property(t => t.Tag).HasMaxLength(50).IsRequired();
        config.Property(t => t.DocumentTypeId).HasMaxLength(50).IsRequired().HasColumnName("DocType");
    }

    private static void MapTimesheet(EntityTypeBuilder<Timesheet> config)
    {
        config.ToTable("Timesheet", "Projecten");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.OrderId).HasColumnName("DossierId");
        config.Property(t => t.StartDt);
        config.Property(t => t.StopDt);
        config.Property(t => t.DurationMinutes).HasColumnType("decimal(18,0)").HasColumnName("DuurInMin");
        config.Property(t => t.DurationHours).HasColumnType("decimal(18,2)").HasColumnName("DuurInUur");
        config.Property(t => t.JobCodeId);
        config.Property(t => t.IsBillable).HasColumnName("Factureerbaar");
        config.Property(t => t.BillableMinutes).HasColumnType("decimal(18,2)").HasColumnName("TeFacturerenTijdInMin");
        config.Property(t => t.BillableHours).HasColumnType("decimal(18,2)").HasColumnName("TeFacturerenTijdInUur");
        config.Property(t => t.PrestantUserId);
        config.Property(t => t.CreatedAt).HasColumnName("AangemaaktOp");
        config.Property(t => t.CreatedByUserId).HasColumnName("AangemaaktUserId");
        config.Property(t => t.Description).HasMaxLength(500).IsRequired().HasColumnName("Omschrijving");
        config.Property(t => t.IsTimerRunning).HasColumnName("TimerOn");
        config.Property(t => t.TimerStartedAt).HasColumnName("TimerStart");
        config.Property(t => t.TimerLastValue);
    }

    private static void MapUserGroup(EntityTypeBuilder<UserGroup> config)
    {
        config.ToTable("UsrGroep", "Instellingen");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(50).IsRequired().HasColumnName("Naam");
        config.Property(t => t.IsInstallationTeam).HasColumnName("IsMontage");
        config.Property(t => t.IsServiceTeam).HasColumnName("IsService");
        config.Property(t => t.IsTransportTeam).HasColumnName("IsTransport");
        config.Property(t => t.OrderStatusGroupId).HasColumnName("DossierStatusGroepId");
    }

    private static void MapVatType(EntityTypeBuilder<VatType> config)
    {
        config.ToTable("BtwType", "Boekhouding");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.Name).HasMaxLength(150).IsRequired().HasColumnName("Naam");
        config.Property(t => t.InvoiceText).HasMaxLength(512).IsRequired().HasColumnName("FactuurTekst");
        config.Property(t => t.Percentage).HasColumnType("decimal(18,2)");
        config.Property(t => t.InvoiceTextEn).HasMaxLength(512).IsRequired().HasColumnName("FactuurTekstEn");
        config.Property(t => t.InvoiceTextFr).HasMaxLength(512).IsRequired().HasColumnName("FactuurTekstFr");
        config.Property(t => t.ExplanationNl).IsRequired().HasColumnName("ToelichtingNl");
        config.Property(t => t.ExplanationFr).IsRequired().HasColumnName("ToelichtingFr");
        config.Property(t => t.ExplanationEn).IsRequired().HasColumnName("ToelichtingEn");
        config.Property(t => t.IsDefault);
        config.Property(t => t.TaxExemptionReason).HasMaxLength(50);
        config.Property(t => t.TaxExemptionReasonCode).HasMaxLength(50);
        config.Property(t => t.PeppolCode).HasMaxLength(50);
    }

    private static void MapWebshopProductStructure(EntityTypeBuilder<WebshopProductStructure> config)
    {
        config.ToTable("ProductStructuurWebshop", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameEn).HasMaxLength(250).IsRequired().HasColumnName("NaamEn");
        config.Property(t => t.NameFr).HasMaxLength(250).IsRequired().HasColumnName("NaamFr");
        config.Property(t => t.NameNl).HasMaxLength(250).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.ParentTaskId).HasColumnName("ParentId");
    }

    private static void MapWebshopStructure(EntityTypeBuilder<WebshopStructure> config)
    {
        config.ToTable("WebshopStructuur", "Products");
        config.HasKey(t => t.Id);
        config.Property(t => t.Id).ValueGeneratedOnAdd();
        config.Property(t => t.NameNl).HasMaxLength(50).IsRequired().HasColumnName("NaamNl");
        config.Property(t => t.ParentTaskId).HasColumnName("ParentId");
        config.Property(t => t.SortOrder).HasColumnName("Volgorde");
    }

}
