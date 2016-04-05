using UnityEngine;
using System.Collections;

public class ReflectedPlayerBullet : MonoBehaviour 
{

	public float bulletSpeed = 30.0f;
	private float selfDestructTimer = 0.0f;
	public bool mSideBullet = false;
	public int mPlayerBulletNumber = 1;

	public void Start()
	{
		bulletSpeed+= (30f/25f*17f);//Speed used to scale with the level number, but we set the speed to be statically at what it used to be at level 17 ~Adam
		//bulletSpeed+= (30f/25f*Application.loadedLevel);
		Vector3 bulletForce;
		bulletForce = new Vector3(0.0f,bulletSpeed, 0f);
		bulletForce = Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * bulletForce;

		GetComponent<Rigidbody>().velocity = bulletForce;
		selfDestructTimer = Time.time + 5.0f;
	}

	void Update()
	{
		if(selfDestructTimer < Time.time || transform.position.y >= 24f)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{

		PlayerShipController playerShip = other.gameObject.GetComponent<PlayerShipController>();
		if(playerShip != null)
		{
			FindObjectOfType<ScoreManager>().HitAPlayer(playerShip.gameObject);
			Destroy(this.gameObject);
		}

	}




}
