using UnityEngine;
using System.Collections;

public class Unit_Controller : Pawn 
{
	Vector2 moveNorm;
	Vector2 moveVelocity;

	// movement properties
	float acceleration = 0.2f;
	float maxWalkSpeed = 0.03f;

	Unit_Physics unitPhysics;

	Animator anim;
	Transform sprite;
	SpriteRenderer spriteRen;
	Vector2 lastFacingVec;

	// Use this for initialization
	void Start () 
	{
		unitPhysics = GetComponent<Unit_Physics>();

		sprite = transform.Find("Sprite");
		anim = sprite.GetComponent<Animator>();
		spriteRen = sprite.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	public override void ControlledUpdateOwner(PlayerController controller)
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
			lastFacingVec = moveNorm;
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

		//animation (should put this in separate script?)
		Vector2 spriteFaceVec;
		if (moveNorm.magnitude > 0.0f) 
		{
			spriteFaceVec = moveNorm;
		}
		else
		{
			spriteFaceVec = lastFacingVec;
		}

		anim.SetFloat("runSpeed", moveNorm.magnitude); 
		if (sprite != null)
		{
			float spriteAngle = Vector2.Angle(spriteFaceVec, Vector2.right);
			Vector3 cross = Vector3.Cross(spriteFaceVec, Vector2.right);
			if (cross.z > 0)
				spriteAngle = 360 - spriteAngle;

			sprite.transform.localEulerAngles = new Vector3(0, 0, spriteAngle);
		}
	}
}
