using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class MechSaveData
{
    const string playerMechDataPath = "Assets/Resources/Data";
    /// <summary>
    /// Gives you a location of player mechs. This string must be used inside string.Format to function correctly, arg0 should be playerID, arg1 should be the mech name. This also needs to be added to Application.persistentDataPath
    /// </summary>
    public const string playerMechPath = "/Profiles/{0}/Mechs/{1}.mech";


    public static void SaveMech(MechData data, string playerName, string name)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        file = File.Open(Application.persistentDataPath + string.Format(playerMechPath, playerName, name), FileMode.OpenOrCreate);

        bf.Serialize(file, data);
        file.Close();
    }

    public static MechData LoadMech(string playerName, string name)
    {
        string path = Application.persistentDataPath + string.Format(playerMechPath, playerName, name);
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            MechData ret = bf.Deserialize(file) as MechData;

            file.Close();

            return ret;
        }
        else
        {
            Debug.LogError(path + " - Does not exists");
            return new MechData();
        }
    }


}


[Serializable]
public class MechData
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

    [SerializeField]
    public int Color1;
    [SerializeField]
    public int Color2;
    [SerializeField]
    public int Color3;
}
