using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropertySetterBlock : TabletComponent
{
    public List<Propertyblock> setproperties;

    public PropertySetterBlock(string code, JObject jObject)
    {
        this.code = code;
        positionInDock = int.Parse(jObject["position"].ToString());
        setproperties = new List<Propertyblock>();
    }

    public override void FireEvent(string specificElementName, JObject message)
    {
        base.OnMessageReceivedFromTablet(new MessageFromTabletArs(specificElementName, message));
    }

    public override JObject schematizeForTablet()
    {
        JObject o = new JObject();
        o["position"] = positionInDock;
        o["type"] = "PropertySetBlock";
        JArray blocklist = new JArray();
        foreach (Propertyblock pb in setproperties) {
            JObject p = new JObject();
            p["name"] = pb.propertyname;
            p["type"] = pb.propertytype.ToString();
            p["description"] = pb.propertydescription;
            switch (pb.propertytype) {
                case PropType.STRING: p["value"] = pb.stringvalue; break;
                case PropType.INT: p["value"] = pb.intvalue; p["min"] = pb.min; p["max"] = pb.max; break;
                case PropType.FLOAT: p["value"] = pb.floatvalue; p["min"] = pb.min; p["max"] = pb.max; break;
                case PropType.ENUM: p["value"] = pb.stringvalue; p["range"] = JArray.FromObject(pb.range); break;
            }
            blocklist.Add(p);
        }
        o["propset"] = blocklist;
        return o;
    }

    public override void setUp(JObject o)
    {
        Propertyblock pb = new Propertyblock();
        pb.propertytype = (PropType)Enum.Parse(typeof(PropType), o["type"].ToString());
        pb.propertyname = o["name"].ToString();
        pb.propertydescription = o["description"].ToString();
        switch (pb.propertytype) {
            case PropType.STRING: 
                pb.stringvalue = o["value"].ToString(); 
                break;
            case PropType.INT:
                pb.intvalue = (int)o["value"];
                pb.max = (int)o["range"][1];
                pb.min = (int)o["range"][0];
                break;
            case PropType.FLOAT:
                pb.floatvalue = (float)o["value"];
                pb.max = (int)o["range"][1];
                pb.min = (int)o["range"][0];
                break;
            case PropType.ENUM:
                pb.stringvalue = o["value"].ToString();
                pb.range = new List<string>();
                foreach (JToken t in o["range"]) {
                    pb.range.Add(t.ToString());
                }
                break;
        }
        setproperties.Add(pb);
    }

    public override void setUp(JArray o)
    {
        foreach (JObject ob in o) {
            setUp(ob);
        }
    }

    public override void updateStatus(JObject o)
    {
        string propname = o["propertyname"].ToString();
        Propertyblock pb = setproperties.Where(x => x.propertyname == propname).FirstOrDefault();
        if (pb != null)
        {
            switch (pb.propertytype)
            {
                case PropType.STRING: pb.stringvalue = o["value"].ToString(); break;
                case PropType.INT: pb.intvalue = (int)o["value"]; break;
                case PropType.FLOAT: pb.floatvalue = (float)o["value"]; break;
                case PropType.ENUM: if (pb.range.Contains(o["value"].ToString())){ pb.stringvalue = o["value"].ToString(); }; break;
            }
        }
    }
}

public class Propertyblock {
    public string propertyname;
    public string propertydescription;
    public PropType propertytype;
    public List<string> range;
    public int min;
    public int max;
    public int intvalue;
    public float floatvalue;
    public string stringvalue;
}

public enum PropType { 
    STRING, INT, FLOAT, ENUM
}
