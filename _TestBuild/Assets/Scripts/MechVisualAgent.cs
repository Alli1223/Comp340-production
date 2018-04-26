using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechVisualAgent : MonoBehaviour 
{
    public Animator animUpperTorso;
    public Animator animLowerTorso;
	
    public Animator animWeaponL;
    public Animator animWeaponR;
    public Animator animGimbalL;
    public Animator animGimbalR;

    public Transform weaponMountL;
    public Transform weaponMountR;
    public Transform shieldMountL;
    public Transform shieldMountR;
    public Transform weaponMountGimbalL;
    public Transform weaponMountGimbalR;

    public Transform upperTorsoParent;

    public UTRigAssembler.ArmType armType;
    public LTRigAssembler.LegType legType;

    public MechIDConst mechID;

    public WeaponParticleInfo particleInfoArmL;
    public WeaponParticleInfo particleInfoArmR;
    public WeaponParticleInfo particleInfoGimbL;
    public WeaponParticleInfo particleInfoGimbR;

    int bool_Move;
    int float_TurnDegree;
    int trigger_Turn;

    public Color[] mechColor = new Color[3];

    Renderer[] allRenderers;

    public void SetAnimation(bool enabled)
    {
        if (enabled)
        {
            animLowerTorso.SetLayerWeight(1, 1f);
            animUpperTorso.SetLayerWeight(1, 1f);
        }
        else
        {
            animLowerTorso.SetLayerWeight(1, 0f);
            animUpperTorso.SetLayerWeight(1, 0f);
        }
    }

    public float GetCinematicTimeOfWeapon(int weaponID)
    {
        float weaponCinematicTime = 0.25f;

        if (weaponID == 0)
        {
            weaponCinematicTime += particleInfoArmL.cinematicLength;
        }
        else if (weaponID == 1)
        {
            weaponCinematicTime += particleInfoArmR.cinematicLength;
        }
        else if (weaponID == 2)
        {
            weaponCinematicTime += particleInfoGimbL.cinematicLength;
        }

        return weaponCinematicTime;
    }
        
    bool trackTarget = false;
    public void ResetTorso()
    {
        trackTarget = false;
    }

    void LateUpdate()
    {
        if (trackTarget)
        {
            upperTorsoParent.LookAt(aimTarget, Vector3.up);
        }
        else
        {
            upperTorsoParent.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void TorsoTwistAt(Vector3 pos)
    {
        aimTarget = pos;
        trackTarget = true;
    }

    public void TorsoTwistAt(Vector3 pos, float time)
    {
        aimTarget = pos;
        trackTarget = true;

        StartCoroutine(ResetTorsoOnDelay(time));
    }

    IEnumerator ResetTorsoOnDelay(float time)
    {
        yield return new WaitForSeconds(time);

        ResetTorso();

        yield break;
    }

    public void SetAnimIDs()
    {
        bool_Move = Animator.StringToHash("walk");
        float_TurnDegree = Animator.StringToHash("turnDegree");
        trigger_Turn = Animator.StringToHash("turn");
    }

    public void SetMove(bool move)
    {
        animLowerTorso.SetBool(bool_Move, move);
    }

    public void Turn(float degrees)
    {
        animLowerTorso.SetFloat(float_TurnDegree, degrees);
        animLowerTorso.SetTrigger(trigger_Turn);
    }

    void OnAnimatorMove()
    {
        //Debug.Log(animLowerTorso.deltaRotation);
        //Quaternion newRot = transform.GetChild(0);
        //newRot *= transform.rotation;
        //transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);
        //transform.rotation = newRot;
    }
        
    public void UpdateRendererReferences()
    {
        allRenderers = GetComponentsInChildren<Renderer>();
    }

    public void FireWeapon(int weaponID, float delay = 0.25f)
	{
		if (weaponID == 0) 
        {
            this.Invoke("CinematicFireArmL", delay);
		} 
        else if (weaponID == 1) 
        {
            this.Invoke("CinematicFireArmR", delay);
		} 
        else if (weaponID == 2) 
        {
            this.Invoke("CinematicFireGimbalL", delay);
		} 
        else if (weaponID == 3) 
        {
            this.Invoke("CinematicFireGimbalR", delay);
		}
	}

    Vector3 aimTarget;

    void CinematicFireArmL()
    {
        animWeaponL.SetTrigger("Shoot");
    }

    void CinematicFireArmR()
    {
        animWeaponR.SetTrigger("Shoot");
    }

    void CinematicFireGimbalL()
    {
        animGimbalL.SetTrigger("Shoot");
    }

    void CinematicFireGimbalR()
    {
        animGimbalR.SetTrigger("Shoot");
    }

    static string[] colorPropertyNames = new string[3] {"_Color1", "_Color2", "_Color3"};

    public void ChangeColor(int colorType, int id)
    {
        mechColor[colorType] = MechColorArray.GetColor(id);
        for (int i = 0; i < allRenderers.Length; i++)
        {
            if (allRenderers[i] != null)
            {
                allRenderers[i].material.SetColor(colorPropertyNames[colorType], mechColor[colorType]);
            }
        }
    }

    public WeaponParticleInfo GetParticleInfo(int weaponID)
    {

        if (weaponID == 0)
        {
            return particleInfoArmL;
        }
        else if (weaponID == 1)
        {
            return particleInfoArmR;
        }
        else if (weaponID == 2)
        {
            return particleInfoGimbL;
        }
        else if (weaponID == 3)
        {
            return particleInfoGimbR;
        }
        else
            return null;
    }
}
