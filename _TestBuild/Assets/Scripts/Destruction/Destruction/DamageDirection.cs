using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDirection : MonoBehaviour {

    private Transform Buidling;
    private Transform player;
    private Vector3 pos;
    private Vector3 dir;
    public Vector3 FallDir;
    public Vector3 east;
    public Vector3 west;
    public Vector3 north;
    public Vector3 south;

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed left click.");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("You selected the " + hit.transform.tag); // ensure you picked right object
                if (hit.transform.tag == "Cover")
                {
                    attackDirection();
                }
            }
        }
    }

    public Vector3 attackDirection()
    {
        Vector3 pos = Buidling.transform.position;
        Vector3 dirt = (this.transform.position - Buidling.transform.position).normalized;//current mech
        return dirt;
    }

    /// <summary>
    /// Returns a direction based on the two objects entered
    /// Example: ComparitiveDirection(a, b); a = the building or the object you want to modify, b = player or the object you don't want to modify
    /// </summary>
    /// <param name="initialObjPos">The building or object you wish to modify</param>
    /// <param name="secondaryObjPos">The Player or object you wish to modify</param>
    /// <returns></returns>
    coverDirection ComparitiveDirection(Vector3 initialObjPos, Vector3 secondaryObjPos)
    {

        Vector3 dir = (secondaryObjPos - initialObjPos).normalized;//current mech
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
    Vector3 CustomDirection(Vector3 objPosition, coverDirection posDir)
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
            Debug.LogError("posDir was equal to None: " + "A direction that was entered some how is none of the directions of the compass. Send help");
            return Vector3.zero;
        }
        return Vector3.zero;
    }
}
