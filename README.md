# Magik Room Manager

**Magik Room Manager** is a Unity-based middleware integration layer designed to connect Unity applications with the **Magika / Magic Room architecture**.  
It provides a unified manager and a set of modular components that enable communication with external services such as lighting, appliances, text-to-speech, Kinect tracking, and experience management within a Magic Room environment.

This repository contains the **core scripts and prefabs** required to integrate a Unity scene with the Magika ecosystem.

---

## 🎥 Gameplay Video

Below is a gameplay video demonstrating the **Magic Room: Tangram Osmo Edition**, showcasing embodied interaction, spatial manipulation, and gesture-based control in the Magic Room environment.

[![Magic Room Gameplay Video](https://img.youtube.com/vi/GdXRwATgJOQ/0.jpg)](https://youtu.be/GdXRwATgJOQ)

> This video replaces the duplicated introduction video and provides a clearer view of real in-room interaction and gameplay flow.

---

## 📦 Repository Overview

- All scripts are located in the **`MagikRoomScripts`** folder inside Unity’s standard `Assets/Scripts` structure.
- Reusable prefabs are provided in the **`Prefabs`** folder to simplify setup and configuration.
- The system follows a **singleton-based architecture**, with `MagicRoomManager` acting as the main access point.

---

## 📁 Project Structure

```
Assets/
├── Scripts/
│   └── MagikRoomScripts/        # Core Magik Room managers and adapters
│
├── Prefabs/
│   └── Magic Room Adapter      # Main prefab for Magika integration
│
└── StreamingAssets/             # Optional shared resources
```

---

## 🚀 Getting Started

### 1. Add the Magic Room Adapter

To connect your Unity scene with the Magika architecture:

1. Open your Unity project
2. Drag the **`Magic Room Adapter`** prefab into your scene
3. Configure the prefab in the Inspector:
   - **HTTP Address** (format: `protocol://host`)
   - **HTTP Port**
   - Enable the required modules (Light, Appliances, Kinect, TTS, etc.)

### Recommended Setup

It is strongly recommended to:
- Create an **empty bootstrap scene** as the **first scene** in the build order
- Add **only** the `Magic Room Adapter` prefab (and other singletons if needed)
- Automatically load the main scene after initialization by setting the **Scene Index** in the prefab Inspector

---

## 🧠 MagicRoomManager Architecture

`MagicRoomManager` is a singleton responsible for:

1. Fetching the **SystemConfiguration** from the Experience Manager (`localhost:7100`)
2. Initializing and enabling selected modules
3. Managing shared resources and optional `StreamingAssetsManager`

All modules are accessed through:

```csharp
MagicRoomManager.Instance
```

Example:
```csharp
var lightManager = MagicRoomManager.Instance.MagicRoomLightManager;
if (lightManager != null)
{
    // Use the light manager
}
```

---

## 🔧 Available Modules

### 🎮 Experience Manager

Handles communication with the Experience Manager server.

**Endpoint:** `/ExperienceManager`

**Method:**
```csharp
SendEvent(string eventName, JObject payload = null)
```

Sends the event with an optional JSON payload.

---

### 🔌 Appliances Manager

Manages communication with the Appliances Server.

**Method:**
```csharp
SendChangeCommand(string appliance, string cmd)
```

- `appliance`: appliance name  
- `cmd`: `"ON"` or `"OFF"`

---

### 💡 Light Manager

Controls lights connected to the Light Server.

```csharp
SendColor(string color, int brightness = 100, string name = null,
          LocDepth depth = LocDepth.all,
          LocHorizontal horizontal = LocHorizontal.all,
          LocVertical vertical = LocVertical.all)
```

```csharp
SendColor(Color col, string name = null,
          LocDepth depth = LocDepth.all,
          LocHorizontal horizontal = LocHorizontal.all,
          LocVertical vertical = LocVertical.all)
```

---

### 🗣 Text To Speech Manager

Manages speech synthesis.

```csharp
GenerateAudioFromText(string text, Voices voice)
GenerateAudioFromText(string text)
```

**Events:**
- `StartSpeak`
- `EndSpeak`

---

### 🦴 Kinect Manager

Handles skeleton tracking and gesture recognition.

```csharp
StartStreamingSkeletons(int interval)
StopStreamingSkeletons()
```

```csharp
ReadLastSamplingKinect()
```

**Events:**
```csharp
Skeletons(List<Skeleton> skeletons)
```

Additional features:
```csharp
GetStatusKinect()
SetGestureRecognitionKinect(Dictionary<string, float> gesture)
ResetGestureRecognized()
```

---

### 📂 Streaming Assets Manager

Loads external resources dynamically.

```csharp
LoadAudioClipFromStreamingAsset(string folder, string filename, Action<AudioClip> callback)
LoadImageFromStreamingAsset(string folder, string filename, Action<Texture2D> callback)
LoadVideoClipFromStreamingAsset(string folder, string filename, VideoPlayer player)
```

Resources can be loaded from:
- Unity `StreamingAssets`
- A custom folder defined by `PathResources`

---

## ⚠️ Notes & Best Practices

- Always check module instances for `null` before use
- Enable only required modules
- Keep the Magic Room Adapter in a bootstrap scene
- Ensure all external servers are running before launch

---

## 📄 License

Specify the license for this project here.

---

## 📬 Contact

For questions, issues, or integration support, please open an issue or contact the project maintainers.
