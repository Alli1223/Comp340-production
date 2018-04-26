using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionDemo : MonoBehaviour
{
    public static DestructionDemo instance;


    private float randomDir;
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

    private LayerMask coverLay;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        coverLay = LayerMask.GetMask("coverLayer");
        // fallDir = Object.FindObjectOfType<DamageDirection>();
       fallDir = new DamageDirection();
        //Demolish();
    }


    void Update()
    {
//        if (Input.GetKeyUp(KeyCode.X))
////            Demolish();
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500f, coverLay);
            if (hit.transform)
            {
                    Debug.Log("Go");
                    BuildingDemolish(hit.transform.gameObject, PlayerManager.currentMech.gameObject);
            }
        }
    } 

    public void BuildingDemolish(GameObject building, GameObject attackingMech)
    {
        if (building.GetComponent<DestroyableMarker>())
        {
            building.GetComponent<DestroyableMarker>().destroyed = true;
            GameObject smoke = Instantiate(smokeOneDemo, new Vector3(building.transform.position.x, smokeOneDemo.transform.position.y, building.transform.position.z), smokeOneDemo.transform.rotation)as GameObject;
            GameObject smokeTwo = Instantiate(smokeTwoDemo, new Vector3(building.transform.position.x, smokeTwoDemo.transform.position.y, building.transform.position.z), smokeTwoDemo.transform.rotation) as GameObject;
            coverDirection hitDir = DamageDirection.ComparitiveDirection(building.transform.position, attackingMech.transform.position);
            smoke.SetActive(true);
            smokeTwo.SetActive(true);
            StartCoroutine(CameraShake.CamShake());
            StartCoroutine(SpawnRubbleCall(hitDir, building.GetComponent<DestroyableMarker>()));
            StartCoroutine(Ticker(hitDir, building.GetComponent<DestroyableMarker>()));
        }
    }


//        public void Demolish()
//    {
//        building = this.gameObject;
//        this.transform.position = building.transform.position;
//        smokeOneDemo.transform.position = new Vector3(this.transform.position.x, smokeOneDemo.transform.position.y, this.transform.position.z);
//        smokeTwoDemo.transform.position = new Vector3(this.transform.position.x, smokeOneDemo.transform.position.y, this.transform.position.z);
//        smokeOneDemo.SetActive(true);
//        smokeTwoDemo.SetActive(true);
//        StartCoroutine(CameraShake.CamShake());
//        StartCoroutine(SpawnRubbleCall());
//        ranDir();
//        StartCoroutine(Ticker());
//        Object.Destroy(this.gameObject, 4.7f); //this will be destroy building destroy this is temp

//    }

    DamageDirection ranDir()
    {
        randomDir = Random.Range(0.0f, 40.0f);
        if(randomDir >= 0.0f && randomDir < 10.0f)
        {
            fallDir.fallDir = Directions.North;
        }
        if (randomDir >= 10.0f && randomDir < 20.0f)
        {
            fallDir.fallDir = Directions.East;
        }
        if (randomDir >= 20.0f && randomDir < 30.0f)
        {
            fallDir.fallDir = Directions.South;
        }
        if (randomDir >= 30.0f && randomDir < 40.0f)
        {
            fallDir.fallDir = Directions.West;
        }
        return fallDir;
    }

    IEnumerator Ticker(coverDirection direct, DestroyableMarker dMark)
    {
        float i = 10;
        while (i > 0)
        {
            Debug.Log("Oh boy");
            Falling(direct, dMark);
            i -= 0.04f;
            yield return new WaitForSeconds(0.04f);
        }
    }
    IEnumerator SpawnRubbleCall(coverDirection dir, DestroyableMarker dMark)
    {
        bool test = true;
        while (test)
        {
            yield return new WaitForSeconds(3.7f);
            SpawnRubble(dir, dMark);
            test = false;
        }
    }

    void Falling(coverDirection dir, DestroyableMarker build)
    {
        if (dir == coverDirection.North)
        {
            build.transform.position = Vector3.MoveTowards(build.transform.position, new Vector3(build.posNorth.x, build.posNorth.y - 5f, build.posNorth.z), 0.3f * Time.deltaTime);
            build.transform.rotation = Quaternion.RotateTowards(build.transform.rotation, build.rotNorth, 80 * Time.deltaTime);
        }
        else if (dir == coverDirection.South)
        {
            build.transform.position = Vector3.MoveTowards(build.transform.position, new Vector3(build.posSouth.x, build.posSouth.y - 5f, build.posSouth.z), 0.3f * Time.deltaTime);
            build.transform.rotation = Quaternion.RotateTowards(build.transform.rotation, build.rotSouth, 80 * Time.deltaTime);
        }
        else if (dir == coverDirection.East)
        {
            build.transform.position = Vector3.MoveTowards(building.transform.position, new Vector3(build.posEast.x, build.posEast.y - 5f, build.posEast.z), 0.3f * Time.deltaTime);
            build.transform.rotation = Quaternion.RotateTowards(building.transform.rotation, build.rotEast, 80 * Time.deltaTime);
        }
        else if (dir == coverDirection.West)
        {
            build.transform.position = Vector3.MoveTowards(building.transform.position, new Vector3(build.posWest.x, build.posWest.y - 5f, build.posWest.z), 0.3f * Time.deltaTime);
            build.transform.rotation = Quaternion.RotateTowards(building.transform.rotation, build.rotWest, 80 * Time.deltaTime);
        }
       
    }

    private void SpawnRubble(coverDirection dir, DestroyableMarker build) // damage direction not working
    {
        if (dir == coverDirection.North)
        {
            GameObject x = Instantiate(rubble, new Vector3(build.transform.position.x, rubble.transform.position.y, build.transform.position.z), rubble.transform.rotation) as GameObject;
            Instantiate(rubble, new Vector3(build.transform.position.x - 1, rubble.transform.position.y, build.transform.position.z ), Quaternion.identity);
            if (Vector3.Distance(floor, rubble.transform.position) > 0.1f)
                build.transform.localPosition -= increaseValues * Time.deltaTime;
        }
        else if (dir == coverDirection.South)
        {
            GameObject x = Instantiate(rubble, build.transform.position, Quaternion.identity)as GameObject;
            Instantiate(rubble, new Vector3(build.transform.position.x, rubble.transform.position.y, build.transform.position.z - 1), Quaternion.identity);
            if (Vector3.Distance(floor, rubble.transform.position) > 0.1f)
                build.transform.localPosition -= increaseValues * Time.deltaTime;
        }
        else if (dir == coverDirection.East)
        {
            GameObject x = Instantiate(rubble, build.transform.position, Quaternion.identity)as GameObject;
            Instantiate(rubble, new Vector3(build.transform.position.x, rubble.transform.position.y, build.transform.position.z + 1), Quaternion.identity);
            if (Vector3.Distance(floor, rubble.transform.position) > 0.1f)
                build.transform.localPosition -= increaseValues * Time.deltaTime;
        }
        else if (dir == coverDirection.West)
        {
            GameObject x = Instantiate(rubble, build.transform.position, Quaternion.identity)as GameObject;
            Instantiate(rubble, new Vector3(build.transform.position.x + 1, rubble.transform.position.y, build.transform.position.z), Quaternion.identity);
            if (Vector3.Distance(floor, rubble.transform.position) > 0.1f)
                build.transform.localPosition -= increaseValues * Time.deltaTime;
        }
       
    }

}
