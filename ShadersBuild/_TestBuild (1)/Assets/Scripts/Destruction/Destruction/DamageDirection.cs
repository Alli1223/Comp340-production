using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDirection : MonoBehaviour {

    private Transform currentBuidling;
    private GameObject currentPlayer;
    private Vector3 pos;
    private Vector3 dir;
    public static Vector3 east;
    public static Vector3 west;
    public static Vector3 north;
    public static Vector3 south;
    public Vector3 fallDir = Directions.West;

    public static Vector3 attackDirection()
    {
        Vector3 fallDir = Vector3.zero;
		fallDir = CustomDirection(Destruction.markedBuilding.transform.position, ComparitiveDirection(Destruction.markedBuilding.transform.position, Destruction.attackingPlayer.transform.position));
        return fallDir;
    }

    /// <summary>
    /// Returns a direction based on the two objects entered
    /// Example: ComparitiveDirection(a, b); a = the building or the object you want to modify, b = player or the object you don't want to modify
    /// </summary>
    /// <param name="initialObjPos">The building or object you wish to modify</param>
    /// <param name="secondaryObjPos">The Player or object you wish to modify</param>
    /// <returns></returns>
    static coverDirection ComparitiveDirection(Vector3 initialObjPos, Vector3 secondaryObjPos)
    {

        Vector3 dir = (secondaryObjPos - initialObjPos);//current mech
        coverDirection fallDir = coverDirection.NONE;

        if (Mathf.Abs(secondaryObjPos.x - initialObjPos.x) < Mathf.Abs(secondaryObjPos.z - initialObjPos.z))
        {
            if (initialObjPos.z < dir.z)
            {
                fallDir = coverDirection.East;
            }
            else//consider equal to as well
            {
                fallDir = coverDirection.West;//fall the other way
            }
        } else
        {
            if (initialObjPos.x > dir.x)
            {
                fallDir = coverDirection.North;//fall the other way
            }
            else
            {
                fallDir = coverDirection.South;//fall the other way
            }
        }
        return fallDir; 
    }

    /// <summary>
    /// Input a objects position and a direction to get a position  plus one tile in that direction
    /// Example: CustomDirection(Vector3.One, coverDirection.North);
    /// Example2: CustomDirection(a, ComparitiveDirection(a, b));
    /// </summary>
    /// <param name="objPosition">The position of the object you want an adjusted position of</param>
    /// <param name="posDir">The direction you want the object to move to</param>
    /// <returns></returns>
    static Vector3 CustomDirection(Vector3 objPosition, coverDirection posDir)
    {
        if (posDir == coverDirection.North)
        {
            return objPosition += Directions.North;
        }
        if (posDir == coverDirection.East)
        {
            return objPosition += Directions.East;
        }
        if (posDir == coverDirection.South)
        {
            return objPosition += Directions.South;
        }
        if (posDir == coverDirection.West)
        {
            return objPosition += Directions.West;
        }
        if (posDir == coverDirection.NONE)
        {
            Debug.LogError("posDir was equal to None: " + "A direction that was entered is none of the directions of the compass.");
            return Vector3.zero;
        }
        return Vector3.zero;
    }
}
