using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy_SyncTransform : NetworkBehaviour 
{
	[SyncVar] Vector2 syncPos;
	[SyncVar] float syncZRot;

	Vector2 lastPos;
	Quaternion lastRot;
	Transform myTransform;
	float lerpRate = 10;
	float posThreshold = 0.01f;
	float rotThreshold = 1.0f;

	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		TransmitMotion();
		LerpMotion();
	}

	void TransmitMotion()
	{
		if (!isServer)
			return;

		if (Vector2.Distance(myTransform.position, lastPos) > posThreshold 
			|| Quaternion.Angle(myTransform.rotation, lastRot) > rotThreshold)
		{
			lastPos = myTransform.position;
			lastRot = myTransform.rotation;

			syncPos = myTransform.position;
			syncZRot = myTransform.localEulerAngles.z;
		}
	}

	void LerpMotion()
	{
		if (isServer)
			return;

		myTransform.position = Vector2.Lerp(myTransform.position, syncPos, Time.deltaTime*lerpRate);

		Vector3 newRot = new Vector3(0,0,syncZRot);
		myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(newRot), Time.deltaTime*lerpRate);
	}
}
