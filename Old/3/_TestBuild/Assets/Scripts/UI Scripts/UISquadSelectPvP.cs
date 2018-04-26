using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UISquadSelectPvP : MonoBehaviour 
{
    public GameObject UIprefab;
    public GameObject healthBarPrefab;
    public GameObject gamePlayUI;

    public int maxMechsAllowed;

    int player1TotalMechsSelected;
    int player2TotalMechsSelected;


    int[] player1SelectedMechsID;
    int[] player2SelectedMechsID;

    public Dropdown player1DropDown;
    public Dropdown player2DropDown;

    int player1ProfileID;
    int player2ProfileID;

    public GameObject player1MechSelectScrollView;
    public GameObject player2MechSelectScrollView;

    public GameObject player1SelectedMechsScrollView;
    public GameObject player2SelectedMechsScrollView;

    int player1HighlightedMechID;
    int player2HighlightedMechID;

    int player1SquadHighlightedMechID;
    int player2SquadHighlightedMechID;

    string[] playerProfiles;

    string[] player1MechNames;
    string[] player2MechNames;


    public void ChangePlayer1Profile(int id)
    {
        player1ProfileID = id;
        LoadMechPerProfile(1);
        ChangePlayer1HighlightedMech(0);

        for (int i = 0; i < maxMechsAllowed; i++)
        {
            player1SelectedMechsID[i] = -1;
        }

        FillListOfSelectedMechs(1);
    }

    public void ChangePlayer2Profile(int id)
    {
        player2ProfileID = id;
        LoadMechPerProfile(2);
        ChangePlayer2HighlightedMech(0);

        for (int i = 0; i < maxMechsAllowed; i++)
        {
            player2SelectedMechsID[i] = -1;
        }

        FillListOfSelectedMechs(2);
    }

    public void ChangePlayer1HighlightedMech(int id)
    {
        player1HighlightedMechID = id;
    }

    public void ChangePlayer2HighlightedMech(int id)
    {
        player2HighlightedMechID = id;
    }

    public void ChangePlayer1SquadHighlightedMech(int id)
    {
        player1SquadHighlightedMechID = id;
    }

    public void ChangePlayer2SquadHighlightedMech(int id)
    {
        player2SquadHighlightedMechID = id;
    }

    
    void LoadAllProfiles()
    {
        playerProfiles = Directory.GetDirectories(Application.persistentDataPath + "/Profiles/");

        for (int i = 0; i < playerProfiles.Length; i++)
        {
            playerProfiles[i] = playerProfiles[i].Remove(0, Application.persistentDataPath.Length + 10);
        }

        List<string> tempList = new List<string>();
        tempList.AddRange(playerProfiles);

        player1DropDown.AddOptions(tempList);
        player2DropDown.AddOptions(tempList);
    }

    void LoadMechPerProfile(int player)
    {
        if (player == 1)
        {
            player1MechNames = Directory.GetFiles(Application.persistentDataPath + "/Profiles/" + playerProfiles[player1ProfileID] + "/Mechs/");

            for (int i = 0; i < player1MechNames.Length; i++)
            {
                player1MechNames[i] = player1MechNames[i].Remove(0, Application.persistentDataPath.Length + 17 + playerProfiles[player1ProfileID].Length);
                player1MechNames[i] = player1MechNames[i].Remove(player1MechNames[i].Length - 5);
            }

            FillListOfAvailableMechs(1);
        }
        else if (player == 2)
        {
            player2MechNames = Directory.GetFiles(Application.persistentDataPath + "/Profiles/" + playerProfiles[player2ProfileID] + "/Mechs/");

            for (int i = 0; i < player1MechNames.Length; i++)
            {
                player2MechNames[i] = player2MechNames[i].Remove(0, Application.persistentDataPath.Length + 17 + playerProfiles[player2ProfileID].Length);
                player2MechNames[i] = player2MechNames[i].Remove(player2MechNames[i].Length - 5);
            }

            FillListOfAvailableMechs(2);
        }
    }

    GameObject[] mechButtons1 = new GameObject[0];
    GameObject[] mechButtons2 = new GameObject[0];

    GameObject[] squadMechButtons1 = new GameObject[0];
    GameObject[] squadMechButtons2 = new GameObject[0];

    public GameObject mechDisplayPlate;

    public void Player1AddMech()
    {
        for (int i = 0; i < maxMechsAllowed; i++)
        {
            if (player1SelectedMechsID[i] == -1)
            {
                player1SelectedMechsID[i] = player1HighlightedMechID;
                player1SquadHighlightedMechID = i;
                FillListOfSelectedMechs(1);
                return;
            }
        }
    }

    public void Player2AddMech()
    {
        for (int i = 0; i < maxMechsAllowed; i++)
        {
            if (player2SelectedMechsID[i] == -1)
            {
                player2SelectedMechsID[i] = player2HighlightedMechID;
                player2SquadHighlightedMechID = i;
                FillListOfSelectedMechs(2);
                return;
            }
        }
    }

    public void Player1RemoveMech()
    {
        if (player1SelectedMechsID[player1SquadHighlightedMechID] >= 0)
        {
            player1SelectedMechsID[player1SquadHighlightedMechID] = -1;
            FillListOfSelectedMechs(1);
        }
    }

    public void Player2RemoveMech()
    {
        if (player2SelectedMechsID[player2SquadHighlightedMechID] >= 0)
        {
            player2SelectedMechsID[player2SquadHighlightedMechID] = -1;
            FillListOfSelectedMechs(2);
        }
    }

    void FillListOfSelectedMechs(int player)
    {
        if (player == 1)
        {
            for (int i = 0; i < squadMechButtons1.Length; i++)
            {
                GameObject.Destroy(squadMechButtons1[i]);
            }

            squadMechButtons1 = new GameObject[maxMechsAllowed];

            for (int i = 0; i < maxMechsAllowed; i++)
            {
                if (player1SelectedMechsID[i] >= 0)
                {
                    squadMechButtons1[i] = GameObject.Instantiate(mechDisplayPlate, player1SelectedMechsScrollView.transform);
                    squadMechButtons1[i].transform.GetChild(0).GetComponent<Text>().text = player1MechNames[player1SelectedMechsID[i]];

                    Button bt = squadMechButtons1[i].GetComponent<Button>();

                    int eventID = i;

                    UnityEngine.Events.UnityAction mechEvent = () =>
                    {
                            this.ChangePlayer1SquadHighlightedMech(eventID);
                    };

                    bt.onClick.AddListener(mechEvent);
                }
            }
        }
        else if (player == 2)
        {
            for (int i = 0; i < squadMechButtons2.Length; i++)
            {
                GameObject.Destroy(squadMechButtons2[i]);
            }

            squadMechButtons2 = new GameObject[maxMechsAllowed];

            for (int i = 0; i < maxMechsAllowed; i++)
            {
                if (player2SelectedMechsID[i] >= 0)
                {
                    squadMechButtons2[i] = GameObject.Instantiate(mechDisplayPlate, player2SelectedMechsScrollView.transform);
                    squadMechButtons2[i].transform.GetChild(0).GetComponent<Text>().text = player2MechNames[player2SelectedMechsID[i]];

                    Button bt = squadMechButtons2[i].GetComponent<Button>();

                    int eventID = i;

                    UnityEngine.Events.UnityAction mechEvent = () =>
                        {
                            this.ChangePlayer2SquadHighlightedMech(eventID);
                        };

                    bt.onClick.AddListener(mechEvent);
                }
            }
        }
    }

    void FillListOfAvailableMechs(int player)
    {
        if (player == 1)
        {
            for (int i = 0; i < mechButtons1.Length; i++)
            {
                GameObject.Destroy(mechButtons1[i]);
            }

            mechButtons1 = new GameObject[player1MechNames.Length];

            for (int i = 0; i < mechButtons1.Length; i++)
            {
                mechButtons1[i] = GameObject.Instantiate(mechDisplayPlate, player1MechSelectScrollView.transform);
                mechButtons1[i].transform.GetChild(0).GetComponent<Text>().text = player1MechNames[i];

                Button bt = mechButtons1[i].GetComponent<Button>();

                int eventID = i;

                UnityEngine.Events.UnityAction mechEvent = () =>
                {
                        this.ChangePlayer1HighlightedMech(eventID);
                };

                bt.onClick.AddListener(mechEvent);
            }
        }
        else if (player == 2)
        {
            for (int i = 0; i < mechButtons2.Length; i++)
            {
                GameObject.Destroy(mechButtons2[i]);
            }

            mechButtons2 = new GameObject[player2MechNames.Length];

            for (int i = 0; i < mechButtons1.Length; i++)
            {
                mechButtons2[i] = GameObject.Instantiate(mechDisplayPlate, player2MechSelectScrollView.transform);
                mechButtons2[i].transform.GetChild(0).GetComponent<Text>().text = player2MechNames[i];

                Button bt = mechButtons2[i].GetComponent<Button>();

                int eventID = i;

                UnityEngine.Events.UnityAction mechEvent = () =>
                    {
                        this.ChangePlayer2HighlightedMech(eventID);
                    };

                bt.onClick.AddListener(mechEvent);
            }
        }
    }

    List<SpawnPoint> spawnPointsPlayer1 = new List<SpawnPoint>();
    List<SpawnPoint> spawnPointsPlayer2 = new List<SpawnPoint>();

    public void StartGame()
    {
        int currentSpawnIndex1 = 0;
        int maxSpawnIndex1 = spawnPointsPlayer1.Count;
        int currentSpawnIndex2 = 0;
        int maxSpawnIndex2 = spawnPointsPlayer2.Count;

        GameObject go = GameObject.Find("Manager");

        PlayerManager pm = go.GetComponent<PlayerManager>();
        pm.allPlayerMechs.Add(new List<PlayerData>());
        pm.allPlayerMechs.Add(new List<PlayerData>());

        for (int i = 0; i < maxMechsAllowed; i++)
        {
            if (currentSpawnIndex1 < maxSpawnIndex1)
            {
                if (player1SelectedMechsID[i] >= 0)
                {
                    GameObject temp = MechIDConst.SpawnMech(
                        spawnPointsPlayer1[currentSpawnIndex1].transform.position, 
                        spawnPointsPlayer1[currentSpawnIndex1].transform.rotation,
                        MechSaveData.LoadMech(playerProfiles[player1ProfileID], player1MechNames[player1SelectedMechsID[i]]), true);

                    UIHealthBar tempHealthBar = GameObject.Instantiate(healthBarPrefab, transform).GetComponent<UIHealthBar>();
                    PlayerData tempPlayer = temp.GetComponent<PlayerData>();

                    tempPlayer.myHealthBar = tempHealthBar;
                    tempHealthBar.target = tempPlayer.transform;
                    tempHealthBar.SetColor(Color.cyan);

                    tempPlayer.curTile = spawnPointsPlayer1[currentSpawnIndex1].closestTile;
                    tempPlayer.playerNum = 0;
//                    PlayerManager.gPlayer.firstPlayers.Add(temp);
                    spawnPointsPlayer1[currentSpawnIndex1].closestTile.occupyingObj = temp;
                    currentSpawnIndex1++;

                    tempPlayer.Initialize(pm, 0);
                }
            }

            if (currentSpawnIndex2 < maxSpawnIndex2)
            {
                if (player2SelectedMechsID[i] >= 0)
                {
                    GameObject temp = MechIDConst.SpawnMech(
                        spawnPointsPlayer2[currentSpawnIndex2].transform.position, 
                        spawnPointsPlayer2[currentSpawnIndex2].transform.rotation,
                        MechSaveData.LoadMech(playerProfiles[player2ProfileID], player2MechNames[player2SelectedMechsID[i]]), true);

                    UIHealthBar tempHealthBar = GameObject.Instantiate(healthBarPrefab, transform).GetComponent<UIHealthBar>();
                    PlayerData tempPlayer = temp.GetComponent<PlayerData>();

                    tempPlayer.myHealthBar = tempHealthBar;
                    tempHealthBar.target = tempPlayer.transform;
                    tempHealthBar.SetColor(Color.red);

                    tempPlayer.curTile = spawnPointsPlayer2[currentSpawnIndex2].closestTile;
                    tempPlayer.playerNum = 1;
//                    PlayerManager.gPlayer.secondPlayers.Add(temp);
                    spawnPointsPlayer2[currentSpawnIndex2].closestTile.occupyingObj = temp;
                    currentSpawnIndex2++;

                    tempPlayer.Initialize(pm, 1);
                }
            }
        }

        gamePlayUI.SetActive(true);
        pm.enabled = true;
        pm.SpawnDone();
        UIprefab.SetActive(false);


        go.GetComponent<TurnManager>().enabled = true;
        go.GetComponent<PlayerMovement>().enabled = true;




    }
        
    void Awake () 
    {
        UIprefab.SetActive(true);
        gamePlayUI.SetActive(false);
        LoadAllProfiles();
        
        player1SelectedMechsID = new int[maxMechsAllowed];
        player2SelectedMechsID = new int[maxMechsAllowed];
        
        for (int i = 0; i < maxMechsAllowed; i++)
        {
            player1SelectedMechsID[i] = -1;
            player2SelectedMechsID[i] = -1;
        }
        
        ChangePlayer1Profile(0);
        ChangePlayer2Profile(0);

        SpawnPoint[] spawnPoints;
        spawnPoints = FindObjectsOfType<SpawnPoint>() as SpawnPoint[];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].team == 0)
            {
                spawnPointsPlayer1.Add(spawnPoints[i]);
            }
            else
            {
                spawnPointsPlayer2.Add(spawnPoints[i]);
            }
        }
    }
}
