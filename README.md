# ✨ Magic Room: Tangram Edition 🧩

An embodied, room-scale Tangram game for attention, planning, and spatial reasoning — built for the **Magic Room** environment.

🎥 **Project introduction video:**  
👉 https://youtu.be/_81DZwDntUg?si=RBXkI99-Qoz_kLeT

---

## 🌟 What Is This Project?

**Magic Room: Tangram Edition** is a Unity-based, projection-driven Tangram game designed for children aged **6–8**, with a particular focus on supporting **attention regulation, planning, and spatial reasoning** in children with ADHD.

Instead of sitting at a screen, children interact using their **whole body**:

- 🧍 Standing on Tangram pieces to select them  
- ✋ Using hand gestures to rotate and flip shapes  
- 🚶 Moving in physical space to place pieces correctly  

The game is played inside a **Magic Room**: a smart, immersive environment that combines **floor and wall projection**, body tracking, and multisensory feedback.

This project was developed as an **academic prototype** within an Advanced User Interface / HCI context.

---

## 🧠 Research & Design Foundations

The interaction design is grounded in:

- **Embodied and spatial interaction**
- The **PASS cognitive model**  
  *(Planning – Attention – Simultaneous – Successive processing)*
- Mediated learning (therapist-in-the-loop)
- Careful control of sensory load to avoid overstimulation

Tangram puzzles are embedded in **story-driven scenarios** (e.g. *Magic Forest*, *Cat Explorer*, *Sleeping Princess*) to maintain engagement and motivation.

---

## 🧩 What the Game Does

The system uses a **clear spatial division of interaction**:

### 🟦 Floor Projection
- Displays Tangram pieces  
- Used for **selection via dwell-time** (standing still on a piece)

### 🟩 Wall Projection
- Displays target silhouettes  
- Shows rotation / flip controls  
- Provides narrative context and feedback  

### 🔄 Core Interaction Flow

1. A Tangram silhouette appears on the wall  
2. Pieces are projected onto the floor  
3. The child selects a piece by standing on it  
4. Orientation is adjusted using hand gestures  
5. The piece is grabbed and moved using body movement  
6. Correct placement snaps into position with audio-visual feedback  
7. The game progresses within a narrative scenario  

---

## 🧱 Relationship to Magic Room Manager (Important!)

⚠️ **This repository is NOT the Magic Room Manager.**

- **Magic Room Manager** handles:
  - room calibration
  - projection mapping
  - sensing & body-tracking infrastructure

- **Magic Room: Tangram Edition** is a **game module built on top of the Magic Room ecosystem**

👉 Think of it like this:

> 🏗️ *Magic Room Manager = the interactive space*  
> 🎮 *Tangram Edition = the game that lives inside it*

---

## 🛠️ Technical Overview

- 🎮 Engine: **Unity**
- 💻 Language: **C#**
- 🧩 Architecture: Unity component-based design
- 🤸 Interaction: gesture-based & body-based (via Magic Room sensing layer)

### Main Functional Modules
- Tangram puzzle management  
- Interaction & manipulation logic  
- Visual feedback & animations  
- Audio feedback & narration triggers  
- Scene & game-state management  

Each Tangram piece is a Unity `GameObject` with attached scripts controlling:
selection, transformation, movement, and placement validation.

---

## 📂 Repository Scope (Scripts-Only by Design)

This repository intentionally follows a **scripts-only Git strategy**.

### ✅ What’s included
- Unity **C# scripts**
- Interaction and gameplay logic
- Minimal configuration files
- Documentation

### 🚫 What’s NOT included
- Unity build outputs (`.exe`, `.app`)
- Large binary assets
- `Library/`, `Temp/`, and build folders
- Hardware calibration data

📦 These are backed up separately using Unity’s tools and external storage.

---

## 🚀 How to Use the Code

### 🔹 If you want to **read or study the code**
You’re good to go 👍  
This repo is ideal for:
- understanding embodied Tangram interaction logic
- studying room-scale AUI design
- extending the gameplay mechanics

### 🔹 If you want to **run or extend the game**
You will need:

1. **Unity** (same or compatible version used in development)
2. A working **Magic Room environment**, including:
   - projection setup (floor + wall)
   - sensing / body-tracking infrastructure
3. The **Magic Room Manager framework** (not included here)

📌 This repo provides the **game logic**, not the full runtime environment.

---

## 🎥 Project Video

We created a short video introducing the project, the interaction concept, and the Magic Room setup:

▶️ **Watch here:**  
https://youtu.be/_81DZwDntUg?si=RBXkI99-Qoz_kLeT

This video works as a visual overview of:
- the interaction paradigm
- spatial layout
- gameplay flow
- narrative structure

---

## 🔄 Version Control Strategy

- `.gitignore` is configured to track **scripts only**
- Large assets and builds are excluded
- Future iterations may migrate selected assets to **Git LFS**

This keeps the repo:
- clean 🧼
- lightweight ⚡
- GitHub-friendly 🐙

---

## 📌 Project Status

This is an **academic prototype**, evaluated through a system-level demonstration in a controlled laboratory environment.  
It is not a commercial product.

---

## 👩‍💻 Authors

- **Ghazal Sepehrirad**  
- **Hedieh Raeisi**

🎓 Supervised by:  
**Prof. Franca Garzotto**

---

## 📜 License

Provided for academic and research purposes.  
See license information if applicable.
