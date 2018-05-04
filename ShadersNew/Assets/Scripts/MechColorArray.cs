using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechColorArray : ScriptableObject 
{
    public static MechColorArray Instance;

    public static Color GetColor(int id)
    {
        return Instance.color[id];
    }

    public static string GetColorName(int id)
    {
        return Instance.colorName[id];
    }

    public static int GetArrayLength()
    {
        return Instance.color.Length;
    }

    [SerializeField]
    Color[] color = new Color[0];

    [SerializeField]
    string[] colorName = new string[0];
	
}
