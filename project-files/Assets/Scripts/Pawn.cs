using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Pawn : NetworkedMonoBehavior {
	// Update function called on all player's machines.
	public virtual void ControlledUpdateAll(PlayerController controller){
		
	}

	// Update function called only on the owner's machine.
	// Most often used to handle player's input.
	public virtual void ControlledUpdateOwner(PlayerController controller)
	{
		
	}

	// Update function called on all non owner's machines.
	public virtual void ControlledUpdateNonOwner(PlayerController controller){
		
	}

	[BRPC] public virtual void Possessed(ulong playerControllerNetworkedId){
		Debug.Log("Pawn (" + NetworkedId + ") was possessed by Player Controller (" 
												+ playerControllerNetworkedId + ")");
	}
}