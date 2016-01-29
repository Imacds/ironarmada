using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Unit_Animation : NetworkedMonoBehavior 
{
	Unit_Physics unitPhysics;

	Animator anim;
	Transform sprite;
	SpriteRenderer spriteRen;
	Vector2 lastFacingVec;

	[NetSync]
	Vector2 moveVel;

	// Use this for initialization
	void Start () 
	{
		unitPhysics = GetComponent<Unit_Physics>();

		sprite = transform.Find("Sprite");
		anim = sprite.GetComponent<Animator>();
		spriteRen = sprite.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsOwner)
			moveVel = unitPhysics.MoveVelocity;
		
		float moveSpeed = moveVel.magnitude;

		//animation
		Vector2 spriteFaceVec;
		if (moveSpeed > 0.0f) 
		{
			spriteFaceVec = moveVel;
			lastFacingVec = moveVel;
		}
		else
		{
			spriteFaceVec = lastFacingVec;
		}

		anim.SetFloat("runSpeed", moveSpeed); 
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
