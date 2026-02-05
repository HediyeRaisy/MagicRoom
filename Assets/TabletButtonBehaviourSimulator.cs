using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletButtonBehaviourSimulator : MonoBehaviour
{
    public string blockcode, buttoncode;

    public void Setup(bool isButton, string BlockCode, string ButtonCode) {

        if (isButton)
        {
            blockcode = BlockCode;
            buttoncode = ButtonCode;
        }
        else {
            gameObject.GetComponent<Button>().interactable = false;
        }

    }

    public void FireEvent() {
        JObject obj = new JObject();
        obj["blockName"] = blockcode;
        JObject payload = new JObject();
        payload["id"] = buttoncode;
        obj["payload"] = payload;

        TabletVisualizationManagement.Instance.ManageHttpRequest(obj.ToString(), null);
    }
}
