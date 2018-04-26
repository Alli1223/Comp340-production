using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct LowerBoundTile
{
    [SerializeField]
    private Tile[] tiles;

    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    public int Width {get {return width;}}
    public int Height {get {return height;}}


    public Tile this[int x,int y]
    {
        get
        {
            return tiles[y * width + x];

        }
        set
        {
            tiles[y * width + x] = value;
        }
    }

    public LowerBoundTile(int width, int height)
    {
        this.width = width;
        this.height = height;

        tiles = new Tile[width * height];
    }
}
