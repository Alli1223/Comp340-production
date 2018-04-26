using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GridGeneration))]
public class GridGenerationInspector : Editor
{    
    public static GridGeneration mainGeneration = GridGeneration.gridSingle;

    static SerializedObject so;
    private static TileVarsHolder loadDat;
//
//    public GunType haha;
//    [ExecuteInEditMode]
//    private static void Awake()
//    {
//        mainGeneration = FindObjectOfType(typeof(GridGeneration))as GridGeneration;
//    }

    public void OnEnable()
    {
        mainGeneration = (GridGeneration)target;
        CheckLoads();
    }

    [MenuItem("Grid Tools/Build Grid", false, 1)]
    private static void BuildGrid()
    {
        SetGridInstance();
        CheckLoads();
        EditorUtility.SetDirty(mainGeneration);
        mainGeneration.CreateGrid(loadDat);
        AssetDatabase.SaveAssets();
    }
    [MenuItem("Grid Tools/Destroy Grid", false, 2)]
    private static void DestroyTheGrid()
    {
        SetGridInstance();
        CheckLoads();
        mainGeneration.DestroyGrid();
    }
    [MenuItem("Grid Tools/Reset Grid", false, 0)]
    private static void ResetGrid()
    {
        SetGridInstance();
        CheckLoads();
        mainGeneration.DestroyGrid();
        mainGeneration.CreateGrid(loadDat);
        AssetDatabase.SaveAssets();
    }
    [MenuItem("Grid Tools/Allign Buildings", false, 3)]
    private static void AllignBuildings()
    {
        SetGridInstance();
        CheckLoads();
        mainGeneration.AllignBuildings();
        mainGeneration.DestroyGrid();
        mainGeneration.CreateGrid(loadDat);
        AssetDatabase.SaveAssets();
    }
    [MenuItem("Grid Tools/Remove Box Children", false, 4)]
    private static void RemoveBoxChildren()
    {
        SetGridInstance();
        CheckLoads();
        mainGeneration.RemoveBuildingBoxColliders();       
    }
    [MenuItem("Grid Tools/Show Tiles", false, 5)]
    private static void ShowTiles()
    {
        SetGridInstance();
        CheckLoads();
        mainGeneration.ShowTiles (); 
    }

    [MenuItem("Grid Tools/Load Data", false, 20)]
    private static void LoadData()
    {
        SetGridInstance();
        if (!LoadVars())
        {
            loadDat = ScriptableObject.CreateInstance<TileVarsHolder>();
            mainGeneration.DestroyGrid();
            AssetDatabase.CreateAsset(loadDat, "Assets/Resources/Data/GridData/" + SceneManager.GetActiveScene().name  + " GridData.asset");
            mainGeneration.CreateGrid(loadDat);
            AssetDatabase.SaveAssets();
            so = new SerializedObject(loadDat);
        }
            
    }

    public static void CheckLoads()
    {
        loadDat = AssetDatabase.LoadAssetAtPath<TileVarsHolder>("Assets/Resources/Data/GridData/" + SceneManager.GetActiveScene().name  + " GridData.asset");
        if (!loadDat)
        {
            loadDat = ScriptableObject.CreateInstance<TileVarsHolder>();
            AssetDatabase.CreateAsset(loadDat, "Assets/Resources/Data/GridData/" + SceneManager.GetActiveScene().name  + " GridData.asset");
        }
        so = new SerializedObject(loadDat);
    }

    public static bool LoadVars()
    {
        TileVarsHolder loadDat = AssetDatabase.LoadAssetAtPath<TileVarsHolder>("Assets/Resources/Data/GridData/" + SceneManager.GetActiveScene().name  + " GridData.asset");
        if (!loadDat)
        {
            Debug.LogError("No Save File Found");
            return false;
        }
        else
        {
            Debug.Log("Save File Found");
            mainGeneration.LoadData(loadDat);
            return true;
        }
    }

    private static void SetGridInstance()
    {
        if (mainGeneration == null)
        {
            mainGeneration = GridGeneration.gridSingle;
        }
    }

    public override void OnInspectorGUI()
    {
//        test.fireTemplate = (GunType)EditorGUILayout.EnumPopup("Gun Type", test.fireTemplate);
//
//        if (test.fireTemplate == GunType.AssaultRifle)
//        {
//            EditorGUILayout.PropertyField(serializedObject.FindProperty("fireDelay"));
//            EditorGUILayout.PropertyField(serializedObject.FindProperty("recoil"));
//            EditorGUILayout.PropertyField(serializedObject.FindProperty("spread"));
//            EditorGUILayout.PropertyField(serializedObject.FindProperty("force"));
//            EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletPrefab"));
//            serializedObject.ApplyModifiedProperties();
//        }
        EditorUtility.SetDirty(mainGeneration);
        if (GUILayout.Button("Build Grid"))
        {
            mainGeneration.CreateGrid(loadDat);
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Delete Grid"))
        {
            mainGeneration.DestroyGrid();
        }

        if (GUILayout.Button("Reset Grid"))
        {
            mainGeneration.DestroyGrid();
            mainGeneration.CreateGrid(loadDat);
            AssetDatabase.SaveAssets();
//            Debug.Log(loadDat.arrayOTiles.Length);
        }

        if (GUILayout.Button("Allign Buildings")) 
		{
            mainGeneration.AllignBuildings();
            mainGeneration.DestroyGrid();
            mainGeneration.CreateGrid(loadDat);
            AssetDatabase.SaveAssets();
		}
        if (GUILayout.Button("Remove Box Children"))
        {
            mainGeneration.RemoveBuildingBoxColliders();
        }


		if (GUILayout.Button ("Show Tiles")) 
		{ 
			mainGeneration.ShowTiles (); 
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("tile_Tex"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tileSize"));
        serializedObject.ApplyModifiedProperties();
    }

}