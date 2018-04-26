using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailer_delayAnim : MonoBehaviour {


	Animator anim;

	public float time;
	float _time;
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
		_time = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_time > time) {
			anim.SetTrigger ("Delay");
		} else {
			_time += Time.deltaTime;
		}
	}
}
