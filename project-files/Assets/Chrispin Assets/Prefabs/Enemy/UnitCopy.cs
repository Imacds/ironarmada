using UnityEngine;
using System.Collections;

public class UnitCopy : MonoBehaviour 
{
	public Transform target;
	float speed = 0.4f;
	float retargetTolerance = 0.08f;
	Vector2[] path;
	int targetIndex;
	Vector2 oldTargetPos;

	void Start() 
	{
		oldTargetPos = target.position;
		PathRequestManager.RequestPath(transform.position,oldTargetPos, OnPathFound);
	}

	void Update() 
	{
		Vector2 newTargetPos = target.position;
		if ( (newTargetPos - oldTargetPos).magnitude > retargetTolerance )
		{
			oldTargetPos = newTargetPos;
			PathRequestManager.RequestPath(transform.position, oldTargetPos, OnPathFound);
		}
	}

	public void OnPathFound(Vector2[] newPath, bool pathSuccessful) 
	{
		if (pathSuccessful) 
		{
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath() 
	{
		targetIndex = 0;
		Vector2 currentWaypoint = path[0];

		while (true) 
		{
			Vector2 transPos2D = transform.position;
			if (transPos2D == currentWaypoint) 
			{
				targetIndex ++;
				if (targetIndex >= path.Length) 
				{
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector2.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
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
