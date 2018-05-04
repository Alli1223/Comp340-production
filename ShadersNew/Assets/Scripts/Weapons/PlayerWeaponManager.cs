using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeaponManager : MonoBehaviour {
	
	public static PlayerWeaponManager instance;
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
	}
	//References the weapon manager instance and provides the references for the right arm weapon
	WeaponManager weaponManager;
	public int rightWeapID;
	public int leftWeapID;
	public int backWeapID;
	public Weapon curWeapon;
	public Weapon rightArmWeapon;
	public Weapon leftArmWeapon;
	public Weapon backWeapon;
	

	void Start() 
	{
		weaponManager = WeaponManager.instance;
		print(weaponManager.weapons.Count);
		for (int i = 0; i < weaponManager.weapons.Count; i++) {
			if (rightWeapID == weaponManager.weapons [i].weaponIDnumber) {
				rightArmWeapon = weaponManager.weapons [i];
				Debug.Log(rightArmWeapon.weaponIDnumber);
			}if (leftWeapID == weaponManager.weapons [i].weaponIDnumber) {
				leftArmWeapon = weaponManager.weapons [i];
				Debug.Log(leftArmWeapon.weaponIDnumber);
			}if (backWeapID == weaponManager.weapons [i].weaponIDnumber) {
				backWeapon = weaponManager.weapons [i];
				Debug.Log(backWeapon.weaponIDnumber);
			}
		}
		curWeapon = rightArmWeapon;
		//Debug.Log(rightArmWeapon.weaponIDnumber);
	//	Debug.Log(curWeapon);
	}
}
