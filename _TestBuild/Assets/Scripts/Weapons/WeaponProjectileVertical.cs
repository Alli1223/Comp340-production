using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectileVertical : WeaponProjectile 
{

    public float verticalHeight;
    Vector3 midPoint;
    bool reachedMidPoint;
    public override void SetTarget(Vector3 pos)
    {
        midPoint = Vector3.Lerp(transform.position, pos, 0.5f) + Vector3.up * verticalHeight;
        reachedMidPoint = false;

        target = pos;
        transform.LookAt(midPoint);
    }

    protected override void Update()
    {
        if (!reachedMidPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, midPoint, speed);
            if (transform.position == midPoint)
            {
                reachedMidPoint = true;
                transform.LookAt(target);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * 1.5f);
            if (transform.position == target)
            {
                Hit();
            }
        }
    }
}
