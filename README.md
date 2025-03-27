# Project Videogame SynapseRunner

![Logo](logo.png)

**A 3D endless runner with procedural generation, set in a cyberpunk world with a low-poly visual style reminiscent of the PlayStation 2 era made in Unity.**

**Jesús Ángel Neri Hernández | Jorge Guzmán Cabrera**  
**Universidad Iberoamericana León**  
**Interactive Digital Design**  
**Course: Programación de Interacción**  
**Instructor: Prof. Enrique Kato Romo**  
**March 2025**

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

This document serves as the paper that will contain the content of this project for the Interaction Programming class. It will include the concept of the game, logic of the game, and the challenges encountered throughout development.  
The tools used in this project will be Unity (version 6000.0.36f1) and Maya 3D.

---

## Game Concept

A first-person 3D action game where the player controls an apostle who must dodge obstacles, collect mechaparts, and survive as long as possible in an infinitely procedurally generated hallway. The game is set in a cyberpunk world with a hacker aesthetic and low-poly visuals. The apostle can upgrade their abilities and equipment as they progress.

### Genre and Theme

- **Genre:** Action
- **Theme:** Low-poly, retro, with cyberpunk elements (CRT monitors, lines of code, digital interfaces) and a dark yet colorful atmosphere.

### Graphic Style

- Cyberpunk
- Noir Futurista
- Estética Hacker
- Inspirations:
  - _Ghost in the Shell_ (1995)
  - Low Poly (2000)

### Gameplay Mechanics

#### Sequence

- **Structure:** Linear (the player moves forward in an infinitely procedurally generated hallway)
- **Number of Players:** Single-player

#### Technical Aspects

- **3D Game**
- **Camera System:** 3D First Person

### Background

#### Story Synopsis

In a dystopian future, humanity has lost its spiritual connection and is enslaved by an artificial intelligence named EVA. You are an apostle, a chosen one tasked with restoring faith and freeing humanity from the simulation controlled by EVA. Using divine abilities and ancient technology, you must navigate an endless hallway filled with system-generated obstacles and enemies, while collecting mechaparts and upgrading your abilities to complete your mission.

### Gameplay Details

#### Objective

- Survive as long as possible.
- Collect mechaparts to upgrade abilities and unlock equipment.

#### How to Win / Lose

- Lose if you hit an obstacle or are caught by enemies.

#### Game Modes

- Single-player (endless runner).

#### Game Structure

- The game is infinite, with a procedurally generated hallway.
- Difficulty increases over time (more obstacles, faster enemies, etc.).
- No levels, but the player can upgrade their weapon with collected mechaparts.

#### Basic Controls

- **WASD** for movement
- **Mouse** for shooting

#### Ending

The game has no defined ending, but players can compete for their personal best or compare scores with friends.

#### Why is this Fun?

- Easy to learn but hard to master.
- Procedural generation ensures every run is unique.
- The cyberpunk aesthetic with a religious twist creates a unique and captivating atmosphere.
- Progression (upgrades and unlocks) keeps players motivated.

---

## Visual Style

### 3D Objects

On older games the number of polygons that were in the models were usually of X per model.

---

## Player

### Movement

The player can move through the X axis, so it can avoid obstacles, but it cannot go forward nor backward. In addition, the player has a **weapon** that allows them to jump.

---

## Collaborative Development Workflow

To ensure efficient collaboration between team members, a basic version control workflow using Git was established.

### Git Workflow

1. **Synchronize with the remote repository:**

   ```bash
   git pull
   ```

2. **Work on your assigned tasks in Unity.**

3. **Save all progress within Unity.**

4. **Stage and commit your changes:**

   ```bash
   git add .
   git commit -m "Implemented X feature / Fixed Y bug"
   ```

5. **Push to the remote repository:**

   ```bash
   git push
   ```

6. **Teammate pulls latest changes:**

   ```bash
   git pull
   ```

---

### Working with Feature Branches

1. **Create and switch to a new branch:**

   ```bash
   git checkout -b feature-branch-name
   ```

2. **Make changes and test.**

3. **Commit and push:**

   ```bash
   git add .
   git commit -m "Implemented/tested feature X"
   git push origin feature-branch-name
   ```

4. **Open a Pull Request on GitHub.**

---

### Merging Feature Branch into `main`

1. **Switch to `main`:**

   ```bash
   git checkout main
   ```

2. **Pull latest changes:**

   ```bash
   git pull
   ```

3. **Merge branch:**

   ```bash
   git merge feature-branch-name
   ```

4. **Resolve conflicts if needed.**

5. **Push updated `main`:**

   ```bash
   git push
   ```

Or merge via GitHub Pull Request.

---

## Collaborative Development with GitHub

### Daily Workflow

1. Pull latest changes:

   ```bash
   git pull
   ```

2. Create or switch to a branch:

   ```bash
   git checkout -b feature-branch-name
   ```

3. Make and test changes.

4. Stage and commit:

   ```bash
   git add .
   git commit -m "feat: add jump mechanic"
   ```

5. Push to GitHub:

   ```bash
   git push origin feature-branch-name
   ```

6. Open a Pull Request to merge into `main`.

---

### Commit Message Format

- `feat:` new feature (e.g., `feat: add enemy AI`)
- `fix:` bug fix
- `refactor:` code refactor
- `style:` formatting
- `docs:` documentation
- `chore:` maintenance

---

### Tracking Progress

- Use GitHub Issues and Projects
- Reference issues in commits: `fix: resolve camera bug #7`

---

### Repository and Permissions

**Repository URL:**  
[https://github.com/jesusneri1024/SynapseRunner](https://github.com/jesusneri1024/SynapseRunner)

**Clone the project:**

```bash
git clone https://github.com/jesusneri1024/SynapseRunner.git
```

**Add a collaborator:**

1. Go to `Settings > Collaborators and teams`
2. Click `Add collaborator`
3. Enter GitHub username and assign access

---

### Weekly Sync Guidelines

- Review open PRs
- Merge into `main`
- Update the development journal

---

### Why Version Control Matters

- Teamwork without conflicts
- Easy rollback
- Clear change history

---

## Development Journal

### Tuesday, March 25

We implemented the base script for player movement, including physical locomotion and camera control. We also created a shooting script that allows the player to shoot projectiles. Although the mechanic works, further refinement is needed to correctly apply recoil physics that pushes the player in the opposite direction. A provisional scene was built to test these features in context.

### Thursday, March 27

Set up version control using GitHub to enable team collaboration. Created the repository at [https://github.com/jesusneri1024/SynapseRunner](https://github.com/jesusneri1024/SynapseRunner) and configured the project for shared development.
