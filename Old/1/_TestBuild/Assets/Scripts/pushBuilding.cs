using UnityEngine;
using System.Collections;
using System;

public class pushBuilding : MonoBehaviour
{
    void FixedUpdate()
    {

        RaycastHit hit;

        if (Input.GetKey(KeyCode.P))
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 20;
            Debug.DrawRay(transform.position, forward, Color.green);
            if (Physics.Raycast(transform.position, Vector3.forward, out hit))
            {
                Debug.Log("hit object", gameObject);
                //Debug.Log(hit.collider.gameObject.name);
                //hit.collider.gameObject.transform.rotation.Set(0, 90, 0, 0);
                if(hit.collider.gameObject.name == "Building")
                {
                    Debug.Log(hit.collider.gameObject.name);
                    hit.collider.gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
                }
            }
        }
    }

}


    