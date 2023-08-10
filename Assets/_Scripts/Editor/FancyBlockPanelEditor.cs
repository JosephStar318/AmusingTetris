using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FancyBlockPanel))]
public class FancyBlockPanelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("SetRandomColors"))
        {
            FancyBlockPanel fancyBlockPanel = (FancyBlockPanel)target;

            fancyBlockPanel.SetUpColors();

        }
        if (GUILayout.Button("Shuffle"))
        {
            FancyBlockPanel fancyBlockPanel = (FancyBlockPanel)target;

            fancyBlockPanel.ShuffleChildren();

        }
    }
}
