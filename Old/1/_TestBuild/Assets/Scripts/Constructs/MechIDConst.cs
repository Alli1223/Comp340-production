using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MechIDConst : MonoBehaviour 
{
    [SerializeField]
    public string displayName;

    [SerializeField]
    public string assetNameHead;
    [SerializeField]
    public string assetNameUpperTorso;
    [SerializeField]
    public string assetNameLowerTorso;
    [SerializeField]
    public string assetNameLegs;
    [SerializeField]
    public string assetNameArms;

    [SerializeField]
    public int assetIDHead;
    [SerializeField]
    public int assetIDUpperTorso;
    [SerializeField]
    public int assetIDLowerTorso;
    [SerializeField]
    public int assetIDLegs;
    [SerializeField]
    public int assetIDArms;

    [SerializeField]
    public int weaponArmLType;
    [SerializeField]
    public int weaponArmLID;
    [SerializeField]
    public int weaponArmRType;
    [SerializeField]
    public int weaponArmRID;
    [SerializeField]
    public int weaponGimbalLType;
    [SerializeField]
    public int weaponGimbalLID;
    [SerializeField]
    public int weaponGimbalRType;
    [SerializeField]
    public int weaponGimbalRID;

    [SerializeField]
    public bool hasWeaponL;
    [SerializeField]
    public bool hasWeaponR;
    [SerializeField]
    public bool hasWeaponGimbalL;
    [SerializeField]
    public bool hasWeaponGimbalR;

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
        return LTRigAssembler.AssetIDToNameLowerTorso(ID);
    }

    public static string AssetIDToNameLowerTorso(int ID)
    {
        return LTRigAssembler.AssetIDToNameLegs(ID); 
    }

    public static GameObject SpawnMech(Vector3 pos, Quaternion rot, int headID, int upperTorsoID, int lowerTorsoID, int armsID, int legsID,
        int weaponTypeL, int weaponL, int weaponTypeR, int weaponR, int weaponGimbalTypeL, int weaponGimbalL,
        int weaponGimbalTypeR, int weaponGimbalR)
    {
        Transform upperTorsoParent;
        Animator lowerTorsoAnim;
        GameObject lowerTorso = LTRigAssembler.Instance.AssembleLowerTorso(lowerTorsoID, legsID, pos, rot, out upperTorsoParent, out lowerTorsoAnim);

        MechVisualAgent visualAgent = lowerTorso.AddComponent<MechVisualAgent>();

        visualAgent.mechID = lowerTorso.AddComponent<MechIDConst>();
        visualAgent.upperTorsoParent = upperTorsoParent;

        UTRigAssembler.Instance.AssembleUpperTorso(upperTorsoID, armsID, headID, visualAgent.upperTorsoParent, 
            out visualAgent.weaponMountL, out visualAgent.weaponMountR, out visualAgent.shieldMountL, out visualAgent.shieldMountR,
            out visualAgent.weaponMountGimbalL, out visualAgent.weaponMountGimbalR);

        GameObject go;
        if (visualAgent.mechID.hasWeaponL)
        {
            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[weaponTypeL].weapons[weaponL].weaponGimbalMountAsset, visualAgent.weaponMountL) as GameObject;
            visualAgent.animWeaponL = go.GetComponent<Animator>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.zero;
        }

        if (visualAgent.mechID.hasWeaponR)
        {
            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[weaponTypeR].weapons[weaponR].weaponGimbalMountAsset, visualAgent.weaponMountR) as GameObject;
            visualAgent.animWeaponL = go.GetComponent<Animator>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.zero;
        }

        if (visualAgent.mechID.hasWeaponGimbalL)
        {
            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[weaponGimbalTypeL].weapons[weaponGimbalL].weaponGimbalMountAsset, visualAgent.weaponMountGimbalL) as GameObject;
            visualAgent.animWeaponL = go.GetComponent<Animator>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.zero;
        }

        if (visualAgent.mechID.hasWeaponGimbalR)
        {
            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[weaponGimbalTypeR].weapons[weaponGimbalR].weaponGimbalMountAsset, visualAgent.weaponMountGimbalR) as GameObject;
            visualAgent.animWeaponL = go.GetComponent<Animator>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.zero;
        }

        return lowerTorso;
    }
}
