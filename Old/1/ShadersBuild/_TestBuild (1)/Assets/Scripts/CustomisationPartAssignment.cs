using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisationPartAssignment : MonoBehaviour {

	PlayerData playerData;

	void start()
	{
		playerData = gameObject.GetComponent<PlayerData> ();
	}
}
