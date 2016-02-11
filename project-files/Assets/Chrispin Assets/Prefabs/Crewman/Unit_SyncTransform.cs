using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[NetworkSettings (channel = 0, sendInterval = 0.1f)]
public class Unit_SyncTransform : NetworkBehaviour 
{
	[SyncVar (hook = "OnSyncPos")] 
	Vector2 syncPos;
	[SyncVar (hook = "OnSyncRot")] 
	float syncRot;

	[SerializeField] Transform myTransform;
	float posLerpRate = 10;
	float rotLerpRate = 15;

	Vector2 lastPos;
	float posThreshold = 0.01f; 
	float lastRot;
	float rotThreshold = 1.0f;

	[SerializeField] bool useHistoricalLerping = false;

	// historical pos lerping variables
	List<Vector2> syncPosList = new List<Vector2>();
	float minHistPosLerpRate = 8;
	float histPosCloseEnough;
	float minHistPosCloseEnough= 0.01f;

	// historical rot lerping variables
	List<float> syncRotList = new List<float>();
	float minHistRotLerpRate = 8;
	float histRotCloseEnough;
	float minHistRotCloseEnough= 1.0f;

	void Start()
	{

	}

	void Update()
	{
		LerpTransform();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		TransmitTransform();
	}

	void LerpTransform()
	{
		if (!isLocalPlayer) 
		{
			if (useHistoricalLerping)
			{
				HistoricalLerping();
			}
			else
			{
				OrdinaryPosLerping();
				OrdinaryRotLerping();
			}
		}
	}

	[Command]
	void CmdProvidePosToServer (Vector2 pos)
	{
		syncPos = pos;
	}

	[Command]
	void CmdProvideRotToServer (float rot)
	{
		syncRot = rot;
	}

	[ClientCallback]
	void TransmitTransform()
	{
		if ( isLocalPlayer )
		{
			if ( Vector2.Distance(myTransform.position, lastPos) > posThreshold )
			{
				CmdProvidePosToServer(myTransform.position); 
				lastPos = myTransform.position;
			}
			if ( IsRotBeyondThreshold(myTransform.localEulerAngles.z, lastRot, rotThreshold) )
			{
				CmdProvideRotToServer(myTransform.localEulerAngles.z); 
				lastRot = myTransform.localEulerAngles.z;
			}
		}
	}

	[Client]
	void OnSyncPos(Vector2 latestPos)
	{
		syncPos = latestPos;
		syncPosList.Add (syncPos);
	}

	[Client]
	void OnSyncRot(float latestRot)
	{
		syncRot = latestRot;
		syncRotList.Add (syncRot);
	}

	void OrdinaryPosLerping()
	{
		myTransform.position = Vector2.Lerp(myTransform.position, syncPos, Time.deltaTime*posLerpRate);
	}

	void OrdinaryRotLerping()
	{
		Vector2 newRot = new Vector3(0, 0, syncRot);
		myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(newRot), Time.deltaTime*rotLerpRate);
	}

	void HistoricalLerping() 
	{
		int posListSize = syncPosList.Count;
		if (posListSize > 0)
		{
			myTransform.position = Vector2.Lerp(myTransform.position, syncPosList[0], Time.deltaTime*posLerpRate);

			histPosCloseEnough = minHistPosCloseEnough + posListSize*0.02f;
			if (Vector2.Distance(myTransform.position, syncPosList[0]) < histPosCloseEnough)
			{
				syncPosList.RemoveAt(0);
			}

			posLerpRate = minHistPosLerpRate + posListSize*2.0f;
		}
		else
		{
			OrdinaryPosLerping();
		}

		int rotListSize = syncRotList.Count;
		if (rotListSize > 0)
		{
			Vector2 newRot = new Vector3(0, 0, syncRotList[0]);
			myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(newRot), Time.deltaTime*rotLerpRate);
			
			histRotCloseEnough = minHistRotCloseEnough + rotListSize*0.02f;
			if (IsRotBeyondThreshold(myTransform.localEulerAngles.z, syncRotList[0], histRotCloseEnough))
			{
				syncRotList.RemoveAt(0);
			}
			
			rotLerpRate = minHistRotLerpRate + rotListSize*2.0f;
		}
		else
		{
			OrdinaryRotLerping();
		}
	}

	bool IsRotBeyondThreshold(float rot1, float rot2, float thresh)
	{
		if (Mathf.Abs(rot2 - rot1) > thresh)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
