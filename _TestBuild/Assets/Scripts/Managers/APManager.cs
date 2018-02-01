using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APManager : MonoBehaviour 
{
    private static APManager gActionManage;

    private ManagersManager tManage;

    void Awake()
    {
        tManage = ManagersManager.manager;
        tManage.tActionManage = gActionManage;
    }

    public bool HasApPoints(int currentPoints, int actionsPoints)
    {
        if (currentPoints >= actionsPoints)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    public void AdjustPoints(Transform objectAP, int actionCost, string side)
    {
        if (side == "p")
        {
            TempPlayerVar playerValues = objectAP.GetComponent<TempPlayerVar>();
            if (HasApPoints(playerValues.currentAP, actionCost))
            {
                playerValues.currentAP -= actionCost;
                if (playerValues.currentAP == 0)
                {
                    tManage.tTurn.RemoveFromAction(objectAP.gameObject);
                }
            }
        }else if (side == "e")
        {
            TempEnemyVar enemyValues = objectAP.GetComponent<TempEnemyVar>();
            if (HasApPoints(enemyValues.currentAP, actionCost))
            {
                enemyValues.currentAP -= actionCost;
                if (enemyValues.currentAP == 0)
                {
                    tManage.tTurn.RemoveFromAction(objectAP.gameObject);
                }
            }
        }

    }

}
