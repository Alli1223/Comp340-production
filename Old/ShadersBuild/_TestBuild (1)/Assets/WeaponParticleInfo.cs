using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParticleInfo : MonoBehaviour 
{

	public GameObject[] prefabs;
	private ParticleSystem[] particleSystems;
	public Transform[] spawnPoints;
	// Use this for initialization
	void Start () 
	{
		particleSystems = new ParticleSystem[prefabs.Length];
		for (int i = 0; i < prefabs.Length; i++) 
		{
			GameObject go = GameObject.Instantiate (prefabs [i], spawnPoints [i]);

			go.transform.localScale = transform.parent.localScale;
			go.transform.localPosition = Vector3.zero;
			//Debug.Log (prefabs [i].name + " - " + go.transform.localRotation.eulerAngles + " - " + transform.parent.name);

			particleSystems [i] = go.transform.GetChild (0).GetComponent<ParticleSystem> ();
		}
	}

	public void Fire()
	{
		for(int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems [i].Play ();
		}
	}
	

}
