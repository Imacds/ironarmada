using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Unit_PartPlacement : NetworkBehaviour 
{
	const float rotate_speed = 10.0f;
	const float max_build_distance = 0.64f;
	ushort crewCantPlaceCounter = 0;

	public List<GameObject> parts;
	public float parts_angle = 0.0f;	//next step angle
	public float target_angle = 0.0f;	//final angle after manual rotation

	[SyncVar] public Vector2 aim_pos;
	Vector2 lastAimPos;
	float aimPosThreshold = 0.01f; 

	GameInfo gameInfo;

	ShipFunctions shipFunctions;

	void Start()
	{
		parts = new List<GameObject>();
		gameInfo = GameObject.Find("GameManager").GetComponent<GameInfo>();
		shipFunctions = this.gameObject.GetComponent<ShipFunctions>();
		//myGO.addCommandID("place");
	}

	[Command]
	void CmdSendAimPosToServer (Vector2 pos)
	{
		aim_pos = pos;
	}

	[Command]
	void CmdSendTargetAngleToServer (float angle)
	{
		target_angle = angle;
	}

	[ClientCallback]
	void TransmitAimPos(Vector2 pos)
	{
		if ( isLocalPlayer )
		{
			if ( Vector2.Distance(pos, lastAimPos) > aimPosThreshold )
			{
				CmdSendAimPosToServer(pos);
				lastAimPos = pos;
			}
		}
	}

	Vector2 RotateVector2( Vector2 v, float angle )
	{
		float sin = Mathf.Sin(angle);
		float cos = Mathf.Cos(angle);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos*tx) - (sin*ty);
		v.y = (cos*ty) + (sin*tx);

		return v;
	}

	void Update()
	{
		GameObject myGO = this.gameObject;

		if (Input.GetMouseButtonDown(1))
		{
			PartProduction productionScript = myGO.GetComponent<PartProduction>();
			productionScript.ProducePart(gameInfo, myGO);
		}

		if (parts != null && parts.Count > 0)
		{
			Vector2 pos = myGO.transform.position;
			aim_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			TransmitAimPos(aim_pos);

			Ship ship = shipFunctions.getShip(myGO);
			if (ship != null && ship.centerPart != null)
			{
				Vector2 shipPos = ship.centerPart.transform.position;

				GameObject refGO = shipFunctions.getShipGO(myGO);

				if (refGO == null)
				{
					Debug.Log("Unit_PartPlacement: refGO not found");
					return;
				}

				if ( isClient )
				{
					PositionParts( pos, aim_pos, parts_angle, ship.centerPart, refGO );
				}

				//CPlayer@ player = this.getPlayer();
				//if (player !is null && player.isMyPlayer()) 
				if (isLocalPlayer) 
				{
					//checks for canPlace
					//u32 gameTime = getGameTime();
					//CRules@ rules = getRules();
					//bool skipCoreCheck = gameTime > getRules().get_u16( "warmup_time" ) || ( ship.isMothership && ( ship.owner == "" ||  ship.owner == "*" || ship.owner == player.getUsername() ) );
					//bool cLinked = false;
					bool overlappingShip = shipFunctions.partsOverlappingShip( parts );
					for (uint i = 0; i < parts.Count; ++i)
					{
						if ( overlappingShip )
						{
							//SetDisplay( parts[i], SColor(255, 255, 0, 0), RenderStyle::additive );
							continue;
						}

						//if ( skipCoreCheck || blocks[i].hasTag( "coupling" ) || blocks[i].hasTag( "repulsor" ) )
						if ( true )
							continue;
						
						//if ( !cLinked )
						//{
						//	CBlob@ core = getMothership( this.getTeamNum() );//could get the core properly based on adjacent blocks
						//	if ( core !is null )
						//		cLinked = coreLinkedDirectional( blocks[i], gameTime, core.getPosition() );
						//}

						//if ( cLinked )
						//	SetDisplay( blocks[i], SColor(255, 255, 0, 0), RenderStyle::additive );
					}

					//can'tPlace heltips
					//bool crewCantPlace = !overlappingShip && cLinked;
					//if ( crewCantPlace )
					//	crewCantPlaceCounter++;
					//else
					//	crewCantPlaceCounter = 0;

					//this.set_bool( "blockPlacementWarn", crewCantPlace && crewCantPlaceCounter > 15 );

					// place
					//if (this.isKeyJustPressed( key_action1 ) && !getHUD().hasMenus() && !getHUD().hasButtons() )
					if ( Input.GetMouseButtonDown(0) )
					{
						if (target_angle == parts_angle && !overlappingShip)
						{
							/*
							CBitStream params;
							params.write_netid( island.centerBlock.getNetworkID() );
							params.write_netid( refBlob.getNetworkID() );
							params.write_Vec2f( pos - islandPos );
							params.write_Vec2f( aimPos - islandPos );
							params.write_f32( target_angle );
							params.write_f32( island.centerBlock.getAngleDegrees() );
							this.SendCommand( this.getCommandID("place"), params );
							*/
							CmdPlace (ship.centerPart, refGO, pos - shipPos, aim_pos - shipPos, ship.centerPart.transform.eulerAngles.z);
						}
						else
						{
							//this.getSprite().PlaySound("Denied.ogg");
						}
					}

					// rotate
					if (Input.GetKeyDown("space"))
					{
						target_angle += 90.0f;
						if (target_angle > 360.0f) 
						{
							target_angle -= 360.0f;
							parts_angle -= 360.0f;
						}
						CmdSendTargetAngleToServer(target_angle);
					}
				}
			}
			else
			{
				// part placement off ship
				if ( isClient )
				{
					PositionPartsOffShip( pos, aim_pos, parts_angle );
				}
					
				if (isLocalPlayer) 
				{
					//checks for canPlace
					bool overlappingShip = shipFunctions.partsOverlappingShip( parts );
					for (uint i = 0; i < parts.Count; ++i)
					{
						if ( overlappingShip )
						{
							//SetDisplay( parts[i], SColor(255, 255, 0, 0), RenderStyle::additive );
							continue;
						}
					}

					// place
					if ( Input.GetMouseButtonDown(0) )
					{
						if (target_angle == parts_angle && !overlappingShip)
						{
							CmdPlaceOffShip (pos, aim_pos);
						}
						else
						{
							//this.getSprite().PlaySound("Denied.ogg");
						}
					}

					// rotate
					if (Input.GetKeyDown("space"))
					{
						target_angle += 90.0f;
						if (target_angle > 360.0f) 
						{
							target_angle -= 360.0f;
							parts_angle -= 360.0f;
						}
						CmdSendTargetAngleToServer(target_angle);
					}
				}
			}

			//slowly rotate to desired angle
			parts_angle += rotate_speed;
			if (parts_angle > target_angle)
			{
				parts_angle = target_angle;
			}
		}
	}
		
	void PositionParts( Vector2 pos, Vector2 aimPos, float partAngle, GameObject centerPart, GameObject refPart )
	{
		if ( centerPart == null )
		{
			Debug.Log("PositionParts: center part not found");
			return;
		}

		Vector2 ship_pos = centerPart.transform.position;
		float angle = centerPart.transform.eulerAngles.z;
		float refPAngle = refPart.transform.eulerAngles.z;//reference block angle
		//current island angle as point of reference
		while(refPAngle > angle + 45)	
		{
			refPAngle -= 90.0f;
		}
		while(refPAngle < angle - 45)
		{
			refPAngle += 90.0f;
		}

		//get offset (based on the centerblock) of block we're standing on
		Vector2 refPartPos2D = refPart.transform.position;
		Vector2 refPOffset = refPartPos2D - ship_pos;
		float gridSize = 0.16f;
		float halfGridSize = gridSize/2.0f;
		refPOffset = RotateVector2(refPOffset, -refPAngle);
		refPOffset.x = refPOffset.x % gridSize;
		refPOffset.y = refPOffset.y % gridSize;
		//not really necessary
		if ( refPOffset.x > halfGridSize )	refPOffset.x -= gridSize;	else if ( refPOffset.x < -halfGridSize )	refPOffset.x += gridSize;
		if ( refPOffset.y > halfGridSize )	refPOffset.y -= gridSize;	else if ( refPOffset.y < -halfGridSize )	refPOffset.y += gridSize;
		refPOffset = RotateVector2(refPOffset, refPAngle);

		ship_pos += refPOffset;
		Vector2 mouseAim = aimPos - pos;
		float mouseDist = Mathf.Min( mouseAim.magnitude, max_build_distance );
		mouseAim.Normalize();
		aimPos = pos + mouseAim * mouseDist;//position of the 'buildpart' pointer
		Vector2 shipAim = aimPos - ship_pos;//ship to 'buildpart' pointer
		shipAim = RotateVector2( shipAim, -refPAngle );
		shipAim = shipFunctions.RelSnapToGrid( shipAim );
		shipAim = RotateVector2( shipAim, refPAngle );
		Vector2 cursor_pos = ship_pos + shipAim;//position of snapped build part

		//rotate and position parts
		for (int i = 0; i < parts.Count; ++i)
		{
			GameObject part = parts[i];
			Vector2 offset = part.GetComponent<Part_Info>().Offset;
			offset = RotateVector2( offset, parts_angle );
			offset = RotateVector2( offset, refPAngle );               

			part.transform.position = cursor_pos + offset;	//align to ship grid
			part.transform.eulerAngles = new Vector3(0, 0, ( refPAngle + parts_angle ) % 360.0f);

			//SetDisplay( block, color_white, RenderStyle::additive, 560.0f );
		}
	}

	void PositionPartsOffShip( Vector2 pos, Vector2 aimPos, float partAngle )
	{
		float angle = 0.0f;
		float refPAngle = 0.0f;//reference block angle
		//current island angle as point of reference
		while(refPAngle > angle + 45)	
		{
			refPAngle -= 90.0f;
		}
		while(refPAngle < angle - 45)
		{
			refPAngle += 90.0f;
		}
			
		Vector2 mouseAim = aimPos - pos;
		float mouseDist = Mathf.Min( mouseAim.magnitude, max_build_distance );
		mouseAim.Normalize();
		aimPos = pos + mouseAim * mouseDist;	//position of the 'buildpart' pointer
		aimPos = shipFunctions.RelSnapToGrid( aimPos );
		Vector2 cursor_pos = aimPos;	//position of snapped build part

		//rotate and position parts
		for (int i = 0; i < parts.Count; ++i)
		{
			GameObject part = parts[i];
			Vector2 offset = part.GetComponent<Part_Info>().Offset;
			offset = RotateVector2( offset, parts_angle );
			offset = RotateVector2( offset, refPAngle );               

			part.transform.position = cursor_pos + offset;	//align to ship grid
			part.transform.eulerAngles = new Vector3(0, 0, ( refPAngle + parts_angle ) % 360.0f);

			//SetDisplay( block, color_white, RenderStyle::additive, 560.0f );
		}
	}

	[Command]
	void CmdPlace (GameObject centerPart, GameObject refGO, Vector2 pos_offset, Vector2 aimPos_offset, float ship_angle)
	{
		if (centerPart == null || refGO == null)
		{
			Debug.Log("place cmd: centerPart not found");
			return;
		}

		Ship ship = shipFunctions.getShip( centerPart.GetComponent<Part_Info>().ShipID );
		if (ship == null)
		{
			Debug.Log("place cmd: ship not found");
			return;
		}

		Vector2 shipPos = centerPart.transform.position;
		float shipAngle = centerPart.transform.eulerAngles.z;
		float angleDelta = shipAngle - ship_angle;//to account for ship angle lag

		bool blocksPlaced = false;
		if (parts.Count > 0)                 
		{	
			PositionParts( shipPos + RotateVector2(pos_offset, angleDelta), shipPos + RotateVector2(aimPos_offset, angleDelta),
				target_angle, centerPart, refGO );

			if ( true )
			{
				int shipID = centerPart.GetComponent<Part_Info>().ShipID;
				for (int i = 0; i < parts.Count; ++i)
				{
					GameObject gO = parts[i];
					if (gO != null)
					{
						gO.GetComponent<Part_Info>().OwnerID = 0;	//so it wont add to owner parts
						//float z = 510.0f;
						//if ( gO.getSprite().getFrame() == 0 )	z = 509.0f;//platforms
						//else if ( gO.hasTag( "weapon" ) )	z = 511.0f;//weaps
						//SetDisplay( gO, color_white, RenderStyle::normal, z );
						if ( !isServer )//add it locally till a sync
						{
							ShipPart ship_part = null;
							ship_part.gameObjectID = gO.GetInstanceID();
							Vector2 gOTransPos2D = gO.transform.position;
							ship_part.offset = gOTransPos2D - shipPos;
							ship_part.offset = RotateVector2(ship_part.offset, -shipAngle);
							ship_part.angle_offset = gO.transform.eulerAngles.z - shipAngle;
							gO.GetComponent<Part_Info>().OwnerID = shipID;
							ship.parts.Add(ship_part);	
						} 
						else
						{
							gO.GetComponent<Part_Info>().OwnerID = 0; // push on ship  
						}

						gO.GetComponent<Part_Info>().PlacedTime = Time.time;
					}
					else
					{
						Debug.Log("place cmd: GO not found");
					}
				}
				//this.set_u32( "placedTime", getGameTime() );
				blocksPlaced = true;
			}
			else
			{
				Debug.Log("place cmd: parts overlapping, cannot place");
				//this.getSprite().PlaySound("Denied.ogg");
				return;	
			}
		}
		else
		{
			Debug.Log("place cmd: no parts");
			return;
		}

		parts.Clear();//releases the parts (they are placed)
		gameInfo.DirtyShipSync = true;
		//directionalSoundPlay( "build_ladder.ogg", this.getPosition() );
	}

	[Command]
	void CmdPlaceOffShip (Vector2 pos, Vector2 aimPos )
	{
		bool blocksPlaced = false;
		if (parts.Count > 0)                 
		{	
			PositionPartsOffShip( pos, aimPos, target_angle);

			if ( true )
			{
				int shipID = 0;	//should be ID of last boarded ship?
				for (int i = 0; i < parts.Count; ++i)
				{
					GameObject gO = parts[i];
					if (gO != null)
					{
						gO.GetComponent<Part_Info>().OwnerID = 0;	//so it wont add to owner parts
						//float z = 510.0f;
						//if ( gO.getSprite().getFrame() == 0 )	z = 509.0f;//platforms
						//else if ( gO.hasTag( "weapon" ) )	z = 511.0f;//weaps
						//SetDisplay( gO, color_white, RenderStyle::normal, z );
						if ( !isServer )//add it locally till a sync
						{
							ShipPart ship_part = null;
							ship_part.gameObjectID = gO.GetInstanceID();
							Vector2 gOTransPos2D = gO.transform.position;
							gO.GetComponent<Part_Info>().OwnerID = shipID;
						} 
						else
						{
							gO.GetComponent<Part_Info>().OwnerID = 0; // push on ship  
						}

						gO.GetComponent<Part_Info>().PlacedTime = Time.time;
					}
					else
					{
						Debug.Log("place cmd: GO not found");
					}
				}
				//this.set_u32( "placedTime", getGameTime() );
				blocksPlaced = true;
			}
			else
			{
				Debug.Log("place cmd: parts overlapping, cannot place");
				//this.getSprite().PlaySound("Denied.ogg");
				return;	
			}
		}
		else
		{
			Debug.Log("place cmd: no parts");
			return;
		}

		parts.Clear();//releases the parts (they are placed)
		gameInfo.DirtyShipSync = true;
		//directionalSoundPlay( "build_ladder.ogg", this.getPosition() );
	}

	/* 
	void SetDisplay( CBlob@ blob, SColor color, RenderStyle::Style style, f32 Z=-10000)
	{
		CSprite@ sprite = blob.getSprite();
		sprite.asLayer().SetColor( color );
		sprite.asLayer().setRenderStyle( style );
		if (Z>-10000){
			sprite.SetZ(Z);
		}
	}
	*/
}
