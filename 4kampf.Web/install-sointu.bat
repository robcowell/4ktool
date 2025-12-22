@echo off
REM Sointu Installation Script for 4kampf Web (Windows Batch)
REM This script helps install Sointu for music synthesis on Windows

echo === Sointu Installation for 4kampf Web ===
echo.

REM Check if Go is installed
where go >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Go is not installed
    echo.
    echo Please install Go first:
    echo   Download from: https://go.dev/dl/
    echo   Or use Chocolatey: choco install golang
    exit /b 1
)

go version
echo.

REM Check if Sointu is already installed
where sointu-compile >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    echo [OK] Sointu is already installed
    exit /b 0
)

echo Sointu is not installed. Installing...
echo.

REM Determine installation directory
set "INSTALL_DIR=%USERPROFILE%\.local\bin"
if not exist "%INSTALL_DIR%" mkdir "%INSTALL_DIR%"

set "USER_BIN_DIR=%USERPROFILE%\bin"
if not exist "%USER_BIN_DIR%" mkdir "%USER_BIN_DIR%"

REM Clone Sointu repository
set "TEMP_DIR=%TEMP%\sointu-install-%RANDOM%"
mkdir "%TEMP_DIR%"

echo Cloning Sointu repository...
git clone https://github.com/vsariola/sointu.git "%TEMP_DIR%\sointu"
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Failed to clone Sointu repository
    rmdir /s /q "%TEMP_DIR%"
    exit /b 1
)

cd "%TEMP_DIR%\sointu"

echo Building Sointu...
go build -o "%INSTALL_DIR%\sointu-compile.exe" ./cmd/sointu-compile
go build -o "%INSTALL_DIR%\sointu-play.exe" ./cmd/sointu-play
go build -o "%USER_BIN_DIR%\sointu-compile.exe" ./cmd/sointu-compile
go build -o "%USER_BIN_DIR%\sointu-play.exe" ./cmd/sointu-play

if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Failed to build Sointu
    cd /d "%~dp0"
    rmdir /s /q "%TEMP_DIR%"
    exit /b 1
)

REM Clean up
cd /d "%~dp0"
rmdir /s /q "%TEMP_DIR%"

echo.
echo [OK] Sointu installed to: %INSTALL_DIR%
echo      Also installed to: %USER_BIN_DIR%
echo.
echo Add to PATH:
echo   1. Open System Properties ^> Environment Variables
echo   2. Add to User PATH: %USER_BIN_DIR%
echo.
echo Verify installation:
echo   sointu-compile.exe --version
echo   sointu-play.exe --version

