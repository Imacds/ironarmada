using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Bullet_Hit : SimpleNetworkedMonoBehavior {
	public int damage = 15;
	private bool shooterNetworkedIdSet = false;
	private ulong shooterNetworkedId = 0;

	void OnTriggerEnter2D(Collider2D other) {
		if (!Networking.PrimarySocket.IsServer) {
			return; // Only do collision detection on the server.
		}
		Debug.Log ("Bullet Trigger");
		Unit_Shooting us = other.GetComponent<Unit_Shooting> ();
		if (us != null) {
			if (shooterNetworkedIdSet) {
				if (us.NetworkedId == shooterNetworkedId) {
					Debug.Log ("Bullet Trigger self");
				} else {
					Debug.Log ("Bullet Trigger other");
					// Hit another person! Woo!
					other.GetComponent<Unit_Health> ().RPC("doDamage", damage);
					Networking.Destroy (this);
				}
			} else {
				Debug.Log ("BT unset");
			}
		}
	}

	[BRPC]
	public void setShooterNetworkedId(ulong shooterNetworkedId) {
		Debug.Log ("Shooter networked id is " + shooterNetworkedId);
		this.shooterNetworkedId = shooterNetworkedId;
		shooterNetworkedIdSet = true;
	}
}
