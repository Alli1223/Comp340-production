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
		weapons.Add(new Weapon ("Ac10b-4", Weapon.WeaponType.Autocannon, 10, 30, 90000, 75, 5, 19, 1));
	}
}


