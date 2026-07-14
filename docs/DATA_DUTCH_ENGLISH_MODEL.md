# 🗂️ Dutch → English Data Model Mapping

![Status](https://img.shields.io/badge/Status-Complete-28a745?style=flat-square) ![Tables](https://img.shields.io/badge/Tables-139-0d47a1?style=flat-square) ![Schemas](https://img.shields.io/badge/Schemas-11-512BD4?style=flat-square) ![Coverage](https://img.shields.io/badge/Coverage-100%25-ff6f00?style=flat-square)

## 📋 Overview

> [!NOTE]
> All mappings are **generated from SQL** — not from the old LLBLGen code. The generator reads the backup schema and produces English DDL, EF entities, and DbContext configuration.

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

## 📈 Coverage Statistics

| Metric | Count | Status | Notes |
|--------|-------|--------|-------|
| **Business tables** | 139 | ✅ Complete | Mapped in EF (`WebShopABMATICDbContext`) |
| **EF column patch** | 4 columns | ✅ Active | Mollie fields on `OrderAdvancePayment` |
| **English schemas** | 11 | ✅ Complete | NL → EN in code; Dutch names in SQL |
| **EF entities** | 139 | ✅ Complete | `Model/Entities/` |
| **TypedList read models** | ~60 | 🟡 Pending | Recreate as Application query DTOs |

---

## 📊 Schema Summary

| English schema | Dutch schema | Tables | Domain |
|----------------|--------------|--------|--------|
| 📁 **Files** | `Bestanden` | 4 | Azure blobs, stored files |
| 💰 **Accounting** | `Boekhouding` | 6 | Invoices, VAT, Intrastat |
| 🤝 **Crm** | `Crm` | 28 | Calendar, tasks, suppliers |
| 👤 **Customers** | `Klanten` | 4 | Customer master, contacts |
| 📧 **Emails** | `Emails` | 3 | Messages, attachments |
| ⚙️ **Settings** | `Instellingen` | 18 | Staff, document types, config |
| 📝 **Logging** | `Logging` | 2 | Errors, project activity |
| 📦 **Products** | `Products` | 38 | Catalog, pricing, stock |
| 🏗️ **Projects** | `Projecten` | 31 | Orders, projects, timesheets |
| ✅ **Tasks** | `Taken` | 4 | Task definitions, dependencies |
| 👥 **Users** | `Users` | 1 | Sick leave |

---

## ✅ Implementation Quality

| Aspect | Status | Details |
|--------|--------|---------|
| **Schema mapping** | ✅ Complete | 11 schemas NL → EN |
| **Table mapping** | ✅ Complete | 139 / 139 business tables |
| **Column mapping** | ✅ Complete | `$ColumnReplacements` in generator |
| **FK normalization** | ✅ Complete | `KlantKlantId` → `CustomerId`, etc. |
| **Duplicate resolution** | ✅ Complete | e.g. `SupplierId` / `RelatedSupplierId` |
| **Entity XML docs** | ✅ Complete | Legacy name in every entity summary |
| **TypedLists** | 🟡 Pending | ~60 query models to recreate |

---

## 🔄 1. Source of truth

| Artifact | Path | Role |
|----------|------|------|
| Legacy SQL | `Bkp_Db/ABMATIC-create-local.sql` | **Input** — Dutch schema from backup |
| English SQL | `Bkp_Db/WebShopABMATIC-create-local.sql` | **Reference** — English DDL for codegen |
| Generator | `scripts/generate-from-sql.ps1` | Parses SQL, applies mapping, codegen |
| Entities | `Model/Entities/` | 139 EF POCOs (`WebShopABMATIC.Data`) |
| DbContext | `Persistence/WebShopABMATICDbContext.cs` | EF Core entry point |

> [!WARNING]
> The old LLBLGen folders (`EntityClasses/`, `TypedListClasses/`) are **not** authoritative.

Each generated entity documents its mapping:

```csharp
/// Entity for [Projects].[Orders] (legacy: [Projecten].[Bestelling]).
public class Order { … }
```

---

## 🗺️ 2. Schema mapping

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

## 🏷️ 3. Key domain renames

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

## ➕ 4. Schema extensions used by the app

The application uses the **139 legacy business tables** on `abmatic_test`. English property names in C# map to Dutch schema/table/column names via `WebShopABMATICModelBuilder`.

### EF migration (Mollie)

| Dutch table | English entity | Columns added | Purpose |
|-------------|----------------|---------------|---------|
| `Projecten.DossierVoorschot` | `OrderAdvancePayment` | `MollieCheckoutUrl`, `MolliePaidAt`, `MolliePaymentId`, `MolliePaymentStatus` | Mollie advance payment |

**Migration:** `20260606141224_OrderAdvancePaymentMollieColumns`.

### Webshop auth (`Klanten.Klant`)

| Dutch column | C# property | Purpose |
|--------------|-------------|---------|
| `LoginWebshop` | `WebshopLogin` | Store customer login |
| `PasswordWebshop` | `WebshopPasswordHash` | Password hash |
| `SaltWebshop` | `WebshopPasswordSalt` | Password salt |

Staff admin login: **`Instellingen.User`** (`Settings.StaffUsers`) — `Login` + `Password`.

### Azure database

| Aspect | Value |
|--------|--------|
| **Server** | `abmatic.database.windows.net` |
| **Database** | `abmatic_test` |
| **Schema** | Dutch legacy (139 tables) |
| **EF** | `dotnet ef database update` on `WebShopABMATICDbContext` when migrations change |
| **Mapping** | `WebShopABMATICModelBuilder` → Dutch physical names |

---

## 📋 5. Entity & table mapping (all 139 tables)

Grouped by English schema. SQL table names are plural; C# entity names are singular. Expand each section to see the full table list.

<details open>
<summary><strong>5.1 📁 Files (<code>Bestanden</code>) — 4 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `AzureFile` | `AzureFiles` | `AzureFile` |
| `AzureFileFolder` | `AzureFileFolders` | `AzureFileFolder` |
| `Bestand` | `StoredFiles` | `StoredFile` |
| `DossierBestanden` | `OrderFileLinks` | `OrderFileLink` |

</details>

<details>
<summary><strong>5.2 💰 Accounting (<code>Boekhouding</code>) — 6 tables</strong></summary>

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
<summary><strong>5.3 🤝 Crm — 28 tables</strong></summary>

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
<summary><strong>5.4 👤 Customers (<code>Klanten</code>) — 4 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Contact` | `Contact` | `Contact` |
| `Klant` | `Customers` | `Customer` |
| `KlantContact` | `CustomerContacts` | `CustomerContact` |
| `KlantType` | `CustomerTypes` | `CustomerType` |

</details>

<details>
<summary><strong>5.5 📧 Emails — 3 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Bijlage` | `EmailAttachments` | `EmailAttachment` |
| `Email` | `EmailMessages` | `EmailMessage` |
| `EmailQueue` | `EmailQueues` | `EmailQueue` |

</details>

<details>
<summary><strong>5.6 ⚙️ Settings (<code>Instellingen</code>) — 18 tables</strong></summary>

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
<summary><strong>5.7 📝 Logging — 2 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Error` | `AppErrors` | `AppError` — auth, CRUD, exports, exceptions |
| `ProjectActiviteit` | `ProjectActivities` | `ProjectActivity` — project `Actie` codes on order events |

**WebShop writes:** `LegacyAuditService` → tables above (see [SPEC_INFRASTRUCTURE.md](./SPEC_INFRASTRUCTURE.md) §3.5). Stock journal: `[Products].[StockBeweging]` / `StockMovement` (separate).

</details>

<details>
<summary><strong>5.8 📦 Products — 38 tables</strong></summary>

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
<summary><strong>5.9 🏗️ Projects (<code>Projecten</code>) — 31 tables</strong></summary>

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
| `DossierLog` | `OrderLogs` | `OrderLog` — webshop checkout / payment / cancel lines |
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
<summary><strong>5.10 ✅ Tasks (<code>Taken</code>) — 4 tables</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `TaakDependency` | `TaskDependencies` | `TaskDependency` |
| `TaakTemplate` | `TaskTemplates` | `TaskTemplate` |
| `TaakTemplateDependencie` | `TaskTemplateDependencies` | `TaskTemplateDependency` |
| `TaakTemplateTaak` | `TaskTemplateTasks` | `TaskTemplateTask` |

</details>

<details>
<summary><strong>5.11 👥 Users — 1 table</strong></summary>

| Dutch table | English table | Entity |
|-------------|---------------|--------|
| `Ziekte` | `SickLeaves` | `SickLeave` |

</details>

---

## 🔤 6. Common column / property terms

| Dutch | English | Example |
|-------|---------|---------|
| `Naam` | `Name` | `KlantNaam` → `CustomerName` |
| `Omschrijving` | `Description` | |
| `Opmerking` | `Notes` | |
| `Aantal` | `Quantity` | |
| `PrijsPerEenheid` | `UnitPrice` | |
| `StukPrijs` | `PieceUnitPrice` | When both exist on same table |
| `Bestelling` / `Dossier` | `Order` | `DossierId` → `OrderId` |
| `Klant` (FK column) | `CustomerId` | `KlantKlantId` → `CustomerId` |
| `Leverancier` | `Supplier` | `SupplierSupplierId` → `SupplierId` |
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
| LLBLGen double FK | Canonical English name | `KlantKlantId` → `CustomerId` |
| Duplicate property | Fallback suffix | `SupplierId` → `RelatedSupplierId` |
| Price columns | Distinct names | `StukPrijs` → `PieceUnitPrice` |

Column mappings live in `$ColumnReplacements` inside `scripts/generate-from-sql.ps1`.

---

## 📊 7. Read models (former TypedLists)

| Aspect | Legacy (LLBLGen) | WebShopABMATIC |
|--------|------------------|----------------|
| **Count** | ~60 TypedList classes | Recreate as needed |
| **Source** | LLBLGen (not in SQL) | Application query DTOs |
| **Usage** | Grid/search projections | EF `IQueryable` or SQL views |
| **Port strategy** | N/A | Do not port verbatim |

---

## 🔐 8. Authentication

| Portal | Legacy table | Login fields | App service |
|--------|--------------|--------------|-------------|
| **Admin** | `Instellingen.User` → `Settings.StaffUsers` | `Login`, `Password` | `LegacySignInService.SignInStaffAsync` → POST `/account/admin-login` |
| **Store** | `Klanten.Klant` → `Customers.Customers` | `LoginWebshop`, `PasswordWebshop`, `SaltWebshop` | `LegacySignInService.SignInCustomerAsync` → POST `/account/store-login` |
| **Session** | — | Cookie `.WebShopABMATIC.Auth` | `LegacyCookieAuthentication` + `LegacyAuthenticationStateProvider` |

Staff bit flags (`Admin`, `Bestellingen`, `Productie`, …) map to cookie **roles**: `Admin`, `Manager`, `Customer` (store only).

---


## 9. Code layout

```
WebShopABMATIC/              ← repo root
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

## 10. Data migration from ABMATIC

| Step | Action | Command / artifact |
|------|--------|------------------|
| 1 | Azure database | `abmatic_test` on `abmatic.database.windows.net` |
| 2 | Apply EF migrations | `dotnet ef database update` on `WebShopABMATICDbContext` |
| 3 | Demo data | Maintained on `abmatic_test` (Azure) — see [DATA_SUMMARY.md](./DATA_SUMMARY.md) |
| 4 | Column mapping | `WebShopABMATICModelBuilder` → Dutch physical names |


## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**
