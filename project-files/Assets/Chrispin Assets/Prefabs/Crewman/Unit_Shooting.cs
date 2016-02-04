using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Unit_Shooting : Pawn 
{
	public GameObject bulletPrefab = null;

	float bulletSpeed = 0.32f;
	int damage = 25;
	float range = 200;
	[SerializeField] Transform myTransform;
	RaycastHit2D hit;

	Animator anim;
	Transform sprite;

	[NetSync]
	Vector2 shootDir;

	// Use this for initialization
	void Start () 
	{
		sprite = transform.Find("Sprite");
		anim = sprite.GetComponent<Animator>();
	}

	void Update ()
	{
		if (IsOwner && Input.GetKeyDown(KeyCode.Mouse0))
		{
			Shoot();
		}
	}
		
	void Shoot()
	{	
		Vector2 transPos2D = transform.position;
		Vector2 aimPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		shootDir = (aimPos - transPos2D).normalized;

		Networking.Instantiate(bulletPrefab, transPos2D, Quaternion.identity, NetworkReceivers.All, callback: BulletSpawned);
	}

	// Callback only happens on the participant that calls instantiate, so we use RPC to set up bullet velocity.
	void BulletSpawned( SimpleNetworkedMonoBehavior bullet )
	{
		bullet.GetComponent<Bullet_Velocity>().RPC("setV", shootDir*bulletSpeed);
	}
}
