using UnityEngine;
using System.Collections;

public class Unit_Controller : MonoBehaviour 
{
	Vector2 moveNorm;
	Vector2 moveVelocity;

	// movement properties
	float acceleration = 0.2f;
	float maxWalkSpeed = 0.01f;

	Unit_Physics unitPhysics;

	// Use this for initialization
	void Start () 
	{
		unitPhysics = GetComponent<Unit_Physics>();
	}
	
	// Update is called once per frame
	void Update()
	{
		float xMove = Input.GetAxisRaw("Horizontal");
		float yMove = Input.GetAxisRaw("Vertical");
		moveNorm = new Vector2(xMove, yMove);
		moveNorm.Normalize ();
	}

	void FixedUpdate() 
	{
		// Handle the player movement controls
		Vector2 newMoveVelocity = moveVelocity;

		if (moveNorm.magnitude != 0.0f) 
		{					
			//apply movement according to input
			newMoveVelocity += moveNorm * acceleration;
			newMoveVelocity = Vector2.ClampMagnitude (newMoveVelocity, maxWalkSpeed);
		} 
		else if (moveVelocity.magnitude > acceleration) 
		{		
			//apply deceleration due to no input
			Vector2 velNorm = moveVelocity;
			velNorm.Normalize ();
			newMoveVelocity -= velNorm * acceleration;
		} 
		else 
		{
			newMoveVelocity = Vector2.zero;
		}

		moveVelocity = moveNorm*maxWalkSpeed;
		unitPhysics.MoveVelocity = moveVelocity;
	}
}
