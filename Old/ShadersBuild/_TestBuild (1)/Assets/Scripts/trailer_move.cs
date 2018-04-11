using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailer_move : MonoBehaviour {

	public Vector3 move;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//transform.localPosition += move * Time.deltaTime;
		transform.Translate (move * Time.deltaTime, Space.Self);
	}
}
