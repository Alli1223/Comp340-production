using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClusterManager 
{
    public static Clusters allClusters;
    public static Clusters playerClusters;
    public static Clusters enemyClusters;

	private static ManagersManager tManage = ManagersManager.manager;


    public static void Reset()
    {
        
    }

    private static void EmptyClusters()
    {
        allClusters.Clear();   
    }

    private static void FillClusters()
    {
        List<GameObject> indObj = new List<GameObject>();
        List<Individual> indivs = new List<Individual>();
        indObj.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        indObj.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));       

        foreach (GameObject x in indObj)
        {
            Individual thisObjInd;
            if (!x.GetComponent<Individual>())
            {
                thisObjInd = x.AddComponent<Individual>();
            }else
            {                
                thisObjInd = x.GetComponent<Individual>();
            }
            indivs.Add(thisObjInd);
			Tile holder = GridExtentions.GetClosestGrid(x.transform.position, tManage.tGrid.tileVariables);
            thisObjInd.myPosition = holder.tPos;
            thisObjInd.enemy = (x.tag == "Enemy");
        }



    }

    private static void FillClustersSort(List<Individual> individualObjs)
    {
        List<Individual> enemies = new List<Individual>();
        List<Individual> players = new List<Individual>();
        foreach (Individual x in individualObjs)
        {
            if (x.enemy)
            {
                enemies.Add(x);
            }
            else
            {
                players.Add(x);
            }
        }


    }

    private static Clusters Grouping(List<Individual> groupItems, side groupID)
    {
        Group grouper;
        List<idHolder> ids = new List<idHolder>();

        foreach (Individual x in groupItems)
        {
            if (groupID == side.Enemy)
            {
                idHolder currentIDHold = new idHolder(x.GetComponent<TempEnemyVar>().uniqueID);
                currentIDHold.pos = groupItems.BinarySearch(x);
                x.uniqueID = currentIDHold.currentID;
                currentIDHold.currentAI = x;
                ids.Add(currentIDHold);
            }
        }

        ids = SortByConnections(ids);
        idHolder[,] groupings = GroupSetter(ids);
        Clusters cA = new Clusters();

        for (int y = 0; y < groupings.GetLength(0); y++)
        {
            Group tempGroup = new Group();
            tempGroup.center = groupings[y, 0].currentAI.myPosition;
            tempGroup.groupID = "Enemy: " + ((y + 4) * 2);

            for (int x = 0; x < groupings.GetLength(1); x++)
            {
                tempGroup.groupMembers.Add(groupings[y, 0].currentAI);
            }
            cA.Add(tempGroup);
        }
        return cA;

//        for (int x = 0; x < ids.Count; x++)
//        {
//            for (int y = 0; y < ids.Count; y++) 
//            {
//                if (!ids[x].information_ID.Contains(ids[y].currentID) && !ids[y].information_ID.Contains(ids[x].currentID))
//                {
//                    int dist = GridPositionDetection.DistCheck(groupItems[ids[x].pos].myPosition, groupItems[ids[y].pos].myPosition);
//                    ids[x].information_ID.Add(ids[y].currentID);
//                    ids[x].information_Dist += dist;
//                    ids[y].information_ID.Add(ids[x].currentID);
//                    ids[y].information_Dist += dist;
//                }
//            }
//        }
    }


    private static List<idHolder> SortByConnections(List<idHolder> curIDs)
    {
        foreach(idHolder x in curIDs)
        {
            ConnectionChecker(x, curIDs);
        }

        List<idHolder> variableAmount = curIDs;
        List<idHolder> sortedIds = new List<idHolder>();

        for (int i = 0; i < curIDs.Count; i++)
        {
            idHolder current = GetLargest(variableAmount);
            sortedIds.Add(current);
            variableAmount.Remove(current);
        }

        return sortedIds;
    }

    private static void ConnectionChecker(idHolder currentID, List<idHolder> items)
    {           
        foreach (idHolder x in items)
        {
            if (x != currentID.currentAI)
            {
                int dist = GridExtentions.DistCheck(currentID.currentAI.myPosition, x.currentAI.myPosition);
                if (dist < GridExtentions.gpd_OuterCircle || dist < GridExtentions.gpd_InnerCircle)
                {
                    currentID.connections++;
                }
            }
        }
    }

    private static idHolder GetLargest(List<idHolder> currIDs)
    {
        int largest = 0;
        idHolder currentBest = null;
        foreach (idHolder x in currIDs)
        {
            if (x.connections > largest)
            {
                currentBest = x;
                largest = x.connections;
            }
        }
        return currentBest;
    }

    private static idHolder[,] GroupSetter(List<idHolder> currIDs)
    {
        List<idHolder> removedList = currIDs;
        idHolder[,] initialGroups = new idHolder[currIDs.Count,currIDs.Count];
        int a = 0;
        for (int first = 0; first < currIDs.Count; first++)
        {  
            int b = 0;
            initialGroups[a, b] = currIDs[first];
            if (removedList.Contains(currIDs[first]))
            {
                removedList.Remove(currIDs[first]);            
                for (int second = 0; second < currIDs.Count; second++)
                {  
                    if (removedList.Contains(currIDs[second]))
                    {
                        int dist = GridExtentions.DistCheck(currIDs[first].currentAI.myPosition, currIDs[second].currentAI.myPosition);
                        if (dist < GridExtentions.gpd_OuterCircle || dist <= GridExtentions.gpd_InnerCircle)
                        {
                            b++;
                            for (int third = 0; third < currIDs.Count; third++)
                            {
                                if (removedList.Contains(currIDs[third]))
                                {
                                    int distance = GridExtentions.DistCheck(currIDs[second].currentAI.myPosition, currIDs[third].currentAI.myPosition);
                                    if (distance <= GridExtentions.gpd_InnerCircle)
                                    {
                                        initialGroups[a, b] = currIDs[third];
                                        b++;
                                    }
                                }
                            }

                            initialGroups[a, b] = currIDs[second];
                            removedList.Remove(currIDs[second]);
                        }
                    }
                }
                a++;
            }
        }

        return initialGroups;
    }

    private static int ComparisionTest(int a, int b)
    {
        int sumInt;
        sumInt = (a - b) / 1000;
        sumInt += b;
        return sumInt;
    }
}
