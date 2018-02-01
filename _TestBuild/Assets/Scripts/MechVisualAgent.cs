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
}
