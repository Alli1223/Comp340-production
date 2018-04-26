using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(GridGeneration))]
public class GridGenerationInspector : Editor
{    
    public GridGeneration mainGeneration;

    SerializedObject so;
    TileVarsHolder loadDat;
//
//    public GunType haha;

    public void OnEnable()
    {
        mainGeneration = (GridGeneration)target;
        loadDat = AssetDatabase.LoadAssetAtPath<TileVarsHolder>("Assets/Resources/Data/GridData/TileVarsHolder.asset");
        if (!loadDat)
        {
            loadDat = ScriptableObject.CreateInstance<TileVarsHolder>();
            AssetDatabase.CreateAsset(loadDat, "Assets/Resources/Data/GridData/TileVarsHolder.asset");
        }
        so = new SerializedObject(loadDat);
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

		if (GUILayout.Button ("Show Tiles")) 
		{ 
			mainGeneration.ShowTiles (); 
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("tile_Tex"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tileSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tileHeight"));
        serializedObject.ApplyModifiedProperties();
    }

}