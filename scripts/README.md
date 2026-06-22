# Scripts — WebShopABMATIC

All **operational scripts** for local setup, database, and codegen live in this folder.  
Run from repo root, e.g. `.\scripts\apply-local-database.ps1`.

---

## Database (local dev)

| Script | Purpose |
|--------|---------|
| [`apply-local-database.ps1`](apply-local-database.ps1) | **All-in-one:** schema + demo seed (optional legacy identity script) |
| [`apply-pending-schema.sql`](apply-pending-schema.sql) | EF migrations / pending schema on `WebShopABMATIC` |
| [`seeds.sql`](seeds.sql) | Demo domain data (products, orders, stock, images, audit, …) |
| [`seed-demo.ps1`](seed-demo.ps1) | Run `seeds.sql` only |
| [`seed-identity.ps1`](seed-identity.ps1) | **Deprecated** — AspNetUsers seed (not used for login) |
| [`WebShopABMATIC-create-local.sql`](WebShopABMATIC-create-local.sql) | Greenfield English schema (generated) |
| [`ABMATIC-create-local.sql`](ABMATIC-create-local.sql) | Legacy Dutch schema source |

**Typical first setup:**

```powershell
.\scripts\apply-local-database.ps1
```

**Login (demo):** `seeds.sql` inserts `StaffUsers` + `Customers.PasswordWebshop` — see [DATA_DEMO_SEED.md](../readme/DATA_DEMO_SEED.md) §7 (`admin@webshop.com` / `demo`, etc.).

**Re-seed demo data only:**

```powershell
.\scripts\seed-demo.ps1
```

`seed-identity.ps1` is **deprecated** for login — runtime uses legacy `StaffUsers` / `Customers` (`LegacySignInService`).

Inventory of seeded tables: [`readme/DATA_SUMMARY.md`](../readme/DATA_SUMMARY.md) · [`readme/SUNDAY.md`](../readme/SUNDAY.md).

---

## Codegen helpers

| Script | Purpose |
|--------|---------|
| [`generate-from-sql.ps1`](generate-from-sql.ps1) | Regenerate English SQL + EF model from legacy SQL |

> **EF migrations** are never run by the app or by scripts — apply them manually with `dotnet ef database update`.

---

## Convention

- **SQL + PowerShell entry points** → `scripts/`
- **C# seed logic** (Identity, DI) → `Infrastructure/Seeding/` — always called through a script here
- Do not add one-off setup commands only in README without a matching script in this folder

---

**© 2026 AdminSense. All rights reserved.**
