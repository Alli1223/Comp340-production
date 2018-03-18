/*
	Created On:		27/09/2017 10:34
	Created By: 	Marc Andrews
	Last Edit: 		11/10/2017 10:34
	Last Edit By: 	Marc Andrews
*/
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour 
{
	
	public int maxMoveDist;
    public static GameObject currentPlayer;
	[HideInInspector]
    public static PlayerManager gPlayer;

	public List<GameObject> currentPlayers = new List<GameObject>();
    public List<GameObject> firstPlayers = new List<GameObject>();
    public List<GameObject> secondPlayers = new List<GameObject>();


	private ManagersManager tManage;


//    private TempMove checker;
    private PlayerData valueStruct;
    private  bool addedToAction = false;
    [HideInInspector]
    public bool shootingMode = false;
    private bool switched = false;

    //Makes Grid gen script a singleton
    void Awake()
    {
        if (gPlayer == null)
            gPlayer = this;
        else
            Destroy (this); 


        currentPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
//        currentPlayer = currentPlayers[0];
        foreach (GameObject x in currentPlayers) 
        {
//          Vector3 tilePos = tManage.tDetect.GetClosestGrid (x.transform.position, tManage.tGrid.currentTiles).position;
//          x.transform.position = new Vector3 (tilePos.x, x.transform.position.y, tilePos.z);
            if (!x.GetComponent<PlayerData> ()) 
            {
                x.AddComponent<PlayerData>();
            }
            if (!x.GetComponent<NavMeshAgent>())
            {
                x.AddComponent<NavMeshAgent>();
            }
            SetPlayers(x.transform);
            ResetDist(x.transform);
        }
        currentPlayer = firstPlayers[0];
        PlayerUniqueIDAssignment();
    }


	void Start()
	{
		tManage = ManagersManager.manager;
		tManage.tPlayer = gPlayer;
        valueStruct = currentPlayer.GetComponent<PlayerData> ();
//		checker = GetComponent<TempMove> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (tManage.tTurn.playersTurn) 
		{
            if (!switched)
            {
                addedToAction = false;
                switched = true;
            }
            PlayerTurn(firstPlayers);
		} else 
        {
            if (switched)
            {
                addedToAction = false;
                switched = false;
            }
            PlayerTurn(secondPlayers);

		}
	}

    public void PlayerTurn(List<GameObject> curPlayers)
    { 
//        tManage.tTurn.ClearAll();
        if (!addedToAction) 
        {
            int count = curPlayers.Count;
            tManage.tTurn.AddMultipleToAction (curPlayers);
            for (int i = 0; i < count; i++) 
            {
                Vector3 tilePos = GridPositionDetection.GetClosestGrid (curPlayers[i].transform.position, tManage.tGrid.currentTiles).position;
                curPlayers[i].transform.position = new Vector3 (tilePos.x, curPlayers[i].transform.position.y, tilePos.z);
                curPlayers[i].GetComponent<MeshRenderer> ().material.color = Color.green;
                if (curPlayers[i].GetComponent<PlayerData>().curHealth <= 0)
                {
                    tManage.tTurn.RemoveFromAction(curPlayers[i]);
                    if (curPlayers.Count > 0 && currentPlayer == curPlayers[i])
                    {
                        curPlayers[0] = curPlayers[i];
                    }
                    curPlayers[i].SetActive(false);
                    curPlayers.Remove(curPlayers[i]);
                }else
                {
                    ResetDist (curPlayers[i].transform);
                }
            }
//            checker.ShowMovable ();
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

                    if (valueStruct.curAP == 0) 
                    {
                        tManage.tTurn.RemoveFromAction (currentPlayer);
                        currentPlayer.GetComponent<MeshRenderer> ().material.color = Color.grey;
                    } else 
                    {
//                        valueStruct.curMoveDist = checker.modifiedMovDist;
                    }
                    valueStruct = hit.transform.GetComponent<PlayerData> () != valueStruct ? hit.transform.GetComponent<PlayerData> () : valueStruct;
                    currentPlayer = hit.transform.gameObject;
                    tManage.tCamManage.changeCamera();
//                    if (shootingMode) 
//                    {
//                        checker.ShootMode ();
//                    } else 
//                    {
//                        checker.ShowMovable ();
//                    }
                    Debug.Log (tManage.tTurn.waitForAction.Count);
                } 
            }
        }

//        if (Input.GetKeyDown (KeyCode.Tab)) 
//        {
//            shootingMode = !shootingMode;
//            if (shootingMode) 
//            {
//                checker.ShootMode ();
//            } else 
//            {
//                checker.ShowMovable ();
//            }
//        }
//        valueStruct.curMoveDist = valueStruct.curMoveDist != checker.modifiedMovDist ? checker.modifiedMovDist : valueStruct.curMoveDist;
        if (valueStruct.curAP == 0 || Input.GetKeyDown(KeyCode.T)) 
        {
            tManage.tTurn.RemoveFromAction (currentPlayer);
            currentPlayer.GetComponent<MeshRenderer> ().material.color = Color.grey;
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

        player.GetComponent<PlayerData> ().curMoveDist = player.GetComponent<PlayerData>().maxMoveDist;
        player.GetComponent<PlayerData> ().curAP = player.GetComponent<PlayerData> ().maxAP;
     }

    private void SetPlayers(Transform player)
    {
        if (player.GetComponent<PlayerData>().playerNum == 0)
            {
                firstPlayers.Add(player.gameObject);
                player.GetComponent<MeshRenderer> ().material.color = Color.green;
            }
            else
            {
                secondPlayers.Add(player.gameObject);
                player.GetComponent<MeshRenderer> ().material.color = Color.red;

            }
    }

    private void PlayerUniqueIDAssignment()
    {
        if (currentPlayers.Count == 0)
        {
            return;
        }
        int idNumerator = 0;
        foreach (GameObject x in currentPlayers)
        {
            int uniqueID = (Random.Range(1, 9) * 100) + idNumerator;
            x.GetComponent<PlayerData>().uniqueID = uniqueID;
            x.name = "Player (ID:" + uniqueID + ")";
            idNumerator++;
        }
    }
}
