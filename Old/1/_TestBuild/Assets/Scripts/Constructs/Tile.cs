using UnityEngine;
using System;

/// <summary>
/// It sucks - Marc.
/// </summary>
[Serializable]
public class Tile 
{
    [SerializeField]
    public GameObject occupyingObj = null;
    [SerializeField]
    public GameObject parent = null;
    [SerializeField]
    public bool isWalkable = false, isOnFire = false, isCover = false;

    [SerializeField]
    public coverDirection tileCover = coverDirection.NONE;

    /// <summary>
    /// The gameobject of this tile.
    /// </summary>
    [SerializeField]
    public GameObject thisTile = null;
    
    /// <summary>
    /// Current tile position.
    /// </summary>
    [SerializeField]
    public Vector3 tPos = Vector3.zero;
    
    /// <summary>
    /// Tile point in cood based on other tiles starting from the closest point to negative x and z. [CurrentObject Number reference, Y position, X position]
    /// </summary>
    [SerializeField]
    public int currentGrid = 0, Ypos = 0, Xpos = 0;


    public void NotWalkable()
    {
        isWalkable = false;
    }
        
    public virtual void NotWalkable(GameObject occupiedObj)
    {
        isWalkable = false;
        if (occupiedObj.layer == 11)
        {
            isCover = true;
        }
        occupyingObj = occupiedObj;
    }

    /// <summary>
    /// Changes the position to it's new position.
    /// </summary>
    /// <param name="newPosition">The new position of the tile.</param>
    public void SetPosition(Vector3 newPosition)
    {
        tPos = newPosition;
    }

    /// <summary>
    /// Changes the array coods applied to this object in reference to the original array.
    /// </summary>
    /// <param name="thisGrid">A number reference to the object the grid is based on.</param>
    /// <param name="Yposition">The z position in 3D space or y in 2d when looking at the grid.</param>
    /// <param name="Xposition">The x position in 2d and 3d space of the object.</param>
    public void CurrentGridCoods(int thisGrid, int Yposition, int Xposition)
    {
        currentGrid = thisGrid;
        Ypos = Yposition;
        Xpos = Xposition;
    }
}
