using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemyMove : MonoBehaviour 
{
	public static EnemyMove tEnemyMove;

	private ManagersManager tManage;
	private Transform enemyTile;
	private List<Transform> tilesClosest = new List<Transform> ();


	//Makes Grid gen script a singleton
	void Awake()
	{
		if (tEnemyMove == null)
			tEnemyMove = this;
		else
			Destroy(this); 

		tManage = ManagersManager.manager;

        enemyTile = GridPositionDetection.GetClosestGrid (EnemyManager.currentEnemy.transform.position, tManage.tGrid.currentTiles);
		tManage.tDetect.FindTilesInDist (enemyTile, tManage.tGrid.tileMeshs, (float)EnemyManager.currentEnemy.GetComponent<TempEnemyVar> ().moveDist);
		tilesClosest.AddRange(tManage.tDetect.FindTilesInDist (enemyTile, tManage.tGrid.tileMeshs, EnemyManager.currentEnemy.GetComponent<TempEnemyVar>().moveDist));
	}

	public void Move()
	{
		ResetTiles ();
		Transform moveToPoint = tManage.tDetect.RealtivePosition (tilesClosest, tManage.tDetect.RealtivePosition (tManage.tPlayer.currentPlayers, enemyTile));
		TempEnemyVar curEnem = EnemyManager.currentEnemy.GetComponent<TempEnemyVar>();
		EnemyManager.currentEnemy.GetComponent<NavMeshAgent> ().SetDestination (moveToPoint.position);
        curEnem.currentAP -= GridPositionDetection.DistCheck(enemyTile.position, moveToPoint.position);
        curEnem.moveDist -= GridPositionDetection.DistCheck(enemyTile.position, moveToPoint.position);
	}
    
    public void Shoot()
    {
        Transform target = SelectTarget();
        if (target == null)
        {
			EnemyManager.currentEnemy.GetComponent<TempEnemyVar>().currentAP -= 7;
            return;
        }
        Debug.Log(target.name);
        target.GetComponent<TempPlayerVar>().health -= Random.Range(5, 20);
		StartCoroutine (HitFeedback(target));
		EnemyManager.currentEnemy.GetComponent<TempEnemyVar>().currentAP -= 7;
    }

    public int GetClosestPlayerDist(Transform enemy)
    {
        return GridPositionDetection.DistCheck(enemy.position, PlayerManager.currentPlayer.transform.position);
    }

	private IEnumerator HitFeedback(Transform player)
	{
		Material playerMat = player.GetComponent<MeshRenderer> ().material; 
		Color playerColour = player.GetComponent<MeshRenderer> ().material.color;
		playerMat.color = Color.yellow;
		yield return new WaitForSeconds (2);
		playerMat.color = Color.green;

	}

    private Transform SelectTarget()
    {
		Vector3 enemyPos = EnemyManager.currentEnemy.transform.position;
        List<Transform> curPlayTrans = new List<Transform>();
        foreach (GameObject x in tManage.tPlayer.currentPlayers)
        {
            curPlayTrans.Add(x.transform);
        }
        Transform closestPlayer = tManage.tDetect.RealtivePosition(curPlayTrans, enemyPos);
        List<Transform> inRangePlayTrans = new List<Transform>();
        Transform inRangeClosest = null;
        foreach (Transform x in curPlayTrans)
        {
            Ray enemyToPlay = new Ray(enemyPos, (x.position - enemyPos));
            RaycastHit hit;
			if (Physics.Raycast(enemyToPlay, out hit,100))
            {
				if (hit.transform == x) 
				{
					if (tManage.tDetect.IsInRange (enemyPos, x.position, 6)) 
					{
						if (x == closestPlayer) 
						{
							inRangeClosest = x;
						} else 
						{
							inRangePlayTrans.Add (x);
						}
					}
				}
            }
        }
        if (inRangePlayTrans.Count != 0)
        {
            if (Random.Range(1, 101) <= ((100 / curPlayTrans.Count) + 25) && inRangeClosest != null)
            {
                return inRangeClosest;
            }
            else
            {
                return inRangePlayTrans[Random.Range(0, inRangePlayTrans.Count - 1)];
            }
        }
        else if (inRangeClosest != null)
        {
            return inRangeClosest;
        }
        else
        {
            return null;
        }

    }

    private void ResetTiles()
    {
        tilesClosest.Clear();
        enemyTile = GridPositionDetection.GetClosestGrid (EnemyManager.currentEnemy.transform.position, tManage.tGrid.currentTiles);
		float range = EnemyManager.currentEnemy.GetComponent<TempEnemyVar>().currentAP < EnemyManager.currentEnemy.GetComponent<TempEnemyVar>().moveDist ? (float)EnemyManager.currentEnemy.GetComponent<TempEnemyVar>().currentAP : (float)EnemyManager.currentEnemy.GetComponent<TempEnemyVar>().moveDist;
        tManage.tDetect.FindTilesInDist (enemyTile, tManage.tGrid.tileMeshs, range);
        tilesClosest.AddRange(tManage.tDetect.FindTilesInDist (enemyTile, tManage.tGrid.tileMeshs, (int)range));
    }

}
