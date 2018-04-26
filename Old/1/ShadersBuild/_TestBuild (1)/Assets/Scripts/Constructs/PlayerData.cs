using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	//Player identifiers 
	public int playerNum;
	public int uniqueID;
	public string charName;

	//player baseline variables and current versions
	public int maxAP;
	public int maxHealth;
	public int maxMoveDist;
	public int curAP;
	public int curHealth;		
	public int curMoveDist;
	//used to show the range of tiles that the player can shoot in	
	public int curWeapRNG;
	public bool movementPending;

	//mech weapon variables - to be connected to the player weapon manager so that it uses this instead
	public Weapon rightArmWeapon;
	public Weapon leftArmWeapon;
	public Weapon backWeapon;
	//mech part variables - to be used to figure out the baseline stats of the mech and also to be linked to the model customization system
	public Head mechHead;
	public UpperTorso mechUpperTorso;
	public LowerTorso mechLowerTorso;
	public Legs mechLegs;
}
