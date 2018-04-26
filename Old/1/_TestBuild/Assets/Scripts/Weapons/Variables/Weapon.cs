using UnityEngine;
using System;

// Made the class serializable so we can save it in the data base.
public class Weapon 
{
	public string weaponName;
	public WeaponType weaponType;
	public int weaponMinDamage;
    public int weaponMaxDamage;
	public int weaponRange;
	public float weaponAccuracy;
	public int weaponAPCost;
	public int weaponWeight;
	public int weaponIDnumber;

	public int weaponSplash;
	public int weaponScatter;
	public int weaponFalloff;
    public enum WeaponType : int
	{
		Autocannon,
		Chaingun,
		Mortar, 
		Missilesilo,
		Rocketsilo,
		Scattercannon,
		Snipercannon,
		Snubcannon
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Weapon"/> class.
	/// Use for Autocannon, Chaingun, Snipercannon, Snubcannon and none splash Rocketsilos.
	/// </summary>
	/// <param name="Name">Name of weapon.</param>
	/// <param name="Type">Type of weapon.</param>
	/// <param name="MinDamage">Minimum damage done by weapon.</param>
	/// <param name="MaxDamage">Maximum damage done by weapon.</param>
	/// <param name="Range">Firing range of weapn.</param>
	/// <param name="Accuracy">Baseline accuracy.</param>
	/// <param name="APCost">AP cost to fire of weapon.</param>
	/// <param name="Weight">Weight of weapon.</param>
	/// <param name="ID">ID number of weapon for sorting.</param>
	public Weapon(string Name,WeaponType Type,int MinDamage, int MaxDamage,int Range,float Accuracy,int APCost,int Weight, int ID)
	{
		weaponName = Name;
		weaponType = Type;
		weaponMinDamage = MinDamage;
		weaponMaxDamage = MaxDamage;
		weaponRange = Range;
		weaponAccuracy = Accuracy;
		weaponAPCost = APCost;
		weaponWeight = Weight;
		weaponIDnumber = ID;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Weapon"/> class.
	/// Use for Missilesilos and Rocketsilos with Splash.
	/// </summary>
	/// <param name="Name">Name of weapon.</param>
	/// <param name="Type">Type of weapon.</param>
	/// <param name="MinDamage">Minimum damage done by weapon.</param>
	//// <param name="MaxDamage">Maximum damage done by weapon.</param>
	/// <param name="Range">Firing range of weapn.</param>
	/// <param name="Accuracy">Baseline accuracy.</param>
	/// <param name="APCost">AP cost to fire of weapon.</param>
	/// <param name="Weight">Weight of weapon.</param>
	/// <param name="Splash">Splash radius of weapon.</param>	
	/// <param name="ID">ID number of weapon for sorting.</param>
	public Weapon(string Name,WeaponType Type,int MinDamage, int MaxDamage,int Range,float Accuracy,int APCost,int Weight, int Splash, int ID)
	{
		weaponName = Name;
		weaponType = Type;
		weaponMinDamage = MinDamage;
		weaponMaxDamage = MaxDamage;
		weaponRange = Range;
		weaponAccuracy = Accuracy;
		weaponAPCost = APCost;
		weaponWeight = Weight;
		weaponSplash = Splash;
		weaponIDnumber = ID;

	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Weapon"/> class.
	/// Use for Mortars.
	/// </summary>
	/// <param name="Name">Name of weapon.</param>
	/// <param name="Type">Type of weapon.</param>
	/// <param name="MinDamage">Minimum damage done by weapon.</param>	
	/// <param name="MaxDamage">Maximum damage done by weapon.</param>
	/// <param name="Range">Firing range of weapn.</param>
	/// <param name="Accuracy">Baseline accuracy.</param>
	/// <param name="APCost">AP cost to fire of weapon.</param>
	/// <param name="Weight">Weight of weapon.</param>
	/// <param name="Splash">Splash radius of weapon.</param>
	/// <param name="Scatter">Possible distance scattered by weapon.</param>
	/// <param name="ID">ID number of weapon for sorting.</param>
	public Weapon(string Name,WeaponType Type,int MinDamage, int MaxDamage,int Range,float Accuracy,int APCost,int Weight, int Splash, int Scatter, int ID)
	{
		weaponName = Name;
		weaponType = Type;
		weaponMinDamage = MinDamage;		
		weaponMaxDamage = MaxDamage;
		weaponRange = Range;
		weaponAccuracy = Accuracy;
		weaponAPCost = APCost;
		weaponWeight = Weight;
		weaponSplash = Splash;
		weaponScatter = Scatter;
		weaponIDnumber = ID;

	}

}

// Added for later
public enum WeightClass : int {Light, Medium, Heavy};


