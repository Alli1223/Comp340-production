using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingDestruction : MonoBehaviour
{

    public int buildingHealth = 100;
    public int currentHealth;
    public int CrumbleSpeed = 10;
    public int bulletDamage = 2; //just for know damage for weapons wil not be defined here
    public int rocketDamage = 25;//just for know damage for weapons wil not be defined here
    bool noHealth;
    bool isDestroyed;
    bool hasCrumbled;

    // Use this for initialization
    void Awake()
    {
        currentHealth = buildingHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            for (int i = 1; i <= 10; i++)
            {
                //transform.localScale -= new Vector3(0.01F, 0.3f, 0);
                if (i == 9)
                {
                    Destroy(gameObject, 3f);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            currentHealth -= bulletDamage;
        }
        if (collision.gameObject.tag == "rocket")
        {
            currentHealth -= rocketDamage;
        }
    }
    
    /*

    void spawnRubble()
    {
        for (int i = 1; i <= 5; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(0, 0.5F, 0);
            cube.transform.localScale = new Vector3(0.2f, 0.5F, 0.2f);
        }
    }*/
}

