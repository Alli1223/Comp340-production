using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unbreakable : MonoBehaviour {

    [SerializeField]
    private int BuildingHealth;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int DestructionOdds;
    [SerializeField]
    private int RocketDamage;
    [SerializeField]
    private int BulletDamage;
    [SerializeField]
    private ParticleSystem smokeOne;
    private IEnumerator coroutine;

    void Start()
    {
        currentHealth = BuildingHealth;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "rocket")
        {

            Debug.Log("key down o");
            currentHealth -= RocketDamage;
            if (currentHealth <= 0)
            {
                CompleteDestruction();
            }
        }
        else if (col.gameObject.tag == "bullets")
        {
            Debug.Log("key down p");
            currentHealth -= BulletDamage;
            if (currentHealth <= 0)
            {
                CompleteDestruction();
            }

        }
    }

    void CompleteDestruction()
    {
        Debug.Log("Complete");
        Object.Destroy(gameObject, 0.2f);
        GameObject blocks = GameObject.CreatePrimitive(PrimitiveType.Cube);
        blocks.transform.position = new Vector3(0, 0, 0);
        blocks.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 savedPos = gameObject.transform.position;
        blocks.transform.position = savedPos;
    }
}
