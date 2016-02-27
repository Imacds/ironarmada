using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : Pawn 
{
	private Vector2 velocity;

	private Rigidbody2D rb;

	void Start(){
		rb = GetComponent<Rigidbody2D>();
	}

	void Update(){
		//transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
	}

	public override void ControlledUpdateOwner(PlayerController controller)
	{
		velocity.x = Input.GetAxis("Horizontal") * 7.5f;
		velocity.y = Input.GetAxis("Vertical") * 7.5f;
		velocity = Vector2.ClampMagnitude(velocity, 7.5f);

		rb.AddForce(velocity);
	}
}