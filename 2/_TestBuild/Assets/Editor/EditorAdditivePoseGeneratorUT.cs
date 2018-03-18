using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class EditorAdditivePoseGeneratorUT : EditorWindow
{
    static EditorAdditivePoseGeneratorUT Instance;

    const string dataPath = "Assets/Resources/Data/UTRigAssembler.asset";
    const string animClipDataPath = "Assets/Resources/Data/ProceduralAnimPoses/UT/";
    const string assetPathUT = "UT";
    const string assetPathAR = "AR";
	const string assetPathHD = "HD";

	SerializedObject so;
    UTRigAssembler myData;

    bool assetAlreadyExists;

    bool foldOut;
    int animSetTab = 0;
    string[] animTabNames = new string[3] {"1 Joint Arm", "2 Joint Arm", "3 Joint Arm"};
	int tab = 0;
	string[] tabNames = new string[3] {"Upper Torso", "Arms", "Heads"};
	Vector2 scrollPos;
	SerializedProperty UTData;
	SerializedProperty ARData;
	SerializedProperty HDData;
	SerializedProperty upperTorsoBaseSkeletonPrefab;
    SerializedProperty UTAnimControler;

	[MenuItem ("Utilities/Additive Pose Generator - UT")]
    public static void ShowWindow()
    {
        Instance = EditorWindow.GetWindow<EditorAdditivePoseGeneratorUT>(false,"UT Pose Generator", true);
		Instance.myData = Instance.LoadData();
		Instance.so = new SerializedObject(Instance.myData);
		Instance.UTData = Instance.so.FindProperty("UTData");
		Instance.ARData = Instance.so.FindProperty("ARData");
		Instance.HDData = Instance.so.FindProperty("HDData");
		Instance.upperTorsoBaseSkeletonPrefab = Instance.so.FindProperty("UpperTorsoBaseSkeleton");
        Instance.UTAnimControler = Instance.so.FindProperty("UTAnimationController");
    }

	void OnGUI()
	{
        EditorGUILayout.PropertyField(upperTorsoBaseSkeletonPrefab, new GUIContent("Base Rig"));
        EditorGUILayout.PropertyField(UTAnimControler, new GUIContent("Base Animation Controller"));
        foldOut = EditorGUILayout.Foldout(foldOut, "Base Animation Sets");
        if (foldOut)
        {
            animSetTab = GUILayout.Toolbar(animSetTab, animTabNames);
            if (animSetTab == 0)
            {
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_Idle"), new GUIContent("Idle"));
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_takeAimL"), new GUIContent("Take Aim - Left"));
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_takeAimR"), new GUIContent("Take Aim - Right"));
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_takeAimShoulders"), new GUIContent("Take Aim - Shoulders"));
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_recoilHeavyR"), new GUIContent("Recoil - Heavy Right"));
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_recoilLightR"), new GUIContent("Recoil - Light Right"));
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_recoilHeavyL"), new GUIContent("Recoil - Heavy Left"));
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_recoilLightL"), new GUIContent("Recoil - Light Left"));
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_recoilHeavyShoulders"), new GUIContent("Recoil - Heavy Shoulder"));
                EditorGUILayout.PropertyField(so.FindProperty("oneJointArm_recoilLightShoulders"), new GUIContent("Recoil - Light Shoulder"));

            }
            else if (animSetTab == 1)
            {
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_Idle"), new GUIContent("Idle"));
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_takeAimL"), new GUIContent("Take Aim - Left"));
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_takeAimR"), new GUIContent("Take Aim - Right"));
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_takeAimShoulders"), new GUIContent("Take Aim - Shoulders"));
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_recoilHeavyR"), new GUIContent("Recoil - Heavy Right"));
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_recoilLightR"), new GUIContent("Recoil - Light Right"));
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_recoilHeavyL"), new GUIContent("Recoil - Heavy Left"));
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_recoilLightL"), new GUIContent("Recoil - Light Left"));
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_recoilHeavyShoulders"), new GUIContent("Recoil - Heavy Shoulder"));
                EditorGUILayout.PropertyField(so.FindProperty("twoJointArm_recoilLightShoulders"), new GUIContent("Recoil - Light Shoulder"));
            }
            else if (animSetTab == 2)
            {
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_Idle"), new GUIContent("Idle"));
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_takeAimL"), new GUIContent("Take Aim - Left"));
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_takeAimR"), new GUIContent("Take Aim - Right"));
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_takeAimShoulders"), new GUIContent("Take Aim - Shoulders"));
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_recoilHeavyR"), new GUIContent("Recoil - Heavy Right"));
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_recoilLightR"), new GUIContent("Recoil - Light Right"));
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_recoilHeavyL"), new GUIContent("Recoil - Heavy Left"));
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_recoilLightL"), new GUIContent("Recoil - Light Left"));
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_recoilHeavyShoulders"), new GUIContent("Recoil - Heavy Shoulder"));
                EditorGUILayout.PropertyField(so.FindProperty("threeJointArm_recoilLightShoulders"), new GUIContent("Recoil - Light Shoulder"));
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
			if (GUILayout.Button("Auto fill Upper Torso"))
			{
				AutoFillUT();
			}
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400));
			EditorGUILayout.PropertyField(UTData, new GUIContent("Upper Torso Data"), true); 
			EditorGUILayout.EndScrollView();
		}
		else if (tab == 1)
		{
			if (GUILayout.Button("Auto fill Arms"))
			{
				AutoFillAR();
			}
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400));
			EditorGUILayout.PropertyField(ARData, new GUIContent("Arms Data"), true); 
			EditorGUILayout.EndScrollView();
		}
		else if (tab == 2)
		{
			if (GUILayout.Button("Auto fill Heads"))
			{
				AutoFillHD();
			}
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400));
			EditorGUILayout.PropertyField(HDData, new GUIContent("Heads Data"), true); 
			EditorGUILayout.EndScrollView();
		}

		so.ApplyModifiedProperties ();
		so.UpdateIfRequiredOrScript();
	}

	void AutoFillUT()
	{
		GameObject[] loadedAssets = Resources.LoadAll(assetPathUT, typeof(GameObject)).Cast<GameObject>().ToArray();
		myData.UTData = new UTRigAssembler.UTinfo[loadedAssets.Length];
		
		for (int i = 0; i < loadedAssets.Length; i++)
		{
			
			myData.UTData[i].asset = loadedAssets[i];
			myData.UTData[i].hasHead = CheckForHeadConnector(myData.UTData[i].asset);
		}
	}
	
	void AutoFillAR()
	{
		GameObject[] loadedAssets = Resources.LoadAll(assetPathAR, typeof(GameObject)).Cast<GameObject>().ToArray();
		myData.ARData = new UTRigAssembler.ARinfo[loadedAssets.Length];
		
		for (int i = 0; i < loadedAssets.Length; i++)
		{
			myData.ARData[i].asset = loadedAssets[i];
			myData.ARData[i].armType = CheckArmType(myData.ARData[i].asset);
		}
	}
	
	void AutoFillHD()
	{
		GameObject[] loadedAssets = Resources.LoadAll(assetPathHD, typeof(GameObject)).Cast<GameObject>().ToArray();
		myData.HDData = new UTRigAssembler.HDinfo[loadedAssets.Length];
		
		for (int i = 0; i < loadedAssets.Length; i++)
		{
			myData.HDData[i].asset = loadedAssets[i];
		}
	}

	bool CheckForHeadConnector(GameObject ut)
	{
		Transform curTransform;
		for (int i = 0; i < ut.transform.childCount; i++)
		{
			curTransform = ut.transform.GetChild(i);
			string curName = curTransform.name;

			if (curName[0] == 'C' && curName[curName.Length - 1] == 'D')
				return true;
		}
		return false;
	}

	void GeneratePoses()
	{
		object[] existingClips = Resources.LoadAll("Data/ProceduralAnimPoses/UT");
		for (int i = 0; i < existingClips.Length; i++)
		{
			AssetDatabase.DeleteAsset(animClipDataPath + existingClips[i].ToString() + ".anim");
		}

		GameObject utGO;
		GameObject arGO;
		for (int utID = 0; utID < myData.UTData.Length; utID++)
		{
			utGO = myData.UTData[utID].asset;
			for (int arID = 0; arID < myData.ARData.Length; arID++)
			{
				arGO = myData.ARData[arID].asset;
				AssetDatabase.CreateAsset(GenerateClip(utGO, arGO, myData.ARData[arID].armType, myData.UTData[utID].hasHead), animClipDataPath + utGO.name + "-" + arGO.name + ".anim");
			}
		}
	}

	AnimationClip GenerateClip(GameObject upperTorsoPrefab, GameObject armPrefab, UTRigAssembler.ArmType armType, bool hasHead)
	{
		//instantiate the upper torso base skeleton
		GameObject utBaseSkeletonInstance = GameObject.Instantiate(myData.UpperTorsoBaseSkeleton, Vector3.zero, Quaternion.identity) as GameObject;
		//instantiate both gameobject and set the at 0 position and rotation
		GameObject upperTorsoInstance = GameObject.Instantiate(upperTorsoPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		GameObject armInstance = GameObject.Instantiate(armPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		//genetate the bone hierarchy
		GenerateBoneHierarchy(utBaseSkeletonInstance);
		//generate the mesh hierarchy
		GenerateMeshArray(upperTorsoInstance, armInstance, armType, hasHead);
		//move the upper arms to match their connector
		ConnectArmsToTorso();
		MatchBonesToMesh(armType, hasHead);
		//create the animation clip and set the curves for each bone and channel
		AnimationClip returnClip = new AnimationClip();
		KeyframePose(ref returnClip);

        GameObject.DestroyImmediate(utBaseSkeletonInstance);
        GameObject.DestroyImmediate(upperTorsoInstance);
        GameObject.DestroyImmediate(armInstance);

		return returnClip;
	}

	Transform[] tempRigAvatar;
	Transform[] tempMeshAvatar;

	enum skelQA : int {UpperTorsoCore, UpperArmL, LowerArmL, HandL, UpperArmR, LowerArmR, HandR, Head, Root, WeaponMaster, WeaponL, WeaponR};
	enum meshQA : int {UpperTorsoCore, UpperArmL, LowerArmL, HandL, UpperArmR, LowerArmR, HandR, HeadConnector, ConnectorL, ConnectorR};

	void GenerateBoneHierarchy(GameObject skeletonInstance)
	{
		Transform curTransform = skeletonInstance.transform;
		tempRigAvatar = new Transform[12];
		tempRigAvatar[(int)skelQA.Root] = curTransform;
		curTransform = curTransform.GetChild(0);
		tempRigAvatar[(int)skelQA.UpperTorsoCore] = curTransform;

		for (int i = 0; i < 4; i++)
		{
			curTransform = tempRigAvatar[(int)skelQA.UpperTorsoCore].GetChild(i);
			string curName = curTransform.name;
			if (curName[0] == 'H')
			{
				tempRigAvatar[(int)skelQA.Head] = curTransform;
			}
			else if (curName[curName.Length - 1] == 'L')
			{
				tempRigAvatar[(int)skelQA.UpperArmL] = curTransform;
				curTransform = curTransform.GetChild(0);
				tempRigAvatar[(int)skelQA.LowerArmL] = curTransform;
				curTransform = curTransform.GetChild(0);
				tempRigAvatar[(int)skelQA.HandL] = curTransform;
			}
			else if (curName[curName.Length - 1] == 'R')
			{
				tempRigAvatar[(int)skelQA.UpperArmR] = curTransform;
				curTransform = curTransform.GetChild(0);
				tempRigAvatar[(int)skelQA.LowerArmR] = curTransform;
				curTransform = curTransform.GetChild(0);
				tempRigAvatar[(int)skelQA.HandR] = curTransform;
			}
			else
			{
				tempRigAvatar[(int)skelQA.WeaponMaster] = curTransform;
				tempRigAvatar[(int)skelQA.WeaponL] = curTransform.GetChild(0);
				tempRigAvatar[(int)skelQA.WeaponR] = curTransform.GetChild(1);
			}
		}
	}

	void GenerateMeshArray(GameObject utInstance, GameObject armInstance, UTRigAssembler.ArmType armType, bool hasHead)
	{
		Transform curTransform = utInstance.transform;
		tempMeshAvatar = new Transform[10];
		tempMeshAvatar[(int)meshQA.UpperTorsoCore] = curTransform;
		int childCount = curTransform.childCount;

		for (int i = 0; i < childCount; i++)
		{
			curTransform = tempMeshAvatar[(int)meshQA.UpperTorsoCore].GetChild(i);
			string curName = curTransform.name;
			if (curName[0] == 'C' && curName[curName.Length - 1] == 'L')
			{
				tempMeshAvatar[(int)meshQA.ConnectorL] = curTransform;
			}
			else if (curName[0] == 'C' && curName[curName.Length - 1] == 'R')
			{
				tempMeshAvatar[(int)meshQA.ConnectorR] = curTransform;

			}
			else if (hasHead && curName[0] == 'C' && curName[curName.Length - 1] == 'D')
			{
				tempMeshAvatar[(int)meshQA.HeadConnector] = curTransform;
			}
		}

		for (int i = 0; i < 2; i++)
		{
			curTransform = armInstance.transform.GetChild(i);
			string curName = curTransform.name;
			if (curName[curName.Length - 1] == 'L')
			{
				tempMeshAvatar[(int)meshQA.UpperArmL] = curTransform;
				if (armType != UTRigAssembler.ArmType.SingleJoint)
				{
					curTransform = curTransform.GetChild(0);
					tempMeshAvatar[(int)meshQA.LowerArmL] = curTransform;
					if (armType != UTRigAssembler.ArmType.TwoJoint)
					{
						curTransform = curTransform.GetChild(0);
						tempMeshAvatar[(int)meshQA.HandL] = curTransform;
					}
				}
			}
			else if (curName[curName.Length - 1] == 'R')
			{
				tempMeshAvatar[(int)meshQA.UpperArmR] = curTransform;
				if (armType != UTRigAssembler.ArmType.SingleJoint)
				{
					curTransform = curTransform.GetChild(0);
					tempMeshAvatar[(int)meshQA.LowerArmR] = curTransform;
					if (armType != UTRigAssembler.ArmType.TwoJoint)
					{
						curTransform = curTransform.GetChild(0);
						tempMeshAvatar[(int)meshQA.HandR] = curTransform;
					}
				}
			}
		}

	}

	void MatchBonesToMesh(UTRigAssembler.ArmType armType, bool hasHead)
	{
		for (int i = 0; i < 8; i++)
		{
			if (armType == UTRigAssembler.ArmType.ThreeJoint)
			{
				if (!hasHead && i == (int)skelQA.Head)
				{

				}
				else
				{
					tempRigAvatar[i].position = tempMeshAvatar[i].position;
				}
			}
			else if (armType == UTRigAssembler.ArmType.TwoJoint)
			{
				if (!hasHead && i == (int)skelQA.Head)
				{

				}
				else
				{
					if (i != (int)skelQA.HandL && i != (int)skelQA.HandR)
						tempRigAvatar[i].position = tempMeshAvatar[i].position;
				}
			}
			else if (armType == UTRigAssembler.ArmType.SingleJoint)
			{
				if (!hasHead && i == (int)skelQA.Head)
				{

				}
				else
				{
					if (i != (int)skelQA.HandL && i != (int)skelQA.HandR && i != (int)skelQA.LowerArmL && i != (int)skelQA.LowerArmR)
						tempRigAvatar[i].position = tempMeshAvatar[i].position;
				}
			}
		}
	}

	string[] channelNames = new string[6] {"localPosition.x", "localPosition.y", "localPosition.z", "localRotation.x", "localRotation.y", "localRotation.z"};

	void KeyframePose(ref AnimationClip ac)
	{
		for(int i = 0; i < tempRigAvatar.Length; i++)
		{
			string relativePath = GenerateRelativePath(tempRigAvatar[i]);
			for (int c = 0; c < 6; c++) 
			{
				AnimationCurve curve;
				float value;
				if (c < 3) 
				{
					value = tempRigAvatar[i].localPosition[c];
					curve = AnimationCurve.Linear(0f, value, 1f, value);
				} 
				else 
				{
					value = tempRigAvatar[i].localRotation.eulerAngles[c - 3];
					curve = AnimationCurve.Linear(0f, value, 1f, value);
				}
				ac.SetCurve(relativePath, typeof(Transform), channelNames[c], curve);
			}
		}
	}

	void ConnectArmsToTorso()
	{
		//Debug.Log(tempMeshAvatar[(int)meshQA.ConnectorL]);
		tempMeshAvatar[(int)meshQA.UpperArmL].position = tempMeshAvatar[(int)meshQA.ConnectorL].position;
		//Debug.Log(tempMeshAvatar[(int)meshQA.ConnectorR]);
		tempMeshAvatar[(int)meshQA.UpperArmR].position = tempMeshAvatar[(int)meshQA.ConnectorR].position;
	}


	bool CheckIfMount(string name)
	{
		if (name[0] == 'W' || name[0] == 'S')
			return true;
		else
			return false;
	}

	bool CheckIfLastJoint(Transform tr)
	{
		for (int i = 0; i < tr.childCount; i++)
		{
			Transform curChild = tr.GetChild(i);
			string trName = curChild.name;
			if (!CheckIfMount(trName))
			{
				if (curChild.childCount > 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	Transform GetNextArmJoint(Transform tr)
	{
		for (int i = 0; i < tr.childCount; i++)
		{
			Transform curChild = tr.GetChild(i);
			if (!CheckIfMount(curChild.name))
			{
				return curChild;
			}
		}
		return null;
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

	UTRigAssembler.ArmType CheckArmType(GameObject armAsset)
	{
		

		if (armAsset.transform.childCount != 2)
		{
			Debug.LogError(armAsset.name + " Arm prefab contains: " + armAsset.transform.childCount + " children. Instead of 2");
		}
		else
		{
			Transform tr = armAsset.transform.GetChild(0);

			if (CheckIfLastJoint(tr))
			{
				return UTRigAssembler.ArmType.SingleJoint;
			}

			tr = GetNextArmJoint(tr);
			if (CheckIfLastJoint(tr))
			{
				return UTRigAssembler.ArmType.TwoJoint;
			}

			tr = GetNextArmJoint(tr);
			if (CheckIfLastJoint(tr))
			{
				return UTRigAssembler.ArmType.ThreeJoint;
			}

		}
		return UTRigAssembler.ArmType.SingleJoint;
	}

    UTRigAssembler LoadData()
    {
        UTRigAssembler loadData = AssetDatabase.LoadAssetAtPath<UTRigAssembler>(dataPath);
        if (loadData != null)
        {
            assetAlreadyExists = true;
            return loadData;
        }
        else
        {
            assetAlreadyExists = false;
            return ScriptableObject.CreateInstance<UTRigAssembler>();
        }
    }

    void SaveData(UTRigAssembler toSave)
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
