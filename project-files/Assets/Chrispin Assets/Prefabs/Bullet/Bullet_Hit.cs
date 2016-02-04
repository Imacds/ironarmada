using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Bullet_Hit : SimpleNetworkedMonoBehavior {
	public int damage = 15;

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Bullet OTE");
	}
}
