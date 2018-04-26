using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerTorso : PartWeightManager {
	

	public string lowerTorsoName;
	public int lowerTorsoArmour;
	public List<WeightClasses> lowerTorsoWeight;
	public List<WeightClasses> lowerTorsoLegWeight;
	public List<WeightClasses> lowerTorsoUpperTorsoWeight;
	public int lowerTorsoAP;
	public int lowerTorsoBonusAcc;



	/// <summary>
	/// Initializes a new instance of the <see cref="LowerTorso"/> class with no bonus.
	/// </summary>
	/// <param name="Name">Part Name.</param>
	/// <param name="Armour">Part armour value.</param>
	/// <param name="PartWeight">Part weight: 0 = light 1 = medium, 2= heavy.</param>
	/// <param name="LegWeight">Accepted leg weight categories: 0 = light 1 = medium, 2= heavy, 3 = light/medium, 4 = light/heavy, 5 = medium/heavy, 6 = light/medium/heavy.</param>
	/// <param name="LowerTorsoWeight">Accepted lower torso weight categories: 0 = light 1 = medium, 2= heavy, 3 = light/medium, 4 = light/heavy, 5 = medium/heavy, 6 = light/medium/heavy.</param>
	/// <param name="PartAP">Part AP value.</param>
	public LowerTorso(string Name, int Armour, int PartWeight,  int LegWeight, int LowerTorsoWeight, int PartAP)
	{
		lowerTorsoName = Name;
		lowerTorsoArmour = Armour;
		CalSelfWeight(PartWeight);
		lowerTorsoWeight = pW;
		CalLegWeight (LegWeight);
		lowerTorsoLegWeight = pLAW;
		CalUpperTorsoWeight (LowerTorsoWeight);
		lowerTorsoUpperTorsoWeight = pUTAW;
		lowerTorsoAP = PartAP;
	}
}
