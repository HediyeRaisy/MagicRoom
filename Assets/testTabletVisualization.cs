using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTabletVisualization : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        int height = 2;
        int width = 5;
        JObject o = new JObject();
        o["height"] = height;
        o["width"] = width;
        JArray arr = new JArray();

        o["components"] = arr;
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                JObject c = new JObject();
                c["cellPositionX"] = j;
                c["cellPositionY"] = i;
                c["content"] = new JObject();
                c["content"]["isSelected"] = false;
                c["content"]["isEnabled"] = true;
                c["content"]["isVisible"] = true;
                c["content"]["code"] = i + "_" + j;
                c["content"]["content"] = "text " + i + "_" + j;
                c["content"]["celltype"] = CellType.BUTTON_TEXT.ToString();
                arr.Add(c);
            }
        }
        TabletVisualizationManagement.Instance.InitializeComponent("MyGrid", o);

        /*JObject t = new JObject();
        t["lenght"] = 3;
        TabletVisualizationManagement.Instance.InitializeComponent("longtextblock", t);
        JObject tt = new JObject();
        tt["text"] = "This is my new text";
        TabletVisualizationManagement.Instance.UpdateComponentVisualization("longtextblock", tt);*/

        JArray propsetter = new JArray();
        JObject p1 = new JObject();
        p1["type"] = PropType.ENUM.ToString();
        p1["name"] = "prop1";
        p1["description"] = "this is my property 1";
        p1["value"] = "val1";
        p1["range"] = new JArray { "val1", "val2", "val3" };
        propsetter.Add(p1);
        JObject p2 = new JObject();
        p2["type"] = PropType.STRING.ToString();
        p2["name"] = "prop2";
        p2["description"] = "this is my property 2";
        p2["value"] = "myvalstring";
        propsetter.Add(p2);
        JObject p3 = new JObject();
        p3["type"] = PropType.INT.ToString();
        p3["name"] = "prop3";
        p3["description"] = "this is my property 3";
        p3["value"] = 1;
        p3["range"] = new JArray { -1, 4 };
        propsetter.Add(p3);
        JObject p4 = new JObject();
        p4["type"] = PropType.FLOAT.ToString();
        p4["name"] = "prop4";
        p4["description"] = "this is my property 4";
        p4["value"] = 1.3f;
        p4["range"] = new JArray { 1, 2 };
        propsetter.Add(p4);

        TabletVisualizationManagement.Instance.InitializeComponent("MyPropertySetter", propsetter); 


        TabletVisualizationManagement.Instance.RegisterToEventHandler("MyGrid", retrievemessage);
        TabletVisualizationManagement.Instance.RegisterToEventHandler("MyPropertySetter", retrievemessage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void retrievemessage(JObject content) {
        Debug.Log(content.ToString());
    }
}
