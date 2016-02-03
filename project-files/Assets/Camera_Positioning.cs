using UnityEngine;
using System.Collections;

public class Camera_Positioning : MonoBehaviour 
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

		if(possesedPawn)
		{
			Camera.main.transform.position = new Vector3(0,0,-10) + possesedPawn.transform.position;
		}
	}
}
