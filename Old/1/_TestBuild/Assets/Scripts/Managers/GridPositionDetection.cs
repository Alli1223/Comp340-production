/*
	Created On:		27/09/2017 10:34
	Created By: 	Marc Andrews
	Last Edit: 		28/09/2017 15:26
	Last Edit By: 	Marc Andrews
*/
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GridPositionDetection : MonoBehaviour
{
    private static ManagersManager tManage;
	private Transform currentPozGrid;
	//Should be moved to a turn manager later
	[HideInInspector]
	public bool gridRefresh;

	public static GridPositionDetection gridDetect;
    
	//Grabs player manager and the grid generator for the current tiles
	void Awake()
    {
		if (gridDetect == null)
			gridDetect = this;
		else
			Destroy(this);


		tManage = ManagersManager.manager;
		tManage.tDetect = gridDetect;
		//currentPozGrid = GetClosestGrid(tPlayer.player.transform, tGrid.currentTiles);
	}

	/// <summary>
	/// Gets the closest grid to the player/units current position.
	/// </summary>
	/// <returns>The closest grid.</returns>
	/// <param name="startPos">Player/units current position.</param>
	/// <param name="grids">The list of grid pieces currently in the level.</param>
    public Transform GetClosestGrid(Vector3 startPos, List<GameObject> grids)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = startPos;
        foreach (GameObject g in grids)
        {
			
			Transform t = g.transform.tag == "gridPiece" ? g.transform : null;
			if (t) 
			{
				float dist = Vector3.Distance (t.position, currentPos);
				if (dist < minDist) 
				{
					tMin = t;
					minDist = dist;
				}
			}
        }
        return tMin;
    }

	/// <summary>
	/// Finds the tiles within a radius around a player and eneables their mesh renderer.
	/// </summary>
	/// <param name="playerTileLocal">The tile beneath the player.</param>
	/// <param name="tilesInGame">Mesh Renderer of all tiles in the level.</param>
	/// <param name="dist">The distance the player can travel (Make sure it is a float).</param>
	public void FindTilesInDist(Transform playerTileLocal,  List<MeshRenderer> tilesInGame, float dist)
	{
		foreach (MeshRenderer b in tilesInGame) 
		{
			Transform tile = b.transform;
			float xAxis = playerTileLocal.position.x;
			float zAxis = playerTileLocal.position.z;

			//
			if((Mathf.RoundToInt(Mathf.Abs(tile.position.x - xAxis)) + Mathf.RoundToInt(Mathf.Abs(tile.position.z - zAxis))) <= dist)
			{
				b.enabled = true;
			} 
			else
			{
				b.enabled = false;
			}

		}
		gridRefresh = false;
	}

	/// <summary>
	/// Returns the tiles within the given dist.
	/// </summary>
	/// <returns>The tiles in dist.</returns>
	/// <param name="enemyTileLocal">The tile beneath the enemy.</param>
	/// <param name="tilesInGame">Mesh Renderer of all tiles in the level.</param>
	/// <param name="dist">The distance the object can still travel (Make sure it is an int).</param>
	public virtual List<Transform> FindTilesInDist(Transform enemyTileLocal,  List<MeshRenderer> tilesInGame, int dist)
	{
		List<Transform> tilesInDist = new List<Transform> ();

		foreach (MeshRenderer b in tilesInGame) 
		{
			Transform tile = b.transform;
			float xAxis = enemyTileLocal.position.x;
			float zAxis = enemyTileLocal.position.z;

			//
			if((Mathf.RoundToInt(Mathf.Abs(tile.position.x - xAxis)) + Mathf.RoundToInt(Mathf.Abs(tile.position.z - zAxis))) <= dist)
			{
				tilesInDist.Add (b.transform);
			} 

		}
		return tilesInDist;
	}

    public static List<GameObject> _CloseObjs(List<GameObject> objs, Vector3 initalPos, int range)
    {
        List<GameObject> objInRange = new List<GameObject> ();

        foreach (GameObject a in objs) 
        {
            if (gridDetect.DistCheck(a.transform.position, initalPos) <= range)
            {
                objInRange.Add(a);
            }
        }
        return objInRange;
    }

	//To be modified
	public int DistModifier(Transform startTile, Transform currentTile, int currentDist)
	{
		int newDist = currentDist;
		newDist -= (Mathf.RoundToInt(Mathf.Abs(currentTile.position.x - startTile.position.x)) + Mathf.RoundToInt(Mathf.Abs(currentTile.position.z - startTile.position.z)));
		return newDist;
	}

    public int DistCheck(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 newPosStart = GetClosestGrid(startPoint, tManage.tGrid.currentTiles).transform.position;
        Vector3 newPosEnd = GetClosestGrid(endPoint, tManage.tGrid.currentTiles).transform.position;
        return (Mathf.RoundToInt(Mathf.Abs(newPosEnd.x - newPosStart.x)) + Mathf.RoundToInt(Mathf.Abs(newPosEnd.z - newPosStart.z)));
    }

    public bool IsInRange(Vector3 startPoint, Vector3 endPoint, int range)
    {
        if (DistCheck(startPoint, endPoint) <= range)
        {
            return true;
        }else
        {
            return false;
        }
    }

	/// <summary>
	/// Returns the closest player Realtive to the enemy.
	/// </summary>
	/// <returns>The position of the player.</returns>
	/// <param name="players">Players in the scene.</param>
	/// <param name="enemy">Current Enemy.</param>
	public Vector3 RealtivePosition(List<GameObject> players, Transform enemy)
	{
		float distToPlayer = Mathf.Infinity;
        Transform enemyTilePos = GetClosestGrid (enemy.position, tManage.tGrid.currentTiles);
		Transform closestPlayer = players[Random.Range(0,players.Count)].transform;
		foreach (GameObject x in players) 
		{
            Transform xTile = GetClosestGrid (x.transform.position, tManage.tGrid.currentTiles);
			float tempDist = Vector3.Distance (xTile.position, enemyTilePos.position);
			if(tempDist < distToPlayer)
			{
				distToPlayer = tempDist;
				closestPlayer = x.transform;
			}
		}
        closestPlayer = GetClosestGrid (closestPlayer.position, tManage.tGrid.currentTiles);
		return closestPlayer.transform.position;
	}

	/// <summary>
	/// Finds the closest object realtive to a position.
	/// </summary>
	/// <returns>The move position.</returns>
	/// <param name="objects">A generic list of Transforms.</param>
	/// <param name="closestPoint">Closest point to the object.</param>
	public virtual Transform RealtivePosition(List<Transform> objects, Vector3 closestPoint)
	{
        if (objects.Count == 0)
        {
            Debug.LogError("GridPositionDetection: Detected empty list");
            return null;
        }
		float distToObj = Mathf.Infinity;
        Transform closestObj = null;
		foreach (Transform x in objects) 
		{
			if (x.transform.position != closestPoint) 
			{
				float tempDist = Vector3.Distance (x.transform.position, closestPoint);
				if (tempDist < distToObj) 
				{
					distToObj = tempDist;
					closestObj = x.transform;
				}
			}
		}
		return closestObj.transform;
	}

    public virtual Vector3? RealtivePosition(List<GameObject> objects, Vector3 closestPoint)
    {
        if (objects.Count == 0)
        {
            Debug.LogError("GridPositionDetection: Detected empty list");
            return null;
        }
        float distToObj = Mathf.Infinity;
        Transform closestObj = null;
        foreach (GameObject x in objects) 
        {
            //          Transform tempCast = x as Transform;
            if (x.transform.position != closestPoint) 
            {
                float tempDist = Vector3.Distance (x.transform.position, closestPoint);
                if (tempDist < distToObj) 
                {
                    distToObj = tempDist;
                    closestObj = x.transform;
                }
            }
        }
        return closestObj.transform.position;
    }
}
