using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour 
{
    public int team;

    [HideInInspector]
    public Tile closestTile;

	// Use this for initialization
	void Start () 
    {
        closestTile = GridExtentions.GetClosestGrid(transform.position, GridGeneration.gridSingle.tileVariables);
        transform.position = closestTile.tPos;
	}
	
	// Update is called once per frame
//	void Update () 
//  {
//		
//	}
}
