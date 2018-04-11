using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EditorDataBaseLayoutData : ScriptableObject 
{
    [SerializeField]
    public List<List<bool>> foldOutInfoWeaponData = new List<List<bool>>();

    [SerializeField]
    public List<List<bool>> foldOutInfoPartsData = new List<List<bool>>();


    public void Initialise(WeaponDataBase myData)
    {

        for (int i = 0; i < 8; i++)
        {
            foldOutInfoWeaponData.Add(new List<bool>(myData.weaponTypeArray[i].weapons.Length));
            for (int c = 0; c < myData.weaponTypeArray[i].weapons.Length; c++)
            {
                foldOutInfoWeaponData[i].Add(false);
            }
        }
    }

    public void Initialise(PartsDataBase myData)
    {
        for (int i = 0; i < myData.mechPartType.Length; i++)
        {
            foldOutInfoPartsData.Add(new List<bool>(myData.mechPartType[i].part.Length));
            for (int c = 0; c < myData.mechPartType[i].part.Length; c++)
            {
                foldOutInfoPartsData[i].Add(false);
            }
        }
    }
}
