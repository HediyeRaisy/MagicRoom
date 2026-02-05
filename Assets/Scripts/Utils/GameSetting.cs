using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetting : MonoBehaviour
{
    public event Action<List<Player>> ReceivedPlayers;

    public GameConfiguration configuration;
    public List<Player> players;

    [HideInInspector]
    public Camera Frontcamera, FloorCamera;

    public static GameSetting instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //set up a base configuration

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        FloorCamera = null;
        Frontcamera = null;
    }

    public void StartGame()
    {
        //set up here the proepr aprameter for the story.

        SceneManager.LoadScene(2);
    }

    public void LoadMenu()
    {
        //destroy eventaul controllers and close streams for
        SceneManager.LoadScene(1);
    }

    private void OnApplicationQuit()
    {
        if (MagicRoomManager.instance != null)
        {
            if (MagicRoomManager.instance.MagicRoomLightManager != null)
            {
                MagicRoomManager.instance.MagicRoomLightManager.SendColor(Color.black);
            }
            if (MagicRoomManager.instance.MagicRoomAppliancesManager != null)
            {
                foreach (string s in MagicRoomManager.instance.MagicRoomAppliancesManager.Appliances)
                {
                    MagicRoomManager.instance.MagicRoomAppliancesManager.SendChangeCommand(s, "OFF");
                }
            }
        }
    }

    internal void updateGameConfig(JObject o)
    {
        configuration.sessionactid = "-1"; //TODO fix the session ID if still needed
        GameConfiguration.SetConfigFromObject(o, configuration);
        int sessionId = int.Parse(configuration.sessionactid);
        //ThemeManager.SetThemeByName(((GameConfiguration)GameSetting.instance.configuration).theme.ToUpper());
        MagicRoomManager.instance.Logger.SessionID = sessionId;
        MagicRoomManager.instance.Logger.AddToLogNewLine("Activity Memory", "SessionId " + sessionId);
        MagicRoomManager.instance.Logger.AddToLogNewLine("Activity Memory", "received configuration " + o.ToString());
        if (configuration.IsValidConfiguration(configuration))
        {
            StartGame();
        }
    }

}