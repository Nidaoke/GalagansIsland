using UnityEngine;
using System.Collections;

public class ShieldKillerBeam : MonoBehaviour 
{




	//Remove shield on a hit ~Adam
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			other.gameObject.GetComponent<PlayerShipController>().mShieldTimer = 0f;
			if(FindObjectOfType<ScoreManager>() != null)
			{
				FindObjectOfType<ScoreManager>().HitAPlayer (other.gameObject);

			}

		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			other.gameObject.GetComponent<PlayerShipController>().mShieldTimer = 0f;
			if(FindObjectOfType<ScoreManager>() != null)
			{
				FindObjectOfType<ScoreManager>().HitAPlayer (other.gameObject);
				
			}
			
		}
	}
}
