using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnManager_EnemySpawner : NetworkBehaviour 
{
	[SerializeField]GameObject enemyPrefab;
	GameObject[] enemySpawns;
	int counter;
	int numberOfEnemies = 6;
	int maxNumEnemies = 80;
	float waveRate = 10.0f;
	bool isSpawnActivated = true;

	public override void OnStartServer ()
	{
		enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");

		StartCoroutine(EnemySpawner());
	}

	IEnumerator EnemySpawner()
	{
		for(;;)
		{
			yield return new WaitForSeconds(waveRate);
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			if (enemies.Length < maxNumEnemies)
			{
				CommenceSpawn();
			}
		}
	}

	void CommenceSpawn()
	{
		if (isSpawnActivated)
		{
			for (int i = 0; i < numberOfEnemies; i++)
			{
				int randomIndex = Random.Range(0, enemySpawns.Length);
				SpawnEnemy(enemySpawns[randomIndex].transform.position);
			}
		}
	}


	void SpawnEnemy(Vector2 spawnPos)
	{
		counter++;
		GameObject go = GameObject.Instantiate(enemyPrefab, spawnPos, Quaternion.identity) as GameObject;
		go.GetComponent<Enemy_ID>().enemyID = "Enemy " + counter;
		NetworkServer.Spawn(go);
	}
}
