using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayAndAutoDestruct : MonoBehaviour
{
    public bool useFixedTime;

    
    public bool playWhenEnabled;

    ParticleSystem _particleSystem;
    bool playing;
    float destructTimer;

	void OnEnable ()
    {
		if(playWhenEnabled)
        {
            Play();
        }
	}

    public void Play()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _particleSystem.Play();
        playing = true;
        destructTimer = _particleSystem.main.duration;
    }

    void Update ()
    {
        if (!useFixedTime)
        {
            if (playing)
            {
                if (destructTimer > 0f)
                {
                    destructTimer -= Time.deltaTime;
                }
                else
                {
                    DestroyImmediate(this.gameObject);
                }
    
            }
        }
	}

    private void FixedUpdate()
    {
        if(useFixedTime)
        {
            if(playing)
            {
                if (destructTimer > 0f)
                {
                    destructTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    DestroyImmediate(this.gameObject);
                }

            }
        }
    }
}
