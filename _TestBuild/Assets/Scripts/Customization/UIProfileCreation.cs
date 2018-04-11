using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIProfileCreation : MonoBehaviour 
{
    public string inputFieldPlayerName { get; set; }
	
    public GameObject profileButtonPrefab;
    public RectTransform profileScrollView;
    public Text profileSelectedText;
    public GameObject mechSelectCanvas;
    UIMechCreation uiMechCreation;

    GameObject[] spawnedButtons = new GameObject[0];
    string[] folderList = new string[0];

    int currentlySelectedProfile;
    void Start()
    {
        uiMechCreation = mechSelectCanvas.GetComponent<UIMechCreation>();
        mechSelectCanvas.SetActive(false);
        LoadProfiles();
    }

    public void SwitchToMechSelect()
    {
        if (folderList.Length != 0)
        {
            uiMechCreation.profileFolder = folderList[currentlySelectedProfile];
            mechSelectCanvas.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void CreateProfile()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Profiles/" + inputFieldPlayerName))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles/" + inputFieldPlayerName);
            LoadProfiles();
        }
    }

    public void LoadProfiles()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Profiles"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles");
        }
        folderList = Directory.GetDirectories(Application.persistentDataPath + "/Profiles");
        for (int i = 0; i < folderList.Length; i++)
        {
            folderList[i] = folderList[i].Remove(0, Application.persistentDataPath.Length + 10);
        }

        CreateProfileButtons();
    }

    public void SelectProfile(int id)
    {
        if (folderList.Length != 0)
        {
            currentlySelectedProfile = id;
            profileSelectedText.text = folderList[id];
        }
    }

    public void DeleteProfile()
    {
        Directory.Delete(Application.persistentDataPath + "/Profiles/" + folderList[currentlySelectedProfile], true);
        LoadProfiles();
    }

    void CreateProfileButtons()
    {
        for (int i = 0; i < spawnedButtons.Length; i++)
        {
            GameObject.Destroy(spawnedButtons[i]);
        }

        spawnedButtons = new GameObject[folderList.Length];
        for (int i = 0; i < folderList.Length; i++)
        {
            spawnedButtons[i] = GameObject.Instantiate(profileButtonPrefab, profileScrollView);
            spawnedButtons[i].transform.GetChild(0).GetComponent<Text>().text = folderList[i];
            Button bt = spawnedButtons[i].GetComponent<Button>();

            int indexCopy = i;

            UnityEngine.Events.UnityAction profileEvent = () => { this.SelectProfile(indexCopy); };

            bt.onClick.AddListener(profileEvent);
        }

        SelectProfile(0);
    }
}
