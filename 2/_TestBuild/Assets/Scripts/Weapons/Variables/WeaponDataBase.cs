using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This Class is used purely to store weapon data sorted by Weapon.WeaponType
[Serializable]
public class WeaponDataBase : ScriptableObject 
{
    public static WeaponDataBase Instance;

    [SerializeField]
    public WeaponTypeStruct[] weaponTypeArray;

    public static void GetWeaponIDByString(out int weaponType, out int weaponID, string name)
    {
        for (int i = 0; i < Instance.weaponTypeArray.Length; i++)
        {
            for (int c = 0; c < Instance.weaponTypeArray[i].weapons.Length; c++)
            {
                if (name == Instance.weaponTypeArray[i].weapons[c].identifierName)
                {
                    weaponType = i;
                    weaponID = c;
                    return;
                }

            }
        }
        weaponType = 0;
        weaponID = 0;
        Debug.LogError("Weapon with identifier: " + name + " does not exist in the database, check the names in Utilities > Weapon Database menu");
    }

    public static GameObject InstantiateWeapon()
    {
        return null;
    }
}
    

// This will be used primarly by WeaponDataBase ScriptableObject in order to save array of arrays
[Serializable]
public struct WeaponTypeStruct
{
    // These are used by customisation system to spawn in correct objects
    [SerializeField]
    public WeaponAsset[] weapons;
}

[Serializable]
public struct WeaponAsset
{
    [SerializeField]
    public string identifierName;
    [SerializeField]
    public GameObject weaponAsset;
    [SerializeField]
    public GameObject weaponGimbalMountAsset;
    [SerializeField]
    public GameObject weaponStaticMountAsset;
}


