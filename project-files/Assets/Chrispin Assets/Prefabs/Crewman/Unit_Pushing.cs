using UnityEngine;
using System.Collections;

public class Unit_Pushing : MonoBehaviour 
{
	float pushSpeed = 0.005f;
	float pushRadius = 0.08f;

	Transform myTransform;
	CircleCollider2D myCollider;
	LayerMask friendlyUnitLayer;
	LayerMask enemyUnitLayer;
	LayerMask allUnitLayers;
	Unit_Physics unitPhysics;

	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
		myCollider = GetComponent<CircleCollider2D>();
		friendlyUnitLayer = (1 << LayerMask.NameToLayer("Friendly Units"));
		enemyUnitLayer = (1 << LayerMask.NameToLayer("Enemy Units"));
		allUnitLayers = friendlyUnitLayer | enemyUnitLayer;
		unitPhysics = GetComponent<Unit_Physics>();
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
		Vector2 myTransPos2D = myTransform.position;

		//get touching units
		Collider2D[] pushColliders = Physics2D.OverlapCircleAll(myTransPos2D, pushRadius, allUnitLayers);
		foreach ( Collider2D tCollider in pushColliders )
		{
			if (tCollider == myCollider)
				continue;
			
			Transform tTransform = tCollider.transform;
			Unit_Physics tPhysics = tTransform.GetComponent<Unit_Physics>();

			if (tPhysics.MoveVelocity == Vector2.zero)
			{
				Vector2 tTransPos2D = tTransform.position;

				Vector2 pushNorm = Vector2.up;	//assign some default value
				if ( myTransPos2D == tTransPos2D )
				{
					//use random push vector
					pushNorm = Random.insideUnitCircle.normalized;
				}
				else
				{
					//push other units away from self
					pushNorm = (tTransPos2D - myTransPos2D).normalized;
				}
				Vector2 pushVelocity = pushNorm*pushSpeed;
				tPhysics.PushVelocity = pushVelocity;
			}
		}
	}
}
