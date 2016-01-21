using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class SpawnManager : NetworkedMonoBehavior {
	public GameObject playerObject;

	void Start () {
		SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();

		SpawnPoint randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
		playerObject.transform.position = randomSpawn.transform.position;
		Networking.Instantiate(playerObject, NetworkReceivers.AllBuffered);
	}
}
