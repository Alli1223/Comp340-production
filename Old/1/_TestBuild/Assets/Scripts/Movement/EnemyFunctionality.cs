using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class EnemyFunctionality : MonoBehaviour
{

    private static ManagersManager tManage;

    private List<GameObject> playersInRange = new List<GameObject>();
    private List<GameObject> targetsInRange = new List<GameObject>();

    private Transform potentialCoverLocat;
    private Vector3 attackPotential;

    // Use this for initialization
    void Start()
    {
        tManage = ManagersManager.manager;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(Search(6));
        }
//        if (potentialCoverLocat != null)
//        {
//            potentialCoverLocat.GetComponent<MeshRenderer>().enabled = true;
//            potentialCoverLocat.GetComponent<MeshRenderer>().material = tManage.actionMat;
//        }
    }

    public stateEnum Search(int range)
    {
        potentialCoverLocat = null;
        stateEnum foundPlayers = FindPlayersInRange(range);
        if (foundPlayers == stateEnum.FAILURE)
        {
            return stateEnum.FAILURE;
        }
        else if (foundPlayers == stateEnum.SUCCESS)
        {
            Vector3 player = tManage.tDetect.RealtivePosition(tManage.tPlayer.currentPlayers, EnemyManager.currentEnemy.transform);
            int closestDist = tManage.tDetect.DistCheck(EnemyManager.currentEnemy.transform.position, player);
            player = (Vector3)tManage.tDetect.RealtivePosition(tManage.tGrid.currentTiles, player);
            if (EnemyManager.currentEnemy.GetComponent<TempEnemyVar>().moveDist > closestDist)
            {
                int? dist = ClosetBetweenCover(player); 
                if (dist != null && dist < closestDist)
                {
                    Vector3 midpoint = (player + tManage.tDetect.GetClosestGrid(EnemyManager.currentEnemy.transform.position, tManage.tGrid.currentTiles).position / 2f);
                    Transform coverLocat = ClosetBetweenCover(midpoint, PlayerVSEnemy(player));
                    potentialCoverLocat = coverLocat;
                    return stateEnum.SUCCESS;
                }
                attackPotential = player;
                return stateEnum.SUCCESS;
            }
            return stateEnum.FAILURE;
        }
        return stateEnum.FAILURE;
    }

    public static int? ClosetBetweenCover(Vector3 point)
    {  
        Tile currentTile = null;
        int bestResult = 10 ^ 6;
        for (int gL = 0; gL < tManage.tGrid.tileVariables.Width; gL++)
        {
            for (int y = 0; y < tManage.tGrid.tileVariables[gL].Width; y++)
            {
                for (int x = 0; x < tManage.tGrid.tileVariables[gL].Height; x++)
                {
                    Tile t = tManage.tGrid.tileVariables[gL][y, x];
                    if (t != null && t.tileCover != coverDirection.NONE)
                    {
                        if (tManage.tDetect.DistCheck(point, t.tPos) < bestResult && tManage.tDetect.DistCheck(EnemyManager.currentEnemy.transform.position, t.tPos) < tManage.tDetect.DistCheck(EnemyManager.currentEnemy.transform.position, point))
                        {
                            currentTile = t;
                            bestResult = tManage.tDetect.DistCheck(EnemyManager.currentEnemy.transform.position, t.tPos);
                        }
                    }
                }

            }
        }
        if (currentTile != null)
        {
            return bestResult;
        }
        else
        {
            return null;
        }
    }

    public static Transform ClosetBetweenCover(Vector3 point, coverDirection dir)
    {
        Tile currentTile = null;
        int bestResult = 10 ^ 6;
        for (int gL = 0; gL < tManage.tGrid.tileVariables.Width; gL++)
        {
            for (int y = 0; y < tManage.tGrid.tileVariables[gL].Width; y++)
            {
                for (int x = 0; x < tManage.tGrid.tileVariables[gL].Height; x++)
                {
                    Tile t = tManage.tGrid.tileVariables[gL][y, x];
                    if (t.tileCover == dir)
                    {
                        if (tManage.tDetect.DistCheck(point, t.tPos) < bestResult)
                        {
                            currentTile = t;
                            bestResult = tManage.tDetect.DistCheck(point, t.tPos);
                        }
                    }
                }
            }
        }
        return currentTile.thisTile.transform;
    }

    public static coverDirection PlayerVSEnemy(Vector3 cP)
    {        
        Vector3 cE = EnemyManager.currentEnemy.transform.position;
        if (Mathf.Abs(cP.x - cE.x) > Mathf.Abs(cP.z - cE.z))
        {
            if (cP.x > cE.x)
            {
                return coverDirection.South;
            }
            else
            {
                return coverDirection.North;
            }
        }
        else
        {
            if (cP.z > cE.z)
            {
                return coverDirection.East;
            }
            else
            {
                return coverDirection.West;
            }
        }
    }


    public stateEnum FindPlayersInRange(int range)
    {
        playersInRange = new List<GameObject>();
        playersInRange = GridPositionDetection._CloseObjs(tManage.tPlayer.currentPlayers, EnemyManager.currentEnemy.transform.position, range);
        if (playersInRange.Count == 0)
        {
            return stateEnum.FAILURE;
        }
        else
        {
            return stateEnum.SUCCESS;
        }
    }

    public stateEnum FindTargetsAtPoint(int range, Vector3 point)
    {
        targetsInRange = new List<GameObject>();
        targetsInRange = GridPositionDetection._CloseObjs(tManage.tPlayer.currentPlayers, point, range);
        if (targetsInRange.Count == 0)
        {
            return stateEnum.FAILURE;
        }
        else
        {
            return stateEnum.SUCCESS;
        }
    }
}
