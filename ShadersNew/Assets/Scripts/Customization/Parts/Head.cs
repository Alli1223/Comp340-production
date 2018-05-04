using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : PartWeightManager {
	
	public string headName;
	public int headArmour;
	public List<WeightClasses> headWeight;
	public List<WeightClasses> headUpperTorsoWeight;
	public int headAP;
	public int headBonusAcc;

	public Head(string Name, int Armour, int PartWeight, int UpperTorsoWeight, int HeadAP)
	{
		headName = Name;
		headArmour = Armour;
		CalSelfWeight (PartWeight);
		headWeight = pW;
		CalUpperTorsoWeight (UpperTorsoWeight);
		headUpperTorsoWeight = pUTAW;
		headAP = HeadAP;
	}
}
