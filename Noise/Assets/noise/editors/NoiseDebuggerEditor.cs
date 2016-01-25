using System;
//#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(NoiseDebugger))]
public class NoiseDebuggerEditor : Editor
{
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(NoiseDebuggerEditor));
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        NoiseDebugger noiseDebugger = target as NoiseDebugger;
        GUILayout.BeginVertical();
        //a group of horizontal items.
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("GenerateNoiseTexture"))
        {
            noiseDebugger.GenerateTexture();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}
//#endif
