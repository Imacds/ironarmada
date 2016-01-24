using UnityEngine;
using System.Collections;

public class Unit_Physics : MonoBehaviour 
{
	Vector2 moveNorm;
	Vector2 velocity;
	Vector2 moveVel;
	Vector2 pushVel;

	public Vector2 TotalVelocity
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

	public Vector2 MoveVelocity
	{
		get
		{
			return moveVel;
		}
		set
		{
			moveVel = value;
		}
	}

	public Vector2 PushVelocity
	{
		get
		{
			return pushVel;
		}
		set
		{
			pushVel = value;
		}
	}

	// collision variables
	CircleCollider2D myCollider;
	int layerMask;
	Rect collisionBox;
	int horizontalRays = 2;
	int verticalRays = 2;
	float collisionMargin = 0.01f; 

	// Use this for initialization
	void Start () 
	{
		myCollider = GetComponent<CircleCollider2D> ();
		layerMask = 1 << LayerMask.NameToLayer ("Unit Collisions");
	}

	void FixedUpdate() 
	{
		velocity = moveVel + pushVel;

		collisionBox = new Rect (	myCollider.bounds.min.x,
			myCollider.bounds.min.y,
			myCollider.bounds.size.x,
			myCollider.bounds.size.y );
		
		// Lateral collision detection
		if (velocity.x != 0.0f) 
		{
			Vector2 startPoint = new Vector2( collisionBox.center.x, collisionBox.yMin + collisionMargin );
			Vector2 endPoint = new Vector2( collisionBox.center.x, collisionBox.yMax - collisionMargin );

			RaycastHit2D hitInfo;

			float sideRayLength = collisionBox.width/2 + Mathf.Abs(velocity.x);
			Vector2 direction = velocity.x > 0 ? Vector2.right : Vector2.left;

			for ( int i = 0; i < horizontalRays; i++ )
			{
				float lerpAmount = (float)i/(float)(horizontalRays - 1);
				Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);

				hitInfo = Physics2D.Raycast(origin, direction, sideRayLength, layerMask);
				Debug.DrawLine(origin, origin + direction*sideRayLength);

				if (hitInfo)
				{
					transform.Translate(direction*(hitInfo.distance - collisionBox.width/2));
					velocity = new Vector2(0.0f, velocity.y);
					break;
				}
			}
		}

		// Vertical collision detection
		if (velocity.y != 0.0f) 
		{
			Vector2 startPoint = new Vector2( collisionBox.xMin + collisionMargin, collisionBox.center.y );
			Vector2 endPoint = new Vector2( collisionBox.xMax - collisionMargin, collisionBox.center.y );

			RaycastHit2D hitInfo;

			float sideRayLength = collisionBox.height/2 + Mathf.Abs(velocity.y);
			Vector2 direction = velocity.y > 0 ? Vector2.up : Vector2.down;

			for ( int i = 0; i < verticalRays; i++ )
			{
				float lerpAmount = (float)i/(float)(verticalRays - 1);
				Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);

				hitInfo = Physics2D.Raycast(origin, direction, sideRayLength, layerMask);
				Debug.DrawLine(origin, origin + direction*sideRayLength);

				if (hitInfo)
				{
					transform.Translate(direction*(hitInfo.distance - collisionBox.height/2));
					velocity = new Vector2(velocity.x, 0.0f);
					break;
				}
			}
		}

		if (velocity.magnitude > 0.0f) 
		{
			// Move the player based on final velocity
			transform.Translate (velocity);

			// Make sure speed is 0 by default if no external input
			moveVel = Vector2.zero;
			pushVel = Vector2.zero;
			velocity = Vector2.zero;
		}
	}

}

