using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartManager : MonoBehaviour {

	public static PartManager instance ;
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
		//Temp activation to add all weapons to the manager inorder to reference them
		AddParts ();
	}

	// The list of weapons that can be access for equiping
	public List <Head> headParts = new List<Head>();
	public List <UpperTorso> upperTorsoParts = new List<UpperTorso>();
	public List <LowerTorso> lowerTorsoParts = new List<LowerTorso>();
	public List <Legs> legParts = new List<Legs>();

	public void AddParts()
	{
		//List of head parts;
		headParts.Add(new Head("IC-U4",25,1,4,5));
		headParts.Add (new Head ("EICU-5", 19, 0, 3, 5));
		headParts.Add (new Head ("AOAC-4", 34, 2, 5, 6));
		//List of Uppertorso parts;
		upperTorsoParts.Add (new UpperTorso ("BLK-15B", 38, 1, 4,4,4, 15));
		upperTorsoParts.Add (new UpperTorso ("EIPU-5", 28, 0, 3,3,3, 13));
		upperTorsoParts.Add (new UpperTorso ("ACCU-4", 45, 2, 5, 5, 5, 20));
		//List of Lowertorso parts;
		lowerTorsoParts.Add (new LowerTorso ("GGF-2", 32, 1, 4, 4, 3));
		lowerTorsoParts.Add (new LowerTorso ("EIEU-5", 25, 0, 3, 3, 3));
		lowerTorsoParts.Add (new LowerTorso ("APU-4", 38, 2, 5, 5, 4));
		//List of Leg parts;x 
		legParts.Add (new Legs ("WL-54B", 25, 1, 4, 3));
		legParts.Add (new Legs ("EIATL-5", 20, 0, 3, 4));
		legParts.Add (new Legs ("AAL-4", 30, 2, 5, 4));
	}
}
