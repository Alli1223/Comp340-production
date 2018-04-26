using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShoot : MonoBehaviour
{
	
    [SerializeField]
    private GameObject bullet;

    private ManagersManager tManage;

    [SerializeField]
    private float force, gunDamage;

    // Use this for initialization
    void Start()
    {
        tManage = ManagersManager.manager;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && tManage.tPlayer.shootingMode && PlayerManager.currentMech.GetComponent<TempPlayerVar>().currentAP >= 6)
        {
            PlayerManager.currentMech.GetComponent<TempPlayerVar> ().currentAP -= 6;
            Ray testRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(testRay, out hit, 500))
            {
				if (hit.transform.tag == "enemy" && hit.transform.GetComponent<MeshRenderer>().material.color == Color.red)
                {
                    Vector3 dir = (hit.transform.position - PlayerManager.currentMech.transform.position);
                    GameObject firedBull = Instantiate(bullet, PlayerManager.currentMech.transform.position, Quaternion.LookRotation(dir));
                    firedBull.GetComponent<TempBullets>().damage = (int)gunDamage;
                    dir.y -= hit.transform.position.y / 8;
					firedBull.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
                }
            }
        }
    }
}
