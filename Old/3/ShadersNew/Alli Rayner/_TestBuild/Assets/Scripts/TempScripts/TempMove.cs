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


        modifiedMovDist = PlayerManager.currentPlayer.GetComponent<TempPlayerVar>().currentDist;

        playerTile = GridPositionDetection.GetClosestGrid(PlayerManager.currentPlayer.transform.position, tManage.tGrid.currentTiles);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playersTurn) 
		{
			ShowMovable ();
            playerTile = GridPositionDetection.GetClosestGrid (PlayerManager.currentPlayer.transform.position, tManage.tGrid.currentTiles);
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

        if(moveToPos != null && modifiedMovDist > 0 && PlayerManager.currentPlayer.GetComponent<TempPlayerVar>().currentAP > 0)
		{
            PlayerManager.currentPlayer.transform.position = Vector3.MoveTowards (PlayerManager.currentPlayer.transform.position, new Vector3 (moveToPos.position.x, PlayerManager.currentPlayer.transform.position.y, moveToPos.position.z), 7);

            Transform newCurrentTile = GridPositionDetection.GetClosestGrid (PlayerManager.currentPlayer.transform.position, tManage.tGrid.currentTiles);
				modifiedMovDist = tManage.tDetect.DistModifier (playerTile, newCurrentTile, modifiedMovDist);
            PlayerManager.currentPlayer.GetComponent<TempPlayerVar> ().currentAP -= GridPositionDetection.DistCheck (playerTile.position, moveToPos.position);
				playerTile = newCurrentTile;
				playersTurn = true;
				moveToPos = null;
		}


	}

    public void ShowMovable()
    {
        modifiedMovDist = PlayerManager.currentPlayer.GetComponent<TempPlayerVar>().currentDist;
        playerTile = GridPositionDetection.GetClosestGrid(PlayerManager.currentPlayer.transform.position, tManage.tGrid.currentTiles);
		int range;
        if (modifiedMovDist < PlayerManager.currentPlayer.GetComponent<TempPlayerVar> ().currentAP) 
		{
			range = modifiedMovDist;
		}else
		{
            range = PlayerManager.currentPlayer.GetComponent<TempPlayerVar> ().currentAP;
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
        playerTile = GridPositionDetection.GetClosestGrid(PlayerManager.currentPlayer.transform.position, tManage.tGrid.currentTiles);
        tManage.tDetect.FindTilesInDist (playerTile, tManage.tGrid.tileMeshs, (float)PlayerManager.currentPlayer.GetComponent<TempPlayerVar> ().shootRange);
		foreach (MeshRenderer x in tManage.tGrid.tileMeshs) 
		{
			x.material.color = tManage.actionMat.color;
		}
		enemies.AddRange (GameObject.FindGameObjectsWithTag ("enemy"));
		foreach (GameObject e in enemies) 
		{
            if (GridPositionDetection.DistCheck (PlayerManager.currentPlayer.transform.position, e.transform.position) > PlayerManager.currentPlayer.GetComponent<TempPlayerVar> ().shootRange) 
			{
				e.GetComponent<MeshRenderer> ().material.color = Color.blue;
			} else 
			{
				e.GetComponent<MeshRenderer> ().material.color = Color.red;

			}
		}
	}
}
