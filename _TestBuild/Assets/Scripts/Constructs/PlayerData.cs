using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour 
{


    public MechIDConst mechID;
    public MechVisualAgent visualAgent;
	//Player identifiers 
	public int playerNum;
	public int uniqueID;
	public string charName;

	//player baseline variables and current versions
	public int maxAP;
	public int maxHealth;
	public int maxMoveDist;
    public int curAP;
	public int curHealth;		
	public int curMoveDist;
	public Tile curTile;
	//used to show the range of tiles that the player can shoot in	
	public int curWeapRNG;
	public bool movementPending;

	//mech weapon variables - to be connected to the player weapon manager so that it uses this instead
	public Weapon rightArmWeapon;
	public Weapon leftArmWeapon;
	public Weapon backWeapon;

    public int rightArmWeaponType;
    public int leftArmWeaponType;
    public int backWeaponType;

    public bool rightArmWeaponFired = false;
    public bool leftArmWeaponFired = false;
    public bool backWeaponFired = false;

    public bool hasRightArmWeapon = false;
    public bool hasLeftArmWeapon = false;
    public bool hasBackWeapon = false;

    public bool isOutOfActions { get; private set; }

    public UIHealthBar myHealthBar;

    int minimumApToFire;

    public int CurrMoveRange()
    {
        if (curMoveDist < curAP)
        {
            return curMoveDist;
        }
        else
        {
            return curAP;
        }
    }

    public void TakeDelayedDamage(int damage, float time)
    {
        StartCoroutine(TakeDelayedDamageCoroutine(damage, time));
    }

    IEnumerator TakeDelayedDamageCoroutine(int damage, float time)
    {
        yield return new WaitForSeconds(time);

        TakeDamage(damage);

        yield break;
    }
       
    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        myHealthBar.UpdateHealth(GetHealthPercent(), damage);
        if (curHealth < 1)
            Death();
    }

    public float GetHealthPercent()
    {
        return  (float)curHealth / (float)maxHealth;
    }

    void CalculateMinimumApToFireWeapon()
    {
        minimumApToFire = 10000;
        int[] apCosts = new int[3];
        if (hasRightArmWeapon && !rightArmWeaponFired)
        {
            apCosts[0] = rightArmWeapon.apCost;
        }
        if (hasLeftArmWeapon && !leftArmWeaponFired)
        {
            apCosts[1] = leftArmWeapon.apCost;
        }
        if (hasBackWeapon && !backWeaponFired)
        {
            apCosts[2] = backWeapon.apCost;
        }

        for (int i = 0; i < apCosts.Length; i++)
        {
            if (apCosts[i] > 0)
            {
                if (minimumApToFire > apCosts[i])
                    minimumApToFire = apCosts[i];
            }
        }
    }

    public void PrepareForTurn()
    {
        isOutOfActions = false;
        curAP = maxAP;
        curMoveDist = maxMoveDist;
        rightArmWeaponFired = false;
        leftArmWeaponFired = false;
        backWeaponFired = false;
    }


    public bool IsOutOfActions()
    {
        bool ret = true;
        
        CalculateMinimumApToFireWeapon();
        //Debug.Log(string.Format("AP: {0} - AP Needed to Fire: {1}", curAP, minimumApToFire));

        // has the mech fired all of his weapons?
        if (!rightArmWeaponFired || !leftArmWeaponFired || !backWeaponFired)
        {
            // can the mech fire its cheapest weapon?
            if (curAP > minimumApToFire)
            {
                ret = false;
            }
        }

        // can the mech still move?
        if (curMoveDist > 0)
        {
            ret = false;
        }
            
        isOutOfActions = ret;
        // if all of the previous checks fail, the mech still has no actions remaining
        return ret;
    }

    public void FireWeapon(int wepID, PlayerData target)
    {
        Weapon weaponUsed;
        int weaponType;
        if (wepID == 0 && !leftArmWeaponFired)
        {
            weaponUsed = leftArmWeapon;
            weaponType = mechID.data.weaponArmLType;
            leftArmWeaponFired = true;
        }
        else if (wepID == 1 && !rightArmWeaponFired)
        {
            weaponUsed = rightArmWeapon;
            weaponType = mechID.data.weaponArmRType;
            rightArmWeaponFired = true;
        }
        else if (wepID == 2 && !backWeaponFired)
        {
            weaponUsed = backWeapon;
            weaponType = mechID.data.weaponGimbalLType;
            backWeaponFired = true;
        }
        else
        {
            Debug.LogWarning("This weapon cannot be fired");
            return;
        }

        curAP -= weaponUsed.apCost;

        WeaponFiringTypes.shooterWeaponID = wepID;
        WeaponFiringTypes.FireWeapon(weaponUsed, weaponType, target, visualAgent.GetParticleInfo(wepID));
    }

    public bool HasWeaponFired(int wepID)
    {
        if (wepID == 0 && !leftArmWeaponFired)
        {
            return leftArmWeaponFired;
        }
        else if (wepID == 1 && !rightArmWeaponFired)
        {
            return rightArmWeaponFired;
        }
        else if (wepID == 2 && !backWeaponFired)
        {
            return backWeaponFired;
        }
        else
        {
            return true;
        }
    }

    public void FireWeapon(int wepID, Tile target)
    {
        Weapon weaponUsed;
        int weaponType;
        if (wepID == 0 && !leftArmWeaponFired)
        {
            weaponUsed = leftArmWeapon;
            weaponType = mechID.data.weaponArmLType;
            leftArmWeaponFired = true;
        }
        else if (wepID == 1 && !rightArmWeaponFired)
        {
            weaponUsed = rightArmWeapon;
            weaponType = mechID.data.weaponArmRType;
            rightArmWeaponFired = true;
        }
        else if (wepID == 2 && !backWeaponFired)
        {
            weaponUsed = backWeapon;
            weaponType = mechID.data.weaponGimbalLType;
            backWeaponFired = true;
        }
        else
        {
            Debug.LogWarning("This weapon cannot be fired");
            return;
        }

        curAP -= weaponUsed.apCost;

        WeaponFiringTypes.shooterWeaponID = wepID;
        WeaponFiringTypes.FireWeapon(weaponUsed, weaponType, target, visualAgent.GetParticleInfo(wepID));
    }

    public int GetWeaponMaxRange(int wepID)
    {
        if (wepID == 0)
            return leftArmWeapon.maxRange;
        else if (wepID == 1)
            return rightArmWeapon.maxRange;
        else if (wepID == 2)
            return backWeapon.maxRange;
        else
        {
            Debug.LogError("Invalid ID for weapon: " + wepID + " - Expected 0 - 2");
            return 0;
        }
    }

    public int GetWeaponMinRange(int wepID)
    {
        if (wepID == 0)
            return leftArmWeapon.minRange;
        else if (wepID == 1)
            return rightArmWeapon.minRange;
        else if (wepID == 2)
            return backWeapon.minRange;
        else
        {
            Debug.LogError("Invalid ID for weapon: " + wepID + " - Expected 0 - 2");
            return 0;
        }
    }

    public int GetWeaponScatter(int wepID)
    {
        if (wepID == 0)
            return leftArmWeapon.scatter;
        else if (wepID == 1)
            return rightArmWeapon.scatter;
        else if (wepID == 2)
            return backWeapon.scatter;
        else
        {
            Debug.LogError("Invalid ID for weapon: " + wepID + " - Expected 0 - 2");
            return 0;
        }
    }

    public Weapon GetWeapon(int wepID)
    {
        if (wepID == 0)
            return leftArmWeapon;
        else if (wepID == 1)
            return rightArmWeapon;
        else if (wepID == 2)
            return backWeapon;
        else
        {
            Debug.LogError("Invalid ID for weapon: " + wepID + " - Expected 0 - 2");
            return new Weapon();
        }
    }

    void Death()
    {
            PlayerManager.gPlayer.currentPlayers.Remove(this.gameObject);
            PlayerManager.gPlayer.allPlayerMechs[playerNum].Remove(this);
        curTile.occupyingObj = null;
        myHealthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
		EndScreenManager.Instance.Playercheck ();
    }
        
    public void Initialize(PlayerManager playerManager, int playerID)
    {
        playerManager.allPlayerMechs[playerID].Add(this);
        playerManager.currentPlayers.Add(this.gameObject);
    }

    public bool WeaponTargetsDirectly(int wepID)
    {
        int weaponType;
        if (wepID == 0 && !leftArmWeaponFired)
        {
            weaponType = mechID.data.weaponArmLType;
        }
        else if (wepID == 1 && !rightArmWeaponFired)
        {
            weaponType = mechID.data.weaponArmRType;
        }
        else if (wepID == 2 && !backWeaponFired)
        {
            weaponType = mechID.data.weaponGimbalLType;
        }
        else
        {
            Debug.LogWarning("This weapon is not equipped");
            return false;
        }

        if (weaponType == 2 || weaponType == 3)
            return false;
        else
            return true;
    }
}
