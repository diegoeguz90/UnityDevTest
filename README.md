# Block Puzzle Game - Unity Project

This repository contains the solution for a Unity developer practical test, completed within a 3-hour time limit.

---

## Game Description

A simple 6x5 grid puzzle game where the player collects groups of colored blocks to score points.

* **Objective:** Get the highest score possible.
* **Rules:**
    * The player starts with 5 moves.
    * Tapping a block collects it and all connected blocks of the same color.
    * Points are awarded based on the number of blocks collected.
    * The grid refills using a gravity mechanic.
    * The game ends when the player runs out of moves.

---

## Features Implemented

* **Task 1 (Asset Integration):** The main game scene and a "Game Over" screen were built using the provided art assets.
* **Task 2 (UI & Data Handling):** The UI for score and moves was implemented and connected to the game logic, including a complete Game Over and Replay cycle.
* **Task 3 (Core Gameplay):** The main puzzle mechanic was fully developed, including the block-finding algorithm, gravity, and grid refill logic.

---

## Technical Specs

* **Engine:** Unity
* **Platform:** Mobile (Portrait)
* **Input:** Unity's new Input System
* **Text:** TextMeshPro

---

## How to Run

1.  Clone this repository.
2.  Open the project folder with Unity Hub.
3.  Open the main scene located in the `Assets/_Scenes/` folder.
4.  Press the **Play** button in the editor.
