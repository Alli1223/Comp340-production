using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFiringTypes 
{
    public static PlayerData shooter;
    public static int shooterWeaponID;

    static WeaponParticleInfo ParticleInfo;

    public static void FireWeapon(Weapon weaponFired, int weaponType, PlayerData target, WeaponParticleInfo particleInfo)
	{
        ParticleInfo = particleInfo;
		switch (weaponType)
		{
			case 0:
				Autocannon(ref weaponFired, target);
				break;
			case 1:
				Chaingun(ref weaponFired, target);
				break;
			case 2:
				Mortar(ref weaponFired, target);
				break;
			case 3:
				Missile(ref weaponFired, target);
				break;
			case 4:
				Rocket(ref weaponFired, target);
				break;
			case 5:
				Scatter(ref weaponFired, target);
				break;
			case 6:
				Sniper(ref weaponFired, target);
				break;
			case 7:
				Snub(ref weaponFired, target);
				break;
		}


	}

    public static void FireWeapon(Weapon weaponFired, int weaponType, Tile target, WeaponParticleInfo particleInfo)
	{
        ParticleInfo = particleInfo;
		switch (weaponType)
		{
			case 0:
				Autocannon(ref weaponFired, target);
				break;
			case 1:
				Chaingun(ref weaponFired, target);
				break;
			case 2:
				Mortar(ref weaponFired, target);
				break;
			case 3:
				Missile(ref weaponFired, target);
				break;
			case 4:
				Rocket(ref weaponFired, target);
				break;
			case 5:
				Scatter(ref weaponFired, target);
				break;
			case 6:
				Sniper(ref weaponFired, target);
				break;
			case 7:
				Snub(ref weaponFired, target);
				break;
		}
	}

	static void Autocannon(ref Weapon weaponFired, Tile target)
	{
		Autocannon(ref weaponFired, target.occupyingObj.GetComponent<PlayerData>());
	}

	static void Autocannon(ref Weapon weaponFired, PlayerData target)
	{
        Vector3 targetPos;
        if (Random.value < weaponFired.accuracy * 0.01f)
        {
            target.TakeDelayedDamage(Random.Range(weaponFired.minDamage, weaponFired.maxDamage), shooter.visualAgent.GetCinematicTimeOfWeapon(shooterWeaponID));
            targetPos = target.visualAgent.upperTorsoParent.position;
        }
        else
        {
            targetPos = (target.visualAgent.upperTorsoParent.position - shooter.transform.position) * 10f;
        }

        ParticleInfo.cinematicTarget = targetPos;
        shooter.visualAgent.FireWeapon(shooterWeaponID);
	}

	static void Chaingun(ref Weapon weaponFired, Tile target)
	{
        Chaingun(ref weaponFired, target.occupyingObj.GetComponent<PlayerData>());
	}

	static void Chaingun(ref Weapon weaponFired, PlayerData target)
	{
        Vector3 targetPos;
		if (Random.value < weaponFired.accuracy * 0.01f) 
		{
            target.TakeDelayedDamage(Random.Range(weaponFired.minDamage, weaponFired.maxDamage), shooter.visualAgent.GetCinematicTimeOfWeapon(shooterWeaponID));
            targetPos = target.visualAgent.upperTorsoParent.position;
        }
        else
        {
            targetPos = (target.visualAgent.upperTorsoParent.position - shooter.transform.position) * 10f;
        }

        ParticleInfo.cinematicTarget = targetPos;
        shooter.visualAgent.FireWeapon(shooterWeaponID);
	}

	static void Snub(ref Weapon weaponFired, Tile target)
	{
		Snub(ref weaponFired, target.occupyingObj.GetComponent<PlayerData>());
	}

	static void Snub(ref Weapon weaponFired, PlayerData target)
	{
        Vector3 targetPos;

		int dmg = 0;
		if (Random.value < weaponFired.accuracy * 0.01f)
		{
			dmg += Random.Range(weaponFired.minDamage, weaponFired.maxDamage);
		}

		if (Random.value < weaponFired.accuracy * 0.01f - 0.2f)
		{
			// using splash radius as min dmg, and scatter as max dmg for the second shot
			dmg += Random.Range(weaponFired.splashRadius, weaponFired.scatter);
		}

        if (dmg != 0)
        {
            target.TakeDelayedDamage(dmg, shooter.visualAgent.GetCinematicTimeOfWeapon(shooterWeaponID));
            targetPos = target.visualAgent.upperTorsoParent.position;
        }
        else
        {
            targetPos = (target.visualAgent.upperTorsoParent.position - shooter.transform.position) * 10f;
        }

        ParticleInfo.cinematicTarget = targetPos;
        shooter.visualAgent.FireWeapon(shooterWeaponID);

	}

	static void Mortar(ref Weapon weaponFired, Tile target)
	{
		List<Tile> tilesInScatter = GridExtentions._TilesInARange(target.tPos, weaponFired.scatter);

        Vector3 targetPos = Vector3.zero;
		for (int i = 0; i < weaponFired.numberOfShots; i++)
		{
			int splashCenter = Random.Range(0, tilesInScatter.Count - 1);
            targetPos = tilesInScatter[splashCenter].tPos;
			if(weaponFired.splashRadius > 0)
			{
				List<Tile> splashTiles = GridExtentions._TilesInARange(tilesInScatter[splashCenter].tPos, weaponFired.splashRadius);

				for (int c = 0; c < splashTiles.Count; c++)
				{
					if (splashTiles[c].occupyingObj != null)
					{
						int dmg = Random.Range(weaponFired.minDamage, weaponFired.maxDamage) - GridExtentions.DistCheck(tilesInScatter[splashCenter].tPos, splashTiles[c].tPos) * 2;
						Mathf.Clamp(dmg, 0, weaponFired.maxDamage);
                        splashTiles[c].occupyingObj.GetComponent<PlayerData>().TakeDelayedDamage(dmg, shooter.visualAgent.GetCinematicTimeOfWeapon(shooterWeaponID));

					}
				}
			}
		}

        ParticleInfo.cinematicTarget = targetPos;
        shooter.visualAgent.FireWeapon(shooterWeaponID);
	}

	static void Mortar(ref Weapon weaponFired, PlayerData target)
	{
		Mortar(ref weaponFired, target.curTile);
	}

	static void Missile(ref Weapon weaponFired, Tile target)
	{
		List<Tile> tilesInSplash = GridExtentions._TilesInARange(target.tPos, weaponFired.splashRadius);
		for (int i = 0; i < tilesInSplash.Count; i++)
		{
			if (tilesInSplash[i].occupyingObj != null)
			{
				int dmg = Random.Range(weaponFired.minDamage, weaponFired.maxDamage) - GridExtentions.DistCheck(target.tPos, tilesInSplash[i].tPos) * 2;
				Mathf.Clamp(dmg, 0, weaponFired.maxDamage);
                tilesInSplash[i].occupyingObj.GetComponent<PlayerData>().TakeDelayedDamage(dmg, shooter.visualAgent.GetCinematicTimeOfWeapon(shooterWeaponID));

			}
		}

        ParticleInfo.cinematicTarget = target.tPos;
        shooter.visualAgent.FireWeapon(shooterWeaponID);
	}

	static void Missile(ref Weapon weaponFired, PlayerData target)
	{
		Missile(ref weaponFired, target.curTile);
	}

	static void Rocket(ref Weapon weaponFired, PlayerData target)
	{
        List<Tile> tilesInSplash;
        Vector3 targetPos = Vector3.zero;
        if (Random.value < weaponFired.accuracy * 0.01f)
        {
            tilesInSplash = GridExtentions._TilesInARange(target.curTile.tPos, weaponFired.splashRadius);
            targetPos = target.transform.position;
        }
        else
        {
            List<Tile> tilesInScatter = GridExtentions._TilesInARange(target.curTile.tPos, weaponFired.scatter);
            for (int i = tilesInScatter.Count - 1; i > -1; i--)
            {
                if(LineOfSightFunctions._TileSight(shooter.curTile.tPos, target.curTile.tPos) == cover.Full)
                {
                    tilesInScatter.RemoveAt(i);
                }
            }
            int splashCenter = Random.Range(0, tilesInScatter.Count);
            tilesInSplash = GridExtentions._TilesInARange(tilesInScatter[splashCenter].tPos, weaponFired.splashRadius);
            targetPos = tilesInScatter[splashCenter].tPos;
        }

        for (int i = 0; i < tilesInSplash.Count; i++)
        {
            if (tilesInSplash[i].occupyingObj != null)
            {
                tilesInSplash[i].occupyingObj.GetComponent<PlayerData>().TakeDelayedDamage(Random.Range(weaponFired.minDamage, weaponFired.maxDamage), shooter.visualAgent.GetCinematicTimeOfWeapon(shooterWeaponID));
            }
        }
        ParticleInfo.cinematicTarget = targetPos;
        shooter.visualAgent.FireWeapon(shooterWeaponID);

	}
        

	static void Rocket(ref Weapon weaponFired, Tile target)
	{
        Rocket(ref weaponFired, target.occupyingObj.GetComponent<PlayerData>());
	}

	static void Scatter(ref Weapon weaponFired, PlayerData target)
	{
        Vector3 targetPos;
		if (Random.value < weaponFired.accuracy * 0.01f) 
		{
            target.TakeDelayedDamage(Random.Range(weaponFired.minDamage, weaponFired.maxDamage), shooter.visualAgent.GetCinematicTimeOfWeapon(shooterWeaponID));
            targetPos = target.visualAgent.upperTorsoParent.position;
        }
        else
        {
            targetPos = (target.visualAgent.upperTorsoParent.position - shooter.transform.position) * 10f;
        }

        ParticleInfo.cinematicTarget = targetPos;
        shooter.visualAgent.FireWeapon(shooterWeaponID);
	}

	static void Scatter(ref Weapon weaponFired, Tile target)
	{
		Scatter(ref weaponFired, target.occupyingObj.GetComponent<PlayerData>());
	}

	static void Sniper(ref Weapon weaponFired, PlayerData target)
	{
        Vector3 targetPos;
		if (Random.value < weaponFired.accuracy * 0.01f) 
		{
            target.TakeDelayedDamage(Random.Range(weaponFired.minDamage, weaponFired.maxDamage), shooter.visualAgent.GetCinematicTimeOfWeapon(shooterWeaponID));
            targetPos = target.visualAgent.upperTorsoParent.position;
        }
        else
        {
            targetPos = (target.visualAgent.upperTorsoParent.position - shooter.transform.position) * 10f;
        }

        ParticleInfo.cinematicTarget = targetPos;
        shooter.visualAgent.FireWeapon(shooterWeaponID);
	}

	static void Sniper(ref Weapon weaponFired, Tile target)
	{
		Sniper(ref weaponFired, target.occupyingObj.GetComponent<PlayerData>());
	}


}
