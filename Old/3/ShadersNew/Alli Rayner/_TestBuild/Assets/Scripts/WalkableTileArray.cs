using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public struct WalkableTileArray
{ 
    [SerializeField]
    private LowerBoundTile[] objGrids;

    [SerializeField]
    private int width;

    public int Width {get {return width;}}


    public LowerBoundTile this[int x]
    {
        get
        {
            return objGrids[x];

        }
        set
        {
            objGrids[x] = value;
        }
    }

    public WalkableTileArray(int width)
    {
        this.width = width;
        objGrids = new LowerBoundTile[width];
    }
}


