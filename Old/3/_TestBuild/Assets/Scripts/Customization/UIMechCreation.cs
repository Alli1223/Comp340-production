using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIMechCreation : MonoBehaviour 
{
    public GameObject profileSelectCanvas;
    public GameObject customisationCanvas;
    [HideInInspector]
    public string profileFolder = "_";
    public Text mechNameText;
    public RectTransform mechSelectScrollView;
    public GameObject mechButtonPrefab;

    public Transform pedestal;

    public string inputFieldMechName { get; set; }

    CustomisationUI custUI;

    string[] mechList = new string[0];
    GameObject[] spawnedButtons = new GameObject[0];
    public int currentSelectedMech { private get; set; }
    GameObject currentlyShownMech;
    public void SwitchToProfileSelect()
    {
        profileSelectCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SwitchToMechCustomisation()
    {
        custUI.LoadMechIntoEditor(profileFolder, mechList[currentSelectedMech]);
        customisationCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    void Start()
    {
        custUI = customisationCanvas.GetComponent<CustomisationUI>();
        currentSelectedMech = 0;
        //LoadMechs();
    }

    void OnEnable()
    {
        LoadMechs();

    }

    public void CreateNewMech()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Profiles/" + profileFolder + "/Mechs"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles/" + profileFolder + "/Mechs");
        }
        MechData newMech = new MechData();
        newMech.name = inputFieldMechName;
        newMech.hasWeaponGimbalL = false;
        newMech.hasWeaponGimbalR = false;
        newMech.hasWeaponR = false;
        newMech.hasWeaponL = true;

        newMech.weaponArmLType = (int)WeaponClass.SnubCannon;
        newMech.weaponArmLID = 0;

        newMech.headID = 0;
        newMech.upperTorsoID = 0;
        newMech.lowerTorsoID = 0;
        newMech.legsID = 0;
        newMech.armsID = 0;

        MechSaveData.SaveMech(newMech, profileFolder, inputFieldMechName);
        LoadMechs();
    }

    void LoadMechs()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Profiles/" + profileFolder + "/Mechs"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles/" + profileFolder + "/Mechs");
        }
        mechList = Directory.GetFiles(Application.persistentDataPath + "/Profiles/" + profileFolder + "/Mechs/");

        for (int i = 0; i < mechList.Length; i++)
        {
            mechList[i] = mechList[i].Remove(0, Application.persistentDataPath.Length + 10 + profileFolder.Length + 7);
            mechList[i] = mechList[i].Remove(mechList[i].Length - 5);
        }
        CreateMechButtons();

    }

    public void DeleteMech()
    {
        File.Delete(Application.persistentDataPath + "/Profiles/" + profileFolder + "/Mechs/" + mechList[currentSelectedMech] + ".mech");
        LoadMechs();
    }

    public void SelectMech(int id)
    {
        if (mechList.Length != 0)
        {
            if (currentlyShownMech != null)
                GameObject.Destroy(currentlyShownMech);
            currentSelectedMech = id;
            mechNameText.text = mechList[id];

            GameObject go = MechIDConst.SpawnMech(Vector3.zero, Quaternion.identity, MechSaveData.LoadMech(profileFolder, mechList[id]), false);
            go.transform.parent = pedestal;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            currentlyShownMech = go;
        }
    }

    void CreateMechButtons()
    {
        for (int i = 0; i < spawnedButtons.Length; i++)
        {
            GameObject.Destroy(spawnedButtons[i]);
        }

        spawnedButtons = new GameObject[mechList.Length];
        for (int i = 0; i < mechList.Length; i++)
        {
            spawnedButtons[i] = GameObject.Instantiate(mechButtonPrefab, mechSelectScrollView);
            spawnedButtons[i].transform.GetChild(0).GetComponent<Text>().text = mechList[i];

            Button bt = spawnedButtons[i].GetComponent<Button>();

            int indexCopy = i;

            UnityEngine.Events.UnityAction buttonEvent = () => { this.SelectMech(indexCopy); };

            bt.onClick.AddListener(buttonEvent);
        }
        SelectMech(currentSelectedMech);
    }
}
