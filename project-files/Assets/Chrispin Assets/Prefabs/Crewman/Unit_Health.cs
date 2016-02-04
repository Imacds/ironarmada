using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using UnityEngine.UI;

public class Unit_Health : NetworkedMonoBehavior 
{
	const int INIT_HEALTH = 100;

	public int health = INIT_HEALTH;
	public bool dead = false;
	public float respawnTimer = 0;

	Text healthText;

	void Awake() {
		healthText = GameObject.Find ("HealthText").GetComponent<Text>();
	}

	[BRPC]
	public void doDamage(int dmg) {
		health -= dmg;
		Debug.Log ("Damage taken " + dmg);
	}

	[BRPC]
	public void respawn() {
		health = INIT_HEALTH;
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

		if (dead && health > 0) {
			dead = false;
			GetComponentInChildren<SpriteRenderer> ().enabled = true;
			transform.FindChild ("DeathParticles").GetComponent<ParticleSystem> ().Stop ();
			transform.FindChild ("DeathParticles").GetComponent<ParticleSystem> ().Clear ();
			transform.FindChild ("DeathParticles").GetComponent<ParticleSystem> ().time = 0;
			// A shiny penny for anyone who figures out how to reset the bloody particle system so it can have another pop
			// next time the character dies.
			GetComponent<Unit_Controller> ().enabled = true;
			GetComponent<Unit_Shooting> ().enabled = true;
		}

		if (dead && Networking.PrimarySocket.IsServer) {
			respawnTimer += Time.deltaTime;
			if (respawnTimer >= 5) {
				respawnTimer = 0;
				RPC ("respawn");
			}
		}
	}
}
