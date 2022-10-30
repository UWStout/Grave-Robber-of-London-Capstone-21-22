using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grave_Tile_Inspector))]
[CanEditMultipleObjects]
public class Grave_Tile_Editor : Editor
{
    private SerializedProperty LowGrave;
    private SerializedProperty MidGrave;
    private SerializedProperty HighGrave;
    private SerializedProperty Headstone_object;
    
    
    private GameObject[] LowGraves;
    private GameObject[] MidGraves;
    private GameObject[] HighGraves;
    private GameObject Headstone;
    void OnEnable()
    {
        LowGrave = serializedObject.FindProperty("LowGraves");
        MidGrave = serializedObject.FindProperty("MidGraves");
        HighGrave = serializedObject.FindProperty("HighGraves");
        Headstone_object = serializedObject.FindProperty("HeadStone");
        
        // Get target object
        var target = Headstone_object.serializedObject.targetObject;
        // Get it's type
        var type = target.GetType();
        // Get the field
        var field = type.GetField(Headstone_object.propertyPath);
        // Check if it is empty or null
        if (field != null)
        {
            // Take it's value and convert to gameobject
            Headstone = (GameObject) field.GetValue(target);
        }
        
        LowGraves = GetGraves(LowGrave);
        MidGraves = GetGraves(MidGrave);
        HighGraves = GetGraves(HighGrave);
    }

    private GameObject[] GetGraves(SerializedProperty GraveArray)
    {
        List<GameObject> Graves = new List<GameObject>();
        for (int i = 0; i < GraveArray.arraySize; i++)
        {
            Graves.Add((GameObject)GraveArray.GetArrayElementAtIndex(i).objectReferenceValue);
        }

        return Graves.ToArray();
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        serializedObject.Update();
        EditorGUILayout.BeginVertical();
        {
            if (GUILayout.Button("Random Low Tier"))
            {
                RandomLowTier();
            }
            if (GUILayout.Button("Random Medium Tier"))
            {
                RandomMediumTier();
            }
            if (GUILayout.Button("Random High Tier"))
            {
                RandomHighTier();
            }
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }

    private void RandomLowTier()
    {
        //0-2 inclusive
        DestroyChildren();
        int grave = Random.Range(0, LowGraves.Length);
        Instantiate(LowGraves[grave], Headstone.transform);
    }

    private void RandomMediumTier()
    {
        //3-5 inclusive
        DestroyChildren();
        int grave = Random.Range(0, MidGraves.Length);
        Instantiate(MidGraves[grave], Headstone.transform);
    }

    private void RandomHighTier()
    {
        //6-8 inclusive
        Debug.Log("High Tier Grave");
        Debug.LogError("Not Yet Implemented");
        RandomLowTier();
    }

    private void DestroyChildren()
    {
        foreach (Transform child in Headstone.transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
    }
}
