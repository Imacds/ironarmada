using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy_Health : NetworkBehaviour 
{
	int health = 50;

	public void takeDamage(int dmg)
	{
		health -= dmg;
		CheckHealth();
	}

	void CheckHealth()
	{
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}
}
