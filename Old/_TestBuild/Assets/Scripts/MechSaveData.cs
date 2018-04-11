using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class MechSaveData
{
    const string playerMechDataPath = "Assets/Resources/Data";

    public static void SaveMech(MechIDConst mech)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Open(Application.persistentDataPath + "/" + mech.displayName + ".mech", FileMode.Create);


        MechData data = new MechData();
        data.name = mech.displayName;
        data.headID = mech.assetIDHead;
        data.armsID = mech.assetIDArms;
        data.legsID = mech.assetIDLegs;
        data.upperTorsoID = mech.assetIDUpperTorso;
        data.lowerTorsoID = mech.assetIDLowerTorso;

        data.hasWeaponL = mech.hasWeaponL;
        data.hasWeaponR = mech.hasWeaponR;
        data.hasWeaponGimbalL = mech.hasWeaponGimbalL;
        data.hasWeaponGimbalR = mech.hasWeaponGimbalR;

        data.weaponArmLType = mech.weaponArmLType;
        data.weaponArmLID = mech.weaponArmLID;
        data.weaponArmRType = mech.weaponArmRType;
        data.weaponArmRID = mech.weaponArmRID;
        data.weaponGimbalLType = mech.weaponGimbalLType;
        data.weaponGimbalLID = mech.weaponGimbalLID;
        data.weaponGimbalRType = mech.weaponGimbalRType;
        data.weaponGimbalRID = mech.weaponGimbalRID;



        bf.Serialize(file, data);
        file.Close();
    }

    public static void LoadMech(ref MechIDConst mech, string name)
    {
        if (File.Exists(Application.persistentDataPath + "/" + name + ".mech"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + name + ".mech", FileMode.Open);
            MechData data = bf.Deserialize(file) as MechData;
            file.Close();

            mech.assetIDArms = data.armsID;
            mech.assetIDHead = data.headID;
            mech.assetIDLegs = data.legsID;
            mech.assetIDLowerTorso = data.lowerTorsoID;
            mech.assetIDUpperTorso = data.upperTorsoID;

            mech.hasWeaponL = data.hasWeaponL;
            mech.hasWeaponR = data.hasWeaponR;
            mech.hasWeaponGimbalL = data.hasWeaponGimbalL;
            mech.hasWeaponGimbalR = data.hasWeaponGimbalR;

            mech.weaponArmLType = data.weaponArmLType;
            mech.weaponArmLID = data.weaponArmLID;
            mech.weaponArmRType = data.weaponArmRType;
            mech.weaponArmRID = data.weaponArmRID;
            mech.weaponGimbalLType = data.weaponGimbalLType;
            mech.weaponGimbalLID = data.weaponGimbalLID;
            mech.weaponGimbalRType = data.weaponGimbalRType;
            mech.weaponGimbalRID = data.weaponGimbalRID;

            mech.displayName = data.name;
        }
    }


}


[Serializable]
class MechData
{
    [SerializeField]
    public string name;
    [SerializeField]
    public int headID;
    [SerializeField]
    public int armsID;
    [SerializeField]
    public int legsID;
    [SerializeField]
    public int upperTorsoID;
    [SerializeField]
    public int lowerTorsoID;

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
}
