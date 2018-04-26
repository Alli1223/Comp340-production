using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfCover : MonoBehaviour
{
    [SerializeField]
    private int BuildingHealth;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private ParticleSystem smokeOne;

    void Start()
    {
        currentHealth = BuildingHealth;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "rocket" || col.gameObject.tag == "bullet")
        {
            currentHealth -= 10;
            if (currentHealth <= 0)
            {
                smokeOne.gameObject.SetActive(true);
                Object.Destroy(gameObject, 0.5f);
                GameObject rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rubble.transform.position = new Vector3(0, 0, 0);
                rubble.transform.localScale = new Vector3(1.0f, 0.2f, 1.0f);

            }
        }

    }
}

