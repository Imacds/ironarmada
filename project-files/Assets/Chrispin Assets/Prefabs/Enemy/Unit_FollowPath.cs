using UnityEngine;
using System.Collections;

public class Unit_FollowPath : MonoBehaviour 
{
	//public Transform target;
	float speed = 0.01f;
	float retargetTolerance = 0.08f;
	float arrivalTolerance = 0.02f;
	Vector2[] path;
	int targetIndex;
	Vector2 oldTargetPos;

	Unit_Physics unitPhysics;
	Unit_FollowPath instance;

	void Awake() 
	{
		//oldTargetPos = target.position;
		//PathRequestManager.RequestPath(transform.position,oldTargetPos, OnPathFound);
		unitPhysics = GetComponent<Unit_Physics>();
		instance = this;
	}

	void Update() 
	{
		/*
		Vector2 newTargetPos = target.position;
		if ( (newTargetPos - oldTargetPos).magnitude > retargetTolerance )
		{
			oldTargetPos = newTargetPos;
			PathRequestManager.RequestPath(transform.position, oldTargetPos, OnPathFound);
		}
		*/
	}

	public void OnPathFound(Vector2[] newPath, bool pathSuccessful) 
	{
		if (pathSuccessful && instance != null) 
		{
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath() 
	{
		if ( path.Length < 1 )
			yield break;
		
		targetIndex = 0;

		Vector2 currentWaypoint = path[0];

		while (true) 
		{
			Vector2 transPos2D = transform.position;
			if ( (currentWaypoint - transPos2D).magnitude < arrivalTolerance )
			{
				targetIndex ++;
				if (targetIndex >= path.Length) 
				{
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			//transform.position = Vector2.MoveTowards(transPos2D,currentWaypoint,speed * Time.deltaTime);

			Vector2 moveVec = currentWaypoint - transPos2D;
			Vector2 moveNorm = moveVec.normalized;
			unitPhysics.MoveVelocity = moveNorm*speed;

			yield return null;

		}
	}

	public void OnDrawGizmos() 
	{
		if (path != null) 
		{
			for (int i = targetIndex; i < path.Length; i ++) 
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector2.one*0.04f);

				if (i == targetIndex) 
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else 
				{
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
