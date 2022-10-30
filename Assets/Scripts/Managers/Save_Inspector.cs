#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Save))]
public class Save_Inspector : Editor
{
    private Save script;

    private void OnEnable()
    {
        script = (Save)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Save"))
        {
            script.SaveToJson();
        }
        if (GUILayout.Button("Load"))
        {
            script.LoadFromJson();
        }
    }
}
#endif
