# Seed ASP.NET Identity roles, demo users, and optional audit fallback.
# Usage: .\scripts\seed-identity.ps1
# Requires: schema applied; domain seed recommended first (links customer@ → Customers).

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
$web = Join-Path $root "Web"

if (-not (Test-Path $web)) {
    throw "Web project not found at $web"
}

Write-Host "Seeding Identity (roles + admin/manager/customer demo users) ..."
Push-Location $web
try {
    dotnet run --no-build -- --seed-identity 2>$null
    if ($LASTEXITCODE -ne 0) {
        dotnet run -- --seed-identity
    }
    if ($LASTEXITCODE -ne 0) {
        throw "Identity seed failed (exit $LASTEXITCODE)."
    }
}
finally {
    Pop-Location
}

Write-Host "Identity seed completed."
