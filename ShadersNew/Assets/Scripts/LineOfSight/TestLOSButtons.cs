using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLOSButtons : MonoBehaviour 
{
    ManagersManager tManage = ManagersManager.manager;
	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RaycastHit hit;
//            Ray test = new Ray(, Camera.main.transform.forward);
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Debug.Log(LineOfSightFunctions._TileSight(GridPositionDetection.GetClosestGrid(PlayerManager.currentPlayer.transform.position, GridGeneration.gridSingle.currentTiles).position, GridPositionDetection.GetClosestGrid(hit.transform.position, GridGeneration.gridSingle.currentTiles).position) + " : " + hit.transform.name);
//                    Debug.Log(LineOfSightFunctions._TileSight(PlayerManager.currentPlayer.transform.position, hit.transform.position) + " : " + hit.transform.name);

                }
            }

        }
	}
}
