using System.Collections;
using System.Collections.Generic;
using System;
using System.Resources;
using System.Linq;
using UnityEditor;
using UnityEngine;



public class EditorAdditivePoseGeneratorLT : EditorWindow 
{

	static EditorAdditivePoseGeneratorLT Instance;
	const string dataPath = "Assets/Resources/Data/LTRigAssembler.asset";
	const string animClipDataPath = "Assets/Resources/Data/ProceduralAnimPoses/LT/";
	const string assetPathLT = "LT";
	const string assetPathLG = "LG";
	bool assetAlreadyExists;
	SerializedObject so;
    LTRigAssembler myData;
    bool foldOut;

	[MenuItem ("Utilities/Additive Pose Generator - LT")]
	public static void ShowWindow()
	{
		Instance = EditorWindow.GetWindow<EditorAdditivePoseGeneratorLT>(false,"LT Pose Generator", true);
		Instance.myData = Instance.LoadData ();
		Instance.so = new SerializedObject (Instance.myData);
		Instance.LTData = Instance.so.FindProperty("LTData");
		Instance.LGData = Instance.so.FindProperty("LGData");
		Instance.lowerTorsoBaseSkeletonPrefab = Instance.so.FindProperty("LowerTorsoBaseSkeleton");
        Instance.LTAnimControler = Instance.so.FindProperty("LTAnimationController");
	}

    int animSetTab = 0;
    string[] animTabNames = new string[2] {"3 Joint Leg", "4 Joint Leg"};
	int tab = 0;
	string[] tabNames = new string[2] { "Lower Torso", "Legs"};
	Vector2 scrollPos;
	SerializedProperty LTData;
	SerializedProperty LGData;
	SerializedProperty lowerTorsoBaseSkeletonPrefab;
    SerializedProperty LTAnimControler;

	void OnGUI()
	{
        EditorGUILayout.PropertyField(lowerTorsoBaseSkeletonPrefab, new GUIContent("Base Rig"));
        EditorGUILayout.PropertyField(LTAnimControler, new GUIContent("Base Animation Controller"));
        foldOut = EditorGUILayout.Foldout(foldOut, "Base Animation Sets");
        if (foldOut)
        {
            animSetTab = GUILayout.Toolbar(animSetTab, animTabNames);
            if (animSetTab == 0)
            {
                EditorGUILayout.PropertyField(so.FindProperty("ThreeJointLeg_IDLE"), new GUIContent("Idle"));
                EditorGUILayout.PropertyField(so.FindProperty("ThreeJointLeg_walk_light"), new GUIContent("Walk - Light Weight"));
                EditorGUILayout.PropertyField(so.FindProperty("ThreeJointLeg_run_light"), new GUIContent("Run - Light Weight"));
                EditorGUILayout.PropertyField(so.FindProperty("ThreeJointLeg_walk_heavy"), new GUIContent("Walk - Heavy Weight"));
                EditorGUILayout.PropertyField(so.FindProperty("ThreeJointLeg_run_heavy"), new GUIContent("Run - Heavy Weight"));
                EditorGUILayout.PropertyField(so.FindProperty("ThreeJointLeg_turn5L"), new GUIContent("Turn 5 deg Left"));
                EditorGUILayout.PropertyField(so.FindProperty("ThreeJointLeg_turn90L"), new GUIContent("Turn 90 deg Left"));
                EditorGUILayout.PropertyField(so.FindProperty("ThreeJointLeg_turn5R"), new GUIContent("Turn 5 deg Right"));
                EditorGUILayout.PropertyField(so.FindProperty("ThreeJointLeg_turn90R"), new GUIContent("Turn 90 deg Right"));
            }
            else if (animSetTab == 1)
            {
                EditorGUILayout.PropertyField(so.FindProperty("FourJointLeg_IDLE"), new GUIContent("Idle"));
                EditorGUILayout.PropertyField(so.FindProperty("FourJointLeg_walk_light"), new GUIContent("Walk - Light Weight"));
                EditorGUILayout.PropertyField(so.FindProperty("FourJointLeg_run_light"), new GUIContent("Run - Light Weight"));
                EditorGUILayout.PropertyField(so.FindProperty("FourJointLeg_walk_heavy"), new GUIContent("Walk - Heavy Weight"));
                EditorGUILayout.PropertyField(so.FindProperty("FourJointLeg_run_heavy"), new GUIContent("Run - Heavy Weight"));
                EditorGUILayout.PropertyField(so.FindProperty("FourJointLeg_turn5L"), new GUIContent("Turn 5 deg Left"));
                EditorGUILayout.PropertyField(so.FindProperty("FourJointLeg_turn90L"), new GUIContent("Turn 90 deg Left"));
                EditorGUILayout.PropertyField(so.FindProperty("FourJointLeg_turn5R"), new GUIContent("Turn 5 deg Right"));
                EditorGUILayout.PropertyField(so.FindProperty("FourJointLeg_turn90R"), new GUIContent("Turn 90 deg Right"));
            }
        }
		if(GUILayout.Button("Save"))
		{
			SaveData (myData);
		}
		if (GUILayout.Button("Generate"))
		{
			GeneratePoses();
		}

		tab = GUILayout.Toolbar ( tab, tabNames);
		if (tab == 0) 
        {
			if (GUILayout.Button("Auto fill Lower Torso Assets"))
			{
				AutoFillLT();
			}
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400));
			EditorGUILayout.PropertyField(LTData, new GUIContent("Lower Torso Data"), true); 
			EditorGUILayout.EndScrollView();
		} 
		else if (tab == 1) 
		{
			if (GUILayout.Button("Auto fill Leg Assets"))
			{
				AutoFillLG();
			}
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400));
			EditorGUILayout.PropertyField(LGData, new GUIContent("Leg Data"), true); 
			EditorGUILayout.EndScrollView();
		}



		so.ApplyModifiedProperties ();
		so.UpdateIfRequiredOrScript();
	}

	void AutoFillLT()
	{
		GameObject[] loadedAssets = Resources.LoadAll(assetPathLT, typeof(GameObject)).Cast<GameObject>().ToArray();
		myData.LTData = new LTRigAssembler.LTinfo[loadedAssets.Length];
		LTData.arraySize = loadedAssets.Length;

		for (int i = 0; i < loadedAssets.Length; i++)
		{
			myData.LTData[i].asset = loadedAssets[i] as GameObject;
			if (!ValidateLTAsset(myData.LTData[i].asset))
			{
				Debug.LogError(myData.LTData[i].asset.name + " - This Lower Torso does not contain appropriate amount of children (3)");
			}
		}			

	}

	void AutoFillLG()
	{
		GameObject[] loadedAssets = Resources.LoadAll(assetPathLG, typeof(GameObject)).Cast<GameObject>().ToArray();
		myData.LGData = new LTRigAssembler.LGinfo[loadedAssets.Length];
		LGData.arraySize = loadedAssets.Length;

		for (int i = 0; i < loadedAssets.Length; i++)
		{
			myData.LGData[i].asset = loadedAssets[i] as GameObject;
			myData.LGData[i].legType = CheckLegType(myData.LGData[i].asset);
		}

	}

	bool ValidateLTAsset(GameObject go)
	{
		if (go.transform.childCount != 3)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

    LTRigAssembler.LegType CheckLegType(GameObject legAsset)
	{
		if (legAsset.transform.childCount != 2)
		{
			Debug.LogError(legAsset.name + " leg prefab contains: " + legAsset.transform.childCount + " children. Instead of 2");
		}
		else
		{
			int joints = 0;
			Transform tr = legAsset.transform.GetChild(0);
			for (int i = 0; i < 4; i++)
			{
				joints++;
				if (tr.childCount != 0)
				{
					tr = tr.GetChild(0);
				}
				else
				{
					if (joints == 3)
					{
						return LTRigAssembler.LegType.ThreeJoint;

					}
					else if (joints == 4)
					{
						return LTRigAssembler.LegType.FourJoint;

					}
					else
					{
						Debug.LogError(legAsset.name + " leg prefab contains: " + joints + " joints. Instead of 3 or 4");
						return LTRigAssembler.LegType.ThreeJoint;
					}
				}
			}
		}
		return LTRigAssembler.LegType.ThreeJoint;
	}



	void GeneratePoses()
	{
		// deleting existing clips
		//Animation[] existingClips = Resources.LoadAll("Data/ProceduralAnimPoses/LT", typeof(AnimationClip)).Cast<AnimationClip>().ToArray();
		object[] existingClips = Resources.LoadAll("Data/ProceduralAnimPoses/LT");
		for (int i = 0; i < existingClips.Length; i++)
		{
			AssetDatabase.DeleteAsset(animClipDataPath + existingClips[i].ToString() + ".anim");
		}

		//generating poses based on assetnames
		GameObject ltGO;
		GameObject lgGO;
		for (int ltID = 0; ltID < myData.LTData.Length; ltID++)
		{
			ltGO = myData.LTData[ltID].asset;
			for (int lgID = 0; lgID < myData.LGData.Length; lgID++)
			{
				lgGO = myData.LGData[lgID].asset;
				AssetDatabase.CreateAsset(GenerateClip(ltGO, lgGO, myData.LGData[lgID].legType), animClipDataPath + ltGO.name + "-" + lgGO.name + ".anim");
			}
		}
	}
	Transform[] tempSkeletonAvatar;
	Transform[] tempMeshAvatar;
	// for easier debugging you can use these to get specific transforms easier
	enum skelQA : int { LowerTorsoCore, UpperLegL, KneeL, LowerLegL, FootL, UpperLegR, KneeR, LowerLegR, FootR, UpperTorsoParent, LowerTorsoRoot};
	enum meshQA : int { LowerTorsoCore, UpperLegL, KneeL, LowerLegL, FootL, UpperLegR, KneeR, LowerLegR, FootR, ConnectorUT, ConnectorL, ConnectorR};


	void GenerateBoneHierarchy(GameObject skeletonInstance)
	{
		Transform curTransform = skeletonInstance.transform;
		curTransform = curTransform.GetChild(0); // this should select the root
		tempSkeletonAvatar = new Transform[11];
		tempSkeletonAvatar[(int)skelQA.LowerTorsoRoot] = curTransform;
		curTransform = curTransform.GetChild(0); // this should select the core
		tempSkeletonAvatar[(int)skelQA.LowerTorsoCore] = curTransform;
		for (int i = 0; i < 3; i++)
		{
			curTransform = tempSkeletonAvatar[(int)skelQA.LowerTorsoCore].GetChild(i); // this should select one of the Upper Legs, but we check just to make sure
			string trName = curTransform.gameObject.name;
			char lastChar = trName[trName.Length -1];
			if (lastChar == 'L')
			{
				//do the left leg
				tempSkeletonAvatar[(int)skelQA.UpperLegL] = curTransform;
				curTransform = curTransform.GetChild(0); // select knee L
				tempSkeletonAvatar[(int)skelQA.KneeL] = curTransform;
				curTransform = curTransform.GetChild(0); // select lower leg L
				tempSkeletonAvatar[(int)skelQA.LowerLegL] = curTransform;
				curTransform = curTransform.GetChild(0); // select foor L
				tempSkeletonAvatar[(int)skelQA.FootL] = curTransform;
			}
			else if (lastChar == 'R')
			{
				// do the right leg
				tempSkeletonAvatar[(int)skelQA.UpperLegR] = curTransform;
				curTransform = curTransform.GetChild(0); // select knee R
				tempSkeletonAvatar[(int)skelQA.KneeR] = curTransform;
				curTransform = curTransform.GetChild(0); // select lower leg R
				tempSkeletonAvatar[(int)skelQA.LowerLegR] = curTransform;
				curTransform = curTransform.GetChild(0); // select foor R
				tempSkeletonAvatar[(int)skelQA.FootR] = curTransform;
			}
			else
			{
				// do the upper torso parent
				tempSkeletonAvatar[(int)skelQA.UpperTorsoParent] = curTransform;
			}
		}

//		for (int i = 0; i < tempSkeletonAvatar.Length; i++)
//		{
//			Debug.Log((skelQA)i + " " + tempSkeletonAvatar[i]);
//		}

	}

    void GenerateMeshArray(GameObject lowerTorsoPrefab, GameObject legPrefab, LTRigAssembler.LegType legType)
	{
		Transform curTransform = lowerTorsoPrefab.transform;
		tempMeshAvatar = new Transform[12];
		tempMeshAvatar[(int)meshQA.LowerTorsoCore] = curTransform;

		//grabing connectors
		for (int i = 0; i < 3; i++)
		{
			curTransform = tempMeshAvatar[(int)meshQA.LowerTorsoCore].GetChild(i);
			string trName = curTransform.gameObject.name;
			char lastChar = trName[trName.Length - 1];
			if (lastChar == 'L')
			{
				// grab the left connector
				tempMeshAvatar[(int)meshQA.ConnectorL] = curTransform;
			}
			else if (lastChar == 'R')
			{
				// grab the right connector
				tempMeshAvatar[(int)meshQA.ConnectorR] = curTransform;
			}
			else
			{
				// grab the upper torso connector
				tempMeshAvatar[(int)meshQA.ConnectorUT] = curTransform;
			}
		}

		// grabing legs
		for (int i = 0; i < 2; i++)
		{
			curTransform = legPrefab.transform.GetChild(i);
			string trName = curTransform.gameObject.name;
			char lastChar = trName[trName.Length - 1];
			if (lastChar == 'L')
			{
				// grab the left upper leg
				tempMeshAvatar[(int)meshQA.UpperLegL] = curTransform;
				if (legType == LTRigAssembler.LegType.FourJoint)
				{	
					curTransform = curTransform.GetChild(0);// grab the knee
					tempMeshAvatar[(int)meshQA.KneeL] = curTransform;
				}
				curTransform = curTransform.GetChild(0); // grab the lower leg
				tempMeshAvatar[(int)meshQA.LowerLegL] = curTransform;
				curTransform = curTransform.GetChild(0); // grab the foot
				tempMeshAvatar[(int)meshQA.FootL] = curTransform;
			}
			else if (lastChar == 'R')
			{
				// grab the left upper leg
				tempMeshAvatar[(int)meshQA.UpperLegR] = curTransform;
				if (legType == LTRigAssembler.LegType.FourJoint)
				{	
					curTransform = curTransform.GetChild(0);// grab the knee
					tempMeshAvatar[(int)meshQA.KneeR] = curTransform;
				}
				curTransform = curTransform.GetChild(0); // grab the lower leg
				tempMeshAvatar[(int)meshQA.LowerLegR] = curTransform;
				curTransform = curTransform.GetChild(0); // grab the foot
				tempMeshAvatar[(int)meshQA.FootR] = curTransform;
			}
		}

//		for (int i = 0; i < tempMeshAvatar.Length; i++)
//		{
//			Debug.Log((meshQA)i + "  " + tempMeshAvatar[i]);
//		}

	}

    AnimationClip GenerateClip(GameObject lowerTorsoPrefab, GameObject legPrefab, LTRigAssembler.LegType legType)
	{
		//instantiate the lower torso base skeleton
		GameObject ltBaseSkeletonInstance = GameObject.Instantiate(myData.LowerTorsoBaseSkeleton, Vector3.zero, Quaternion.identity) as GameObject;
		//instantiate both game objects and set them 0 postion & rotation
		GameObject lowerTorsoInstance = GameObject.Instantiate(lowerTorsoPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		GameObject legsInstance = GameObject.Instantiate(legPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		//identify the entire hierarchy of bones (should probably be done in a different function)
		GenerateBoneHierarchy(ltBaseSkeletonInstance);
		//generate array for quick mesh access
		GenerateMeshArray(lowerTorsoInstance, legsInstance, legType);
		//translate the LT on Y axis by calculating the difference between leg connector and upper leg
		//translate the upper leg in X and Z to match the each connector
		ConnectTorsoToUpperLegs();
		//match the bone positions & rotation to the mesh's origin/pivot points
		MatchBonesToMesh(legType);
		//create the animation clip & set the curves correctly for each bone & each channel of that bone ( FUKIN KILL ME, should probly make a function for that, shits gon be long )

		AnimationClip returnClip = new AnimationClip ();
		KeyframePose(ref returnClip);

		//delete the instantiated go's, dont need those anymore
		GameObject.DestroyImmediate(ltBaseSkeletonInstance);
		GameObject.DestroyImmediate(lowerTorsoInstance);
		GameObject.DestroyImmediate(legsInstance);
		//return the animation clipa

		return returnClip;
	}

	string[] channelNames = new string[6] {"localPosition.x", "localPosition.y", "localPosition.z", "localRotation.x", "localRotation.y", "localRotation.z"};

	void KeyframePose(ref AnimationClip ac)
	{
		for(int i = 0; i < tempSkeletonAvatar.Length; i++)
		{
			string relativePath = GenerateRelativePath(tempSkeletonAvatar[i]);
			for (int c = 0; c < 6; c++) 
			{
				AnimationCurve curve;
				float value;
				if (c < 3) 
				{
					value = tempSkeletonAvatar[i].localPosition[c];
					curve = AnimationCurve.Linear(0f, value, 1f, value);
				} 
				else 
				{
					value = tempSkeletonAvatar[i].localRotation.eulerAngles[c - 3];
					curve = AnimationCurve.Linear(0f, value, 1f, value);
				}
				ac.SetCurve(relativePath, typeof(Transform), channelNames[c], curve);
			}
		}
	}

	string GenerateRelativePath(Transform pathTo)
	{
		List<string> path = new List<string>();

		Transform curTransform = pathTo;
		//Debug.Log(curTransform);

		while (!ReferenceEquals(curTransform.parent, null)) 
		{
			path.Add(curTransform.name);
			curTransform = curTransform.parent;
		}

		string combinedPath = "";

		for (int i = path.Count -1; i > 0; i--)
		{
			combinedPath += path[i] + "/";
		}
		combinedPath += pathTo.name;

		return combinedPath;

	}

	void ConnectTorsoToUpperLegs()
	{
		// getting the Y difference between connector and torso core
        float connectorToCoreDis = -tempMeshAvatar[(int)meshQA.ConnectorL].localPosition.y;
		// getting the Y difference between connector and upper leg
		float connectorToUpLegDis =  tempMeshAvatar[(int)meshQA.UpperLegL].position.y - tempMeshAvatar[(int)meshQA.ConnectorL].position.y;

        tempMeshAvatar[(int)meshQA.LowerTorsoCore].transform.position = new Vector3(0f, connectorToUpLegDis, 0f);

		Vector3 tmpv3 = tempMeshAvatar[(int)meshQA.UpperLegL].transform.position;
		tempMeshAvatar[(int)meshQA.UpperLegL].transform.position = new Vector3(tempMeshAvatar[(int)meshQA.ConnectorL].transform.position.x, tmpv3.y, tmpv3.z);

		tmpv3 = tempMeshAvatar[(int)meshQA.UpperLegL].transform.position;
		tempMeshAvatar[(int)meshQA.UpperLegR].transform.position = new Vector3(tempMeshAvatar[(int)meshQA.ConnectorR].transform.position.x, tmpv3.y, tmpv3.z);
		//Debug.Log(tempMeshAvatar[(int)meshQA.LowerTorsoCore].transform.position + "      " + connectorToCoreDis + "   " + connectorToUpLegDis);
	}

	void MatchBonesToMesh(LTRigAssembler.LegType legType)
	{
		for (int i = 0; i < 10; i++)
		{
			if (legType == LTRigAssembler.LegType.ThreeJoint)
			{
				if (i != (int)skelQA.KneeL && i != (int)skelQA.KneeR)
				{
					tempSkeletonAvatar[i].transform.position = tempMeshAvatar[i].transform.position;
				}
				else if (i == (int)skelQA.KneeL)
				{
					tempSkeletonAvatar[i].transform.position = tempMeshAvatar[i + 1].transform.position;
				}
				else if (i == (int)skelQA.KneeR)
				{
					tempSkeletonAvatar[i].transform.position = tempMeshAvatar[i + 1].transform.position;
				}
			}
			else
			{
				tempSkeletonAvatar[i].transform.position = tempMeshAvatar[i].transform.position;
			}
		}
	}

	LTRigAssembler LoadData()
	{
		LTRigAssembler loadData = AssetDatabase.LoadAssetAtPath<LTRigAssembler>(dataPath);
		if (loadData != null)
		{
			assetAlreadyExists = true;
			return loadData;
		}
		else
		{
			assetAlreadyExists = false;
			return ScriptableObject.CreateInstance<LTRigAssembler>();
		}
	}

	void SaveData(LTRigAssembler toSave)
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
