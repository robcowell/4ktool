@echo off
REM Build script for Sointu WebAssembly module (Windows)
REM This script clones Sointu, builds it to WASM, and copies it to the project

setlocal enabledelayedexpansion

set SCRIPT_DIR=%~dp0
set PROJECT_ROOT=%SCRIPT_DIR%..
set SOINTU_DIR=%PROJECT_ROOT%\sointu
set WASM_OUTPUT=%SCRIPT_DIR%wwwroot\wasm\sointu.wasm
set WASM_EXEC_OUTPUT=%SCRIPT_DIR%wwwroot\js\wasm_exec.js

echo Building Sointu to WebAssembly...
echo Project root: %PROJECT_ROOT%
echo Sointu directory: %SOINTU_DIR%
echo WASM output: %WASM_OUTPUT%

REM Check if Go is installed
where go >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Error: Go is not installed.
    echo Please install Go 1.21 or later from https://go.dev/dl/
    exit /b 1
)

REM Check Go version
for /f "tokens=3" %%i in ('go version') do set GO_VERSION=%%i
echo Found Go version: %GO_VERSION%

REM Clone Sointu if it doesn't exist
if not exist "%SOINTU_DIR%" (
    echo Cloning Sointu repository...
    cd /d "%PROJECT_ROOT%"
    git clone https://github.com/vsariola/sointu.git
) else (
    echo Sointu directory exists, updating...
    cd /d "%SOINTU_DIR%"
    git pull
)

REM Ensure wasm directory exists
if not exist "%SCRIPT_DIR%wwwroot\wasm" mkdir "%SCRIPT_DIR%wwwroot\wasm"

REM Build for WebAssembly
echo Building Sointu for WebAssembly...
cd /d "%SOINTU_DIR%"

REM Set Go environment for WASM
set GOOS=js
set GOARCH=wasm

REM Check if sointu-play command exists
if exist "cmd\sointu-play" (
    echo Building sointu-play...
    go build -o "%WASM_OUTPUT%" ./cmd/sointu-play
) else if exist "cmd\sointu" (
    echo Building sointu...
    go build -o "%WASM_OUTPUT%" ./cmd/sointu
) else (
    echo Error: Could not find sointu-play or sointu command directory
    echo Available commands:
    dir /b cmd 2>nul
    exit /b 1
)

REM Copy Go WASM runtime if it exists
for /f "tokens=*" %%i in ('go env GOROOT') do set GOROOT=%%i
set WASM_EXEC_SRC=%GOROOT%\misc\wasm\wasm_exec.js
if exist "%WASM_EXEC_SRC%" (
    echo Copying Go WASM runtime...
    copy "%WASM_EXEC_SRC%" "%WASM_EXEC_OUTPUT%" >nul
    echo Copied wasm_exec.js to %WASM_EXEC_OUTPUT%
) else (
    echo Warning: wasm_exec.js not found at %WASM_EXEC_SRC%
    echo You may need to load it manually or it may not be required
)

REM Verify WASM file was created
if exist "%WASM_OUTPUT%" (
    echo Successfully built Sointu WASM module
    echo   Location: %WASM_OUTPUT%
) else (
    echo Error: WASM file was not created
    exit /b 1
)

echo.
echo Build complete! The WASM module is ready at:
echo   %WASM_OUTPUT%
echo.
echo Note: The actual Sointu WASM build may require additional modifications
echo to export the functions expected by sointu-wasm-interop.js
echo See SOINTU_WASM.md for details.

