using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextDatabank : MonoBehaviour 
{
	[HideInInspector]
	public List<string> loreText;

	[HideInInspector]
	public TextAsset FirstLevelDialogue;


	public int curSelection;
	
	void Start()
	{
		loreText = new List<string>();
		FirstLevelDialogue = Resources.Load ("FirstMissionDialogue") as TextAsset;
		AddText();
	}

	void AddText()
	{
		string entireText = FirstLevelDialogue.text;
		string[] individualLines;
		individualLines = entireText.Split('\n');
		loreText.AddRange(individualLines);

	}
}
