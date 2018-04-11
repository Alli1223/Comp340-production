using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFiringTypes : MonoBehaviour {

	public static WeaponFiringTypes instance;
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
	}

	ShootingManager shootingManager;
	void Start()
	{
		shootingManager = ShootingManager.instance;
	}

	/// <summary>
	/// Fires standard variety weapons E.G. Autocannons, Snub Cannons, Sniper Cannons etc.
	/// </summary>
	public virtual void FireWeapon(int minDam,int maxDam, float accuracy)
	{
		if (Random.Range (0, 100) <= accuracy) {
			shootingManager.curEnemyData.health -= Random.Range (minDam, maxDam);
				if(shootingManager.curEnemyData.health <= 0)
				{
				shootingManager.curEnemyData.Death();
				}
		}else{
			print("miss");
		}
	}


}
