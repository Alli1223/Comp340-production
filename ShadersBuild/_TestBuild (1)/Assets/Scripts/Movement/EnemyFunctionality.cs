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
            Debug.Log(SearchForPlayer(6));
            Debug.Log(" : " + attackPotential);
            GameObject tempCover = GameObject.Find("GameObject (1)");
            tempCover.transform.position = potentialCoverLocat.position;
        }
    }


//    public stateEnum SearchForMovePoint(int range)
//    {
//        stateEnum foundPlayers = FindPlayersInRange(range);
//        if (foundPlayers == stateEnum.FAILURE)
//        {
//            
//        }
//        else if (foundPlayers == stateEnum.SUCCESS)
//        {
//            
//        }
//    }

    /// <summary>
    /// Searches for player.
    /// </summary>
    /// <returns>If the operation was successful.</returns>
    /// <param name="range">Range the enemy can search in tiles.</param>
    public stateEnum SearchForPlayer(int range)
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
            int closestDist = GridPositionDetection.DistCheck(EnemyManager.currentEnemy.transform.position, player);
            player = (Vector3)tManage.tDetect.RealtivePosition(tManage.tGrid.currentTiles, player);
            if (EnemyManager.currentEnemy.GetComponent<TempEnemyVar>().moveDist > closestDist)
            {
                int? dist = ClosetBetweenCover(player); 
                if (dist != null && dist < closestDist)
                {
                    Vector3 midpoint = (player + GridPositionDetection.GetClosestGrid(EnemyManager.currentEnemy.transform.position, tManage.tGrid.currentTiles).position / 2f);
                    Transform coverLocat = ClosetBetweenCover(midpoint, PlayerVSEnemy(player));
                    potentialCoverLocat = coverLocat;
                    attackPotential = player;
                    return stateEnum.SUCCESS;
                }
                attackPotential = player;
                return stateEnum.SUCCESS;
            }
            return stateEnum.FAILURE;
        }
        return stateEnum.FAILURE;
    }

    /// <summary>
    /// Gets the closest cover between the enemy and the point.
    /// </summary>
    /// <returns>Closest cover between enemy and point.</returns>
    /// <param name="point">Point that you want to find cover between.</param>
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
                        if (GridPositionDetection.DistCheck(point, t.tPos) < bestResult && GridPositionDetection.DistCheck(EnemyManager.currentEnemy.transform.position, t.tPos) < GridPositionDetection.DistCheck(EnemyManager.currentEnemy.transform.position, point))
                        {
                            currentTile = t;
                            bestResult = GridPositionDetection.DistCheck(EnemyManager.currentEnemy.transform.position, t.tPos);
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
    /// <summary>
    /// Closest between cover.
    /// </summary>
    /// <returns>Returns closest cover of cover type.</returns>
    /// <param name="point">Point you want to find the closest cover to.</param>
    /// <param name="dir">Which direction the cover is (i.e north would work against target that is south of the point).</param>
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
                        if (GridPositionDetection.DistCheck(point, t.tPos) < bestResult)
                        {
                            currentTile = t;
                            bestResult = GridPositionDetection.DistCheck(point, t.tPos);
                        }
                    }
                }
            }
        }
        return currentTile.thisTile.transform;
    }

    /// <summary>
    /// Does a comparison between a position and the enemy position.
    /// </summary>
    /// <returns> Cover direction the enemy is to the player (useful for finding the right cover to use).</returns>
    /// <param name="cP">current player position (or a position you want to cover against).</param>
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

    /// <summary>
    /// Finds the players in range of the enemy.
    /// </summary>
    /// <returns>If it succeeded in finding players in the enemies view range.</returns>
    /// <param name="range">Range the enemy can see.</param>
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

    /// <summary>
    /// Finds the targets around a point within the range listed.
    /// </summary>
    /// <returns>Whether it succeeded in finding the targets around a point.</returns>
    /// <param name="range">Range in a radius it can see (using tile distance).</param>
    /// <param name="point">Point you want to check around.</param>
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

//    public stateEnum FindEnemies()
//    {
//
//    }
//
//    public stateEnum FindClosestEnemy()
//    {
//
//    }

//    public Group()
//    {
//
//    }
}
