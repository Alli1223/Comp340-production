using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Group : MonoBehaviour 
{
    public Vector3[] individualPos;
    public Vector3 center;
    public List<Individual> groupMembers;
    public int identificationLength;
    public string groupID;

    public Vector3 DensestPoint()
    {
//        CreateGroupList();
        RateDensity(identificationLength);
        int density = 0;
        int currentID = 0;
        foreach (Individual i in groupMembers)
        {
            if (i.densityRating > density)
            {
                density = (int)i.densityRating;
                currentID = i.uniqueID;
            }
        }
        return groupMembers[currentID].myPosition;
    }

//    private void CreateGroupList()
//    {
//        if (groupMembers.Count != 0)
//        {
//            groupMembers.Clear();
//        }
//        int id = 0;
//        groupMembers = new List<Individual>();
//        foreach (Vector3 x in individualPos)
//        {
//            groupMembers = new List<Individual>(individualPos.Length);
//            groupMembers[id].myPosition = x;
//            id++;
//        }
//        identificationLength = id;
//    }

    private void RateDensity(int loopAmount)
    {
        for (int i = 0; i < loopAmount; i++)
        {
            int worstLengthID = 0;
            int bestLengthID = 0;
            Vector3 largestDist = new Vector3(0,0,0);
            Vector3 smallestDist = new Vector3(2000,2000,2000);
            for (int b = 0; b < loopAmount; b++) 
            {
                if(b >= i)
                {
                    if (Vector3.Distance(groupMembers[i].myPosition, groupMembers[b].myPosition) < Vector3.Distance(groupMembers[i].myPosition, smallestDist))
                    {
                        smallestDist = groupMembers[i].myPosition;
                        bestLengthID = i;
                    }
                    else if(Vector3.Distance(groupMembers[i].myPosition, groupMembers[b].myPosition) > Vector3.Distance(groupMembers[i].myPosition, largestDist))
                    {
                        largestDist = groupMembers[i].myPosition;
                        worstLengthID = i;
                    }
                }
            }
            groupMembers[worstLengthID].densityRating -= 1;
            groupMembers[bestLengthID].densityRating += 1;
        }
    }

     
}
