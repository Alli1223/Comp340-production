using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Clusters : MonoBehaviour 
{
    List<Group> currentClusters;

    public void Add(Group newGroup)
    {
        currentClusters.Add(newGroup);
    }

    public void Remove(Group newGroup)
    {
        currentClusters.Remove(newGroup);
    }

    public void Clear()
    {
        currentClusters.Clear();
    }

    public Group GetGroup(int t)
    {
        if (currentClusters.Count >= t)
        {
            return currentClusters[t];
        }
        else
        {
            Debug.LogError("No such cluster (out of range)");
            return null;
        }
    }

    public Group GetClosestGroup(Vector3 point)
    {
        if (currentClusters.Count == 0)
        {
            Debug.LogError("There is no cluster");
            return null;
        }

        int? identifer = null;
        Vector3 bestResult = new Vector3(20000,20000,20000);
        for (int i = 0; i < currentClusters.Count; i++)
        {
            foreach (Vector3 x in currentClusters[i].individualPos)
            {
                if (point != x)
                {
                    float newPoint = Vector3.Distance(point, x);
                    if (newPoint < Vector3.Distance(bestResult, x))
                    {
                        if (identifer == null)
                        {
                            bestResult = x;
                            identifer = i;
                        }
                        if(Vector3.Distance(currentClusters[i].center, x) < Vector3.Distance(currentClusters[(int)identifer].center, bestResult))
                        {
                            bestResult = x;
                            identifer = i;
                        }
                    }
                }
            }
        }
        return currentClusters[(int)identifer];
    }
}
