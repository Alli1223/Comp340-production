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

	public void FireWeapon(int id)
	{
		if (id == 0) {
			if(animWeaponL != null)
				animWeaponL.SetTrigger ("Shoot");
		} else if (id == 1) {
			if(animWeaponR != null)
				animWeaponR.SetTrigger ("Shoot");
		} else if (id == 2) {
			if(animGimbalL != null)
				animGimbalL.SetTrigger ("Shoot");
		} else if (id == 3) {
			if(animGimbalR != null)
				animGimbalR.SetTrigger ("Shoot");
		}
	}

//    void Update()
//    {
//        for (int i = 0; i < allRenderers.Length; i++)
//        {
//            if (allRenderers[i] != null)
//            {
//                allRenderers[i].material.SetColor("_Color1", mechColor[0]);
//                allRenderers[i].material.SetColor("_Color2", mechColor[1]);
//                allRenderers[i].material.SetColor("_Color3", mechColor[2]);
//            }
//        }
//    }


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
}
