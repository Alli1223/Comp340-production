using UnityEngine;
using System.Collections;


//rigidbody kinematic
//trigger on 

public class Dominos : MonoBehaviour
{
    private float DomRot = 5.0F;
    private Vector3 DomRotCurrent = new Vector3(0,0,0);//
    private Vector3 DomRotTarget = new Vector3(90,0,0);
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
    //public static int rubbleDamage = 1000; 

    public Vector3 boxRadius;

    private void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Building") || (other.gameObject.name == "rubble(Clone)"))
        {
            Debug.Log("col");
            DominosFall();
            DestroyBuild();
        }
    }
    
    void DominosFall()
    {
        DomRot += Input.GetAxis("Horizontal");
        DomRotCurrent = Vector3.Lerp(DomRotCurrent, DomRotTarget, Time.deltaTime);
        this.transform.eulerAngles = new Vector3(DomRot, 0, 0);
    }

    private void DestroyBuild()
    {
        if (Physics.CheckBox(transform.position, boxRadius))
        {
                Destroy(this.gameObject, 3.0f);
                Debug.Log("dominos");
                StartCoroutine(DominoRubble());
            //smokeThree.gameObject.SetActive(true);
            //smokeFour.gameObject.SetActive(true);
        }
    }

    IEnumerator DominoRubble()
    {
        yield return new WaitForSeconds(2.9f);
        Instantiate(DomRubble, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        if (Vector3.Distance(floor, DomRubble.transform.position) > 0.1f)
            DomRubble.transform.localPosition -= increaseValues * Time.deltaTime;
    }
}



