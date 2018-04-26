﻿using System.Collections;
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
        int layerMask = LayerMask.GetMask("coverCollider");

        int successCount = 0;
        int p1Counter = p1Corners.Count;
        int p2Counter = p2Corners.Count;
        List<Vector3> p1Suc = new List<Vector3>();
        List<Vector3> p2Suc = new List<Vector3>();

        for (int x = 0; x < p1Counter; x++)
        {
            for (int y = 0; y < p2Counter; y++)
            {
                if (p1Corners[x] == p2Corners[y])
                {
                    p1Corners.Remove(p1Corners[x]);
                    p2Corners.Remove(p2Corners[y]);
                    p1Counter -= 1;
                    p2Counter -= 1;
                    if(x>0)
                    x -= 1;
                    if(y>0)
                    y -= 1;
                }
                else
                {
                    float dist = Vector3.Distance(p1Corners[x], p2Corners[y]);
                    if(Physics.Raycast(p1Corners[x], p2Corners[y] - p1Corners[x], dist, layerMask))
                    {
                        p1Suc.Add(p1Corners[x]);
                        p2Suc.Add(p2Corners[y]);
                        successCount++;
                    }
//                    Debug.DrawRay(p1Corners[x], p2Corners[y] - p1Corners[x], Color.red, 200);
                }
            }
        }

        if (successCount >= 10)
        {
            Debug.DrawRay(yourPos, theirPos - yourPos, Color.red, 200);
            return cover.Full;
        }
        else if (successCount >= 4)
        {
            Debug.DrawRay(yourPos, theirPos - yourPos, Color.yellow, 200);
            return cover.Half;
        }
        else
        {
            Debug.DrawRay(yourPos, theirPos - yourPos, Color.blue, 200);
            return cover.None;
        }
    }

    private static List<Vector3> _TileCorners(Vector3 tilePos)
    {
        float height = tilePos.y + 1.5f;
        List<Vector3> tiles = new List<Vector3>(5);
        tiles.Add(new Vector3(tilePos.x + 0.5f, height, tilePos.z + 0.5f));
        tiles.Add(new Vector3(tilePos.x - 0.5f, height, tilePos.z + 0.5f));
        tiles.Add(new Vector3(tilePos.x - 0.5f, height, tilePos.z - 0.5f));
        tiles.Add(new Vector3(tilePos.x + 0.5f, height, tilePos.z - 0.5f));
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
            cover coverAmount = _TileSight (PlayerManager.currentMech.transform.position, target);
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
