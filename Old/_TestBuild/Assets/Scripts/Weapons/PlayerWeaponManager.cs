using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour {
	//References the weapon manager instance and provides the references for the right arm weapon
	WeaponManager weaponManager;
	public int rightWeapID;
	public Weapon rightArmWeapon;

	void Start() 
	{
		weaponManager = WeaponManager.instance;
		for (int i = 0; i < weaponManager.weapons.Count; i++) {
			if (rightWeapID == weaponManager.weapons [i].weaponIDnumber) {
				rightArmWeapon = weaponManager.weapons [i];
			}
		}
	}
}
