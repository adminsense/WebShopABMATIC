# Audit System - Immo

![Status](https://img.shields.io/badge/Status-Production%20Ready-success?style=flat-square) ![Version](https://img.shields.io/badge/Version-1.2-blue?style=flat-square) ![Updated](https://img.shields.io/badge/Updated-2026--04--03-informational?style=flat-square)

**Complete audit trail automatically tracking all CRUD operations across 30+ pages in the system.**

---

## 1. 🎯 Access

**URL:** `/audit-logs` | **Access:** Administrators only | **Menu:** Audit Logs (📋 clipboard icon)

---

## 2. 📊 Main Grid Screen

![Audit Logs Grid](images/tela_grid.png)

### Grid Features

**Advanced Filters (top section):**
- **Date From / Date To**: Filter by date range
- **Action**: Dropdown with organized options in categories:
  - **Authentication** - Login, Logout, Enable 2FA, Disable 2FA
  - **CRUD - Core** - Create, Update, Delete (standard operations)
  - **Workflow** - WKFLOWSAVE, WKFLOWACCEPT, WKFLOWDECLINE, WKFLOWFINISH (guided workflow events)
  - **Payments** - RECALCPAYSCHEDULE, Create/Update/Delete Rent Payment, ApplyRentIndex
  - **Contracts** - CONTRACTCHANGESTATUS (status transitions)
  - **Reports** - Report Export, Report Export (Detailed)
  - **Data Access** - Data Access Export, Query, View
- **Severity**: Dropdown - All, Information, Warning, Error, Critical
- **User**: Search by username or email
- **Person**: Filter by table/entity name (Contact, Owner, Tenant, Guarantor, etc)
- **Status**: All, Success, Failed
- **Buttons**: 
  - **Apply Filters** (blue button with icon) - Execute search with selected criteria
  - **Clear** (red button with icon) - Reset all filters
  - **Badges legend** (yellow button with help icon) - Opens a modal with all Action badges (icon + name)

**Grid Columns:**
- **Timestamp**: Exact date and time of action (yyyy-MM-dd HH:mm:ss format)
- **Severity**: Badge showing level - Information (blue), Warning (yellow), Error/Critical (red)
- **Action**: Color-coded badge:
  - 🟢 **CREATE** (green, `bg-success`) - New record created
  - 🔵 **UPDATE** (blue, `bg-primary`) - Record modified
  - 🔴 **DELETE** (red, `bg-danger`) - Record deleted
  - 🔵 **LOGIN** (blue, `bg-primary`) - User authentication
  - ⚫ **LOGOUT** (gray, `bg-secondary`) - User logout
  - 🔐 **ENABLE2FA** (blue, `bg-primary`) - Two-factor authentication enabled
  - 🔓 **DISABLE2FA** (gray, `bg-secondary`) - Two-factor authentication disabled
  - 🏷️ **LABELOVERRIDE** (gray, `.audit-badge-admin` / `#5c636a`) - UI label override saved (Admin → Labels beheren)
  - 📈 **APPLYRENTINDEX** (yellow/orange, `.audit-badge-payment` / `#fd7e14`) - Apply rent index adjustment
  - 🔷 **RECALCPAYSCHEDULE** (yellow/orange, `.audit-badge-payment` / `#fd7e14`) - Automatic payment schedule recalculation
  - 🧾 **CONTRACTCHANGESTATUS** (cyan, `.audit-badge-contract` / `#0dcaf0`) - Contract status transition (e.g. Active → Terminated) with key lifecycle dates
  - 💾 **WKFLOWSAVE** (blue, `bg-info`) - Workflow step 4: Contract document prepared for saving
  - ✅ **WKFLOWACCEPT** (green, `bg-success`) - Workflow step 5: User accepted contract
  - 🚫 **WKFLOWDECLINE** (red, `bg-danger`) - Workflow step 5: User declined/rejected contract
  - ✓ **WKFLOWFINISH** (green, `bg-success`) - Workflow step 6: Contract created in database after acceptance
  - 📄 **CREATEDOC** (green, `bg-success`) - Document uploaded from disk
  - 📝 **DOCFROMTEMPLATE** (blue, `bg-primary`) - Document created from template
  - 📄 **DELETEDOC** (red, `bg-danger`) - Document soft-deleted
  - 📤 **FILEMANAGERUPLOAD** (green, `bg-success`) - File Manager: upload
  - 📁 **FILEMANAGERCREATEFOLDER** (green, `bg-success`) - File Manager: create folder
  - ↔️ **FILEMANAGERMOVE** (blue, `bg-primary`) - File Manager: move file
  - ✏️ **FILEMANAGERRENAME** (blue, `bg-primary`) - File Manager: rename file
  - 🗑️ **FILEMANAGERDELETEFOLDER** (red, `bg-danger`) - File Manager: delete folder (only when empty)
  - ⚠️ **FILEMANAGEROUTOFSYNC** (dark, `bg-dark`) - File Manager: disk vs database out-of-sync (Critical)
  - 🟣 **Rental report `reportKey` values (Entity column):** `ContractsReport`, `RentPaymentsReport`, `RentIndexationsReport`
- **Entity**: For CUD, table/entity name (e.g., Contact, Owner, Tenant, Building, Unit); for exports, the `reportKey` (e.g., `RentPaymentsReport`); for payment actions, the entity involved in calculation (PaymentSchedule); for workflow, the contract type (RentalContract)
- **User**: Email or name of who performed the action (or "System" for automated actions)
- **Entity ID**: Unique identifier of the affected record
- **IP Address**: Origin IP address of the request
- **Status**: ✅ Success (green checkmark) or ❌ Failed (red X)
- **Actions**: 👁️ View details button (blue) - Opens modal with complete information

**Features:**
- **Sticky Header**: Table header stays visible while scrolling through large result sets
- **Pagination**: 50 logs per page with Previous/Next navigation
- **Row Highlighting**: Failed operations shown with red background
- **Sortable**: Data sorted by timestamp (most recent first)

---

## 3. 📋 Audited Actions Reference

**Normalized Action Categories:**

### 🟢 CREATE — New Records
| Entity | Adapter | Notes |
|--------|---------|-------|
| **Owner** | OwnerAdapter | Property owner information |
| **Building** | BuildingAdapter | Building/property records |
| **Building Owner** | BuildingOwnerAdapter | Ownership relationships |
| **Contact** | AuthenticationAdapter | User/contact profiles |
| **Contact Role** | LookupAdapter | Role definitions (Tenant, Owner, etc) |
| **Unit** | UnitAdapter | Unit/apartment records |
| **Unit Type** | LookupAdapter | Unit classification types |
| **Unit Fixed Cost** | UnitFixedCostAdapter | Utility costs, fees, heating |
| **Unit Utility Meter** | UnitUtilityMeterAdapter | Utility meter records |
| **Utility Type** | LookupAdapter | Utility classification (Heat, Water, etc) |
| **Utility Meter Reading** | UtilityMeterReadingAdapter | Meter reading data |
| **Unit Telecom Connection** | UnitTelecomConnectionAdapter | Phone/telecom data |
| **Telecom Type** | LookupAdapter | Telecom classification |
| **Inspection** | InspectionAdapter | Property inspections |
| **Inspection Item** | InspectionItemAdapter | Inspection checklist items |
| **Inspection Photo** | InspectionPhotoAdapter | Inspection photos/attachments |
| **Inspection Type** | LookupAdapter | Inspection classification |
| **Expense** | ExpenseAdapter | Operational expenses |
| **Expense Category** | LookupAdapter | Expense classification |
| **Rental Contract** | RentalContractAdapter | Normal form: `CreateRentalContract` via `/rental/contracts`; Workflow form: `WKFLOWSAVE` (step 4) → `WKFLOWACCEPT` (step 5) → `WKFLOWFINISH` (step 6, creates contract) via `/menu/workflow` |
| **Rental Contract Party** | RentalContractPartyAdapter | Contract tenants/guarantors/owners |
| **Rent Payment** | RentPaymentAdapter | Payment records |
| **Rent Indexation** | RentIndexationAdapter | Rent index calculations - logs `APPLYRENTINDEX` action |
| **Payment Schedule** | PaymentScheduleAdapter | Generated payment schedules; logs `RECALCPAYSCHEDULE` when automatic recalculation happens |
| **Health Index** | HealthIndexValueAdapter | Health index values |
| **Word Document** | WordViewerAdapter | Document uploaded directly from disk (action: `CREATEDOC`) |
| **Word Document (Template)** | WordViewerAdapter | Document created from template (action: `DOCFROMTEMPLATE`) |

---

## 4. 📄 WORD VIEWER — Document Management ![UNDER DEVELOPMENT](https://img.shields.io/badge/UNDER%20DEVELOPMENT-red?style=flat)

**Actions:**
- **🟢 CREATEDOC** (green, `bg-success`) - Document uploaded from disk (.doc or .docx)
- **📝 DOCFROMTEMPLATE** (blue, `.audit-badge-template` style) - Document created from predefined template
- **🔴 DELETEDOC** (red, `bg-danger`) - Document soft-deleted (retained for audit trail)

| Action | Trigger | Entity | Notes |
|--------|---------|--------|-------|
| **CREATEDOC** | User uploads .doc or .docx file from disk | WordDocument | Direct file upload; stores file size, name, contact ID in NewValues |
| **DOCFROMTEMPLATE** | User creates document from template.docx | WordDocument | Uses predefined template; stores template origin, file name, contact ID in NewValues |
| **DELETEDOC** | User deletes document from list | WordDocument | Soft delete - document marked as deleted but retained for audit; document ID stored in EntityId |

---

## 4.1 📁 FILE MANAGER — Contract Documents

**Actions (logged via `IAuditService` from `/menu/file-manager`):**

| Action | Trigger | Entity | Notes |
|--------|---------|--------|-------|
| **FILEMANAGERUPLOAD** | User uploads file | FileManager | NewValues: `FileName`, `FolderId`, `SizeBytes` |
| **Delete** | User deletes file | FileManager | EntityName must be `FileManager`. NewValues: `FileName`, `FolderId` |
| **FILEMANAGERMOVE** | User moves file to another folder | FileManager | NewValues: `FileName`, `SourceFolderId`, `TargetFolderId` (maps to `OriginFrom`/`OriginTo`) |
| **FILEMANAGERRENAME** | User renames file | FileManager | NewValues: `FolderId`, `OldName`, `NewName` (maps to `OriginFrom`/`OriginTo`) |
| **FILEMANAGERCREATEFOLDER** | User creates folder | FileManager | NewValues: `Name`, `Path`, `ParentFolderId` |
| **FILEMANAGERDELETEFOLDER** | User deletes folder | FileManager | Only empty folders can be deleted. NewValues: `FolderName`, `ParentFolderId` |
| **FILEMANAGEROUTOFSYNC** | Storage consistency check fails (DB vs disk) | FileManager | Logged as **Critical** severity. AdditionalInfo contains `FolderId`/`FileId` when available |

**Supported File Formats:**
- ✅ `.doc` - Microsoft Word 97-2003
- ✅ `.docx` - Microsoft Word 2007+
- ❌ `.pdf` - Use dedicated Export to PDF function

**Document Lifecycle:**
- User navigates to `/menu/word-viewer/documents` 
- Click **"New Document"** or **"From Template"** mode selector
- Upload file (new mode) or use template (template mode)
- On success: **CreateWordDocument** or **CreateWordDocumentFromTemplate** logged
- Document appears in list with metadata (creation date, creator, status)
- User can export to PDF (future: PDF export logs separately)
- User can delete: **DeleteWordDocument** logged, document marked deleted

### 🔵 UPDATE — Record Modifications
- All entities above support UPDATE operations
- Captures **before/after** values (OldValues / NewValues)
- Field-level changes tracked in JSON format
- Examples: Price changes, owner updates, status transitions (Active→Terminated)

### 🧾 CONTRACTCHANGESTATUS — Status Transitions (Dedicated)
- Logged when a rental contract **Status changes** (for example, user sets Status to `Terminated` and adjusts lifecycle dates).
- **Entity**: `RentalContract`
- **OldValues/NewValues** include:
  - `Status`
  - `NoticeDate` (Opzeg gedaan dd)
  - `MoveOutDate`
  - `EndDate` (Terminated)
  - `ClosedDate` (Complete)

### 🔴 DELETE — Record Removals
- All entities above support DELETE operations
- Preserves **complete record** in OldValues before deletion
- Enables full recovery and accountability
- Prevents deletion of records in use (foreign key constraints enforced)

### 🔵 LOGIN — User Authentication
| Action | Context |
|--------|---------|
| **Login Success** | User authentication via email/password |
| **Login Failed** | Failed login attempts (wrong password, account locked) |
| **2FA Challenge** | Two-factor authentication initiations |
| **2FA Enabled** | User enables 2FA security |
| **2FA Disabled** | User disables 2FA security |

### ⚫ LOGOUT — User Session Termination
| Trigger | Notes |
|---------|-------|
| **Manual Logout** | User clicks Logout button |
| **Circuit Close** | Browser closed, tab closed, auto-logout (30min timeout) |
| **Session Expiry** | Automatic logout after inactivity threshold |

---

## 5. 💰 PAYMENT SCHEDULE — Financial Operations
**Actions:**
- **RECALCPAYSCHEDULE** (🔷 yellow/orange badge)
- **APPLYRENTINDEX** (📈 yellow/orange badge)
- **CREATERENTPAYMENT** (follows CREATE badge logic → 🟢 green)
- **UPDATERENTPAYMENT** (follows UPDATE badge logic → 🔵 blue)
- **DELETERENTPAYMENT** (follows DELETE badge logic → 🔴 red)

| Action | Trigger | Entity | Notes |
|--------|---------|--------|-------|
| **RECALCPAYSCHEDULE** | System auto-calculates when loading payment schedules; occurs when: rent changes, contract dates shift, or indexation applied | PaymentSchedule | Generated automatically; updates existing payment transactions with new amounts due; may affect multiple payment periods |
| **APPLYRENTINDEX** | User applies rent indexation to contract | RentPayment | Recalculates all future payments with new rent amount; applies index factor to rent base |
| **CREATERENTPAYMENT** | User manually adds payment record | RentPayment | Manual entry of one-off payments, arrears, deposits |
| **UPDATERENTPAYMENT** | User modifies payment details | RentPayment | Changes to payment amounts, dates, status |
| **DELETERENTPAYMENT** | User removes payment record | RentPayment | Removal of incorrect entries; cascades to transactions |

**Payment Schedule Chain:**
- Contract created with monthly rent → PaymentSchedule generation starts
- RECALCPAYSCHEDULE logs whenever user navigates to payment page (periodic recalc)
- APPLYRENTINDEX triggers when user applies rent increase → all future payments recalculated
- Manual CREATERENTPAYMENT/UPDATERENTPAYMENT for special transactions (deposits, one-time charges)

---

## 6. 💾 WORKFLOW — Contract Workflow Events
**Actions:**
- **WKFLOWSAVE** (💾 blue badge `bg-info`) - Document prepared for saving
- **WKFLOWACCEPT** (✅ green badge `bg-success`) - Contract accepted
- **WKFLOWDECLINE** (🚫 red badge `bg-danger`) - Contract declined
- **WKFLOWFINISH** (✓ green badge `bg-success`) - Contract finalized

| Action | Trigger | Entity | Step | Notes |
|--------|---------|--------|------|-------|
| **WKFLOWSAVE** | User clicks "Save Contract + Preview" | RentalContract | 4 → 5 | Contract document prepared; audits transition from Review to Preview. No DB save yet. |
| **WKFLOWACCEPT** | User clicks "Accept & Sign" | RentalContract | 5 → 6 | Consumer accepts contract terms; moving to final signing step. |
| **WKFLOWDECLINE** | User clicks "Decline" | RentalContract | 5 → 4 | User rejects contract; returns to Review step (no DB save). Can modify and re-submit. |
| **WKFLOWFINISH** | User clicks "Close & Finish" | RentalContract | 6 | Contract created in DB with all parties and financial terms. Final step completes workflow. |

**Workflow Flow (6 Steps):**
- **Steps 1-3**: Form input (parties, location, financial) → no audit events
- **Step 4 (Review)**: Final review before preview → User clicks "Save Contract + Preview" → **`WKFLOWSAVE`** logged → auto-advance to Step 5
- **Step 5 (Preview)**: Visual contract preview in PDF format → User chooses:
  - **Accept** → **`WKFLOWACCEPT`** logged → auto-advance to Step 6
  - **Decline** → **`WKFLOWDECLINE`** logged → return to Step 4 (no save)
- **Step 6 (Signed)**: Final contract display (read-only) → User clicks "Close & Finish" → **`WKFLOWFINISH`** logged → Contract saved to DB with `ContractDocumentFileId` → Return to menu

**Key Points:**
- Contract is NOT saved to DB until Step 6 (after user accepts)
- Document file reference stored in `ContractDocumentFileId` column
- All 4 workflow actions are mutually exclusive in sequence (linear progression)
- Failed workflow leaves no DB record (only audit trail of attempts)

---

## 7. 🟣 REPORT — Export Operations
| Report | Formats | Entity Column |
|--------|---------|---------------|
| **Contracts Report** | CSV, PDF | `ContractsReport` |
| **Rent Payments Report** | CSV, PDF | `RentPaymentsReport` |
| **Rent Indexations Report** | CSV, PDF | `RentIndexationsReport` |
| **Meter Readings Report** | CSV, PDF | `MeterReadingsReport` |
| **Utility Consumption Report** | CSV, PDF | `UtilityConsumptionReport` |
| **Telecom Connections Report** | CSV, PDF | `TelecomConnectionsReport` |
| **Inspections Report** | CSV, PDF | `InspectionsReport` |
| **Expenses Report** | CSV, PDF | `ExpensesReport` |

**Export Details:**
- Format captured in NewValues: `{ "reportKey": "ContractsReport", "format": "pdf", "filters": {...} }`
- User, timestamp, IP address, and success status all tracked
- Enables audit of sensitive data exports (compliance & GDPR)

### ⚠️ ERROR Logging
- **All Failed Operations** — Exceptions captured with full stack trace
- **Access Denied** — Permission/authorization failures
- **Validation Errors** — Business rule violations
- **Data Integrity Issues** — Constraint violations, missing references

---

## 4. 📍 Page-to-Adapter Mapping

**How Audit Logging Works:**

Each Blazor page routes database mutations through a dedicated **adapter** (port implementation) which logs operations to the audit trail. This ensures a **single-layer audit** — one entry per operation, no duplicates.

### Phase 1 — Home (`/`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| Dashboard | AuthenticationAdapter | Login, Logout |

### Phase 2 — Lookup Tables (`/menu/lookup-tables`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| Unit Type | LookupAdapter | Create, Update, Delete Unit Type |
| Contact Role | LookupAdapter | Create, Update, Delete Contact Role |
| Utility Type | LookupAdapter | Create, Update, Delete Utility Type |
| Telecom Type | LookupAdapter | Create, Update, Delete Telecom Type |
| Expense Category | LookupAdapter | Create, Update, Delete Expense Category |
| Inspection Type | LookupAdapter | Create, Update, Delete Inspection Type |
| Health Index | HealthIndexValueAdapter | Create, Update, Delete Health Index |

### Phase 3 — Core (`/menu/core`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| Owner | OwnerAdapter | Create, Update, Delete Owner |
| Building | BuildingAdapter | Create, Update, Delete Building |
| Building Owner | BuildingOwnerAdapter | Create, Update, Delete Building Owner (relationships) |
| Contact | AuthenticationAdapter | Create, Update, Delete Contact (registration) |
| Unit | UnitAdapter | Create, Update, Delete Unit |
| Unit Fixed Cost | UnitFixedCostAdapter | Create, Update, Delete Unit Fixed Cost |

### Phase 4 — Rental (`/menu/rental`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| Rental Contract | RentalContractAdapter | Create, Update, Delete Rental Contract |
| Rental Contract Party | RentalContractPartyAdapter | Create, Update, Delete Party (Tenant/Guarantor/Owner association) |
| Rent Payment | RentPaymentAdapter | Create, Update, Delete Rent Payment |
| Rent Indexation | RentIndexationAdapter | Create, Update, Delete Rent Indexation; ApplyRentIndexation |
| Payment Schedule | PaymentScheduleAdapter | RECALCULATEPAYMENTSCHEDULE (automatic on view), Create/Update/Delete Payment Transactions |
| Contracts Report | IAuditService | Export Contract data (CSV/PDF) |
| Rent Payments Report | IAuditService | Export Payment data (CSV/PDF); includes recalculated payment schedule |
| Rent Indexations Report | IAuditService | Export Indexation data (CSV/PDF) |

### Phase 5 — Operational (`/menu/operational`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| Unit Utility Meter | UnitUtilityMeterAdapter | Create, Update, Delete Utility Meter |
| Utility Meter Reading | UtilityMeterReadingAdapter | Create, Update, Delete Meter Reading |
| Unit Telecom Connection | UnitTelecomConnectionAdapter | Create, Update, Delete Telecom Connection |
| Inspection | InspectionAdapter | Create, Update, Delete Inspection |
| Inspection Item | InspectionItemAdapter | Create, Update, Delete Inspection Item |
| Inspection Photo | InspectionPhotoAdapter | Create, Update, Delete Inspection Photo |
| Expense | ExpenseAdapter | Create, Update, Delete Expense |
| Meter Readings Report | IAuditService | Export Meter Readings (CSV/PDF) |
| Utility Consumption Report | IAuditService | Export Consumption data (CSV/PDF) |
| Telecom Connections Report | IAuditService | Export Telecom data (CSV/PDF) |
| Inspections Report | IAuditService | Export Inspection data (CSV/PDF) |
| Expenses Report | IAuditService | Export Expense data (CSV/PDF) |

### Phase 6 — Contract Details (`/menu/contract-details`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| Contract Details | RentalContractAdapter | Create, Update, Delete Rental Contract with status transitions |
| Tenant/Party Management | RentalContractPartyAdapter | Create, Update, Delete Contract Parties |
| Fixed Cost Management | UnitFixedCostAdapter | Create, Update, Delete Fixed Costs |
| Rent Payment Tracking | RentPaymentAdapter | Create, Update, Delete Rent Payments |
| Contract Details Report | IAuditService | Export Contract Details (CSV/PDF) |

### Phase 7 — Workflow (`/menu/workflow`)
| Page | Adapter/Service | Audited Actions |
|------|-----------------|-----------------|
| Workflow - New Contract | RentalContractAdapter | Create Rental Contract (workflow steps 1-4, then save in step 5) |
| Workflow - Contract Preview | RentalContractAdapter | `WKFLOWSAVE` (Step 4); `WKFLOWACCEPT` (Step 5 Accept); `WKFLOWDECLINE` (Step 5 Decline); `WKFLOWFINISH` (Step 6) |
| Parties in Workflow | RentalContractPartyAdapter | Create Contract Parties (added in step 1; only persisted on save/success) |
| Fixed Costs in Workflow | UnitFixedCostAdapter | Create, Update, Delete Fixed Costs (quick input in step 3; only persisted on save/success) |

**Workflow Outcome:**
- ✅ **Success Path:** Steps 1→2→3→4 (`WKFLOWSAVE`)→5 (`WKFLOWACCEPT`)→6 (`WKFLOWFINISH`) = Contract created with all parties & costs
- ❌ **Decline Path:** Steps 1→2→3→4 (`WKFLOWSAVE`)→5 (`WKFLOWDECLINE`)→4 = Workflow canceled, no contract saved

### Phase 8 — Users Management (`/menu/admin/users`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| User Management | AuthenticationAdapter | Create, Update, Delete User; Enable/Disable 2FA; Role assignments |

### Phase 9 — Audit Logs (`/audit-logs`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| Audit Logs View | None (read-only) | No mutations logged (display only) |

### Phase 10 — My Profile (`/signup`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| Profile | AuthenticationAdapter | Update Contact (profile changes); Update Password |

### Phase 11 — Word Viewer Documents (`/menu/word-viewer/documents`)
| Page | Adapter | Audited Actions |
|------|---------|-----------------|
| Document Management | WordViewerAdapter | **CreateWordDocument** (document uploaded directly), **CreateWordDocumentFromTemplate** (document created from template), **DeleteWordDocument** (document soft-deleted) |

**Action Details:**
- **CreateWordDocument**: Logged when user uploads a new .doc or .docx file directly
- **CreateWordDocumentFromTemplate**: Logged when user creates document from predefined template (template.docx)
- **DeleteWordDocument**: Logged when user deletes a document (soft delete - marked as deleted but retained for audit)

**Single-Layer Principle:**
- ✅ All mutations go through adapters
- ✅ Adapters call `IAuditService` post-SaveChangesAsync
- ✅ No duplicate page-level logging
- ✅ One audit entry per database operation

---

## 5. 📋 Details Modal

![Audit Details Modal](../Immo/wwwroot/images/tela_detalhe.png)

### Modal Contents

**Action Information:**
- **Timestamp**: Exact date/time when action occurred
- **Action**: Type with color-coded badge (CREATE/UPDATE/DELETE/LOGIN/LOGOUT/REPORT/WKFLOWSAVE/WKFLOWACCEPT/WKFLOWDECLINE/WKFLOWFINISH)
- **Entity**: Name of the table/entity affected
- **User**: Full user identification with ID (or "System" for automated processes)

**Status & Context:**
- **Status**: ✅ Success (green) or ❌ Failed (red) with error message if applicable
- **Severity**: Event level - Information, Warning, Error, Critical
- **Duration**: Execution time in milliseconds (performance tracking)
- **Entity ID**: Unique identifier of the record

**Technical Details:**
- **IP Address**: Origin of the request (useful for security audits)
- **User Agent**: Browser/application information (e.g., Chrome, Edge, API client)

**Data Changes (JSON format):**
- **Old Values**: Complete record state BEFORE the change
  - For DELETE operations: Shows all data that was deleted
  - For UPDATE operations: Shows previous values
  - For CREATE operations: Empty
- **New Values**: Complete record state AFTER the change
  - For CREATE operations: Shows all new data
  - For UPDATE operations: Shows modified values
  - For DELETE operations: Empty
  - For **REPORT** (`ReportExport`): **New Values** holds `reportKey`, `format`, and optional `filters` (period, ids, etc.)

**Perfect for:**
- Investigating what exactly changed in a record
- Recovering deleted data
- Understanding who made changes and when
- Tracking field-level modifications

---

## 6. 💡 Common Use Cases

### 1. Audit Deleted Records
**Scenario:** Manager needs to know who deleted an important contact  
**Steps:**
1. Open Audit Logs
2. Set filters: Action = Delete, Entity = Contact
3. Review results to find user, timestamp, and deleted data
4. Click 👁️ to see complete Old Values with all fields

**Result:** Full record recovery capability + accountability

### 2. Track Data Modifications
**Scenario:** Investigate changes made to building information  
**Steps:**
1. Filter by: Entity = Building, Date From/To = period
2. Review list of UPDATE actions
3. Open details modal
4. Compare Old Values vs New Values side-by-side

**Result:** Complete audit trail of what changed, by whom, and when

### 3. Security - Suspicious Login Activity
**Scenario:** Check for unauthorized access attempts  
**Steps:**
1. Filter: Action = Login, Date From = last night
2. Review failed login attempts (❌ status)
3. Check IP addresses and User Agent
4. Identify unusual patterns or locations

**Result:** Enhanced security monitoring and threat detection

### 4. Compliance Reporting
**Scenario:** Generate audit report for GDPR compliance  
**Steps:**
1. Filter by date range (e.g., last quarter)
2. Review all operations on sensitive data
3. Export logs or take screenshots
4. Document user actions for auditors

**Result:** Complete audit trail for regulatory compliance

---

---

## 7. 🔒 Security & Compliance

**Data Recorded:**
- ✅ Entity IDs and affected records
- ✅ Field-level changes (before/after)
- ✅ User identification and timestamps
- ✅ IP addresses and user agents
- ✅ Operation success/failure status

**Data Never Recorded:**
- ❌ Passwords (even hashed)
- ❌ Authentication tokens
- ❌ API keys
- ❌ Credit card information
- ❌ Sensitive personal data (medical records, etc)

**Compliance Standards:**
- ✅ **GDPR** (General Data Protection Regulation - EU)
- ✅ **SOX** (Sarbanes-Oxley Act - USA)
- ✅ **ISO 27001** (Information Security Management)

**Traceability:**
- **Who?** → UserId, UserName
- **What?** → Action, EntityName, EntityId, Old/New Values
- **When?** → Timestamp (UTC)
- **Where?** → IpAddress, UserAgent
- **Result?** → Success/Failed with error details

---

## 8. 📋 Complete Actions Reference

**All possible Action values (organized by category):**

### Authentication (4 actions)
- `Login` - User login successful
- `Logout` - User session terminated
- `Enable2FA` - Two-factor authentication enabled
- `Disable2FA` - Two-factor authentication disabled

### CRUD Operations (3 base actions + 70+ entity variants)
- `Create` - Generic create operation
- `Update` - Generic update operation
- `Delete` - Generic delete operation

**Entity-specific CRUD:** `Create{Entity}`, `Update{Entity}`, `Delete{Entity}` pattern applied to:
- Building, Contact, Owner, Unit
- Expense, Inspection, Rental Contract, etc.
- 70+ total CRUD action variants

### Workflow (4 actions)
- `WKFLOWSAVE` - Contract document prepared (Step 4)
- `WKFLOWACCEPT` - Contract accepted by user (Step 5)
- `WKFLOWDECLINE` - Workflow rejected by user (Step 5, returns to review)
- `WKFLOWFINISH` - Workflow completed, contract created in DB (Step 6)

### Contracts (1 action)
- `CONTRACTCHANGESTATUS` - Contract change status

### Contract Details (consultation)
- `CONTRACTDETAILSVIEW` - Contract details - view
- `CONTRACTDETAILSMANAGEPAYMENTS` - Contract details - manage payments
- `CONTRACTDETAILSMANAGEPARTIES` - Contract details - manage parties

### Payment Management (5 actions)
- `RECALCULATEPAYMENTSCHEDULE` - Automatic payment schedule recalculation
- `CREATERENTPAYMENT` - Manual rent payment entry
- `UPDATERENTPAYMENT` - Rent payment modification
- `DELETERENTPAYMENT` - Rent payment removal
- `ApplyRentIndexation` - Rent indexation applied to payments

### Reports & Exports (2 actions)
- `Report` - Generic report export
- `ReportExport` - Detailed report export with format (CSV/PDF) tracking

### Admin / Configuration (1 action)
- `LabelOverride` - UI labels override saved via `/admin/ui-labels` (badge: `LABELOVERRIDE`)

### Data Access & Analytics (3 actions)
- `DataAccess.Export` - Data export operation
- `DataAccess.Query` - Data query operation
- `DataAccess.View` - Data view/filter operation

**Total: 90+ unique actions tracked in audit log**

---

## �📊 Technical Details

**Automatic Logging:** 30+ pages/components automatically record all operations  
**Total Actions Tracked:** 90+ unique action types  
**Storage:** SQL Server with optimized indexes for fast queries  
**Performance:** Indexed by Timestamp, UserId, Action, EntityName, Success  
**Retention:** Configurable (recommended: 90 days for Info, 6 months for Warnings, 1 year for Critical)

---

## 9. 🔍 Useful Queries

### Logs of a specific user
```csharp
var userLogs = await dbContext.AuditLogs
    .Where(a => a.UserId == userId)
    .OrderByDescending(a => a.Timestamp)
    .Take(100)
    .ToListAsync();
```

### Modification history of a record
```csharp
var history = await dbContext.AuditLogs
    .Where(a => a.EntityName == "Contact" && a.EntityId == "5")
    .OrderBy(a => a.Timestamp)
    .ToListAsync();
```

### Failed logins (last 24 hours)
```csharp
var failed = await dbContext.AuditLogs
    .Where(a => a.Action == "Login" && !a.Success 
        && a.Timestamp >= DateTime.UtcNow.AddDays(-1))
    .ToListAsync();
```

### Critical events
```csharp
var critical = await dbContext.AuditLogs
    .Where(a => a.Severity == "Critical" || a.Severity == "Error")
    .OrderByDescending(a => a.Timestamp)
    .Take(50)
    .ToListAsync();
```

### All deletions in a period
```csharp
var deletes = await dbContext.AuditLogs
    .Where(a => a.Action == "Delete" 
        && a.Timestamp >= startDate 
        && a.Timestamp <= endDate)
    .ToListAsync();
```

### Modifications to a specific field
```csharp
var fieldChanges = await dbContext.AuditLogs
    .Where(a => a.EntityName == "Contact" 
        && a.EntityId == contactId.ToString()
        && a.Action == "Update"
        && a.OldValues.Contains("\"email\""))
    .OrderBy(a => a.Timestamp)
    .ToListAsync();
```
---

## 10. 📝 Compliance

Meets requirements for:
- ✅ **GDPR** - General Data Protection Regulation (EU)
- ✅ **SOX** - Sarbanes-Oxley Act (USA)
- ✅ **ISO 27001** - Information Security

Complete traceability:
- **Who?** → UserId, UserName
- **What?** → Action, EntityName, EntityId
- **When?** → Timestamp
- **Where?** → IpAddress, UserAgent
- **How changed?** → OldValues, NewValues
- **Result?** → Success, ErrorMessage

---

## 11. 💾 Retention

| Type | Period |
|------|--------|
| Critical/Error | 1 year or more |
| Warning | 6 months |
| Information | 90 days |

---

## 12. ✨ Benefits

- ✅ **Complete audit trail in database** - All operations tracked automatically
- ✅ **GDPR compliance** - Meets regulatory data protection requirements
- ✅ **Track who changed what and when** - Full operation traceability
- ✅ **Business intelligence data** - Analyze patterns and trends
- ✅ **Security & compliance monitoring** - Detect suspicious activities

### ⚠️ Important Rule

**DO NOT use ILogger** - All logging goes through **IAuditService** to maintain audit trail consistency and ensure all operations are properly tracked in the database.

---

## Documentation

- 🏠 [Main Documentation](../README.md) - Project overview and structure
- 📋 [Audit Log Implementation Report](AUDIT_LOG_IMPLEMENTATION_REPORT.md) - Complete audit log inventory with code locations
- 📋 [Audit Single-Layer Roadmap](AUDIT_SINGLE_LAYER_ROADMAP.md) - Menus, screens, adapter-centric CUD checklist

---

**© 2026 AdminSense. All rights reserved.**
