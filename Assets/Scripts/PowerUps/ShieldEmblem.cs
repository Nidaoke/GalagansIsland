using UnityEngine;
using System.Collections;
using Assets.Scripts.Achievements;

public class ShieldEmblem : MonoBehaviour 
{
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(new Vector3(0f,-5f*Time.deltaTime,0f));

	}
	
	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "SecondShip") 
		{
			if(FindObjectOfType<LevelKillCounter>() != null)
			{
				FindObjectOfType<LevelKillCounter>().mP1ShieldTime = 25f;
				other.GetComponent<PlayerShipController>().mShieldTimer = 25f; 
			}
			other.GetComponentInParent<PlayerShipController> ().mShielded = true;
			other.GetComponentInParent<PlayerShipController> ().mShieldTimer = 25f;
			Destroy(this.gameObject);

		}

		if(other.GetComponent<PlayerShipController>() != null)
		{
			if(AchievementManager.instance != null)
			{
				if(!other.GetComponent<PlayerShipController>().mShielded)
				{
					AchievementManager.instance.IDontCare.ResetValue();
				}
				AchievementManager.instance.UpgradesCollected.IncreseValue();
			}

			other.GetComponent<PlayerShipController>().mShielded = true;
			other.GetComponent<PlayerShipController>().mShieldTimer = 25f; 

			if(FindObjectOfType<LevelKillCounter>() != null)
			{
				if(other.GetComponent<PlayerOneShipController>() != null)
				{
					FindObjectOfType<LevelKillCounter>().mP1ShieldTime = 25f;
					other.GetComponent<PlayerShipController>().mShieldTimer = 25f; 
				}
				else if (other.GetComponent<PlayerTwoShipController>() != null)
				{
					FindObjectOfType<LevelKillCounter>().mP2ShieldTime = 25f;
					other.GetComponent<PlayerShipController>().mShieldTimer = 25f; 
				}
			}


			if(FindObjectOfType<GetReady>() != null)
			{
				if(other.GetComponent<PlayerOneShipController>() != null)
				{
					FindObjectOfType<GetReady>().mP1ShieldTime = 25f;
				}
				else if (other.GetComponent<PlayerTwoShipController>() != null)
				{
					FindObjectOfType<GetReady>().mP2ShieldTime = 25f;
				}
			}


			Destroy(this.gameObject);
		}

	}
}