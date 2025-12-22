# Sointu Installation Script for 4kampf Web (Windows PowerShell)
# This script helps install Sointu for music synthesis on Windows

Write-Host "=== Sointu Installation for 4kampf Web ===" -ForegroundColor Cyan
Write-Host ""

# Check if Go is installed
try {
    $goVersion = go version 2>&1
    Write-Host "✓ Go is installed: $goVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Go is not installed" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please install Go first:"
    Write-Host "  Download from: https://go.dev/dl/"
    Write-Host "  Or use Chocolatey: choco install golang"
    exit 1
}

# Check if Sointu is already installed
$sointuCompile = Get-Command sointu-compile -ErrorAction SilentlyContinue
if ($sointuCompile) {
    Write-Host "✓ Sointu is already installed" -ForegroundColor Green
    Write-Host "  Location: $($sointuCompile.Source)"
    exit 0
}

Write-Host "Sointu is not installed. Installing..." -ForegroundColor Yellow
Write-Host ""

# Determine installation directory
$installDir = Join-Path $env:USERPROFILE ".local\bin"
if (-not (Test-Path $installDir)) {
    New-Item -ItemType Directory -Path $installDir -Force | Out-Null
}

# Alternative: Use a directory in PATH
$pathDirs = $env:PATH -split ';'
$userBinDir = Join-Path $env:USERPROFILE "bin"
if (-not (Test-Path $userBinDir)) {
    New-Item -ItemType Directory -Path $userBinDir -Force | Out-Null
}

# Clone Sointu repository
$tempDir = Join-Path $env:TEMP "sointu-install-$(Get-Random)"
New-Item -ItemType Directory -Path $tempDir -Force | Out-Null

Write-Host "Cloning Sointu repository..."
try {
    git clone https://github.com/vsariola/sointu.git $tempDir\sointu
} catch {
    Write-Host "❌ Failed to clone Sointu repository" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Red
    Remove-Item -Path $tempDir -Recurse -Force -ErrorAction SilentlyContinue
    exit 1
}

Set-Location "$tempDir\sointu"

Write-Host "Building Sointu..."
try {
    go build -o "$installDir\sointu-compile.exe" ./cmd/sointu-compile
    go build -o "$installDir\sointu-play.exe" ./cmd/sointu-play
    
    # Also build to user bin directory
    go build -o "$userBinDir\sointu-compile.exe" ./cmd/sointu-compile
    go build -o "$userBinDir\sointu-play.exe" ./cmd/sointu-play
} catch {
    Write-Host "❌ Failed to build Sointu" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Red
    Set-Location $PSScriptRoot
    Remove-Item -Path $tempDir -Recurse -Force -ErrorAction SilentlyContinue
    exit 1
}

# Clean up
Set-Location $PSScriptRoot
Remove-Item -Path $tempDir -Recurse -Force

Write-Host ""
Write-Host "✓ Sointu installed to: $installDir" -ForegroundColor Green
Write-Host "  Also installed to: $userBinDir" -ForegroundColor Green
Write-Host ""
Write-Host "Add to PATH:" -ForegroundColor Yellow
Write-Host "  1. Open System Properties > Environment Variables"
Write-Host "  2. Add to User PATH: $userBinDir"
Write-Host ""
Write-Host "Or run in PowerShell (current session only):"
Write-Host "  `$env:PATH += `";$userBinDir`""
Write-Host ""
Write-Host "Verify installation:"
Write-Host "  sointu-compile.exe --version"
Write-Host "  sointu-play.exe --version"

