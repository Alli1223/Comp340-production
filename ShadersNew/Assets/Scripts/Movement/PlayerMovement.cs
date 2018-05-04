using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour 
{
	[SerializeField]
	[Range(0.1f,1f)]
	private float tickTime;
	public static bool shootingMode;

    private ManagersManager tManage;
    private NavMeshAgent player;

    private bool nextPlayersTurn;

    private Transform currTile;
    private Transform currPlayer;
    private NavMeshAgent currPlayAI;

    private Vector3? moveToPoint;

    private enum PlayerActions
    {
        NONE,
        ShootingMode,
        Move
    };

    private PlayerActions pendingAction;

    private bool tickEnabled = true, actionPending = false, rotationPending = false, rotationStarted = false, moveStarted = false, moveValues = false, moveFinish = false; 

    public PlayerData currPlayData;

	// Use this for initialization
	void Start () 
    {
        tManage = ManagersManager.manager;
        StartCoroutine(MoveUpdate());
        Transform tempTile = GridPositionDetection.GetClosestGrid(PlayerManager.currentPlayer.transform.position, tManage.tGrid.currentTiles);
        ShowMovable(tempTile, PlayerManager.currentPlayer.GetComponent<PlayerData>().maxMoveDist);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!actionPending)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
                if (hit.transform != null && hit.transform.CompareTag("gridPiece") && hit.transform.GetComponent<MeshRenderer>().enabled == true)
                {
                    moveToPoint = hit.point;
                    pendingAction = PlayerActions.Move;
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                pendingAction = PlayerActions.ShootingMode;
            }
            if (pendingAction != PlayerActions.NONE)
            {
                actionPending = true;
            }
        }

        if (rotationPending)
        {
            currPlayer.rotation = Quaternion.RotateTowards(currPlayer.rotation, rotationObj.transform.rotation, Time.deltaTime * 110);
            if (currPlayer.rotation.eulerAngles.y == rotationObj.transform.rotation.eulerAngles.y)
            {
                rotationPending = false;
                moveStarted = true;

            }
        }
	}

    IEnumerator MoveUpdate()
    {
        int i = 0;
        while (tickEnabled)
        {
            i++;
            Debug.Log("Tick: " + tickTime);
            MoveCheck();
			yield return new WaitForSecondsRealtime(tickTime);
        }
    }

    void MoveCheck()
    {        
		if (nextPlayersTurn || currPlayer == null || currPlayer != PlayerManager.currentPlayer.transform && !actionPending)
        {
			currPlayer = PlayerManager.currentPlayer.transform;
			currTile = GridPositionDetection.GetClosestGrid (currPlayer.position, tManage.tGrid.currentTiles);
            currPlayAI = currPlayer.GetComponent<NavMeshAgent>();
            currPlayData = currPlayer.GetComponent<PlayerData>();
            if (shootingMode)
            {
                ShootingMode(currTile);
            }
            else
            {
                ShowMovable(currTile, currPlayData.curMoveDist);
            }
            nextPlayersTurn = false;
        }

        if (actionPending)
        {
			if (pendingAction == PlayerActions.Move && !shootingMode)
            {
                if (moveToPoint != null && currPlayData.curMoveDist > 0 && currPlayData.curAP > 0 )
                {
                    if (!moveValues)
                    {
                        tTile = GridPositionDetection.GetClosestGrid((Vector3)moveToPoint, tManage.tGrid.currentTiles);
                        moved = GridPositionDetection.DistCheck(currPlayer.position, tTile.position);
                        pendingPath = currPlayAI.path;
                        bool temp = currPlayAI.CalculatePath(tTile.position, pendingPath);
                        currentPath = CreatePath(pendingPath.corners, tTile.position);
                        pathLength = 1;
                        moveValues = true;
                    }
                    if (!rotationStarted)
                    {
                        RotatePlayer(currentPath[pathLength]);
                    }
                    if (moveStarted)
                    {
                        currPlayAI.SetDestination(tTile.position);
                        moveStarted = false;
//                        Move(currentPath,pathLength,currPlayAI);
//                    }
//                    else
//                    {
//                        if (currPlayAI.hasPath)
//                        {                            
//                            if (Vector3.Distance(GridPositionDetection.GetClosestGrid(currPlayer.position, tManage.tGrid.currentTiles).position, tTile.position) <= 1)
//                            {
//                                moveFinish = true;
////                                currPlayAI.path = null;
//                            }
//                        }
//                    }
//                    if (moveFinish)
//                    {
                        EndMoveAction();
                    }
                }
            }
            else if(pendingAction == PlayerActions.ShootingMode)
            {
                shootingMode = !shootingMode;
                if (shootingMode)
                {
                    ShootingMode(currTile);
                    EndAction();
                }
                else
                {
                    ShowMovable(currTile, currPlayData.curMoveDist);
                    EndAction();
                }
            }
        }
    }
    Transform tTile;
    Vector3 tilePos;
    GameObject rotationObj;
    NavMeshPath pendingPath;
    Vector3[] currentPath;
    int pathLength;
    int moved;

    private void Move(Vector3[] path, int point, NavMeshAgent pL)
    {
        if (point < path.Length)
        { 
            Vector3 movDot = GridPositionDetection.GetClosestGrid(path[point], tManage.tGrid.currentTiles).position;
            pL.SetDestination(movDot);
            Vector3 playDot = GridPositionDetection.GetClosestGrid(currPlayer.position, tManage.tGrid.currentTiles).position;
            if (Vector3.Distance(playDot, movDot) <= 1.5f)
            {
                point++;
//                moveStarted = false;
            }
        }
        if (point == path.Length)
        {            
            if (Vector3.Distance(currPlayer.position, tTile.position) <= 1f)
            {
                nextPlayersTurn = true;
                ShowMovable(currTile, currPlayData.curMoveDist);
                moveFinish = true;
            }
        }
        else if(moveStarted = false)
        {
            rotationStarted = true;
        }

    }

    void RotatePlayer(Vector3 lookTo)
    {
        rotationObj = new GameObject();
        tilePos = (Vector3)moveToPoint;
//        currPlayer.GetComponent<NavMeshAgent>().path.corners
        rotationObj.transform.position = currPlayer.position;
        rotationObj.transform.LookAt(new Vector3(tilePos.x, currPlayer.position.y, tilePos.z));
        rotationPending = true;
        rotationStarted = true;
    }


    private Vector3[] CreatePath(Vector3[] currentTurns, Vector3 endPoint)
    {
        Vector3[] turns = new Vector3[(currentTurns.Length + 1)];
        turns[0] = endPoint;
        int upTick = 1;
        for(int downTick = (currentTurns.Length - 1); downTick > 0; downTick--)
        {            
            turns[upTick] = currentTurns[downTick];
            upTick++;
        }
        return turns;
    }

    void EndAction()
    {
        pendingAction = PlayerActions.NONE;
        actionPending = false;
    }

    private void EndMoveAction()
    {
        currPlayData.curMoveDist -= moved;
        currPlayData.curAP -=  moved ;
        currTile = tTile;
        //                        currPlayer.GetComponent<NavMeshAgent>().SetDestination(tTile.position);
        moveToPoint = null;
        rotationStarted = false;
        moveStarted = false;
        moveValues = false;
        moveFinish = false;
        nextPlayersTurn = true;
        EndAction();
		ShowMovable(currTile, currPlayData.curMoveDist);
        Destroy(rotationObj);
    }

    void ShootingMode(Transform pTile)
    {
        List<GameObject> enemies = new List<GameObject>();

        tManage.tDetect.FindTilesInDist (pTile, tManage.tGrid.tileMeshs, (float)PlayerManager.currentPlayer.GetComponent<PlayerData> ().curWeapRNG);
        foreach (MeshRenderer x in tManage.tGrid.tileMeshs) 
        {
            x.material.color = tManage.actionMat.color;
        }
        enemies.AddRange (GameObject.FindGameObjectsWithTag ("Player"));
        foreach (GameObject e in enemies) 
        {
            if (currPlayData.playerNum != e.GetComponent<PlayerData>().playerNum)
            {
                if (GridPositionDetection.DistCheck(PlayerManager.currentPlayer.transform.position, e.transform.position) > PlayerManager.currentPlayer.GetComponent<PlayerData>().curWeapRNG)
                {
                    e.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else
                {
                    e.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

        }
    }

    public void ShowMovable(Transform point, int range)
    {
        List<GameObject> enemies = new List<GameObject>();

        tManage.tDetect.FindTilesInDist(point, tManage.tGrid.tileMeshs, (float)range);
        if (tManage.tGrid.tileMeshs [0].material.color == tManage.actionMat.color) 
        {
            tManage.tGrid.RevertTileColour ();
            enemies.AddRange (GameObject.FindGameObjectsWithTag ("Player"));
            foreach (GameObject e in enemies) 
            {  
                if (currPlayData.playerNum != e.GetComponent<PlayerData>().playerNum)
                {
                    e.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

}
