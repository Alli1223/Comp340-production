/*
	Created On:		27/09/2017 10:34
	Created By: 	Marc Andrews
	Last Edit: 		11/10/2017 10:34
	Last Edit By: 	Marc Andrews
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour 
{
	
	public int maxMoveDist;
    public static GameObject currentPlayer;
	[HideInInspector]
    public static PlayerManager gPlayer;

	public List<GameObject> currentPlayers = new List<GameObject>();

	private ManagersManager tManage;


    //Makes Grid gen script a singleton
    void Awake()
	{
		if (gPlayer == null)
			gPlayer = this;
		else
			Destroy (this); 


        currentPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        currentPlayer = currentPlayers[0];
		foreach (GameObject x in currentPlayers) 
		{
//			Vector3 tilePos = tManage.tDetect.GetClosestGrid (x.transform.position, tManage.tGrid.currentTiles).position;
//			x.transform.position = new Vector3 (tilePos.x, x.transform.position.y, tilePos.z);
			if (!x.GetComponent<TempPlayerVar> ()) 
			{
                x.AddComponent<TempPlayerVar>();
			}
            ResetDist(x.transform);
		}
    }

	TempMove checker;
	TempPlayerVar valueStruct;
	bool addedToAction = false;
	[HideInInspector]
	public bool shootingMode = false;

	void Start()
	{
		tManage = ManagersManager.manager;
		tManage.tPlayer = gPlayer;
		valueStruct = currentPlayer.GetComponent<TempPlayerVar> ();
		checker = GetComponent<TempMove> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (tManage.tTurn.playersTurn) 
		{
			if (!addedToAction) 
			{
				tManage.tTurn.AddMultipleToAction (currentPlayers);
				foreach (GameObject x in currentPlayers) 
				{
					Vector3 tilePos = tManage.tDetect.GetClosestGrid (x.transform.position, tManage.tGrid.currentTiles).position;
					x.transform.position = new Vector3 (tilePos.x, x.transform.position.y, tilePos.z);
					x.GetComponent<MeshRenderer> ().material.color = Color.green;
                    if (x.GetComponent<TempPlayerVar>().health <= 0)
                    {
                        tManage.tTurn.RemoveFromAction(x);
                        currentPlayers.Remove(x);
                        if (currentPlayers.Count > 0 && currentPlayer == x)
                        {
                            currentPlayers[0] = x;
                        }
						x.SetActive(false);
                    }else
                    {
						ResetDist (x.transform);
                    }
				}
				checker.ShowMovable ();
				addedToAction = true;
			}
				if (Input.GetMouseButtonDown (0)) 
				{
					Ray cameraToPoint = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
				if (Physics.Raycast (cameraToPoint, out hit)) 
				{
					if (hit.transform.tag == "Player" && tManage.tTurn.waitForAction.Contains (hit.transform.gameObject)) 
					{

						if (valueStruct.currentAP == 0) 
						{
							tManage.tTurn.RemoveFromAction (currentPlayer);
							currentPlayer.GetComponent<MeshRenderer> ().material.color = Color.grey;
						} else 
						{
							valueStruct.currentDist = checker.modifiedMovDist;
						}
						valueStruct = hit.transform.GetComponent<TempPlayerVar> () != valueStruct ? hit.transform.GetComponent<TempPlayerVar> () : valueStruct;
						currentPlayer = hit.transform.gameObject;
						tManage.tCamManage.changeCamera();
						if (shootingMode) 
						{
							checker.ShootMode ();
						} else 
						{
							checker.ShowMovable ();
						}
						Debug.Log (tManage.tTurn.waitForAction.Count);
					} 
				}
			}

			if (Input.GetKeyDown (KeyCode.Tab)) 
			{
				shootingMode = !shootingMode;
				if (shootingMode) 
				{
					checker.ShootMode ();
				} else 
				{
					checker.ShowMovable ();
				}
			}
			valueStruct.currentDist = valueStruct.currentDist != checker.modifiedMovDist ? checker.modifiedMovDist : valueStruct.currentDist;
			if (valueStruct.currentAP == 0 || Input.GetKeyDown(KeyCode.T)) 
			{
				tManage.tTurn.RemoveFromAction (currentPlayer);
				currentPlayer.GetComponent<MeshRenderer> ().material.color = Color.grey;
			}
		} else 
		{
			addedToAction = false;
		}
	}

    public Transform FindPlayer(Transform pCompare)
    {
        Transform returnPlayer = pCompare;
        foreach (GameObject x in currentPlayers)
        {
            if (x.transform == pCompare)
            {               
                returnPlayer = x.transform;
            }
        }
        return returnPlayer;
    }

	public void ResetDist(Transform player)
	{
//		Vector3 tilePos = tManage.tDetect.GetClosestGrid (player.position, tManage.tGrid.currentTiles).position;
//		player.position = new Vector3 (tilePos.x, player.position.y, tilePos.z);
		player.GetComponent<MeshRenderer> ().material.color = Color.green;

		player.GetComponent<TempPlayerVar> ().currentDist = maxMoveDist;
		player.GetComponent<TempPlayerVar> ().currentAP = player.GetComponent<TempPlayerVar> ().maximumAP;
	}
}
