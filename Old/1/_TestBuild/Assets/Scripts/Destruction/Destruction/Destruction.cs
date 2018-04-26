using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is going to be the script for when the building is destroyed

public class Destruction : MonoBehaviour {

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
	// Make private unless required in other scripts
	[SerializeField]
    private ParticleSystem smokeOne;
    public Vector3 savedBuildPos;
    private int buildingHeight;

    void Start() {
        currentHealth = BuildingHealth;
    }

    void OnCollisionEnter(Collision col)
    { 
       if (col.gameObject.tag == "rocket")//small explosion
        {
            Debug.Log("key down");
            currentHealth -= RocketDamage;
            if (currentHealth <= 0) //large explosions
            {
                DestructionType();
            }
        }
        else if (col.gameObject.tag == "bullets")
        {
            Debug.Log("key down p");
            currentHealth -= BulletDamage;
            if (currentHealth <= 0)
            {
                DestructionType();
            }

        }
    }

    void DestructionType() // this section is used to determine how the building is destroyed, falls to create more cover or completly destroyed
    {
        int DestructionChanceRange = Random.Range(0, 10);
        Debug.Log(DestructionChanceRange);
        if (DestructionChanceRange > DestructionOdds)
        {
             CompleteDestruction();
        }
        else
        {
            StartCoroutine(FallenTower());
        }
    }

    void CompleteDestruction()
    {
        Debug.Log("Complete");
        Object.Destroy(gameObject, 0.2f);
        GameObject Rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Rubble.transform.position = new Vector3(0, 0, 0);
        Rubble.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

	IEnumerator FallenTower()
    {		
		smokeOne.gameObject.SetActive (true);
		yield return new WaitForSeconds (1);
        Debug.Log("Fallen");
		Vector3 savedBuildPos = gameObject.transform.position;
        Object.Destroy(gameObject, 0.2f);
        SpawnRubble();
    }

    void SpawnRubble()
    {
        for (int i = 0; i < buildingHeight; i++)
        {
            GameObject rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rubble.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            rubble.transform.position = savedBuildPos.normalized;
        }
    }

}

