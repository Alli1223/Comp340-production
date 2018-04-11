using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour 
{
    //1.414 

    private float distMovable = 6;
    private float oneDistSize = 1;

    private Vector3 startingPoint;

    GameObject tile;


	// Use this for initialization
	void Start () 
	{
        startingPoint = this.transform.position;
        tile = GameObject.CreatePrimitive(PrimitiveType.Cube);

        for (float x = -distMovable; x < distMovable + 1; x++)
        {
            for (float z = -distMovable; z < distMovable + 1; z++)
            {
                float xT;
                float zT;
                float sqrMulti = 0;
                Vector3 positionHolder = new Vector3(x, startingPoint.y, z);
                xT = positionHolder.x - startingPoint.x;
                if (xT != 0)
                {
                    zT = positionHolder.z - startingPoint.z;
                    if (zT != 0)
                    {
                        sqrMulti = Vector3.Distance(startingPoint, new Vector3(xT,startingPoint.y,zT));
//                        sqrMulti = sqrMulti / 1.414f;
                        if(sqrMulti >= -distMovable && sqrMulti <= distMovable)
                            tile = Instantiate(tile, startingPoint + positionHolder, Quaternion.identity)as GameObject; 
                    }
                    else
                    {
                        
                        sqrMulti = 200;
                    }
                }
                else
                {
                    sqrMulti = 200;
                }
                if(sqrMulti != 0)
                {
                    
                }
            }
        }
        Debug.Log(Vector3.Distance(new Vector3(50,1,6), new Vector3(52,1,6)));
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
