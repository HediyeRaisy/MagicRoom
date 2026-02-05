using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TabletVisualizationManagement))]
public class TabletVisualizationManagerEditor : Editor
{
    private SerializedProperty width;
    private SerializedProperty height;
    private SerializedProperty ControlButton;
    private SerializedProperty tabletCompoentns;
    TabletVisualizationManagement m;
    int selectedmode = 0;

    private void OnEnable()
    {
        m = (TabletVisualizationManagement)target;
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("heigth");
        ControlButton = serializedObject.FindProperty("ControlButton");

        // Reset in case of mistakes while programming the interfac
        /*m.codes = new List<string>();
        m.com = new List<string>();
        m.paras = new List<string>();*/
    }

    int positionX = 1, positionY= 1;
    string[] listoftypes = new string[] { "Grid", "Property setting", "LongTextBlock" };
    string fieldname = "";
    Vector2 scrollposition;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        float originalValue = EditorGUIUtility.labelWidth;
        EditorGUILayout.LabelField("Configure the tablet application", EditorStyles.boldLabel, GUILayout.Height(40));

        EditorGUILayout.IntSlider(width, 1, 3, "width of the grid: ");
        EditorGUILayout.IntSlider(height, 1, 3, "height of the grid: ");
        if (m.ControlButton == null || m.ControlButton.Length < 5) {
            m.ControlButton = new bool[5];
        }

        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth * 0.6f;
        m.ControlButton[0] = EditorGUILayout.Toggle("Activate repeat command", m.ControlButton[0]);
        m.ControlButton[1] = EditorGUILayout.Toggle("Activate previous command", m.ControlButton[1]);
        m.ControlButton[2] = EditorGUILayout.Toggle("Activate pause command", m.ControlButton[2]);
        m.ControlButton[3] = EditorGUILayout.Toggle("Activate next command", m.ControlButton[3]);
        m.ControlButton[4] = EditorGUILayout.Toggle("Activate skip command", m.ControlButton[4]);
        EditorGUIUtility.labelWidth = originalValue;

        GUILayout.Box(GUIContent.none, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.9f), GUILayout.Height(2));

        
        selectedmode = EditorGUILayout.Popup(selectedmode, listoftypes);
        
        fieldname = EditorGUILayout.TextField("Component name: ", fieldname);
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth * 0.5f;
        positionX = EditorGUILayout.IntSlider("Horizontal position in grid",positionX, 1, m.width);
        positionY = EditorGUILayout.IntSlider("Vertical position in grid", positionY, 1, m.heigth);
        EditorGUIUtility.labelWidth = originalValue;
        

        if (GUILayout.Button("Add new Component"))
        {
            m.codes.Add(fieldname);
            m.com.Add(listoftypes[selectedmode]);
            JObject par = new JObject();
            par["position"] = (positionY - 1) * m.width + positionX - 1;
            switch (listoftypes[selectedmode]) {
                case "Grid":
                    //m.tabletComponents.Add(fieldname, new GridTabletBlock(fieldname, (positionY - 1)* m.width + positionX -1));
                    break;
                case "Property setting":
                    break;
                case "LongTextBlock":
                    break;
                default:
                    break;
            }
            m.paras.Add(par.ToString());
            EditorUtility.SetDirty(target);
        }
        GUILayout.Box(GUIContent.none, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.9f), GUILayout.Height(2));

        EditorGUILayout.LabelField("compoentn inserted = " + m.tabletComponents.Count);

        scrollposition = EditorGUILayout.BeginScrollView(scrollposition, GUILayout.Width(EditorGUIUtility.currentViewWidth*0.9f), GUILayout.Height(100));
        for (int i = 0; i < m.codes.Count; i++) {
            GUILayout.Box(GUIContent.none, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.4f), GUILayout.Height(2));
            EditorGUILayout.LabelField("Name: " + m.codes.ElementAt(i));
            EditorGUILayout.LabelField("Type: " + m.com.ElementAt(i));
            EditorGUILayout.LabelField("Params: " + m.paras.ElementAt(i));
            if (GUILayout.Button("Remove")) {
                m.codes.RemoveAt(i);
                m.com.RemoveAt(i);
                m.paras.RemoveAt(i);

                EditorUtility.SetDirty(target);
            }

        }
        EditorGUILayout.EndScrollView();
        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
