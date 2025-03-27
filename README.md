# Project Videogame: SynapseRunner

![Logo](logo.png)

## A 3D endless runner with procedural generation

Set in a cyberpunk world with a low-poly visual style reminiscent of the PlayStation 2 era.  
Made in Unity.

**Jesús Ángel Neri Hernández** | **Jorge Guzmán Cabrera**  
**Universidad Iberoamericana León**  
Interactive Digital Design  
**Course:** Programación de Interacción  
**Instructor:** Prof. Enrique Kato Romo  
**Date:** March 2025

---

## Table of Contents

- [Introduction](#introduction)
- [Game Concept](#game-concept)
  - [Genre and Theme](#genre-and-theme)
  - [Graphic Style](#graphic-style)
  - [Gameplay Mechanics](#gameplay-mechanics)
  - [Background](#background)
  - [Gameplay Details](#gameplay-details)
- [Visual Style](#visual-style)
- [Player](#player)
- [Collaborative Development Workflow](#collaborative-development-workflow)
- [Collaborative Development with GitHub](#collaborative-development-with-github)
- [Development Journal](#development-journal)

---

## Introduction

This document serves as the paper that will contain the content of this project for the Interaction Programming class. It includes the game concept, game logic, and challenges encountered throughout development.  
Tools: Unity (version 6000.0.36f1) and Maya 3D.

---

## Game Concept

A first-person 3D action game where the player controls an apostle who must dodge obstacles, collect mechaparts, and survive as long as possible in an infinitely procedurally generated hallway. The game is set in a cyberpunk world with a hacker aesthetic and low-poly visuals.

### Genre and Theme

**Genre:** Action  
**Theme:** Low-poly, retro, with cyberpunk elements (CRT monitors, lines of code, digital interfaces), and a dark yet colorful atmosphere.

### Graphic Style

- Cyberpunk
- Noir Futurista
- Estética Hacker
- **Inspirations:**
  - _Ghost in the Shell_ (1995)
  - Low Poly (2000)

### Gameplay Mechanics

#### Sequence

- **Structure:** Linear (infinite hallway)
- **Players:** Single-player

#### Technical Aspects

- **3D Game**
- **Camera System:** First Person

### Background

#### Story Synopsis

In a dystopian future, humanity is enslaved by an artificial intelligence named EVA. You are an apostle, chosen to restore faith and free humanity. Navigate an endless hallway with divine abilities and collect mechaparts to upgrade and survive.

### Gameplay Details

#### Objective

- Survive as long as possible
- Collect mechaparts to upgrade abilities

#### How to Win / Lose

- **Lose** if you hit an obstacle or get caught by enemies

#### Game Modes

- Single-player (endless runner)

#### Game Structure

- Procedurally generated infinite hallway
- Difficulty increases over time
- Player upgrades weapon with collected parts

#### Basic Controls

- `WASD`: Movement
- `Mouse`: Shooting

#### Ending

The game has no defined ending; it's based on personal scores.

#### Why is this Fun?

- Easy to learn, hard to master
- Procedural uniqueness
- Aesthetic atmosphere
- Progression through upgrades

---

## Visual Style

### 3D Objects

Old-school poly count inspirations.

---

## Player

### Movement

- Player moves on the **X axis** to avoid obstacles
- Cannot move forward/backward
- **Weapon** allows player to jump

---

## Collaborative Development Workflow

To collaborate efficiently, we use Git. Here's the workflow:

1. **Pull the latest changes:**

   ```bash
   git pull
   ```

2. **Work on your tasks in Unity**

3. **Stage and commit changes:**

   ```bash
   git add .
   git commit -m "Implemented X feature / Fixed Y bug"
   ```

4. **Push to remote:**

   ```bash
   git push
   ```

5. **Your teammate pulls before starting work:**
   ```bash
   git pull
   ```

---

### Working with Feature Branches

1. **Create branch:**

   ```bash
   git checkout -b feature-branch-name
   ```

2. **Work and test locally**

3. **Commit and push:**

   ```bash
   git add .
   git commit -m "Implemented/tested feature X"
   git push origin feature-branch-name
   ```

4. **Open Pull Request on GitHub**

---

### Merging a Feature Branch

1. Checkout main:
   ```bash
   git checkout main
   git pull
   git merge feature-branch-name
   git push
   ```

Or via GitHub:

- Open repo > Pull Requests > New Pull Request
- Review > Merge

---

## Collaborative Development with GitHub

### Daily Workflow

1. `git pull`
2. `git checkout -b feature-branch-name`
3. Work & test in Unity
4. `git add .`
5. `git commit -m "feat: add jump mechanic"`
6. `git push origin feature-branch-name`
7. Open a Pull Request

### Merging via Pull Request

- Go to GitHub
- New PR > Add description > Create > Merge

### Commit Message Format

- `feat:` New feature
- `fix:` Bug fix
- `refactor:` Code change
- `style:` Formatting
- `docs:` Documentation
- `chore:` Maintenance

### Tracking Progress

- Use GitHub Issues for bugs/tasks
- Reference in commits (`fix: resolve camera bug #7`)
- Use Projects/Milestones

### Repository

**URL:**  
[https://github.com/jesusneri1024/SynapseRunner](https://github.com/jesusneri1024/SynapseRunner)

**Clone:**

```bash
git clone https://github.com/jesusneri1024/SynapseRunner.git
```

**Add Collaborator:**

- Settings > Collaborators
- Add username with Write/Admin access

### Weekly Sync Guidelines

- Review PRs
- Merge tested branches
- Update development journal

### Why Version Control?

- Enables team collaboration
- Rollback capabilities
- Full documentation of changes

---

## Handling Large Files with ExternalAssets

### What Goes in `ExternalAssets`

- 3D Models (.fbx, .obj)
- Audio (.wav, .mp3)
- Textures (.png, .tga)

### Not Synced with GitHub

- The folder is listed in `.gitignore`
- Files are manually downloaded via links in `README.txt`

### AssetVerifier Script

- Warns about missing files
- Updates `README.txt` with links
- Lists all external assets

### Adding New Assets

1. Place file in `Assets/ExternalAssets`
2. Run Unity to trigger AssetVerifier
3. Update `README.txt` with download link
4. Notify teammates

---

## Development Journal

### Tuesday, March 25

- Implemented player movement script
- Created shooting mechanic
- Built a provisional test scene

### Thursday, March 27

- Set up GitHub repo: [https://github.com/jesusneri1024/SynapseRunner](https://github.com/jesusneri1024/SynapseRunner)
- Progress:
  - `feat:` Add recoil shooting + test level
  - `chore:` Attach Rigidbody to player
  - `feat:` Import character & camera movement scripts
  - `feat:` AssetVerifier: auto update & file detection
  - `docs:` Update `.gitignore` for `ExternalAssets`

---
