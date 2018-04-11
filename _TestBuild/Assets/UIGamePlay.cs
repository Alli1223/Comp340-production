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

    public void SelectWeapon(int id)
    {
        //ManagersManager.manager.tPlayer
    }
        
}
