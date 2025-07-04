# Minecraft Launcher Application

## Overview
A custom Minecraft launcher application built with .NET 8 and Windows Forms. The launcher automatically checks for updates, downloads required files and mods, and launches the Minecraft client.

## Structure

### Main Components

- **MainForm.cs** - Main window (400x300 pixels, non-resizable)
  - Server name label
  - Progress bar for loading operations
  - Status label for current operations
  - "Play" button
  - Launcher version label

- **GameUpdater.cs** - Handles update checking and file downloads
  - `GetServerVersion()` - Retrieves server version information
  - `DownloadFile()` - Downloads files with progress reporting
  - `CalculateFileHash()` - Calculates SHA256 file hashes
  - `ValidateLocalFiles()` - Validates local files against server

- **MinecraftStarter.cs** - Handles game launching
  - Java detection and version checking
  - Minecraft process launching with proper arguments
  - Game directory management

- **ConfigManager.cs** - Configuration and profile management
  - Launcher configuration persistence
  - Game profile management
  - Directory structure creation

### Models

- **ServerInfo.cs** - Server information model
  - minecraft_version
  - mods_hash
  - client_files (list)
  - mods (list)

- **GameProfile.cs** - Game profile model
  - Game settings and configuration

## Building and Running

### Prerequisites
- .NET 8 SDK
- Windows 10/11

### Building
```bash
dotnet build
```

### Publishing as Single File
```bash
dotnet publish -c Release
```

This will create a single executable file in `bin/Release/net8.0-windows/win-x64/publish/`

## Configuration

The launcher uses the following directory structure:
```
%APPDATA%/.minecraft_custom/
├── versions/
│   └── server-1.20.1/
├── mods/
├── launcher_profiles.json
└── launcher_version.txt
```

## API Integration

The launcher expects a server API endpoint at `/api/version` returning:
```json
{
  "minecraft_version": "1.20.1",
  "mods_hash": "abc123def456",
  "client_files": [
    {
      "name": "minecraft-1.20.1.jar",
      "url": "http://yourserver.com/files/minecraft-1.20.1.jar",
      "hash": "sha256_hash"
    }
  ],
  "mods": [
    {
      "name": "mod1.jar",
      "url": "http://yourserver.com/mods/mod1.jar",
      "hash": "sha256_hash"
    }
  ]
}
```

## Features

- ✅ Automatic update checking
- ✅ File integrity validation (SHA256)
- ✅ Progress tracking during downloads
- ✅ Single file deployment
- ✅ Java detection and validation
- ✅ Configuration management
- ✅ Error handling and user feedback

## Development

The application is structured with proper separation of concerns:
- UI layer (MainForm)
- Service layer (GameUpdater, MinecraftStarter, ConfigManager)
- Model layer (ServerInfo, GameProfile)

All methods include proper error handling and logging capabilities for future enhancement.