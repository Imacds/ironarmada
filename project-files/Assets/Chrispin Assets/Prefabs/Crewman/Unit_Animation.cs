using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Unit_Animation : NetworkedMonoBehavior 
{
	Unit_Physics unitPhysics;

	Animator anim;
	Transform sprite;

	[NetSync]
	float moveSpeed;
	[NetSync]
	Vector2 facingVector;

	// Use this for initialization
	void Start () 
	{
		unitPhysics = GetComponent<Unit_Physics>();

		sprite = transform.Find("Sprite");
		anim = sprite.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		Vector2 moveVel = unitPhysics.MoveVelocity;

		if (IsOwner)
			moveSpeed = moveVel.magnitude;

		Vector2 transPos2D = transform.position;
		Vector2 aimPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		//sync velocity and facing
		if (IsOwner)
		{
			if (moveSpeed > 0.0f) 
			{
				facingVector = moveVel;
			}
			else
			{
				facingVector = aimPos - transPos2D;
			}
		}

		//animation
		anim.SetFloat("runSpeed", moveSpeed); 
		if (sprite != null)
		{
			float spriteAngle = Vector2.Angle(facingVector, Vector2.right);
			Vector3 cross = Vector3.Cross(facingVector, Vector2.right);
			if (cross.z > 0)
				spriteAngle = 360 - spriteAngle;

			sprite.transform.localEulerAngles = new Vector3(0, 0, spriteAngle);
		}
	}
}
