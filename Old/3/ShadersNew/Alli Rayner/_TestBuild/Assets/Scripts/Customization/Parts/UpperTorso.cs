using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class UpperTorso : PartWeightManager {

	public string upperTorsoName;
	public int upperTorsoArmour;
	public List<WeightClasses> upperTorsoWeight;
	public List<WeightClasses> upperTorsoHeadWeight;
	public List<WeightClasses> upperTorsoWeaponWeight;
	public List<WeightClasses> upperTorsoLowerTorsoWeight;
	public int upperTorsoAP;
	public int upperTorsoBonusAcc;
	public int upperTorsoBonusDamage;
	public int upperTorsoBonusDamageType;
	void Start()
	{
	}


	/// <summary>
	/// Initializes a new baseline instance of the <see cref="UpperTorso"/> class with no bonuses.
	/// </summary>
	/// <param name="Name">Name.</param>
	/// <param name="Armour">Armour.</param>
	/// <param name="PartWeight">Part weight.</param>
	/// <param name="HeadWeight">Head weight.</param>
	/// <param name="WeaponWeight">Weapon weight.</param>
	/// <param name="LowerTorsoWeight">Lower torso weight.</param>
	/// <param name="PartAP">Part A.</param>
	public UpperTorso(string Name, int Armour, int PartWeight, int HeadWeight, int WeaponWeight, int LowerTorsoWeight, int PartAP)
	{
		upperTorsoName = Name;
		upperTorsoArmour = Armour;
		CalSelfWeight(PartWeight);
		upperTorsoWeight = pW;
		CalHeadWeight(HeadWeight);
		upperTorsoHeadWeight = pHAW;
		CalWeaponWeight(WeaponWeight);
		upperTorsoWeaponWeight = pWAW;
		CalLowerTorsoWeight(LowerTorsoWeight);
		upperTorsoLowerTorsoWeight = pLTAW;
	}



}
