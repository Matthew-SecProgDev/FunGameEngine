# FunGameEngine

A personal 2D game-development project built with **C# and MonoGame**, consisting of a reusable engine library and a playable sample game.

The project explores game-engine architecture, gameplay programming, collision detection, animation, event-driven communication, and resource management.

## Features

- **Gameplay:** Player movement, attacks, enemy behavior, and combat.
- **Collision detection:** Collision handling for game objects, attacks, and projectiles.
- **Animation:** Sprite animation and gameplay animation controllers.
- **Audio:** Sound effects and background music playback.
- **Particles:** Particle effects used during gameplay.
- **Inventory:** Collectible items and inventory management.
- **UI:** Heads-up display, health controls, and inventory indicators.
- **Game states:** A state-based architecture for organizing gameplay.
- **Events:** Event-driven communication between game systems.

## Project Structure

```text
src/
├── Fun.Engine/       # Reusable MonoGame engine components
└── Fun.SimpleGame/   # Playable game and game-specific systems
```

### Fun.Engine

Contains reusable systems for rendering, animation, physics and collision detection, input, audio, resource management, events, and user interfaces.

### Fun.SimpleGame

Contains gameplay-specific components, including the player, enemies, combat rules, inventory, HUD, and gameplay states.

## Technologies

- C#
- .NET
- MonoGame

## Getting Started

Clone the repository:

```bash
git https://github.com/Matthew-SecProgDev/FunGameEngine.git
cd FunGameEngine
```

Run the game using the `Fun.SimpleGame` startup project.

*Additional platform requirements and verified launch instructions will be documented after final build testing.*

## Screenshots and Demo

Gameplay screenshots and a short demonstration video will be added here.

## Project Status

This is a personal learning and engineering project, not a commercial game. It focuses on gameplay systems and reusable engine infrastructure rather than original game artwork.

## Third-Party Assets

The game uses third-party artwork and other assets. Their sources and applicable licenses should be listed here before the repository is presented as a portfolio sample.
