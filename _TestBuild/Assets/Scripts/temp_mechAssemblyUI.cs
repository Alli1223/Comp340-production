using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class temp_mechAssemblyUI : MonoBehaviour 
{
    /*
    public GameObject headPanel;

    public Text textFieldHead;
    public Text textFieldUpperTorso;
    public Text textFieldLowerTorso;
    public Text textFieldArms;
    public Text textFieldLegs;
    public Text textFieldColorName;
    public Image imageColorPreview;
    public RectTransform rectTrColorViewport;
    public Transform pedestal;

    // TEMP this should be removed and the script should be able to run using MechIDConst class fully
    int currentHeadID;
    int currentUpperTorsoID;
    int currentLowerTorsoID;
    int currentArmsID;
    int currentLegsID;

    GameObject currentUpperTorsoObj;
    GameObject currentLowerTorsoObj;

    Transform upperTorsoParent;

    Animator animUT;
    Animator animLT;

    MechIDConst currentMechConst;
    MechVisualAgent mechVisualAgent;

    GameObject weaponArmL;
    GameObject weaponArmR;
    GameObject weaponGimbalL;

    public GameObject colorSquarePrefab;

    GameObject[] colorSquares;

    string[] weaponTypeNames = new string[8] {"Auto Cannon", "Chaingun", "Mortar", "Missile Silo", "Rocket Silo", "Scatter Cannnon", "Sniper Cannon", "Snub Cannon"};
    public Text armWpnTypeTextR;
    public Text armWpnTypeTextL;
    public Text gimbalWpnTypeTextL;

    int currentColorMask = 0;

    bool animationsEnabled = true;
    int[] mechColors = new int[3];

    void FillInColorSquares(int colorMask)
    {
        colorSquares = new GameObject[MechColorArray.GetArrayLength()];
        for (int i = 0; i < colorSquares.Length; i++)
        {
            colorSquares[i] = GameObject.Instantiate(colorSquarePrefab, rectTrColorViewport);
            Button bt = colorSquares[i].GetComponent<Button>();
            bt.image.color = MechColorArray.GetColor(i);

            int indexCopy = i;
            UnityEngine.Events.UnityAction colorPick = () =>
                {
                    this.SetColor(indexCopy);
                };
            bt.onClick.AddListener(colorPick);
        }
    }

    void UpdateColorUI()
    {
        textFieldColorName.text = MechColorArray.GetColorName(mechColors[currentColorMask]);
        imageColorPreview.color = MechColorArray.GetColor(mechColors[currentColorMask]);
    }

    void UpdateColors()
    {
        UpdateColorUI();
        for (int i = 0; i < 3; i++)
        {
            mechVisualAgent.ChangeColor(i, MechColorArray.GetColor(mechColors[i]));
        }
    }

    public void SelectColorMask(int id)
    {
        currentColorMask = id;
        UpdateColorUI();
    }

    void SetColor(int id)
    {
        mechColors[currentColorMask] = id;
        UpdateColors();
    }

	void Start () 
    {
        // TEMP this should probably be loaded in a different script, preferably as soon as the game runs
        FillInColorSquares(currentColorMask);
        AssembleLT(0, 0, false);
        AssembleUT(0, 0, 0, false);
        ChangeWeaponTypeArmL(0);
        ChangeWeaponTypeArmR(0);
        ChangeWeaponTypeGimbalL(0);
        ConfigureVisualAgent();
	}

    public void SaveCurrentMech()
    {
        currentMechConst.displayName = "testSaveMech";
        MechSaveData.SaveMech(currentMechConst);
    }

    public void LoadMech()
    {
        MechSaveData.LoadMech(ref currentMechConst, "testSaveMech");

        currentLegsID = currentMechConst.assetIDLegs;
        currentHeadID = currentMechConst.assetIDHead;
        currentArmsID = currentMechConst.assetIDArms;
        currentUpperTorsoID = currentMechConst.assetIDUpperTorso;
        currentLowerTorsoID = currentMechConst.assetIDLowerTorso;

        AssembleLT(currentLegsID, currentLowerTorsoID, false);
        AssembleUT(currentHeadID, currentUpperTorsoID, currentArmsID, false);
    }
	
    void EnableHeadPanel(bool enable)
    {
        headPanel.SetActive(enable);
    }

    // Used by UI next / previous buttons
    public void ChangeHead(int Add)
    {
        AssembleUT(Add, 0, 0, true);
    }
    // Used by UI next / previous buttons
    public void ChangeArms(int Add)
    {
        AssembleUT(0, 0, Add, true);
    }
    // Used by UI next / previous buttons
    public void ChangeUT(int Add)
    {
        AssembleUT(0, Add, 0, true);
    }
    // Used by UI next / previous buttons
    public void ChangeLT(int Add)
    {
        AssembleLT(0, Add, true);
    }
    // Used by UI next / previous buttons
    public void ChangeLegs(int Add)
    {
        AssembleLT(Add, 0, true);
    }

    // Used for animations testing, triggered by UI
    public void LTorsoTurnAngle(float turn)
    {
        animLT.SetFloat("turnDegree", turn);
    }

    // Used for animations testing, triggered by UI
    public void LTorsoTurn()
    {
        animLT.SetTrigger("turn");
    }
    // Used for animations testing, triggered by UI
    public void ToggleAnimations()
    {
        if (animationsEnabled)
        {
            animationsEnabled = false;
            animLT.SetLayerWeight(1, 0f);
            animUT.SetLayerWeight(1, 0f);
        }
        else
        {
            animationsEnabled = true;
            animLT.SetLayerWeight(1, 1f);
            animUT.SetLayerWeight(1, 1f);
        }
    }
    // Used for animations testing, triggered by UI
    public void ToggleMove()
    {
        if (animLT.GetBool("walk"))
            animLT.SetBool("walk", false);
        else
            animLT.SetBool("walk", true);
    }

    // Used for animations testing, triggered by UI
    public void SetAnimWalkSpeed(float speed)
    {
        animLT.SetFloat("walkSpeed", speed);
    }

    // Used for animations testing, triggered by UI
    public void SetAnimWalkWeight(float weight)
    {
        animLT.SetFloat("walkWeight", weight);
    }

    // Used for animations testing, triggered by UI
    public void SetAnimWalkSpeedMod(float mod)
    {
        animLT.SetFloat("walkSpeedMultiplier", mod);
    }

    // Function used to assemble or re-assemble a new upper torso
    public void AssembleUT(int headIDAdd, int torsoIDAdd, int armsIDAdd, bool additive)
    {
        // delete the old upper torso (and all the parts that it contained)
        if (currentUpperTorsoObj != null)
        {
            GameObject.Destroy(currentUpperTorsoObj);
        }

        // should the inputs be treated as a previous / next or jump onto the specified id
        // On additive, make sure the ID doesnt go out of the array range
        if (additive)
        {
            int maxHeadID = UTRigAssembler.Instance.HDData.Length - 1;
            int maxTorsoID = UTRigAssembler.Instance.UTData.Length - 1;
            int maxArmID = UTRigAssembler.Instance.ARData.Length - 1;

            currentHeadID += headIDAdd;
            currentUpperTorsoID += torsoIDAdd;
            currentArmsID += armsIDAdd;

            if (currentHeadID > maxHeadID)
                currentHeadID = 0;
            else if (currentHeadID < 0)
                currentHeadID = maxHeadID;

            if (currentUpperTorsoID > maxTorsoID)
                currentUpperTorsoID = 0;
            else if (currentUpperTorsoID < 0)
                currentLowerTorsoID = maxTorsoID;

            if (currentArmsID > maxArmID)
                currentArmsID = 0;
            else if (currentArmsID < 0)
                currentArmsID = maxArmID;
        }


        // spawn in the new upper torso on the specified object, and get required variables for other components
        currentUpperTorsoObj = UTRigAssembler.Instance.AssembleUpperTorso(currentUpperTorsoID, currentArmsID, currentHeadID, upperTorsoParent,
            out currentMechConst.weaponMountL, out currentMechConst.weaponMountR, out currentMechConst.shieldMountL, out currentMechConst.shieldMountR,
            out currentMechConst.gimbalMountL, out currentMechConst.gimbalMountR, out animUT);

        // if this specific torso has a head mount
        if (UTRigAssembler.Instance.UTData[currentUpperTorsoID].hasHead)
        {
            // then enable the panel that allows changes to head part
            EnableHeadPanel(true);
            textFieldHead.text = UTRigAssembler.Instance.HDData[currentHeadID].asset.name;
        }
        else
        {
            // if not then disable that panel
            EnableHeadPanel(false);
        }
            
        // update names of the part on UI
        textFieldUpperTorso.text = UTRigAssembler.Instance.UTData[currentUpperTorsoID].asset.name;
        textFieldArms.text = UTRigAssembler.Instance.ARData[currentArmsID].asset.name;

        // if animations are not enabled then set 2nd layer weight to 0
        if (!animationsEnabled)
            animUT.SetLayerWeight(1, 0f);
        
        // update variables required by MechIDConst
        UpdateMechInfoUpper(ref currentMechConst);

        // ensures correct weapon are spawned in as replacement
        TransferWeapons(ref currentMechConst);
        // TEMP Updates the weapon UI text, this should probably be run in TransferWeapons() instead
        UpdateWeaponUIText();
        mechVisualAgent.UpdateRendererReferences();
        UpdateColors();
    }

    public void AssembleLT(int legIDAdd, int torsoIDAdd, bool additive)
    {
        GameObject oldLowerTorsoOBJ = currentLowerTorsoObj;

        // used to store the old animation variables to ensure consistency on change
        float oldAnimWalkSpeed = 0f;
        float oldAnimWalkWeight = 0f;
        float oldAnimWalkSpeedModifier = 0f;
        float oldAnimTurnDegree = 0f;
        bool oldAnimWalk = false;

        // fill in the animation variables if a mech already exists
        if (currentLowerTorsoObj != null)
        {
            if (currentUpperTorsoObj != null)
            {
                // temporarely detach the upper torso
                currentUpperTorsoObj.transform.SetParent(null);
            }
            oldAnimWalk = animLT.GetBool("walk");
            oldAnimWalkSpeed = animLT.GetFloat("walkSpeed");
            oldAnimWalkWeight = animLT.GetFloat("walkWeight");
            oldAnimWalkSpeedModifier = animLT.GetFloat("walkSpeedMultiplier");
            oldAnimTurnDegree = animLT.GetFloat("turnDegree");


        }

        // should the inputs be treated as a previous / next or jump onto the specified id
        // On additive, make sure the ID doesnt go out of the array range
        if (additive)
        {
            int maxLegID = LTRigAssembler.Instance.LGData.Length - 1;
            int maxTorsoID = LTRigAssembler.Instance.LTData.Length - 1;

            currentLegsID += legIDAdd;
            currentLowerTorsoID += torsoIDAdd;

            if (currentLegsID > maxLegID)
                currentLegsID = 0;
            else if (currentLegsID < 0)
                currentLegsID = maxLegID;

            if (currentLowerTorsoID > maxTorsoID)
                currentLowerTorsoID = 0;
            else if (currentLowerTorsoID < 0)
                currentLowerTorsoID = maxTorsoID;
        }

        // spawn in the lower torso and fill required variables
        currentLowerTorsoObj = LTRigAssembler.Instance.AssembleLowerTorso(currentLowerTorsoID, currentLegsID, Vector3.zero, Quaternion.identity, out upperTorsoParent, out animLT);

        // Update UI names
        textFieldLowerTorso.text = LTRigAssembler.Instance.LTData[currentLowerTorsoID].asset.name;
        textFieldLegs.text = LTRigAssembler.Instance.LGData[currentLegsID].asset.name;

        // if the last mech was walking, set this mech to walk aswell
        if(oldAnimWalk)
            ToggleMove();

        // set all animation variables to the old variables
        SetAnimWalkSpeed(oldAnimWalkSpeed);
        SetAnimWalkWeight(oldAnimWalkWeight);
        SetAnimWalkSpeedMod(oldAnimWalkSpeedModifier);
        LTorsoTurnAngle(oldAnimTurnDegree);

        // if upper torso exists, reattach it to the lower torso
        if (currentUpperTorsoObj != null)
        {
            currentUpperTorsoObj.transform.SetParent(upperTorsoParent);
            currentUpperTorsoObj.transform.localPosition = Vector3.zero;
            currentUpperTorsoObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        // add a new MechIDConst to the new gameobject, then sync all the values and finally remove the old mech
        MechIDConst newMechInfo = currentLowerTorsoObj.AddComponent<MechIDConst>();
        mechVisualAgent = currentLowerTorsoObj.AddComponent<MechVisualAgent>();
        if (oldLowerTorsoOBJ != null)
        {
            CopyOldMechInfo(oldLowerTorsoOBJ.GetComponent<MechIDConst>(), ref newMechInfo);
            GameObject.Destroy(oldLowerTorsoOBJ);
            ConfigureVisualAgent();
        }
        else
        {
            UpdateMechInfoLower(ref newMechInfo);
        }

        // if animations are not enabled, set 2nd layer weight to 0
        if (!animationsEnabled)
            animLT.SetLayerWeight(1, 0f);

        // update the reference to the current mech
        currentMechConst = newMechInfo;
        mechVisualAgent.UpdateRendererReferences();
        UpdateColors();
        currentLowerTorsoObj.transform.SetParent(pedestal);
        currentLowerTorsoObj.transform.localRotation = Quaternion.Euler(-90f, 0f,0f);
    }

    void ConfigureVisualAgent()
    {
        mechVisualAgent.weaponMountL = currentMechConst.weaponMountL;
        mechVisualAgent.weaponMountR = currentMechConst.weaponMountR;
        mechVisualAgent.weaponMountGimbalL = currentMechConst.gimbalMountL;
        mechVisualAgent.weaponMountGimbalR = currentMechConst.gimbalMountR;
        mechVisualAgent.shieldMountL = currentMechConst.shieldMountL;
        mechVisualAgent.shieldMountR = currentMechConst.shieldMountR;

        mechVisualAgent.animLowerTorso = animLT;
        mechVisualAgent.animUpperTorso = animUT;
        mechVisualAgent.UpdateRendererReferences();
    }


    // TEMP this needs a bit more work for efficiency, currently it replaces all weapons even if only 1 was changed
    void TransferWeapons(ref MechIDConst mechInfo)
    {
        GameObject go;


        if (weaponArmL != null)
            GameObject.Destroy(weaponArmL);

        if (weaponArmR != null)
            GameObject.Destroy(weaponArmR);

        if (weaponGimbalL != null)
            GameObject.Destroy(weaponGimbalL);

        UTRigAssembler.ArmType armType = UTRigAssembler.Instance.ARData[mechInfo.assetIDArms].armType;

        if (WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmLType].weapons.Length > currentMechConst.weaponArmLID)
        {
            weaponArmL = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmLType].weapons[currentMechConst.weaponArmLID].weaponAsset, mechInfo.weaponMountL) as GameObject;
            weaponArmL.transform.localPosition = Vector3.zero;
            weaponArmL.transform.localRotation = Quaternion.Euler(Vector3.zero);
			mechVisualAgent.animWeaponL = weaponArmL.GetComponent<Animator> ();

            if (armType == UTRigAssembler.ArmType.SingleJoint)
            {
                go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmLType].weapons[currentMechConst.weaponArmLID].weaponGimbalMountAsset, weaponArmL.transform) as GameObject;
            }
            else
            {
                go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmLType].weapons[currentMechConst.weaponArmLID].weaponStaticMountAsset, weaponArmL.transform) as GameObject;
            }
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }


        if (WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmRType].weapons.Length > currentMechConst.weaponArmRID)
        {
            weaponArmR = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmRType].weapons[currentMechConst.weaponArmRID].weaponAsset, mechInfo.weaponMountR) as GameObject;
            weaponArmR.transform.localPosition = Vector3.zero;
            weaponArmR.transform.localRotation = Quaternion.Euler(Vector3.zero);
			mechVisualAgent.animWeaponR = weaponArmR.GetComponent<Animator> ();

            if (armType == UTRigAssembler.ArmType.SingleJoint)
            {
                go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmRType].weapons[currentMechConst.weaponArmRID].weaponGimbalMountAsset, weaponArmR.transform) as GameObject;
            }
            else
            {
                go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmRType].weapons[currentMechConst.weaponArmRID].weaponStaticMountAsset, weaponArmR.transform) as GameObject;
            }
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }


        if (WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponGimbalLType].weapons.Length > currentMechConst.weaponGimbalLID)
        {
            weaponGimbalL = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponGimbalLType].weapons[currentMechConst.weaponGimbalLID].weaponAsset, mechInfo.gimbalMountL) as GameObject;
            weaponGimbalL.transform.localPosition = Vector3.zero;
            weaponGimbalL.transform.localRotation = Quaternion.Euler(Vector3.zero);

			mechVisualAgent.animGimbalL = weaponGimbalL.GetComponent<Animator> ();
            go = GameObject.Instantiate(WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponGimbalLType].weapons[currentMechConst.weaponGimbalLID].weaponGimbalMountAsset, weaponGimbalL.transform) as GameObject;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			mechVisualAgent.FireWeapon (0);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			mechVisualAgent.FireWeapon (1);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			mechVisualAgent.FireWeapon (2);
		}
	}

    // makes sure all required variables are transfered over on change
    void UpdateMechInfoLower(ref MechIDConst mechInfo)
    {

        mechInfo.assetIDLowerTorso = currentLowerTorsoID;
        mechInfo.assetIDLegs = currentLegsID;

        if (currentMechConst != null)
        {
            mechInfo.weaponArmLID = currentMechConst.weaponArmLID;
            mechInfo.weaponArmLType = currentMechConst.weaponArmLType;
            mechInfo.weaponArmRID = currentMechConst.weaponArmRID;
            mechInfo.weaponArmRType = currentMechConst.weaponArmRType;
            mechInfo.weaponGimbalLID = currentMechConst.weaponGimbalLID;
            mechInfo.weaponGimbalLType = currentMechConst.weaponGimbalLType;
            mechInfo.weaponGimbalRID = currentMechConst.weaponGimbalRID;
            mechInfo.weaponGimbalRType = currentMechConst.weaponGimbalRType;
        }
    }

    // makes sure all required variables are transfered over on change
    void UpdateMechInfoUpper(ref MechIDConst mechInfo)
    {


        mechInfo.assetIDHead = currentHeadID;
        mechInfo.assetIDUpperTorso = currentUpperTorsoID;
        mechInfo.assetIDArms = currentArmsID;

    }

    // makes sure all required variables are transfered over on change
    void CopyOldMechInfo(MechIDConst oldMech, ref MechIDConst newMech)
    {

        newMech.assetIDHead = currentHeadID;
        newMech.assetIDUpperTorso = currentUpperTorsoID;
        newMech.assetIDLowerTorso = currentLowerTorsoID;
        newMech.assetIDLegs = currentLegsID;
        newMech.assetIDArms = currentArmsID;

        newMech.weaponMountL = oldMech.weaponMountL;
        newMech.weaponMountR = oldMech.weaponMountR;
        newMech.shieldMountL = oldMech.shieldMountL;
        newMech.shieldMountR = oldMech.shieldMountR;
        newMech.gimbalMountL = oldMech.gimbalMountL;
        newMech.gimbalMountR = oldMech.gimbalMountR;

        newMech.weaponArmLID = oldMech.weaponArmLID;
        newMech.weaponArmLType = oldMech.weaponArmLType;
        newMech.weaponArmRID = oldMech.weaponArmRID;
        newMech.weaponArmRType = oldMech.weaponArmRType;
        newMech.weaponGimbalLID = oldMech.weaponGimbalLID;
        newMech.weaponGimbalLType = oldMech.weaponGimbalLType;
        newMech.weaponGimbalRID = oldMech.weaponGimbalRID;
        newMech.weaponGimbalRType = oldMech.weaponGimbalRType;
    }



    public void ChangeWeaponTypeArmL(int Add)
    {
        int maxID = WeaponDataBase.Instance.weaponTypeArray.Length - 1;
        currentMechConst.weaponArmLType += Add;
        if (currentMechConst.weaponArmLType > maxID)
        {
            currentMechConst.weaponArmLType = 0;
        }
        else if (currentMechConst.weaponArmLType < 0)
        {
            currentMechConst.weaponArmLType = maxID;
        }
            
        if (WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmLType].weapons.Length != 0)
        {
            TransferWeapons(ref currentMechConst);
        }


        armWpnTypeTextL.text = weaponTypeNames[currentMechConst.weaponArmLType];
    }

    public void ChangeWeaponTypeArmR(int Add)
    {
        int maxID = WeaponDataBase.Instance.weaponTypeArray.Length - 1;
        currentMechConst.weaponArmRType += Add;
        if (currentMechConst.weaponArmRType > maxID)
        {
            currentMechConst.weaponArmRType = 0;
        }
        else if (currentMechConst.weaponArmRType < 0)
        {
            currentMechConst.weaponArmRType = maxID;
        }

        if (WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponArmRType].weapons.Length != 0)
        {
            TransferWeapons(ref currentMechConst);
        }


        armWpnTypeTextR.text = weaponTypeNames[currentMechConst.weaponArmRType];
    }

    public void ChangeWeaponTypeGimbalL(int Add)
    {
        int maxID = WeaponDataBase.Instance.weaponTypeArray.Length - 1;
        currentMechConst.weaponGimbalLType += Add;
        if (currentMechConst.weaponGimbalLType > maxID)
        {
            currentMechConst.weaponGimbalLType = 0;
        }
        else if (currentMechConst.weaponGimbalLType < 0)
        {
            currentMechConst.weaponGimbalLType = maxID;
        }
            
        if (WeaponDataBase.Instance.weaponTypeArray[currentMechConst.weaponGimbalLType].weapons.Length != 0)
        {
            TransferWeapons(ref currentMechConst);
        }

        gimbalWpnTypeTextL.text = weaponTypeNames[currentMechConst.weaponGimbalLType];
    }

    void UpdateWeaponUIText()
    {
        armWpnTypeTextL.text = weaponTypeNames[currentMechConst.weaponArmLType];
        armWpnTypeTextR.text = weaponTypeNames[currentMechConst.weaponArmRType];
        gimbalWpnTypeTextL.text = weaponTypeNames[currentMechConst.weaponGimbalLType];
    }
    */
}
