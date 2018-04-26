using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableMarker : MonoBehaviour 
{
    public bool destroyed;

    public Vector3 posEast;
    public Quaternion rotEast;

    public Vector3 posWest;
    public Quaternion rotWest;

    public Vector3 posSouth;
    public Quaternion rotSouth;

    public Vector3 posNorth;
    public Quaternion rotNorth;

    void Awake()
    {
        posEast = this.transform.position;
        rotEast = this.transform.rotation;
        rotEast.eulerAngles = new Vector3(90, rotEast.eulerAngles.y, rotEast.eulerAngles.z);

        posNorth = this.transform.position;
        rotNorth = this.transform.rotation;
        rotNorth.eulerAngles = new Vector3(rotNorth.eulerAngles.z, rotNorth.eulerAngles.y, 90);
        //
        posSouth = this.transform.position;
        rotSouth = this.transform.rotation;
        rotSouth.eulerAngles = new Vector3(-90, rotSouth.eulerAngles.y, rotSouth.eulerAngles.z);
        //
        posWest = this.transform.position;
        rotWest = this.transform.rotation;
        rotWest.eulerAngles = new Vector3(rotWest.eulerAngles.z, rotWest.eulerAngles.y, -90);
    }

    void onTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<DestroyableMarker>())
        {
            if (other.gameObject.GetComponent<DestroyableMarker>().destroyed)
            {
                if (destroyed)
                {
                    return;
                }
                DestructionDemo.instance.BuildingDemolish(this.gameObject, other.gameObject);

            }
            else
            {
                DestructionDemo.instance.BuildingDemolish(other.gameObject, this.gameObject);
            }
        }
    }
}
