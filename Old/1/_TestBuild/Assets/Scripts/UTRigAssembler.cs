using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UTRigAssembler : ScriptableObject 
{
    public static UTRigAssembler Instance;

    [Serializable]
    public enum ArmType {SingleJoint, TwoJoint, ThreeJoint}

	[SerializeField]
	public GameObject UpperTorsoBaseSkeleton;
    [SerializeField]
    public RuntimeAnimatorController UTAnimationController;

    [SerializeField]
    public AnimationClip oneJointArm_Idle;
    [SerializeField]
    public AnimationClip oneJointArm_takeAimL;
    [SerializeField]
    public AnimationClip oneJointArm_takeAimR;
    [SerializeField]
    public AnimationClip oneJointArm_takeAimShoulders;
    [SerializeField]
    public AnimationClip oneJointArm_recoilHeavyR;
    [SerializeField]
    public AnimationClip oneJointArm_recoilLightR;
    [SerializeField]
    public AnimationClip oneJointArm_recoilHeavyL;
    [SerializeField]
    public AnimationClip oneJointArm_recoilLightL;
    [SerializeField]
    public AnimationClip oneJointArm_recoilHeavyShoulders;
    [SerializeField]
    public AnimationClip oneJointArm_recoilLightShoulders;


    [SerializeField]
    public AnimationClip twoJointArm_Idle;
    [SerializeField]
    public AnimationClip twoJointArm_takeAimL;
    [SerializeField]
    public AnimationClip twoJointArm_takeAimR;
    [SerializeField]
    public AnimationClip twoJointArm_takeAimShoulders;
    [SerializeField]
    public AnimationClip twoJointArm_recoilHeavyR;
    [SerializeField]
    public AnimationClip twoJointArm_recoilLightR;
    [SerializeField]
    public AnimationClip twoJointArm_recoilHeavyL;
    [SerializeField]
    public AnimationClip twoJointArm_recoilLightL;
    [SerializeField]
    public AnimationClip twoJointArm_recoilHeavyShoulders;
    [SerializeField]
    public AnimationClip twoJointArm_recoilLightShoulders;

    [SerializeField]
    public AnimationClip threeJointArm_Idle;
    [SerializeField]
    public AnimationClip threeJointArm_takeAimL;
    [SerializeField]
    public AnimationClip threeJointArm_takeAimR;
    [SerializeField]
    public AnimationClip threeJointArm_takeAimShoulders;
    [SerializeField]
    public AnimationClip threeJointArm_recoilHeavyR;
    [SerializeField]
    public AnimationClip threeJointArm_recoilLightR;
    [SerializeField]
    public AnimationClip threeJointArm_recoilHeavyL;
    [SerializeField]
    public AnimationClip threeJointArm_recoilLightL;
    [SerializeField]
    public AnimationClip threeJointArm_recoilHeavyShoulders;
    [SerializeField]
    public AnimationClip threeJointArm_recoilLightShoulders;

    [Serializable]
    public struct UTinfo
    {
		public GameObject asset;
		public bool hasHead;
    }

    [Serializable]
    public struct ARinfo
    {
		public GameObject asset;
		public ArmType armType;
    }

	[Serializable]
	public struct HDinfo
	{
		public GameObject asset;
	}

    [SerializeField]
    public UTinfo[] UTData;

    [SerializeField]
    public ARinfo[] ARData;

	[SerializeField]
	public HDinfo[] HDData;


	Transform[] tempRigAvatar;
	Transform[] tempMeshAvatar;
	// for debugging you can use these to check specific transforms easier
	enum UTRigID : int {UpperTorsoCore, UpperArmL, LowerArmL, HandL, UpperArmR, LowerArmR, HandR, Head, Root, WeaponMaster, WeaponL, WeaponR};
	enum UTMeshID : int { UpperTorsoCore, UpperArmL, LowerArmL, HandL, UpperArmR, LowerArmR, HandR, Head, WeaponMountL, WeaponMountR, ShieldMountL, ShieldMountR, GimbalMountL, GimbalMountR};

    public static ArmType GetArmType(int assetID)
    {
        return Instance.ARData[assetID].armType;
    }

    public static int AssetNameToIDHead(string name)
    {
        for (int i = 0; i < Instance.HDData.Length; i++)
        {
            if (name == Instance.HDData[i].asset.name)
            {
                return i;
            }
        }
        Debug.LogError("Asset named: " + name + " | Has not been found (Head)");
        return 0;
    }

    public static int AssetNameToIDUpperTorso(string name)
    {
        for (int i = 0; i < Instance.UTData.Length; i++)
        {
            if (name == Instance.UTData[i].asset.name)
            {
                return i;
            }
        }
        Debug.LogError("Asset named: " + name + " | Has not been found (Upper Torso)");
        return 0;
    }

    public static int AssetNameToIDArms(string name)
    {
        for (int i = 0; i < Instance.ARData.Length; i++)
        {
            if (name == Instance.ARData[i].asset.name)
            {
                return i;
            }
        }
        Debug.LogError("Asset named: " + name + " | Has not been found (Arms)");
        return 0;
    }

    public static string AssetIDToNameHead(int ID)
    {
        return Instance.HDData[ID].asset.name;
    }

    public static string AssetIDToNameUpperTorso(int ID)
    {
        return Instance.UTData[ID].asset.name;
    }

    public static string AssetIDToNameArms(int ID)
    {
        return Instance.ARData[ID].asset.name;
    }

    /// <summary>
    /// Assembles the upper torso based on asset names given, parents it and returns it as GameObject.
    /// </summary>
    /// <returns>The upper torso.</returns>
    /// <param name="torsoAssetName">Name of the torso asset.</param>
    /// <param name="armAssetName">Name of the arm asset.</param>
    /// <param name="headAssetName">Name of the head asset.</param>
    /// <param name="parent">Transform to be used as the parent of this upper torso, this should come as output from lower torso assembler.</param>
    /// <param name="weaponMountL">Output reference to Left Weapon Mount.</param>
    /// <param name="weaponMountR">Output reference to Right Weapon Mount</param>
    /// <param name="shieldMountL">Output reference to Left Shield Mount</param>
    /// <param name="shieldMountR">Output reference to Right Shield Mount</param>
    /// <param name="gimbalMountL">Output reference to Left Gimbal Mount</param>
    /// <param name="gimbalMountR">Output reference to Right Gimbal Mount</param>
    /// <param name="anim">Output reference to Animator component</param>
    public GameObject AssembleUpperTorso(string torsoAssetName, string armAssetName, string headAssetName, Transform parent, out Transform weaponMountL, out Transform weaponMountR, out Transform shieldMountL, out Transform shieldMountR,
        out Transform gimbalMountL, out Transform gimbalMountR, out Animator anim)
	{
        GameObject returnObject = TorsoAssembler(UTAssetNameToID(torsoAssetName), ARAssetNameToID(armAssetName), HDAssetNameToID(headAssetName), parent);

        weaponMountL = tempMeshAvatar[(int)UTMeshID.WeaponMountL];
        weaponMountR = tempMeshAvatar[(int)UTMeshID.WeaponMountR];
        shieldMountL = tempMeshAvatar[(int)UTMeshID.ShieldMountL];
        shieldMountR = tempMeshAvatar[(int)UTMeshID.ShieldMountR];
        gimbalMountL = tempMeshAvatar[(int)UTMeshID.GimbalMountL];
        gimbalMountR = tempMeshAvatar[(int)UTMeshID.GimbalMountR];
        anim = tempAnim;
        return returnObject;
	}

    /// <summary>
    /// Assembles the upper torso based on asset names given, parents it and returns it as GameObject.
    /// </summary>
    /// <returns>The upper torso.</returns>
    /// <param name="torsoID">ID of the torso asset.</param>
    /// <param name="armsID">ID of the arm asset.</param>
    /// <param name="headID">ID of the head asset.</param>
    /// <param name="parent">Transform to be used as the parent of this upper torso, this should come as output from lower torso assembler.</param>
    /// <param name="weaponMountL">Output reference to Left Weapon Mount.</param>
    /// <param name="weaponMountR">Output reference to Right Weapon Mount</param>
    /// <param name="shieldMountL">Output reference to Left Shield Mount</param>
    /// <param name="shieldMountR">Output reference to Right Shield Mount</param>
    /// <param name="gimbalMountL">Output reference to Left Gimbal Mount</param>
    /// <param name="gimbalMountR">Output reference to Right Gimbal Mount</param>
    /// <param name="anim">Output reference to Animator component</param>
    public GameObject AssembleUpperTorso(int torsoID, int armsID, int headID, Transform parent, out Transform weaponMountL, out Transform weaponMountR, out Transform shieldMountL, out Transform shieldMountR,
        out Transform gimbalMountL, out Transform gimbalMountR, out Animator anim)
    {
        GameObject returnObject = TorsoAssembler(torsoID, armsID, headID, parent);

        weaponMountL = tempMeshAvatar[(int)UTMeshID.WeaponMountL];
        weaponMountR = tempMeshAvatar[(int)UTMeshID.WeaponMountR];
        shieldMountL = tempMeshAvatar[(int)UTMeshID.ShieldMountL];
        shieldMountR = tempMeshAvatar[(int)UTMeshID.ShieldMountR];
        gimbalMountL = tempMeshAvatar[(int)UTMeshID.GimbalMountL];
        gimbalMountR = tempMeshAvatar[(int)UTMeshID.GimbalMountR];
        anim = tempAnim;
        return returnObject;
    }

    public int UTAssetNameToID(string name)
    {
        for (int i = 0; i < UTData.Length; i++)
        {
            if (name == UTData[i].asset.name)
                return i;
        }
        Debug.LogError("Upper Torso Asset Named: " + name + " | Has not been found");
        return 0;
    }

    public int ARAssetNameToID(string name)
    {
        for (int i = 0; i < ARData.Length; i++)
        {
            if (name == ARData[i].asset.name)
                return i;
        }
        Debug.LogError("Arm Asset Named: " + name + " | Has not been found");
        return 0;
    }

    public int HDAssetNameToID(string name)
    {
        for (int i = 0; i < HDData.Length; i++)
        {
            if (name == HDData[i].asset.name)
                return i;
        }
        Debug.LogError("Head Asset Named: " + name + " | Has not been found");
        return 0;
    }

    public GameObject AssembleUpperTorso(int torsoID, int armID, int headID, Transform parent, out Transform weaponMountL, out Transform weaponMountR, out Transform shieldMountL, out Transform shieldMountR,
        out Transform gimbalMountL, out Transform gimbalMountR)
    {
        GameObject returnObject = TorsoAssembler(torsoID, armID, headID, parent);

        weaponMountL = tempMeshAvatar[(int)UTMeshID.WeaponMountL];
        weaponMountR = tempMeshAvatar[(int)UTMeshID.WeaponMountR];
        shieldMountL = tempMeshAvatar[(int)UTMeshID.ShieldMountL];
        shieldMountR = tempMeshAvatar[(int)UTMeshID.ShieldMountR];
        gimbalMountL = tempMeshAvatar[(int)UTMeshID.GimbalMountL];
        gimbalMountR = tempMeshAvatar[(int)UTMeshID.GimbalMountR];
        return returnObject;
    }
        

    GameObject TorsoAssembler(int torsoID, int armID, int headID, Transform parent)
    {
        GameObject rigInstance = GameObject.Instantiate(UpperTorsoBaseSkeleton, Vector3.zero, Quaternion.identity);
        GameObject upperTorsoInstance = GameObject.Instantiate(UTData[torsoID].asset, Vector3.zero, Quaternion.identity);
        GameObject armsInstance = GameObject.Instantiate(ARData[armID].asset, Vector3.zero, Quaternion.identity);
        GameObject headInstance;
        if (UTData[torsoID].hasHead)
        {
            headInstance = GameObject.Instantiate(HDData[headID].asset, Vector3.zero, Quaternion.identity);
        }
        else
        {
            headInstance = rigInstance;
        }

        GenerateBoneHierarchy(rigInstance);
        GenerateMeshArray(upperTorsoInstance, armsInstance, headInstance, ARData[armID].armType, UTData[torsoID].hasHead);

        CombineObjectsFromArray();
        tempAnim = rigInstance.GetComponent<Animator>();
        OverrideAnimationClips(torsoID, armID, ARData[armID].armType);

        rigInstance.transform.SetParent(parent);
        rigInstance.transform.localPosition = Vector3.zero;
        rigInstance.transform.localRotation = Quaternion.Euler(Vector3.zero);

        GameObject.Destroy(armsInstance);
        return rigInstance;
    }

    void GenerateBoneHierarchy(GameObject skeletonInstance)
    {
        Transform curTransform = skeletonInstance.transform;
        tempRigAvatar = new Transform[12];
        tempRigAvatar[(int)UTRigID.Root] = curTransform;
        curTransform = curTransform.GetChild(0);
        tempRigAvatar[(int)UTRigID.UpperTorsoCore] = curTransform;

        for (int i = 0; i < 4; i++)
        {
            curTransform = tempRigAvatar[(int)UTRigID.UpperTorsoCore].GetChild(i);
            string curName = curTransform.name;
            if (curName[0] == 'H')
            {
                tempRigAvatar[(int)UTRigID.Head] = curTransform;
            }
            else if (curName[curName.Length - 1] == 'L')
            {
                tempRigAvatar[(int)UTRigID.UpperArmL] = curTransform;
                curTransform = curTransform.GetChild(0);
                tempRigAvatar[(int)UTRigID.LowerArmL] = curTransform;
                curTransform = curTransform.GetChild(0);
                tempRigAvatar[(int)UTRigID.HandL] = curTransform;
            }
            else if (curName[curName.Length - 1] == 'R')
            {
                tempRigAvatar[(int)UTRigID.UpperArmR] = curTransform;
                curTransform = curTransform.GetChild(0);
                tempRigAvatar[(int)UTRigID.LowerArmR] = curTransform;
                curTransform = curTransform.GetChild(0);
                tempRigAvatar[(int)UTRigID.HandR] = curTransform;
            }
            else
            {
                tempRigAvatar[(int)UTRigID.WeaponMaster] = curTransform;
                tempRigAvatar[(int)UTRigID.WeaponL] = curTransform.GetChild(0);
                tempRigAvatar[(int)UTRigID.WeaponR] = curTransform.GetChild(1);
            }
        }
    }

    void GenerateMeshArray(GameObject torso, GameObject arms, GameObject head, ArmType armType, bool hasHead)
    {
        tempMeshAvatar = new Transform[14];
        if (hasHead)
        {
            tempMeshAvatar[(int)UTMeshID.Head] = head.transform;
        }
        Transform curTransform = torso.transform;
        tempMeshAvatar[(int)UTMeshID.UpperTorsoCore] = curTransform;

        int childCount = curTransform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            curTransform = tempMeshAvatar[(int)UTMeshID.UpperTorsoCore].GetChild(i);
            if (curTransform.name[0] == 'G' && curTransform.name[curTransform.name.Length - 1] == 'L')
            {
                curTransform = curTransform.GetChild(0);
                curTransform = curTransform.GetChild(0);
                tempMeshAvatar[(int)UTMeshID.GimbalMountL] = curTransform;
            }
            else if (curTransform.name[0] == 'G' && curTransform.name[curTransform.name.Length - 1] == 'R')
            {
                curTransform = curTransform.GetChild(0);
                curTransform = curTransform.GetChild(0);
                tempMeshAvatar[(int)UTMeshID.GimbalMountR] = curTransform;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            curTransform = arms.transform.GetChild(i);
            if (curTransform.name[curTransform.name.Length - 1] == 'L')
            {
                tempMeshAvatar[(int)UTMeshID.UpperArmL] = curTransform;
                // single joint arm
                if (armType != ArmType.SingleJoint)
                {
                    curTransform = curTransform.GetChild(0);
                    tempMeshAvatar[(int)UTMeshID.LowerArmL] = curTransform;
                    // two joint arm
                    if (armType == ArmType.ThreeJoint)
                    {
                        curTransform = curTransform.GetChild(0);
                        tempMeshAvatar[(int)UTMeshID.HandL] = curTransform;
                        //three joint arm
                    }
                }
                GetHardPoints(curTransform, 0);
            }
            else
            {
                tempMeshAvatar[(int)UTMeshID.UpperArmR] = curTransform;
                // single joint arm
                if (armType != ArmType.SingleJoint)
                {
                    curTransform = curTransform.GetChild(0);
                    tempMeshAvatar[(int)UTMeshID.LowerArmR] = curTransform;
                    // two joint arm
                    if (armType == ArmType.ThreeJoint)
                    {
                        curTransform = curTransform.GetChild(0);
                        tempMeshAvatar[(int)UTMeshID.HandR] = curTransform;
                        //three joint arm
                    }
                }
                GetHardPoints(curTransform, 1);
            }
        }
//        for (int i = 0; i < 14; i++)
//        {
//            if (tempMeshAvatar[i] != null)
//            {
//                Debug.Log(tempMeshAvatar[i].name + "   " + (UTMeshID)i);
//            }
//        }
    }

    void GetHardPoints(Transform curTranform, int side)
    {
        Transform temp;
        if (side == 0)
        {
            for (int i = 0; i < curTranform.childCount; i++)
            {
                temp = curTranform.GetChild(i);
                if (temp.name[0] == 'S')
                {
                    tempMeshAvatar[(int)UTMeshID.ShieldMountL] = temp;
                }
                else if (temp.name[0] == 'W')
                {
                    tempMeshAvatar[(int)UTMeshID.WeaponMountL] = temp;
                }
            }
        }
        else
        {
            for (int i = 0; i < curTranform.childCount; i++)
            {
                temp = curTranform.GetChild(i);
                if (temp.name[0] == 'S')
                {
                    tempMeshAvatar[(int)UTMeshID.ShieldMountR] = temp;
                }
                else if (temp.name[0] == 'W')
                {
                    tempMeshAvatar[(int)UTMeshID.WeaponMountR] = temp;
                }
            }
        }
    }

    void CombineObjectsFromArray()
    {
        for (int i = 0; i < 8; i++)
        {
            if (tempMeshAvatar[i] != null)
            {
                tempMeshAvatar[i].SetParent(tempRigAvatar[i]);
                tempMeshAvatar[i].localPosition = Vector3.zero;
                tempMeshAvatar[i].localRotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }


    Animator tempAnim;
    void OverrideAnimationClips(int torsoID, int armID, ArmType armType)
    {
        AnimatorOverrideController animOverride = new AnimatorOverrideController(UTAnimationController);
        tempAnim.runtimeAnimatorController = UTAnimationController;
        tempAnim.applyRootMotion = true;

        AnimationClip[] oc = tempAnim.runtimeAnimatorController.animationClips;

        animOverride[oc[0].name] = LoadAdditiveFixClip(torsoID, armID);

        Debug.LogWarning("Animation override for upper torso is currently in its bare bones form and will stay that way untill all animations are in engine");

        tempAnim.runtimeAnimatorController = animOverride;
    }

    string UTAnimClipsDataPath = "Data/ProceduralAnimPoses/UT/";
    AnimationClip LoadAdditiveFixClip(int torsoID, int armID)
    {
        string pathToClip = UTAnimClipsDataPath + UTData[torsoID].asset.name + "-" + ARData[armID].asset.name;
        return Resources.Load(pathToClip, typeof(AnimationClip)) as AnimationClip;
    }
}
