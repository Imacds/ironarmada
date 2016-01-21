using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Ship : Pawn {
	public float linearThrust = 100f;
	public float angularThrust = 100f;

	private Rigidbody2D rb;

	void Start(){
		rb = GetComponent<Rigidbody2D>();
	}

	public override void ControlledUpdateOwner(PlayerController controller){
		float linearInput = Input.GetAxis("Vertical");
		float angularInput = Input.GetAxis("Horizontal");

		rb.AddForce(transform.rotation * new Vector3(0, linearInput * linearThrust, 0));
		rb.AddTorque(angularInput * angularThrust);
	}
}