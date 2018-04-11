﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour 
{
    public bool playersTurn = true;

    public static TurnManager gTurn;
    private PlayerManager tPlayer;

	public List<GameObject> waitForAction 
	{
		get 
		{
			return waitingForAction;
		}
	}
	private List<GameObject> waitingForAction = new List<GameObject>();


    //Makes Grid gen script a singleton
    void Awake()
    {
        if (gTurn == null)
            gTurn = this;
        else
            Destroy(this); 

        tPlayer = PlayerManager.gPlayer;
    }

	// Use this for initialization
	void Start () 
	{
        
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (waitingForAction.Count == 0) 
		{
			playersTurn = !playersTurn;
			Debug.Log ("Oh");
		}
	}

	public void AddToAction(GameObject startObj)
	{
			waitingForAction.Add (startObj);
	}

	public void AddMultipleToAction(List<GameObject> startObjs)
	{
		waitingForAction.AddRange (startObjs);
	}

	public virtual void AddMultipleToAction(GameObject[] startObjs)
	{
		waitingForAction.AddRange (startObjs);
	}

	public void RemoveFromAction(GameObject finObj)
	{
		waitingForAction.Remove (finObj);
	}

}