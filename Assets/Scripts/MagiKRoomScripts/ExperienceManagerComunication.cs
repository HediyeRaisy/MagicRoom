using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class ExperienceManagerComunication : MonoBehaviour
{
    public delegate void MessageHandler(string content);

    public event MessageHandler MessageHandlerEvent;

    private const string endpoint = "ExperienceManager";
    private const string address = "http://localhost:7100";

    private string autenticationToken = "";

    private int _activityID;

    public void setActivityID(int value) {
        _activityID = value; 
    }

    private void Start()
    {
        MagicRoomManager.instance.HttpListenerForMagiKRoom.RequestHandlers.Add(new Regex(@"^/" + endpoint + @"$"), MenageMessage);
        _activityID = MagicRoomManager.instance.activityidentifier[0];
        login();
        getRoomConfiguration();
    }

    private void MenageMessage(string message, NameValueCollection query)
    {
        MessageHandlerEvent?.Invoke(message);
    }

    private IEnumerator SendCommand(string evenType, string token = null, MagicRoomManager.WebCallback callback = null)
    {
        JObject message = new JObject();
        message["event"] = evenType;
        if (token != null)
        {
            message["token"] = token;
        }
        string json = message.ToString(Newtonsoft.Json.Formatting.None);
        Debug.LogError("Message Sent " + json);
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(address, "POST")
        {
            uploadHandler = new UploadHandlerRaw(body),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (!request.isNetworkError)
        {
            callback?.Invoke(request.downloadHandler.text);
        }
    }

    private IEnumerator SendCommand(string evenType, string commandname, string command)
    {
        JObject message = new JObject();
        message["event"] = evenType;
        message[commandname] = command;
        message["gameid"] = _activityID;
        string json = message.ToString(Newtonsoft.Json.Formatting.None);
        Debug.LogError("Message Sent " + json);
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(address, "POST")
        {
            uploadHandler = new UploadHandlerRaw(body),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (!request.isNetworkError)
        {
            Debug.LogError(request.downloadHandler.text);

            if (request.downloadHandler.text == "{\"status\":\"not accepted\"}" && command == "GAME_READY")
            {
                yield return new WaitForSeconds(2);
                StartCoroutine(SendCommand(evenType, commandname, command));
            }
        }
    }

    private IEnumerator SendCommand(string evenType, string commandname, JObject command)
    {
        JObject message = new JObject();
        message["event"] = evenType;
        message[commandname] = command;
        message["gameid"] = _activityID;
        string json = message.ToString(Newtonsoft.Json.Formatting.None);
        Debug.LogError("Message Sent " + json);
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(address, "POST")
        {
            uploadHandler = new UploadHandlerRaw(body),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
    }

    private IEnumerator Login()
    {
        JObject message = new JObject();
        message["event"] = "loginCaregiver";
        message["loginId"] = "i3lab";
        message["password"] = "p455w0rd";
        string json = message.ToString(Newtonsoft.Json.Formatting.None);
        Debug.LogError("Message Sent " + json);
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(address, "POST")
        {
            uploadHandler = new UploadHandlerRaw(body),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.ConnectionError)
        {
            JObject o = JObject.Parse(request.downloadHandler.text);
            autenticationToken = o["authorizationToken"].ToString();
        }
    }

    public void SendResponse(string eventName, JObject payload = null)
    {
        dynamic response = new JObject();
        response.@response = new JObject();
        response.@response.type = eventName;
        if (payload != null)
        {
            response.response.payload = payload;
        }
        StartCoroutine(SendCommand(response));
    }


    public void getGameConfigurationFromEM()
    {
        StartCoroutine(SendCommand("getGameConfiguration", "", (string text) => {
            JObject o = JObject.Parse(text);
            GameSetting.instance.updateGameConfig((JObject)o["payload"]);
        }));
    }

    public void getPlayers()
    {
        StartCoroutine(DelayedGetUserList());
    }

    private IEnumerator DelayedGetUserList()
    {
        yield return new WaitUntil(() => autenticationToken != "");
        StartCoroutine(SendCommand("getPlayerList", autenticationToken, (string text) => {
            JObject a = JObject.Parse(text);
            GameSetting.instance.players.Clear();
            foreach (JObject o in (JArray)a["players"])
            {
                Player p = new Player();
                p.id = int.Parse(o["id"].ToString());
                p.name = o["name"].ToString();
                p.surname = o["surname"].ToString();
                p.phonema = p.name + " " + p.surname;
                p.isAtypical = bool.Parse(o["isAtypical"].ToString());
                GameSetting.instance.players.Add(p);
            }
        }));
    }

    private void getRoomConfiguration()
    {
        StartCoroutine(SendCommand("getRoomConfiguration", "", (string text) => {
            JObject o = JObject.Parse(text);
            SystemConfiguration sys = new SystemConfiguration();
            o = (JObject)o["response"]["payload"];
            sys.floorOffsetX = float.Parse(o["floorOffsetX"].ToString());
            sys.floorOffsetY = float.Parse(o["floorOffsetY"].ToString());
            sys.floorOffsetZ = float.Parse(o["floorOffsetZ"].ToString());
            sys.floorSizeX = float.Parse(o["floorSizeX"].ToString());
            sys.floorSizeY = float.Parse(o["floorSizeY"].ToString());
            sys.floorSizeZ = float.Parse(o["floorSizeZ"].ToString());
            sys.floorScreen = int.Parse(o["floorScreen"].ToString());
            sys.frontalScreen = int.Parse(o["frontalScreen"].ToString());
            //sys.resourcesPath = o["resourcesPath"].ToString();
            MagicRoomManager.instance.systemConfiguration = sys;
            ThemeManager.StartUp();
        }));

    }

    public void updateTabletVisualization(JObject o)
    {
        StartCoroutine(SendCommand("setTabletVisualization", "gamevisualization", o));
    }

    private void login()
    {
        StartCoroutine(Login());
    }

    public void sendCommandGameReadyForMaximization()
    {
        StartCoroutine(SendCommand("stateChangeRequest", "command", "GAME_READY"));
    }

    public void sendCommandGameEnded()
    {
        StartCoroutine(SendCommand("stateChangeRequest", "command", "END_GAME"));
    }

}