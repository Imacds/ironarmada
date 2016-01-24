using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy_Attack : NetworkBehaviour 
{
	float attackRate = 3.0f;
	float nextAttack;
	int damage = 10;
	float minDistance = 0.12f;
	float currentDistance;
	Transform myTransform;
	Unit_EnemyTargeting targetScript;

	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
		targetScript = GetComponent<Unit_EnemyTargeting>();

		if (isServer)
		{
			StartCoroutine(Attack());
		}
	}

	IEnumerator Attack()
	{
		for(;;)
		{
			yield return new WaitForSeconds(0.2f);
			CheckIfTargetInRange();
		}
	}

	void CheckIfTargetInRange()
	{
		if (targetScript.targetTransform != null)
		{
			currentDistance = Vector2.Distance(targetScript.targetTransform.position, myTransform.position);

			if (currentDistance < minDistance && Time.time > nextAttack)
			{
				nextAttack = Time.time + attackRate;

				targetScript.targetTransform.GetComponent<Unit_Health>().takeDamage(damage);
			}
		}
	}
}
