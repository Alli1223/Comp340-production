using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineOfSightFunctions 
{
    public static cover _TileSight(Vector3 yourPos, Vector3 theirPos)
    {
        Vector3 p1 = new Vector3(yourPos.x, 0, yourPos.z);
        List<Vector3> p1Corners = _TileCorners(p1);
        Vector3 p2 = new Vector3(theirPos.x, 0, theirPos.z);
        List<Vector3> p2Corners = _TileCorners(p2);

        int successCount = 0;
        for (int x = 0; x < p1Corners.Count; x++)
        {
            for (int y = 0; y < p2Corners.Count; y++)
            {
                if (p1Corners[x] == p2Corners[y])
                {
                    p1Corners.Remove(p1Corners[x]);
                    p2Corners.Remove(p1Corners[y]);
                }
                else
                {
                    if(!Physics.Raycast(p1Corners[x], p1Corners[x] - p2Corners[y], GridPositionDetection.gridDetect.DistCheck(p1Corners[x], p2Corners[y]), 11))
                    {
                        successCount++;
                    }
                }
            }
        }

        if (successCount >= 10)
        {
            return cover.Full;
        }
        else if (successCount >= 4)
        {
            return cover.Half;
        }
        else
        {
            return cover.None;
        }
    }

    private static List<Vector3> _TileCorners(Vector3 tilePos)
    {
        List<Vector3> tiles = new List<Vector3>();
        tiles[0] = new Vector3(tilePos.x + 0.5f, 0, tilePos.z + 0.5f);
        tiles[1] = new Vector3(tilePos.x - 0.5f, 0, tilePos.z + 0.5f);
        tiles[2] = new Vector3(tilePos.x - 0.5f, 0, tilePos.z - 0.5f);
        tiles[3] = new Vector3(tilePos.x + 0.5f, 0, tilePos.z - 0.5f);
        return tiles;            
    }


	public static bool _HasLineOfSight(side pE, Vector3 target)
	{
		if (pE == side.Enemy) 
        {
			cover coverAmount = _TileSight (EnemyManager.currentEnemy.transform.position, target);
			if (coverAmount == cover.Half || coverAmount == cover.None) 
			{
				return true;
			} else 
			{
                return false;
			}
		} else 
        {
            cover coverAmount = _TileSight (PlayerManager.currentPlayer.transform.position, target);
            if (coverAmount == cover.Half || coverAmount == cover.None) 
            {
                return true;
            } else 
            {
                return false;
            }
		}
	}
}
