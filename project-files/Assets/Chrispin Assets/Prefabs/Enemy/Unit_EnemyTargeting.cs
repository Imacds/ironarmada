using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Unit_EnemyTargeting : NetworkBehaviour 
{

	Transform myTransform;
	public Transform targetTransform;
	CircleCollider2D targetCollider;
	LayerMask friendlyUnitLayer;
	Unit_FollowPath followPathScript;
	float radius = 100;
	float arrivalTolerance = 0.32f;
	float secondsPerCheck = 2.0f;

	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
		friendlyUnitLayer = 1<<LayerMask.NameToLayer("Friendly Units");
		followPathScript = GetComponent<Unit_FollowPath>();

		if (isServer)
		{
			StartCoroutine(DoCheck());
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
	}

	void SearchForTarget()
	{
		if (!isServer)
			return;

		if (targetTransform == null)
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(myTransform.position, radius, friendlyUnitLayer);

			if (hitColliders.Length>0)
			{
				int randomInt = Random.Range(0, hitColliders.Length);
				targetTransform = hitColliders[randomInt].transform;
				targetCollider = targetTransform.GetComponent<CircleCollider2D>();
			}
		}

		if (targetTransform != null && targetCollider.enabled == false)
		{
			targetTransform = null;
		}
	}

	void MoveToTarget()
	{
		if (targetTransform != null && isServer)
		{
			SetPathDestination(targetTransform);
		}
	}

	void SetPathDestination(Transform targTrans)
	{
		if (followPathScript != null)
		{
			PathRequestManager.RequestPath(myTransform.position, targTrans.position, followPathScript.OnPathFound);
		}
	}

	IEnumerator DoCheck()
	{
		for(;;)
		{
			SearchForTarget();
			MoveToTarget();

			yield return new WaitForSeconds(secondsPerCheck);
		}
	}
}
