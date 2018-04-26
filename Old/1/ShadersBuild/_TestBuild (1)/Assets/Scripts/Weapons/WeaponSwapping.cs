	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapping : MonoBehaviour {

	PlayerWeaponManager playerWeaponManager;
//	public int weapIDR;
//	public int weapIDL;
//	public int weapIDB;
//	
	
	void Start()
	{
		playerWeaponManager = PlayerWeaponManager.instance;
	}
	
	public void SwapWeapon(int ID)
	{
//		if (ID == playerWeaponManager.rightWeapID)
//		{
//			playerWeaponManager.curWeapon = playerWeaponManager.rightArmWeapon;
//		}
//		else if (ID == playerWeaponManager.leftWeapID)
//		{
//			playerWeaponManager.curWeapon = playerWeaponManager.leftArmWeapon;
//		}
//		else if (ID == playerWeaponManager.backWeapID)
//		{
//			playerWeaponManager.curWeapon = playerWeaponManager.backWeapon;  
//		}
		if (ID == 0)
		{
			playerWeaponManager.curWeapon = playerWeaponManager.rightArmWeapon;
			Debug.Log(playerWeaponManager.curWeapon == null);
		}
		else if (ID == 1)
		{
			playerWeaponManager.curWeapon = playerWeaponManager.leftArmWeapon;
		}
		else if (ID == 2)
		{
			playerWeaponManager.curWeapon = playerWeaponManager.backWeapon;  
		}
	}
}
