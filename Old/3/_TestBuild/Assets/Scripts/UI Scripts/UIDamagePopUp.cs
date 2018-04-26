using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDamagePopUp : MonoBehaviour 
{
    float lifeTime = 1f;

    public void SetText(int damage)
    {
        GetComponent<Text>().text = damage.ToString();
    }

    void Update()
    {
        if (lifeTime <= 0f)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            lifeTime -= Time.deltaTime;
        }
    }
}
