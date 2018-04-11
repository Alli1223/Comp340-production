using System.Collections;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEditor;
using System;

//[Serializable]
public sealed class EditorReferencePose : EditorWindow 
{		
	EditorReferencePoseData myData;
    static EditorReferencePose Instance;

    const string dataPath = "Assets/Editor/EditorData/referencePoseData.asset";
    bool assetAlreadyExists;

	SerializedProperty toOverrideProperty;
	SerializedObject so;

	[MenuItem ("Utilities/Reference Pose Override")]
	public static void ShowWindow()
	{
        Instance = EditorWindow.GetWindow<EditorReferencePose>(false, "Pose Override", true);
        Instance.myData = Instance.LoadData();
        Instance.so = new SerializedObject(Instance.myData);
	}

    string[] propertyNames = new string[5] {"3JointLegs", "4JointLegs", "1JointArms", "2JointArms", "3JointArms"};
	string[] tabNames = new string[5] { "3 Joint Legs", "4 Joint Legs", "1 Joint Arms", "2 Joint Arms", "3 Joint Arms" };
	int tab = 0;


	void OnGUI()
	{
        tab = GUILayout.Toolbar(tab, tabNames);


        if(GUILayout.Button("Save"))
        {
            SaveData(myData);
        }
        if (GUILayout.Button("Apply"))
        {
            Apply(tab);
        }

        GUILayout.Label ("Additive Pose Reference Set up", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField (so.FindProperty ("sampleClip" + propertyNames[tab]), new GUIContent("Sample Clip"));
        EditorGUILayout.PropertyField (so.FindProperty ("sampleTime" + propertyNames[tab]), new GUIContent("Sample Time"));


        toOverrideProperty = so.FindProperty("toOverride" + propertyNames[tab]);
        //Debug.Log(toOverrideProperty.objectReferenceValue);
        EditorGUILayout.PropertyField (toOverrideProperty, new GUIContent("Clips to override"), true);

        so.ApplyModifiedProperties ();

	}

    void Apply(int id)
    {
        switch (id)
        {
            case 0:
                for (int i = 0; i < myData.toOverride3JointLegs.Length; i++)
                {
                    AnimationUtility.SetAdditiveReferencePose(myData.toOverride3JointLegs[i], myData.sampleClip3JointLegs, myData.sampleTime3JointLegs);
                }
                break;
            case 1:
                for (int i = 0; i < myData.toOverride4JointLegs.Length; i++)
                {
                    AnimationUtility.SetAdditiveReferencePose(myData.toOverride4JointLegs[i], myData.sampleClip4JointLegs, myData.sampleTime4JointLegs);
                }
                break;
            case 2:
                for (int i = 0; i < myData.toOverride1JointArms.Length; i++)
                {
                    AnimationUtility.SetAdditiveReferencePose(myData.toOverride1JointArms[i], myData.sampleClip1JointArms, myData.sampleTime1JointArms);
                };
                break;
            case 3:
                for (int i = 0; i < myData.toOverride2JointArms.Length; i++)
                {
                    AnimationUtility.SetAdditiveReferencePose(myData.toOverride2JointArms[i], myData.sampleClip2JointArms, myData.sampleTime2JointArms);
                };
                break;
            case 4:
                for (int i = 0; i < myData.toOverride3JointArms.Length; i++)
                {
                    AnimationUtility.SetAdditiveReferencePose(myData.toOverride3JointArms[i], myData.sampleClip3JointArms, myData.sampleTime3JointArms);
                };
                break;
        }

    }

    EditorReferencePoseData LoadData()
    {
        EditorReferencePoseData loadData = AssetDatabase.LoadAssetAtPath<EditorReferencePoseData>(dataPath);
        if (loadData != null)
        {
            assetAlreadyExists = true;
            return loadData;
        }
        else
        {
            assetAlreadyExists = false;
            return ScriptableObject.CreateInstance<EditorReferencePoseData>();
        }
    }

    void SaveData(EditorReferencePoseData toSave)
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


