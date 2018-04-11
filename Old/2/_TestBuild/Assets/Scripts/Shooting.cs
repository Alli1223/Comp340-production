using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public int velocity = 10;
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * velocity;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
