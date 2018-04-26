using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static Camera MainCam;
    public IEnumerator coroutine;
    public static float duration = 3.2f;

    // Use this for initialization
     void Start()
    {
        MainCam = Camera.main;
    }


    public static IEnumerator CamShake()
    {
            Debug.Log("shake");
            float elap = 0.0f;
            float magnitude = 0.05f;

            Vector3 startPos = Camera.main.transform.position;

            while (elap < duration)
            {
                elap += Time.deltaTime;

                float percentComplete = elap / duration;
                float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

                float xShake = Random.value * Camera.main.transform.position.x - 0.3f;
                float yShake = Random.value * Camera.main.transform.position.y - 0.3f;
                xShake *= magnitude * damper;
                yShake *= magnitude * damper;

            Camera.main.transform.position = new Vector3(xShake - 5, yShake + 5, startPos.z);
                yield return null;
            }
        Camera.main.transform.position = startPos;
        }
}

