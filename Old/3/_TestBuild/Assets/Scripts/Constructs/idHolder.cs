using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idHolder : MonoBehaviour 
{
    public int pos, currentID, connections;
    public float information_Dist;
    public List<int> information_ID;
    public Individual currentAI;

    public idHolder(int currentIdentifier)
    {
        currentID = currentIdentifier;
    }

    public idHolder(int no, int id, float dist)
    {
        information_ID[no] = id;
        information_Dist += dist;
    }

}
