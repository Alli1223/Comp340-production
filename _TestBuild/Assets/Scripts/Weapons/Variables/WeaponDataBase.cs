using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeightClass : int {Light, Medium, Heavy};

// This Class is used purely to store weapon data sorted by Weapon.WeaponType
[Serializable]
public class WeaponDataBase : ScriptableObject 
{
    public static WeaponDataBase Instance;

    [SerializeField]
    public WeaponType[] weaponTypeArray;

    public static void GetWeaponIDByString(out int weaponType, out int weaponID, string name)
    {
        for (int i = 0; i < Instance.weaponTypeArray.Length; i++)
        {
            for (int c = 0; c < Instance.weaponTypeArray[i].weapons.Length; c++)
            {
                if (name == Instance.weaponTypeArray[i].weapons[c].name)
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

[System.Serializable]
public enum WeaponClass : int
{
    AutoCannon,
    Chaingun,
    Mortar, 
    MissileSilo,
    RocketSilo,
    ScatterCannon,
    SniperCannon,
    SnubCannon
}
    

// This will be used primarly by WeaponDataBase ScriptableObject in order to save array of arrays
[Serializable]
public struct WeaponType
{
    // These are used by customisation system to spawn in correct objects
    [SerializeField]
    public Weapon[] weapons;
}

[Serializable]
public struct Weapon
{
    [SerializeField]
    public string name;
    [SerializeField]
    public GameObject weaponAsset;
    [SerializeField]
    public GameObject weaponGimbalMountAsset;
    [SerializeField]
    public GameObject weaponStaticMountAsset;

    [SerializeField]
    public int minDamage; //{ get; private set; }
    [SerializeField]
    public int maxDamage; //{ get; private set; }

    [SerializeField]
    public int minRange; //{ get; private set; }
    [SerializeField]
    public int maxRange; //{ get; private set; }

    [SerializeField]
    public int splashRadius;
    [SerializeField]
    public int scatter;

    [SerializeField]
    public int apCost;
    [SerializeField]
    public WeightClass weightClass;
    [SerializeField]
    public int numberOfShots;

    [SerializeField]
    public float accuracy;
}