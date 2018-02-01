using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LTRigAssembler : ScriptableObject 
{
	public static LTRigAssembler Instance;

	[Serializable]
	public enum LegType {ThreeJoint, FourJoint};

	[SerializeField]
	public GameObject LowerTorsoBaseSkeleton;
    [SerializeField]
    public RuntimeAnimatorController LTAnimationController;

    [SerializeField]
    public AnimationClip ThreeJointLeg_IDLE;
    [SerializeField]
    public AnimationClip ThreeJointLeg_walk_light;
    [SerializeField]
    public AnimationClip ThreeJointLeg_run_light;
    [SerializeField]
    public AnimationClip ThreeJointLeg_walk_heavy;
    [SerializeField]
    public AnimationClip ThreeJointLeg_run_heavy;
    [SerializeField]
    public AnimationClip ThreeJointLeg_turn5L;
    [SerializeField]
    public AnimationClip ThreeJointLeg_turn90L;
    [SerializeField]
    public AnimationClip ThreeJointLeg_turn5R;
    [SerializeField]
    public AnimationClip ThreeJointLeg_turn90R;

    [SerializeField]
    public AnimationClip FourJointLeg_IDLE;
    [SerializeField]
    public AnimationClip FourJointLeg_walk_light;
    [SerializeField]
    public AnimationClip FourJointLeg_run_light;
    [SerializeField]
    public AnimationClip FourJointLeg_walk_heavy;
    [SerializeField]
    public AnimationClip FourJointLeg_run_heavy;
    [SerializeField]
    public AnimationClip FourJointLeg_turn5L;
    [SerializeField]
    public AnimationClip FourJointLeg_turn90L;
    [SerializeField]
    public AnimationClip FourJointLeg_turn5R;
    [SerializeField]
    public AnimationClip FourJointLeg_turn90R;

	[Serializable]
	public struct LTinfo
	{
		[SerializeField]
		public GameObject asset;
	}
	[Serializable]
	public struct LGinfo
	{
		[SerializeField]
		public GameObject asset;
		[SerializeField]
		public LegType legType;
	}

	[SerializeField]
	public LTinfo[] LTData;
	[SerializeField]
	public LGinfo[] LGData;

	Transform[] tempRigAvatar;
	Transform[] tempMeshAvatar;
	// for easier debugging you can use these to get specific transforms easier
	enum LTRigID : int { LowerTorsoCore, UpperLegL, KneeL, LowerLegL, FootL, UpperLegR, KneeR, LowerLegR, FootR, UpperTorsoParent};
	enum LTMeshID : int { LowerTorsoCore, UpperLegL, KneeL, LowerLegL, FootL, UpperLegR, KneeR, LowerLegR, FootR};

    Animator tempAnim;

	/// <summary>
	/// Instantiates and assembles the lower torso based on values given and gives it back as gameobject.
	/// </summary>
	/// <param name="torsoAssetName">Name of the asset.</param>
	/// <param name="legAssetName">Name of the asset.</param>
	/// <param name="atPosition">Position of the lower torso</param>
	/// <param name="atRotation">Rotation of the lower torso</param>
    /// <param name="upperTorsoParent">Output reference to the upper torso parent. This is the transform which should be used as a parent for upper torso</param>
    /// <param name="animator">Output reference to the Animator component</param>
    public GameObject AssembleLowerTorso(string torsoAssetName, string legAssetName, Vector3 atPosition, Quaternion atRotation, out Transform upperTorsoParent, out Animator animator)
	{
        GameObject go = TorsoAssembler(AssetNameToIDLowerTorso(torsoAssetName), AssetNameToIDLegs(legAssetName), atPosition, atRotation);
        upperTorsoParent = tempRigAvatar[(int)LTRigID.UpperTorsoParent];
        animator = tempAnim;
        return go;
	}
	/// <summary>
	/// Instantiates and assembles the lower torso based on values given and gives it back as gameobject.
	/// </summary>
	/// <returns>The lower torso.</returns>
	/// <param name="torsoID">ID of the asset</param>
	/// <param name="legID">ID of the asset</param>
	/// <param name="atPosition">Position of the lower torso</param>
	/// <param name="atRotation">Rotation of the lower torso</param>
    /// <param name="upperTorsoParent">Output reference to the upper torso parent. This is the transform which should be used as a parent for upper torso</param>
    /// <param name="animator">Output reference to the Animator component</param>
    public GameObject AssembleLowerTorso(int torsoID, int legID, Vector3 atPosition, Quaternion atRotation, out Transform upperTorsoParent, out Animator animator)
	{
        
        GameObject go = TorsoAssembler(torsoID, legID, atPosition, atRotation);
        upperTorsoParent = tempRigAvatar[(int)LTRigID.UpperTorsoParent];
        animator = tempAnim;
        return go;
	}
    public static string AssetIDToNameLegs(int ID)
    {
        return Instance.LGData[ID].asset.name;
    }

    public static string AssetIDToNameLowerTorso(int ID)
    {
        return Instance.LTData[ID].asset.name;
    }

    public static int AssetNameToIDLowerTorso(string name)
	{
		for (int i = 0; i < Instance.LTData.Length; i++)
		{
			if (name == Instance.LTData[i].asset.name)
			{
				return i;
			}
		}
        Debug.LogError("Asset named: " + name + " | Has not been found (Lower Torso)");
		return 0;
	}

	public static int AssetNameToIDLegs(string name)
	{
		for (int i = 0; i < Instance.LGData.Length; i++)
		{
			if (name == Instance.LGData[i].asset.name)
			{
				return i;
			}
		}
		Debug.LogError("Asset named: " + name + " | Has not been found (Legs");
		return 0;
	}

    GameObject TorsoAssembler(int torsoID, int legID, Vector3 atPosition, Quaternion atRotation)
	{
		GameObject torsoInstance = GameObject.Instantiate(LTData[torsoID].asset, atPosition, atRotation);
        GameObject legInstance = GameObject.Instantiate(LGData[legID].asset, atPosition, atRotation);
		GameObject rig = GameObject.Instantiate(LowerTorsoBaseSkeleton, atPosition, atRotation);

        tempAnim = rig.GetComponent<Animator>();

        GenerateRigArray(rig);
        GenerateMeshArray(torsoInstance, legInstance, LGData[legID].legType);
        CombineObjectsFromArrays(LGData[legID].legType);
        OverrideAnimationClips(torsoID, legID, LGData[legID].legType);
        GameObject.Destroy(legInstance);
		return rig;
	}

    void GenerateMeshArray(GameObject torso, GameObject legs, LegType legType)
	{
		Transform curTransform = torso.transform;
		tempMeshAvatar = new Transform[9];
		tempMeshAvatar[(int)LTMeshID.LowerTorsoCore] = curTransform;

		// grabing legs
		for (int i = 0; i < 2; i++)
		{
			curTransform = legs.transform.GetChild(i);
			string trName = curTransform.gameObject.name;
			char lastChar = trName[trName.Length - 1];
            if (lastChar == 'L')
            {
                // grab the left upper leg
                tempMeshAvatar[(int)LTMeshID.UpperLegL] = curTransform;
                if (legType == LTRigAssembler.LegType.FourJoint)
                {	
                    curTransform = curTransform.GetChild(0);// grab the knee
                    tempMeshAvatar[(int)LTMeshID.KneeL] = curTransform;
                }
                curTransform = curTransform.GetChild(0); // grab the lower leg
                tempMeshAvatar[(int)LTMeshID.LowerLegL] = curTransform;
                curTransform = curTransform.GetChild(0); // grab the foot
                tempMeshAvatar[(int)LTMeshID.FootL] = curTransform;
            }
            else if (lastChar == 'R')
            {
                // grab the left upper leg
                tempMeshAvatar[(int)LTMeshID.UpperLegR] = curTransform;
                if (legType == LTRigAssembler.LegType.FourJoint)
                {	
                    curTransform = curTransform.GetChild(0);// grab the knee
                    tempMeshAvatar[(int)LTMeshID.KneeR] = curTransform;
                }
                curTransform = curTransform.GetChild(0); // grab the lower leg
                tempMeshAvatar[(int)LTMeshID.LowerLegR] = curTransform;
                curTransform = curTransform.GetChild(0); // grab the foot
                tempMeshAvatar[(int)LTMeshID.FootR] = curTransform;
            }
		}
	}

    void GenerateRigArray(GameObject rig)
	{
		Transform curTransform = rig.transform;
		curTransform = curTransform.GetChild(0); // this should select the root
		tempRigAvatar = new Transform[10];
		curTransform = curTransform.GetChild(0); // this should select the core
		tempRigAvatar[(int)LTRigID.LowerTorsoCore] = curTransform;
		for (int i = 0; i < 3; i++)
		{
			curTransform = tempRigAvatar[(int)LTRigID.LowerTorsoCore].GetChild(i); // this should select one of the Upper Legs, but we check just to make sure
			string trName = curTransform.gameObject.name;
			char lastChar = trName[trName.Length - 1];
			if (lastChar == 'L')
			{
				//do the left leg
				tempRigAvatar[(int)LTRigID.UpperLegL] = curTransform;
				curTransform = curTransform.GetChild(0); // select knee L
				tempRigAvatar[(int)LTRigID.KneeL] = curTransform;
				curTransform = curTransform.GetChild(0); // select lower leg L
				tempRigAvatar[(int)LTRigID.LowerLegL] = curTransform;
				curTransform = curTransform.GetChild(0); // select foor L
				tempRigAvatar[(int)LTRigID.FootL] = curTransform;
			}
			else if (lastChar == 'R')
			{
				// do the right leg
				tempRigAvatar[(int)LTRigID.UpperLegR] = curTransform;
				curTransform = curTransform.GetChild(0); // select knee R
				tempRigAvatar[(int)LTRigID.KneeR] = curTransform;
				curTransform = curTransform.GetChild(0); // select lower leg R
				tempRigAvatar[(int)LTRigID.LowerLegR] = curTransform;
				curTransform = curTransform.GetChild(0); // select foor R
				tempRigAvatar[(int)LTRigID.FootR] = curTransform;
			}
			else
			{
				// do the upper torso parent
				tempRigAvatar[(int)LTRigID.UpperTorsoParent] = curTransform;
			}
		}

	}

	void CombineObjectsFromArrays(LegType legType)
	{
		for (int i = 0; i < tempMeshAvatar.Length; i++)
		{
			if (legType == LegType.ThreeJoint)
			{
				if (i != (int)LTMeshID.KneeL && i != (int)LTMeshID.KneeR)
				{
					tempMeshAvatar[i].SetParent(tempRigAvatar[i]);
                    tempMeshAvatar[i].localPosition = Vector3.zero;
                    tempMeshAvatar[i].localRotation = Quaternion.Euler(Vector3.zero);
				}
			}
			else
			{
                tempMeshAvatar[i].SetParent(tempRigAvatar[i]);
                tempMeshAvatar[i].localPosition = Vector3.zero;
                tempMeshAvatar[i].localRotation = Quaternion.Euler(Vector3.zero);
			}
		}
	}

    void OverrideAnimationClips(int torsoID, int legID, LegType legType)
    {
        AnimatorOverrideController animOverride = new AnimatorOverrideController(LTAnimationController);
        tempAnim.runtimeAnimatorController = LTAnimationController;
        tempAnim.applyRootMotion = true;
        //tempAnim.updateMode = AnimatorUpdateMode.AnimatePhysics;

        AnimationClip[] oc = tempAnim.runtimeAnimatorController.animationClips;

        Debug.LogWarning("Animation override for legs is still missing a lot of animations, currently working in its bare bones version");

//        for (int i = 0; i < oc.Length; i++) // leave this here, its useful for debugging
//        {
//            Debug.Log(i + " - " + oc[i].name);
//        }

        animOverride[oc[0].name] = LoadAdditiveFixClip(torsoID, legID);
        if (legType == LegType.ThreeJoint)
        {
            animOverride[oc[1].name] = ThreeJointLeg_IDLE;
            animOverride[oc[2].name] = ThreeJointLeg_walk_light;
            animOverride[oc[3].name] = ThreeJointLeg_run_light;
            animOverride[oc[4].name] = ThreeJointLeg_walk_heavy;
            animOverride[oc[5].name] = ThreeJointLeg_run_heavy;
            animOverride[oc[6].name] = ThreeJointLeg_turn90L;
            animOverride[oc[7].name] = ThreeJointLeg_turn5L;
            animOverride[oc[8].name] = ThreeJointLeg_turn5R;
            animOverride[oc[9].name] = ThreeJointLeg_turn90R;
        }
        else if (legType == LegType.FourJoint)
        {
            Debug.LogWarning("Animation override is missing its animations for 4 joint legs, currently using 3 joint animations as replacement");
            animOverride[oc[1].name] = ThreeJointLeg_IDLE;
            animOverride[oc[2].name] = ThreeJointLeg_walk_light;
            animOverride[oc[3].name] = ThreeJointLeg_run_light;
            animOverride[oc[4].name] = ThreeJointLeg_walk_heavy;
            animOverride[oc[5].name] = ThreeJointLeg_run_heavy;
            animOverride[oc[6].name] = ThreeJointLeg_turn90L;
            animOverride[oc[7].name] = ThreeJointLeg_turn5L;
            animOverride[oc[8].name] = ThreeJointLeg_turn5R;
            animOverride[oc[9].name] = ThreeJointLeg_turn90R;
        }

        tempAnim.runtimeAnimatorController = animOverride;

    }


    string LTAnimClipsDataPath = "Data/ProceduralAnimPoses/LT/";
    AnimationClip LoadAdditiveFixClip(int torsoID, int legID)
    {
        string pathToClip = LTAnimClipsDataPath + LTData[torsoID].asset.name + "-" + LGData[legID].asset.name;
        return Resources.Load(pathToClip, typeof(AnimationClip)) as AnimationClip;
    }
}
