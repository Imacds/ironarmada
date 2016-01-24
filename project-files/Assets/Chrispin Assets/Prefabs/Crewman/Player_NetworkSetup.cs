using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour 
{
	[SerializeField] Camera topDownCamera;
	[SerializeField] AudioListener audioListener;

	// Use this for initialization
	public override void OnStartLocalPlayer ()
	{
		GameObject.Find("SceneCamera").SetActive(false);
		GetComponent<Unit_Controller>().enabled = true;
		topDownCamera.enabled = true;
		audioListener.enabled = true;
	}
}
