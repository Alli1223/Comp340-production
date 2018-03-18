using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APManager : MonoBehaviour 
{
    private static APManager gActionManage;

    private ManagersManager tManage;

    void Awake()
    {
        if (gActionManage == null)
            gActionManage = this;
            else
                Destroy(this);

        tManage = ManagersManager.manager;
        tManage.tActionManage = gActionManage;
    }

    public static bool HasApPoints(int currentPoints, int actionsPoints)
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

    public void AdjustPoints(PlayerData dataObj, int actionCost)
    {
//        if (side == "p")
//        {
        if (HasApPoints(dataObj.curAP, actionCost))
        {
            dataObj.curAP -= actionCost;
            if (dataObj.curAP == 0)
            {
                tManage.tTurn.RemoveFromAction(dataObj.gameObject);
            }
        }
//        }
//        else if (side == "e")
//        {
//            TempEnemyVar enemyValues = objectAP.GetComponent<TempEnemyVar>();
//            if (HasApPoints(enemyValues.currentAP, actionCost))
//            {
//                enemyValues.currentAP -= actionCost;
//                if (enemyValues.currentAP == 0)
//                {
//                    tManage.tTurn.RemoveFromAction(objectAP.gameObject);
//                }
//            }
//        }

    }

}
