using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{

	public static ShootingManager instance;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
	}
	// Variables for enemies
	public List<GameObject> enemies = new List<GameObject> ();
	private GameObject[] buildings;
	public EnemyData curEnemyData;
	public BuildingData curBuildData;
	// bools that state the current state of the targeting
	private bool displaySelection;
	private bool targeted;
	// Gameobject for the currently firing player
	public GameObject curPlayer;
	private bool halfCover;
	public float usedAccuracy;

	//variables that reference the weapon that the player is using and how it fires
	Weapon curWeap;
	PlayerWeaponManager playerWeapManager;
	WeaponFiringTypes weapFireTypes;
	private ManagersManager tManage;
	private GameObject buildingData;


	void Start ()
	{
		enemies.AddRange (GameObject.FindGameObjectsWithTag ("Player"));
		weapFireTypes = WeaponFiringTypes.instance;
		tManage = ManagersManager.manager;
		buildings = GameObject.FindGameObjectsWithTag ("Building");
		//curWeap = playerWeapManager.curWeapon;
	}

	void Update ()
	{
		// temp code to control when the player can select enemies inorder to fire upon them
//		if(displaySelection == true){
//			if (targeted == false) {
//				TargetHighlight ();
//			}
		if (Input.GetMouseButtonDown (1) && tManage.tPlayer.shootingMode) {			
			StartShooting ();
			curWeap = playerWeapManager.curWeapon;
		//	Debug.Log (curWeap.weaponIDnumber);
			if (PlayerManager.currentPlayer.GetComponent<TempPlayerVar> ().currentAP >= curWeap.weaponAPCost) {
				// raycast from the mouse to check if the player has selected an enemy, will be
				// redone with the grid system once combined
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				
				StartTargeting ();
				if (Physics.Raycast (ray, out hit, 100)) {
					if (hit.transform.tag == "Player") {
						if (hit.transform.GetComponent<EnemyData> ().targetable == true) {
							if (LineOfSightFunctions._TileSight (GridPositionDetection.GetClosestGrid (PlayerManager.currentPlayer.transform.position, GridGeneration.gridSingle.currentTiles).position, GridPositionDetection.GetClosestGrid (hit.transform.position, GridGeneration.gridSingle.currentTiles).position) != cover.Full) {
								curEnemyData = hit.transform.GetComponent<EnemyData> ();
								if (LineOfSightFunctions._TileSight (GridPositionDetection.GetClosestGrid (PlayerManager.currentPlayer.transform.position, GridGeneration.gridSingle.currentTiles).position, GridPositionDetection.GetClosestGrid (hit.transform.position, GridGeneration.gridSingle.currentTiles).position) == cover.Half)
									halfCover = true;
								Fire (halfCover);
							}
						}
					} else if (hit.transform.tag == "Building") {
						if (hit.transform.GetComponent<BuildingData> ().targetable == true) {
							curBuildData = hit.transform.GetComponent<BuildingData> ();
							FireBuilding ();
						}
					} else {
						Debug.Log ("missclick");
					}
				}
			}
		}


	}

	public void StartShooting ()
	{
		//Temporary. Gets the players current weapon 
		playerWeapManager = GetComponent<PlayerWeaponManager> ();
		curWeap = playerWeapManager.curWeapon;
		//StartTargeting ();
	}

	void StartTargeting ()
	{//goes through the enemies in the scene and checks distance, if in range then they are set as targetable
		foreach (GameObject n in enemies) {
			if (Vector3.Distance (PlayerManager.currentPlayer.transform.position, n.transform.position) <= curWeap.weaponRange) {
				curEnemyData = n.GetComponent<EnemyData> ();
				curEnemyData.targetable = true;
			}
		}
		foreach (GameObject b in buildings) {
			if (Vector3.Distance (PlayerManager.currentPlayer.transform.position, b.transform.position) <= curWeap.weaponRange) {
				curBuildData = b.GetComponent<BuildingData> ();
				curBuildData.targetable = true;
			}
		}
		//displaySelection = true;
	}

	//	void TargetHighlight()
	//	{// highlights the enemies within range as cyan, will be redone latter in dev
	//		foreach (GameObject m in enemies) {
	//			if (m.GetComponent<EnemyData> ().targetable == true) {
	//				Renderer rend = m.GetComponent<Renderer> ();
	//				rend.material.SetColor ("_Color", Color.cyan);
	//				targeted = true;
	//			}
	//		}
	//	}

	void Fire (bool cover)
	{// fires the current weapon at the enemy
		if (cover == true)
			usedAccuracy = curWeap.weaponAccuracy / 2;
		weapFireTypes.FireWeapon (curWeap.weaponMinDamage, curWeap.weaponMaxDamage, usedAccuracy);
	}

	void FireBuilding ()
	{
		buildingData = curEnemyData.gameObject;
		curPlayer = PlayerManager.currentPlayer;
		weapFireTypes.FireWeapon (curWeap.weaponMinDamage, curWeap.weaponMaxDamage, buildingData, curPlayer);
	}
	

}
  