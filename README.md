# Magic Room: Tangram Edition

## Overview

**Magic Room: Tangram Edition** is a Unity-based, room-scale interactive Tangram game designed to support **attention regulation, planning, and spatial reasoning** in children aged **6–8**, with a particular focus on children with **ADHD**.

The project transforms traditional Tangram puzzles into an **embodied and spatial experience** using full-body movement, gesture-based interaction, and large-scale **floor and wall projection**. Instead of interacting through a mouse, keyboard, or touchscreen, children solve puzzles by moving in physical space, selecting pieces with their body, and manipulating shapes through natural gestures.

This project was developed as an **academic prototype** within the context of Advanced User Interface and Human–Computer Interaction research.

---

## Relationship to Magic Room Manager

This repository **is not the Magic Room Manager**.

- **Magic Room Manager** is the underlying smart-space framework responsible for:
  - room calibration  
  - projection mapping  
  - sensing and body-tracking infrastructure  

- **Magic Room: Tangram Edition** is a **game module built on top of the Magic Room ecosystem**.

In short:

> **Magic Room Manager provides the interactive environment**  
> **Magic Room: Tangram Edition provides the Tangram gameplay, logic, and interaction design**

The Tangram game reuses the Magic Room interaction paradigm but introduces **new mechanics, narratives, and cognitive goals** specific to Tangram-based problem solving.

---

## What the Game Does

The game implements a **projection-based Advanced User Interface** with a clear spatial division of interaction:

- **Floor projection**
  - Displays Tangram pieces
  - Used for selection through dwell-time (standing on a piece)

- **Wall projection**
  - Displays target silhouettes
  - Provides rotation / flip controls
  - Shows narrative context and feedback

### Core Interaction Flow

1. A Tangram silhouette appears on the wall  
2. Pieces are projected on the floor  
3. The child selects a piece by standing on it (dwell-time)  
4. Orientation is adjusted via hand gestures (rotate / flip)  
5. The child grabs and moves the piece using body movement  
6. Correct placement snaps into position with audio-visual feedback  
7. The game progresses piece by piece within a narrative scenario  

The experience is embedded in **story-driven modules** (e.g. *Magic Forest*, *Cat Explorer*, *Sleeping Princess*) to promote motivation and sustained engagement.

---

## Design Foundations

The project is grounded in:

- **Embodied and spatial interaction**
- The **PASS cognitive model** (Planning, Attention, Simultaneous, Successive)
- Mediated learning (therapist-in-the-loop)
- Controlled sensory stimulation to avoid overload

The system is designed for **short, structured sessions** in therapy rooms, schools, or research labs equipped with projection and motion-tracking.

---

## Technical Overview

- **Engine:** Unity  
- **Language:** C#  
- **Architecture:** Component-based Unity architecture  
- **Interaction:** Gesture-based and body-based input (Magic Room sensing layer)  

### Main Functional Modules

- Tangram puzzle management  
- Interaction and manipulation logic  
- Visual feedback and animations  
- Audio feedback and narration triggers  
- Scene and game-state management  

Each Tangram piece is implemented as a Unity GameObject with attached scripts controlling selection, transformation, movement, and placement validation.

---

## Repository Scope (Important)

This repository intentionally follows a **scripts-only Git strategy**.

### Included in this repo
- Unity **C# scripts**
- Game logic and interaction code
- Minimal configuration files
- Documentation

### Not included
- Unity build outputs (`.exe`, `.app`, etc.)
- Large binary assets
- Project `Library/`, `Temp/`, or build folders
- Hardware-specific calibration data

These are managed separately using Unity’s tooling and external backups.

---

## Version Control Strategy

- `.gitignore` is configured to **track only scripts**
- Large assets and builds are excluded to keep the repository lightweight
- Future versions may migrate selected assets to **Git LFS**

This approach ensures:
- clean Git history  
- no GitHub file-size limit issues  
- focus on reproducible interaction logic  

---

## How to Use This Repository

This repository is intended for:

- Reviewing the **interaction logic and game architecture**
- Extending or adapting Tangram gameplay within a Magic Room setup
- Academic reference and further research development

To run the full experience, a **Magic Room installation** (projection + tracking) and the corresponding environment configuration are required.

---

## Project Status

This project is an **academic prototype** evaluated through a system-level demonstration in a controlled laboratory environment. It is not a commercial product.

---

## Authors

- Ghazal Sepehrirad  
- Hedieh Raeisi  

Supervised by:  
**Prof. Franca Garzotto**

---

## License

This project is provided for academic and research purposes.  
See license information if applicable.
