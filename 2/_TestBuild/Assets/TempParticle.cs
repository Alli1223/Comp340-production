using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempParticle : MonoBehaviour 
{
	public ParticleSystem expOne;
	public ParticleSystem expTwo;
	public ParticleSystem sparks;


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Keypad0)) 
		{
			expOne.Play ();
		}

		if (Input.GetKeyDown (KeyCode.Keypad1)) 
		{
			expTwo.Play ();
		}

		if (Input.GetKeyDown (KeyCode.Keypad2)) 
		{
			sparks.Play ();
		}
	}
}
