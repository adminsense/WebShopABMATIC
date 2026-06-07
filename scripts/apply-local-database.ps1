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
    Write-Host "Running demo seed ..."
    sqlcmd -S $Server -E -d $Database -i (Join-Path $PSScriptRoot "seeds.sql")
    if ($LASTEXITCODE -ne 0) { throw "seeds.sql failed." }
}

if (-not $SkipIdentitySeed) {
    Write-Host "Seeding Identity roles, users, and audit demo rows ..."
    Push-Location (Join-Path $root "Web")
    try {
        dotnet run --no-build -- --seed-identity
        if ($LASTEXITCODE -ne 0) {
            dotnet run -- --seed-identity
            if ($LASTEXITCODE -ne 0) { throw "Identity seed failed." }
        }
    }
    finally {
        Pop-Location
    }
}

Write-Host "Database setup completed."
