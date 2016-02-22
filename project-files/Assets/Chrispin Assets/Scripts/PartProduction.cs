using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Network;

public class PartProduction : SimpleNetworkedMonoBehavior 
{
	public GameObject partPrefab;
	List<GameObject> parts = new List<GameObject>();

	public void ProducePart( GameInfo gameInfo, GameObject myGO, int amount = 1 )
	{
		int myGOTeam = myGO.GetComponent<Generic_Team>().TeamNum;

		if (true)	// was OwningNetWorker.IsServer
		{
			for ( int i = 0; i < amount; i++ )
			{
				MakePart( new Vector2( i, 0 ), Vector2.zero, 0.0f, myGOTeam );
			}

			List<GameObject> myGO_parts = myGO.GetComponent<Unit_PartPlacement>().parts;
			myGO_parts.Clear();
			int gOID = myGO.GetInstanceID();
			//u16 playerID = blob.getPlayer().getNetworkID();
			for (int i = 0; i < parts.Count; i++)
			{
				Debug.Log("Part added");
				GameObject gO = parts[i];
				myGO_parts.Add( gO );	
				gO.GetComponent<Part_Info>().OwnerID = gOID;
				//b.set_u16( "playerID", playerID );
				gO.GetComponent<Part_Info>().ShipID = -1; // don't push on ship
			}
			GetComponent<Unit_PartPlacement>().parts = myGO_parts;
			parts.Clear();
		}
	}

	void MakePart( Vector2 offset, Vector2 pos, float angle, int team = -1 )
	{
		Vector3 eulerAngle = new Vector3(0, 0, angle);
		Networking.Instantiate(partPrefab, pos + offset, Quaternion.Euler(eulerAngle), NetworkReceivers.All, PartSpawned);
	}
		
	void PartSpawned( SimpleNetworkedMonoBehavior part )
	{
		//part.getSprite().SetFrame( blockType );
		//part.set_f32( "weight", Block::getWeight( block ) );

		part.GetComponent<Part_Info>().ShipID = 0;
		part.GetComponent<Part_Info>().PlacedTime = Time.time;

		parts.Add(part.gameObject);
	}
}
