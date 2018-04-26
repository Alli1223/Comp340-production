using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour 
{

    PlayerData currentMech;

    public Text hpText;
    public Text apText;

    public GameObject weaponPanel;

    public Text textWeaponName;
    public Text textRange;
    public Text textDamage;
    public Text textAPCost;
    public Text textAccuracy;
    public Text textAccuracyInput;

	void OnEnable () 
    {
        ManagersManager.manager.tPlayer.gamePlayUI = this;
        weaponPanel.SetActive(false);
	}
	
    public void SetMech(PlayerData playerData)
    {
        currentMech = playerData;
        UpdateStatPanel();
    }

    public void UpdateStatPanel()
    {
        hpText.text = string.Format("{0} / {1}", currentMech.curHealth, currentMech.maxHealth);
        apText.text = string.Format("{0} / {1}", currentMech.curAP, currentMech.maxAP);
    }

    public void ShowWeaponStats(int wepID)
    {
        Weapon currentWeapon = currentMech.GetWeapon(wepID);
        bool weaponHasAccuracy = currentMech.WeaponTargetsDirectly(wepID);

        textWeaponName.text = currentWeapon.name;

        if (currentWeapon.minRange > 0)
        {
            textRange.text = string.Format("{0} - {1}", currentWeapon.minRange, currentWeapon.maxRange);
        }
        else
            textRange.text = currentWeapon.maxRange.ToString();

        textDamage.text = string.Format("{0} - {1}", currentWeapon.minDamage, currentWeapon.maxRange);

        textAPCost.text = currentWeapon.apCost.ToString();

        if (weaponHasAccuracy)
        {
            textAccuracy.text = "Accuracy:";
            textAccuracyInput.text = string.Format("{0}%", currentWeapon.accuracy);
        }
        else
        {
            textAccuracy.text = "Scatter:";
            textAccuracyInput.text = currentWeapon.scatter.ToString() + " Tiles";
        }

        weaponPanel.SetActive(true);
    }

    public void HideWeaponStats()
    {
        weaponPanel.SetActive(false);
    }
        
}
