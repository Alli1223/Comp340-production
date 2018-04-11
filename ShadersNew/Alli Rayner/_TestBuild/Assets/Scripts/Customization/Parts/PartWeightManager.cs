using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PartWeightManager {
 
	public enum WeightClasses
	{
		Light,
		Medium,
		Heavy
	}
	
	/*pW = Part Weight. pHAW = part Head Attachement Weight. pWAW = part Weapon Attachment Weight. 
	  pLTAW = part Lower Torso Attachment Weight. pUTAW = part Upper Torso Attachment Weight
	  pLAW = part Leg Attachment Weight. 
	*/
	public List<WeightClasses> pW = new List<WeightClasses>();

	public List<WeightClasses> pUTAW = new List<WeightClasses> ();
	public List<WeightClasses> pLAW = new List<WeightClasses> ();
	public List<WeightClasses> pHAW = new List<WeightClasses>();
	public List<WeightClasses> pWAW = new List<WeightClasses>();
	public List<WeightClasses> pLTAW = new List<WeightClasses>();
	
	public void CalSelfWeight(int input)
	{
		if (input == 0)
		{
			pW.Add(WeightClasses.Light);
		}else if (input == 1)
		{
			pW.Add(WeightClasses.Medium);
		}else if (input == 2)
		{
			pW.Add(WeightClasses.Heavy);
		}

	}
	public void CalHeadWeight(int input)
	{
		if (input == 0)
		{
			pHAW.Add(WeightClasses.Light);
		}else if (input == 1)
		{
			pHAW.Add(WeightClasses.Medium);
		}else if (input == 2)
		{
			pHAW.Add(WeightClasses.Heavy);
		}else if (input == 3)
		{
			pHAW.Add(WeightClasses.Light);
			pHAW.Add(WeightClasses.Medium);
		}else if (input == 4)
		{
			pHAW.Add(WeightClasses.Light);
			pHAW.Add(WeightClasses.Heavy);
		}else if (input == 5)
		{
			pHAW.Add(WeightClasses.Medium);
			pHAW.Add(WeightClasses.Heavy);
		}else if (input == 6)
		{
			pHAW.Add(WeightClasses.Light);
			pHAW.Add(WeightClasses.Medium);
			pHAW.Add(WeightClasses.Heavy);
		}

	}public void CalWeaponWeight(int input)
	{
		if (input == 0)
		{
			pWAW.Add(WeightClasses.Light);
		}else if (input == 1)
		{
			pWAW.Add(WeightClasses.Medium);
		}else if (input == 2)
		{
			pWAW.Add(WeightClasses.Heavy);
		}else if (input == 3)
		{
			pWAW.Add(WeightClasses.Light);
			pWAW.Add(WeightClasses.Medium);
		}else if (input == 4)
		{
			pWAW.Add(WeightClasses.Light);
			pWAW.Add(WeightClasses.Heavy);
		}else if (input == 5)
		{
			pWAW.Add(WeightClasses.Medium);
			pWAW.Add(WeightClasses.Heavy);
		}else if (input == 6)
		{
			pWAW.Add(WeightClasses.Light);
			pWAW.Add(WeightClasses.Medium);
			pWAW.Add(WeightClasses.Heavy);
		}

	}public void CalUpperTorsoWeight(int input)
	{
		if (input == 0)
		{
			pUTAW.Add(WeightClasses.Light);
		}else if (input == 1)
		{
			pUTAW.Add(WeightClasses.Medium);
		}else if (input == 2)
		{
			pUTAW.Add(WeightClasses.Heavy);
		}else if (input == 3)
		{
			pUTAW.Add(WeightClasses.Light);
			pUTAW.Add(WeightClasses.Medium);
		}else if (input == 4)
		{
			pUTAW.Add(WeightClasses.Light);
			pUTAW.Add(WeightClasses.Heavy);
		}else if (input == 5)
		{
			pUTAW.Add(WeightClasses.Medium);
			pWAW.Add(WeightClasses.Heavy);
		}else if (input == 6)
		{
			pUTAW.Add(WeightClasses.Light);
			pUTAW.Add(WeightClasses.Medium);
			pUTAW.Add(WeightClasses.Heavy);
		}

	}public void CalLowerTorsoWeight(int input)
	{
		if (input == 0)
		{
			pLTAW.Add(WeightClasses.Light);
		}else if (input == 1)
		{
			pLTAW.Add(WeightClasses.Medium);
		}else if (input == 2)
		{
			pLTAW.Add(WeightClasses.Heavy);
		}else if (input == 3)
		{
			pLTAW.Add(WeightClasses.Light);
			pLTAW.Add(WeightClasses.Medium);
		}else if (input == 4)
		{
			pLTAW.Add(WeightClasses.Light);
			pLTAW.Add(WeightClasses.Heavy);
		}else if (input == 5)
		{
			pLTAW.Add(WeightClasses.Medium);
			pLTAW.Add(WeightClasses.Heavy);
		}else if (input == 6)
		{
			pLTAW.Add(WeightClasses.Light);
			pLTAW.Add(WeightClasses.Medium);
			pLTAW.Add(WeightClasses.Heavy);
		}

	}public void CalLegWeight(int input)
	{
		if (input == 0)
		{
			pLAW.Add(WeightClasses.Light);
		}else if (input == 1)
		{
			pLAW.Add(WeightClasses.Medium);
		}else if (input == 2)
		{
			pLAW.Add(WeightClasses.Heavy);
		}else if (input == 3)
		{
			pLAW.Add(WeightClasses.Light);
			pLAW.Add(WeightClasses.Medium);
		}else if (input == 4)
		{
			pLAW.Add(WeightClasses.Light);
			pLAW.Add(WeightClasses.Heavy);
		}else if (input == 5)
		{
			pLAW.Add(WeightClasses.Medium);
			pLAW.Add(WeightClasses.Heavy);
		}else if (input == 6)
		{
			pLAW.Add(WeightClasses.Light);
			pLAW.Add(WeightClasses.Medium);
			pLAW.Add(WeightClasses.Heavy);
		}

	}
	
	
}
