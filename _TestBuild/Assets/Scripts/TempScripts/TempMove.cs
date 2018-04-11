using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMove : MonoBehaviour 
{
	private ManagersManager tManage;
	Transform moveToPos;
	Transform playerTile;

	private bool playersTurn;

	public int modifiedMovDist;

	// Use this for initialization
	void Start () 
	{		
		tManage = ManagersManager.manager;


        modifiedMovDist = PlayerManager.currentMech.GetComponent<TempPlayerVar>().currentDist;

        playerTile = GridExtentions.GetClosestGrid(PlayerManager.currentMech.transform.position, tManage.tGrid.currentTiles);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playersTurn) 
		{
			ShowMovable ();
            playerTile = GridExtentions.GetClosestGrid (PlayerManager.currentMech.transform.position, tManage.tGrid.currentTiles);
			playersTurn = false;
		}
		if (!tManage.tPlayer.shootingMode) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray testRay = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (testRay, out hit, 500)) 
				{
					if (hit.transform.GetComponent<MeshRenderer> ().enabled == true && hit.transform.tag == "gridPiece") 
					{
						moveToPos = hit.transform;
					}
				}
			}
				
		}

        if(moveToPos != null && modifiedMovDist > 0 && PlayerManager.currentMech.GetComponent<TempPlayerVar>().currentAP > 0)
		{
            PlayerManager.currentMech.transform.position = Vector3.MoveTowards (PlayerManager.currentMech.transform.position, new Vector3 (moveToPos.position.x, PlayerManager.currentMech.transform.position.y, moveToPos.position.z), 7);

            Transform newCurrentTile = GridExtentions.GetClosestGrid (PlayerManager.currentMech.transform.position, tManage.tGrid.currentTiles);
				modifiedMovDist = tManage.tDetect.DistModifier (playerTile, newCurrentTile, modifiedMovDist);
            PlayerManager.currentMech.GetComponent<TempPlayerVar> ().currentAP -= GridExtentions.DistCheck (playerTile.position, moveToPos.position);
				playerTile = newCurrentTile;
				playersTurn = true;
				moveToPos = null;
		}


	}

    public void ShowMovable()
    {
        modifiedMovDist = PlayerManager.currentMech.GetComponent<TempPlayerVar>().currentDist;
        playerTile = GridExtentions.GetClosestGrid(PlayerManager.currentMech.transform.position, tManage.tGrid.currentTiles);
		int range;
        if (modifiedMovDist < PlayerManager.currentMech.GetComponent<TempPlayerVar> ().currentAP) 
		{
			range = modifiedMovDist;
		}else
		{
            range = PlayerManager.currentMech.GetComponent<TempPlayerVar> ().currentAP;
		}
		tManage.tDetect.FindTilesInDist(playerTile, tManage.tGrid.tileMeshs, (float)range);
		if (tManage.tGrid.tileMeshs [0].material.color == tManage.actionMat.color) 
		{
			tManage.tGrid.RevertTileColour ();
			foreach (GameObject x in enemies) 
			{
				x.GetComponent<MeshRenderer> ().material.color = Color.red;
			}
		}
    }

	List<GameObject> enemies = new List<GameObject> ();
	public void ShootMode()
	{
        playerTile = GridExtentions.GetClosestGrid(PlayerManager.currentMech.transform.position, tManage.tGrid.currentTiles);
        tManage.tDetect.FindTilesInDist (playerTile, tManage.tGrid.tileMeshs, (float)PlayerManager.currentMech.GetComponent<TempPlayerVar> ().shootRange);
		foreach (MeshRenderer x in tManage.tGrid.tileMeshs) 
		{
			x.material.color = tManage.actionMat.color;
		}
		enemies.AddRange (GameObject.FindGameObjectsWithTag ("enemy"));
		foreach (GameObject e in enemies) 
		{
            if (GridExtentions.DistCheck (PlayerManager.currentMech.transform.position, e.transform.position) > PlayerManager.currentMech.GetComponent<TempPlayerVar> ().shootRange) 
			{
				e.GetComponent<MeshRenderer> ().material.color = Color.blue;
			} else 
			{
				e.GetComponent<MeshRenderer> ().material.color = Color.red;

			}
		}
	}
}
