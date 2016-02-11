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
}
