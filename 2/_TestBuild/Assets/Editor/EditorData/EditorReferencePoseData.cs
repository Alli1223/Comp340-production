using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EditorReferencePoseData : ScriptableObject
{
	[SerializeField]
    public AnimationClip sampleClip3JointLegs;
	[SerializeField]
	public float sampleTime3JointLegs;
	[SerializeField]
	public AnimationClip[] toOverride3JointLegs;

    [SerializeField]
    public AnimationClip sampleClip4JointLegs;
    [SerializeField]
    public float sampleTime4JointLegs;
    [SerializeField]
    public AnimationClip[] toOverride4JointLegs;

    [SerializeField]
    public AnimationClip sampleClip1JointArms;
    [SerializeField]
    public float sampleTime1JointArms;
    [SerializeField]
    public AnimationClip[] toOverride1JointArms;

    [SerializeField]
    public AnimationClip sampleClip2JointArms;
    [SerializeField]
    public float sampleTime2JointArms;
    [SerializeField]
    public AnimationClip[] toOverride2JointArms;

    [SerializeField]
    public AnimationClip sampleClip3JointArms;
    [SerializeField]
    public float sampleTime3JointArms;
    [SerializeField]
    public AnimationClip[] toOverride3JointArms;
}
