using UnityEngine;
using System.Collections;

public class CodexSwitch : MonoBehaviour 
{	
	public int selection;
	public GameObject entryScreen;
	TextDatabank textDatabank;
	AccessDatabank accessDataBank;

	void Start()
	{
		textDatabank = GetComponentInParent<TextDatabank>();
		accessDataBank = GetComponentInParent<AccessDatabank>();
	}


	public void SelectEntry()
	{
		entryScreen.SetActive (true);
		textDatabank.curSelection = selection;
		accessDataBank.IncrementText();
	}
}
