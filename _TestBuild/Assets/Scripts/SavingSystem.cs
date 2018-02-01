using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SavingSystem : MonoBehaviour {

    public bool saveOnExit = true;

    //! Use this for initialization
    void Start () {
		
	}
	
	//! Update is called once per frame
	void Update () {
		
	}

    // Function gets called when application closes
    void OnApplicationQuit()
    {
        // if save on exit os on then save
        if (saveOnExit)
        {
            Debug.Log("GAME SAVING..");
            try
            {
                SaveLoad.Save();
                Debug.Log("GAME SUCESSIVELY SAVED TO: " + Application.persistentDataPath);
            }
            catch
            {
                Debug.Log("ERROR SAVING GAME");
            }
        }
    }
}



//! The data to be saved
[System.Serializable]
public class Game
{
    //! Game variables to be stored here
    public static Game currentGame;
    public PlayerManager player;
    public static List<MechIDConst> allMechs = new List<MechIDConst>();
    public static List<EnemyData> allEnemies = new List<EnemyData>();


    //! Game constructor
    public Game ()
    {
        player = new PlayerManager();

    }
}

public static class SaveLoad
{
    //! List of game saves
    public static List<Game> savedGames = new List<Game>();
    

    //! Save a game state
    public static void Save()
    {
        savedGames.Add(Game.currentGame);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }


    //! Load a game state
    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            Debug.Log("Loading Save Game ..");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
        else
            Debug.Log("Save game file does not exist");
    }
}

