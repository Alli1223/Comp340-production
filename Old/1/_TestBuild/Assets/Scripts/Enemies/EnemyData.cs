using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour {


	// temporary enemy class that contains a destory function
	public bool targetable;
	public int health;

	public void Death()
	{
		Destroy (gameObject);
	}
}
