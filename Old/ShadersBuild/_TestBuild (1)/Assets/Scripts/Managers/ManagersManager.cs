using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersManager : MonoBehaviour 
{
	public static ManagersManager manager;

	[HideInInspector]
	public TurnManager tTurn;
	[HideInInspector]
	public GridPositionDetection tDetect;
	[HideInInspector]
	public GridGeneration tGrid;
	[HideInInspector]
	public PlayerManager tPlayer;
	[HideInInspector]
	public EnemyManager tEnemyMan;
	[HideInInspector]
    public APManager tActionManage;
	[HideInInspector]
	public CameraManager tCamManage;
	public Material actionMat;

	// Use this for initialization
	void Awake ()
	{
		if (manager == null)
			manager = this;
		else
			DestroyImmediate (this);  
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
