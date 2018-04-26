using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorWeaponDataBase : EditorWindow 
{
    public static EditorWeaponDataBase Instance;

    WeaponDataBase myData;

    const string dataPath = "Assets/Resources/Data/WeaponDataBase.asset";
    string[] tabWpnTypeNames = new string[8] {"Autocannon","Chaingun","Mortar","Missile Silo","Rocket Silo","Scatter Cannon","Sniper Cannon","Snub Cannon"};
    int tabWpnTypeID;

    SerializedObject so;
    SerializedProperty weaponTypeArray;
    SerializedProperty weaponAssetArray;
    Vector2 scrollPos;
	
    [MenuItem ("Utilities/Weapon Data Base")]
    public static void ShowWindow()
    {
        Instance = EditorWindow.GetWindow<EditorWeaponDataBase>(false,"Weapon Data Base", true);
        Instance.myData = Instance.LoadData();
        Instance.so = new SerializedObject(Instance.myData);
    }
        

    void OnGUI()
    {
        if(GUILayout.Button("Save"))
        {
            SaveData(myData);
        }

        tabWpnTypeID = GUILayout.Toolbar(tabWpnTypeID, tabWpnTypeNames);
        weaponTypeArray = so.FindProperty("weaponTypeArray").GetArrayElementAtIndex(tabWpnTypeID);
        bool delete = false;
        int deleteAt = 0;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int c = 0; c < myData.weaponTypeArray[tabWpnTypeID].weapons.Length; c++)
        {
            weaponAssetArray = weaponTypeArray.FindPropertyRelative("weapons").GetArrayElementAtIndex(c);

            EditorGUILayout.LabelField(c.ToString());
            EditorGUI.indentLevel += 1;

            SerializedProperty idName = weaponAssetArray.FindPropertyRelative("identifierName");
            SerializedProperty weaponAsset = weaponAssetArray.FindPropertyRelative("weaponAsset");
            SerializedProperty gimbalAsset = weaponAssetArray.FindPropertyRelative("weaponGimbalMountAsset");
            SerializedProperty staticAsset = weaponAssetArray.FindPropertyRelative("weaponStaticMountAsset");

            EditorGUILayout.PropertyField(idName, new GUIContent("Identifier Name: "));
            EditorGUILayout.PropertyField(weaponAsset, new GUIContent("Asset: "));
            EditorGUILayout.PropertyField(gimbalAsset, new GUIContent("Gimbal Mount: "));
            EditorGUILayout.PropertyField(staticAsset, new GUIContent("Static Mount: "));

            EditorGUI.indentLevel -= 1;


            if (GUILayout.Button("Remove"))
            {
                deleteAt = c;
                delete = true;
            }
        }

        if (delete)
        {
            weaponTypeArray.FindPropertyRelative("weapons").DeleteArrayElementAtIndex(deleteAt);
            delete = false;
        }

        if (GUILayout.Button("Add New"))
        {
            int length = myData.weaponTypeArray[tabWpnTypeID].weapons.Length;
            WeaponAsset[] newArray = new WeaponAsset[length + 1];

            for (int i = 0; i < length; i++)
            {
                newArray[i] = myData.weaponTypeArray[tabWpnTypeID].weapons[i];
            }
                
            myData.weaponTypeArray[tabWpnTypeID].weapons = newArray;
            myData.weaponTypeArray[tabWpnTypeID].weapons[length].weaponAsset = null;
            myData.weaponTypeArray[tabWpnTypeID].weapons[length].weaponGimbalMountAsset = null;
            myData.weaponTypeArray[tabWpnTypeID].weapons[length].weaponStaticMountAsset = null;
            scrollPos = Instance.maxSize;
        }
            
        Debug.Log(scrollPos);
        EditorGUILayout.EndScrollView();


        so.ApplyModifiedProperties();
        so.UpdateIfRequiredOrScript();
    }
        

    bool assetAlreadyExists;

    WeaponDataBase LoadData()
    {
        WeaponDataBase loadData = AssetDatabase.LoadAssetAtPath<WeaponDataBase>(dataPath);
        if (loadData != null)
        {
            assetAlreadyExists = true;
            return loadData;
        }
        else
        {
            assetAlreadyExists = false;
            return ScriptableObject.CreateInstance<WeaponDataBase>();
        }
    }

    void SaveData(WeaponDataBase toSave)
    {
        if (assetAlreadyExists)
        {   
            AssetDatabase.SaveAssets();
        }
        else
        {
            AssetDatabase.CreateAsset(toSave, dataPath);
        }

    }
	
}


