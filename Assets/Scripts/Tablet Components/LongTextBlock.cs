using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongTextBlock : TabletComponent
{
    string[] text;
    int pos = 0;

    public LongTextBlock(string code, JObject jObject)
    {
        this.code = code;
        positionInDock = int.Parse(jObject["position"].ToString());
    }

    public override void FireEvent(string specificElementName, JObject message)
    {
        base.OnMessageReceivedFromTablet(new MessageFromTabletArs(specificElementName, message));
    }

    public override JObject schematizeForTablet()
    {
        JObject o = new JObject();
        o["position"] = positionInDock;
        o["type"] = "PropertyLongTextBlock";
        int tempindex = pos;
        string s = "";
        if (text != null)
        {
            for (int i = 0; i < text.Length; i++)
            {
                s += text[tempindex] + "\n";
                tempindex = (tempindex + 1) % text.Length;
            }
            o["text"] = s;
        }
        else {
            o["text"] = "";
        }
        return o;
    }

    public override void setUp(JObject o)
    {
        int len = (int)o["lenght"];
        text = new string[len];
        pos = 0;
    }

    public override void setUp(JArray o)
    {
        text = new string[o.Count];
        for(int i = 0; i < o.Count; i++) {
            text[i] = o[i].ToString();
        }
        pos = o.Count-1;
    }

    public override void updateStatus(JObject o)
    {
        string t = o["text"].ToString();
        pos = (pos + 1) % text.Length;
        text[pos] = t;
    }
}
