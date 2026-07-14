# Formats a single .cs file after an agent edit (Claude Code PostToolUse / Cursor afterFileEdit).
# Fail-open: never block the agent. Product rules (DB, Mollie, Adminsence) live in AGENTS/SPECs — not here.
$ErrorActionPreference = 'Continue'

function Get-EditedPath([object]$payload) {
    if ($null -eq $payload) { return $null }
    if ($payload.file_path) { return [string]$payload.file_path }
    if ($payload.path) { return [string]$payload.path }
    if ($payload.tool_input -and $payload.tool_input.file_path) {
        return [string]$payload.tool_input.file_path
    }
    return $null
}

try {
    $raw = [Console]::In.ReadToEnd()
    if ([string]::IsNullOrWhiteSpace($raw)) { exit 0 }

    $payload = $raw | ConvertFrom-Json
    $filePath = Get-EditedPath $payload
    if ([string]::IsNullOrWhiteSpace($filePath)) { exit 0 }

    $ext = [IO.Path]::GetExtension($filePath)
    if ($ext -ne '.cs') { exit 0 }

    if (-not (Test-Path -LiteralPath $filePath)) { exit 0 }

    $repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
    $full = [IO.Path]::GetFullPath($filePath)
    $root = [IO.Path]::GetFullPath($repoRoot).TrimEnd('\')
    if (-not $full.StartsWith($root, [StringComparison]::OrdinalIgnoreCase)) { exit 0 }

    $relative = $full.Substring($root.Length).TrimStart('\', '/')
    $sln = Join-Path $repoRoot 'WebShopABMATIC.sln'
    if (-not (Test-Path -LiteralPath $sln)) { exit 0 }

    $dotnet = Get-Command dotnet -ErrorAction SilentlyContinue
    if (-not $dotnet) { exit 0 }

    # Whitespace only — fast enough for a per-edit hook; full style/analyzers stay manual/CI.
    & dotnet format whitespace $sln --include $relative --no-restore --verbosity quiet 2>$null | Out-Null
}
catch {
    # Fail open
}
exit 0
