using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CustomisationUI : MonoBehaviour 
{
    [SerializeField]
    Transform pedestal;
    public GameObject partButtonPrefab;
    public RectTransform partsScrollView;
    public Text[] partNameText = new Text[5];
    public Text[] weaponNameText = new Text[3];
    public GameObject mechSelectMenu;

    public InputField currentMechNameText;

    public RectTransform[] partTabs = new RectTransform[5];
    public RectTransform[] weaponTabs = new RectTransform[3];
    public RectTransform partTabIndicator;

    GameObject currentMech;
    MechVisualAgent currentMechVisualAgent;

    int currentTab;
    public int currentWeaponType;
    public int currentColorType { set; get; }

    MechData mechData;
    string _profileName;
    string _mechName;
    public string _newMechName { set; private get; }

    GameObject[] spawnedButtons = new GameObject[0];

    public Text weightInputText;
    public Text movementInputText;
    public Text armorInputText;
    public Text apInputText;
    public Text visionInputText;

    public RectTransform colorPickerScrollView;
    public GameObject colorButtonPrefab;

    GameObject[] spawnedColorButtons = new GameObject[0];
    public GameObject colorMenu;

    public Text textWeaponName;
    public Text textRange;
    public Text textDamage;
    public Text textAPCost;
    public Text textAccuracy;
    public Text textSplash;
    public Text textAccuracyLabel;

    public void Start()
    {
        GenerateColorPickerButtons();
    }

    public void ToggleColorMenu()
    {
        if (colorMenu.activeInHierarchy)
        {
            colorMenu.SetActive(false);
        }
        else
        {
            colorMenu.SetActive(true);
        }
    }

    void GenerateColorPickerButtons()
    {
        for (int i = 0; i < spawnedColorButtons.Length; i++)
        {
            GameObject.Destroy(spawnedColorButtons[i]);
        }

        spawnedColorButtons = new GameObject[MechColorArray.GetArrayLength()];

        for (int i = 0; i < spawnedColorButtons.Length; i++)
        {
            spawnedColorButtons[i] = GameObject.Instantiate(colorButtonPrefab, colorPickerScrollView);
            Button bt = spawnedColorButtons[i].GetComponent<Button>();
            bt.image.color = MechColorArray.GetColor(i);

            int indexCopy = i;

            UnityEngine.Events.UnityAction colorPick = () =>
            {
                    this.ChangeColor(indexCopy);
            };

            bt.onClick.AddListener(colorPick);
        }
    }

    public void ChangeColor(int id)
    {
        switch (currentColorType)
        {
            case 0:
                mechData.Color1 = id;
                break;
            case 1:
                mechData.Color2 = id;
                break;
            case 2:
                mechData.Color3 = id;
                break;
        }

        currentMechVisualAgent.ChangeColor(currentColorType, id);


    }

    public void LoadMechIntoEditor(string profileName, string mechName)
    {
        currentMech = pedestal.GetChild(0).gameObject;
        _profileName = profileName;
        _mechName = mechName;
        mechData = MechSaveData.LoadMech(profileName, mechName);

        if (!currentlyOnParts)
            TogglePartSelect();

        ChangePartTab(0);
        ChangePart(mechData.headID);
        currentMechNameText.text = mechData.name;
    }
        

    public void ChangePartTab(int tab)
    {
        currentTab = tab;
        if (currentlyOnParts)
        {
            FillAvailableParts();
            partTabIndicator.position = partTabs[tab].position;
        }
        else
        {
            FillAvailableWeapons();
            partTabIndicator.position = weaponTabs[tab].position;
            UpdateWeaponStats(currentMech.GetComponent<PlayerData>());
        }

    }

    bool currentlyOnParts = true;
    public GameObject partSelectionPanel;
    public GameObject weaponSelectionPanel;

    public void TogglePartSelect()
    {
        if (currentlyOnParts)
        {
            currentlyOnParts = false;
            partSelectionPanel.SetActive(false);
            weaponSelectionPanel.SetActive(true);
            ChangePartTab(0);
        }
        else
        {
            currentlyOnParts = true;
            partSelectionPanel.SetActive(true);
            weaponSelectionPanel.SetActive(false);
            ChangePartTab(0);
        }
    }

    public void Exit()
    {
        GameObject.Destroy(currentMech);
        mechSelectMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SaveMechAndExit()
    {
        if (_newMechName.Length > 0)
        {
            if (_mechName != _newMechName)
            {
                File.Delete(Application.persistentDataPath + "/Profiles/" + _profileName + "/Mechs/" + _mechName + ".mech");
                mechData.name = _newMechName;
                MechSaveData.SaveMech(mechData, _profileName, _newMechName);

                GameObject.Destroy(currentMech);
                mechSelectMenu.SetActive(true);
                gameObject.SetActive(false);
                return;
            }
        }

        MechSaveData.SaveMech(mechData, _profileName, _mechName);

        GameObject.Destroy(currentMech);
        mechSelectMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    void UpdatePartText()
    {
        partNameText[0].text = PartsDataBase.Instance.mechPartType[0].part[mechData.headID].name;
        partNameText[1].text = PartsDataBase.Instance.mechPartType[1].part[mechData.upperTorsoID].name;
        partNameText[2].text = PartsDataBase.Instance.mechPartType[2].part[mechData.lowerTorsoID].name;
        partNameText[3].text = PartsDataBase.Instance.mechPartType[3].part[mechData.legsID].name;
        partNameText[4].text = PartsDataBase.Instance.mechPartType[4].part[mechData.armsID].name;
    }

    void UpdateWeaponText()
    {
        if (mechData.hasWeaponL)
            weaponNameText[0].text = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmLType].weapons[mechData.weaponArmLID].name;
        else
            weaponNameText[0].text = "Unequipped";

        if (mechData.hasWeaponR)
            weaponNameText[1].text = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponArmRType].weapons[mechData.weaponArmRID].name;
        else
            weaponNameText[1].text = "Unequipped";

        if (mechData.hasWeaponGimbalL)
            weaponNameText[2].text = WeaponDataBase.Instance.weaponTypeArray[mechData.weaponGimbalLType].weapons[mechData.weaponGimbalLID].name;
        else
            weaponNameText[2].text = "Unequipped";
    }

    MechPart LoadPart(int partType, int partID)
    {
        return PartsDataBase.Instance.mechPartType[partType].part[partID];
    }

    void FillAvailableWeapons()
    {
        for (int i = 0; i < spawnedButtons.Length; i++)
        {
            GameObject.Destroy(spawnedButtons[i]);
        }

        spawnedButtons = new GameObject[WeaponDataBase.Instance.weaponTypeArray[currentWeaponType].weapons.Length];

        for (int i = 0; i < spawnedButtons.Length; i++)
        {
            Weapon weapon = WeaponDataBase.Instance.weaponTypeArray[currentWeaponType].weapons[i];

            spawnedButtons[i] = GameObject.Instantiate(partButtonPrefab, partsScrollView);
            spawnedButtons[i].transform.GetChild(0).GetComponent<Text>().text = weapon.name;

            Button bt = spawnedButtons[i].GetComponent<Button>();

            int eventID = i;

            UnityEngine.Events.UnityAction partEvent = () =>
            {
                    this.ChangeWeapon(eventID);
            };

            bt.onClick.AddListener(partEvent);
        }
    }

    void UpdateWeaponStats(PlayerData pd)
    {
        bool weaponEquipped = true;

        if (currentTab == 0)
        {
            if (pd.leftArmWeaponType == -1)
                weaponEquipped = false;
        }
        else if (currentTab == 1)
        {
            if (pd.rightArmWeaponType == -1)
                weaponEquipped = false;
        }
        else if (currentTab == 2)
        {
            if (pd.backWeaponType == -1)
                weaponEquipped = false;
        }

        if (weaponEquipped)
        {

            Weapon currentWeapon = pd.GetWeapon(currentTab);
            textWeaponName.text = currentWeapon.name;

            if (currentWeapon.minRange > 0)
            {
                textRange.text = string.Format("{0} - {1}", currentWeapon.minRange, currentWeapon.maxRange);
            }
            else
                textRange.text = currentWeapon.maxRange.ToString();

            textDamage.text = string.Format("{0} - {1}", currentWeapon.minDamage, currentWeapon.maxDamage);

            textAPCost.text = currentWeapon.apCost.ToString();

            if (pd.WeaponTargetsDirectly(currentTab))
            {
                textAccuracyLabel.text = "Accuracy:";
                textAccuracy.text = currentWeapon.accuracy.ToString();
            }
            else
            {
                textAccuracyLabel.text = "Scatter:";
                textAccuracy.text = currentWeapon.scatter.ToString();
            }

            textSplash.text = currentWeapon.splashRadius.ToString();
        }
        else
        {
            textWeaponName.text = "Unequipped";
            textRange.text = " - ";
            textDamage.text = " - ";
            textAPCost.text = " - ";
            textAccuracy.text = " - ";
            textAccuracyLabel.text = "Accuracy:";
            textSplash.text = " - ";
        }
    }

    void FillAvailableParts()
    {
        for (int i = 0; i < spawnedButtons.Length; i++)
        {
            GameObject.Destroy(spawnedButtons[i]);
        }

        spawnedButtons = new GameObject[PartsDataBase.Instance.mechPartType[currentTab].part.Length];

        for (int i = 0; i < spawnedButtons.Length; i++)
        {
            MechPart part = LoadPart(currentTab, i);
            spawnedButtons[i] = GameObject.Instantiate(partButtonPrefab, partsScrollView);
            spawnedButtons[i].transform.GetChild(0).GetComponent<Text>().text = part.name;

            Button bt = spawnedButtons[i].GetComponent<Button>();

            int eventID = i;

            UnityEngine.Events.UnityAction partEvent = () =>
            {
                    this.ChangePart(eventID);
            };

            bt.onClick.AddListener(partEvent);
        }
    }

    PassiveBonus currentBonus;

    void UpdateStatsPanel()
    {

        currentBonus = PartsDataBase.CalculateTotalBonus(mechData.headID, mechData.upperTorsoID, mechData.lowerTorsoID, mechData.legsID, mechData.armsID);

        int tempInt = PartsDataBase.CalculateTotalWeight(mechData);

        weightInputText.text = tempInt.ToString();

        tempInt = PartsDataBase.CalculateTotalMovement(tempInt, currentBonus.bonusMovement);
        movementInputText.text = tempInt.ToString();

        tempInt = PartsDataBase.CalculateTotalArmor(mechData.headID, mechData.upperTorsoID, mechData.lowerTorsoID, mechData.legsID, mechData.armsID);
        armorInputText.text = tempInt.ToString();

        tempInt = PartsDataBase.CalculateTotalAP(mechData.headID, mechData.upperTorsoID, mechData.lowerTorsoID, mechData.legsID, mechData.armsID);
        apInputText.text = tempInt.ToString();

        tempInt = PartsDataBase.CalculateTotalVision(currentBonus.bonusVision);

        visionInputText.text = tempInt.ToString();
    }

    public void ChangeWeaponType(int type)
    {
        currentWeaponType = type;
        FillAvailableWeapons();
    }

    public void UnEquipWeapon(int i)
    {
        switch (i)
        {
            case 0:
                mechData.hasWeaponL = false;
                mechData.weaponArmLType = -1;
                break;
            case 1:
                mechData.weaponArmRType = -1;
                mechData.hasWeaponR = false;
                break;
            case 2:
                mechData.weaponGimbalLType = -1;
                mechData.hasWeaponGimbalL = false;
                break;
            case 3:
                mechData.weaponGimbalRType = -1;
                mechData.hasWeaponGimbalR = false;
                break;
        }
    }

    void ChangeWeapon(int id)
    {
        
        switch (currentTab)
        {
            case 0:
                mechData.weaponArmLType = currentWeaponType;
                mechData.weaponArmLID = id;
                mechData.hasWeaponL = true;
                break;
            case 1:
                mechData.weaponArmRType = currentWeaponType;
                mechData.weaponArmRID = id;
                mechData.hasWeaponR = true;
                break;
            case 2:
                mechData.weaponGimbalLType = currentWeaponType;
                mechData.weaponGimbalLID = id;
                mechData.hasWeaponGimbalL = true;
                break;
            case 3:
                mechData.weaponGimbalRType = currentWeaponType;
                mechData.weaponGimbalRID = id;
                mechData.hasWeaponGimbalR = true;
                break;
        }

        RebuildMech();
    }

    void ChangePart(int id)
    {
        PartType pt = (PartType)currentTab;

        Debug.Log(id);
        switch (pt)
        {
            case PartType.Head:
                mechData.headID = id;
                break;
            case PartType.Arms:
                mechData.armsID = id;
                break;
            case PartType.Legs:
                mechData.legsID = id;
                break;
            case PartType.UpperTorso:
                mechData.upperTorsoID = id;
                break;
            case PartType.LowerTorso:
                mechData.lowerTorsoID = id;
                break;
        }

        RebuildMech();
    }

    void RebuildMech()
    {
        if (currentMech != null)
        {
            GameObject.Destroy(currentMech);
        }

        currentMech = MechIDConst.SpawnMech(pedestal.position, pedestal.rotation, mechData, false);
        currentMechVisualAgent = currentMech.GetComponent<MechVisualAgent>();

        currentMech.transform.parent = pedestal;

        UpdatePartText();
        UpdateStatsPanel();
        UpdateWeaponText();
        UpdateWeaponStats(currentMech.GetComponent<PlayerData>());
    }


	
}
