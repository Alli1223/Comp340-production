using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class TestPart : PartWeightManager
{

    public string testPartName;
    public int testPartArmour;
	public List<WeightClasses> testPartWeight;
    public List<WeightClasses> testPartHeadWeight;
    public List<WeightClasses> testPartWeaponWeight;
    public List<WeightClasses> testPartLowerTorsoWeight;
    public int testPartAP;
    public int testPartBonusAcc;
    public int testPartBonusDamage;
    public int testPartBonusDamageType;

    private int wCL;

  

    void Start()
    {

        //		new UpperTorso(test(1,2)
        wCL = Enum.GetNames(typeof(WeightClasses)).Length;
        //new UpperTorso ("test", 1, WeightClasses.Light, , test (1, 2,null), test (1, 2,null), 4, 4);
    }

	public TestPart(string Name, int Armour, int PartWeight, int HeadWeight, int WeaponWeight, int LowerTorsoWeight, int PartAP)
    {
        testPartName = Name;
		testPartArmour = Armour;
		CalSelfWeight(PartWeight);
		testPartWeight = pW;
		CalHeadWeight(HeadWeight);
		testPartHeadWeight = pHAW;
		CalWeaponWeight(WeaponWeight);
		testPartWeaponWeight = pWAW;
		CalLowerTorsoWeight(LowerTorsoWeight);
		testPartLowerTorsoWeight = pLTAW;


	}
    
	
	
	
	
    Enum[] test(int a, int? b, int? c)
    {
        WeightClasses aA;
        WeightClasses bB;
        WeightClasses cC;


		if (a <= wCL && b == null && c == null)
		{
			aA = (WeightClasses)a;

			Enum[] testA = new Enum[0];
			testA[0] = aA;
			return testA;
		}
		else if (a <= wCL && b <= wCL && c == null)
		{
			aA = (WeightClasses)a;
			bB = (WeightClasses)b;

			Enum[] testB = new Enum[1];
			testB[0] = aA;
			testB[1] = bB;
			return testB;
		}
		else if (a <= wCL && b == null && c == null)
		{
			aA = (WeightClasses)a;
			bB = (WeightClasses)b;
			cC = (WeightClasses)c;

			Enum[] testC = new Enum[2];
			testC[0] = aA;
			testC[1] = bB;
			testC[2] = cC;
			return testC;
		}
		else
		
           // print("broke");
        return null;

    }

}