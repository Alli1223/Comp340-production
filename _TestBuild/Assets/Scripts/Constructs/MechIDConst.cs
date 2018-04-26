using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;


public class MechIDConst : MonoBehaviour 
{
    public MechData data;

    public int totalWeight { get; private set; }
    public List<PassiveBonus> passiveBonuses { get; private set; }
    public int totalAP { get; private set; }
    public int totalArmor { get; private set; }



    public Transform weaponMountL;
    public Transform weaponMountR;
    public Transform shieldMountL;
    public Transform shieldMountR;
    public Transform gimbalMountL;

    public Transform gimbalMountR;

    public static int AssetNameToIDHead(string name)
    {
        return UTRigAssembler.AssetNameToIDHead(name);
    }

    public static int AssetNameToIDUpperTorso(string name)
    {
        return UTRigAssembler.AssetNameToIDUpperTorso(name);
    }

    public static int AssetNameToIDArms(string name)
    {
        return UTRigAssembler.AssetNameToIDArms(name);
    }

    public static int AssetNameToIDLowerTorso(string name)
    {
        return LTRigAssembler.AssetNameToIDLowerTorso(name);
    }

    public static int AssetNameToIDLegs(string name)
    {
        return LTRigAssembler.AssetNameToIDLegs(name);
    }

    public static string AssetIDToNameHead(int ID)
    {
        return UTRigAssembler.AssetIDToNameHead(ID);
    }

    public static string AssetIDToNameUpperTorso(int ID)
    {
        return UTRigAssembler.AssetIDToNameUpperTorso(ID);
    }

    public static string AssetIDToNameArms(int ID)
    {
        return UTRigAssembler.AssetIDToNameArms(ID);
    }

    public static string AssetIDToNameLegs(int ID)
    {
        return LTRigAssembler.AssetIDToNameLegs(ID);
    }

    public static string AssetIDToNameLowerTorso(int ID)
    {
        return LTRigAssembler.AssetIDToNameLowerTorso(ID); 
    }

//    public static GameObject SpawnMech(Vector3 pos, Quaternion rot, int headID, int upperTorsoID, int lowerTorsoID, int armsID, int legsID,
//        int weaponTypeL, int weaponL, int weaponTypeR, int weaponR, int weaponGimbalTypeL, int weaponGimbalL,
//        int weaponGimbalTypeR, int weaponGimbalR)
//    {
//        Transform upperTorsoParent;
//        Animator lowerTorsoAnim;
//        GameObject lowerTorso = LTRigAssembler.Instance.AssembleLowerTorso(lowerTorsoID, legsID, pos, rot, out upperTorsoParent, out lowerTorsoAnim);
//
//        MechVisualAgent visualAgent = lowerTorso.AddComponent<MechVisualAgent>();
//        PlayerData playerData = lowerTorso.AddComponent<PlayerData>();
//
//        visualAgent.mechID = lowerTorso.AddComponent<MechIDConst>();
//        visualAgent.upperTorsoParent = upperTorsoParent;
//
//        UTRigAssembler.Instance.AssembleUpperTorso(upperTorsoID, armsID, headID, visualAgent.upperTorsoParent, 
//            out visualAgent.weaponMountL, out visualAgent.weaponMountR, out visualAgent.shieldMountL, out visualAgent.shieldMountR,
//            out visualAgent.weaponMountGimbalL, out visualAgent.weaponMountGimbalR);
//
//        GameObject go;
//        if (visualAgent.mechID.data.hasWeaponL)
//        {
//            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[weaponTypeL].weapons[weaponL].weaponGimbalMountAsset, visualAgent.weaponMountL) as GameObject;
//            visualAgent.animWeaponL = go.GetComponent<Animator>();
//            go.transform.localPosition = Vector3.zero;
//            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
//            go.transform.localScale = Vector3.zero;
//        }
//
//        if (visualAgent.mechID.data.hasWeaponR)
//        {
//            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[weaponTypeR].weapons[weaponR].weaponGimbalMountAsset, visualAgent.weaponMountR) as GameObject;
//            visualAgent.animWeaponL = go.GetComponent<Animator>();
//            go.transform.localPosition = Vector3.zero;
//            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
//            go.transform.localScale = Vector3.zero;
//        }
//
//        if (visualAgent.mechID.data.hasWeaponGimbalL)
//        {
//            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[weaponGimbalTypeL].weapons[weaponGimbalL].weaponGimbalMountAsset, visualAgent.weaponMountGimbalL) as GameObject;
//            visualAgent.animWeaponL = go.GetComponent<Animator>();
//            go.transform.localPosition = Vector3.zero;
//            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
//            go.transform.localScale = Vector3.zero;
//        }
//
//        if (visualAgent.mechID.data.hasWeaponGimbalR)
//        {
//            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[weaponGimbalTypeR].weapons[weaponGimbalR].weaponGimbalMountAsset, visualAgent.weaponMountGimbalR) as GameObject;
//            visualAgent.animWeaponL = go.GetComponent<Animator>();
//            go.transform.localPosition = Vector3.zero;
//            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
//            go.transform.localScale = Vector3.zero;
//        }
//
//
//
//        return lowerTorso;
//    }

    public static GameObject SpawnMech(Vector3 pos, Quaternion rot, MechData mechData, bool animationEnabled)
    {
        Transform upperTorsoParent;
        Animator lowerTorsoAnimator;
        GameObject lowerTorso = LTRigAssembler.Instance.AssembleLowerTorso(
            PartsDataBase.Instance.mechPartType[(int)PartType.LowerTorso].part[mechData.lowerTorsoID].assetID, 
            PartsDataBase.Instance.mechPartType[(int)PartType.Legs].part[mechData.legsID].assetID, 
            pos, rot, out upperTorsoParent, out lowerTorsoAnimator);

        MechVisualAgent visualAgent = lowerTorso.AddComponent<MechVisualAgent>();
        PlayerData mechInfo = lowerTorso.AddComponent<PlayerData>();
        MechIDConst mechIDConst = lowerTorso.AddComponent<MechIDConst>();
        NavMeshAgent navMeshAgent = lowerTorso.AddComponent<NavMeshAgent>();

        mechInfo.mechID = mechIDConst;
        mechIDConst.data = mechData;
        visualAgent.mechID = mechIDConst;
        visualAgent.upperTorsoParent = upperTorsoParent;
        visualAgent.animLowerTorso = lowerTorsoAnimator;
        navMeshAgent.radius = 0.2f;
        int armAssetID = PartsDataBase.Instance.mechPartType[(int)PartType.Arms].part[mechData.armsID].assetID;

        UTRigAssembler.Instance.AssembleUpperTorso(
            PartsDataBase.Instance.mechPartType[(int)PartType.UpperTorso].part[mechData.upperTorsoID].assetID,
            armAssetID,
            PartsDataBase.Instance.mechPartType[(int)PartType.Head].part[mechData.headID].assetID,
            visualAgent.upperTorsoParent, out visualAgent.weaponMountL,
            out visualAgent.weaponMountR, out visualAgent.shieldMountL, out visualAgent.shieldMountR, out visualAgent.weaponMountGimbalL, out visualAgent.weaponMountGimbalR, out visualAgent.animUpperTorso);

        GameObject go;



        if (visualAgent.mechID.data.hasWeaponL)
        {
            mechInfo.leftArmWeapon = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmLType].weapons[mechData.weaponArmLID];
            mechInfo.hasLeftArmWeapon = true;
            go = GameObject.Instantiate(mechInfo.leftArmWeapon.weaponAsset, visualAgent.weaponMountL) as GameObject;
            visualAgent.animWeaponL = go.GetComponent<Animator>();
            visualAgent.particleInfoArmL = go.GetComponent<WeaponParticleInfo>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.one;

            if (UTRigAssembler.GetArmType(armAssetID) == UTRigAssembler.ArmType.SingleJoint)
            {
                go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmLType].weapons[mechData.weaponArmLID].weaponGimbalMountAsset, go.transform) as GameObject;
            }
            else
            {
                go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmLType].weapons[mechData.weaponArmLID].weaponStaticMountAsset, go.transform) as GameObject;
            }

            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.one;

        }

        if (visualAgent.mechID.data.hasWeaponR)
        {
            mechInfo.rightArmWeapon = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmRType].weapons[mechData.weaponArmRID];
            mechInfo.hasRightArmWeapon = true;
            go = GameObject.Instantiate(mechInfo.rightArmWeapon.weaponAsset, visualAgent.weaponMountR) as GameObject;
            visualAgent.animWeaponR = go.GetComponent<Animator>();
            visualAgent.particleInfoArmR = go.GetComponent<WeaponParticleInfo>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.one;

            if (UTRigAssembler.GetArmType(armAssetID) == UTRigAssembler.ArmType.SingleJoint)
            {
                go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmRType].weapons[mechData.weaponArmRID].weaponGimbalMountAsset, go.transform) as GameObject;
            }
            else
            {
                go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmRType].weapons[mechData.weaponArmRID].weaponStaticMountAsset, go.transform) as GameObject;
            }

            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.one;
        }

        if (visualAgent.mechID.data.hasWeaponGimbalL)
        {
            mechInfo.backWeapon = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponGimbalLType].weapons[mechData.weaponGimbalLID];
            mechInfo.hasBackWeapon = true;
            go = GameObject.Instantiate(mechInfo.backWeapon.weaponAsset, visualAgent.weaponMountGimbalL) as GameObject;
            visualAgent.animGimbalL = go.GetComponent<Animator>();
            visualAgent.particleInfoGimbL = go.GetComponent<WeaponParticleInfo>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.one;

            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[mechData.weaponGimbalLType].weapons[mechData.weaponGimbalLID].weaponGimbalMountAsset, go.transform) as GameObject;

            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.one;
        }

        if (visualAgent.mechID.data.hasWeaponGimbalR)
        {
            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[mechData.weaponGimbalRType].weapons[mechData.weaponGimbalRID].weaponAsset, visualAgent.weaponMountGimbalR) as GameObject;
            visualAgent.animGimbalR = go.GetComponent<Animator>();
            visualAgent.particleInfoGimbR = go.GetComponent<WeaponParticleInfo>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.one;

            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[mechData.weaponGimbalRType].weapons[mechData.weaponGimbalRID].weaponGimbalMountAsset, go.transform) as GameObject;

            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.one;
        }

        visualAgent.SetAnimation(animationEnabled);

        visualAgent.UpdateRendererReferences();


        visualAgent.ChangeColor(0, mechData.Color1);
        visualAgent.ChangeColor(1, mechData.Color2);
        visualAgent.ChangeColor(2, mechData.Color3);

        mechInfo.maxHealth = PartsDataBase.CalculateTotalArmor(mechData.headID, mechData.upperTorsoID, mechData.lowerTorsoID, mechData.legsID, mechData.armsID);
        mechInfo.curHealth = mechInfo.maxHealth;

        mechInfo.maxAP = PartsDataBase.CalculateTotalAP(mechData.headID, mechData.upperTorsoID, mechData.lowerTorsoID, mechData.legsID, mechData.armsID);
        mechInfo.curAP = mechInfo.maxAP;

        PassiveBonus pb = PartsDataBase.CalculateTotalBonus(mechData.headID, mechData.upperTorsoID, mechData.lowerTorsoID, mechData.legsID, mechData.armsID);

        mechInfo.maxMoveDist = PartsDataBase.CalculateTotalMovement(PartsDataBase.CalculateTotalWeight(mechData), pb.bonusMovement);

        mechInfo.visualAgent = visualAgent;



        lowerTorso.tag = "Player";
        lowerTorso.layer = 9;
        return lowerTorso;

    }
}
