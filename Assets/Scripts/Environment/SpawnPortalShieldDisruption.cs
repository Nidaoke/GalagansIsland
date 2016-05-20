using UnityEngine;
using System.Collections;

//Whenever this object's trigger box touches a player ship, it removes their shield but dow NOT damage them, unlike the "Shield Killer"  beams ~Adam
//It also destoys Player Bullets that are fired into it ~Adam
public class SpawnPortalShieldDisruption : MonoBehaviour 
{

	void OnTriggerEnter(Collider other)
	{
		PlayerShipController playerShip = other.GetComponent<PlayerShipController>();
		if(playerShip!= null)
		{
			playerShip.mShieldTimer = 0f;
		}
		else
		{
			PlayerBulletController playerBullet = other.GetComponent<PlayerBulletController>();
			if(playerBullet != null)
			{
				Destroy(other.gameObject);
			}
		}
	}
}
