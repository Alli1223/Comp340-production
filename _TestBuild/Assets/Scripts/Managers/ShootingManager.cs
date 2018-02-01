using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour {

	public static ShootingManager instance;
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
	}
	// Variables for enemies 
	private GameObject[] enemies;
	public EnemyData curEnemyData;
	// bools that state the current state of the targeting
	private bool displaySelection;
	private bool targeted;
	// int for the currently firing player
	public int curPlayer;

	//variables that reference the weapon that the player is using and how it fires
	Weapon curWeap;
	PlayerWeaponManager playerWeapManager;
	WeaponFiringTypes weapFireTypes;


	void Start () {
		enemies = GameObject.FindGameObjectsWithTag ("enemy");
		weapFireTypes = WeaponFiringTypes.instance;
	}

	void Update () 
	{
		// temp code to control when the player can select enemies inorder to fire upon them
		if(displaySelection == true){
			if (targeted == false) {
				TargetHighlight ();
			}
			if (Input.GetMouseButtonDown (0)) {
				// raycast from the mouse to check if the player has selected an enemy, will be
				// redone with the grid system once combined
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if(Physics.Raycast(ray, out hit, 100)){
					if(hit.transform.GetComponent<EnemyData>().targetable == true){
						curEnemyData = hit.transform.GetComponent<EnemyData> ();
						Fire ();
					}
				}
			}
		}


	}

	public void StartShooting()
	{
		//Temporary. Gets the players current weapon 
		playerWeapManager = GetComponent<PlayerWeaponManager>();
		curWeap = playerWeapManager.rightArmWeapon;
		StartTargeting ();
	}

	void StartTargeting()
	{//goes through the enemies in the scene and checks distance, if in range then they are set as targetable
		foreach (GameObject n in enemies) {
			if (Vector3.Distance (gameObject.transform.position, n.transform.position) <= curWeap.weaponRange){
				curEnemyData = n.GetComponent<EnemyData> ();
				curEnemyData.targetable = true;
			}
		}
		displaySelection = true;
	}

	void TargetHighlight()
	{// highlights the enemies within range as cyan, will be redone latter in dev
		foreach (GameObject m in enemies) {
			if (m.GetComponent<EnemyData> ().targetable == true) {
				Renderer rend = m.GetComponent<Renderer> ();
				rend.material.SetColor ("_Color", Color.cyan);
				targeted = true;
			}
		}
	}

	void Fire()
	{// fires the current weapon at the enemy
		weapFireTypes.FireWeapon (curWeap.weaponMinDamage,curWeap.weaponMaxDamage,curWeap.weaponAccuracy);
	}

}
  