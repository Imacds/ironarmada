using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Camera_Positioning : SimpleNetworkedMonoBehavior 
{

	PlayerController playerController;

	// Use this for initialization
	void Start () 
	{
		playerController = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Pawn possesedPawn = playerController.possesedPawn;

		if(possesedPawn && IsOwner)
		{
			Camera.main.transform.position = new Vector3(0,0,-10) + possesedPawn.transform.position;
		}
	}
}
