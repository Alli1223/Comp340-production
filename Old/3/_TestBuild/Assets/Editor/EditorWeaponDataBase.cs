using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorWeaponDataBase : EditorWindow 
{
    public static EditorWeaponDataBase Instance;

    EditorDataBaseLayoutData layoutData;

    WeaponDataBase myData;

    const string dataPath = "Assets/Resources/Data/WeaponDataBase.asset";
    string[] tabWpnTypeNames = new string[8] {"Autocannon","Chaingun","Mortar","Missile","Rocket","Scatter","Sniper","Snub"};
    int tabWpnTypeID;

    SerializedObject so;
    SerializedProperty weaponTypeArray;
    SerializedProperty weaponAssetArray;
    Vector2 scrollPos;

    [MenuItem ("Content/Weapon Data Base")]
    public static void ShowWindow()
    {
        
        Instance = EditorWindow.GetWindow<EditorWeaponDataBase>(false, "Weapon Data Base", true);
        Instance.minSize = new Vector2(700f, 500f);
        Instance.myData = Instance.LoadData();
        Instance.so = new SerializedObject(Instance.myData);
        Instance.layoutData.Initialise(Instance.myData);
    }
        

    void OnGUI()
    {
        //EditorUtility.SetDirty(layoutData);

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

            SerializedProperty idName = weaponAssetArray.FindPropertyRelative("name");

            layoutData.foldOutInfoWeaponData[tabWpnTypeID][c] = EditorGUILayout.Foldout(layoutData.foldOutInfoWeaponData[tabWpnTypeID][c], idName.stringValue, true);

            if (layoutData.foldOutInfoWeaponData[tabWpnTypeID][c])
            {
                SerializedProperty weaponAsset = weaponAssetArray.FindPropertyRelative("weaponAsset");
                SerializedProperty gimbalAsset = weaponAssetArray.FindPropertyRelative("weaponGimbalMountAsset");
                SerializedProperty staticAsset = weaponAssetArray.FindPropertyRelative("weaponStaticMountAsset");

                SerializedProperty weaponMinDmg = weaponAssetArray.FindPropertyRelative("minDamage");
                SerializedProperty weaponMaxDmg = weaponAssetArray.FindPropertyRelative("maxDamage");
                SerializedProperty weaponApCost = weaponAssetArray.FindPropertyRelative("apCost");
                SerializedProperty weaponNumberOfShots = weaponAssetArray.FindPropertyRelative("numberOfShots");

                SerializedProperty weaponSplashRadius = weaponAssetArray.FindPropertyRelative("splashRadius");
                SerializedProperty weaponScatter = weaponAssetArray.FindPropertyRelative("scatter");

                SerializedProperty weaponMinRange = weaponAssetArray.FindPropertyRelative("minRange");
                SerializedProperty weaponMaxRange = weaponAssetArray.FindPropertyRelative("maxRange");
                SerializedProperty weaponWeightClass = weaponAssetArray.FindPropertyRelative("weightClass");
                SerializedProperty weaponAccuracy = weaponAssetArray.FindPropertyRelative("accuracy");

                EditorGUI.indentLevel += 1;
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.PropertyField(idName, new GUIContent("Name: "));
                    EditorGUILayout.PropertyField(weaponWeightClass, new GUIContent("Weight Class"));

                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(10f);

                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField("Damage: ");
                    weaponMinDmg.intValue = EditorGUILayout.IntField("Min", weaponMinDmg.intValue);
                    weaponMaxDmg.intValue = EditorGUILayout.IntField("Max", weaponMaxDmg.intValue);

                    EditorGUILayout.EndHorizontal();
                }

                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField("Range: ");
                    weaponMinRange.intValue = EditorGUILayout.IntField("Min", weaponMinRange.intValue);
                    weaponMaxRange.intValue = EditorGUILayout.IntField("Max", weaponMaxRange.intValue);


                    EditorGUILayout.EndHorizontal();
                }

                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.PropertyField(weaponNumberOfShots, new GUIContent("Number of shots:"));
                    weaponSplashRadius.intValue = EditorGUILayout.IntField("Splash:", weaponSplashRadius.intValue);


                    EditorGUILayout.EndHorizontal();
                }

                {
                    EditorGUILayout.BeginHorizontal();
                    weaponApCost.intValue = EditorGUILayout.IntField("AP Cost:", weaponApCost.intValue);
                    weaponScatter.intValue = EditorGUILayout.IntField("Scatter:", weaponScatter.intValue);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();
                weaponAccuracy.floatValue = EditorGUILayout.FloatField("Accuracy:", weaponAccuracy.floatValue);
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();




                GUILayout.Space(20f);
                // VISUAL SHIT
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(Screen.width * 0.5f);
                EditorGUILayout.LabelField("Visual Assets:");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(weaponAsset, new GUIContent("Asset: "));
                EditorGUILayout.PropertyField(gimbalAsset, new GUIContent("Gimbal Mount: "));
                EditorGUILayout.PropertyField(staticAsset, new GUIContent("Static Mount: "));
            

                EditorGUI.indentLevel -= 1;

                GUI.color = Color.red;
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(Screen.width * 0.4f);
                if (GUILayout.Button("Remove"))
                {
                    deleteAt = c;
                    delete = true;

                }

                GUILayout.Space(Screen.width * 0.4f);
                EditorGUILayout.EndHorizontal();
                GUI.color = Color.white;
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }

        if (delete)
        {
            weaponTypeArray.FindPropertyRelative("weapons").DeleteArrayElementAtIndex(deleteAt);
            layoutData.foldOutInfoWeaponData[tabWpnTypeID].RemoveAt(deleteAt);
        }

        if (GUILayout.Button("Add New"))
        {
            int length = myData.weaponTypeArray[tabWpnTypeID].weapons.Length;
            Weapon[] newArray = new Weapon[length + 1];

            for (int i = 0; i < length; i++)
            {
                newArray[i] = myData.weaponTypeArray[tabWpnTypeID].weapons[i];
            }
                
                
            myData.weaponTypeArray[tabWpnTypeID].weapons = newArray;
            myData.weaponTypeArray[tabWpnTypeID].weapons[length].weaponAsset = null;
            myData.weaponTypeArray[tabWpnTypeID].weapons[length].weaponGimbalMountAsset = null;
            myData.weaponTypeArray[tabWpnTypeID].weapons[length].weaponStaticMountAsset = null;
            scrollPos = Instance.maxSize;

            layoutData.foldOutInfoWeaponData[tabWpnTypeID].Add(true);
        }
            
        EditorGUILayout.EndScrollView();


        so.ApplyModifiedProperties();
        so.UpdateIfRequiredOrScript();
    }
        

    bool assetAlreadyExists;

    const string layoutPath = "Assets/Editor/EditorData/DataBaseLayout.asset";

    WeaponDataBase LoadData()
    {
        layoutData = AssetDatabase.LoadAssetAtPath<EditorDataBaseLayoutData>(layoutPath);
        if(layoutData == null)
        {
            AssetDatabase.CreateAsset(new EditorDataBaseLayoutData(), layoutPath);
            layoutData = AssetDatabase.LoadAssetAtPath<EditorDataBaseLayoutData>(layoutPath);

        }

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


