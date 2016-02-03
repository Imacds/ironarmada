using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using Steamworks;

/*
	The player class. Everytime a player joins an instance of this class is created and assigned to that player.
	This class is responsible for directing the user's actions to the correct pawns in the scene as well as holding
	key data about the player such as username, xp, cash etc.

	Author : Hannah Crawford
*/

public class PlayerController : NetworkedMonoBehavior
{
	public string steamName = "Unknown Player";

	// The pawn to spawn when the client joins.
	public GameObject defaultPawn;

	// The pawn that the player is currently controlling
	public Pawn possesedPawn;

	void Start () {
		if(!IsOwner) return;

		if(SteamManager.Initialized) steamName = SteamFriends.GetPersonaName();

		// Spawn the defaultPawn on all player's machines, calls PlayerSpawned when done.
		Networking.Instantiate(defaultPawn, NetworkReceivers.AllBuffered, PawnSpawned);

		// Temp
		FindObjectOfType<ForgeChat>().SendChatMessage(steamName + " joined", true);
	}

	// Callback
	void PawnSpawned(SimpleNetworkedMonoBehavior pawnSnmb)
	{
		Debug.Log("Pawn Spawned");
		pawnSnmb.transform.position = transform.position;
		RPC("PossessPawn", NetworkReceivers.AllBuffered, pawnSnmb.NetworkedId, OwnerId);
	}

	void Update ()
	{
		if(possesedPawn)
		{
			possesedPawn.ControlledUpdateAll(this);
			if(possesedPawn.IsOwner) possesedPawn.ControlledUpdateOwner(this);
			else possesedPawn.ControlledUpdateNonOwner(this);
		}

		if(!IsOwner) return;
	}

	void OnGUI(){
		if(!IsOwner) return;

		// Checking everything gets set properly
		if(possesedPawn) GUI.Label(new Rect(10, 10, 200, 50), "Pawn ID : " + possesedPawn.NetworkedId);
		GUI.Label(new Rect(10, 30, 200, 50), "Player Controller ID : " + NetworkedId);
		if(possesedPawn) GUI.Label(new Rect(10, 50, 200, 50), "Pawn Owner ID : " + possesedPawn.OwnerId);
		GUI.Label(new Rect(10, 70, 200, 50), "Player ID : " + OwnerId);
	}

	// Sets possessedPawn to the pawn with the matching id.
	// Changes the owner of the pawn to the player with the matching id.
	// Notifies the pawn that is has been possessed with the network id of the PlayerController that possessed it.
	[BRPC] void PossessPawn(ulong pawnNetworkedId, ulong playerNetworkedId)
	{
		Pawn[] pawns = FindObjectsOfType<Pawn>();
		for(int i=0; i<pawns.Length; i++){
			if(pawns[i].NetworkedId == pawnNetworkedId){
				// only server or current owner can change the owner od a snmb.
				if(IsServerOwner) 
					pawns[i].ChangeOwner(playerNetworkedId);

				pawns[i].RPC("Possessed", NetworkReceivers.AllBuffered, NetworkedId);
				possesedPawn = pawns[i];
				break;
			}
		}
	}
}