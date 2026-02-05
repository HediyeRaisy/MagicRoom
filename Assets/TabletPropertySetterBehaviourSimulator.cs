using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletPropertySetterBehaviourSimulator : MonoBehaviour
{
    public string blockcode, buttoncode;
    public PropType type;

    void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }
    public void Setup(PropType prop, string BlockCode, string ButtonCode, string propname, int min = 0, int max = 1, string[]opt = null) {
        blockcode = BlockCode;
        buttoncode = ButtonCode;
        Transform refobj = null;
        type = prop;
        switch (prop) {
            case PropType.STRING:
                refobj = transform.GetChild(2);
                break;
            case PropType.INT:
                refobj = transform.GetChild(0);
                Slider s = refobj.GetChild(1).GetComponent<Slider>();
                s.wholeNumbers = true;
                s.minValue = min;
                s.maxValue = max;
                break;
            case PropType.FLOAT:
                refobj = transform.GetChild(0);
                Slider s2 = refobj.GetChild(1).GetComponent<Slider>();
                s2.wholeNumbers = false;
                s2.minValue = min;
                s2.maxValue = max;
                break;
            case PropType.ENUM:
                refobj = transform.GetChild(1);
                Dropdown d = refobj.GetChild(1).GetComponent<Dropdown>();
                d.options.Clear();
                foreach (string o in opt) {
                    d.options.Add(new Dropdown.OptionData(o));
                }
                break;
        }
        refobj.gameObject.SetActive(true);
        refobj.GetChild(0).GetComponent<Text>().text = propname;
    }

    public void FireText() {
        JObject obj = new JObject();
        obj["blockName"] = blockcode;
        JObject payload = new JObject();
        payload["id"] = buttoncode;
        payload["value"] = transform.GetChild(2).GetChild(1).GetComponent<InputField>().text;
        obj["payload"] = payload;

        TabletVisualizationManagement.Instance.ManageHttpRequest(obj.ToString(), null);
    }

    public void FireDropdown() {
        JObject obj = new JObject();
        obj["blockName"] = blockcode;
        JObject payload = new JObject();
        payload["id"] = buttoncode;
        payload["value"] = transform.GetChild(1).GetChild(1).GetComponent<Dropdown>().options[transform.GetChild(1).GetChild(1).GetComponent<Dropdown>().value].text;
        obj["payload"] = payload;
        TabletVisualizationManagement.Instance.ManageHttpRequest(obj.ToString(), null);
    }

    public void FireSlider() {
        JObject obj = new JObject();
        obj["blockName"] = blockcode;
        JObject payload = new JObject();
        payload["id"] = buttoncode;
        payload["value"] = transform.GetChild(0).GetChild(1).GetComponent<Slider>().value;
        obj["payload"] = payload;
        TabletVisualizationManagement.Instance.ManageHttpRequest(obj.ToString(), null);
    }
}


