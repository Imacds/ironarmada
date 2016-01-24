using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_ID : NetworkBehaviour 
{

	[SyncVar] string playerUniqueID;
	NetworkInstanceId playerNetID;
	Transform myTransform;
	
	bool haveSetMyPlayerID = false;

	public override void OnStartLocalPlayer()
	{
		GetPlayerNetID();
		SetPlayerNetID();
	}

	// Use this for initialization
	void Awake () 
	{
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!haveSetMyPlayerID)
		{
			SetPlayerNetID();

			string myTransformName = myTransform.name;
			if (myTransformName.Contains("Player "))
			{
				haveSetMyPlayerID = true;
			}
			else
			{
				haveSetMyPlayerID = false;
			}
		}
	}

	[Client]
	void GetPlayerNetID()
	{
		playerNetID = GetComponent<NetworkIdentity>().netId;
		CmdSendMyIDToServer(MakeUniqueID());
	}
	
	void SetPlayerNetID()
	{
		if (!isLocalPlayer)
		{
			myTransform.name = playerUniqueID;
		}
		else
		{
			myTransform.name = MakeUniqueID();
		}
	}

	string MakeUniqueID()
	{
		string uniqueID = "Player " + playerNetID.ToString();
		return uniqueID;
	}

	[Command]
	void CmdSendMyIDToServer( string identity )
	{
		playerUniqueID = identity;
	}
}
