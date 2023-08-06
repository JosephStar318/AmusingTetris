using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TetrisDebug))]
public class TetrisDebugEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TetrisDebug tetrisDebug = (TetrisDebug)target;
        if(GUILayout.Button("Spawn Random Block"))
        {
            tetrisDebug.SpawnRandomBlock();
        }
    }
}
