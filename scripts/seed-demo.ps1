# Demo domain data only (SQL). Idempotent — safe to re-run.
# Usage: .\scripts\seed-demo.ps1 [-Server MULLER] [-Database WebShopABMATIC]

param(
    [string]$Server = "MULLER",
    [string]$Database = "WebShopABMATIC"
)

$ErrorActionPreference = "Stop"

Write-Host "Running demo seed on $Server / $Database ..."
sqlcmd -S $Server -E -d $Database -i (Join-Path $PSScriptRoot "seeds.sql")
if ($LASTEXITCODE -ne 0) {
    throw "seeds.sql failed (exit $LASTEXITCODE)."
}

Write-Host "Demo seed completed."
