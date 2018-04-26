using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBullets : MonoBehaviour 
{
    [HideInInspector]
    public int damage;

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(DestroyBull());
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "enemy")
        {            
            other.gameObject.GetComponent<TempHealthSystem>().Damage(damage);
        }
        Destroy(gameObject);
    }

    IEnumerator DestroyBull()
    {
        yield return new WaitForSecondsRealtime(5f);
        Destroy(gameObject);
    }
}
