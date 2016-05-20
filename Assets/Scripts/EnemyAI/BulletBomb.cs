using UnityEngine;
using System.Collections;

public class BulletBomb : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		EnemyShipAI enemyShip = other.GetComponent<EnemyShipAI>();
		if(enemyShip != null)
		{
			if(enemyShip.mDestroyedByBombs)
			{
				enemyShip.EnemyShipDie();
			}
		}
		PlayerBulletController playerBullet = other.GetComponent<PlayerBulletController>();
		EnemyBulletController enemyBullet = other.GetComponent<EnemyBulletController>();
		if(playerBullet != null || (enemyBullet != null && enemyBullet.mDestroyedByBombs) )
		{
			Destroy (other.gameObject);
		}
		if(other.GetComponent<PlayerShipController>() != null && FindObjectOfType<ScoreManager>() != null)
		{
			FindObjectOfType<ScoreManager>().LoseALife();
		}
	}
}
