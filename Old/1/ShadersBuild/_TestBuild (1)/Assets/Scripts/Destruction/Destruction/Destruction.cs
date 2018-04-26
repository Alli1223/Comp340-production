using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour {

	public static Destruction instance;
	void awake ()
	{

		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
	}
	[SerializeField]
    public static ParticleSystem smokeOne;
    [SerializeField]
    public static  ParticleSystem smokeTwo;
    private static int buildingHeight = 1;
    private static Vector3 floor;
    public static Vector3 increaseValues = new Vector3(0, 0.1f, 0);
	public static GameObject attackingPlayer;
	public static GameObject markedBuilding;
    static GameObject player;
    static GameObject building;
    public static GameObject rubble;

    public void DestructionType (GameObject player, GameObject building) // this section is used to determine how the building is destroyed, falls to create more cover or completly destroyed
    {
            Debug.Log("destructiontype");
            attackingPlayer = player;
            markedBuilding = building;//need to remove at some point
            int DestructionChanceRange = Random.Range(0, 10);
            Debug.Log(DestructionChanceRange);
            if (DestructionChanceRange > 7)
            {
                CompleteDestruction();
            }
            else
            {
                FallenTower();
            }
    }

     void CompleteDestruction()
    {
        Debug.Log("Complete");
        Object.Destroy(markedBuilding, 0.2f); //this will be destroy building destroy this is temp
        smokeOne.gameObject.SetActive(true);
        smokeTwo.gameObject.SetActive(true);
        StartCoroutine(CameraShake.CamShake());
        attackingPlayer = null;
        markedBuilding = null;// set back to null at end
    }

	public void FallenTower()
    {
        Debug.Log("fall");
        Object.Destroy(markedBuilding, 0.2f);
        smokeOne.gameObject.SetActive (true);
        smokeTwo.gameObject.SetActive(true);
        StartCoroutine(CameraShake.CamShake());
        markedBuilding.transform.localScale += new Vector3(0, markedBuilding.transform.localScale.y * 2, 0);
        markedBuilding.transform.eulerAngles = (new Vector3(90, 0, 0)* Time.deltaTime);
        SpawnRubble();
    }


    public  static void SpawnRubble() // damage direction not working
    {
        for (int i = 0; i < buildingHeight; i++)
        {
            Instantiate(rubble, new Vector3(markedBuilding.transform.position.x, markedBuilding.transform.position.y, 0), Quaternion.identity);
            Instantiate(rubble, new Vector3(markedBuilding.transform.position.x + 1, markedBuilding.transform.position.y + 1, 0), Quaternion.identity);
            if (Vector3.Distance(floor, rubble.transform.position) > 0.3f)
                building.transform.localPosition -= increaseValues * Time.deltaTime;
            Debug.Log("rubble");
            
        } 
        attackingPlayer = null;
        markedBuilding = null; 
    }

    //instatntiate gameobject public private orefab rubble
    /*GameObject rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rubble.GetComponent<Renderer>().material.color = Color.black;
            rubble.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (Vector3.Distance(floor, rubble.transform.position) > 0.3f)
                building.transform.localPosition -= increaseValues * Time.deltaTime;*/

}

