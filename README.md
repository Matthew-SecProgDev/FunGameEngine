# FunGameEngine

A personal **2D action-game development project** built with **C#**, **.NET 8**, and **MonoGame**. The repository contains a reusable engine library and a sample game demonstrating how its systems work together.

The project focuses on practical gameplay programming and the design of reusable game infrastructure. It is a work in progress, not a commercial game or a production-ready engine.

## Gameplay Demo

[**FunGameEngine Gameplay**](src/docs/demo/FunGameEngine-Gameplay-Preview.gif)

[**Watch the gameplay video**](src/docs/demo/FunGameEngine-Short-Gameplay-Demo.mp4)

The recording shows the sample game's arena gameplay, character movement, enemies, combat effects, and on-screen HUD. The video is a short highlight, not a demonstration of every implemented feature.

## Features

### Gameplay (`Fun.SimpleGame`)

- Player movement, attacks, projectiles, and combat interactions
- Enemy behavior and configuration-driven enemy spawning
- Collision handling for game objects, attacks, and projectiles
- Sprite animations and combat effects
- Inventory and collectible-item logic
- Health indicators and other HUD elements

### Reusable infrastructure (`Fun.Engine`)

- Game-state management and update/draw lifecycle
- Camera and rendering helpers
- Input processing
- Animation support
- Collision-detection infrastructure
- Event-based communication between systems
- Audio, particle effects, and resource handling
- User-interface components

## Repository Structure

```text
FunGameEngine/
├── src/
│   ├── Fun.Engine/             # Reusable MonoGame engine components
│   └── Fun.SimpleGame/         # Sample game and gameplay-specific code
│       └── Assets/             # Game textures, audio, and other assets
├── docs/
│   └── demo/
│       ├── FunGameEngine-Gameplay-Demo.mp4
│       ├── FunGameEngine-Short-Gameplay-Demo.mp4
│       └── FunGameEngine-Gameplay-Preview.gif
├── Fun.sln
└── README.md
```

## Technologies

- **Language:** C#
- **Framework:** .NET 8
- **Game framework:** MonoGame DesktopGL 3.8.1.303
- **Content pipeline:** MonoGame Content Builder

## Getting Started

### Requirements

- .NET 8 SDK
- A system supported by MonoGame DesktopGL, with the necessary graphics/runtime dependencies

The project was developed on Windows. Run the commands below from the repository root in a regular PowerShell terminal (or another terminal with the .NET SDK available). The build commands have been tested locally in PowerShell, but setup has **not yet been independently verified on a clean machine**.

### Clone and build

```powershell
git clone https://github.com/Matthew-SecProgDev/FunGameEngine.git
cd FunGameEngine

dotnet restore Fun.sln
dotnet build Fun.sln -c Debug --no-restore
```

### Run the sample game

After building, the intended launch command from the repository root is:

```powershell
dotnet run --project src/Fun.SimpleGame/Fun.SimpleGame.csproj -c Debug
```

**Current run-time limitation:** The sample requires a `MainConfig.json` configuration file. At present, launching the project from the repository root can fail with `FileNotFoundException` because the application looks for `MainConfig.json` in the current working directory. Before the command above can be considered reliable for a fresh clone, the project needs to include a suitable non-sensitive configuration file and load it from a stable location (for example, by copying it to the build output and resolving it relative to `AppContext.BaseDirectory`). The build and run instructions are therefore **not yet fully verified end to end**.

You can also open `Fun.sln` in Visual Studio and choose **Fun.SimpleGame** as the startup project. Its working directory and configuration-file location must still be correct.

### Troubleshooting

**`MSB4126: The specified solution configuration "Debug|x64" is invalid`**

The Visual Studio **x64 Native Tools Command Prompt** may set `Platform=x64` in the environment, while `Fun.sln` does not define that solution configuration. Use regular PowerShell for the commands above. Alternatively, clear the inherited platform for the current Command Prompt session and retry:

```cmd
set Platform=
dotnet restore Fun.sln
dotnet build Fun.sln -c Debug --no-restore
```

Do not add `-p:Platform="Any CPU"` to the default instructions unless that solution platform is confirmed and needed; ordinary PowerShell builds do not require it in the tested environment.

**MonoGame content pipeline or local-tool errors**

The sample uses the MonoGame content pipeline and has a local .NET tool manifest under `src/Fun.SimpleGame/.config/`. If a clean checkout reports missing content-building tools, review the project’s MonoGame tooling setup and restore the tools from the appropriate manifest directory before rebuilding.

**`NU1903` warning for `System.Text.Json`**

A local restore reported a high-severity security advisory for `System.Text.Json` version `8.0.4`. This is separate from the `MainConfig.json` run-time failure. The affected dependency should be updated to a compatible patched version, including checking whether it is direct or transitive. Until that is resolved, a successful build may still display package-audit warnings.

## Current Status and Scope

This project is an evolving gameplay/engine programming exercise. Its goals are to explore combat and gameplay mechanics, separate reusable engine code from game-specific code, and improve architecture through practical implementation.

It is **not** a complete commercial game. More automated tests, build verification, documentation, and code-quality improvements remain future work.

## Assets and Attribution

This repository contains third-party artwork and audio used for development and demonstration. Ownership and reuse rights for those assets may differ from the source code. An attribution list with original sources and applicable licenses still needs to be completed; **do not assume these assets are free for unrestricted reuse**.

## Portfolio Context

This project demonstrates my experience integrating gameplay systems in C# and MonoGame.
