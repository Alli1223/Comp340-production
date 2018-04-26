using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHealthSystem : MonoBehaviour 
{
    [SerializeField]
    private int health = 100;


	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (health <= 0)
        {
			gameObject.SetActive (false);
        }
	}

    public void Damage(int damageTaken)
    {
        health -= damageTaken; 
    }
}
