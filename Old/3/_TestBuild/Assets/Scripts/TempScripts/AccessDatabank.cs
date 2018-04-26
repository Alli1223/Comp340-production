using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class AccessDatabank : MonoBehaviour
{

	public TextDatabank textDatabank;
	public Text mainText;
	public int textNumber;
	public float letterDelay = 0.01f;
	private float curCountdown;
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			IncrementText ();
		}

	}

	public void IncrementText ()
	{

		StopAllCoroutines ();
		textNumber = textDatabank.curSelection;
		mainText.text = "";

	
		mainText.text = textDatabank.loreText [textNumber];

		if (textNumber >= textDatabank.loreText.Count) {
			textNumber = 0;
		}
		StartCoroutine(DialogueWait(3));
	
		textDatabank.curSelection++;
	}

	IEnumerator WriteText (string textToWrite)
	{
		foreach (char letter in textToWrite.ToCharArray()) {
			mainText.text += letter;
			yield return new WaitForSeconds (letterDelay);
		}
	}
	
	IEnumerator DialogueWait(float countdown)
	{
		Debug.Log("initated");
		curCountdown = countdown;
		while(curCountdown >0)
		{
			yield return new WaitForSeconds(1);
			curCountdown --;
		}
		Debug.Log("waited");
		IncrementText();
	}
}
