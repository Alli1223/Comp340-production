using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour 
{
    [SerializeField]
    protected GameObject hitParticlePrefab;
    [HideInInspector]
    protected Vector3 target;
    [SerializeField]
    protected float speed;

    public virtual void SetTarget(Vector3 pos)
    {
        target = pos;
        transform.LookAt(pos);
    }
	
	// Update is called once per frame
    protected virtual void Update () 
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed);
        if (transform.position == target)
        {
            Hit();
        }
	}

    protected void Hit()
    {
        if(hitParticlePrefab != null)
            GameObject.Instantiate(hitParticlePrefab, transform.position, Quaternion.Euler(Vector3.zero));
        GameObject.Destroy(this.gameObject);
    }
}
