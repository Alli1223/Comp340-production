using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndScreenManager : MonoBehaviour {

	public static EndScreenManager Instance;
	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != null)
			Destroy (this);
	}
	public int menuScene = 0;
	public int endScene = 4;

	public Button returnToMenu;

	List<GameObject> chkplayers = new List<GameObject>(); 
	List<int> playerNumb = new List<int>();
	bool winner;
	void Start()
	{
		returnToMenu.onClick.AddListener(delegate {	LoadMenu(); });
	}

//	void Update()
//	{
//		if (Input.GetKeyDown(KeyCode.Return))
//			Playercheck ();
//	}

	public void Playercheck()
	{
		chkplayers.AddRange (GameObject.FindGameObjectsWithTag ("Player"));
		foreach (GameObject player in chkplayers) 
		{
			playerNumb.Add (player.GetComponent<PlayerData> ().playerNum);;
		}
		foreach (int pNumb in playerNumb) 
		{
			if (pNumb == PlayerManager.currentMech.playerNum) 
			{
				Debug.Log ("i hate verything");
				winner = true;
			}
			else 
			{
				winner = false;
				playerNumb.Clear();
				break;
			}
		}
		if (winner == true) 
		{
			LoadEnd ();
		}
		chkplayers.Clear();
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene (menuScene);
	}

	public void LoadEnd()
	{
		SceneManager.LoadScene (endScene);
	}
}
