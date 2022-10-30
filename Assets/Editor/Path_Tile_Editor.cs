using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.WSA;


[CustomEditor(typeof(PathTile))]
[CanEditMultipleObjects]
public class Path_Tile_Editor : Editor
{
    private SerializedProperty MaterialArray;
    private SerializedProperty Tiles;
    
    private Dictionary<string, Material> PathMaterials;
    
    private GameObject TL;
    private GameObject TR;
    private GameObject BL;
    private GameObject BR;
    
    private void OnEnable()
    {
        MaterialArray = serializedObject.FindProperty("PathMaterials");
        Tiles = serializedObject.FindProperty("ChildTiles");
        PathMaterials = GetMaterials();
        AssignTiles();
    }

    private void AssignTiles()
    {
        for (int i = 0; i < Tiles.arraySize; i++)
        {
            switch (Tiles.GetArrayElementAtIndex(i).objectReferenceValue.name)
            {
                case "TL":
                    TL = (GameObject)Tiles.GetArrayElementAtIndex(i).objectReferenceValue;
                    break;
                case "TR":
                    TR = (GameObject)Tiles.GetArrayElementAtIndex(i).objectReferenceValue;
                    break;
                case "BL":
                    BL = (GameObject)Tiles.GetArrayElementAtIndex(i).objectReferenceValue;
                    break;
                case "BR":
                    BR = (GameObject)Tiles.GetArrayElementAtIndex(i).objectReferenceValue;
                    break;
            }
        }
    }
    
    private Dictionary<string, Material> GetMaterials()
    {
        Dictionary<string, Material> mats = new Dictionary<string, Material>();
        for (int i = 0; i < MaterialArray.arraySize; i++)
        {
            Material mat = (Material)MaterialArray.GetArrayElementAtIndex(i).objectReferenceValue;
            mats[mat.name] = mat;
        }

        return mats;
    }
    
    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();
        
        DrawLine();

        EditorGUILayout.LabelField("3-Way Intersection");
        // T intersection
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("⊤"))
                {
                    AssignMaterials(PathMaterials["Bottom"], PathMaterials["Bottom"], PathMaterials["Upper_Right"], PathMaterials["Upper_Left"]);
                }
                if (GUILayout.Button("⊢"))
                {
                    AssignMaterials(PathMaterials["Right"], PathMaterials["Bottom_Left"], PathMaterials["Right"], PathMaterials["Upper_Left"]);
                }
                
                
                
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("⊥"))
                {
                    AssignMaterials(PathMaterials["Bottom_Right"], PathMaterials["Bottom_Left"], PathMaterials["Upper"], PathMaterials["Upper"]);
                }
                if (GUILayout.Button("⊣"))
                {
                    AssignMaterials(PathMaterials["Bottom_Right"], PathMaterials["Left"], PathMaterials["Upper_Right"], PathMaterials["Left"]);
                }
                
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        
        DrawLine();
        
        EditorGUILayout.LabelField("Dead End");
        // Dead End
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("⊓"))
                {
                    AssignMaterials(PathMaterials["Upper_Left_InnerCorner"], PathMaterials["Upper_Right_InnerCorner"], PathMaterials["Right"], PathMaterials["Left"]);
                }
                if (GUILayout.Button("⊏"))
                {
                    AssignMaterials(PathMaterials["Upper_Left_InnerCorner"], PathMaterials["Bottom"], PathMaterials["Bottom_Left_InnerCorner"], PathMaterials["Upper"]);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                
                if (GUILayout.Button("⊔"))
                {
                    AssignMaterials(PathMaterials["Right"], PathMaterials["Left"], PathMaterials["Bottom_Left_InnerCorner"], PathMaterials["Bottom_Right_InnerCorner"]);
                }
                if (GUILayout.Button("⊐"))
                {
                    AssignMaterials(PathMaterials["Bottom"], PathMaterials["Upper_Right_InnerCorner"], PathMaterials["Upper"], PathMaterials["Bottom_Right_InnerCorner"]);
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        
        DrawLine();
        
        // Dead End
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("∟"))
                {
                    AssignMaterials(PathMaterials["Upper_Left_InnerCorner"], PathMaterials["Bottom"], PathMaterials["Right"], PathMaterials["Upper_Left"]);
                }
                if (GUILayout.Button("∟"))
                {
                    AssignMaterials(PathMaterials["Right"], PathMaterials["Bottom_Left"], PathMaterials["Bottom_Left_InnerCorner"], PathMaterials["Upper"]);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                
                if (GUILayout.Button("∟"))
                {
                    AssignMaterials(PathMaterials["Bottom"], PathMaterials["Upper_Right_InnerCorner"], PathMaterials["Upper_Right"], PathMaterials["Left"]);
                }
                if (GUILayout.Button("∟"))
                {
                    AssignMaterials(PathMaterials["Bottom_Right"], PathMaterials["Left"], PathMaterials["Upper"], PathMaterials["Bottom_Right_InnerCorner"]);
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        
        DrawLine();
        
        EditorGUILayout.LabelField("Straight");
        // Straight
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("|"))
                {
                    AssignMaterials(PathMaterials["Right"], PathMaterials["Left"], PathMaterials["Right"], PathMaterials["Left"]);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                
                if (GUILayout.Button("―"))
                {
                    AssignMaterials(PathMaterials["Bottom"], PathMaterials["Bottom"], PathMaterials["Upper"], PathMaterials["Upper"]);
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        
        DrawLine();
        
        EditorGUILayout.LabelField("4-Way Intersection");
        // Cross intersection
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("+"))
            {
                AssignMaterials(PathMaterials["Bottom_Right"], PathMaterials["Bottom_Left"], PathMaterials["Upper_Right"], PathMaterials["Upper_Left"]);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawLine()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.EndHorizontal();
    }

    private void AssignMaterials(Material TLM, Material TRM, Material BLM, Material BRM)
    {
        TL.GetComponent<Renderer>().material = TLM;
        TR.GetComponent<Renderer>().material = TRM;
        BL.GetComponent<Renderer>().material = BLM;
        BR.GetComponent<Renderer>().material = BRM;
    }
}
