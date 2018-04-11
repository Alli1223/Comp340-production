using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailer_replay_animation : MonoBehaviour {


	public float maxTime;
	public float minTime;
	float _time;

	Animator anim;
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
		_time = Random.Range (minTime, maxTime);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_time <= 0f) {
			anim.SetTrigger ("Delay");
			_time = Random.Range (minTime, maxTime);
		} else {
			_time -= Time.deltaTime;
		}
	}
}
