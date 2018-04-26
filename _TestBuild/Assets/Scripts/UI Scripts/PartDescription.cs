using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartDescription : MonoBehaviour {

	public Dropdown weaponChangeDropdown;
	public Text descriptionBox;
	private TextDatabank tData;
	void Start()
	{
		tData = TextDatabank.instance;
		weaponChangeDropdown.onValueChanged.AddListener(delegate {OnTypeChange();});
	}

	public void OnTypeChange()
	{
		descriptionBox.text = tData.weaponDescriptionTxt[weaponChangeDropdown.value];
	}
}
