using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionDemo : MonoBehaviour
{
    public DamageDirection fallDir;
    private static Vector3 floor;
    public static Vector3 increaseValues = new Vector3(0, 0.1f, 0);
    public static GameObject attackingPlayer;
    public static GameObject markedBuilding;
    static GameObject player;
    static GameObject building;
    public  GameObject rubble;
    [SerializeField]
    public GameObject smokeOneDemo;
    [SerializeField]
    public GameObject smokeTwoDemo;
    private Vector3 savedBuildingPosEast;
    private Quaternion savedBuildRotEast;

    private Vector3 savedBuildingPosWest;
    private Quaternion savedBuildRotWest;

    private Vector3 savedBuildingPosSouth;
    private Quaternion savedBuildRotSouth;

    private Vector3 savedBuildingPosNorth;
    private Quaternion savedBuildRotNorth;

    private void Start()
    {
        // fallDir = Object.FindObjectOfType<DamageDirection>();
       fallDir = new DamageDirection();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            building = this.gameObject;
            this.transform.position = building.transform.position;
            smokeOneDemo.SetActive(true);
            smokeTwoDemo.SetActive(true);
            StartCoroutine(CameraShake.CamShake());
            StartCoroutine(SpawnRubbleCall());
            savedBuildingPosEast = building.transform.position;
            savedBuildRotEast = building.transform.rotation;
            savedBuildRotEast.eulerAngles = new Vector3(90, savedBuildRotEast.eulerAngles.y, savedBuildRotEast.eulerAngles.z);
            //
            savedBuildingPosNorth = building.transform.position;
            savedBuildRotNorth = building.transform.rotation;
            savedBuildRotNorth.eulerAngles = new Vector3(savedBuildRotNorth.eulerAngles.z, savedBuildRotNorth.eulerAngles.y, 90);
            //
            savedBuildingPosSouth = building.transform.position;
            savedBuildRotSouth = building.transform.rotation;
            savedBuildRotSouth.eulerAngles = new Vector3(-90, savedBuildRotSouth.eulerAngles.y, savedBuildRotSouth.eulerAngles.z);
            //
            savedBuildingPosWest = building.transform.position;
            savedBuildRotWest = building.transform.rotation;
            savedBuildRotWest.eulerAngles = new Vector3(savedBuildRotWest.eulerAngles.z, savedBuildRotWest.eulerAngles.y, -90);
            StartCoroutine(Ticker());
            Object.Destroy(this.gameObject, 3f); //this will be destroy building destroy this is temp
        }
    }

    IEnumerator Ticker()
    {
        while (true)
        {
            Falling();
            yield return new WaitForSeconds(0.04f);
        }
    }
    IEnumerator SpawnRubbleCall()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.6f);
            SpawnRubble();
        }
    }
    void Falling()
    {
        Debug.Log(fallDir.fallDir + "Test");
        if (fallDir.fallDir == Directions.North)
        {
            Debug.Log("north");
            building.transform.position = Vector3.MoveTowards(building.transform.position, new Vector3(savedBuildingPosNorth.x, savedBuildingPosNorth.y - 5f, savedBuildingPosNorth.z), 0.3f * Time.deltaTime);
            building.transform.rotation = Quaternion.RotateTowards(building.transform.rotation, savedBuildRotNorth, 80 * Time.deltaTime);
        }
         else if (fallDir.fallDir == Directions.South)
        {
            Debug.Log("south");
            building.transform.position = Vector3.MoveTowards(building.transform.position, new Vector3(savedBuildingPosSouth.x, savedBuildingPosSouth.y - 5f, savedBuildingPosSouth.z), 0.3f * Time.deltaTime);
            building.transform.rotation = Quaternion.RotateTowards(building.transform.rotation, savedBuildRotSouth, 80 * Time.deltaTime);
        }
        else if (fallDir.fallDir == Directions.East)
        {
            Debug.Log("east");
            building.transform.position = Vector3.MoveTowards(building.transform.position, new Vector3(savedBuildingPosEast.x, savedBuildingPosEast.y - 5f, savedBuildingPosEast.z), 0.3f * Time.deltaTime);
            building.transform.rotation = Quaternion.RotateTowards(building.transform.rotation, savedBuildRotEast, 80 * Time.deltaTime);
        }
        else if (fallDir.fallDir == Directions.West)
        {
            Debug.Log("west");
            building.transform.position = Vector3.MoveTowards(building.transform.position, new Vector3(savedBuildingPosWest.x, savedBuildingPosWest.y - 5f, savedBuildingPosWest.z), 0.3f * Time.deltaTime);
            building.transform.rotation = Quaternion.RotateTowards(building.transform.rotation, savedBuildRotWest, 80 * Time.deltaTime);
        }
       
    }

    private void SpawnRubble() // damage direction not working
    {
            Instantiate(rubble, building.transform.position, Quaternion.identity);
            Instantiate(rubble, new Vector3(building.transform.position.x + 1, building.transform.position.y, building.transform.position.z), Quaternion.identity);
            if (Vector3.Distance(floor, rubble.transform.position) > 0.1f)
                building.transform.localPosition -= increaseValues * Time.deltaTime;
    }

}
