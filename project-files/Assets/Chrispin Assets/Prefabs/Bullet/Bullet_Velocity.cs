using UnityEngine;
using System.Collections;

public class Bullet_Velocity : MonoBehaviour 
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
}
