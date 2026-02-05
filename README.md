# Magik Room Manager

This repo contains all the elements that allow you to connect with Magika Middlewares and require assets. All scripts can be found in the "MagikRoomScripts" folder in the classic "Scripts" folder of Unity project structure. To simplify the use, there are some prefabs in the "Prefabs" folder that automate some operations.

## Use Magik Room Adapter

To allow the connection with Magika architecture just add "Magic Room Adapter" prefab in your scene. In the inspector of this game object you can configure some parameters such as the _Address HTTP_ (which must be a valid URL, so in the form of protocol://host), _Port HTTP_ and which modules to activate. Since MagicRoomManager script has to initialize all component before using them, it is recommended to create an empty scene (which should be the first scene) containing only this prefab (and eventually other singletons like StreamingAssetsManager). To automatically load another scene after initialization, simply indicate the index of this scene in the prefab inspector.
All modules are accessible through the singleton instance of MagicRoomManager. For example, if you want to get the light manager use MagicRoomManager.Instance.MagicRoomLightManager anywhere in your code (obviously you have to enable this module in "Magic Room Adapter" prefab and, possibly, check that the instance of MagicRoomLightManager is not null before using it).
The first operation that MagicRoomManager does is to fetch the SystemConfiguration from ExperienceManager (localhost:7100). Then adds modules components that are enabled and set up the StreamingAssetsManager if it is present (if add it after and want to use shared resources, you have to set the PathResources variable).

### Experience Manager

The only method that you can call is `SendEvent(string eventName, JObject payload = null)`. This method sends the event _eventName_ with an optional _payload_. Experience Manager communicate with the Unity app through the endpoint _/ExperienceManager_.

### Appliances Manager

This component manages the communication with Appliances Server. From the instance you can get the list of all appliance name (Appliances list). The only method that you can call is `SendChangeCommand(string appliance, string cmd)` where _appliance_ is the name of plug and _cmd_ is "ON" or "OFF".

### Light Manager

This component manages the communication with Light Server. From the instance you can get the list of all light names (Lights list). The only method that you can call is `SendColor(string color, int brightness = 100, string name = null, LocDepth depth = LocDepth.all, LocHorizontal horizontal = LocHorizontal.all, LocVertical vertical = LocVertical.all)` where _color_ is an html color code (lower case). This method has an overload `SendColor(Color col, string name = null, LocDepth depth = LocDepth.all, LocHorizontal horizontal = LocHorizontal.all, LocVertical vertical = LocVertical.all)` that merge _color_ and _brightness_ in a RGBA format. If name is specified then the command is applied only on that light.

### Text To Speach Manager

This component manages the communication with Text To Speach Server. From the intance you can get the list of all voice names (ListOfVoice list). The only method that you can call is `GenerateAudioFromText(string text, Voices voice)` that plays the audio from the text string using the specific voice. This method has and overload `GenerateAudioFromText(string text)` that uses the voice named "Magika" if exists. In addition, there are two events: StartSpeak and EndSpeak. The first is triggerd when the speach start and the second is triggered when the speach end (This is useful when you want to, for example, synchronize the lip).

### Kinect Manager

This component manages the communication with Kinect Server. In order to start the use, you have to call `StartStreamingSkeletons(int interval)` where _interval_ is the number of millisecond between two samples. To stop the streaming use `StopStreamingSkeletons()`. To read manually the last skeletons use `ReadLastSamplingKinect()`. Every time one skeleton is read, the _Skeletons_ event is trigger. This event requires a method that accepts `List<Skeleton>` and returns void. Note that this list contains only the skeletons detected so there aren't "Vector0" elements. There is also a `GetStatusKinect()` method that checks if Kinect is connected and update the public `StatusKinectSensor` bool variable. In addition you can add a gesture recognition with `SetGestureRecognitionKinect(Dictionary<string, float> gesture)` method (this requires registering the gesture in Kinect Server) and reset them with `ResetGestureRecognized()`.


### Streaming Assets

This component (singleton) allows you to load assets dynamicaly from two main folder: one is the standard StreamingAssets and the other is the one specified in the public string _PathResources_. There are three types of assets that can be loaded: Audio, Texture2D, Video. The first two can be loaded respectively with `LoadAudioClipFromStreamingAsset(string folder, string filename, Action<AudioClip> callback)` and `LoadImageFromStreamingAsset(string folder, string filename, Action<Texture2D> callback)` where _folder_ is the path of the folder where to search for the _filename_ (with extension) and _callback_ is the function called after the resource is loaded. Both functions return a Coroutine (null if the loading fails) that can be used to check the end of loading. Instead for the video asset you can use the `LoadVideoClipFromStreamingAsset(string folder, string filename, VideoPlayer player)` which starts a video streaming in the _player_ passed to the function. This function returns a bool that states the loading result.

# Magic 
