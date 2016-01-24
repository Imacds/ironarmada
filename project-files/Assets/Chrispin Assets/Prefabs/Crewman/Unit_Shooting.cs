using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Unit_Shooting : NetworkBehaviour {

	int damage = 25;
	float range = 200;
	[SerializeField] Transform myTransform;
	RaycastHit2D hit;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckIfShooting();
	}

	void CheckIfShooting()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Shoot();
		}
	}

	void Shoot()
	{	
		hit = Physics2D.Raycast(myTransform.TransformPoint(0.10f, 0, 0), myTransform.right, range);
		//Debug.DrawRay(myTransform.TransformPoint(0.10f, 0, 0), myTransform.right);
		if (hit)
		{
			Debug.Log(hit.transform.tag);

			if (hit.transform.tag == "Player")
			{
				string hitID = hit.transform.name;

				CmdSendIDHit( hitID, damage );
			}
			else if (hit.transform.tag == "Enemy")
			{
				string hitID = hit.transform.name;

				CmdSendEnemyIDHit( hitID, damage );
			}	

		}
	}

	[Command]
	void CmdSendIDHit( string identity, int dmg )
	{
		GameObject hitGO = GameObject.Find(identity);
		hitGO.GetComponent<Unit_Health>().takeDamage(dmg);

	}

	[Command]
	void CmdSendEnemyIDHit( string identity, int dmg )
	{
		GameObject hitGO = GameObject.Find(identity);
		if ( hitGO != null )
		{
			hitGO.GetComponent<Enemy_Health>().takeDamage(dmg);
		}
	}
}
