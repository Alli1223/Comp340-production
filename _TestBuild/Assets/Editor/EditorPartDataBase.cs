using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorPartDataBase : EditorWindow
{

	static EditorPartDataBase Instance;
	PartsDataBase myData;
	const string dataPath = "Assets/Resources/Data/MechPartsDataBase.asset";
	Vector2 scrollPos;
    EditorDataBaseLayoutData layoutData;


	int currentTab;
	string[] tabNames = new string[5]{ "Head", "Upper Torso", "Lower Torso", "Legs", "Arms"};

    SerializedObject so;

	[MenuItem ("Content/Mech Parts Data Base")]
	public static void ShowWindow()
	{

		Instance = EditorWindow.GetWindow<EditorPartDataBase>(false, "Mech Parts", true);
		Instance.minSize = new Vector2(500f, 300f);
		Instance.myData = Instance.LoadData();
		Instance.so = new SerializedObject(Instance.myData);
		Instance.layoutData.Initialise(Instance.myData);
	}

    string[] weightClassNames = new string[3] {"Light", "Medium", "Heavy"};

	void OnGUI()
	{
		currentTab = GUILayout.Toolbar(currentTab, tabNames);

        SerializedProperty partTypeArray = so.FindProperty("mechPartType").GetArrayElementAtIndex(currentTab);
		scrollPos = GUILayout.BeginScrollView(scrollPos);
        int deleteAt = 0;
        bool delete = false;
        for (int i = 0; i < myData.mechPartType[currentTab].part.Length; i++)
        {
            SerializedProperty currentPartsArray = partTypeArray.FindPropertyRelative("part").GetArrayElementAtIndex(i);

            SerializedProperty currentPartName = currentPartsArray.FindPropertyRelative("name");


            layoutData.foldOutInfoPartsData[currentTab][i] = EditorGUILayout.Foldout(layoutData.foldOutInfoPartsData[currentTab][i], currentPartName.stringValue, true);

            if (layoutData.foldOutInfoPartsData[currentTab][i])
            {
                SerializedProperty currentPartWeightClass = currentPartsArray.FindPropertyRelative("weightClass");
                SerializedProperty currentPartWeightLimit = currentPartsArray.FindPropertyRelative("weightLimit");

                SerializedProperty currentPartArmor = currentPartsArray.FindPropertyRelative("armor");
                SerializedProperty currentPartAP = currentPartsArray.FindPropertyRelative("ap");

                SerializedProperty currentPartPassiveBonusToggle = currentPartsArray.FindPropertyRelative("passiveBonus").FindPropertyRelative("enabled");


                SerializedProperty currentPartHasSkill = currentPartsArray.FindPropertyRelative("hasSkill");
                SerializedProperty currentPartSkillID = currentPartsArray.FindPropertyRelative("skillID");

                SerializedProperty currentPartAsset = currentPartsArray.FindPropertyRelative("asset");


                // NAME
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Name: ");
                currentPartName.stringValue = GUILayout.TextField(currentPartName.stringValue);
                GUILayout.EndHorizontal();

                // ARMOR | AP
                GUILayout.BeginHorizontal();
                currentPartArmor.intValue = EditorGUILayout.IntField("Armor:", currentPartArmor.intValue);
                currentPartAP.intValue = EditorGUILayout.IntField("AP:", currentPartAP.intValue);
                GUILayout.EndHorizontal();

                // WEIGHT
                EditorGUILayout.PropertyField(currentPartWeightClass, new GUIContent("Weight:"));
                EditorGUILayout.LabelField("Weight Limit:");
                GUILayout.BeginHorizontal();
                for (int c = 0; c < 3; c++)
                {
                    currentPartWeightLimit.GetArrayElementAtIndex(c).boolValue = GUILayout.Toggle(currentPartWeightLimit.GetArrayElementAtIndex(c).boolValue, weightClassNames[c]);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(10f);


                // BONUS
                currentPartPassiveBonusToggle.boolValue = GUILayout.Toggle(currentPartPassiveBonusToggle.boolValue, "Passive Bonus");

                if (currentPartPassiveBonusToggle.boolValue)
                {
                    SerializedProperty bonus = currentPartsArray.FindPropertyRelative("passiveBonus");

                    SerializedProperty bonusMovement = bonus.FindPropertyRelative("bonusMovement");
                    SerializedProperty bonusVision = bonus.FindPropertyRelative("bonusVision");
                    SerializedProperty separateDamage = bonus.FindPropertyRelative("separateDamage");
                    SerializedProperty separateAccuracy = bonus.FindPropertyRelative("separateAccuracy");
                    SerializedProperty bonusDamage = bonus.FindPropertyRelative("bonusDamage");
                    SerializedProperty bonusAccuracy = bonus.FindPropertyRelative("bonusAccuracy");



                    GUILayout.BeginHorizontal();
                    bonusMovement.intValue = EditorGUILayout.IntField("Movement:", bonusMovement.intValue);
                    bonusVision.intValue = EditorGUILayout.IntField("Vision:", bonusVision.intValue);
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10f);

                    // BONUS DAMAGE
                    separateDamage.boolValue = EditorGUILayout.Toggle("Separate Damage Bonus:", separateDamage.boolValue);

                    if (separateDamage.boolValue)
                    {
                        for (int c = 0; c < 8; c++)
                        {
                            WeaponClass wt = (WeaponClass)c;
                            bonusDamage.GetArrayElementAtIndex(c).intValue = EditorGUILayout.IntField(wt.ToString() + ":", bonusDamage.GetArrayElementAtIndex(c).intValue);
                        }
                    }
                    else
                    {
                        bonusDamage.GetArrayElementAtIndex(0).intValue = EditorGUILayout.IntField("Damage Bonus:", bonusDamage.GetArrayElementAtIndex(0).intValue);
                        for (int c = 1; c < 8; c++)
                        {
                            bonusDamage.GetArrayElementAtIndex(c).intValue = bonusDamage.GetArrayElementAtIndex(0).intValue;
                        }
                    }


                    GUILayout.Space(10f);
                    separateAccuracy.boolValue = EditorGUILayout.Toggle("Separate Accuracy Bonus:", separateAccuracy.boolValue);

                    if (separateAccuracy.boolValue)
                    {
                        for (int c = 0; c < 8; c++)
                        {
                            WeaponClass wt = (WeaponClass)c;
                            bonusAccuracy.GetArrayElementAtIndex(c).floatValue = EditorGUILayout.FloatField(wt.ToString() + ":", bonusAccuracy.GetArrayElementAtIndex(c).floatValue);
                        }
                    }
                    else
                    {
                        bonusAccuracy.GetArrayElementAtIndex(0).floatValue = EditorGUILayout.FloatField("Accuracy Bonus:", bonusAccuracy.GetArrayElementAtIndex(0).floatValue);
                        for (int c = 1; c < 8; c++)
                        {
                            bonusAccuracy.GetArrayElementAtIndex(c).floatValue = bonusAccuracy.GetArrayElementAtIndex(0).floatValue;
                        }
                    }
                }



                GUILayout.BeginHorizontal();
                currentPartHasSkill.boolValue = GUILayout.Toggle(currentPartHasSkill.boolValue, "Active Skill");

                if (currentPartHasSkill.boolValue)
                {
                    currentPartSkillID.intValue = EditorGUILayout.IntField("Skill ID:", currentPartSkillID.intValue);
                }

                GUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(currentPartAsset);

                GUILayout.Space(5f);
			
                GUI.color = Color.red;

                GUILayout.BeginHorizontal();
                GUILayout.Space(Screen.width * 0.3f);
                if (GUILayout.Button("Remove"))
                {
                    deleteAt = i;
                    delete = true;

                }
                GUILayout.Space(Screen.width * 0.3f);
                GUILayout.EndHorizontal();
                GUI.color = Color.white;
                
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }

		

        if(GUILayout.Button("Add new"))
        {
            int length = myData.mechPartType[currentTab].part.Length;
            MechPart[] newArray = new MechPart[length + 1];

            for(int i = 0; i < length; i++)
            {
                newArray[i] = myData.mechPartType[currentTab].part[i];
            }

            newArray[length] = new MechPart((PartType)currentTab);
            myData.mechPartType[currentTab].part = newArray;
            layoutData.foldOutInfoPartsData[currentTab].Add(true);
        }

        GUILayout.EndScrollView();

        if (delete)
        {
            partTypeArray.FindPropertyRelative("part").DeleteArrayElementAtIndex(deleteAt);
            layoutData.foldOutInfoPartsData[currentTab].RemoveAt(deleteAt);
        }

        so.ApplyModifiedProperties();
        so.UpdateIfRequiredOrScript();

	}
    const string layoutPath = "Assets/Editor/EditorData/DataBaseLayout.asset";

	PartsDataBase LoadData()
	{
        
		PartsDataBase data = AssetDatabase.LoadAssetAtPath<PartsDataBase> (dataPath);
		if (data == null) 
		{
			AssetDatabase.CreateAsset(new PartsDataBase(), dataPath);
			data = AssetDatabase.LoadAssetAtPath<PartsDataBase> (dataPath);
		}

        layoutData = AssetDatabase.LoadAssetAtPath<EditorDataBaseLayoutData>(layoutPath);
        if (layoutData == null)
        {
            AssetDatabase.CreateAsset(new EditorDataBaseLayoutData(), layoutPath);
            layoutData = AssetDatabase.LoadAssetAtPath<EditorDataBaseLayoutData>(layoutPath);
        }

		return data;
	}

}
