using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGarage : MonoBehaviour 
{
    public float sensitivity;

	void Update () 
    {
        if (Input.GetMouseButton(1))
        {
            float inputX = Input.GetAxisRaw("Mouse X");
            float inputY = Input.GetAxisRaw("Mouse Y");
            transform.Rotate(0f, 0f, inputX * sensitivity);
        }
	}
}
