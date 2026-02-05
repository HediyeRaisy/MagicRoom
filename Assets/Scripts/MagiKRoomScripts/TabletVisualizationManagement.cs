using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TabletVisualizationManagement : MonoBehaviour
{
    private static TabletVisualizationManagement _instance;

    public bool[] ControlButton;
    public int heigth = 1;
    public int width = 1;
    public Action Next;
    private bool next = false; 
    public Action Skip;
    private bool skip = false;
    public Action Repeat;
    private bool repeat = false;
    public Action Prev;
    private bool prev = false;
    public Action Pause;
    private bool pause = false;
    private const string endpoint = "Tablet";
    public delegate void RequestHandler(JObject content);
    public Dictionary<string, RequestHandler> RequestHandlers = new Dictionary<string, RequestHandler>();

    public List<string> codes;
    public List<string> com;
    public List<string> paras;

    public Dictionary<string, TabletComponent> tabletComponents = new Dictionary<string, TabletComponent>();
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            Debug.Log("CODES " + codes.Count);
            /*tabletComponents = new Dictionary<string, TabletComponent>();
            tabletComponents.Add("cardgrid", new GridTabletBlock("cardgrid", 0, 5,3));*/
            for (int i = 0; i < codes.Count; i++) {
                TabletComponent c;
                Debug.Log(codes.ElementAt(i));
                switch (com.ElementAt(i)) {
                    case "Grid": c = new GridTabletBlock(codes.ElementAt(i), JObject.Parse(paras.ElementAt(i))); break;
                    case "Property setting": c = new PropertySetterBlock(codes.ElementAt(i), JObject.Parse(paras.ElementAt(i))); break;
                    case "LongTextBlock": c = new LongTextBlock(codes.ElementAt(i), JObject.Parse(paras.ElementAt(i))); break;
                    default: c = null;break;
                }
                tabletComponents.Add(codes.ElementAt(i), c);
            }
            Debug.Log(tabletComponents.Count);
            StartCoroutine(waitforhttplistener());
        }
        else {
            GameObject.DestroyImmediate(gameObject);
        }
    }

    void Update()
    {
        if (next) {
            Next?.Invoke();
            next = false;
        }
        if (skip)
        {
            Skip?.Invoke();
            skip = false;
        }
        if (prev)
        {
            Prev?.Invoke();
            prev = false;
        }
        if (repeat)
        {
            Repeat?.Invoke();
            repeat = false;
        }
        if (pause)
        {
            Pause?.Invoke();
            pause = false;
        }
    }

    IEnumerator waitforhttplistener() {
        yield return new WaitUntil(() => { return MagicRoomManager.instance.HttpListenerForMagiKRoom != null; });
        MagicRoomManager.instance.HttpListenerForMagiKRoom.RequestHandlers.Add(new Regex(@"^/" + endpoint + @"$"), ManageHttpRequest);
    }

    public void ManageHttpRequest(string content, NameValueCollection query)
    {
        JObject message = JObject.Parse(content);
        string blockid = message["blockName"].ToString();
        //I bottoni a comando non sono un componente vero e proprio perchè sono una costante ... o forse dovrei fare un pomonente apposta?
        if (blockid == "controlButton")
        {
            string buttonId = message["payload"]["id"].ToString();
            switch (buttonId)
            {
                case "next":
                    next = true;
                    break;
                case "prev":
                    prev = true;
                    break;
                case "skip":
                    skip = true;
                    break;
                case "repeat":
                    repeat = true;
                    break;
                case "pause":
                    pause = true;
                    break;
            }
        }
        else
        {
            if (tabletComponents.Keys.ToArray().Contains(blockid))
            {
                JObject specificContent = (JObject)message["payload"];
                tabletComponents[blockid].FireEvent(blockid, specificContent);
            }
        }
    }

    public void RegisterToEventHandler(string componentName, RequestHandler req) {
        RequestHandlers.Add(componentName, req);
        if (tabletComponents.Keys.ToArray().Contains(componentName))
        {
            tabletComponents[componentName].CommandReceived += FireMessageHandlerFromComponent;
        }
    }

    public void FireMessageHandlerFromComponent(object sender, MessageFromTabletArs ars) {
        foreach (KeyValuePair<string, RequestHandler> entry in RequestHandlers)
        {
            if (entry.Key == ars.ComponentName)
            {
                entry.Value(ars.Content);
            }
        }
    }

    public string ConvertTexturetoBase64ForTablet(Texture2D texture) {
        byte[] array = ImageConversion.EncodeToPNG(texture);
        return System.Convert.ToBase64String(array);
    }
    



    public void InitializeComponent(string code, JObject content) {
        tabletComponents[code].setUp(content);
        MagicRoomManager.instance.ExperienceManagerComunication.updateTabletVisualization(SerializeTabletview());
    }
    public void InitializeComponent(string code, JArray content)
    {
        tabletComponents[code].setUp(content);
        MagicRoomManager.instance.ExperienceManagerComunication.updateTabletVisualization(SerializeTabletview());
    }

    public void UpdateComponentVisualization(string code, JObject content)
    {
        tabletComponents[code].updateStatus(content);
        MagicRoomManager.instance.ExperienceManagerComunication.updateTabletVisualization(SerializeTabletview());
    }

    public JObject SerializeTabletview() {
        JObject result = new JObject();
        result["controlButton"] = new JObject();
        result["controlButton"]["repeat"] = ControlButton[0];
        result["controlButton"]["prev"] = ControlButton[1];
        result["controlButton"]["pause"] = ControlButton[2];
        result["controlButton"]["next"] = ControlButton[3];
        result["controlButton"]["skip"] = ControlButton[4];
        result["gridwidth"] = width;
        result["gridheight"] = heigth;

        JArray compoents = new JArray();
        foreach (string tc in tabletComponents.Keys) {
            JObject cmp = new JObject();
            cmp["name"] = tc;
            cmp["component"] = tabletComponents[tc].schematizeForTablet();
            compoents.Add(cmp);
        }
        result["components"] = compoents;
        return result;
    }

    public static TabletVisualizationManagement Instance { get => _instance; set => _instance = value; }
}
