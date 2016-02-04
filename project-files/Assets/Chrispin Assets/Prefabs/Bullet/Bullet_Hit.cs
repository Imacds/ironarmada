using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Bullet_Hit : SimpleNetworkedMonoBehavior {
	public int damage = 15;

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("Bullet Collide");
	}
}
