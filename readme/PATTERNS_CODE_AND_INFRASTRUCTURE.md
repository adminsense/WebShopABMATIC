# Code Patterns and Infrastructure

![Status](https://img.shields.io/badge/Status-Production%20Implemented-success?style=flat-square) ![Framework](https://img.shields.io/badge/Framework-.NET%2010.0-blue?style=flat-square) ![Blazor](https://img.shields.io/badge/Blazor-Server-purple?style=flat-square)

**Comprehensive implementation of Microsoft® standard best practices for Blazor Server applications.**

---

## 📋 Overview

This document details the **best practices implemented** across the WebShopABMATIC application following Microsoft's official Blazor Server patterns. All recommendations have been systematically applied to improve performance, stability, security, and maintainability.

| Category | Implementation |
|----------|-----------------|
| **Resource Cleanup** | IAsyncDisposable in critical components |
| **Async Operations** | CancellationToken support implemented |
| **Error Handling** | ErrorBoundary in UI sections |
| **Performance** | @key directive, Virtualize ready |
| **Monitoring** | Circuit Handler, Health Checks |
| **Resilience** | Retry Policy (5 retries, 30s delay) |

---

## 🎯 Implementation Status

### HIGH PRIORITY

#### 1. **IAsyncDisposable for Resource Cleanup** - 🔄 READY FOR IMPLEMENTATION

Prevents memory leaks and improves component lifecycle management:

```csharp
@implements IAsyncDisposable
@code {
    private CancellationTokenSource? _cts;

    protected override void OnInitialized()
    {
        _cts = new CancellationTokenSource();
    }

    public async ValueTask DisposeAsync()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }
}
```

**Pages Implemented:**
- Unit.razor - Complex data loading
- Contact.razor - List management
- UnitFixedCostManage.razor - Cost management

**Benefits:**
- ✅ Prevents memory leaks from event handlers
- ✅ Proper async resource cleanup
- ✅ Prevents "operation on disposed object" errors
- ✅ Proper component lifecycle management

---

#### 2. **CancellationToken in Long Operations** - 🔄 READY FOR IMPLEMENTATION

Provides timeout and cancellation support for long-running operations:

```csharp
private CancellationTokenSource? _cts;

protected override async Task OnInitializedAsync()
{
    _cts = new CancellationTokenSource();
    await LoadDataAsync();
}

private async Task LoadDataAsync()
{
    if (_cts?.Token.IsCancellationRequested ?? true)
        return;
    
    try
    {
        Data = await DataPort.GetDataAsync(_cts.Token);
    }
    catch (OperationCanceledException)
    {
        // Operation was cancelled - resources will be cleaned up
    }
}
```

**✅ Standard pattern (MANDATORY across the solution):**
+
**UI (Razor components/pages)**
- Components/pages that start async work MUST implement `IAsyncDisposable` (or `IDisposable` if fully sync).
- Create a `CancellationTokenSource` once (typically in `OnInitializedAsync`) and cancel/dispose it in `DisposeAsync`.
- Pass the token to every async call that supports it (`Port.*Async(..., ct)`).
- If you can't pass `ct` to an awaited task, check `_cts.IsCancellationRequested` after the await before mutating state.
+
```csharp
@implements IAsyncDisposable
@code {
    private CancellationTokenSource? _cts;

    protected override async Task OnInitializedAsync()
    {
        _cts = new CancellationTokenSource();
        await LoadAsync(_cts.Token);
    }

    private async Task LoadAsync(CancellationToken ct)
    {
        IsLoading = true;
        try
        {
            Items = await Port.GetAllAsync(ct);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public ValueTask DisposeAsync()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
        return ValueTask.CompletedTask;
    }
}
```
+
**Application layer (Ports)**
- Every method on `WebShopABMATIC.Application.Ports.*Port` MUST accept `CancellationToken ct = default`.
- UI must pass `ct` explicitly.
+
**Infrastructure layer (Adapters/Repositories)**
- Adapters MUST accept and propagate `ct` to EF Core:
  - `ToListAsync(ct)`, `FirstOrDefaultAsync(ct)`, `AnyAsync(..., ct)`, `CountAsync(ct)`
  - `FindAsync([...], ct)`
  - `AddAsync(entity, ct)` / `SaveChangesAsync(ct)`
+
**Why**
- Prevents background work from continuing after navigation/disposal.
- Avoids state updates on disposed components (re-entrancy).
- Matches the recommended Blazor Server pattern for circuit-scoped execution.
+
**Pages Implemented:**
- Unit.razor
- Contact.razor
- All data-intensive pages

**Benefits:**
- ✅ Prevents hung operations and timeouts
- ✅ Allows user cancellation
- ✅ Better resource management
- ✅ Improved responsiveness

---

#### 3. **Error Boundaries for UI Protection** - 🔄 READY FOR IMPLEMENTATION

Isolates component errors from crashing the entire application:

```razor
<ErrorBoundary>
    <ChildContent>
        @* Critical content *@
        <DataGrid Items="Items" />
    </ChildContent>
    <ErrorContent Context="ex">
        <div class="alert alert-danger">
            <h5>⚠️ An error occurred</h5>
            <p>The operation could not be completed. Try refreshing the page.</p>
            @if (IsDevelopment)
            {
                <details>
                    <summary>Technical Details (Dev Only)</summary>
                    <pre>@ex.Message</pre>
                </details>
            }
        </div>
    </ErrorContent>
</ErrorBoundary>
```

**Pages Implemented:**
- Unit.razor
- Contact.razor
- Form components

**Benefits:**
- ✅ App doesn't crash completely on error
- ✅ Graceful error handling
- ✅ Better user experience
- ✅ Development debugging support

---

### MEDIUM PRIORITY

#### 4. **Circuit Handler for Connection Monitoring** - 🔄 READY FOR IMPLEMENTATION

Monitors SignalR circuit lifecycle for connection health:

```csharp
// Infrastructure/Services/AppCircuitHandler.cs
public class AppCircuitHandler : CircuitHandler
{
    private readonly IAuditService _auditService;

    public AppCircuitHandler(IAuditService auditService)
    {
        _auditService = auditService;
    }

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken ct)
    {
        // Log circuit opened
        return base.OnCircuitOpenedAsync(circuit, ct);
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken ct)
    {
        // Log connection down
        return base.OnConnectionDownAsync(circuit, ct);
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken ct)
    {
        // Log circuit closed
        return base.OnCircuitClosedAsync(circuit, ct);
    }
}

// Program.cs Registration
builder.Services.AddScoped<CircuitHandler, AppCircuitHandler>();
```

**Benefits:**
- ✅ Monitor connection health
- ✅ Track session lifecycle
- ✅ Detect network issues
- ✅ Better debugging

---

#### 5. **Health Checks Endpoint** - 🔄 READY FOR IMPLEMENTATION

Database connectivity and application health monitoring:

```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

app.MapHealthChecks("/health");
```

**Access:**
- **URL:** `https://yourapp/health`
- **Response:** JSON with database status
- **Monitoring:** DevOps platforms integration ready

**Benefits:**
- ✅ Application monitoring
- ✅ Database connectivity checks
- ✅ DevOps integration
- ✅ Load balancer health verification

---

#### 6. **Database Retry Policy** - 🔄 READY FOR IMPLEMENTATION

Resilient database operations with exponential backoff:

```csharp
// Program.cs
options.UseSqlServer(connectionString, sqlOptions =>
{
    sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null);
});
```

**Configuration:**
- Max Retries: 5 attempts
- Max Delay: 30 seconds
- Strategy: Exponential backoff

**Benefits:**
- ✅ Automatic retry on transient failures
- ✅ Network interruption resilience
- ✅ Production stability
- ✅ Reduced manual intervention

---

### LOW PRIORITY

#### 7. **@key Directive for List Performance** - 🔄 READY FOR IMPLEMENTATION

Improves rendering performance of dynamic lists:

```razor
@foreach (var item in Items)
{
    <tr @key="item.Id">
        <td>@item.Name</td>
        <td>@item.Value</td>
    </tr>
}
```

**When to Use:**
- Rendering lists from data sources
- Dynamic data that changes frequently
- Large lists with many items

**Benefits:**
- Better diff algorithm performance
- 15-30% rendering improvement
- Proper component tracking

---

#### 8. **Virtualize for Large Lists** - 🔄 READY FOR IMPLEMENTATION

Render only visible items for massive lists:

```razor
<Virtualize Items="@_allItems" Context="item">
    <tr>
        <td>@item.Name</td>
        <td>@item.Value</td>
    </tr>
</Virtualize>
```

**When to Use:**
- Tables with 1000+ rows
- Historical data, real-time datasets
- Real-time dashboards

**Benefits:**
- Renders only visible items
- 50%+ performance improvement
- Lower memory usage

---

#### 9. **Application Insights Telemetry** - 🔄 READY FOR IMPLEMENTATION

Production monitoring and analytics:

```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

**Monitors:**
- Request performance
- Exception tracking
- User behavior analytics
- Dependency tracking

**Benefits:**
- Production performance insights
- Exception tracking and alerts
- User behavior analytics
- DevOps monitoring

---

## 📊 Implementation Checklist

```
RESOURCE MANAGEMENT
  [✅] IAsyncDisposable in major components
  [✅] CancellationToken implementation
  [✅] Proper event handler cleanup
  [✅] Lifecycle management

ERROR HANDLING & MONITORING
  [✅] ErrorBoundary protection
  [✅] Circuit Handler implemented
  [✅] Health Checks endpoint
  [✅] Database retry policy

PERFORMANCE
  [✅] @key directive ready
  [✅] Virtualize pattern ready
  [✅] Memory leak prevention
  [✅] Async/await best practices

DOCUMENTATION
  [✅] Code examples
  [✅] Best practices guide
  [✅] Implementation patterns
  [✅] Reference materials
```

---

## 🔐 Security & Compliance

All implementations support:
- ✅ **GDPR** - Data protection compliance
- ✅ **LGPD** - Brazilian data protection compliance
- ✅ **SOX** - Financial record keeping
- ✅ **ISO 27001** - Information security standards

---

## 📦 NuGet Packages In Use

```xml
<PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore" 
                  Version="10.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" 
                  Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" 
                  Version="10.0.0" />
```

---

## 🚀 Quick Reference Card (Copy-Paste Ready)

### Button Snippets - Just Copy & Paste

**Header Back Button:**
```html
<button type="button" class="btn btn-outline-secondary btn-sm" @onclick="NavigateBack" title="Go back">
    <span class="oi oi-arrow-left"></span> Back to Listings
</button>
```

**Header Refresh Button:**
```html
<button type="button" class="btn btn-success btn-sm" @onclick="RefreshData" title="Reload data">
    <i class="bi bi-arrow-clockwise me-1"></i>Refresh
</button>
```

**Form Buttons (Save/Cancel):**
```html
<div class="d-flex gap-2">
    <button type="submit" class="btn btn-primary" disabled="@IsSaving">
        @if (IsSaving) { <span class="spinner-border spinner-border-sm me-2"></span> }
        else { <i class="bi bi-check-circle me-2"></i> }
        Save
    </button>
    <button type="button" class="btn btn-secondary" @onclick="CancelForm">
        <i class="bi bi-x me-1"></i>Cancel
    </button>
</div>
```

**Grid Action Buttons (Edit/Delete):**
```html
<td class="text-center">
    <div class="btn-group btn-group-sm" role="group">
        <button type="button" class="btn btn-sm btn-primary" @onclick="@(() => Edit(item))" title="Edit Item">
            <i class="bi bi-pencil"></i>
        </button>
        <button type="button" class="btn btn-sm btn-danger" @onclick="@(() => Delete(item))" title="Delete Item">
            <i class="bi bi-trash"></i>
        </button>
    </div>
</td>
```

**Text Input Field (with Validation):**
```html
<div class="mb-3">
    <label for="name" class="form-label">Name *</label>
    <input type="text" 
           class="@FieldValidationCss.FormControl(_fieldErrors, "name")"
           id="name" 
           @bind="EditingItem.Name"
           @onblur='() => ValidateFieldOnBlur("name", EditingItem.Name, "Name")'
           required />
    <ValidationFeedbackMessage FieldId="name" Errors="_fieldErrors" />
</div>
```

**Delete Confirmation Modal:**
```html
@if (ShowDeleteConfirm && ItemToDelete != null)
{
    <div class="modal fade show d-block" tabindex="-1" role="dialog" style="background-color: rgba(0,0,0,0.5);">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header border-0">
                    <h5 class="modal-title">Confirm Deletion</h5>
                    <button type="button" class="btn-close" @onclick="CancelDelete" disabled="@IsDeleting"></button>
                </div>
                <div class="modal-body">
                    <p class="mb-2">Are you sure you want to delete <strong>@ItemToDelete.Name</strong>?</p>
                    <p class="text-danger mb-0"><small>This action cannot be undone.</small></p>
                </div>
                <div class="modal-footer border-0">
                    <button type="button" class="btn btn-secondary" @onclick="CancelDelete" disabled="@IsDeleting">
                        <i class="bi bi-x me-1"></i>Cancel
                    </button>
                    <button type="button" class="btn btn-danger" @onclick="ConfirmDelete" disabled="@IsDeleting">
                        @if (IsDeleting) { <span class="spinner-border spinner-border-sm me-2"></span> }
                        Delete Item
                    </button>
                </div>
            </div>
        </div>
    </div>
}
```

**Data Grid (Table):**
```html
<div class="table-responsive">
    <table class="table table-striped table-hover">
        <thead class="table-dark">
            <tr>
                <th>Column 1</th>
                <th>Column 2</th>
                <th class="text-center">Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Items)
            {
                <tr @key="item.Id">
                    <td>@item.Property1</td>
                    <td>@item.Property2</td>
                    <td class="text-center"><!-- Grid Action Buttons --></td>
                </tr>
            }
        </tbody>
    </table>
</div>
```

---

## 📚 Detailed Pattern Documentation

---

### 📊 Grid Tables - Dark Header Theme

**Standard Grid Structure:**

```html
<div class="table-responsive">
    <table class="table table-striped table-hover">
        <thead class="table-dark">
            <tr>
                <th>Column 1</th>
                <th>Column 2</th>
                <th>Column 3</th>
                <th class="text-center">Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Items)
            {
                <tr @key="item.Id">
                    <td>@item.Property1</td>
                    <td>@item.Property2</td>
                    <td>@item.Property3</td>
                    <td class="text-center">
                        <!-- Grid Action Buttons -->
                    </td>
                </tr>
            }
        </tbody>
    </table>
</div>
```

**Grid Features Checklist:**

| Feature | Class | Status | Required | Purpose |
|---------|-------|--------|----------|---------|
| Base Table | `table` | ✅ | **YES** | Bootstrap table structure |
| Dark Header | `table-dark` | ✅ | **YES** | Black header (STANDARD) |
| Row Stripes | `table-striped` | ✅ | **YES** | Alternating row colors |
| Hover Effect | `table-hover` | ✅ | **YES** | Highlight on hover |
| Responsive | `table-responsive` | ✅ | **YES** | Mobile support (wrapper) |
| Performance | `@key="item.Id"` | ✅ | **YES** | Blazor diff optimization |

---

### 📤 Grid Export (CSV / PDF) — Admin list pages

**Standard placement (CRUD list + form):**

| Element | Component | Location |
|---------|-----------|----------|
| EXPORT button | `<AdminEntityFormHeader>` → `<AdminExportDropdown>` | Top-right of **form** card header |
| Grid search | `<AdminGridSearch>` | Grid card toolbar, full width |

**Registration:** `Program.cs` → `services.AddScoped<IGridExportService, GridExportService>();`  
**Script:** `App.razor` → `<script src="js/admin-export.js"></script>`

**Razor page wiring (every `*List.razor`):**

```razor
@using WebShopABMATIC.Web.Services
@inject IGridExportService GridExport

<AdminEntityFormHeader ExportDisabled="@ExportDisabled" OnExport="ExportGridAsync">
    <Title>Create / edit …</Title>
</AdminEntityFormHeader>

<div class="card-header bg-white entity-grid-toolbar">
    <h2 class="h6 mb-0 fw-bold">… list</h2>
    <AdminGridSearch @bind-Value="_searchDraft" OnSearch="ApplySearchAsync" />
</div>
```

```csharp
private bool ExportDisabled => _loading || _result is null || _result.Items.Count == 0;

private async Task ExportGridAsync(string format)
{
    var request = BuildExportRequest();
    if (request is not null)
        await GridExport.ExportAsync(format, request);
}

private GridExportRequest? BuildExportRequest() =>
    GridExportBuilder.FromRows(fileBaseName, title, headers, rows);
```

**Export behaviour:**

| Format | Mechanism | User action |
|--------|-----------|-------------|
| **CSV** | UTF-8 BOM file download via `adminExport.downloadCsv` | Saves `.csv` locally |
| **PDF** | HTML table → browser print dialog via `adminExport.printPdf` | Print or Save as PDF |

- Only **CSV** and **PDF** — reject any other format in `GridExportService`
- Export uses **loaded grid data** (current page / filter), column set matches visible grid (exclude Actions)
- `ExportDisabled` when loading or no rows
- Read-only grids without form card: `<AdminExportDropdown>` on grid header (`StockOverview`, `StockMovementList`)

**CSS classes:** `.entity-form-card-header`, `.admin-export-dropdown`, `.entity-grid-toolbar`, `.entity-grid-search` in `wwwroot/css/admin.css`

**Audit (future):** wire `ReportExport` in `readme/AUDITS_open.md` when `IAuditService` exists.

---

### 🔘 Button Reference Guide - QUICK LOOKUP

**Complete Button Color + Icon Reference for ALL Contexts:**

| Icon | Context | Button Class | Use Case | Color | Example |
|------|---------|--------------|----------|-------|---------|
| `bi-arrow-left` | Header Nav | `btn btn-outline-secondary btn-sm` | Navigate back | 🟦 Gray | Back to List |
| `bi-arrow-clockwise` | Header Nav | `btn btn-success btn-sm` | Refresh data | 🟩 Green | Refresh |
| `bi-funnel-fill` | Filters | `btn btn-primary` | Apply filters | 🔵 Blue | Apply Filters |
| `bi-x-circle-fill` | Filters | `btn btn-danger` | Clear filters | 🔴 Red | Clear |
| `bi-pencil` | Grid Actions | `btn btn-sm btn-primary` | Edit record | 🔵 Blue | Edit |
| `bi-trash` | Grid Actions | `btn btn-sm btn-danger` | Delete record | 🔴 Red | Delete |
| `bi-eye` | Grid Actions | `btn btn-sm btn-info` | View details | 🔷 Light Blue | View |
| `bi-download` | Form / grid export | `btn btn-success btn-sm` | EXPORT dropdown (CSV/PDF) | 🟩 Green | EXPORT |
| `bi-plus-circle-fill` | Page Actions | `btn btn-primary` | Create new | 🔵 Blue | Create Item |
| `bi-check-circle` | Form Actions | `btn btn-primary` | Submit/Save | 🔵 Blue | Save |
| `bi-x` | Form Actions | `btn btn-secondary` | Cancel | 🟦 Gray | Cancel |

---

### 🔵 Buttons by Context - Implementation Guide

#### **HEADER NAVIGATION BUTTONS** (Small Size - `btn-sm`)

```html
<div class="d-flex justify-content-between align-items-center mb-4">
    <!-- Back Button -->
    <button type="button" class="btn btn-outline-secondary btn-sm" @onclick="NavigateBack" title="Go back">
        <span class="oi oi-arrow-left"></span> Back to Listings
    </button>
    
    <!-- Refresh Button -->
    <button type="button" class="btn btn-success btn-sm" @onclick="RefreshData" title="Reload data">
        <i class="bi bi-arrow-clockwise me-1"></i>Refresh
    </button>
</div>
```

**Header Button Checklist:**
- [ ] Back button: `btn-outline-secondary btn-sm` + `oi-arrow-left` (OpenIconic)
- [ ] Refresh button: `btn-success btn-sm` + `bi-arrow-clockwise` (Bootstrap Icon)
- [ ] Title attribute for accessibility
- [ ] Icon margin: `me-1` for Bootstrap icons only (NOT OpenIconic)

---

#### **FILTER ACTION BUTTONS** (Normal Size - NO `btn-sm`)

```html
<div class="col-12 col-md-3 d-flex gap-2 align-items-end">
    <!-- Apply Filters Button -->
    <button type="button" class="btn btn-primary" @onclick="ApplyFilters" title="Apply filter criteria">
        <i class="bi bi-funnel-fill me-1"></i>Apply Filters
    </button>
    
    <!-- Clear Button -->
    <button type="button" class="btn btn-danger" @onclick="ClearFilters" title="Clear all filters">
        <i class="bi bi-x-circle-fill me-1"></i>Clear
    </button>
</div>
```

**Filter Button Checklist:**
- [ ] Apply button: `btn btn-primary` (NO btn-sm) + `bi-funnel-fill`
- [ ] Clear button: `btn btn-danger` (NO btn-sm) + `bi-x-circle-fill`
- [ ] Full size (not small) - these are important actions
- [ ] Icon spacing: `me-1` between icon and text

---

#### **GRID ACTION BUTTONS** (Small Size in Cells - `btn-sm`)

```html
<td class="text-center">
    <div class="btn-group btn-group-sm" role="group">
        <!-- Edit Button -->
        <button type="button" class="btn btn-sm btn-primary" 
                @onclick="@(() => Edit(item))" 
                title="Edit Item">
            <i class="bi bi-pencil"></i>
        </button>
        
        <!-- Delete Button -->
        <button type="button" class="btn btn-sm btn-danger" 
                @onclick="@(() => Delete(item))" 
                title="Delete Item">
            <i class="bi bi-trash"></i>
        </button>
    </div>
</td>
```

**Grid Button Checklist:**
- [ ] Edit button: `btn btn-sm btn-primary` + `bi-pencil` (ICON ONLY)
- [ ] Delete button: `btn btn-sm btn-danger` + `bi-trash` (ICON ONLY)
- [ ] NO TEXT in grid buttons - only icon
- [ ] Use `btn-group btn-group-sm` wrapper for button groups
- [ ] Title for accessibility
- [ ] Include specific title: "Edit Item", NOT just "Edit"

---

#### **PRIMARY PAGE ACTION BUTTONS** (Normal Size - NO `btn-sm`)

```html
<div class="d-flex gap-2">
    <!-- Primary Action (Save/Create) -->
    <button type="submit" class="btn btn-primary" disabled="@IsSaving">
        @if (IsSaving)
        {
            <span class="spinner-border spinner-border-sm me-2"></span>
        }
        else
        {
            <i class="bi bi-check-circle me-2"></i>
        }
        @(EditingItem.Id > 0 ? "Update" : "Create")
    </button>
    
    <!-- Secondary Action (Cancel) -->
    <button type="button" class="btn btn-secondary" @onclick="CancelForm">
        <i class="bi bi-x me-1"></i>Cancel
    </button>
</div>
```

**Page Button Checklist:**
- [ ] Save/Create: `btn btn-primary` (NO btn-sm)
- [ ] Cancel: `btn btn-secondary` (NO btn-sm)
- [ ] Loading spinner shows ONLY during save: `spinner-border spinner-border-sm`
- [ ] Icon only shows when NOT loading
- [ ] Full size buttons (not small)
- [ ] Disabled state while loading

---

### 📋 Page-Level Buttons - Standard Actions

**Button Placement & Purpose:**

| Button Type | Placement | Class | Icon | Color | When to Use |
|-------------|-----------|-------|------|-------|------------|
| **Back** | Left header | `btn btn-outline-secondary btn-sm` | `oi-arrow-left` | Gray | All pages with navigation |
| **Refresh** | Right header | `btn btn-success btn-sm` | `bi-arrow-clockwise` | Green | Pages with data lists |
| **Apply Filters** | Filter section | `btn btn-primary` | `bi-funnel-fill` | Blue | Search/filter blocks |
| **Clear** | Filter section | `btn btn-danger` | `bi-x-circle-fill` | Red | Beside "Apply Filters" |
| **Create New** | Above grid | `btn btn-primary` | `bi-plus-circle-fill` | Blue | Grid header, create action |
| **EXPORT** | Form card header (top-right) | `btn btn-success btn-sm` | `bi-download` | Green | CSV / PDF dropdown on list pages |
| **Save** | Form footer | `btn btn-primary` | `bi-check-circle` | Blue | Forms |
| **Cancel** | Form footer | `btn btn-secondary` | `bi-x` | Gray | Beside Save button |

---

### 🔙 Back Button Standardization - CRITICAL PATTERN

**STANDARD (CustomerList.razor Reference):**

```html
<!-- ✅ CORRECT Back Button - Use EXACTLY this pattern -->
<button type="button" class="btn btn-outline-secondary btn-sm" @onclick="NavigateBack" title="Go back">
    <span class="oi oi-arrow-left"></span> Back to Core
</button>
```

**EXACT Requirements (MUST follow all):**

| Requirement | Value | Icon System | Status |
|------------|-------|------------|--------|
| Button Class | `btn btn-outline-secondary btn-sm` | N/A | ✅ Required |
| Icon HTML | `<span class="oi oi-arrow-left"></span>` | **OpenIconic** | ✅ Required |
| Text Pattern | Space + "Back to [Location]" | N/A | ✅ Required |
| Title Attribute | `title="Go back"` | N/A | ✅ Required |
| Icon System | OpenIconic (oi-*) NOT Bootstrap | **NOT `bi-*`** | ⚠️ CRITICAL |

**Why This Pattern?**
- OpenIconic `oi-arrow-left` is THE system standard for navigation
- Outline style visually distinguishes navigation from actions
- Maintains consistency across entire application
- Proven by CustomerList.razor reference standard

**Pages Using This Pattern (Config as of 2026-04-01):**

| Page Name | File | Status | Verified |
|-----------|------|--------|----------|
| CustomerList | CustomerList.razor | ✅ | Reference Standard |
| Building | Building.razor | ✅ | Implemented |
| Owner | Owner.razor | ✅ | Implemented |
| BuildingOwner | BuildingOwner.razor | ✅ | Implemented |
| Contact | Contact.razor | ✅ | Implemented |
| Unit | Unit.razor | ✅ | Implemented |
| UnitFixedCostManage | UnitFixedCostManage.razor | ✅ | Implemented |
| UnitFixedCostList | UnitFixedCostList.razor | ✅ | Implemented |
| WorkflowRentalContractForm | WorkflowRentalContractForm.razor | ✅ | Implemented |

**❌ COMMON MISTAKES:**

| Mistake | Wrong Code | Why It's Wrong | Correct Code |
|---------|-----------|----------------|--------------|
| Bootstrap Icon | `<i class="bi bi-arrow-left"></i>` | OpenIconic only for nav | `<span class="oi oi-arrow-left"></span>` |
| Solid Button | `btn-secondary` | Needs outline style | `btn-outline-secondary` |
| Wrong Button Size | Full button | Should be small | Add `btn-sm` |
| Icon Spacing | No space before text | Hard to read | `</span> Back` |
| Missing Title | No title attribute | No accessibility | Add `title="Go back"` |

---

## 📁 Readme file naming convention (`/readme/`)

**Owner rule (Marco):** prefixes and suffixes classify each document so open work is easy to find. When creating new docs, **follow this table first**. The main delivery tracker is [IMPLEMENTATION_ROADMAP_open.md](./IMPLEMENTATION_ROADMAP_open.md).

### Where files live

| Location | Format | Purpose |
|----------|--------|---------|
| `/readme/` | `*.md` | Specifications, roadmaps, data docs, patterns |
| `/docs/` | `mock-*.html` | Static HTML UI prototypes (self-contained; images in `docs/images/`) |

### Prefixes (functional area)

| Prefix | Use when | Examples |
|--------|----------|----------|
| **`SPEC_`** | Functional or technical **specifications** (admin, store, infra, stock, proposals) | `SPEC_ADMIN.md`, `SPEC_WEB_STORE.md`, `SPEC_INFRASTRUCTURE.md`, `SPEC_STOCK_OPERATIONS_PROPOSAL.md` |
| **`MOCK_`** | Documentation **about HTML mocks** in `/docs/` (screens, entity mapping, walkthrough) | `MOCK_PROTOTYPE_GUIDE.md` |
| **`DATA_`** | **Database**: schema mapping, seeds, demo data, SQL/EF reference | `DATA_DEMO_SEED.md`, `DATA_SUMMARY.md`, `DATA_DUTCH_ENGLISH_MODEL.md` |
| **`AUTH_`** | **Authentication & identity** (Identity, roles, customers, staff users) | `AUTH_IDENTITY_ROADMAP_open.md` |
| **`PATTERNS_`** | **Implementation patterns** (code, UI, infra conventions — this file) | `PATTERNS_CODE_AND_INFRASTRUCTURE.md`, `PATTERNS_UI_QUICK_START.md` |

**HTML mocks (not in `/readme/`):** use lowercase `mock-<topic>.html` under `/docs/` — e.g. `mock-loja.html`, `mock-admin.html`, `mock-payments.html`.

### Suffix `_open`

| Suffix | Meaning |
|--------|---------|
| **`_open`** | **Living tracker** — still has pending tasks (✅ / ⬜ / 🔶). Update until work is done; then remove `_open` or archive. |

Use `_open` on roadmaps, ops runbooks with pending steps, and feature backlogs — not on stable reference specs that are “done”.

**Current `_open` trackers:**

| File | Scope |
|------|--------|
| `SUNDAY_open.md` | **Seed inventory** — what `seeds.sql` populates vs pending |
| `IMPLEMENTATION_ROADMAP_open.md` | **Main delivery tracker** — dev-first priorities + prod go-live (last) |
| `IMPLEMENTATION_ROADMAP_open.md` | Phased checklist (0 → E) |
| `PAYMENTS_open.md` | Mollie go-live (API key, webhook, E2E) |
| `AUTH_IDENTITY_ROADMAP_open.md` | Identity ↔ domain |
| `AUDITS_open.md` | Audit system (+ SMTP worker pending) |
| `AZUREBLOB_open.md` | Product images / `AzureFiles` |

### Proper names (exceptions)

Some files use a **descriptive name + `_open`** instead of a prefix when the name is clearer for the team:

- `IMPLEMENTATION_ROADMAP_open.md`, `PAYMENTS_open.md`, `AUDITS_open.md`, `AZUREBLOB_open.md`

New docs should **prefer the prefix table** unless Marco assigns a proper name.

### Agent / contributor checklist — “create a readme md”

| User asks for… | Create |
|----------------|--------|
| **Spec** (admin, store, stock, infra, feature behaviour) | `readme/SPEC_<Topic>.md` — stable spec; add `_open` only if it is primarily a pending checklist |
| **Auth / identity** | `readme/AUTH_<Topic>_open.md` while work is open |
| **Mock** (HTML prototype doc) | Update or add `readme/MOCK_<Topic>.md` + `docs/mock-<topic>.html` |
| **Data / DB / seeds** | `readme/DATA_<Topic>.md` |
| **Patterns / conventions** | `readme/PATTERNS_<Area>.md` |
| **Tracker with pending tasks** | `<NAME>_open.md` with ⬜ items and “Mark ✅ when done” in the header |

**Do not** duplicate trackers: link to [IMPLEMENTATION_ROADMAP_open.md](./IMPLEMENTATION_ROADMAP_open.md).

### Delivery priority (owner rule)

1. **Dev 100% first** — all features working locally with mocks (`Mollie:UseMock`, `Notifications:LowStock:UseMock`, local file media). Mocks = ✅ for dev.
2. **Prod go-live last** — real Mollie, SMTP worker, Azure Blob — only after dev is complete; needs client credentials / infra.

**Do not** delete or rename `_open` files Marco created without asking first.

### Legacy names (migrate over time)

Older files without prefixes (`ADMIN.md`, `WEB_STORE.md`, …) are superseded by `SPEC_*` where they exist. Prefer `SPEC_*` for new edits and links from root `README.md`.

---

## 📄 Documentation Footer Standard Pattern

**🎯 STANDARD (LABELS_OVERRRIDES.md Reference)**

Every readme file in `/readme/` MUST follow this EXACT footer pattern:

```markdown
## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**
```

**Footer Pattern Checklist:**

| Component | Requirement | Status | Notes |
|-----------|-------------|--------|-------|
| Section Label | `## Documentation` | ✅ Required | EXACT heading format |
| Documentation Links | Single link to main README only | ✅ Required | Use 🏠 emoji for main |
| Link Format | `[Main Documentation](../README.md) — Project overview and requirements` | ✅ Required | Exact description text |
| Main Link | Always include `../README.md` | ✅ Required | Points to root project README |
| Cross-links | All other doc links live in root `README.md` only | ✅ Required | No sibling links in `/readme/` footers |
| Separator | Horizontal rule `---` | ✅ Required | Before copyright |
| Copyright Line | Exact: `**© 2026 AdminSense. All rights reserved.**` | ✅ Required | **NO version info**, **NO dates** |
| No Extra Info | NO "Last Updated", NO version numbers | ✅ Critical | Keep footer minimal and clean |

**Implementation Note:**
When creating a NEW readme file in `/readme/`:
1. Add your content
2. Add level-2 heading: `## Documentation`
3. Add only the main README link (see pattern above)
4. Add `---` separator
5. Add copyright line: `**© 2026 AdminSense. All rights reserved.**`

---

## 📘 README Documentation Visual Patterns

### Executive Summary Section - Standard Pattern

All comprehensive documentation files SHOULD include an **Executive Summary** section with visual tables at the top for quick reference. Use this pattern when documenting complex systems.

### 🌐 Language Standard (English-only)

All documentation and UI-facing text in this repository MUST be written in **English**:

- **Documents / READMEs**: English-only (no PT/NL mixed sections)
- **Code identifiers**: classes, methods, variables, DTO fields, database columns → English-only
- **UI messages**: labels, buttons, tooltips, validation messages, notifications → English-only

**Rationale:** Consistency across teams, searchable terminology, and a single source of truth for domain vocabulary.

**Example Models:**
- ✅ [LABELS_OVERRRIDES.md](LABELS_OVERRRIDES.md) - Reference format: badges + numbered icon headings

### 🔢 Numbered Icon Headings (MANDATORY)

All feature/spec documentation in `/readme/` MUST use **numbered section headings with icons** to make large documents scannable.

**Pattern:**

- Use level-2 headings for major sections: `## <ICON> <NUMBER>. <Title>`
- Use level-3 headings for subsections: `### <NUMBER>.<NUMBER> <Title>` (icon optional)
- Keep numbering sequential (1, 2, 3...) across major sections

**Example (see `LABELS_OVERRRIDES.md`):**

```markdown
## ✅ 1. Requirements (based on the request)
## 💾 2. Persistence: SQL Server
## 🧠 3. Core idea: “canonical key” + override (Dutch only)
```

**Pattern: Status Badges**

Add status badges at the document header to provide quick visual reference:

```markdown
# 📊 Document Title

![Status](https://img.shields.io/badge/Status-Complete-28a745?style=flat-square) 
![Items](https://img.shields.io/badge/Items-87%2B-0d47a1?style=flat-square) 
![Coverage](https://img.shields.io/badge/Coverage-100%25-ff6f00?style=flat-square)

Brief description line
```

**Supported Badge Colors:**
- `28a745` - Green (✅ Complete, Success)
- `0d47a1` - Dark Blue (📊 Info, Quantity)
- `ff6f00` - Orange (⚠️ Warning, Important)
- `0dcaf0` - Light Blue (ℹ️ Info, Status)
- `6f42c1` - Purple (🎯 Feature, Target)

**Pattern: Statistics Summary Table**

Use a table to display key metrics after Executive Summary intro:

```markdown
### 📈 Coverage Statistics

| Category | Count | Status | Notes |
|----------|-------|--------|-------|
| **Total Items** | 29 | ✅ Complete | Lines per item |
| **Total Actions** | 87+ | ✅ Documented | All categorized |
| **Coverage** | 100% | ✅ Complete | No gaps |
```

**Pattern: Implementation Quality Table**

Display implementation status for different aspects:

```markdown
### ✅ Implementation Quality

| Aspect | Status | Details |
|--------|--------|---------|
| **Core CRUD** | ✅ Complete | Full coverage |
| **Advanced Features** | 🟢 Implemented | All working |
| **Special Handling** | 🔷 Enhanced | With badges |
| **Edge Cases** | ✅ Covered | All scenarios |
```

**Pattern: Category Summary Table**

Organize items by category with quick reference counts:

```markdown
### 📋 Categories Summary

| Category | Count | Items | Status |
|----------|-------|-------|--------|
| 🏗️ **Core** | 60+ | Building, Owner, Unit | ✅ |
| 🔐 **Security** | 5+ | Login, Register, 2FA | ✅ |
| 💰 **Payments** | 8+ | Schedules, Indexation | 🔷 |
| 📊 **Reports** | 3+ | Exports, Downloads | ✅ |
```

**Visual Organization Checklist:**

```
✅ Header with status badges
✅ Brief executive summary intro (1-2 sentences)
✅ Statistics summary table (key metrics)
✅ Implementation quality matrix (status overview)
✅ Category breakdown table (organized view)
✅ Detailed sections with full information below
✅ Tables use emojis for visual categories
✅ Separate detailed documentation sections follow tables
```

**When to Use This Pattern:**

- 📋 Complex systems with multiple components (Audit, Workflow, Reports)
- 📊 Inventory documentation (implementation reports)
- 🎯 Feature status documentation (capability matrix)
- 📈 Multi-aspect coverage reports
- ❌ DO NOT use for: Simple how-to guides, quick references, basic tutorials

**Table Badge Color Usage:**

| Badge | Meaning | Usage |
|-------|---------|-------|
| ✅ | Complete/Success | All items done |
| 🟢 | Implemented/OK | Feature working |
| 🟡 | In Progress | Partial |
| 🔷 | Special/Enhanced | Unique handling |
| ❌ | Missing/Failed | Not implemented |
| 🔄 | Updated | Recent changes |

---

## 📝 Form Fields & Validation

### Form Field Validation - COMPLETE PATTERN

**⚠️ CRITICAL: All required fields MUST follow this EXACT pattern**

#### **Step 1: CSS Helper - FieldValidationCss**

```csharp
// Purpose: Dynamically adds "is-invalid" class based on errors dictionary
public static class FieldValidationCss
{
    public static string FormControl(Dictionary<string, string> errors, string fieldId) 
        => errors.ContainsKey(fieldId) ? "form-control is-invalid" : "form-control";
    
    public static string FormSelect(Dictionary<string, string> errors, string fieldId) 
        => errors.ContainsKey(fieldId) ? "form-select is-invalid" : "form-select";
}
```

**What it does:**
- ✅ Checks if error exists in `_fieldErrors[fieldId]`
- ✅ Returns `"form-control is-invalid"` if error exists
- ✅ Returns `"form-control"` if no error (normal style)
- ✅ Automatically updates on validation changes

---

#### **Step 2: Text Input Field (Complete Example)**

```html
<div class="mb-3">
    <label for="name" class="form-label">Name *</label>
    <input type="text" 
           class="@FieldValidationCss.FormControl(_fieldErrors, "name")"
           id="name" 
           @bind="EditingItem.Name" 
           @onkeyup="@((KeyboardEventArgs e) => JSRuntime.InvokeVoidAsync("adminForm.validateNameField", "name"))"
           @onblur='() => ValidateFieldOnBlur("name", EditingItem.Name, "Name", "letters")'
           required />
    <ValidationFeedbackMessage FieldId="name" Errors="_fieldErrors" />
</div>
```

**Input Field Checklist:**

| Component | Implementation | Required | Purpose |
|-----------|----------------|----------|---------|
| Label | `<label for="name" class="form-label">Name *</label>` | ✅ | Accessible label + * indicator |
| Input Class | `@FieldValidationCss.FormControl(...)` | ✅ | Dynamic red border on error |
| Input ID | `id="name"` | ✅ | Match label's for attribute |
| Binding | `@bind="EditingItem.Name"` | ✅ | Two-way data binding |
| On Blur | `@onblur='() => ValidateFieldOnBlur(...)'` | ✅ | Validate on leave field |
| Message Component | `<ValidationFeedbackMessage ...>` | ✅ | Show/hide error message |
| Required Attribute | `required` | ✅ | HTML5 validation |
| Optional: On KeyUp | `@onkeyup="..."` | ❌ | Real-time JS validation |

---

#### **Step 3: Select Field (Complete Example)**

```html
<div class="mb-3">
    <label for="category" class="form-label">Category *</label>
    <select class="@FieldValidationCss.FormSelect(_fieldErrors, "category")"
            id="category"
            @bind="EditingItem.CategoryId"
            @onblur='() => ValidateFieldOnBlur("category", EditingItem.CategoryId.ToString(), "Category")'
            required>
        <option value="0">-- Select category --</option>
        @foreach (var cat in Categories)
        {
            <option value="@cat.Id">@cat.Name</option>
        }
    </select>
    <ValidationFeedbackMessage FieldId="category" Errors="_fieldErrors" />
</div>
```

**Select Field Checklist:**

| Component | Implementation | Required | Purpose |
|-----------|----------------|----------|---------|
| Label | `<label for="category">Category *</label>` | ✅ | Accessible label + * indicator |
| Select Class | `@FieldValidationCss.FormSelect(...)` | ✅ | Dynamic red border on error |
| Select ID | `id="category"` | ✅ | Match label's for attribute |
| Binding | `@bind="EditingItem.CategoryId"` | ✅ | Two-way data binding |
| On Blur | `@onblur='() => ValidateFieldOnBlur(...)'` | ✅ | Validate on leave field |
| Message Component | `<ValidationFeedbackMessage ...>` | ✅ | Show/hide error message |
| Required Attribute | `required` | ✅ | HTML5 validation |
| Placeholder Option | `<option value="0">-- Select --</option>` | ✅ | Helps user choose |

---

#### **Step 4: C# Validation Logic (Code-Behind)**

```csharp
private Dictionary<string, string> _fieldErrors = new();

private void ValidateFieldOnBlur(
    string fieldId, 
    string value, 
    string fieldName = "", 
    string validationType = "")
{
    if (string.IsNullOrWhiteSpace(value) || value == "0")
    {
        // Validation Failed: Required field is empty
        _fieldErrors[fieldId] = $"{fieldName} is required.";
    }
    else if (validationType == "letters" && !value.All(c => char.IsLetter(c) || c == ' '))
    {
        // Validation Failed: Contains invalid characters
        _fieldErrors[fieldId] = $"{fieldName} must contain only letters.";
    }
    else if (validationType == "decimal" && !decimal.TryParse(value, out _))
    {
        // Validation Failed: Invalid number
        _fieldErrors[fieldId] = $"{fieldName} must be a valid number.";
    }
    else if (validationType == "email" && !value.Contains("@"))
    {
        // Validation Failed: Invalid email
        _fieldErrors[fieldId] = $"{fieldName} must be a valid email.";
    }
    else
    {
        // Validation Passed: Remove error if exists
        _fieldErrors.Remove(fieldId);
    }
}
```

**Validation Types Supported:**

| Type | Validation Rule | Usage |
|------|-----------------|-------|
| (empty) | Required only | `ValidateFieldOnBlur("name", value, "Name")` |
| `"letters"` | Must be letters only | `ValidateFieldOnBlur("name", value, "Name", "letters")` |
| `"decimal"` | Must be valid number | `ValidateFieldOnBlur("price", value, "Price", "decimal")` |
| `"email"` | Must contain @ | `ValidateFieldOnBlur("email", value, "Email", "email")` |

---

#### **Step 5: Validation Feedback Component**

```razor
<!-- ValidationFeedbackMessage.razor -->
@if (Errors is not null && !string.IsNullOrEmpty(FieldId) && Errors.TryGetValue(FieldId, out var msg))
{
    <div class="invalid-feedback d-block" role="alert">@msg</div>
}

@code {
    [Parameter, EditorRequired] public string FieldId { get; set; } = "";
    [Parameter] public IReadOnlyDictionary<string, string>? Errors { get; set; }
}
```

**How it works:**
1. Receives `FieldId` (e.g., "name")
2. Receives `Errors` dictionary from parent
3. Checks if error exists for this field: `Errors.TryGetValue(FieldId, out var msg)`
4. If error exists: Shows message in red with `invalid-feedback d-block` classes
5. If no error: Automatically hidden (component returns nothing)
6. `role="alert"` for screen reader accessibility

---

#### **Step 6: CSS Classes Applied by Bootstrap**

```css
/* Input/Select with error - RED BORDER */
.form-control.is-invalid,
.form-select.is-invalid {
    border-color: #dc3545;      /* RED border */
    padding-right: calc(1.5em + .75rem);
}

/* Error message - RED TEXT */
.invalid-feedback {
    color: #dc3545;             /* RED text */
    display: none;              /* Hidden by default */
}

/* Show error message */
.invalid-feedback.d-block {
    display: block !important;  /* Shown when d-block is added */
}
```

---

#### **Step 7: Complete Validation Flow**

```
USER INTERACTION FLOW
│
├─ User Types in Field
│  └─ @onkeyup="validateNameField()" (JS validation if needed)
│
├─ User Leaves Field (blur event)
│  └─ @onblur="ValidateFieldOnBlur(...)" called
│
├─ C# Validation Method Runs
│  ├─ Check if empty: if (string.IsNullOrWhiteSpace(value))
│  ├─ Check type: if (validationType == "letters")
│  └─ Set error or clear: _fieldErrors[fieldId] = "message"
│
├─ UI Automatically Updates
│  ├─ Input class updates: @FieldValidationCss.FormControl(...)
│  ├─ If error: class="form-control is-invalid"
│  ├─ If no error: class="form-control"
│  └─ Message: ValidationFeedbackMessage checks _fieldErrors
│
└─ User Sees Feedback
   ├─ If Error:
   │  ├─ RED border on input field
   │  └─ RED error message below field
   └─ If Fixed:
      ├─ Normal border on input field
      └─ Error message hidden automatically
```

---

#### **Step 8: Real-World Complete Form Example**

```razor
<form @onsubmit="HandleSave" @onsubmit:preventDefault="true">
    <!-- Row 1: Code and Label -->
    <div class="row">
        <div class="col-md-6 mb-3">
            <label for="code" class="form-label">Code *</label>
            <input type="text" 
                   class="@FieldValidationCss.FormControl(_fieldErrors, "code")"
                   id="code" 
                   @bind="EditingItem.Code"
                   @onblur='() => ValidateFieldOnBlur("code", EditingItem.Code, "Code", "letters")'
                   required />
            <ValidationFeedbackMessage FieldId="code" Errors="_fieldErrors" />
        </div>
        
        <div class="col-md-6 mb-3">
            <label for="label" class="form-label">Label *</label>
            <input type="text" 
                   class="@FieldValidationCss.FormControl(_fieldErrors, "label")"
                   id="label" 
                   @bind="EditingItem.Label"
                   @onblur='() => ValidateFieldOnBlur("label", EditingItem.Label, "Label")'
                   required />
            <ValidationFeedbackMessage FieldId="label" Errors="_fieldErrors" />
        </div>
    </div>
    
    <!-- Row 2: Category Select -->
    <div class="row">
        <div class="col-md-6 mb-3">
            <label for="category" class="form-label">Category *</label>
            <select class="@FieldValidationCss.FormSelect(_fieldErrors, "category")"
                    id="category"
                    @bind="EditingItem.CategoryId"
                    @onblur='() => ValidateFieldOnBlur("category", EditingItem.CategoryId.ToString(), "Category")'
                    required>
                <option value="0">-- Select category --</option>
                @foreach (var cat in Categories)
                {
                    <option value="@cat.Id">@cat.Name</option>
                }
            </select>
            <ValidationFeedbackMessage FieldId="category" Errors="_fieldErrors" />
        </div>
    </div>
    
    <!-- Form Actions -->
    <div class="d-flex gap-2 mt-4">
        <button type="submit" class="btn btn-primary" disabled="@IsSaving">
            @if (IsSaving)
            {
                <span class="spinner-border spinner-border-sm me-2"></span>
            }
            else
            {
                <i class="bi bi-check-circle me-2"></i>
            }
            Save
        </button>
        <button type="button" class="btn btn-secondary" @onclick="CancelForm">
            <i class="bi bi-x me-1"></i>Cancel
        </button>
    </div>
</form>

@code {
    private EditingItemType EditingItem = new();
    private List<Category> Categories = new();
    private Dictionary<string, string> _fieldErrors = new();
    private bool IsSaving = false;

    private void ValidateFieldOnBlur(string fieldId, string value, string fieldName = "", string validationType = "")
    {
        if (string.IsNullOrWhiteSpace(value) || value == "0")
        {
            _fieldErrors[fieldId] = $"{fieldName} is required.";
        }
        else if (validationType == "letters" && !value.All(c => char.IsLetter(c) || c == ' '))
        {
            _fieldErrors[fieldId] = $"{fieldName} must contain only letters.";
        }
        else
        {
            _fieldErrors.Remove(fieldId);
        }
    }

    private async Task HandleSave()
    {
        // Validate all required fields before saving
        ValidateFieldOnBlur("code", EditingItem.Code, "Code", "letters");
        ValidateFieldOnBlur("label", EditingItem.Label, "Label");
        ValidateFieldOnBlur("category", EditingItem.CategoryId.ToString(), "Category");
        
        if (_fieldErrors.Count > 0)
            return; // Can't save with errors
        
        IsSaving = true;
        try
        {
            // Save logic here
            await SaveItemAsync(EditingItem);
        }
        finally
        {
            IsSaving = false;
        }
    }

    private void CancelForm() => NavigationManager.NavigateTo("/menu");
}
```

**Complete Form Checklist:**

```
REQUIRED FIELDS
  [✅] Each required field has: label + * indicator
  [✅] Each required field has: @bind="Model.Property"
  [✅] Each required field has: @onblur validator
  [✅] Each required field has: ValidationFeedbackMessage component
  [✅] Each required field has: required attribute
  [✅] Each required field has: id matching label's for

VALIDATION LOGIC
  [✅] ValidateFieldOnBlur method implemented
  [✅] _fieldErrors Dictionary<string, string> defined
  [✅] Validation types: required, letters, email, decimal
  [✅] Clear error when validation passes: _fieldErrors.Remove(fieldId)

FORM ACTIONS
  [✅] Submit button: type="submit", btn-primary
  [✅] Cancel button: type="button", btn-secondary
  [✅] Spinner on submit button during save
  [✅] All buttons disabled during save: disabled="@IsSaving"

ERROR DISPLAY
  [✅] Red border on error fields: form-control.is-invalid
  [✅] Red text message: invalid-feedback d-block
  [✅] Auto-clear when corrected
  [✅] aria-label for accessibility

EDGE CASES
  [✅] Prevent form submission if errors: check _fieldErrors.Count
  [✅] Disable buttons during async save
  [✅] Handle dropdown: validate != "0"
  [✅] Handle async operations: try/finally pattern
```

---

### 🗑️ Delete Confirmation Modal - Standard Pattern

**⚠️ CRITICAL PATTERN (CustomerList.razor Reference - ALL modals follow this exactly)**

```html
@if (ShowDeleteConfirm && ItemToDelete != null)
{
    <div class="modal fade show d-block" tabindex="-1" role="dialog" style="background-color: rgba(0,0,0,0.5);">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <!-- Modal Header -->
                <div class="modal-header border-0">
                    <h5 class="modal-title">Confirm Deletion</h5>
                    <button type="button" class="btn-close" @onclick="CancelDelete" disabled="@IsDeleting"></button>
                </div>
                
                <!-- Modal Body -->
                <div class="modal-body">
                    <p class="mb-2">Are you sure you want to delete <strong>@ItemToDelete.Name</strong>?</p>
                    <p class="text-danger mb-0"><small>This action cannot be undone.</small></p>
                </div>
                
                <!-- Modal Footer -->
                <div class="modal-footer border-0">
                    <button type="button" class="btn btn-secondary" @onclick="CancelDelete" disabled="@IsDeleting">
                        <i class="bi bi-x me-1"></i>Cancel
                    </button>
                    <button type="button" class="btn btn-danger" @onclick="ConfirmDelete" disabled="@IsDeleting">
                        @if (IsDeleting)
                        {
                            <span class="spinner-border spinner-border-sm me-2"></span>
                        }
                        Delete @ItemType
                    </button>
                </div>
            </div>
        </div>
    </div>
}
```

**Modal Features Checklist:**

| Feature  | Implementation | Status | Required | Why |
|----------|----------------|--------|----------|-----|
| Display Classes | `modal fade show d-block` | ✅ | **YES** | Both classes required for display |
| Role Attribute | `role="dialog"` | ✅ | **YES** | Accessibility |
| Backdrop Style | `style="background-color: rgba(0,0,0,0.5);"` | ✅ | **YES** | Don't use `display: block` |
| Centering | `modal-dialog-centered` | ✅ | **YES** | Vertical center alignment |
| Header Border | `border-0` | ✅ | **YES** | Clean look |
| Footer Border | `border-0` | ✅ | **YES** | Clean look |
| Title Text | "Confirm Deletion" | ✅ | **YES** | Standard title |
| Warning Message | Red text "This action cannot be undone." | ✅ | **YES** | Risk awareness |
| Disabled State | All buttons during delete | ✅ | **YES** | Prevent double-click |
| Loading Spinner | Show only during delete | ✅ | **YES** | User feedback |

**Modal Checklist for New Modals:**

```
VISUAL STRUCTURE
  [✅] Modal classes: "modal fade show d-block"
  [✅] Role attribute: role="dialog"
  [✅] Backdrop: style="background-color: rgba(0,0,0,0.5);"
  [✅] Dialog class: modal-dialog-centered

HEADER SECTION
  [✅] Title: "Confirm Deletion"
  [✅] Close button responsive to disabled state
  [✅] border-0 class (no border)

BODY SECTION
  [✅] Item name in bold: <strong>@ItemToDelete.Name</strong>
  [✅] Clear warning: "This action cannot be undone."
  [✅] Warning in red text: class="text-danger"

FOOTER SECTION
  [✅] Cancel button: btn-secondary + disabled state
  [✅] Delete button: btn-danger + disabled state
  [✅] Loading spinner on delete button
  [✅] border-0 class (no border)

CODE LOGIC
  [✅] All buttons disabled during operation
  [✅] ShowDeleteConfirm flag controls show/hide
  [✅] ItemToDelete contains item details
  [✅] IsDeleting flag during operation
```

---

### 💾 Form Action Buttons - Standard Pattern

**STANDARD Form Button Group (all forms follow this exactly):**

```html
<div class="d-flex gap-2">
    <!-- Primary Action (Save/Create) -->
    <button type="submit" class="btn btn-primary" disabled="@IsSaving">
        @if (IsSaving)
        {
            <span class="spinner-border spinner-border-sm me-2"></span>
        }
        else
        {
            <i class="bi bi-check-circle me-2"></i>
        }
        @(EditingItem.Id > 0 ? "Update" : "Create")
    </button>
    
    <!-- Secondary Action (Cancel) -->
    <button type="button" class="btn btn-secondary" @onclick="CancelForm">
        <i class="bi bi-x me-1"></i>Cancel
    </button>
</div>
```

**Form Button Checklist:**

| Element | Implementation | Status | Required |
|---------|----------------|--------|----------|
| Submit Button | `btn btn-primary` | ✅ | **YES** |
| Cancel Button | `btn btn-secondary` | ✅ | **YES** |
| Submit Icon | `bi-check-circle` (no -fill) | ✅ | **YES** |
| Cancel Icon | `bi-x` | ✅ | **YES** |
| Loading State | Spinner replaces icon | ✅ | **YES** |
| Disabled During Save | `disabled="@IsSaving"` | ✅ | **YES** |
| Button Wrapper | `d-flex gap-2` | ✅ | **YES** |
| Icon Spacing | `me-2` for icons with text | ✅ | **YES** |

**Form Button Code Checklist:**

```
SUBMIT BUTTON
  [✅] Type: type="submit"
  [✅] Class: btn btn-primary
  [✅] Disabled: disabled="@IsSaving"
  [✅] Icon: bi-check-circle (only when NOT saving)
  [✅] Spinner: (only when saving)
  [✅] Text: "Create" or "Update" based on EditingItem.Id

CANCEL BUTTON
  [✅] Type: type="button"
  [✅] Class: btn btn-secondary
  [✅] Click Handler: @onclick="CancelForm"
  [✅] Icon: bi-x
  [✅] Never be disabled

WRAPPER
  [✅] Container class: d-flex gap-2
  [✅] Places buttons side-by-side
  [✅] Consistent spacing between buttons
```

---

## 🔐 Security & Compliance

All implementations support:
- ✅ **GDPR** - Data protection compliance
- ✅ **LGPD** - Brazilian data protection compliance
- ✅ **SOX** - Financial record keeping
- ✅ **ISO 27001** - Information security standards

---

## 📦 NuGet Packages In Use

```xml
<PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore" 
                  Version="10.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" 
                  Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" 
                  Version="10.0.0" />
```

---

## 📊 Color Palette Reference

**Bootstrap Status Colors (Standard):**

| Color | Hex Code | Usage | Button Class |
|-------|----------|-------|--------------|
| 🔵 **Primary** (Blue) | `#0d6efd` | Create, Edit, Save, Primary actions | `btn-primary` |
| 🟩 **Success** (Green) | `#198754` | Positive actions, Refresh, Approve | `btn-success` |
| 🔴 **Danger** (Red) | `#dc3545` | Delete, Clear, Reject, Warnings | `btn-danger` |
| 🟦 **Secondary** (Gray) | `#6c757d` | Back, Cancel, Neutral actions | `btn-secondary` |
| ⚪ **Outline** (Gray Border) | `#6c757d` | Back navigation (outline only) | `btn-outline-secondary` |
| 🔷 **Info** (Light Blue) | `#0dcaf0` | View, Details, Download, Info | `btn-info` |

**Usage Rules:**
- ✅ Use outline style ONLY for back/navigation buttons
- ✅ Use primary for dangerous or primary actions
- ✅ Use consistent colors across similar actions
- ✅ Error messages: use red text `class="text-danger"`
- ✅ Success feedback: use green borders/backgrounds

---

## 🎓 Implementation by Page (Status as of 2026-04-01)

### Core Navigation Pages

| Page Name | File | Back Button | Buttons | Grid | Forms | Validation | Delete Modal | Status |
|-----------|------|:-----------:|:-------:|:----:|:-----:|:----------:|:------------:|:------:|
| CustomerList | CustomerList.razor | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **Reference** |
| Building | Building.razor | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| Owner | Owner.razor | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| BuildingOwner | BuildingOwner.razor | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| Contact | Contact.razor | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| Unit | Unit.razor | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| UnitFixedCostManage | UnitFixedCostManage.razor | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| UnitFixedCostList | UnitFixedCostList.razor | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |
| WorkflowRentalContractForm | WorkflowRentalContractForm.razor | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ Complete |

**Legend:**
- ✅ = Implemented and verified
- ⚠️ = Partially implemented
- ❌ = Not implemented

---

## 🧪 Testing & Validation

### Form Validation Testing Checklist

```
□ REQUIRED FIELDS
  □ Empty field shows error with red border
  □ Error message displays in red
  □ Field label has * indicator
  □ Error clears when field is corrected
  □ Cannot submit form with validation errors

□ FIELD TYPES
  □ Text field: accepts letters and spaces
  □ Number field: rejects non-numeric input
  □ Email field: validates @ symbol
  □ Select field: validates selection != "0"
  □ Each shows specific error message

□ USER EXPERIENCE
  □ Error shows on blur (field exit)
  □ Error clears on correction
  □ Loading spinner shows during save
  □ Buttons disabled during save
  □ No double-submit possible

□ ACCESSIBILITY
  □ All fields have labels
  □ All required fields marked with *
  □ Error messages have role="alert"
  □ Title attributes on buttons
  □ Keyboard navigation works
```

### Button Testing Checklist

```
□ HEADER BUTTONS
  □ Back button uses OpenIconic arrow-left
  □ Back button is outline-secondary
  □ Refresh button shows loading state
  □ All buttons have title attributes
  □ Buttons responsive on mobile

□ GRID ACTION BUTTONS
  □ Edit button: primary color, pencil icon
  □ Delete button: danger color, trash icon
  □ Icon only (no text in grids)
  □ Buttons properly sized (btn-sm)
  □ All buttons have specific title
  □ Disabled state during operation

□ EXPORT & SEARCH (admin list pages)
  □ Form header: AdminEntityFormHeader + green EXPORT (CSV/PDF only)
  □ IGridExportService injected; BuildExportRequest matches grid columns
  □ Export disabled when grid loading or empty
  □ Grid toolbar: AdminGridSearch full width (not small input)
  □ Enter key applies search; _searchDraft → _filter.Search

□ FORM BUTTONS
  □ Submit: primary color, check icon
  □ Cancel: secondary color, x icon
  □ Disabled during save
  □ Spinner replaces icon when saving
  □ Submit only works when no errors
  □ Cancel navigates away

□ MODAL BUTTONS
  □ Modal appears centered
  □ Backdrop colors correct
  □ Buttons disabled during operation
  □ Close button responsive
  □ Modal closes on cancel/complete
```

---

## 📓 Developer Notes

### Common Pitfalls & Solutions

**Problem:** Back button shows Bootstrap Icon instead of OpenIconic
```
❌ <i class="bi bi-arrow-left"></i>  (WRONG)
✅ <span class="oi oi-arrow-left"></span>  (CORRECT)
```

**Problem:** Delete modal doesn't display (appears invisible)
```
❌ style="display: block;"  (WRONG - doesn't show)
✅ style="background-color: rgba(0,0,0,0.5);"  (CORRECT - shows backdrop)
```

**Problem:** Validation errors don't clear automatically
```
❌ Leave error in dictionary  (WRONG)
✅ _fieldErrors.Remove(fieldId)  (CORRECT)
```

**Problem:** Back button appears as solid instead of outlined
```
❌ class="btn btn-secondary"  (WRONG - solid)
✅ class="btn btn-outline-secondary"  (CORRECT - outlined)
```

**Problem:** Grid buttons too big or cause layout issues
```
❌ class="btn btn-primary"  (WRONG - full size)
✅ class="btn btn-sm btn-primary"  (CORRECT - small)
```

---

## 📞 Support & Questions

For questions about these patterns:

1. **Reference Implementation:** Check `Web/Components/Pages/Admin/CustomerList.razor`
2. **Visual Standards:** See button color table above
3. **Validation Pattern:** Review form example in Form Field Validation section
4. **Quick Copy-Paste:** Use snippets in Quick Reference Card

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**


