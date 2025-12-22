#!/bin/bash
# Build script for Sointu WebAssembly module
# This script clones Sointu, builds it to WASM, and copies it to the project

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"
SOINTU_DIR="$PROJECT_ROOT/sointu"
WASM_OUTPUT="$SCRIPT_DIR/wwwroot/wasm/sointu.wasm"
WASM_EXEC_OUTPUT="$SCRIPT_DIR/wwwroot/js/wasm_exec.js"

echo "Building Sointu to WebAssembly..."
echo "Project root: $PROJECT_ROOT"
echo "Sointu directory: $SOINTU_DIR"
echo "WASM output: $WASM_OUTPUT"

# Check if Go is installed
if ! command -v go &> /dev/null; then
    echo "Error: Go is not installed."
    echo "Please install Go 1.21 or later from https://go.dev/dl/"
    exit 1
fi

# Check Go version
GO_VERSION=$(go version | awk '{print $3}' | sed 's/go//')
echo "Found Go version: $GO_VERSION"

# Clone Sointu if it doesn't exist
if [ ! -d "$SOINTU_DIR" ]; then
    echo "Cloning Sointu repository..."
    cd "$PROJECT_ROOT"
    git clone https://github.com/vsariola/sointu.git
else
    echo "Sointu directory exists, updating..."
    cd "$SOINTU_DIR"
    git pull
fi

# Ensure wasm directory exists
mkdir -p "$(dirname "$WASM_OUTPUT")"

# Build for WebAssembly
echo "Building Sointu for WebAssembly..."
cd "$SOINTU_DIR"

# Set Go environment for WASM
export GOOS=js
export GOARCH=wasm

# Check if sointu-wasm command exists (WASM-specific build with exported functions)
if [ -d "./cmd/sointu-wasm" ]; then
    echo "Building sointu-wasm (with exported functions)..."
    go build -o "$WASM_OUTPUT" ./cmd/sointu-wasm
elif [ -d "./cmd/sointu-play" ]; then
    echo "Building sointu-play (CLI version, no exported functions)..."
    echo "Warning: This will build a CLI program, not a library with exported functions."
    echo "For real-time synthesis, use sointu-wasm instead."
    go build -o "$WASM_OUTPUT" ./cmd/sointu-play
elif [ -d "./cmd/sointu" ]; then
    echo "Building sointu (CLI version, no exported functions)..."
    echo "Warning: This will build a CLI program, not a library with exported functions."
    echo "For real-time synthesis, use sointu-wasm instead."
    go build -o "$WASM_OUTPUT" ./cmd/sointu
else
    echo "Error: Could not find sointu-wasm, sointu-play, or sointu command directory"
    echo "Available commands:"
    ls -la ./cmd/ 2>/dev/null || echo "No cmd directory found"
    exit 1
fi

# Copy Go WASM runtime if it exists
GOROOT=$(go env GOROOT)
# Try multiple possible locations (Go version differences)
WASM_EXEC_SRC=""
for path in "$GOROOT/misc/wasm/wasm_exec.js" "$GOROOT/lib/wasm/wasm_exec.js"; do
    if [ -f "$path" ]; then
        WASM_EXEC_SRC="$path"
        break
    fi
done

# Also try finding it
if [ -z "$WASM_EXEC_SRC" ]; then
    WASM_EXEC_SRC=$(find "$GOROOT" -name "wasm_exec.js" 2>/dev/null | head -1)
fi

if [ -n "$WASM_EXEC_SRC" ] && [ -f "$WASM_EXEC_SRC" ]; then
    echo "Copying Go WASM runtime from $WASM_EXEC_SRC..."
    cp "$WASM_EXEC_SRC" "$WASM_EXEC_OUTPUT"
    echo "Copied wasm_exec.js to $WASM_EXEC_OUTPUT"
else
    echo "Warning: wasm_exec.js not found in Go installation"
    echo "You may need to download it from: https://github.com/golang/go/blob/master/misc/wasm/wasm_exec.js"
    echo "Or it may not be required for your Go version"
fi

# Verify WASM file was created
if [ -f "$WASM_OUTPUT" ]; then
    WASM_SIZE=$(du -h "$WASM_OUTPUT" | cut -f1)
    echo "✓ Successfully built Sointu WASM module"
    echo "  Location: $WASM_OUTPUT"
    echo "  Size: $WASM_SIZE"
else
    echo "Error: WASM file was not created"
    exit 1
fi

echo ""
echo "Build complete! The WASM module is ready at:"
echo "  $WASM_OUTPUT"
echo ""
echo "Note: The actual Sointu WASM build may require additional modifications"
echo "to export the functions expected by sointu-wasm-interop.js"
echo "See SOINTU_WASM.md for details."

