using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Bullet_Velocity : NetworkedMonoBehavior 
{
	Vector2 velocity;

	public Vector2 Velocity
	{
		get
		{
			return velocity;
		}
		set
		{
			velocity = value;
		}
	}

	void FixedUpdate () 
	{
		Vector2 transPos2D = transform.position;
		transform.position = transPos2D + velocity;
	}

	void Awake() {
		AddNetworkVariable (() => velocity, v => velocity = (Vector2) v);
	}

	[BRPC]
	public void setV(Vector2 v) {
		velocity = v;
	}
}
