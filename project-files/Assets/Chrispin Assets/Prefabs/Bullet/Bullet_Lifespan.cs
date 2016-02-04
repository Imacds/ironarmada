using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Bullet_Lifespan : SimpleNetworkedMonoBehavior {
	public float lifespan = 3;
	float lifeRemaining;

	void Start () {
		lifeRemaining = lifespan;
	}
	
	void Update () {
		if (Networking.PrimarySocket.IsServer) {
			lifespan -= Time.deltaTime;

			if (lifespan <= 0) {
				Networking.Destroy (this);
			}
		}
	}
}
