using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class GridTabletBlock : TabletComponent
{
    public int width = -1;
    public int height = -1;
    public Dictionary<Vector2, ContentGridCell> objectsinGrid;

    public GridTabletBlock(string code, int pos) {
        this.code = code;
        positionInDock = pos;
        objectsinGrid = new Dictionary<Vector2, ContentGridCell>();
    }

    public GridTabletBlock(string code, JObject jObject)
    {
        this.code = code;
        positionInDock = int.Parse(jObject["position"].ToString());
        objectsinGrid = new Dictionary<Vector2, ContentGridCell>();
    }

    public override void setUp(JObject o)
    {
        width = int.Parse(o["width"].ToString());
        height = int.Parse(o["height"].ToString());
        setUp((JArray)o["components"]);
    }
    public override void setUp(JArray a)
    {
        if (width == -1) {
            if (a.Count < 5) {
                width = a.Count;
                height = 1;
            }
            else {
                width = 5;
                height = Mathf.CeilToInt(a.Count / 5f);
            }
        }
        foreach (JObject ob in a)
        {
            GridCell gc = ob.ToObject<GridCell>();
            Vector2 pos = new Vector2(gc.cellPositionX, gc.cellPositionY);
            objectsinGrid.Add(pos, gc.content);
        }
    }

    public override void FireEvent(string specificElementName, JObject message)
    {
        base.OnMessageReceivedFromTablet(new MessageFromTabletArs(specificElementName, message));
    }


    public override void updateStatus(JObject o)
    {
        ActionGridCell agc = o.ToObject<ActionGridCell>();

        ContentGridCell cgc = objectsinGrid.Values.Where((x) => x.code == agc.code).FirstOrDefault();
        if (cgc != null) {
            switch (agc.action) {
                case CellAction.ACTIVATE: cgc.isSelected = true; break;
                case CellAction.DEACTIVATE: cgc.isSelected = false; break;
                case CellAction.DISABLE: cgc.isEnabled = true; break;
                case CellAction.ENABLE: cgc.isEnabled = false; break;
                case CellAction.HIDE: cgc.isVisible = false; break;
                case CellAction.UNHIDE: cgc.isVisible = true; break;
                case CellAction.UPDATECONTENT: cgc.content = agc.content; break;
            }
        }
    }

    public override JObject schematizeForTablet()
    {
        JObject o = new JObject();
        o["position"] = positionInDock;
        o["type"] = "GridBlock";
        o["height"] = height;
        o["width"] = width;
        JArray blocklist = new JArray();
        foreach (Vector2 v in objectsinGrid.Keys) {
            JObject cell = new JObject();
            cell["x"] = v.x;
            cell["y"] = v.y;
            cell["celltype"] = objectsinGrid[v].celltype.ToString();
            cell["code"] = objectsinGrid[v].code;
            cell["content"] = objectsinGrid[v].content;
            cell["isEnabled"] = objectsinGrid[v].isEnabled;
            cell["isSelected"] = objectsinGrid[v].isSelected;
            cell["isVisible"] = objectsinGrid[v].isVisible;
            blocklist.Add(cell);
        }
        o["cells"] = blocklist;
        return o;
    }
}

public class GridCell{
    public int cellPositionX;
    public int cellPositionY;
    public ContentGridCell content;
}

public class ContentGridCell {
    public bool isSelected;
    public bool isEnabled;
    public bool isVisible;
    public string code;
    public string content;
    public CellType celltype;
}

public class ActionGridCell
{
    public string code;
    public string content;
    public CellAction action;
}

public enum CellType { 
    TEXT, IMAGE, BUTTON_TEXT, BUTTON_IMAGE
}

public enum CellAction { 
    ACTIVATE, DEACTIVATE, ENABLE, DISABLE, HIDE, UNHIDE, UPDATECONTENT
}
