using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Unit_Death : NetworkBehaviour 
{
	Unit_Health healthScript;
	Image crosshairImage;

	public override void PreStartClient ()
	{
		healthScript = GetComponent<Unit_Health>();
		healthScript.EventDie += DisablePlayer;
	}

	public override void OnStartLocalPlayer ()
	{
		//crosshairImage = GameObject.Find("Crosshair Image").GetComponent<Image>();
	}

	public override void OnNetworkDestroy()
	{
		healthScript.EventDie -= DisablePlayer;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void DisablePlayer()
	{
		GetComponent<Unit_Controller>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;
		GetComponent<Unit_Shooting>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;

		healthScript.isDead = true;

		if (isLocalPlayer)
		{
			//crosshairImage.enabled = false;
			
			//Respawn button
			GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnButton.SetActive(true);
		}
	}
}
