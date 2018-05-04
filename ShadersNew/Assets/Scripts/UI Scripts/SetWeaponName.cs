using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetWeaponName : MonoBehaviour {

	PlayerWeaponManager weapManager;
	public int weapSlotSet = 0;
	public Text text;
	void Start () {
		weapManager = PlayerWeaponManager.instance;
		ChangeWeaponName ();
	}

	public void ChangeWeaponName()
	{
		if (weapSlotSet == 0) {
			text.text = weapManager.rightArmWeapon.weaponName;
		}
		if (weapSlotSet == 1) {
			gameObject.GetComponentInChildren<Text> ().text = weapManager.leftArmWeapon.weaponName;
		}
		if (weapSlotSet == 2) {
			gameObject.GetComponentInChildren<Text> ().text = weapManager.backWeapon.weaponName;
		}
	}


}
