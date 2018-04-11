using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour 
{
	public static EnemyManager gEnemyMan;
	public static GameObject currentEnemy;

	private List<GameObject> currentEnemies = new List<GameObject>();
	private ManagersManager tManage;

	private EnemyMove tAIMove;
	private int distMoved = 6;

	//Makes Grid gen script a singleton
	void Awake()
	{
		if (gEnemyMan == null)
			gEnemyMan = this;
		else
			Destroy (this); 
		
		tManage = ManagersManager.manager;
		tManage.tEnemyMan = gEnemyMan;

		currentEnemies.AddRange (GameObject.FindGameObjectsWithTag ("enemy"));
		currentEnemy = currentEnemies [0];


		foreach (GameObject x in currentEnemies) 
		{
			if (!x.GetComponent<TempEnemyVar> ()) 
			{
				x.AddComponent<TempEnemyVar>();
			}
			x.GetComponent<TempEnemyVar> ().moveDist = 6;
            x.GetComponent<TempEnemyVar> ().currentAP = 10;

		}
        EnemyUniqueIDAssignment();
	}
	bool addedToAction = false;
	// Update is called once per frame
	void Update () 
	{
		if (!tManage.tTurn.playersTurn) 
		{
			if (!addedToAction) 
			{
                tManage.tTurn.AddToAction (currentEnemies);
				foreach (GameObject x in currentEnemies) 
				{
					x.GetComponent<TempEnemyVar> ().moveDist = 6;
                    x.GetComponent<TempEnemyVar>().currentAP = 10;
				}
				addedToAction = true;
			}

            if (currentEnemy.GetComponent<TempEnemyVar>().moveDist == 0 || distMoved == 0 || currentEnemy.GetComponent<TempEnemyVar>().currentAP <= 3 && GetComponent<EnemyMove>().GetClosestPlayerDist(currentEnemy.transform) <= 1 || currentEnemy.GetComponent<TempEnemyVar>().currentAP == 0)
            {
                if (tManage.tTurn.waitForAction.Count == 1)
                {
                    currentEnemy.GetComponent<TempEnemyVar> ().moveDist = 6;
                    currentEnemy.GetComponent<TempEnemyVar>().currentAP = 10;
                    tManage.tTurn.RemoveFromAction(currentEnemy);
                }
                else
                {	
                    currentEnemy.GetComponent<TempEnemyVar> ().moveDist = 6;
                    currentEnemy.GetComponent<TempEnemyVar>().currentAP = 10;
                    tManage.tTurn.RemoveFromAction(currentEnemy);
                    currentEnemy = SelectNewEnemy(tManage.tTurn.waitForAction, currentEnemies);
                    Debug.Log(currentEnemy.name);
                }
            }
            else if (currentEnemy.GetComponent<TempEnemyVar>().currentAP >= 7)
            {
                if (Random.Range(1, 101) <= 16 || GetComponent<EnemyMove>().GetClosestPlayerDist(currentEnemy.transform) <= 3 && Random.Range(1, 101) <= 75)
                {
                    GetComponent<EnemyMove>().Shoot();
                }
            }
            else 
			{
				GetComponent<EnemyMove> ().Move ();
                if (currentEnemy.GetComponent<TempEnemyVar>().currentAP <= 3)
                {
                    if (tManage.tTurn.waitForAction.Count == 1)
                    {
                        currentEnemy.GetComponent<TempEnemyVar> ().moveDist = 6;
                        currentEnemy.GetComponent<TempEnemyVar>().currentAP = 10;
                        tManage.tTurn.RemoveFromAction(currentEnemy);
                    }
                    else
                    {
                        currentEnemy.GetComponent<TempEnemyVar> ().moveDist = 6;
                        currentEnemy.GetComponent<TempEnemyVar>().currentAP = 10;
                        tManage.tTurn.RemoveFromAction(currentEnemy);
                        currentEnemy = SelectNewEnemy(tManage.tTurn.waitForAction, currentEnemies);
                    }
                }
			}
		} else 
		{
			addedToAction = false;
		}
	}

	private GameObject SelectNewEnemy(List<GameObject> queuedObjs, List<GameObject> allObjs)
	{
		List<GameObject> needAction = new List<GameObject> ();
		GameObject newEnemy;

		for (int i = 0; i < queuedObjs.Count; i++) 
		{
			for(int x = 0; x < allObjs.Count; x++)
			{
				if (queuedObjs[i] == allObjs[x]) 
				{
					needAction.Add (allObjs[x]);
				}
			}
		}

		if (needAction.Count > 1) 
		{
			newEnemy = needAction [Random.Range (0, needAction.Count - 1)];
		} else
		{
			newEnemy = needAction [0];
		}
		return newEnemy;
	}

    private void EnemyUniqueIDAssignment()
    {
        if (currentEnemies.Count == 0)
        {
            return;
        }
        int idNumerator = 0;
        foreach (GameObject x in currentEnemies)
        {
            int uniqueID = (Random.Range(1, 9) * 1000) + idNumerator;
            x.GetComponent<TempEnemyVar>().uniqueID = uniqueID;
            x.name = "Enemy (ID:" + uniqueID + ")";
            idNumerator++;
        }
    }

}
