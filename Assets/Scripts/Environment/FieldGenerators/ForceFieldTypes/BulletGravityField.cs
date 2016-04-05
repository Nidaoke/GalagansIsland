using UnityEngine;
using System.Collections;

//Adjusts the rigidbody velocities of player bullets that enter to alter their trajectories ~Adam

public class BulletGravityField : ForceFieldBase 
{
	[SerializeField] private float mGravityPullStrength = 5f;
	// Use this for initialization
	void Start () 
	{
		base.Start();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	void OnTriggerEnter(Collider other)
	{
		if(mCurrentState == ForceFieldState.ON)
		{
			PlayerBulletController playerBullet = other.GetComponent<PlayerBulletController>();
			if(playerBullet != null)
			{
				Rigidbody bulletRigidbody = other.GetComponent<Rigidbody>();
				Vector3 forcePull = Vector3.Normalize(other.transform.position - this.transform.position)*mGravityPullStrength;

				bulletRigidbody.AddForce(forcePull);
				//Make sure the bullet is facing the same way it's going ~Adam
				bulletRigidbody.gameObject.transform.up = bulletRigidbody.velocity;
				}
		}
	}
	void OnTriggerStay(Collider other)
	{
		if(mCurrentState == ForceFieldState.ON)
		{
			PlayerBulletController playerBullet = other.GetComponent<PlayerBulletController>();
			if(playerBullet != null)
			{
				Rigidbody bulletRigidbody = other.GetComponent<Rigidbody>();
				Vector3 forcePull = Vector3.Normalize(other.transform.position - this.transform.position)*mGravityPullStrength;

				bulletRigidbody.AddForce(forcePull);
				//Make sure the bullet is facing the same way it's going ~Adam
				bulletRigidbody.gameObject.transform.up = bulletRigidbody.velocity;
			}
		}
	}
}
