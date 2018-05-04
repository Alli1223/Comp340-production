using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : PartWeightManager{

	public string legName;
	public int legArmour;
	public List<WeightClasses> legWeight;
	public List<WeightClasses> legLowerTorsoWeight;
	public int legAP;
	public int legBonusAcc;

	public Legs(string Name, int Armour, int PartWeight, int LowerTorsoWeight, int LegAP)
	{
		legName = Name;
		legArmour = Armour;
		CalSelfWeight (PartWeight);
		legWeight = pW;
		CalLowerTorsoWeight (LowerTorsoWeight);
		legLowerTorsoWeight = pLTAW;
		legAP = LegAP;
	}
}