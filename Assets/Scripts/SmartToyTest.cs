using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SmartToyTest : MonoBehaviour
{
    private SmartToy toyObject;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            List<string> toys = MagicRoomManager.instance.MagicRoomSmartToyManager.GetAllToy();
            foreach (string toy in toys)
            {
                Debug.Log(toy);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            string toy = MagicRoomManager.instance.MagicRoomSmartToyManager.GetAllToy()[0];
            toyObject = MagicRoomManager.instance.MagicRoomSmartToyManager.GetSmartToyByName(toy).GetComponent<SmartToy>();
            MagicRoomManager.instance.MagicRoomSmartToyManager.SubscribeEvent(EventType.TCP, toyObject.state.Id);
        }
        if (toyObject != null)
        {
            List<ButtonDescription> buttons = toyObject.state.sensors.buttons;
            foreach (ButtonDescription b in buttons)
            {
                if (b.Press)
                    Debug.Log(b.Id);
            }
        }
        if (toyObject != null)
        {
            List<RfidDescription> rfids = toyObject.state.sensors.rfids;
            foreach (RfidDescription b in rfids)
            {
                if (b.Code != "" || b.Code != null)
                    Debug.Log(b.Id + " read " + b.Code);
                Debug.Log(MagicRoomManager.instance.MagicRoomSmartToyManager.GetRfidAssosiation(b.Code));
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (toyObject != null)
            {
                MagicRoomManager.instance.MagicRoomSmartToyManager.SendCommandToToy(toyObject.state.Id, new Newtonsoft.Json.Linq.JArray() { MagicRoomManager.instance.MagicRoomSmartToyManager.elaborateSmartToyCommand("light", new Dictionary<string, JToken>() { { "id", "rfid" }, { "color", "#ff0000" }, { "brightness", 100 } }) });
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (toyObject != null)
            {
                MagicRoomManager.instance.MagicRoomSmartToyManager.SendCommandToToy(toyObject.state.Id, new Newtonsoft.Json.Linq.JArray() { MagicRoomManager.instance.MagicRoomSmartToyManager.elaborateSmartToyCommand("buzzer", new Dictionary<string, JToken>() { { "id", "buzzer" }, { "track", "" } }) });
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            MagicRoomManager.instance.MagicRoomSmartToyManager.UnregisterApp();
        }
    }

    public void ping()
    {
        Debug.Log("ping");
    }

    public void ping2(string test)
    {
        Debug.Log("ping " + test);
    }

    public void testPing(Dictionary<PartToTrack, string> val)
    {
        Debug.Log(val);
    }
}