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
	ManagersManager manager;
	Destruction destructionSystem;
	void Start()
	{
		manager = ManagersManager.manager;
		shootingManager = ShootingManager.instance;
		destructionSystem = Destruction.instance;
	}
	public GameObject projectileEffect;
	Transform curEnemyPos;
	Vector3 enemyPos;

	/// <summary>
	/// Fires standard variety weapons E.G. Autocannons, Snub Cannons, Sniper Cannons etc.
	/// </summary>
	public virtual void FireWeapon(int minDam,int maxDam, float accuracy)
	{
		curEnemyPos = shootingManager.curEnemyData.GetComponentInParent<Transform> ();
		enemyPos = new Vector3 (curEnemyPos.transform.position.x, curEnemyPos.transform.position.y, curEnemyPos.transform.position.z);
		Instantiate (projectileEffect,enemyPos,Quaternion.identity) ;

		//start the firing animation and the audio here
		//spawn projectiles and fire them, play impact sounds afterwards
		//stop
		if (Random.Range (0, 100) <= accuracy) {
			shootingManager.curEnemyData.health -= Random.Range (minDam, maxDam);
				if(shootingManager.curEnemyData.health <= 0)
				{
				//manager.tPlayer.secondPlayers.Remove (shootingManager.curEnemyData.transform.gameObject);
				shootingManager.enemies.Remove (curEnemyPos.gameObject);
				shootingManager.curEnemyData.Death();
				}
		}else{
			print("miss");
		}
		shootingManager.curEnemyData = null;
	}

	/// <summary>
	/// Fires the current weapon of the mech at the targeted building.
	/// </summary>
	/// <param name="minDam">Minimum dam.</param>
	/// <param name="maxDam">Max dam.</param>
	/// <param name="building">Building.</param>
	/// <param name="curPlayer">Current player.</param>
	public virtual void FireWeapon(int minDam,int maxDam, GameObject building, GameObject curPlayer)
	{
		shootingManager.curBuildData.health -= Random.Range (minDam, maxDam);
		if (shootingManager.curBuildData.health <= 0) {
			destructionSystem.DestructionType (curPlayer, building);
		}
	}


}
