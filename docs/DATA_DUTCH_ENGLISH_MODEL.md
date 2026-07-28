# ðŸ—‚ï¸ Dutch â†’ English Data Model Mapping

<p style="display:flex;flex-wrap:nowrap;gap:0.35rem;align-items:center;overflow-x:auto;margin:0.5rem 0 0;"><img alt="Status" src="https://img.shields.io/badge/Status-Complete-28a745?style=flat-square" /><img alt="Tables" src="https://img.shields.io/badge/Tables-139-0d47a1?style=flat-square" /><img alt="Schemas" src="https://img.shields.io/badge/Schemas-11-512BD4?style=flat-square" /><img alt="Coverage" src="https://img.shields.io/badge/Coverage-100%25-ff6f00?style=flat-square" /></p>

## ðŸ“‹ Overview

> [!IMPORTANT]
> **Database first (global):** Live Azure SQL **`abmatic_test`** is the source of truth. The app maps English C# names â†’ Dutch physical schema.  
> **Never invent** columns, tables, EF migrations, `dotnet ef database update`, or schema scripts for the ERP â€” for **any** feature. Map to what already exists.

### Language layers (DE-PARA)

| Layer | Convention |
|-------|------------|
| **SQL / ERP** | Mostly **Dutch** (`Projecten`, `Klanten`, `ProductPrijzen`, â€¦). Physical names never â€œfixedâ€ from the app. |
| **C# code** | **English** types, properties, ports, use cases (`DeliveryType`, `GrossSalesPrice`). |
| **EF mapping** | English property â†’ Dutch column in `WebShopABMATICModelBuilder` (+ entity XML with legacy Dutch name). |
| **Labels / product names / messages from ERP** | Keep **as stored** (usually Dutch). UI chrome may stay English. |

Freight/delivery fee mapping (no mock â‚¬9): [DATA_FREIGHT_DELIVERY.md](./DATA_FREIGHT_DELIVERY.md).

| Category | Role |
|----------|------|
| **Live DB** | `abmatic.database.windows.net` / `abmatic_test` â€” **authoritative** Dutch ERP tables |
| **EF entities** | `Model/Entities/` â€” 139 C# classes mapped to Dutch tables |
| **Persistence** | `Persistence/` â€” `WebShopABMATICDbContext` + `WebShopABMATICModelBuilder` |
| **Historical SQL dumps** | `Bkp_Db/*` (if present) â€” **reference only**, not a migrate/seed workflow |

---

## ðŸ“ˆ Coverage Statistics

| Metric | Count | Status | Notes |
|--------|-------|--------|-------|
| **Business tables** | 139 | âœ… Complete | Mapped in EF (`WebShopABMATICDbContext`) to live `abmatic_test` |
| **English schemas** | 11 | âœ… Complete | NL â†’ EN in code; Dutch names in SQL |
| **EF entities** | 139 | âœ… Complete | `Model/Entities/` |
| **TypedList read models** | ~60 | ðŸŸ¡ Pending | Recreate as Application query DTOs |

---

## ðŸ“Š Schema Summary

| English schema | Dutch schema | Tables | Domain |
|----------------|--------------|--------|--------|
| ðŸ“ **Files** | `Bestanden` | 4 | Azure blobs, stored files |
| ðŸ’° **Accounting** | `Boekhouding` | 6 | Invoices, VAT, Intrastat |
| ðŸ¤ **Crm** | `Crm` | 28 | Calendar, tasks, suppliers |
| ðŸ‘¤ **Customers** | `Klanten` | 4 | Customer master, contacts |
| ðŸ“§ **Emails** | `Emails` | 3 | Messages, attachments |
| âš™ï¸ **Settings** | `Instellingen` | 18 | Staff, document types, config |
| ðŸ“ **Logging** | `Logging` | 2 | Errors, project activity |
| ðŸ“¦ **Products** | `Products` | 38 | Catalog, pricing, stock |
| ðŸ—ï¸ **Projects** | `Projecten` | 31 | Orders, projects, timesheets |
| âœ… **Tasks** | `Taken` | 4 | Task definitions, dependencies |
| ðŸ‘¥ **Users** | `Users` | 1 | Sick leave |

---

## âœ… Implementation Quality

| Aspect | Status | Details |
|--------|--------|---------|
| **Schema mapping** | âœ… Complete | 11 schemas NL â†’ EN |
| **Table mapping** | âœ… Complete | 139 / 139 business tables |
| **Column mapping** | âœ… Complete | Fluent mapping in `WebShopABMATICModelBuilder` |
| **FK normalization** | âœ… Complete | `KlantKlantId` â†’ `CustomerId`, etc. |
| **Duplicate resolution** | âœ… Complete | e.g. `SupplierId` / `RelatedSupplierId` |
| **Entity XML docs** | âœ… Complete | Legacy name in every entity summary |
| **TypedLists** | ðŸŸ¡ Pending | ~60 query models to recreate |

---

## ðŸ”„ 1. Source of truth

| Artifact | Path / location | Role |
|----------|-----------------|------|
| **Live ERP database** | Azure SQL `abmatic_test` | **Authoritative** Dutch schema + data |
| Entities | `Model/Entities/` | 139 EF POCOs (`WebShopABMATIC.Data`) |
| DbContext | `Persistence/WebShopABMATICDbContext.cs` | EF Core entry point |
| ModelBuilder | `Persistence/` mapping | English â†” Dutch physical names |

> [!WARNING]
> Do **not** use EF migrations or repo SQL scripts to alter `abmatic_test`.  
> Historical `Bkp_Db/` / codegen scripts (if still in the tree) are **archive/reference only**.

Each entity documents its mapping:

```csharp
/// Entity for [Projects].[Orders] (legacy: [Projecten].[Bestelling]).
public class Order { â€¦ }
```

---

## ðŸ—ºï¸ 2. Schema Mapping

| Dutch schema | English schema | Purpose |
|--------------|----------------|---------|
| `Bestanden` | `Files` | File storage (Azure blobs, binary files) |
| `Boekhouding` | `Accounting` | Invoices, credit notes, accounting lines |
| `Crm` | `Crm` | CRM, calendar, tasks, suppliers |
| `Klanten` | `Customers` | Customer master data |
| `Instellingen` | `Settings` | Company settings, staff users, document types |
| `Logging` | `Logging` | Application / audit logs |
| `Products` | `Products` | Catalog, pricing, stock |
| `Projecten` | `Projects` | Projects, orders, order lines |
| `Emails` | `Emails` | Email messages and attachments |
| `Taken` | `Tasks` | Task definitions and dependencies |
| `Users` | `Users` | HR / sick leave |

---

## ðŸ·ï¸ 3. Key Domain Renames

| Dutch (legacy UI / DB) | English (C# / EF) | Entity | Notes |
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

## âž• 4. Schema Extensions / Integrations

The application uses the **139 legacy business tables** on `abmatic_test`. English property names in C# map to Dutch schema/table/column names via `WebShopABMATICModelBuilder`.

### ðŸ’³ Payments (Mollie)

Store/checkout integrations **encode** payment state in **existing** ERP advance-payment fields (see [SPEC_MOLLIE_PAYMENTS_open.md](./SPEC_MOLLIE_PAYMENTS_open.md) and store checkout code). Do **not** add Mollie columns via EF migrations.

### ðŸ” Webshop Auth (`Klanten.Klant`)

| Dutch column | C# property | Purpose |
|--------------|-------------|---------|
| `LoginWebshop` | `WebshopLogin` | Store customer login |
| `PasswordWebshop` | `WebshopPasswordHash` | Password hash |
| `SaltWebshop` | `WebshopPasswordSalt` | Password salt |

Staff admin login: **`Instellingen.User`** (`Settings.StaffUsers`) â€” `Login` + `Password`.

### â˜ï¸ Azure Database

| Aspect | Value |
|--------|--------|
| **Server** | `abmatic.database.windows.net` |
| **Database** | `abmatic_test` |
| **Schema** | Dutch legacy (139 tables) â€” **DB-first** |
| **EF** | Read/write mapped tables only â€” **no** `dotnet ef database update` for ERP |
| **Mapping** | `WebShopABMATICModelBuilder` â†’ Dutch physical names |

---

## ðŸ“‹ 5. Entity & Table Mapping (All 139 Tables)

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
| `Error` | `AppErrors` | `AppError` â€” auth, CRUD, exports, exceptions |
| `ProjectActiviteit` | `ProjectActivities` | `ProjectActivity` â€” project `Actie` codes on order events |

**WebShop writes:** `LegacyAuditService` â†’ tables above (see [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) Â§3.5). Stock journal: `[Products].[StockBeweging]` / `StockMovement` (separate).

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
| `ProductAttribuut` | `ProductAttributes` | `ProductAttribute` | **New (catalog filters)** â€” dictionary; see [PLAN_CATALOG_FILTERS.md](./PLAN_CATALOG_FILTERS.md) |
| `ProductAttribuutItem` | `ProductAttributeValues` | `ProductAttributeValue` | **New (catalog filters)** â€” per-product `Waarde`; `ProductProdId` â†’ `ProductId` |
| `ProductPropertieItem` | `ProductPropertyItems` | `ProductPropertyItem` | Legacy ERP property sheet â€” **not** used for store catalog filters |
| `ProductProperty` | `ProductProperty` | `ProductProperty` | Legacy â€” **not** used for store catalog filters |
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
| `DossierLog` | `OrderLogs` | `OrderLog` â€” webshop checkout / payment / cancel lines |
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
| `Waarde` | `Value` | e.g. `ProductAttribuutItem.Waarde` |
| `NaamEn` / `NaamNl` / `NaamFr` | `NameEn` / `NameNl` / `NameFr` | Attribute dictionary labels |
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

Column mappings are maintained in `WebShopABMATICModelBuilder` (fluent EF). Historical codegen scripts are **not** part of the active workflow.

---

## ðŸ“Š 7. Read models (former TypedLists)

| Aspect | Legacy (LLBLGen) | WebShopABMATIC |
|--------|------------------|----------------|
| **Count** | ~60 TypedList classes | Recreate as needed |
| **Source** | LLBLGen (not in SQL) | Application query DTOs |
| **Usage** | Grid/search projections | EF `IQueryable` or SQL views |
| **Port strategy** | N/A | Do not port verbatim |

---

## ðŸ” 8. Authentication

| Portal | Legacy table | Login fields | App service |
|--------|--------------|--------------|-------------|
| **Admin** | `Instellingen.User` â†’ `Settings.StaffUsers` | `Login`, `Password` | `LegacySignInService.SignInStaffAsync` â†’ POST `/account/admin-login` |
| **Store** | `Klanten.Klant` â†’ `Customers.Customers` | `LoginWebshop`, `PasswordWebshop`, `SaltWebshop` | `LegacySignInService.SignInCustomerAsync` â†’ POST `/account/store-login` |
| **Session** | â€” | Cookie `.WebShopABMATIC.Auth` | `LegacyCookieAuthentication` + `LegacyAuthenticationStateProvider` |

Staff bit flags (`Admin`, `Bestellingen`, `Productie`, â€¦) map to cookie **roles**: `Admin`, `Manager`, `Customer` (store only).

Staff â†” group: `Instellingen.User.UsrGroepId` â†’ `StaffUser.UserGroupId` â†’ `[Instellingen].[UsrGroep]` (`UserGroup`). Admin UI: `/admin/staff-users` (password + group + Admin/Manager) and `/admin/user-groups`. No separate My profile — staff data only on Staff user.

---


## 9. Code layout

```
WebShopABMATIC/              â† repo root
  Domain/                  # pure domain entities (hexagonal core)
  Application/             # use cases, DTOs, inbound/outbound ports
  Infrastructure/          # EF repositories, auth, media, Mollie
  WebShopABMATIC.Client/   # Blazor Server UI (admin + store)
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

## 10. Working against ABMATIC (DB-first)

| Step | Action |
|------|--------|
| 1 | Connect to Azure SQL `abmatic_test` (`connWebShopABMATIC`) |
| 2 | Map entities via `WebShopABMATICModelBuilder` to **existing** Dutch tables |
| 3 | Use data already on `abmatic_test` â€” see [DATA_SUMMARY.md](./DATA_SUMMARY.md) |
| 4 | **Never** apply EF migrations or schema scripts from this app to change the ERP DB |

## Documentation

- ðŸ  [Main Documentation](../README.md) â€” Project overview and requirements

---

**Â© 2026 AdminSense. All rights reserved.**
