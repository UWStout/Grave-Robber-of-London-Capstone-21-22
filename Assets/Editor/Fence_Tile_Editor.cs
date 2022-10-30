using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FenceTile))]
[CanEditMultipleObjects]
public class Fence_Tile_Editor : Editor
{
    private SerializedProperty Up;
    private SerializedProperty Down;
    private SerializedProperty Left;
    private SerializedProperty Right;
    
    void OnEnable()
    {
        Up = serializedObject.FindProperty("FenceUp");
        Left = serializedObject.FindProperty("FenceLeft");
        Right = serializedObject.FindProperty("FenceRight");
        Down = serializedObject.FindProperty("FenceDown");
        
    }
    private List<Quests> _Quests = new List<Quests>();
    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((FenceTile)target), typeof(FenceTile), false);
        GUI.enabled = true;
        
        serializedObject.Update();
        // Up
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(Up, new GUIContent("Up"));
        if (GUILayout.Button("Toggle"))
        {
            // Get target object
            var target = Up.serializedObject.targetObject;
            // Get it's type
            var type = target.GetType();
            // Get the field
            var field = type.GetField(Up.propertyPath);
            // Check if it is empty or null
            if (field != null)
            {
                // Take it's value and convert to gameobject
                GameObject value = (GameObject) field.GetValue(target);
                // Toggle it's active state
                value.SetActive(!value.activeSelf);
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        // Down
        EditorGUILayout.PropertyField(Down,new GUIContent("Down"));
        if (GUILayout.Button("Toggle"))
        {
            var target = Down.serializedObject.targetObject;
            var type = target.GetType();
            var field = type.GetField(Down.propertyPath);
            if (field != null)
            {
                GameObject value = (GameObject) field.GetValue(target);
                value.SetActive(!value.activeSelf);
            }
        }
        EditorGUILayout.EndHorizontal();
        // Left
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(Left,new GUIContent("Left"));
        if (GUILayout.Button("Toggle"))
        {
            var target = Left.serializedObject.targetObject;
            var type = target.GetType();
            var field = type.GetField(Left.propertyPath);
            if (field != null)
            {
                GameObject value = (GameObject) field.GetValue(target);
                value.SetActive(!value.activeSelf);
            }
        }
        EditorGUILayout.EndHorizontal();
        // Right
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(Right,new GUIContent("Right"));
        if (GUILayout.Button("Toggle"))
        {
            var target = Right.serializedObject.targetObject;
            var type = target.GetType();
            var field = type.GetField(Right.propertyPath);
            if (field != null)
            {
                GameObject value = (GameObject) field.GetValue(target);
                value.SetActive(!value.activeSelf);
            }
        }
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();

    }

}