using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorColorArray : EditorWindow 
{
    static EditorColorArray Instance;
    const string dataPath = "Assets/Resources/Data/MechColors.asset";
    MechColorArray myData;

    SerializedObject so;

    [MenuItem("Utilities/Mech Colors")]
    static void ShowWindow()
    {
        Instance = EditorWindow.GetWindow<EditorColorArray>(false, "Mech Colors", true);
        Instance.myData = Instance.Load();
        Instance.so = new SerializedObject(Instance.myData);
    }

    Vector2 scrollPos;

    void OnGUI()
    {
        bool delete = false;
        int deleteAt = 0;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        SerializedProperty colorArray = so.FindProperty("color");
        SerializedProperty nameArray = so.FindProperty("colorName");

        for (int i = 0; i < colorArray.arraySize; i++)
        {
            SerializedProperty tempColor = colorArray.GetArrayElementAtIndex(i);
            SerializedProperty tempName = nameArray.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginHorizontal();

            tempName.stringValue = EditorGUILayout.TextField(tempName.stringValue);
            tempColor.colorValue = EditorGUILayout.ColorField(tempColor.colorValue);
            if (GUILayout.Button("Delete"))
            {
                delete = true;
                deleteAt = i;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add new"))
        {
            int length = colorArray.arraySize;
            colorArray.InsertArrayElementAtIndex(length);
            nameArray.InsertArrayElementAtIndex(length);
        }

        if (delete)
        {
            colorArray.DeleteArrayElementAtIndex(deleteAt);
            nameArray.DeleteArrayElementAtIndex(deleteAt);
        }

        EditorGUILayout.EndScrollView();

        so.ApplyModifiedProperties();
        so.UpdateIfRequiredOrScript();
    }

    MechColorArray Load()
    {
        MechColorArray ret = AssetDatabase.LoadAssetAtPath<MechColorArray>(dataPath);
        if (ret == null)
        {
            ret = new MechColorArray();
            AssetDatabase.CreateAsset(ret, dataPath);
            return AssetDatabase.LoadAssetAtPath<MechColorArray>(dataPath);
        }
        else
        {
            return ret;
        }
    }
}
