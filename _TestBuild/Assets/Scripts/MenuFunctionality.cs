﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuFunctionality : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    
    public void MissionOneButton(int index)
    {
        
        SceneManager.LoadScene(index);
    }

    public void LoadGameButton()
    {
        SaveLoad.Load();
        Game saveGame;
        saveGame = SaveLoad.savedGames;
    }
}
