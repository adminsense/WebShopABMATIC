# 🎨 UI Patterns - Quick Start Guide for Developers

![Status](https://img.shields.io/badge/Status-Quick%20Reference-28a745?style=flat-square) ![Copy-Paste](https://img.shields.io/badge/Copy--Paste-Ready-0dcaf0?style=flat-square) ![UI%20Patterns](https://img.shields.io/badge/UI%20Patterns-Complete-512BD4?style=flat-square)

**Visual Quick Reference | Copy-Paste Ready | No Reading Required**

---

## 🖼️ Mock Prototype Layout (WebShopABMATIC)

The HTML admin mock in `docs/mock-admin.html` follows the **AB-MATIC** admin shell. Three screen types map to the reference images in `images/`:

| Screen | Image | Mock file / view |
|--------|-------|------------------|
| **Dashboard** — sidebar, top bar, portfolio cards, logout, version | ![Main](images/main_screen.png) | `docs/mock-admin.html` → **Start** |
| **Hub** — back link, entity cards, “X form” buttons | ![Menu](images/menu_screen.png) | `docs/mock-admin.html` → sidebar menus (Webshop, Catalog, …) |
| **List + form** — filters, Apply/Clear, `table-dark` grid, edit form | ![Forms](images/forms_screen.png) | `docs/mock-admin.html` → entity list / form views |

Storefront + admin entry: `docs/mock-loja.html` (**Admin Panel** after `StaffUser.Admin` login).

**Full menu/entity/table documentation:** [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md)

The button, grid, and form patterns below are what the mock (and future Blazor pages) must follow.

---

## 🚦 Icon System Cheat Sheet

### 🎯 Where to Use Each Icon Set

| Icon Set | System | Used For | Example | Import |
|----------|--------|----------|---------|--------|
| `oi oi-*` | **OpenIconic** | Navigation/UI | Back, Menu, Settings | HTML: `<span class="oi oi-X"></span>` |
| `bi bi-*` | **Bootstrap Icons** | Actions/Content | Edit, Delete, Save | HTML: `<i class="bi bi-X"></i>` |

**⚠️ CRITICAL:** Never mix them! OpenIconic for nav, Bootstrap for actions.

---

## 🔘 Button Size & Style Quick Reference

```
┌─────────────────────────────────────────────────┐
│ CONTEXT                    │ CLASS               │
├─────────────────────────────────────────────────┤
│ Header (Back)              │ btn outline-secondary btn-sm │
│ Header (Refresh)           │ btn btn-success btn-sm       │
│ Filter Apply/Clear         │ btn btn-primary              │
│         (NO btn-sm)        │ btn btn-danger               │
│ Grid Actions (Edit/Delete) │ btn btn-sm btn-primary       │
│         (NO btn-sm)        │ btn btn-sm btn-danger        │
│ Form Save/Cancel           │ btn btn-primary              │
│         (NO btn-sm)        │ btn btn-secondary            │
└─────────────────────────────────────────────────┘
```

**Rule:** If button in GRID → add `btn-sm` | If button in FORM/HEADER → NO `btn-sm` (except **EXPORT** on form card header → `btn-sm`)

---

## 📋 Copy-Paste Snippets

### 1️⃣ BACK BUTTON - Always use this exactly

```html
<button type="button" class="btn btn-outline-secondary btn-sm" @onclick="NavigateBack" title="Go back">
    <span class="oi oi-arrow-left"></span> Back
</button>
```

✅ What's special:
- OpenIconic `oi-arrow-left` (NOT `bi-arrow-left`)
- `btn-outline-secondary` (NOT solid `btn-secondary`)
- ALWAYS `btn-sm`

---

### 2️⃣ GRID ACTION BUTTONS - Edit & Delete

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

✅ What's special:
- ICON ONLY (no text in grids)
- Always `btn-sm btn-primary` and `btn-sm btn-danger`
- Wrapped in `btn-group btn-group-sm`
- Title includes what you're editing/deleting

---

### 3️⃣ FORM BUTTONS - Save & Cancel

```html
<div class="d-flex gap-2">
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
```

✅ What's special:
- Spinner replaces icon when saving
- Icon `me-2` (margin before text)
- NO `btn-sm` (full size buttons)
- Both buttons disabled during save

---

### 4️⃣ FILTER BUTTONS - Apply & Clear

```html
<div class="d-flex gap-2">
    <button type="button" class="btn btn-primary" @onclick="ApplyFilters" title="Apply filter criteria">
        <i class="bi bi-funnel-fill me-1"></i>Apply Filters
    </button>
    <button type="button" class="btn btn-danger" @onclick="ClearFilters" title="Clear all filters">
        <i class="bi bi-x-circle-fill me-1"></i>Clear
    </button>
</div>
```

✅ What's special:
- NO `btn-sm` (full size)
- Icon `me-1` (margin before text)
- `bi-funnel-fill` with `-fill` suffix
- `bi-x-circle-fill` with `-fill` suffix

---

### 5️⃣ TEXT INPUT - With Validation

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

✅ What's special:
- `*` in label for required fields
- Use `FieldValidationCss.FormControl()` helper
- `@onblur` (not on keyup)
- Always include `ValidationFeedbackMessage`

**Code-Behind:**
```csharp
private Dictionary<string, string> _fieldErrors = new();

private void ValidateFieldOnBlur(string fieldId, string value, string fieldName = "")
{
    if (string.IsNullOrWhiteSpace(value))
        _fieldErrors[fieldId] = $"{fieldName} is required.";
    else
        _fieldErrors.Remove(fieldId);
}
```

---

### 6️⃣ DELETE CONFIRMATION MODAL

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
                        @if (IsDeleting)
                        {
                            <span class="spinner-border spinner-border-sm me-2"></span>
                        }
                        Delete Item
                    </button>
                </div>
            </div>
        </div>
    </div>
}
```

✅ What's special:
- BOTH `modal` and `d-block` classes required
- `style="background-color: rgba(0,0,0,0.5);"` (NOT `display: block`)
- `border-0` on header AND footer
- Title: Always "Confirm Deletion"
- Warning in red: "This action cannot be undone."
- All buttons disabled during delete

---

### 7️⃣ DATA GRID / TABLE

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
                    <td class="text-center"><!-- Grid buttons here --></td>
                </tr>
            }
        </tbody>
    </table>
</div>
```

✅ What's special:
- ALWAYS `table-dark` (black header)
- ALWAYS `table-striped table-hover`
- ALWAYS `@key="item.Id"` (performance)
- ALWAYS wrap in `table-responsive`

---

### 8️⃣ EXPORT BUTTON — Form card header (green dropdown)

**Placement:** top-right of the **create/edit form** card header (same row as the form title). Matches the AB-MATIC admin mock.

**Component:** `<AdminEntityFormHeader>` wraps the title and embeds `<AdminExportDropdown>`.

```razor
@inject IGridExportService GridExport

<div class="card shadow-sm border entity-form-card admin-form-card">
    <AdminEntityFormHeader ExportDisabled="@ExportDisabled" OnExport="ExportGridAsync">
        <Title>@(_isEditing ? "Edit product price" : "Create product price")</Title>
    </AdminEntityFormHeader>
    <div class="card-body">
        @* EditForm … *@
    </div>
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
    GridExportBuilder.FromRows(
        "product-prices",
        "Product price list",
        new[] { "Id", "ProductId", "GrossSalesPrice" },
        _result?.Items.Select(item => (IReadOnlyList<string>)new[]
        {
            GridExportBuilder.Cell(item.Id),
            GridExportBuilder.Cell(item.ProductId),
            GridExportBuilder.Cell(item.GrossSalesPrice)
        }));
```

What's special:
- Green **`btn btn-success btn-sm`** label **EXPORT** with chevron (not full-size)
- Dropdown options are **only CSV and PDF** — no other formats
- Exports the **current grid rows** (respect filters/search), not the form draft
- Disabled when grid is loading or empty
- Read-only pages without a form card: put `<AdminExportDropdown>` on the grid toolbar instead (`StockOverview`, `StockMovementList`)

**Files:** `Web/Components/Admin/AdminEntityFormHeader.razor`, `AdminExportDropdown.razor`, `Web/Services/GridExportService.cs`, `wwwroot/js/admin-export.js`

---

### 9️⃣ GRID SEARCH — Full-width toolbar search

**Placement:** right side of the grid card header (`entity-grid-toolbar`), stretching from the **Search** label to the magnifying-glass icon.

**Component:** `<AdminGridSearch>` — do **not** use a small standalone `<input type="search">`.

```razor
<div class="card-header bg-white entity-grid-toolbar">
    <h2 class="h6 mb-0 fw-bold">Product price list</h2>
    <AdminGridSearch @bind-Value="_searchDraft" OnSearch="ApplySearchAsync" />
</div>
```

```csharp
private string _searchDraft = string.Empty;

private async Task ApplySearchAsync()
{
    _filter.Search = _searchDraft;
    _filter.Page = 1;
    await LoadGridAsync(_cts?.Token ?? CancellationToken.None);
}
```

What's special:
- `.entity-grid-search` uses `flex: 1` so the field fills the toolbar (title left, search grows to the right)
- Input group: `[Search label][text field…][magnifying glass icon]`
- Enter key triggers `OnSearch`; binding uses `_searchDraft` → `_filter.Search` on apply
- CSS: `Web/wwwroot/css/admin.css` (`.entity-grid-toolbar`, `.entity-grid-search`)

---

## 🎨 Color Code Reference

| Color | Hex | Button Class | When to Use |
|-------|-----|--------------|------------|
| 🔵 Blue | `#0d6efd` | `btn-primary` | Create, Edit, Save |
| 🟩 Green | `#198754` | `btn-success` | Refresh, Approve, **EXPORT** dropdown |
| 🔴 Red | `#dc3545` | `btn-danger` | Delete, Clear, Reject |
| 🟦 Gray Fill | `#6c757d` | `btn-secondary` | Cancel |
| ⚪ Gray Outline | `#6c757d` | `btn-outline-secondary` | Back navigation |
| 🔷 Light Blue | `#0dcaf0` | `btn-info` | View, Download |

---

## ✅ Pre-Flight Checklist

Before submitting a page, verify:

### 🔘 Buttons
- [ ] Back button: `oi-arrow-left` (OpenIconic, NOT `bi-`)
- [ ] Back button: `btn-outline-secondary btn-sm` (outlined, small)
- [ ] Grid buttons: `btn-sm btn-primary` and `btn-sm btn-danger` (small)
- [ ] Form buttons: `btn btn-primary` and `btn btn-secondary` (NO `btn-sm`)
- [ ] All buttons have `title` attribute
- [ ] Icons use `me-1` or `me-2` spacing

### 📋 Forms
- [ ] Required fields have `*` in label
- [ ] Required fields have `@onblur` validation
- [ ] Required fields have `ValidationFeedbackMessage` component
- [ ] Required fields have `required` attribute
- [ ] Form uses `FieldValidationCss.FormControl()` or `.FormSelect()`
- [ ] Validation errors have specific messages
- [ ] Buttons disabled during save: `disabled="@IsSaving"`

### 🗂️ Grids
- [ ] Header: `class="table-dark"`
- [ ] Rows: `@key="item.Id"` on `<tr>`
- [ ] Table: `table-responsive` wrapper
- [ ] Table: `table-striped table-hover`
- [ ] Actions: icon-only (NO text in grids)
- [ ] Toolbar: `<AdminGridSearch>` (full width, not a small input)
- [ ] Form header: `<AdminEntityFormHeader>` with EXPORT (CSV/PDF)
- [ ] `@inject IGridExportService GridExport` + `BuildExportRequest()` on list pages

### 🔲 Modals
- [ ] Classes: `modal fade show d-block`
- [ ] Style: `style="background-color: rgba(0,0,0,0.5);" ` (NOT display: block)
- [ ] Dialog: `modal-dialog-centered`
- [ ] Header/Footer: `border-0` (no border)
- [ ] Title: "Confirm Deletion"
- [ ] Warning: Red text "This action cannot be undone."
- [ ] All buttons: `disabled="@IsDeleting"`

---

## 🔥 Common Mistakes (Don't Do These!)

```
❌ Use bi-arrow-left for back button
✅ Use oi-arrow-left instead

❌ Back button with btn-secondary (solid)
✅ Back button with btn-outline-secondary (outlined)

❌ Grid buttons without btn-sm
✅ Grid buttons with btn-sm btn-primary

❌ Form buttons with btn-sm
✅ Form buttons without btn-sm (full size)

❌ Modal style="display: block;"
✅ Modal style="background-color: rgba(0,0,0,0.5);"

❌ Delete modal missing d-block class
✅ Delete modal with "modal fade show d-block"

❌ Validation errors in permanent state
✅ Validation errors clear: _fieldErrors.Remove(fieldId)

❌ Grid buttons with text like "Edit Item"
✅ Grid buttons icon-only: just <i class="bi bi-pencil"></i>

❌ Form without specific error messages
✅ Form with messages like "Name is required."

❌ Input field without required attribute
✅ Input field with required attribute AND @onblur
```

---

## 🚀 Lightning Speed Lookup

**Q: How do I style the back button?**
A: Copy snippet #1 above. Use `oi-arrow-left` and `btn-outline-secondary btn-sm`.

**Q: How do I validate a form field?**
A: Copy snippet #5. Use `@onblur`, `FieldValidationCss.FormControl()`, and `ValidationFeedbackMessage`.

**Q: How do I make grid action buttons?**
A: Copy snippet #2. Use `btn-sm btn-primary` and `btn-sm btn-danger`, icon-only.

**Q: How do I show a delete confirmation?**
A: Copy snippet #6. Need `modal fade show d-block` and warning text in red.

**Q: What's the table structure?**
A: Copy snippet #7. Must have `table-dark`, `@key="item.Id"`, and `table-responsive` wrapper.

**Q: What color for [action]?**
A: See Color Code Reference above. Primary (blue) for main actions, danger (red) for delete.

---

## 📞 Still Confused?

1. Find your page in `Web/Components/Pages/Admin/`
2. Look at `CustomerList.razor` - it has EVERYTHING!
3. Copy what you need from this guide
4. Run the app and verify it looks right

**Remember:** When in doubt, copy CustomerList.razor - it's the reference standard!

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**© 2026 AdminSense. All rights reserved.**
