using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TileVarsHolder : ScriptableObject 
{
    [SerializeField]
    public static TileVarsHolder instance;
    [SerializeField]
    public WalkableTileArray walkTwo = new WalkableTileArray(0);
}
