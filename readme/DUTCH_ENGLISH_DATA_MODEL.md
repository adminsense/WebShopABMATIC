# ðŸ—‚ï¸ Dutch â†’ English Data Model Mapping

![Status](https://img.shields.io/badge/Status-Complete-28a745?style=flat-square) ![Tables](https://img.shields.io/badge/Tables-139-0d47a1?style=flat-square) ![Schemas](https://img.shields.io/badge/Schemas-11-512BD4?style=flat-square) ![Coverage](https://img.shields.io/badge/Coverage-100%25-ff6f00?style=flat-square)

> [!IMPORTANT]
> **Executive Summary:** Complete de-para reference for migrating the legacy ABMATIC database (Dutch names) to the WebShopABMATIC vNext model (English names). All 139 business tables are mapped and generated from SQL.

---

## ðŸ“‹ Overview

> [!NOTE]
> All mappings are **generated from SQL** â€” not from the old LLBLGen code. The generator reads the backup schema and produces English DDL, EF entities, and DbContext configuration.

| Category | Source | Output |
|----------|--------|--------|
| **Legacy schema** | `Bkp_Db/ABMATIC-create-local.sql` | Dutch tables (input) |
| **English DDL** | `Bkp_Db/WebShopABMATIC-create-local.sql` | 139 business tables |
| **EF entities** | `Model/Entities/` | 139 C# classes |
| **Persistence** | `Persistence/` | DbContext + ModelBuilder |
| **Generator** | `scripts/generate-from-sql.ps1` | Regenerates everything |

> [!TIP]
> Regenerate after any mapping change:

```powershell
powershell -ExecutionPolicy Bypass -File scripts/generate-from-sql.ps1
dotnet build Persistence/WebShopABMATIC.Data.Persistence.csproj
```

---

## ðŸ“ˆ Coverage Statistics

| Metric | Count | Status | Notes |
|--------|-------|--------|-------|
| **Tables in backup** | 146 | â„¹ï¸ Reference | Includes staging tables |
| **Business tables migrated** | 139 | âœ… Complete | All mapped to English |
| **Tables excluded** | 7 | âœ… Documented | Staging + legacy backups |
| **English schemas** | 11 | âœ… Complete | NL â†’ EN rename |
| **EF entities generated** | 139 | âœ… Complete | Build passes |
| **TypedList read models** | ~60 | ðŸŸ¡ Pending | Not in SQL â€” recreate in Application |

---

## ðŸ“Š Schema Summary

| English schema | Dutch schema | Tables | Domain |
|----------------|--------------|--------|--------|
| ðŸ“ **Files** | `Bestanden` | 4 | Azure blobs, stored files |
| ðŸ’° **Accounting** | `Boekhouding` | 6 | Invoices, VAT, Intrastat |
| ðŸ¤ **Crm** | `Crm` | 28 | Calendar, tasks, suppliers |
| ðŸ‘¤ **Customers** | `Klanten` | 4 | Customer master, contacts |
| ðŸ“§ **Emails** | `Emails` | 3 | Messages, attachments |
| âš™ï¸ **Settings** | `Instellingen` | 18 | Staff, templates, config |
| ðŸ“ **Logging** | `Logging` | 2 | Errors, project activity |
| ðŸ“¦ **Products** | `Products` | 38 | Catalog, pricing, stock |
| ðŸ—ï¸ **Projects** | `Projecten` | 31 | Orders, projects, timesheets |
| âœ… **Tasks** | `Taken` | 4 | Task templates, dependencies |
| ðŸ‘¥ **Users** | `Users` | 1 | Sick leave |

---

## âœ… Implementation Quality

| Aspect | Status | Details |
|--------|--------|---------|
| **Schema mapping** | âœ… Complete | 11 schemas NL â†’ EN |
| **Table mapping** | âœ… Complete | 139 / 139 business tables |
| **Column mapping** | âœ… Complete | `$ColumnReplacements` in generator |
| **FK normalization** | âœ… Complete | `KlantKlantId` â†’ `CustomerId`, etc. |
| **Duplicate resolution** | âœ… Complete | e.g. `SupplierId` / `RelatedSupplierId` |
| **Entity XML docs** | âœ… Complete | Legacy name in every entity summary |
| **TypedLists** | ðŸŸ¡ Pending | ~60 query models to recreate |

---

## ðŸ”„ 1. Source of truth

| Artifact | Path | Role |
|----------|------|------|
| Legacy SQL | `Bkp_Db/ABMATIC-create-local.sql` | **Input** â€” Dutch schema from backup |
| English SQL | `Bkp_Db/WebShopABMATIC-create-local.sql` | **Output** â€” vNext DDL |
| Generator | `scripts/generate-from-sql.ps1` | Parses SQL, applies mapping, codegen |
| Entities | `Model/Entities/` | 139 EF POCOs (`WebShopABMATIC.Data`) |
| DbContext | `Persistence/WebShopABMATICDbContext.cs` | EF Core entry point |

> [!WARNING]
> The old LLBLGen folders (`EntityClasses/`, `TypedListClasses/`) are **not** authoritative.

Each generated entity documents its mapping:

```csharp
/// Entity for [Projects].[Orders] (legacy: [Projecten].[Bestelling]).
public class Order { â€¦ }
```

---

## ðŸ—ºï¸ 2. Schema mapping

| Dutch schema | English schema | Purpose |
|--------------|----------------|---------|
| `Bestanden` | `Files` | File storage (Azure blobs, binary files) |
| `Boekhouding` | `Accounting` | Invoices, credit notes, accounting lines |
| `Crm` | `Crm` | CRM, calendar, tasks, suppliers |
| `Klanten` | `Customers` | Customer master data |
| `Instellingen` | `Settings` | Company settings, staff users, templates |
| `Logging` | `Logging` | Application / audit logs |
| `Products` | `Products` | Catalog, pricing, stock |
| `Projecten` | `Projects` | Projects, orders, order lines |
| `Emails` | `Emails` | Email messages and attachments |
| `Taken` | `Tasks` | Task templates and dependencies |
| `Users` | `Users` | HR / sick leave |

Legacy `dbo.*` import/staging tables are **not** migrated.

---

## ðŸ·ï¸ 3. Key domain renames

| Dutch (legacy UI / DB) | English (vNext) | Entity | Notes |
|------------------------|-------------------|--------|-------|
| `Dossier` / `Bestelling` | `Order` | `Order` | Sales order header |
| `DossierDetail` / `BestellingDetail` | `OrderLine` | `OrderLine` | Order line item |
| `Klant` | `Customer` | `Customer` | Customer master |
| `Documenten` | `AccountingDocument` | `AccountingDocument` | Invoice / credit note |
| `Werf` | `JobSite` | `JobSite` | Construction site |
| `Taken` | `TaskItem` | `TaskItem` | CRM task |
| `User` (staff) | `StaffUser` | `StaffUser` | Internal user |
| `Bijlage` | `EmailAttachment` | `EmailAttachment` | Email attachment link |
| `DossierProjectDetail` | `OrderProjectLine` | `OrderProjectLine` | Project BOM line |

---

## ðŸš« 4. Tables intentionally excluded

| Dutch table | Reason | Status |
|-------------|--------|--------|
| `dbo.abm$` | Product import staging | âŒ Excluded |
| `dbo.imp_Intrastat2` | Intrastat import staging | âŒ Excluded |
| `dbo.impStockPlaatsen` | Stock location import | âŒ Excluded |
| `dbo.instrastatimp` | Intrastat import | âŒ Excluded |
| `dbo.instrastatproductenupdateimport` | Product update import | âŒ Excluded |
| `Boekhouding.DocumentDetailold` | Legacy backup of document lines | âŒ Excluded |
| `Products.stockreserveringbackup` | Stock reservation backup | âŒ Excluded |

---

## ðŸ“‹ 5. Entity & table mapping (all 139 tables)

Grouped by English schema. SQL table names are plural; C# entity names are singular. Expand each section to see the full table list.

<details open>
<summary><strong>5.1 ðŸ“ Files (<code>Bestanden</code>) â€” 4 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `AzureFile` | `AzureFiles` | `AzureFile` |
| `AzureFileFolder` | `AzureFileFolders` | `AzureFileFolder` |
| `Bestand` | `StoredFiles` | `StoredFile` |
| `DossierBestanden` | `OrderFileLinks` | `OrderFileLink` |

</details>

<details>
<summary><strong>5.2 ðŸ’° Accounting (<code>Boekhouding</code>) â€” 6 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `BtwType` | `VatTypes` | `VatType` |
| `DocumentDetail` | `AccountingDocumentLines` | `AccountingDocumentLine` |
| `Documenten` | `AccountingDocuments` | `AccountingDocument` |
| `DocumentType` | `DocumentTypes` | `DocumentType` |
| `IntrastatReportLine` | `IntrastatReportLines` | `IntrastatReportLine` |
| `KlantExtraKortingen` | `CustomerExtraDiscounts` | `CustomerExtraDiscount` |

</details>

<details>
<summary><strong>5.3 ðŸ¤ Crm â€” 28 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Aanspreking` | `Salutations` | `Salutation` |
| `Activiteiten` | `Activities` | `Activity` |
| `Agenda` | `CalendarEntries` | `CalendarEntry` |
| `AgendaLabel` | `CalendarLabels` | `CalendarLabel` |
| `AgendaLog` | `CalendarLogs` | `CalendarLog` |
| `AgendaStatus` | `CalendarStatuses` | `CalendarStatus` |
| `Betaaltermijn` | `PaymentTerms` | `PaymentTerm` |
| `City` | `City` | `City` |
| `ContactProjectRol` | `ContactProjectRoles` | `ContactProjectRole` |
| `Country` | `Country` | `Country` |
| `KlantDossierStatusOpmerking` | `CustomerOrderStatusRemarks` | `CustomerOrderStatusRemark` |
| `KlantFollowUp` | `CustomerFollowUps` | `CustomerFollowUp` |
| `KlantJobcodeTarief` | `CustomerJobCodeRates` | `CustomerJobCodeRate` |
| `KlantLeveradres` | `CustomerDeliveryAddresses` | `CustomerDeliveryAddress` |
| `KlantLeverancierKorting` | `CustomerSupplierDiscounts` | `CustomerSupplierDiscount` |
| `KlantMaatProduct` | `CustomerCustomProducts` | `CustomerCustomProduct` |
| `KlantMaatProductDetail` | `CustomerCustomProductLines` | `CustomerCustomProductLine` |
| `KlantMaatproductStaffel` | `CustomerCustomProductTiers` | `CustomerCustomProductTier` |
| `KlantOpmerkingen` | `CustomerNotes` | `CustomerNote` |
| `KlantProductKorting` | `CustomerProductDiscounts` | `CustomerProductDiscount` |
| `KlantStatus` | `CustomerStatuses` | `CustomerStatus` |
| `Manufacturer` | `Manufacturer` | `Manufacturer` |
| `ProjectContact` | `ProjectContacts` | `ProjectContact` |
| `Supplier` | `Supplier` | `Supplier` |
| `SupplierConact` | `SupplierContacts` | `SupplierContact` |
| `TaakActies` | `TaskActions` | `TaskAction` |
| `TaakType` | `TaskTypes` | `TaskType` |
| `Taken` | `TaskItems` | `TaskItem` |

</details>

<details>
<summary><strong>5.4 ðŸ‘¤ Customers (<code>Klanten</code>) â€” 4 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Contact` | `Contact` | `Contact` |
| `Klant` | `Customers` | `Customer` |
| `KlantContact` | `CustomerContacts` | `CustomerContact` |
| `KlantType` | `CustomerTypes` | `CustomerType` |

</details>

<details>
<summary><strong>5.5 ðŸ“§ Emails â€” 3 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Bijlage` | `EmailAttachments` | `EmailAttachment` |
| `Email` | `EmailMessages` | `EmailMessage` |
| `EmailQueue` | `EmailQueues` | `EmailQueue` |

</details>

<details>
<summary><strong>5.6 âš™ï¸ Settings (<code>Instellingen</code>) â€” 18 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `AutoNummering` | `AutoNumberings` | `AutoNumbering` |
| `BaseCompany` | `BaseCompany` | `BaseCompany` |
| `BaseCompanyAccess` | `BaseCompanyAccess` | `BaseCompanyAcces` |
| `BaseCompanyVatNumber` | `BaseCompanyVatNumbers` | `BaseCompanyVatNumber` |
| `Betalingswijze` | `PaymentMethods` | `PaymentMethod` |
| `GridLayout` | `GridLayouts` | `GridLayout` |
| `HerstellingKostPrijs` | `RepairCostPrices` | `RepairCostPrice` |
| `LangTag` | `LanguageTags` | `LanguageTag` |
| `Parameter` | `AppSettings` | `AppSetting` |
| `ProductKortingSuggestie` | `ProductDiscountSuggestions` | `ProductDiscountSuggestion` |
| `ProductKortingSuggestieDetail` | `ProductDiscountSuggestionLines` | `ProductDiscountSuggestionLine` |
| `StdFacturatieVoorwaarden` | `StandardBillingTerms` | `StandardBillingTerm` |
| `StdFacturatieVoorwaardenDetail` | `StandardBillingTermLines` | `StandardBillingTermLine` |
| `Taal` | `Languages` | `Language` |
| `TemplateType` | `TemplateType` | `TemplateType` |
| `Templates` | `DocumentTemplates` | `DocumentTemplate` |
| `User` | `StaffUsers` | `StaffUser` |
| `UsrGroep` | `UserGroups` | `UserGroup` |

</details>

<details>
<summary><strong>5.7 ðŸ“ Logging â€” 2 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Error` | `AppErrors` | `AppError` |
| `ProjectActiviteit` | `ProjectActivities` | `ProjectActivity` |

</details>

<details>
<summary><strong>5.8 ðŸ“¦ Products â€” 38 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `DrawGroup` | `DrawGroup` | `DrawGroup` |
| `Grondstof` | `RawMaterials` | `RawMaterial` |
| `IntrastatCode` | `IntrastatCode` | `IntrastatCode` |
| `LosseProducten` | `MiscellaneousProducts` | `MiscellaneousProduct` |
| `OrderTemplate` | `OrderTemplate` | `OrderTemplate` |
| `OrderTemplateDetail` | `OrderTemplateDetail` | `OrderTemplateDetail` |
| `PrestatieTarief` | `ServiceRates` | `ServiceRate` |
| `PrijslijstCategorie` | `PriceListCategories` | `PriceListCategory` |
| `PrijslijstTeksten` | `PriceListTexts` | `PriceListText` |
| `Product` | `Product` | `Product` |
| `ProductAankoopKortingen` | `ProductPurchaseDiscounts` | `ProductPurchaseDiscount` |
| `ProductEenheid` | `ProductUnits` | `ProductUnit` |
| `ProductHandleiding` | `ProductManuals` | `ProductManual` |
| `ProductOptionValue` | `ProductOptionValue` | `ProductOptionValue` |
| `ProductOptions` | `ProductOptions` | `ProductOption` |
| `ProductPopupRetourKolom` | `ProductPopupReturnColumns` | `ProductPopupReturnColumn` |
| `ProductPopupTemplate` | `ProductPopupTemplate` | `ProductPopupTemplate` |
| `ProductPopupTemplateDetail` | `ProductPopupTemplateLines` | `ProductPopupTemplateLine` |
| `ProductPopupWaardeType` | `ProductPopupValueTypes` | `ProductPopupValueType` |
| `ProductPrijzen` | `ProductPrices` | `ProductPrice` |
| `ProductPrijzenVerkoopKorting` | `ProductPriceSalesDiscounts` | `ProductPriceSalesDiscount` |
| `ProductProductionGroup` | `ProductProductionGroup` | `ProductProductionGroup` |
| `ProductProductionsGroepen` | `ProductProductionGroupLinks` | `ProductProductionGroupLink` |
| `ProductPropertieItem` | `ProductPropertyItems` | `ProductPropertyItem` |
| `ProductProperty` | `ProductProperty` | `ProductProperty` |
| `ProductStaffel` | `ProductQuantityTiers` | `ProductQuantityTier` |
| `ProductStockLocatie` | `ProductStockLocations` | `ProductStockLocation` |
| `ProductStructuur` | `ProductStructures` | `ProductStructure` |
| `ProductStructuurWebShopABMATIC` | `WebShopABMATICProductStructures` | `WebShopABMATICProductStructure` |
| `ProductSubProduct` | `ProductSubProduct` | `ProductSubProduct` |
| `ProductType` | `ProductType` | `ProductType` |
| `ReportingGroep1` | `ReportingGroups` | `ReportingGroup` |
| `StockBeweging` | `StockMovements` | `StockMovement` |
| `StockLocatie` | `StockLocations` | `StockLocation` |
| `StockOrder` | `StockOrder` | `StockOrder` |
| `StockOrderDetail` | `StockOrderLines` | `StockOrderLine` |
| `StockOrderLevering` | `StockOrderDeliveries` | `StockOrderDelivery` |
| `WebShopABMATICStructuur` | `WebShopABMATICStructures` | `WebShopABMATICStructure` |

</details>

<details open>
<summary><strong>5.9 ðŸ—ï¸ Projects (<code>Projecten</code>) â€” 31 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Bestelling` | `Orders` | `Order` |
| `BestellingDetail` | `OrderLines` | `OrderLine` |
| `BestellingStatus` | `OrderStatuses` | `OrderStatus` |
| `BestellingStatusToegangen` | `OrderStatusAccesses` | `OrderStatusAccess` |
| `BestellingType` | `OrderTypes` | `OrderType` |
| `BinnengebrachtProduct` | `CustomerDeliveredProducts` | `CustomerDeliveredProduct` |
| `DossierDetailText` | `OrderLineTexts` | `OrderLineText` |
| `DossierDevelopmentDetail` | `OrderDevelopmentLines` | `OrderDevelopmentLine` |
| `DossierFeedback` | `OrderFeedbacks` | `OrderFeedback` |
| `DossierInstallatieDetail` | `OrderInstallationLines` | `OrderInstallationLine` |
| `DossierLeveringsTypeProduct` | `OrderDeliveryTypeProducts` | `OrderDeliveryTypeProduct` |
| `DossierLog` | `OrderLogs` | `OrderLog` |
| `DossierOpmerking` | `OrderRemarks` | `OrderRemark` |
| `DossierProjectDetail` | `OrderProjectLines` | `OrderProjectLine` |
| `DossierStatusGroep` | `OrderStatusGroups` | `OrderStatusGroup` |
| `DossierStructuur` | `OrderStructures` | `OrderStructure` |
| `DossierVerwerkingsType` | `OrderProcessingTypes` | `OrderProcessingType` |
| `DossierVoorschot` | `OrderAdvancePayments` | `OrderAdvancePayment` |
| `FacturatieAfspraak` | `BillingAgreements` | `BillingAgreement` |
| `JobCode` | `JobCode` | `JobCode` |
| `LeveringType` | `DeliveryTypes` | `DeliveryType` |
| `OnderhoudsContract` | `MaintenanceContracts` | `MaintenanceContract` |
| `OnderhoudsContractDetail` | `MaintenanceContractLines` | `MaintenanceContractLine` |
| `Project` | `Project` | `Project` |
| `ProjectInstallatie` | `ProjectInstallations` | `ProjectInstallation` |
| `ProjectLog` | `ProjectLog` | `ProjectLog` |
| `ProjectPartij` | `ProjectParties` | `ProjectParty` |
| `ProjectPartijContact` | `ProjectPartyContacts` | `ProjectPartyContact` |
| `ProjectPartijGroep` | `ProjectPartyGroups` | `ProjectPartyGroup` |
| `Timesheet` | `Timesheet` | `Timesheet` |
| `Werf` | `JobSites` | `JobSite` |

</details>

<details>
<summary><strong>5.10 âœ… Tasks (<code>Taken</code>) â€” 4 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `TaakDependency` | `TaskDependencies` | `TaskDependency` |
| `TaakTemplate` | `TaskTemplates` | `TaskTemplate` |
| `TaakTemplateDependencie` | `TaskTemplateDependencies` | `TaskTemplateDependency` |
| `TaakTemplateTaak` | `TaskTemplateTasks` | `TaskTemplateTask` |

</details>

<details>
<summary><strong>5.11 ðŸ‘¥ Users â€” 1 table</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Ziekte` | `SickLeaves` | `SickLeave` |

</details>

---

## ðŸ”¤ 6. Common column / property terms

| Dutch | English | Example |
|-------|---------|---------|
| `Naam` | `Name` | `KlantNaam` â†’ `CustomerName` |
| `Omschrijving` | `Description` | |
| `Opmerking` | `Notes` | |
| `Aantal` | `Quantity` | |
| `PrijsPerEenheid` | `UnitPrice` | |
| `StukPrijs` | `PieceUnitPrice` | When both exist on same table |
| `Bestelling` / `Dossier` | `Order` | `DossierId` â†’ `OrderId` |
| `Klant` (FK column) | `CustomerId` | `KlantKlantId` â†’ `CustomerId` |
| `Leverancier` | `Supplier` | `SupplierSupplierId` â†’ `SupplierId` |
| `GemaaktOp` / `AangemaaktOp` | `CreatedAt` | |
| `AangemaaktDoor` | `CreatedBy` | |
| `AangepastOp` | `ModifiedAt` | |
| `Volgorde` | `SortOrder` | |
| `Bus` | `Box` | Address box number |
| `Huisnr` | `HouseNumber` | |
| `Btwnr` / `Btw` | `VatNumber` / `Vat` | |
| `Voorschot` | `AdvancePayment` | |

### 6.1 FK normalization rules

| Pattern | Resolution | Example |
|---------|------------|---------|
| LLBLGen double FK | Canonical English name | `KlantKlantId` â†’ `CustomerId` |
| Duplicate property | Fallback suffix | `SupplierId` â†’ `RelatedSupplierId` |
| Price columns | Distinct names | `StukPrijs` â†’ `PieceUnitPrice` |

Column mappings live in `$ColumnReplacements` inside `scripts/generate-from-sql.ps1`.

---

## ðŸ“Š 7. Read models (former TypedLists)

| Aspect | Legacy | vNext plan |
|--------|--------|------------|
| **Count** | ~60 TypedList classes | Recreate as needed |
| **Source** | LLBLGen (not in SQL) | Application query DTOs |
| **Usage** | Grid/search projections | EF `IQueryable` or SQL views |
| **Port strategy** | N/A | Do not port verbatim |

---

## ðŸ” 8. Legacy â†’ vNext identity

| Legacy | vNext | Status |
|--------|-------|--------|
| `Instellingen.User` (`StaffUser`) | ASP.NET Core Identity (staff) | ðŸŸ¡ Planned |
| `Klanten.Klant.LoginWebShopABMATIC` | Storefront customer auth | ðŸŸ¡ Planned |
| `Klanten.Contact.ContactLogin` | Contact portal login | ðŸŸ¡ Evaluate per screen |

Staff permissions today live as bit flags on `StaffUser` (`Admin`, `Bestellingen`, `Productie`, â€¦). vNext maps these to **roles/policies** (`Admin`, `Manager`, `Customer`).

---


## 9. Code layout

```
WebShopABMATIC/              ← repo root
  Domain/                  # pure domain entities (hexagonal core)
  Application/             # use cases, DTOs, inbound/outbound ports
  Infrastructure/          # EF repositories, Identity, media
  Web/                     # Blazor Server UI (admin + store)
  Model/
    WebShopABMATIC.Data.csproj
    Entities/                # 139 EF persistence models
  Persistence/
    WebShopABMATIC.Data.Persistence.csproj
    WebShopABMATICDbContext.cs
    WebShopABMATICModelBuilder.cs
```

| Namespace | Project |
|-----------|---------|
| `WebShopABMATIC.Domain.*` | `WebShopABMATIC.Domain` |
| `WebShopABMATIC.Application.*` | `WebShopABMATIC.Application` |
| `WebShopABMATIC.Data.Entities` | `WebShopABMATIC.Data` |
| `WebShopABMATIC.Data.Persistence` | `WebShopABMATIC.Data.Persistence` |

---

## 10. Data migration from ABMATIC

| Step | Action | Command / artifact |
|------|--------|------------------|
| 1 | Restore legacy DB | `ABMATIC.bacpac` or `ABMATIC-create-local.sql` |
| 2 | Create English DB | `sqlcmd -S localhost -E -i Bkp_Db/WebShopABMATIC-create-local.sql` |
| 3 | ETL data | `INSERTâ€¦SELECT` using this mapping |
| 4 | Point vNext | Connection string â†’ English `WebShopABMATIC` DB |
| 5 | Future changes | EF Core migrations (baseline = generated SQL) |


## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**
