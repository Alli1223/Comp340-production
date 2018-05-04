using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour 
{
	public static WeaponManager instance ;
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
		//Temp activation to add all weapons to the manager inorder to reference them
		AddWeapon ();
	}

	// The list of weapons that can be access for equiping
	public List <Weapon> weapons = new List<Weapon>();
	public void AddWeapon()
	{
		//Autocannons
		weapons.Add(new Weapon ("MK.II Autocannon",Weapon.WeaponType.Autocannon,12,26,1003,72,12,1,1001));
		weapons.Add(new Weapon("MK.I Autocannon", Weapon.WeaponType.Autocannon, 10, 20, 12, 72, 11, 0, 1002));
//		weapons.Add(new Weapon("Alexei Autocannon", Weapon.WeaponType.Autocannon, 14, 23, 12, 75, 13, 1, 003));
//		weapons.Add(new Weapon("HAMG", Weapon.WeaponType.Autocannon, 17, 28, 13, 78, 13, 2, 004));
		//Snipercannons	
		weapons.Add(new Weapon("SC-15B", Weapon.WeaponType.Snipercannon, 20, 45, 17, 85, 19, 2, 2001));
	//	weapons.Add(new Weapon("MK.II Snipercannon", Weapon.WeaponType.Snipercannon, 20, 30, 15, 85, 14, 1, 102));
		weapons.Add(new Weapon("Vasily Anti-materiel Cannon", Weapon.WeaponType.Snipercannon, 24, 30, 16, 80, 16, 1, 2002));
		//weapons.Add(new Weapon("MK.I Snipercannon", Weapon.WeaponType.Snipercannon, 15, 22, 17, 80, 15, 0, 104));
		//Scattercannons
	//	weapons.Add(new Weapon("The Diplomat", Weapon.WeaponType.Scattercannon, 15, 50, 5, 73, 14, 2, 201));
		weapons.Add(new Weapon("Scattershot Cannon", Weapon.WeaponType.Scattercannon, 15, 35, 6, 68, 14, 1, 3001));
		//weapons.Add(new Weapon("MK.II Scattercannon", Weapon.WeaponType.Scattercannon, 15, 24, 4, 90, 10, 1, 203));
		weapons.Add(new Weapon("MK.I Scattercannon", Weapon.WeaponType.Scattercannon, 15, 22, 5, 70, 10, 0, 3002));
		//Mortars
		weapons.Add(new Weapon("Downpour Launcher", Weapon.WeaponType.Mortar, 8, 20, 17, 0, 14, 1, 2, 3, 4001));
		weapons.Add(new Weapon("MK.I Mortar", Weapon.WeaponType.Mortar, 8, 20, 20, 0, 16, 2, 2, 3, 4002));
		//weapons.Add(new Weapon("MK.II Mortar", Weapon.WeaponType.Mortar, 10, 24, 17, 0, 20, 1, 0, 2, 403));
		//weapons.Add(new Weapon("NL-15A", Weapon.WeaponType.Mortar, 0, 0, 17, 0, 20, 1, 0, 1, 404));
		//Snubcannons
		weapons.Add(new Weapon("MK.II Snubcannon", Weapon.WeaponType.Snubcannon, 5, 15, 5, 75, 7, 0, 5001));
//		weapons.Add(new Weapon("Snubcannon", Weapon.WeaponType.Snubcannon, 8, 15, 5, 75, 8, 0, 502));
		weapons.Add(new Weapon("MK.I Snubcannon", Weapon.WeaponType.Snubcannon, 6, 14, 7, 70, 8, 0, 5002));
//		weapons.Add(new Weapon("SNC", Weapon.WeaponType.Snubcannon, 15, 24, 5, 75, 10, 2, 504));
		//Rocketsilos
		weapons.Add(new Weapon("MK.II Rocketsilo", Weapon.WeaponType.Rocketsilo, 15, 20, 8, 70, 18, 1, 2, 6001));
		weapons.Add(new Weapon("Progenitor Rocketsilo", Weapon.WeaponType.Rocketsilo, 18, 42, 8, 72, 18, 2, 2, 6002));
		//weapons.Add(new Weapon("MK.I Rocketsilo", Weapon.WeaponType.Rocketsilo, 12, 18, 10, 0, 20, 2, 1, 4, 603));
		//weapons.Add(new Weapon("SRSR-13", Weapon.WeaponType.Rocketsilo, 14, 18, 7, 75, 17, 1, 3, 604));
		//Missilesilos
		weapons.Add(new Weapon("CYRUS-6A", Weapon.WeaponType.Missilesilo, 20, 45, 25, 100, 25, 2, 4, 7001));
	//	weapons.Add(new Weapon("Aegis Missilesilo", Weapon.WeaponType.Missilesilo, 25, 30, 23, 100, 22, 2, 3, 702));
		weapons.Add(new Weapon("LMAP-5B", Weapon.WeaponType.Missilesilo, 17, 27, 20, 100, 17, 1, 7002));
		//Chainguns
//		weapons.Add(new Weapon("PDFAC5", Weapon.WeaponType.Chaingun, 9, 15, 8, 78, 9, 0, 801));
		weapons.Add(new Weapon("BOLT-2", Weapon.WeaponType.Chaingun, 11, 17, 7, 70, 10, 0, 8001));
		weapons.Add(new Weapon("VECTOR-103", Weapon.WeaponType.Chaingun, 11, 16, 7, 75, 11, 0, 8002));
//		weapons.Add(new Weapon("HCG-19", Weapon.WeaponType.Chaingun, 17, 22, 8, 78, 11, 1, 704));
	}
}


