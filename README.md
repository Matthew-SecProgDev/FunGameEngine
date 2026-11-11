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
│       ├── Assets/             # Game textures, audio, and other assets
│       └── Content/            # MonoGame content pipeline project
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

The project was developed on Windows. The following commands describe the intended setup from the repository root; they have **not yet been independently verified on a clean machine**.

### Clone and build

```powershell
git clone https://github.com/Matthew-SecProgDev/FunGameEngine.git
cd FunGameEngine

dotnet restore Fun.sln
dotnet build Fun.sln -c Debug
```

### Run the sample game

```powershell
dotnet run --project src/Fun.SimpleGame/Fun.SimpleGame.csproj -c Debug
```

You can also open `Fun.sln` in Visual Studio and choose **Fun.SimpleGame** as the startup project.

The sample game uses the MonoGame content pipeline and a local .NET tool manifest under `src/Fun.SimpleGame/.config/`. If content or tool restoration fails on a clean checkout, verify the MonoGame Content Builder setup before running. The commands above are a starting point, not a claim of completed cross-machine verification.

## Current Status and Scope

This project is an evolving gameplay/engine programming exercise. Its goals are to explore combat and gameplay mechanics, separate reusable engine code from game-specific code, and improve architecture through practical implementation.

It is **not** a complete commercial game. More automated tests, build verification, documentation, and code-quality improvements remain future work.

## Assets and Attribution

This repository contains third-party artwork and audio used for development and demonstration. Ownership and reuse rights for those assets may differ from the source code. An attribution list with original sources and applicable licenses still needs to be completed; **do not assume these assets are free for unrestricted reuse**.

## Portfolio Context

This project demonstrates my experience integrating gameplay systems in C# and MonoGame.
