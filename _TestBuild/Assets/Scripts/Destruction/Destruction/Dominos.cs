using UnityEngine;
using System.Collections;


//rigidbody kinematic
//trigger on 
//destruction demo on building
//dominos on building
//set smoke effects

public class Dominos : MonoBehaviour
{
    public DamageDirection fallDir;
    private static Vector3 floor;
    public static Vector3 increaseValues = new Vector3(0, 0.1f, 0);
    public static GameObject attackingPlayer;
    public static GameObject markedBuilding;
    static GameObject player;
    static GameObject building;
    public GameObject DomRubble;
    [SerializeField]
    public static ParticleSystem smokeThree;
    [SerializeField]
    public static ParticleSystem smokeFour;
    [SerializeField]
    public static ParticleSystem RubbleSmokeOne;
    [SerializeField]
    public static ParticleSystem RubbleSmokeTwo;
    //public static int rubbleDamage = 1000;
    public Vector3 boxRadius;
   // private float DomRot = 5.0F;
   // private Vector3 DomRotCurrent = new Vector3(0, 0, 0);//
   // private Vector3 DomRotTarget = new Vector3(90, 0, 0);
    float x;
    float y;

    private void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Building") || (other.gameObject.name == "rubble(Clone)"))
        {
            Debug.Log("col");
            StartCoroutine(DominosFall());
            DestroyBuild();
        }
    }
    
    IEnumerator DominosFall()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("fall");
        x += Time.deltaTime * 100;
        this.transform.rotation = Quaternion.Euler(0, 0, -25);
       // y -= Time.deltaTime * 1;
        //this.transform.position = Vector3 (0, y, 0);
    }

    private void DestroyBuild()
    {
        if (Physics.CheckBox(transform.position, boxRadius))
        {
                Destroy(this.gameObject, 1.3f);
                Debug.Log("dominos");
                StartCoroutine(DominoRubble());
            //smokeThree.gameObject.SetActive(true);
            //smokeFour.gameObject.SetActive(true);
        }
    }

    IEnumerator DominoRubble()
    {
        yield return new WaitForSeconds(1.0f);
        //RubbleSmokeOne.gameObject.SetActive(true);
        //RubbleSmokeTwo.gameObject.SetActive(true);
        Instantiate(DomRubble, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        if (Vector3.Distance(floor, DomRubble.transform.position) > 0.1f)
            DomRubble.transform.localPosition -= increaseValues * Time.deltaTime;
    }
}



