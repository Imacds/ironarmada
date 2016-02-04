using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using UnityEngine.UI;

public class Unit_Health : NetworkedMonoBehavior 
{
	const int INIT_HEALTH = 100;

	public int health = INIT_HEALTH;
	public bool dead = false;

	Text healthText;

	private void Awake() {
		healthText = GameObject.Find ("HealthText").GetComponent<Text>();
		AddNetworkVariable (() => health, h => health = (int) h);
	}


	void Update() {
		if (IsSetup && IsOwner) {
			healthText.text = "Health: " + health.ToString();
		}

		if (health <= 0 && !dead) {
			dead = true;
			GetComponentInChildren<SpriteRenderer> ().enabled = false;
			transform.FindChild ("DeathParticles").GetComponent<ParticleSystem> ().Play ();
			GetComponent<Unit_Controller> ().enabled = false;
			GetComponent<Unit_Shooting> ().enabled = false;
		}
	}
}
