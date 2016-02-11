using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PartProduction : NetworkBehaviour 
{
	public GameObject partPrefab;

	public void ProducePart( GameInfo gameInfo, GameObject myGO, int amount = 1 )
	{
		int myGOTeam = myGO.GetComponent<Generic_Team>().TeamNum;

		if (isServer)
		{
			List<GameObject> parts = new List<GameObject>();
			for ( int i = 0; i < amount; i++ )
			{
				parts.Add(MakePart( new Vector2( i, 0 ), Vector2.zero, 0.0f, myGOTeam ));
			}

			List<GameObject> myGO_parts;
			myGO_parts = myGO.GetComponent<Unit_PartPlacement>().parts;
			myGO_parts.Clear();
			int gOID = myGO.GetInstanceID();
			//u16 playerID = blob.getPlayer().getNetworkID();
			for (int i = 0; i < parts.Count; i++)
			{
				GameObject gO = parts[i];
				myGO_parts.Add( gO );	
				gO.GetComponent<Part_Info>().OwnerID = gOID;
				//b.set_u16( "playerID", playerID );
				gO.GetComponent<Part_Info>().ShipID = -1; // don't push on ship
			}
		}
	}

	GameObject MakePart( Vector2 offset, Vector2 pos, float angle, int team = -1 )
	{
		GameObject part = GameObject.Instantiate(partPrefab, pos + offset, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(part);
		if (part != null) 
		{
			//part.getSprite().SetFrame( blockType );
			//part.set_f32( "weight", Block::getWeight( block ) );
			Vector3 eulerAngle = new Vector3(0, 0, angle);
			part.transform.rotation = Quaternion.Euler(eulerAngle);

			part.GetComponent<Part_Info>().ShipID = 0;
			part.GetComponent<Part_Info>().PlacedTime = Time.time;
		}
		return part;
	}
}
