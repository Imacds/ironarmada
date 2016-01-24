using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy_ID : NetworkBehaviour 
{
	[SyncVar] public string enemyID;
	Transform myTransform;

	bool haveSetMyID = false;

	// Use this for initialization
	void Awake () 
	{
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		SetID();
		if (!haveSetMyID)
		{
			SetID();

			string myTransformName = myTransform.name;
			if (myTransformName.Contains("Enemy "))
			{
				haveSetMyID = true;
			}
			else
			{
				haveSetMyID = false;
			}
		}
	}

	void SetID()
	{
		myTransform.name = enemyID;
	}
}
