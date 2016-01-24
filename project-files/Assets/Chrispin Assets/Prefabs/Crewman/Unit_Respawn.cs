using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Unit_Respawn : NetworkBehaviour 
{
	Unit_Health healthScript;
	Image crosshairImage;
	GameObject respawnButton;

	public override void PreStartClient ()
	{
		healthScript = GetComponent<Unit_Health>();
		healthScript.EventRespawn += EnablePlayer;
	}

	public override void OnStartLocalPlayer ()
	{
		SetRespawnButton();
	}

	public override void OnNetworkDestroy()
	{
		healthScript.EventRespawn -= EnablePlayer;
	}

	void SetRespawnButton()
	{
		respawnButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnButton;
		respawnButton.GetComponent<Button>().onClick.AddListener(CommenceRespawn);
		respawnButton.SetActive(false);
	}

	void EnablePlayer()
	{
		GetComponent<Unit_Controller>().enabled = true;
		GetComponent<CircleCollider2D>().enabled = true;
		GetComponent<Unit_Shooting>().enabled = true;
		GetComponent<SpriteRenderer>().enabled = true;
		
		if (isLocalPlayer)
		{
			//crosshairImage.enabled = false;
			
			//Respawn button
			respawnButton.SetActive(false);
		}
	}

	void CommenceRespawn()
	{
		CmdRespawnOnServer();
	}

	[Command]
	void CmdRespawnOnServer()
	{
		healthScript.ResetHealth();
	}
}
