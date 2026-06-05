# ðŸŽ¨ UI Patterns - Quick Start Guide for Developers

![Status](https://img.shields.io/badge/Status-Quick%20Reference-28a745?style=flat-square) ![Copy-Paste](https://img.shields.io/badge/Copy--Paste-Ready-0dcaf0?style=flat-square) ![UI%20Patterns](https://img.shields.io/badge/UI%20Patterns-Complete-512BD4?style=flat-square)

**Visual Quick Reference | Copy-Paste Ready | No Reading Required**

---

## Mock prototype layout (WebShopABMATIC)

The HTML admin mock in `docs/mock-admin.html` follows the **IMMO BELGIUM** admin shell. Three screen types map to the reference images in `readme/images/`:

| Screen | Image | Mock file / view |
|--------|-------|------------------|
| **Dashboard** — sidebar, top bar, portfolio cards, logout, version | ![Main](../readme/images/main_screen.png) | `docs/mock-admin.html` → **Start** |
| **Hub** — back link, entity cards, “X form” buttons | ![Menu](../readme/images/menu_screen.png) | `docs/mock-admin.html` → sidebar menus (Webshop, Catalog, …) |
| **List + form** — filters, Apply/Clear, `table-dark` grid, edit form | ![Forms](../readme/images/forms_screen.png) | `docs/mock-admin.html` → entity list / form views |

Storefront + admin entry: `docs/mock-loja.html` (**Admin Panel** after `StaffUser.Admin` login).

**Full menu/entity/table documentation:** [MOCK_PROTOTYPE_GUIDE.md](MOCK_PROTOTYPE_GUIDE.md)

The button, grid, and form templates below are what the mock (and future Blazor pages) must follow.

---

## ðŸš¦ Icon System Cheat Sheet

### ðŸŽ¯ Where to Use Each Icon Set

| Icon Set | System | Used For | Example | Import |
|----------|--------|----------|---------|--------|
| `oi oi-*` | **OpenIconic** | Navigation/UI | Back, Menu, Settings | HTML: `<span class="oi oi-X"></span>` |
| `bi bi-*` | **Bootstrap Icons** | Actions/Content | Edit, Delete, Save | HTML: `<i class="bi bi-X"></i>` |

**âš ï¸ CRITICAL:** Never mix them! OpenIconic for nav, Bootstrap for actions.

---

## ðŸ”˜ Button Size & Style Quick Reference

```
â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
â”‚ CONTEXT                    â”‚ CLASS               â”‚
â”œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”¤
â”‚ Header (Back)              â”‚ btn outline-secondary btn-sm â”‚
â”‚ Header (Refresh)           â”‚ btn btn-success btn-sm       â”‚
â”‚ Filter Apply/Clear         â”‚ btn btn-primary              â”‚
â”‚         (NO btn-sm)        â”‚ btn btn-danger               â”‚
â”‚ Grid Actions (Edit/Delete) â”‚ btn btn-sm btn-primary       â”‚
â”‚         (NO btn-sm)        â”‚ btn btn-sm btn-danger        â”‚
â”‚ Form Save/Cancel           â”‚ btn btn-primary              â”‚
â”‚         (NO btn-sm)        â”‚ btn btn-secondary            â”‚
â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
```

**Rule:** If button in GRID â†’ add `btn-sm` | If button in FORM/HEADER â†’ NO `btn-sm`

---

## ðŸ“‹ Copy-Paste Templates

### 1ï¸âƒ£ BACK BUTTON - Always use this exactly

```html
<button type="button" class="btn btn-outline-secondary btn-sm" @onclick="NavigateBack" title="Go back">
    <span class="oi oi-arrow-left"></span> Back
</button>
```

âœ… What's special:
- OpenIconic `oi-arrow-left` (NOT `bi-arrow-left`)
- `btn-outline-secondary` (NOT solid `btn-secondary`)
- ALWAYS `btn-sm`

---

### 2ï¸âƒ£ GRID ACTION BUTTONS - Edit & Delete

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

âœ… What's special:
- ICON ONLY (no text in grids)
- Always `btn-sm btn-primary` and `btn-sm btn-danger`
- Wrapped in `btn-group btn-group-sm`
- Title includes what you're editing/deleting

---

### 3ï¸âƒ£ FORM BUTTONS - Save & Cancel

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

âœ… What's special:
- Spinner replaces icon when saving
- Icon `me-2` (margin before text)
- NO `btn-sm` (full size buttons)
- Both buttons disabled during save

---

### 4ï¸âƒ£ FILTER BUTTONS - Apply & Clear

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

âœ… What's special:
- NO `btn-sm` (full size)
- Icon `me-1` (margin before text)
- `bi-funnel-fill` with `-fill` suffix
- `bi-x-circle-fill` with `-fill` suffix

---

### 5ï¸âƒ£ TEXT INPUT - With Validation

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

âœ… What's special:
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

### 6ï¸âƒ£ DELETE CONFIRMATION MODAL

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

âœ… What's special:
- BOTH `modal` and `d-block` classes required
- `style="background-color: rgba(0,0,0,0.5);"` (NOT `display: block`)
- `border-0` on header AND footer
- Title: Always "Confirm Deletion"
- Warning in red: "This action cannot be undone."
- All buttons disabled during delete

---

### 7ï¸âƒ£ DATA GRID / TABLE

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

âœ… What's special:
- ALWAYS `table-dark` (black header)
- ALWAYS `table-striped table-hover`
- ALWAYS `@key="item.Id"` (performance)
- ALWAYS wrap in `table-responsive`

---

## ðŸŽ¨ Color Code Reference

| Color | Hex | Button Class | When to Use |
|-------|-----|--------------|------------|
| ðŸ”µ Blue | `#0d6efd` | `btn-primary` | Create, Edit, Save |
| ðŸŸ© Green | `#198754` | `btn-success` | Refresh, Approve |
| ðŸ”´ Red | `#dc3545` | `btn-danger` | Delete, Clear, Reject |
| ðŸŸ¦ Gray Fill | `#6c757d` | `btn-secondary` | Cancel |
| âšª Gray Outline | `#6c757d` | `btn-outline-secondary` | Back navigation |
| ðŸ”· Light Blue | `#0dcaf0` | `btn-info` | View, Download |

---

## âœ… Pre-Flight Checklist

Before submitting a page, verify:

### Buttons
- [ ] Back button: `oi-arrow-left` (OpenIconic, NOT `bi-`)
- [ ] Back button: `btn-outline-secondary btn-sm` (outlined, small)
- [ ] Grid buttons: `btn-sm btn-primary` and `btn-sm btn-danger` (small)
- [ ] Form buttons: `btn btn-primary` and `btn btn-secondary` (NO `btn-sm`)
- [ ] All buttons have `title` attribute
- [ ] Icons use `me-1` or `me-2` spacing

### Forms
- [ ] Required fields have `*` in label
- [ ] Required fields have `@onblur` validation
- [ ] Required fields have `ValidationFeedbackMessage` component
- [ ] Required fields have `required` attribute
- [ ] Form uses `FieldValidationCss.FormControl()` or `.FormSelect()`
- [ ] Validation errors have specific messages
- [ ] Buttons disabled during save: `disabled="@IsSaving"`

### Grids
- [ ] Header: `class="table-dark"`
- [ ] Rows: `@key="item.Id"` on `<tr>`
- [ ] Table: `table-responsive` wrapper
- [ ] Table: `table-striped table-hover`
- [ ] Actions: icon-only (NO text in grids)

### Modals
- [ ] Classes: `modal fade show d-block`
- [ ] Style: `style="background-color: rgba(0,0,0,0.5);" ` (NOT display: block)
- [ ] Dialog: `modal-dialog-centered`
- [ ] Header/Footer: `border-0` (no border)
- [ ] Title: "Confirm Deletion"
- [ ] Warning: Red text "This action cannot be undone."
- [ ] All buttons: `disabled="@IsDeleting"`

---

## ðŸ”¥ Common Mistakes (Don't Do These!)

```
âŒ Use bi-arrow-left for back button
âœ… Use oi-arrow-left instead

âŒ Back button with btn-secondary (solid)
âœ… Back button with btn-outline-secondary (outlined)

âŒ Grid buttons without btn-sm
âœ… Grid buttons with btn-sm btn-primary

âŒ Form buttons with btn-sm
âœ… Form buttons without btn-sm (full size)

âŒ Modal style="display: block;"
âœ… Modal style="background-color: rgba(0,0,0,0.5);"

âŒ Delete modal missing d-block class
âœ… Delete modal with "modal fade show d-block"

âŒ Validation errors in permanent state
âœ… Validation errors clear: _fieldErrors.Remove(fieldId)

âŒ Grid buttons with text like "Edit Item"
âœ… Grid buttons icon-only: just <i class="bi bi-pencil"></i>

âŒ Form without specific error messages
âœ… Form with messages like "Name is required."

âŒ Input field without required attribute
âœ… Input field with required attribute AND @onblur
```

---

## ðŸš€ Lightning Speed Lookup

**Q: How do I style the back button?**
A: Copy template #1 above. Use `oi-arrow-left` and `btn-outline-secondary btn-sm`.

**Q: How do I validate a form field?**
A: Copy template #5. Use `@onblur`, `FieldValidationCss.FormControl()`, and `ValidationFeedbackMessage`.

**Q: How do I make grid action buttons?**
A: Copy template #2. Use `btn-sm btn-primary` and `btn-sm btn-danger`, icon-only.

**Q: How do I show a delete confirmation?**
A: Copy template #6. Need `modal fade show d-block` and warning text in red.

**Q: What's the table structure?**
A: Copy template #7. Must have `table-dark`, `@key="item.Id"`, and `table-responsive` wrapper.

**Q: What color for [action]?**
A: See Color Code Reference above. Primary (blue) for main actions, danger (red) for delete.

---

## ðŸ“ž Still Confused?

1. Find your page in `Immo/Pages/`
2. Look at `ContactRole.razor` - it has EVERYTHING!
3. Copy what you need from this guide
4. Run the app and verify it looks right

**Remember:** When in doubt, copy ContactRole.razor - it's the reference standard!

---

## Documentation

- 🏠 [Main Documentation](../README.md) — Project overview and requirements

---

**Â© 2026 AdminSense. All rights reserved.**
