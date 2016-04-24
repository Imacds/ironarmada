using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInfo : MonoBehaviour 
{
	List<Ship> allShips;
	public List<Ship> AllShips
	{
		get
		{
			return allShips;
		}
		set
		{
			allShips = value;
		}
	}

	uint shipsID;
	public uint ShipsID
	{
		get
		{
			return shipsID;
		}
		set
		{
			shipsID = value;
		}
	}

	bool dirtyShipSync;
	public bool DirtyShipSync
	{
		get
		{
			return dirtyShipSync;
		}
		set
		{
			dirtyShipSync = value;
		}
	}

	/*
	public Dictionary<string, object> info = new Dictionary<string, object>();

	public void Set( string key, UnityEngine.Object value )
	{
		info.Add(key, value);
	}

	public bool Get<T>( string key, out object value )
	{
		return info.TryGetValue(key, out value);
	}

	public object Get( string key )
	{
		object value;
		info.TryGetValue(key, out value);
		return value;
	}
	*/
}
