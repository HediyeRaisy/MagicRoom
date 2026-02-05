using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabletSimulator : MonoBehaviour
{
    public Text appname;
    public GameObject controlblock;
    public GridLayoutGroup centralActivityBlock;
    private TabletVisualizationManagement tvm;
    public GameObject prefabGrid, prefabTabletInnerGridCell, prefabTabletInnerTextElement, prefabTabletInnerSetvalueElement;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        StartCoroutine(delaystart());
    }

    private IEnumerator delaystart() {
        yield return new WaitUntil(() => TabletVisualizationManagement.Instance != null);
        tvm = TabletVisualizationManagement.Instance;
        for (int i = 0; i < tvm.ControlButton.Length; i++)
        {
            controlblock.transform.GetChild(i).GetComponent<Button>().interactable = tvm.ControlButton[i];
        }
        centralActivityBlock.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        centralActivityBlock.constraintCount = tvm.heigth;
        int width = (int)(centralActivityBlock.GetComponent<RectTransform>().rect.width / tvm.width);
        int height = (int)(centralActivityBlock.GetComponent<RectTransform>().rect.height / tvm.heigth);
        centralActivityBlock.cellSize = new Vector2(width, height);
        updateSchema(TabletVisualizationManagement.Instance.SerializeTabletview());
    }

    // Update is called once per frame
    void Update()
    {
        updateSchema(TabletVisualizationManagement.Instance.SerializeTabletview());
    }

    public void updateSchema(JObject schema) {
        //TODO: ordianre in base as sch["position"}
        JArray componentListschema = (JArray)schema["components"];
        List<JToken> orderedschema = componentListschema.OrderBy(o => (int)o["component"]["position"]).ToList();

        int i = 0;
        foreach (JObject com in orderedschema) {
            string compname = com["name"].ToString();
            JObject sch = (JObject)com["component"];
            switch (sch["type"].ToString()) {
                case "GridBlock":

                    GameObject grid_comp;
                    if (centralActivityBlock.transform.childCount < orderedschema.Count) {
                        grid_comp = GameObject.Instantiate(prefabGrid);
                        grid_comp.transform.SetParent(centralActivityBlock.transform);
                        grid_comp.GetComponent<RectTransform>().localScale = Vector2.one;
                        grid_comp.transform.localPosition = Vector3.zero;
                    }
                    else {
                        grid_comp = centralActivityBlock.transform.GetChild(i).gameObject;
                    }
                    GridLayoutGroup inerlayout = grid_comp.GetComponent<GridLayoutGroup>();
                    inerlayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                    float cellwidth = centralActivityBlock.GetComponent<GridLayoutGroup>().cellSize.x / int.Parse(sch["width"].ToString());
                    float cellheight = centralActivityBlock.GetComponent<GridLayoutGroup>().cellSize.y / int.Parse(sch["height"].ToString());
                    inerlayout.constraintCount = int.Parse(sch["width"].ToString());
                    inerlayout.cellSize = new Vector2(cellwidth,cellheight);
                    grid_comp.name = compname;
                    int j = 0;
                    
                    JArray cellsblock = (JArray)sch["cells"];
                    if (cellsblock.Count < 1) { return; }
                    List<JToken> orderedcells = cellsblock.OrderBy(o => (int)o["y"]).ThenBy(o => (int)o["x"]).ToList();

                    foreach (JObject cell in orderedcells) {
                        
                        
                        GameObject ingridcell;
                        if (grid_comp.transform.childCount < orderedcells.Count)
                        {
                            ingridcell = GameObject.Instantiate(prefabTabletInnerGridCell);
                            ingridcell.transform.SetParent(grid_comp.transform);
                            ingridcell.GetComponent<RectTransform>().localScale = Vector2.one;
                        }
                        else {
                            ingridcell = grid_comp.transform.GetChild(j).gameObject;
                        }
                        
                        Button b = ingridcell.GetComponent<Button>();
                        ingridcell.name = cell["code"].ToString();
                        ingridcell.SetActive(bool.Parse(cell["isVisible"].ToString()));
                        switch ((CellType)Enum.Parse(typeof(CellType), cell["celltype"].ToString()))
                        {
                            case CellType.TEXT:
                                b.interactable = false;
                                b.image = null;
                                ingridcell.transform.GetChild(0).GetComponent<Text>().text = cell["content"].ToString();
                                ingridcell.GetComponent<TabletButtonBehaviourSimulator>().Setup(false, compname, cell["code"].ToString());
                                break;
                            case CellType.IMAGE:
                                b.interactable = false;
                                b.image.sprite = imagefromBase64(cell["content"].ToString());
                                ingridcell.transform.GetChild(0).GetComponent<Text>().text = "";
                                ingridcell.GetComponent<TabletButtonBehaviourSimulator>().Setup(false, compname, cell["code"].ToString());
                                break;
                            case CellType.BUTTON_TEXT:
                                b.interactable = bool.Parse(cell["isEnabled"].ToString());
                                b.image = null;
                                ingridcell.transform.GetChild(0).GetComponent<Text>().text = cell["content"].ToString();
                                ingridcell.GetComponent<TabletButtonBehaviourSimulator>().Setup(true, compname, cell["code"].ToString());
                                break;
                            case CellType.BUTTON_IMAGE:
                                b.interactable = bool.Parse(cell["isEnabled"].ToString());
                                b.image.sprite = imagefromBase64(cell["content"].ToString());
                                ingridcell.transform.GetChild(0).GetComponent<Text>().text = "";
                                ingridcell.GetComponent<TabletButtonBehaviourSimulator>().Setup(true, compname, cell["code"].ToString());
                                break;
                            default: break;
                        }
                        j++;
                    }
                    break;
                case "PropertyLongTextBlock":
                    GameObject text_comp;
                    if (centralActivityBlock.transform.childCount < orderedschema.Count)
                    {
                        text_comp = GameObject.Instantiate(prefabGrid);
                        text_comp.transform.SetParent(centralActivityBlock.transform);
                        text_comp.GetComponent<RectTransform>().localScale = Vector2.one;
                        text_comp.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        text_comp = centralActivityBlock.transform.GetChild(i).gameObject;
                    }
                    GridLayoutGroup inerlayout2 = text_comp.GetComponent<GridLayoutGroup>();
                    inerlayout2.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                    float textwidth = centralActivityBlock.GetComponent<GridLayoutGroup>().cellSize.x * 0.9f;
                    string[] textes = sch["text"].ToString().Split("\n");
                    float textheight = centralActivityBlock.GetComponent<GridLayoutGroup>().cellSize.y / Mathf.Max(1,textes.Length);
                    inerlayout2.constraintCount = 1;
                    inerlayout2.cellSize = new Vector2(textwidth, textheight);
                    text_comp.name = compname;
                    foreach (string s in textes) {
                        GameObject inntertext = GameObject.Instantiate(prefabTabletInnerTextElement);
                        inntertext.transform.SetParent(text_comp.transform);
                        inntertext.GetComponent<RectTransform>().localScale = Vector2.one;
                        inntertext.GetComponent<Text>().text = s;
                    }

                    break;
                case "PropertySetBlock":
                    GameObject prop_comp;
                    if (centralActivityBlock.transform.childCount < orderedschema.Count)
                    {
                        prop_comp = GameObject.Instantiate(prefabGrid);
                        prop_comp.transform.SetParent(centralActivityBlock.transform);
                        prop_comp.GetComponent<RectTransform>().localScale = Vector2.one;
                        prop_comp.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        prop_comp = centralActivityBlock.transform.GetChild(i).gameObject;
                    }
                    GridLayoutGroup inerlayout3 = prop_comp.GetComponent<GridLayoutGroup>();
                    inerlayout3.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                    float propwidth = centralActivityBlock.GetComponent<GridLayoutGroup>().cellSize.x * 0.9f;
                    JArray props = (JArray)sch["propset"];
                    float propheight = centralActivityBlock.GetComponent<GridLayoutGroup>().cellSize.y / Mathf.Max(1, props.Count);
                    inerlayout3.constraintCount = 1;
                    inerlayout3.cellSize = new Vector2(propwidth, propheight);
                    prop_comp.name = compname;
                    int j3 = 0;
                    foreach (JObject s in props)
                    {
                        GameObject innerprop;
                        
                        if (prop_comp.transform.childCount < props.Count)
                        {
                            innerprop = GameObject.Instantiate(prefabTabletInnerSetvalueElement);
                            innerprop.transform.SetParent(prop_comp.transform);
                            innerprop.GetComponent<RectTransform>().localScale = Vector2.one;
                        }
                        else
                        {
                            innerprop = prop_comp.transform.GetChild(j3).gameObject;
                        }
                        PropType p = (PropType)Enum.Parse(typeof(PropType), s["type"].ToString());
                        switch (p) {
                            case PropType.STRING:
                                innerprop.GetComponent<TabletPropertySetterBehaviourSimulator>().Setup(p, compname, s["name"].ToString(), s["description"].ToString());
                                break;
                            case PropType.INT:
                                innerprop.GetComponent<TabletPropertySetterBehaviourSimulator>().Setup(p, compname, s["name"].ToString(), s["description"].ToString(), (int)s["min"], (int)s["max"]);
                                break;
                            case PropType.FLOAT:
                                innerprop.GetComponent<TabletPropertySetterBehaviourSimulator>().Setup(p, compname, s["name"].ToString(), s["description"].ToString(), (int)s["min"], (int)s["max"]);
                                break;
                            case PropType.ENUM:
                                List<string> opts = new List<string>();
                                foreach (string opt in s["range"]) {
                                    opts.Add(opt);
                                }
                                innerprop.GetComponent<TabletPropertySetterBehaviourSimulator>().Setup(p, compname, s["name"].ToString(), s["description"].ToString(), 0, 0, opts.ToArray());
                                break;
                        }
                        j3++;
                    }
                    break;
                default: break;
            }
            i++;
        }
    }

    private Sprite  imagefromBase64(string text) {
        byte[] data = System.Convert.FromBase64String(text);
        Texture2D texture = new Texture2D(0, 0);
        ImageConversion.LoadImage(texture, data);
        Sprite tempSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 0);
        return tempSprite;

    }

    public void FireEvent(string code) {
        JObject obj = new JObject();
        obj["blockName"] = "controlButton";
        JObject payload = new JObject();
        payload["id"] = code;
        obj["payload"] = payload;

        TabletVisualizationManagement.Instance.ManageHttpRequest(obj.ToString(), null);
    }
}
