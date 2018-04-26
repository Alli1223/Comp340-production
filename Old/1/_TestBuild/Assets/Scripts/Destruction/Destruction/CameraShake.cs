using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera MainCam;
    private IEnumerator coroutine;

    // Use this for initialization
    void Start()
    {
        MainCam = gameObject.GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("q");
            coroutine = camShake();
            StartCoroutine(coroutine);
        }
    }

    IEnumerator camShake()
    {
        Debug.Log("shake");
        float elap = 0.0f;
        float duration = 2.2f;
        float magnitude = 0.1f;

        Vector3 startPos = MainCam.transform.position;

        while (elap < duration)
        {
            elap += Time.deltaTime;

            float percentComplete = elap / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            float xShake = Random.value * MainCam.transform.position.x - 0.3f;
            float yShake = Random.value * MainCam.transform.position.y - 0.3f;
            xShake *= magnitude * damper;
            yShake *= magnitude * damper;

            MainCam.transform.position = new Vector3(xShake - 4, yShake + 4, startPos.z);
            yield return null;
        }
        MainCam.transform.position = startPos;
    }
}

