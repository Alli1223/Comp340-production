using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextDatabank : MonoBehaviour 
{

	public static TextDatabank instance;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy (this);
	}

	[HideInInspector]
	public List<string> loreText;

	[HideInInspector]
	public TextAsset FirstLevelDialogue;

	public TextAsset DescriptionFile;
	public List<string> weaponDescriptionTxt;


	public int curSelection;
	
	void Start()
	{
		loreText = new List<string>();
		weaponDescriptionTxt = new List<string> ();
		FirstLevelDialogue = Resources.Load ("Text/FirstMissionDialogue") as TextAsset;
		DescriptionFile = Resources.Load ("Text/WeaponDescriptions") as TextAsset;
		//AddText();
		CutDescriptions ();
	}

	void AddText()
	{
		string entireText = FirstLevelDialogue.text;
		string[] individualLines;
		individualLines = entireText.Split('\n');
		loreText.AddRange(individualLines);
	}

	void CutDescriptions()
	{
		string allText = DescriptionFile.text;
		string[] chunks;
		chunks = allText.Split ('\n');
		weaponDescriptionTxt.AddRange (chunks);
	}
}
