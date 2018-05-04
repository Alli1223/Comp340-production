using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public GameObject builds;
    public GameObject currentBuilding;
    List<GameObject> buildingList;


    void Start()
    {
        buildingList = new List<GameObject>();
        builds = GameObject.FindWithTag("building");
    }

    void FixedUpdate () {
        buildingList.Add(builds);
    }

   /* public static GameObject CurrentBuilding()
    {
		GameObject currBuild = null;
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
					hit.transform.gameObject =  currBuild;
                }
            }
        }
        return currBuild;
    }*/
}
////subnautica/////