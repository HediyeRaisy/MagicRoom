# Magik Room Manager

**Magik Room Manager** is a Unity-based middleware integration layer designed to connect Unity applications with the **Magika / Magic Room architecture**.  
It provides a unified manager and a set of modular components that enable communication with external services such as lighting, appliances, text-to-speech, Kinect tracking, and experience management within a Magic Room environment.

This repository contains the **core scripts and prefabs** required to integrate a Unity scene with the Magika ecosystem.

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

Sends a named event with an optional JSON payload to the Experience Manager.

---

### 🔌 Appliances Manager

Manages communication with the Appliances Server.

**Features:**
- Access the list of available appliances
- Send ON/OFF commands

**Method:**
```csharp
SendChangeCommand(string appliance, string cmd)
```

- `appliance`: name of the appliance
- `cmd`: `"ON"` or `"OFF"`

---

### 💡 Light Manager

Controls lights connected to the Light Server.

**Features:**
- Retrieve available lights
- Set colors and brightness globally or per light

**Methods:**
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

- Colors must be specified as lowercase HTML color codes or Unity `Color`
- If `name` is provided, the command targets a specific light

---

### 🗣 Text To Speech Manager

Handles communication with the Text-To-Speech server.

**Features:**
- Retrieve available voices
- Generate and play speech audio
- Trigger speech lifecycle events

**Methods:**
```csharp
GenerateAudioFromText(string text, Voices voice)
GenerateAudioFromText(string text)
```

If no voice is specified, the system uses the `"Magika"` voice if available.

**Events:**
- `StartSpeak`
- `EndSpeak`

These are useful for synchronization (e.g., lip-sync or animation timing).

---

### 🦴 Kinect Manager

Manages Kinect-based body tracking and gesture recognition.

**Streaming:**
```csharp
StartStreamingSkeletons(int interval)
StopStreamingSkeletons()
```

- `interval`: milliseconds between samples

**Data Access:**
```csharp
ReadLastSamplingKinect()
```

**Events:**
```csharp
Skeletons(List<Skeleton> skeletons)
```

Only detected skeletons are returned (no empty vectors).

**Additional Features:**
- Check sensor status:
```csharp
GetStatusKinect()
```
- Gesture recognition:
```csharp
SetGestureRecognitionKinect(Dictionary<string, float> gesture)
ResetGestureRecognized()
```

---

### 📂 Streaming Assets Manager

Singleton component for loading external resources dynamically.

**Supported asset types:**
- Audio
- Images (`Texture2D`)
- Video

**Methods:**
```csharp
LoadAudioClipFromStreamingAsset(string folder, string filename, Action<AudioClip> callback)
LoadImageFromStreamingAsset(string folder, string filename, Action<Texture2D> callback)
```

Both return a `Coroutine` (or `null` if loading fails).

**Video loading:**
```csharp
LoadVideoClipFromStreamingAsset(string folder, string filename, VideoPlayer player)
```

Returns `true` if successful.

Resources can be loaded from:
- Unity’s `StreamingAssets`
- A custom folder defined by `PathResources`

---

## ⚠️ Notes & Best Practices

- Always check module instances for `null` before use
- Enable only the modules required for your experience
- Keep the Magic Room Adapter in a dedicated bootstrap scene
- Ensure external servers (Light, Kinect, Appliances, TTS) are running before launch

---

## 📄 License

Specify the license for this project here.

---

## 📬 Contact

For questions, issues, or integration support, please open an issue or contact the project maintainers.
