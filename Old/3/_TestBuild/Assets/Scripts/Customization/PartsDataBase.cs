using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType : int { Head, UpperTorso, LowerTorso, Legs, Arms };

public class PartsDataBase : ScriptableObject 
{
    public static PartsDataBase Instance;

    const int baseVision = 10;
    static int[] maxMovement = new int[28]
        {
        15, //0
        15, //1
        15, //2
        15, //3
        15, //4
        15, //5
        15, //6
        15, //7
        14, //8
        14, //9
        13, //10
        13, //11
        12, //12
        12, //13
        11, //14
        10, //15
        9,  //16
        8,  //17
        7,  //18
        6,  //19
        5,  //20
        5,  //21
        5,  //22
        5,  //23
        5,  //24
        5,  //25
        5,  //26
        5   //27
    };

        


    [SerializeField]
    public MechPartType[] mechPartType = new MechPartType[5];

    public void GenerateID()
    {
        for (int i = 0; i < mechPartType.Length; i++)
        {
            for (int c = 0; c < mechPartType[i].part.Length; c++)
            {
                if (mechPartType[i].part[c].asset == null)
                {
                    Debug.LogError(mechPartType[i].part[c].name + string.Format(" ({0}) Does not have a asset assigned, please assign one you dingbat", (PartType)i));
                    mechPartType[i].part[c].assetID = 0;
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            mechPartType[i].part[c].assetID = UTRigAssembler.AssetNameToIDHead(mechPartType[i].part[c].asset.name);
                            break;
                        case 1:
                            mechPartType[i].part[c].assetID = UTRigAssembler.AssetNameToIDUpperTorso(mechPartType[i].part[c].asset.name);
                            break;
                        case 2:
                            mechPartType[i].part[c].assetID = LTRigAssembler.AssetNameToIDLowerTorso(mechPartType[i].part[c].asset.name);
                            break;
                        case 3:
                            mechPartType[i].part[c].assetID = LTRigAssembler.AssetNameToIDLegs(mechPartType[i].part[c].asset.name);
                            break;
                        case 4:
                            mechPartType[i].part[c].assetID = UTRigAssembler.AssetNameToIDArms(mechPartType[i].part[c].asset.name);
                            break;
                    }
                }

            }
        }
    }



    public static int CalculateTotalMovement(int totalWeight, int bonusMovement)
    {
        return maxMovement[totalWeight] + bonusMovement;
    }

    public static int CalculateTotalVision(int bonus)
    {
        return baseVision + bonus;
    }



    /// <summary>
    /// Determines if specified part can be equipped on the current part, if you input incorrect types, for example head to legs, you will get errors.
    /// </summary>
    /// <returns><c>true</c> if can equip this part the specified partType partID targetType targetID; otherwise, <c>false</c>.</returns>
    /// <param name="partType">Current paty type.</param>
    /// <param name="partID">the ID of the current part.</param>
    /// <param name="targetType">part type of the target.</param>
    /// <param name="targetID">the ID of the target part.</param>
    public static bool CanEquipThisPart(PartType partType, int partID, PartType targetType, int targetID)
    {
        int targetWeightClass = (int)Instance.mechPartType[(int)targetType].part[targetID].weightClass;

        bool passedTypeValidation;
        if (partType == PartType.Legs && targetType == PartType.LowerTorso)
            passedTypeValidation = true;   
        else if (partType == PartType.LowerTorso && targetType == PartType.UpperTorso)
            passedTypeValidation = true; 
        else if (partType == PartType.UpperTorso && targetType == PartType.Head)
            passedTypeValidation = true;
        else if (partType == PartType.UpperTorso && targetType == PartType.Arms)
            passedTypeValidation = true;
        else
            passedTypeValidation = false;

        if (passedTypeValidation)
            return GetWeightClassLimit((int)partType, partID, targetWeightClass);
        else
        {
            Debug.LogError(partType + " cannot be attached to " + targetType);
            return false;
        }
    }

    static bool GetWeightClassLimit(int partType, int partID, int weightClass)
    {
        return Instance.mechPartType[partType].part[partID].weightLimit[weightClass];
    }

    /// <summary>
    /// Calculates the total AP using the part ID's.
    /// </summary>
    /// <returns>The total A.</returns>
    /// <param name="headID">Head ID.</param>
    /// <param name="upperTorsoID">Upper torso ID.</param>
    /// <param name="lowerTorsoID">Lower torso ID.</param>
    /// <param name="legsID">Legs ID.</param>
    /// <param name="armsID">Arms ID.</param>
    public static int CalculateTotalAP(int headID, int upperTorsoID, int lowerTorsoID, int legsID, int armsID)
    {
        int total = 0;
        int[] array = new int[5] { headID, upperTorsoID, lowerTorsoID, legsID, armsID };
        for (int i = 0; i < array.Length; i++)
        {
            total += Instance.mechPartType[i].part[array[i]].ap;
        }

        return total;
    }
    /// <summary>
    /// Calculates the total armor using the part ID's.
    /// </summary>
    /// <returns>The total armor.</returns>
    /// <param name="headID">Head ID</param>
    /// <param name="upperTorsoID">Upper torso ID</param>
    /// <param name="lowerTorsoID">Lower torso ID</param>
    /// <param name="legsID">Legs ID</param>
    /// <param name="armsID">Arms ID</param>
    public static int CalculateTotalArmor(int headID, int upperTorsoID, int lowerTorsoID, int legsID, int armsID)
    {
        int total = 0;
        int[] array = new int[5] { headID, upperTorsoID, lowerTorsoID, legsID, armsID };
        for(int i = 0; i < array.Length; i++)
        {
            total += Instance.mechPartType[i].part[array[i]].armor;
        }

        return total;
    }

    /// <summary>
    /// Calculates the total armor using Mech Data provided
    /// </summary>
    /// <returns>The total armor.</returns>
    /// <param name="mechData">Mech data.</param>
    public static int CalculateTotalArmor(MechData mechData)
    {
        int total = 0;
        int[] array = new int[5] { mechData.headID, mechData.upperTorsoID, mechData.lowerTorsoID, mechData.legsID, mechData.armsID };
        for (int i = 0; i < array.Length; i++)
        {
            total += Instance.mechPartType[i].part[array[i]].armor;
        }

        return total;
    }

    /// <summary>
    /// Calculates the total bonus.
    /// </summary>
    /// <returns>The total bonus.</returns>
    /// <param name="headID">Head ID.</param>
    /// <param name="upperTorsoID">Upper torso ID.</param>
    /// <param name="lowerTorsoID">Lower torso ID.</param>
    /// <param name="legsID">Legs ID.</param>
    /// <param name="armsID">Arms ID.</param>
    public static PassiveBonus CalculateTotalBonus(int headID, int upperTorsoID, int lowerTorsoID, int legsID, int armsID)
    {
        PassiveBonus total = new PassiveBonus(true);
        int[] array = new int[5] { headID, upperTorsoID, lowerTorsoID, legsID, armsID };
        for (int i = 0; i < array.Length; i++)
        {
            if (Instance.mechPartType[i].part[array[i]].passiveBonus.enabled)
            {
                total.bonusMovement += Instance.mechPartType[i].part[array[i]].passiveBonus.bonusMovement;
                total.bonusVision += Instance.mechPartType[i].part[array[i]].passiveBonus.bonusVision;
                for (int c = 0; c < 8; c++)
                {
                    total.bonusDamage[c] += Instance.mechPartType[i].part[array[i]].passiveBonus.bonusDamage[c];
                    total.bonusAccuracy[c] += Instance.mechPartType[i].part[array[i]].passiveBonus.bonusAccuracy[c];
                }

            }
        }
        return total;
    }

    /// <summary>
    /// Calculates the total weight of the mech using the given ID's. This does not include weapons
    /// </summary>
    /// <returns>The total weight.</returns>
    /// <param name="headID">Head ID</param>
    /// <param name="upperTorsoID">Upper torso ID</param>
    /// <param name="lowerTorsoID">Lower torso ID</param>
    /// <param name="legsID">Legs ID</param>
    /// <param name="armsID">Arms ID</param>
    public static int CalculateTotalWeight(int headID, int upperTorsoID, int lowerTorsoID, int legsID, int armsID)
    {
        int total = 0;
        int[] array = new int[5] { headID, upperTorsoID, lowerTorsoID, legsID, armsID };
        for(int i = 0; i < array.Length; i++)
        {
            total += (int)Instance.mechPartType[i].part[array[i]].weightClass + 1;
        }

        return total;
    }

    /// <summary>
    /// Calculates the total weight, including weapons.
    /// </summary>
    /// <returns>The total weight.</returns>
    /// <param name="mechData">Mech data which is used to calculate the weight.</param>
    public static int CalculateTotalWeight(MechData mechData)
    {
        int total = CalculateTotalWeight(mechData.headID, mechData.upperTorsoID, mechData.lowerTorsoID, mechData.legsID, mechData.armsID);

        Weapon[] weapons = new Weapon[4];
        bool[] hasWeapon = new bool[4];

        if (mechData.hasWeaponL)
        {
            hasWeapon[0] = true;
            weapons[0] = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmLType].weapons[mechData.weaponArmLID];
        }
        if (mechData.hasWeaponR)
        {
            hasWeapon[1] = true;
            weapons[1] = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmRType].weapons[mechData.weaponArmRID];
        }
        if (mechData.hasWeaponGimbalL)
        {
            hasWeapon[2] = true;
            weapons[2] = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponGimbalLType].weapons[mechData.weaponGimbalLID];
        }
        if (mechData.hasWeaponGimbalR)
        {
            hasWeapon[3] = true;
            weapons[3] = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponGimbalRType].weapons[mechData.weaponGimbalRID];
        }

        for (int i = 0; i < 4; i++)
        {
            if (hasWeapon[i])
            {
                total += (int)weapons[i].weightClass + 1;
            }
        }

        return total;
    }

}

[System.Serializable]
public struct MechPartType
{
    [SerializeField]
    public MechPart[] part;
}

[System.Serializable]
public struct MechPart
{
	[SerializeField]
	public WeightClass weightClass;
	[SerializeField]
	public int ap;
	[SerializeField]
	public int armor;
	[SerializeField]
    public bool hasSkill;
	[SerializeField]
	public int skillID;

	[SerializeField]
	public int assetID;
	[SerializeField]
	public GameObject asset;

    [SerializeField]
    public string name;

    public bool[] weightLimit;

    public PassiveBonus passiveBonus;

    public MechPart(PartType pt)
    {
        name = "New " + pt.ToString();
        weightClass = WeightClass.Light;
        ap = 0;
        armor = 0;
        hasSkill = false;
        skillID = 0;
        assetID = 0;
        asset = null;
        passiveBonus = new PassiveBonus(false);
        weightLimit = new bool[3];
    }
}

[System.Serializable]
public struct PassiveBonus
{
    [SerializeField]
    public bool enabled;
    [SerializeField]
    public int bonusVision;
    [SerializeField]
    public int bonusMovement;
    [SerializeField]
    public bool separateDamage;
    [SerializeField]
    public int[] bonusDamage;
    [SerializeField]
    public bool separateAccuracy;
    [SerializeField]
    public float[] bonusAccuracy;

    public PassiveBonus(bool enable)
    {
        enabled = enable;
        bonusVision = 0;
        bonusMovement = 0;
        separateAccuracy = false;
        separateDamage = false;
        bonusDamage = new int[8];
        bonusAccuracy = new float[8];
    }
}
