using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : Pawn 
{
	[NetSync] private Vector2 velocity;

	private Rigidbody2D rb;

	void Start(){
		rb = GetComponent<Rigidbody2D>();
	}

	void Update(){
		
	}

	public override void ControlledUpdateOwner(PlayerController controller){
		
	}
}