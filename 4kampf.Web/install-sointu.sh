#!/bin/bash
# Sointu Installation Script for 4kampf Web (macOS/Linux)
# This script helps install Sointu for music synthesis

set -e

echo "=== Sointu Installation for 4kampf Web ==="
echo ""

# Detect OS
if [[ "$OSTYPE" == "darwin"* ]]; then
    OS_NAME="macOS"
elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
    OS_NAME="Linux"
else
    OS_NAME="Unix-like"
fi

echo "Detected OS: $OS_NAME"
echo ""

# Check if Go is installed
if ! command -v go &> /dev/null; then
    echo "❌ Go is not installed"
    echo ""
    echo "Please install Go first:"
    if [[ "$OSTYPE" == "darwin"* ]]; then
        echo "  macOS: brew install go"
    elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
        echo "  Linux: sudo apt install golang  # Debian/Ubuntu"
        echo "         sudo yum install golang   # RHEL/CentOS"
    fi
    echo "  Or download from: https://go.dev/dl/"
    exit 1
fi

echo "✓ Go is installed: $(go version)"
echo ""

# Check if Sointu is already installed
if command -v sointu-compile &> /dev/null; then
    echo "✓ Sointu is already installed"
    sointu-compile --version 2>/dev/null || echo "  (version check not available)"
    exit 0
fi

echo "Sointu is not installed. Installing..."
echo ""

# Determine installation directory (cross-platform)
INSTALL_DIR="${HOME}/.local/bin"
if [ ! -d "$INSTALL_DIR" ]; then
    mkdir -p "$INSTALL_DIR"
    echo "Created directory: $INSTALL_DIR"
fi

# Clone Sointu repository
TEMP_DIR=$(mktemp -d)
cd "$TEMP_DIR"

echo "Cloning Sointu repository..."
if ! git clone https://github.com/vsariola/sointu.git; then
    echo "❌ Failed to clone Sointu repository"
    echo "   Check your internet connection and Git installation"
    rm -rf "$TEMP_DIR"
    exit 1
fi

cd sointu

echo "Building Sointu..."
if ! go build -o "$INSTALL_DIR/sointu-compile" ./cmd/sointu-compile; then
    echo "❌ Failed to build sointu-compile"
    cd /
    rm -rf "$TEMP_DIR"
    exit 1
fi

if ! go build -o "$INSTALL_DIR/sointu-play" ./cmd/sointu-play; then
    echo "❌ Failed to build sointu-play"
    cd /
    rm -rf "$TEMP_DIR"
    exit 1
fi

# Make executables executable (in case permissions are wrong)
chmod +x "$INSTALL_DIR/sointu-compile"
chmod +x "$INSTALL_DIR/sointu-play"

# Clean up
cd /
rm -rf "$TEMP_DIR"

echo ""
echo "✓ Sointu installed to: $INSTALL_DIR"
echo ""

# Detect shell and provide appropriate instructions
SHELL_NAME=$(basename "$SHELL")
if [[ "$SHELL_NAME" == "zsh" ]]; then
    RC_FILE="$HOME/.zshrc"
elif [[ "$SHELL_NAME" == "bash" ]]; then
    RC_FILE="$HOME/.bashrc"
else
    RC_FILE="$HOME/.profile"
fi

echo "Add to PATH:"
echo "  Add this line to $RC_FILE:"
echo "    export PATH=\"\$PATH:$INSTALL_DIR\""
echo ""
echo "Then run:"
echo "  source $RC_FILE"
echo ""
echo "Or for current session only:"
echo "  export PATH=\"\$PATH:$INSTALL_DIR\""
echo ""
echo "Verify installation:"
echo "  sointu-compile --version"
echo "  sointu-play --version"

