using UnityEngine;
using System.Collections;

public class Part_Info : MonoBehaviour 
{
	int myShipID;
	public int ShipID
	{
		get
		{
			return myShipID;
		}
		set
		{
			myShipID = value;
		}
	}

	int lastShipID;
	public int LastShipID
	{
		get
		{
			return lastShipID;
		}
		set
		{
			lastShipID = value;
		}
	}

	Vector2 offset;
	public Vector2 Offset
	{
		get
		{
			return offset;
		}
		set
		{
			offset = value;
		}
	}

	int ownerID;
	public int OwnerID
	{
		get
		{
			return ownerID;
		}
		set
		{
			ownerID = value;
		}
	}

	float placedTime;
	public float PlacedTime
	{
		get
		{
			return placedTime;
		}
		set
		{
			placedTime = value;
		}
	}

	Vector2 velocity;
	public Vector2 Velocity
	{
		get
		{
			return velocity;
		}
		set
		{
			velocity = value;
		}
	}

	float angularVelocity;
	public float AngularVelocity
	{
		get
		{
			return angularVelocity;
		}
		set
		{
			angularVelocity = value;
		}
	}

	float mass;
	public float Mass
	{
		get
		{
			return mass;
		}
		set
		{
			mass = value;
		}
	}

	string playerOwner;
	public string PlayerOwner
	{
		get
		{
			return playerOwner;
		}
		set
		{
			playerOwner = value;
		}
	}

	int teamNum;
	public int TeamNum
	{
		get
		{
			return teamNum;
		}
		set
		{
			teamNum = value;
		}
	}
}
