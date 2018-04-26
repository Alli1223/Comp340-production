using UnityEngine;

public static class RunOnStartup 
{
    // add any functions to run before first scene is loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RunOnApplicationStart()
    {
        UTRigAssembler.Instance = Resources.Load("Data/UTRigAssembler") as UTRigAssembler;
        LTRigAssembler.Instance = Resources.Load("Data/LTRigAssembler") as LTRigAssembler;
        WeaponDataBase.Instance = Resources.Load("Data/WeaponDataBase") as WeaponDataBase;
        MechColorArray.Instance = Resources.Load("Data/MechColors") as MechColorArray;
        PartsDataBase.Instance = Resources.Load("Data/MechPartsDataBase") as PartsDataBase;
        PartsDataBase.Instance.GenerateID();

    }
	
}
