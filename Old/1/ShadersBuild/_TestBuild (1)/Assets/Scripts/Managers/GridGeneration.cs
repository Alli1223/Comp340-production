/*
	Created On:		25/09/2017
	Created By: 	Marc Andrews
	Last Edit: 		27/09/2017 10:32
	Last Edit By: 	Marc Andrews
    
*/
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GridGeneration : MonoBehaviour 
{
    public static GridGeneration gridSingle;
	//Makes Grid gen script a singleton
    void Awake()
    {
        if (gridSingle == null)
            gridSingle = this;
        else
            Destroy(this); 

		ManagersManager.manager.tGrid = gridSingle;
        LoadVars();
    }


    public Tile thisShouldShow = new Tile();

    [Range (1,10)]
    public int tileSize = 1;
	[Range(0.1f,1)]
	public float tileHeight = 1;
    [SerializeField]
    public WalkableTileArray tileVariables = new WalkableTileArray(0);
    public List<GameObject> currentTiles;
	public List<MeshRenderer> tileMeshs;
	[SerializeField]
	private Material tile_Tex;

    private float halfSize;
    private GameObject tileHolder;

    private int currentGridNoRef;

    private int objScaleX, objScaleZ;
    private List<GameObject> walkableObjs = new List<GameObject>();
    private List<Tile> coverTiles = new List<Tile>();
    private List<Tile> coverDirTiles = new List<Tile>();


    void Start()
    {
        RevertTileColour();
        TileVarsHolder test = Resources.Load("Assets/Resources/Data/GridData/TileVarsHolder.asset", typeof(TileVarsHolder))as TileVarsHolder;
    }

    /// <summary>
    /// Creates a grid when called.
    /// </summary>
    public void CreateGrid(TileVarsHolder tileHold)
    {
        walkableObjs = new List<GameObject> ();
        halfSize = tileSize;
        halfSize /= 2;

        //

        //Finds all of the objects tagged walkable and puts them into a list
        walkableObjs.AddRange(GameObject.FindGameObjectsWithTag("WalkTer"));
        walkableObjs = walkableObjs.OrderBy(v => v.transform.position.x).ThenByDescending(v => v.transform.position.z).ToList();

        currentGridNoRef = 0;
        objScaleX = 0;
        objScaleZ = 0;
        DimensionScale (walkableObjs);
        tileVariables = new WalkableTileArray(walkableObjs.Count);
//        ,objScaleX,objScaleZ];
//        tileHold.walkTwo = new WalkableTileArray(walkableObjs.Count);



//        tileHold.arrayOTiles = tileVariables;

        //Will gen a grid of tiles for every walkable obj
        foreach (GameObject x in walkableObjs)
        {
            //Adds a Gameobject to hold the grid for each obj
            tileVariables[currentGridNoRef] = new LowerBoundTile(objScaleX, objScaleZ);
            tileHolder = new GameObject ();
            tileHolder.name = "Tile Holder: " + x.name + " GridNoRef: " + currentGridNoRef;
            tileHolder.transform.parent = x.transform;
            currentTiles.Add (tileHolder);
            TileGen(x.transform, tileVariables[currentGridNoRef]);
			x.name = "Floor_Grid ID: " + (1000 + currentGridNoRef);
            currentGridNoRef++;
        } 
        if (tileShown)
        {
            ShowTiles();
            ShowTiles();
        }
        tileHold.walkTwo = tileVariables;
//        tileHold.SetTile(0, 0, 0, thisShouldShow);
    }

    /// <summary>
    /// Generates a grid of tiles on start obj.
    /// </summary>
    /// <param name="startObj">The object that you want to spawn a grid of tiles onto.</param>
    private void TileGen(Transform startObj, LowerBoundTile thisObjGrid)
    {
        //bottom left position (bottom left being the closest point to negative x and z)
        Vector3 objBotLeft = cornerSurfacePoint(startObj);

        int Ypos = 0;
        int Xpos = 0;

		//creates a tile piece for every loop and adds it to current tiles;
		for (int y = 0; y < startObj.localScale.z; y += tileSize)
        {
			for (int x = 0; x < startObj.localScale.x; x += tileSize)
            {
                GameObject nwTilePiece = GameObject.CreatePrimitive(PrimitiveType.Cube);
				nwTilePiece.transform.position = objBotLeft + new Vector3(x, 0, y);
				nwTilePiece.transform.localScale = new Vector3 (tileSize, tileHeight, tileSize);
                nwTilePiece.tag = "gridPiece";              
				nwTilePiece.transform.parent = tileHolder.transform;
				nwTilePiece.GetComponent<MeshRenderer> ().material = tile_Tex;
                nwTilePiece.GetComponent<MeshRenderer>().enabled = tileShown;

				Tile nwTile = new Tile ();
				nwTile.SetPosition (objBotLeft + new Vector3 (x, 0, y));
                nwTile.CurrentGridCoods(currentGridNoRef, Ypos, Xpos);
                nwTile.thisTile = nwTilePiece;
                nwTile.isCover = false;
                Collider[] objectsInRange = Physics.OverlapSphere (nwTile.tPos, halfSize);
				for (int i = 0; i < objectsInRange.Length; i++) 
				{
                    if (!walkableObjs.Contains(objectsInRange[i].gameObject) && objectsInRange[i].tag != "gridPiece" && objectsInRange[i].gameObject.layer == 11)
                    {
                        nwTile.NotWalkable();
                        nwTile.isCover = true;
                        nwTilePiece.SetActive(false);
                    }
				}
                thisObjGrid[Ypos,Xpos] = nwTile;
//                [currentGridNoRef, Ypos, Xpos] = nwTile;
                if (nwTile.isCover)
                {
                    coverTiles.Add(nwTile);
                }
                else
                {
                    nwTile.tileCover = coverDirection.NONE;
                }
                currentTiles.Add(nwTilePiece);
				tileMeshs.Add (nwTilePiece.GetComponent<MeshRenderer> ());
				Ypos++;
            }
            Ypos = 0;
			Xpos++;
        }  
        foreach (Tile t in coverTiles)
        {            
			if ((t.Ypos + 1) < tileVariables [currentGridNoRef].Width) {
				Tile nwerTile = tileVariables [t.currentGrid] [t.Ypos + 1, t.Xpos];
				nwerTile.tileCover = !nwerTile.isCover ? coverDirection.North : coverDirection.NONE;
				if (!nwerTile.isCover) {
					coverDirTiles.Add (nwerTile);
				}
			} else if ((t.Ypos - 1) >= 0) {
				Tile nwerTile = tileVariables [t.currentGrid] [t.Ypos - 1, t.Xpos];
				nwerTile.tileCover = !nwerTile.isCover ? coverDirection.South : coverDirection.NONE;
				if (!nwerTile.isCover) {
					coverDirTiles.Add (nwerTile);
				}
			} else if ((t.Xpos + 1) < tileVariables [currentGridNoRef].Height) {
				Tile nwerTile = tileVariables [t.currentGrid] [t.Ypos, t.Xpos + 1];
				if (nwerTile != null) {
					nwerTile.tileCover = !nwerTile.isCover ? coverDirection.West : coverDirection.NONE;
					if (!nwerTile.isCover) {
						coverDirTiles.Add (nwerTile);
					}
				}
			} else if ((t.Xpos - 1) >= 0) {
				Tile nwerTile = tileVariables [t.currentGrid] [t.Ypos, t.Xpos - 1];
				nwerTile.tileCover = !nwerTile.isCover ? coverDirection.East : coverDirection.NONE;
				if (!nwerTile.isCover) {
					coverDirTiles.Add (nwerTile);
				}
			} else 
			{
				t.tileCover = coverDirection.Building;
			}
        }
//        SaveVars();
    }

    void SaveVars()
    {
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Create(Application.persistentDataPath + "/savedVars.baconAndGravy");
//        bf.Serialize(file, tileVariables);
//        file.Close();
    }

    void LoadVars()
    {
        if (File.Exists(Application.persistentDataPath + "/savedVars.baconAndGravy"))
        {
//            BinaryFormatter bf = new BinaryFormatter();
//            FileStream file = File.Open(Application.persistentDataPath + "/savedVars.baconAndGravy", FileMode.Open);
//            tileVariables = (Tile[,,])bf.Deserialize(file);
//            file.Close();
        }
    }


	/// <summary>
	/// Gets a bottom left point (bottom left being the closest point to negative x and z) at the surface of the object
	/// </summary>
	/// <returns>The closest point to negative x and z at the surface of the object.</returns>
	/// <param name="centredObj">The object you want a bottom left surface point of.</param>
    private Vector3 cornerSurfacePoint(Transform centredObj)
    {
        Vector3 outPoint = centredObj.position;
        outPoint -= centredObj.localScale * 0.5f;
		outPoint += new Vector3 (halfSize, 0, halfSize);
        outPoint.y += centredObj.localScale.y;
        return outPoint;
    }

	/// <summary>
	/// Destroys all tile grids and their parents.
	/// </summary>
    public void DestroyGrid()
    {
		tileMeshs.Clear ();
        coverTiles.Clear();
		GameObject tempTilHold = tileHolder;
		currentTiles.Remove (tileHolder);
		DestroyImmediate(tempTilHold);
		currentTiles.AddRange(GameObject.FindGameObjectsWithTag("gridPiece"));
        tileVariables = new WalkableTileArray(0);
        foreach(GameObject c in currentTiles)
        {
			DestroyImmediate(c);
        }
		currentTiles.Clear ();
    }

	private void DimensionScale(List<GameObject> walkableObjects)
	{
		float highestX = 0;
		float highestZ = 0;
		foreach (GameObject x in walkableObjects) 
		{
			float sizeX = x.transform.localScale.x;
			float sizeZ = x.transform.localScale.z;
			if (sizeX > highestX) 
			{
				highestX = sizeX;
			}
			if (sizeZ > highestZ) 
			{
				highestZ = sizeZ;
			}

		}
		objScaleX = Mathf.RoundToInt(highestX);
		objScaleZ = Mathf.RoundToInt(highestZ);
        Debug.Log(objScaleX + " : " + objScaleZ);
	}

	private bool tileShown; 

	public void ShowTiles() 
	{ 
		tileShown = !tileShown; 
		foreach (MeshRenderer x in tileMeshs) 
		{ 
			if (tileShown) 
			{ 
			x.enabled = true; 
			} 
			else 
			{ 
				x.enabled = false; 
			} 
		}

        foreach (Tile x in coverDirTiles)
        {
            if (x.thisTile != null)
            {
                if (tileShown == true)
                {
                    Material temp = new Material(tile_Tex);
                    if (x.tileCover == coverDirection.North)
                    {                    
                        temp.color = Color.red;
                        x.thisTile.GetComponent<MeshRenderer>().material = temp;
                    }
                    else if (x.tileCover == coverDirection.South)
                    {
                        temp.color = Color.yellow;
                        x.thisTile.GetComponent<MeshRenderer>().material = temp;
                    }
                    else if (x.tileCover == coverDirection.West)
                    {
                        temp.color = Color.cyan;
                        x.thisTile.GetComponent<MeshRenderer>().material = temp;
                    }
                    else if (x.tileCover == coverDirection.East)
                    {
                        temp.color = Color.green;
                        x.thisTile.GetComponent<MeshRenderer>().material = temp;
                    }
                }
                else
                {
                    x.thisTile.GetComponent<MeshRenderer>().material = tile_Tex;
                }
            }
        }
	}

	public void RevertTileColour()
	{
		foreach (MeshRenderer x in tileMeshs) 
		{
			x.material.color = tile_Tex.color;	
		}
	}
}
