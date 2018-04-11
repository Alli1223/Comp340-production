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
    public static PlayerData currentMech;
	[HideInInspector]
    public static PlayerManager gPlayer;
    PlayerMovement playerMovement;

	public List<GameObject> currentPlayers = new List<GameObject>();

    public List<List<PlayerData>> allPlayerMechs = new List<List<PlayerData>>();
    int currentSelectedPlayer;

    [SerializeField]
    LayerMask gridLayer;
    [HideInInspector]
    public UIGamePlay gamePlayUI;

	private ManagersManager tManage;

    private PlayerData playerDat;
    private  bool addedToAction = false;
    [HideInInspector]
    public bool shootingMode = false;

    Transform tileHighlighter;
    public GameObject tileHighlighterPrefab;

    //Makes Grid gen script a singleton
    void Awake()
    {
        if (gPlayer == null)
            gPlayer = this;
        else
            Destroy(this); 
		
        tManage = ManagersManager.manager;

    }

    // mechs have been spawned in the scene and have been added to the list
    public void SpawnDone()
    {
        playerMovement = GetComponent<PlayerMovement>();
        tileHighlighter = GameObject.Instantiate(tileHighlighterPrefab).transform;
        PlayerUniqueIDAssignment();
        currentSelectedPlayer = -1;
        EndTurn();
    }

	void Start()
	{
        tManage.tPlayer = gPlayer;
	}


    void SelectMech(GameObject desiredObject)
    {
        playerDat = desiredObject.GetComponent<PlayerData>();
        currentMech = playerDat;
        gamePlayUI.SetMech(playerDat);
        playerMovement.SetPlayer();
    }

    void SelectMech(PlayerData desiredMech)
    {
        playerDat = desiredMech;
        currentMech = playerDat;
        gamePlayUI.SetMech(desiredMech);
        playerMovement.SetPlayer();
    }

    void SelectMech(int playerID, int mechID)
    {
        playerDat = allPlayerMechs[currentSelectedPlayer][mechID];
        currentMech = playerDat;
        gamePlayUI.SetMech(playerDat);
        playerMovement.SetPlayer();
    }



    public void EndTurn()
    {
        currentSelectedPlayer++;

        if (currentSelectedPlayer >= allPlayerMechs.Count)
        {
            currentSelectedPlayer = 0;
        }

        for (int i = 0; i < allPlayerMechs[currentSelectedPlayer].Count; i++)
        {
            allPlayerMechs[currentSelectedPlayer][i].PrepareForTurn();
        }

        for (int i = 0; i < allPlayerMechs[currentSelectedPlayer].Count; i++)
        {
            tManage.tTurn.AddToAction(allPlayerMechs[currentSelectedPlayer][i].gameObject);
        }
        SelectMech(currentSelectedPlayer, 0);

        // TEMP tbc do UI stuff to indicate the switch
    }
        

    void SelectMech(Tile clickedTile)
    {
        if (clickedTile.occupyingObj != null)
        {
            //clickedTile.isVisiable = true;
            PlayerData occupyingMech = clickedTile.occupyingObj.GetComponent<PlayerData>();

            if (!occupyingMech.isOutOfActions)
            {
                playerDat = occupyingMech;
                currentMech = playerDat;
                gamePlayUI.SetMech(playerDat);
                playerMovement.SetPlayer();
            }
        }
    }

    // select a unit that currently still can perform actions
    public void SelectUnitNotOutOfAction()
    {
        for (int i = 0; i < allPlayerMechs[currentSelectedPlayer].Count; i++)
        {
            if (!allPlayerMechs[currentSelectedPlayer][i].isOutOfActions)
            {
                SelectMech(allPlayerMechs[currentSelectedPlayer][i]);
                return;
            }
        }
    }

    public void GoToNextActiveUnit()
    {
        int currentPlayerID = 0;

        for (int i = 0; i < allPlayerMechs[currentSelectedPlayer].Count; i++)
        {
            if (allPlayerMechs[currentSelectedPlayer][i] == currentMech)
            {
                currentPlayerID = i;
                break;
            }
        }

        bool foundUnit = false;

        while (!foundUnit)
        {
            currentPlayerID++;

            if (currentPlayerID >= allPlayerMechs[currentSelectedPlayer].Count)
            {
                currentPlayerID = 0;
            }

            if (!allPlayerMechs[currentSelectedPlayer][currentPlayerID].isOutOfActions)
            {
                SelectMech(allPlayerMechs[currentSelectedPlayer][currentPlayerID]);
                foundUnit = true;
            }
        }
    }

    public void Update()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, gridLayer.value);
        if (hit.transform != null)
        {
            Tile highligthedTile = GridExtentions.GetClosestGrid(hit.point, GridGeneration.gridSingle.tileVariables);
            tileHighlighter.position = highligthedTile.tPos + Vector3.up * 0.01f;
                    
            if (Input.GetMouseButtonDown(0))
            {
                SelectMech(highligthedTile);
            }
        
            if (Input.GetMouseButtonDown(1) && currentMech != null)
            {
                GetComponent<PlayerMovement>().UpdateMe(highligthedTile, PlayerMovement.PlayerActions.Move);
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GoToNextActiveUnit();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EndTurn();
        }

    }
//    public void PlayerTurn(List<GameObject> curPlayers)
//    { 
//        if (!curPlayers.Contains(currentMech))
//        {
//            SelectMech(curPlayers[0]);
//        }
//
//        if (!addedToAction)
//        {
//            int count = curPlayers.Count;
//            tManage.tTurn.AddToAction(curPlayers);
//            for (int i = 0; i < count; i++)
//            {
//                Vector3 tilePos = GridExtentions.GetClosestGrid(curPlayers[i].transform.position, tManage.tGrid.currentTiles).position;
//                curPlayers[i].transform.position = new Vector3(tilePos.x, curPlayers[i].transform.position.y, tilePos.z);
//                if (curPlayers[i].GetComponent<PlayerData>().curHealth <= 0)
//                {
//                    tManage.tTurn.RemoveFromAction(curPlayers[i]);
//                    if (curPlayers.Count > 0 && currentMech == curPlayers[i])
//                    {
//                        curPlayers[0] = curPlayers[i];
//                    }
//                    curPlayers[i].SetActive(false);
//                    curPlayers.Remove(curPlayers[i]);
//                }
//                else
//                {
//                    ResetDist(curPlayers[i].transform);
//                }
//            }
//            addedToAction = true;
//        }
//
//        RaycastHit hit;
//        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f ,gridLayer.value);
//                if (hit.transform != null)
//        {
//            Tile highligthedTile = GridExtentions.GetClosestGrid(hit.point, GridGeneration.gridSingle.tileVariables);
//            tileHighlighter.position = highligthedTile.tPos + Vector3.up * 0.01f;
//            
//            if (Input.GetMouseButtonDown (0)) 
//            {
//                SelectMech(highligthedTile);
//            }
//
//            if (Input.GetMouseButtonDown(1) && currentMech != null)
//            {
//                GetComponent<PlayerMovement>().UpdateMe(highligthedTile, PlayerMovement.PlayerActions.Move);
//                //gamePlayUI.UpdateStatPanel();
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.Return))
//        {
//            ForceEndTurn();
//        }
//    }

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
			x.GetComponent<PlayerData> ().curTile = GridExtentions.GetClosestGrid(x.transform.position, tManage.tGrid.tileVariables);

            x.name = "Player (ID:" + uniqueID + ")";
            idNumerator++;
        }

    }
}
