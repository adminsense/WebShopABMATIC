# Apply pending schema + demo seed + Identity users (one-time / before demo).
# Usage: .\scripts\apply-local-database.ps1 [-Server MULLER] [-SkipSeed] [-SkipIdentitySeed]
param(
    [string]$Server = "MULLER",
    [string]$Database = "WebShopABMATIC",
    [switch]$SkipSeed,
    [switch]$SkipIdentitySeed
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot

Write-Host "Applying pending schema on $Server / $Database ..."
sqlcmd -S $Server -E -d $Database -i (Join-Path $PSScriptRoot "apply-pending-schema.sql")
if ($LASTEXITCODE -ne 0) { throw "apply-pending-schema.sql failed." }

if (-not $SkipSeed) {
    & (Join-Path $PSScriptRoot "seed-demo.ps1") -Server $Server -Database $Database
}

if (-not $SkipIdentitySeed) {
    & (Join-Path $PSScriptRoot "seed-identity.ps1")
}

Write-Host "Database setup completed."
