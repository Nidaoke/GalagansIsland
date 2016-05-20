using UnityEngine;
using System.Collections;

public class ShieldKillerBeam : MonoBehaviour 
{




	//Remove shield on a hit ~Adam
	void OnTriggerEnter(Collider other)
	{
		PlayerShipController playerShip = other.GetComponent<PlayerShipController>();
		if(playerShip != null)
		{
			playerShip.mShieldTimer = 0f;
			if(FindObjectOfType<ScoreManager>() != null)
			{
				FindObjectOfType<ScoreManager>().HitAPlayer (other.gameObject);

			}

		}
	}

	void OnTriggerStay(Collider other)
	{
		PlayerShipController playerShip = other.GetComponent<PlayerShipController>();
		if(playerShip != null)
		{
			playerShip.mShieldTimer = 0f;
			if(FindObjectOfType<ScoreManager>() != null)
			{
				FindObjectOfType<ScoreManager>().HitAPlayer (other.gameObject);
				
			}
			
		}
	}
}
