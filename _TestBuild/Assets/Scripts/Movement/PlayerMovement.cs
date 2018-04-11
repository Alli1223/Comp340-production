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

	private Tile currTile;
    private Transform currPlayer;
    private NavMeshAgent currPlayAI;
    private int currWeapon;

    private Vector3? moveToPoint;

    public enum PlayerActions
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
        if (PlayerManager.currentMech)
        {
            Transform tempTile = GridExtentions.GetClosestGrid(PlayerManager.currentMech.transform.position, tManage.tGrid.tileVariables).thisTile.transform;
            ShowMovable(tempTile.position, PlayerManager.currentMech.GetComponent<PlayerData>().CurrMoveRange());
        }
            
	}

    public void SetPlayer()
    {
        currPlayer = PlayerManager.currentMech.transform;
        currPlayAI = currPlayer.GetComponent<NavMeshAgent>();
        currPlayData = currPlayer.GetComponent<PlayerData>();
        currTile = currPlayData.curTile;

        if (shootingMode)
        {
            shootingMode = !shootingMode;
        }

        ShowMovable(currPlayer.position, currPlayData.CurrMoveRange());
    }
	
	// Update is called once per frame
	void Update () 
    {
        

        if (!actionPending)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (currPlayData.hasLeftArmWeapon && !currPlayData.leftArmWeaponFired)
                {
                    if (currPlayData.leftArmWeapon.apCost <= currPlayData.curAP)
                    {
                        currWeapon = 0;
                        pendingAction = PlayerActions.ShootingMode;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (currPlayData.hasRightArmWeapon && !currPlayData.rightArmWeaponFired)
                {
                    if (currPlayData.rightArmWeapon.apCost <= currPlayData.curAP)
                    {
                        currWeapon = 1;
                        pendingAction = PlayerActions.ShootingMode;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (currPlayData.hasBackWeapon && !currPlayData.backWeaponFired)
                {
                    if (currPlayData.backWeapon.apCost <= currPlayData.curAP)
                    {
                        currWeapon = 2;
                        pendingAction = PlayerActions.ShootingMode;
                    }
                }
            }
        }



        if (pendingAction != PlayerActions.NONE)
        {
            actionPending = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndAction();
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

    public void UpdateMe(Tile x, PlayerActions b)
    {
        if (!actionPending)
        {
            if (!shootingMode && x.occupyingObj == null)
            {
                moveToPoint = x.tPos;

                pendingAction = PlayerActions.Move;
                actionPending = true;

                    
            }
            else
            {
                if (x.occupyingObj != null)
                {
                    PlayerData pd = x.occupyingObj.GetComponent<PlayerData>();
                    if (pd.playerNum != currPlayData.playerNum)
                    {
                        if (currPlayData.GetWeaponMaxRange(currWeapon) >= GridExtentions.DistCheck(currPlayer.transform.position, x.tPos))
                        {
                            currPlayData.FireWeapon(currWeapon, pd);
                            shootingMode = false;
                            ShowMovable(currTile.thisTile.transform.position, currPlayData.CurrMoveRange());
                            //ShowMovable(currTile.thisTile.transform, currPlayData.CurrMoveRange());
                            EndAction();
                        }
                    }
                }
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
        if (nextPlayersTurn || currPlayer == null || currPlayer != PlayerManager.currentMech.transform && !actionPending)
        {
            if (PlayerManager.currentMech != null)
            {
                currPlayer = PlayerManager.currentMech.transform;
                currPlayAI = currPlayer.GetComponent<NavMeshAgent>();
                currPlayData = currPlayer.GetComponent<PlayerData>();
                currTile = currPlayData.curTile;
                if (shootingMode)
                {
                    ShootingMode(currTile.thisTile.transform);
                }
                else
                {
                    ShowMovable(currTile.thisTile.transform.position, currPlayData.CurrMoveRange());
                }
                nextPlayersTurn = false;
            }
        }

        if (actionPending)
        {
			if (pendingAction == PlayerActions.Move && !shootingMode)
            {
                if (moveToPoint != null && currPlayData.curMoveDist > 0 && currPlayData.curAP > 0 )
                {
                    if (!moveValues)
                    {
						tTile = GridExtentions.GetClosestGrid((Vector3)moveToPoint, tManage.tGrid.tileVariables);
                        if (tTile.tileCover == coverDirection.Building)
                        {
                            pendingAction = PlayerActions.NONE;     
                            actionPending = false;
                            moveToPoint = null;
                            return;
                        }
                        moved = GridExtentions.DistCheck(currPlayer.position, tTile.tPos);
                        pendingPath = currPlayAI.path;
						bool temp = currPlayAI.CalculatePath(tTile.tPos, pendingPath);
						currentPath = CreatePath(pendingPath.corners, tTile.tPos);
                        pathLength = 1;
                        moveValues = true;
                    }
                    if (!rotationStarted)
                    {
                        RotatePlayer(currentPath[pathLength]);
                    }
                    if (moveStarted)
                    {
						currPlayData.curTile = tTile;
						currPlayAI.SetDestination(tTile.tPos);
                        moveStarted = false;
                        EndMoveAction();
                    }
                }
            }
            else if(pendingAction == PlayerActions.ShootingMode)
            {
                shootingMode = !shootingMode;
                if (shootingMode)
                {
//                    HideMovable();
                    //Shoot mode tbc
					ShootingMode(currTile.thisTile.transform);
                    EndAction();
                }
                else
                {
                    ShowMovable(currTile.thisTile.transform.position, currPlayData.CurrMoveRange());
                    EndAction();
                }
            }
        }
    }
    Tile tTile;
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
			Vector3 movDot = GridExtentions.GetClosestGrid(path[point], tManage.tGrid.tileVariables).tPos;
            pL.SetDestination(movDot);
			Vector3 playDot = GridExtentions.GetClosestGrid(currPlayer.position, tManage.tGrid.tileVariables).tPos;
            if (Vector3.Distance(playDot, movDot) <= 1.5f)
            {
                point++;
//                moveStarted = false;
            }
        }
        if (point == path.Length)
        {            
			if (Vector3.Distance(currPlayer.position, tTile.tPos) <= 1f)
            {
                nextPlayersTurn = true;
                ShowMovable(currTile.thisTile.transform.position, currPlayData.CurrMoveRange());
                //ShowMovable(currTile.thisTile.transform, currPlayData.CurrMoveRange());
                moveFinish = true;
            }
        }
        else if(moveStarted == false)
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
        if (currPlayData.IsOutOfActions())
        {
            tManage.tTurn.RemoveFromAction(currPlayer.gameObject);

        }
    }

    private void EndMoveAction()
    {
        currPlayData.curMoveDist -= moved;
        currPlayData.curAP -=  moved ;
        currTile.PolandIsFree();
        currTile = tTile;
        currTile.Occupy(currPlayer.gameObject);
        moveToPoint = null;
        rotationStarted = false;
        moveStarted = false;
        moveValues = false;
        moveFinish = false;
        nextPlayersTurn = true;
        EndAction();
        Destroy(rotationObj);
        tManage.tPlayer.gamePlayUI.UpdateStatPanel();
        if (currPlayData.IsOutOfActions())
        {
            tManage.tTurn.RemoveFromAction(currPlayer.gameObject);
        }
        else
        {
            ShowMovable(currTile.tPos, currPlayData.CurrMoveRange());
        }
    }

    void ShootingMode(Transform pTile)
    {
        List<GameObject> enemies = new List<GameObject>();

        tManage.tDetect.FindTilesInDist (pTile, tManage.tGrid.tileMeshs, currPlayData.GetWeaponMaxRange(currWeapon));
        ShowMovable(pTile.position, currPlayData.GetWeaponMaxRange(currWeapon), false);
//        foreach (MeshRenderer x in tManage.tGrid.tileMeshs) 
//        {
//            x.material.color = tManage.actionMat.color;
//        }
        enemies.AddRange (GameObject.FindGameObjectsWithTag ("Player"));
        foreach (GameObject e in enemies) 
        {
            if (currPlayData.playerNum != e.GetComponent<PlayerData>().playerNum)
            {
                if (GridExtentions.DistCheck(PlayerManager.currentMech.transform.position, e.transform.position) > currPlayData.GetWeaponMaxRange(currWeapon))
                {
//                    e.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else
                {
//                    e.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

        }
    }


    List<Tile> currentMoveableTiles = new List<Tile>();

    public void ShowMovable(Vector3 origin, int range, bool defaultMaterial = true)
    {
        HideMovable();

        currentMoveableTiles = GridExtentions._TilesInARange(currTile.tPos, range);

        GridExtentions.SetMaterial(defaultMaterial);

        for (int i = 0; i < currentMoveableTiles.Count; i++)
        {
            currentMoveableTiles[i].thisTile.SetActive(true);
        }

    }

    public void HideMovable()
    {
        for (int i = 0; i < currentMoveableTiles.Count; i++)
        {
            currentMoveableTiles[i].thisTile.SetActive(false);
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
                    //e.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

}
