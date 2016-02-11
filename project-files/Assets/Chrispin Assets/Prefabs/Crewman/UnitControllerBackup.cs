using UnityEngine;
using System.Collections;

public class UnitControllerBackup : MonoBehaviour 
{
	Vector2 moveNorm;
	Vector2 velocity;

	// movement properties
	float acceleration = 0.2f;
	float maxWalkSpeed = 1.0f;

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
		layerMask = 1 << LayerMask.NameToLayer ("playerCollisions");
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate() 
	{
		// Handle the player movement controls
		Vector2 newVelocity = velocity;

		float xMove = Input.GetAxisRaw("Horizontal");
		float yMove = Input.GetAxisRaw("Vertical");
		moveNorm = new Vector2(xMove, yMove);
		moveNorm.Normalize ();
		if (moveNorm.magnitude != 0.0f) 
		{					
			//apply movement according to input
			newVelocity += moveNorm * acceleration;
			newVelocity = Vector2.ClampMagnitude (newVelocity, maxWalkSpeed);
		} 
		else if (velocity.magnitude > acceleration) 
		{		
			//apply deceleration due to no input
			Vector2 velNorm = velocity;
			velNorm.Normalize ();
			newVelocity -= velNorm * acceleration;
		} 
		else 
		{
			newVelocity = Vector2.zero;
		}

		velocity = moveNorm*maxWalkSpeed;

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

			float sideRayLength = collisionBox.width/2 + Mathf.Abs(velocity.x*Time.deltaTime);
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

			float sideRayLength = collisionBox.height/2 + Mathf.Abs(velocity.y*Time.deltaTime);
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
			transform.Translate (velocity * Time.deltaTime);
		}
	}

}
